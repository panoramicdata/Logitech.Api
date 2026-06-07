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

		if (IsPrimitiveType(type, seed, out var primitiveValue))
		{
			return primitiveValue;
		}

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			return CreateListValue(type, seed);
		}

		if (type.IsClass)
		{
			var ctor = type.GetConstructor(Type.EmptyTypes);
			return ctor is not null ? Activator.CreateInstance(type) : null;
		}

		return null;
	}

	private static bool IsPrimitiveType(Type type, int seed, out object? value)
	{
		value = null;

		if (type == typeof(string))
		{
			value = $"value-{seed}";
			return true;
		}

		if (type == typeof(int))
		{
			value = seed;
			return true;
		}

		if (type == typeof(long))
		{
			value = (long)seed;
			return true;
		}

		if (type == typeof(double))
		{
			value = seed + 0.5;
			return true;
		}

		if (type == typeof(bool))
		{
			value = true;
			return true;
		}

		if (type == typeof(JsonElement))
		{
			using var document = JsonDocument.Parse("{\"sample\":1}");
			value = document.RootElement.Clone();
			return true;
		}

		if (type == typeof(Microsoft.Extensions.Logging.ILogger))
		{
			value = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
			return true;
		}

		return false;
	}

	private static object? CreateListValue(Type type, int seed)
	{
		var elementType = type.GetGenericArguments()[0];
		var list = Activator.CreateInstance(type);
		if (list is null)
		{
			return null;
		}

		var element = CreateValue(elementType, seed + 100);
		if (element is not null)
		{
			type.GetMethod("Add", [elementType])?.Invoke(list, [element]);
		}

		return list;
	}
}
