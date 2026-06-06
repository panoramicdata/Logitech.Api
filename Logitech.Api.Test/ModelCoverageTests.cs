using System.Collections;
using System.Reflection;

namespace Logitech.Api.Test;

public sealed class ModelCoverageTests
{
	[Fact]
	public void PlaceRequest_HoldsExpectedValues()
	{
		PlaceRequest request = new()
		{
			Continuation = "cont-token",
			Limit = 25,
			Rooms = true,
			Desks = false,
			Unlicensed = true,
			Projection = "place.info"
		};

		request.Continuation.Should().Be("cont-token");
		request.Limit.Should().Be(25);
		request.Rooms.Should().BeTrue();
		request.Desks.Should().BeFalse();
		request.Unlicensed.Should().BeTrue();
		request.Projection.Should().Be("place.info");
	}

	[Fact]
	public void AllPublicModels_HaveWorkingGettersAndSetters()
	{
		Type[] modelTypes =
		[
			typeof(LogitechSyncClientOptions),
			typeof(ComputerDevice),
			typeof(ComputerDeviceNetwork),
			typeof(Desk),
			typeof(DeviceNetwork),
			typeof(DeviceNetworkConfig),
			typeof(DeviceSensors),
			typeof(DeviceWarranty),
			typeof(GenericDevice),
			typeof(LogitechDevice),
			typeof(Peripherals),
			typeof(Peripheral),
			typeof(PeripheralCount),
			typeof(PlaceContract),
			typeof(PlaceRequest),
			typeof(PlaceResponse),
			typeof(Room)
		];

		foreach (var modelType in modelTypes)
		{
			var instance = Activator.CreateInstance(modelType)!;
			ExerciseProperties(instance);
		}
	}

	private static void ExerciseProperties(object instance)
	{
		var properties = instance
			.GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		var seed = 1;
		foreach (var property in properties)
		{
			if (property.CanWrite)
			{
				var value = CreateValue(property.PropertyType, seed);
				if (value is not null)
				{
					property.SetValue(instance, value);
				}
			}

			_ = property.GetValue(instance);
			seed++;
		}
	}

	private static object? CreateValue(Type type, int seed)
	{
		var nullableUnderlying = Nullable.GetUnderlyingType(type);
		if (nullableUnderlying is not null)
		{
			return CreateValue(nullableUnderlying, seed);
		}

		if (type == typeof(string))
		{
			return $"value-{seed}";
		}

		if (type == typeof(int))
		{
			return seed;
		}

		if (type == typeof(long))
		{
			return (long)seed;
		}

		if (type == typeof(double))
		{
			return seed + 0.5;
		}

		if (type == typeof(bool))
		{
			return true;
		}

		if (type == typeof(JsonElement))
		{
			using var document = JsonDocument.Parse("{\"sample\":1}");
			return document.RootElement.Clone();
		}

		if (type == typeof(Microsoft.Extensions.Logging.ILogger))
		{
			return Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
		}

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			var elementType = type.GetGenericArguments()[0];
			var list = (IList)Activator.CreateInstance(type)!;
			var element = CreateValue(elementType, seed + 100);
			if (element is not null)
			{
				list.Add(element);
			}

			return list;
		}

		if (type.IsClass)
		{
			var ctor = type.GetConstructor(Type.EmptyTypes);
			return ctor is not null ? Activator.CreateInstance(type) : null;
		}

		return null;
	}
}
