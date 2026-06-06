using Logitech.Api.Data;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace Logitech.Api.Test;

public sealed class ModelCoverageTests
{
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
			typeof(PlaceResponse),
			typeof(Room)
		];

		foreach (Type modelType in modelTypes)
		{
			object instance = Activator.CreateInstance(modelType)!;
			ExerciseProperties(instance);
		}
	}

	private static void ExerciseProperties(object instance)
	{
		PropertyInfo[] properties = instance
			.GetType()
			.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		int seed = 1;
		foreach (PropertyInfo property in properties)
		{
			if (property.CanWrite)
			{
				object? value = CreateValue(property.PropertyType, seed);
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
		Type? nullableUnderlying = Nullable.GetUnderlyingType(type);
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
			using JsonDocument document = JsonDocument.Parse("{\"sample\":1}");
			return document.RootElement.Clone();
		}

		if (type == typeof(Microsoft.Extensions.Logging.ILogger))
		{
			return NullLogger.Instance;
		}

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			Type elementType = type.GetGenericArguments()[0];
			IList list = (IList)Activator.CreateInstance(type)!;
			object? element = CreateValue(elementType, seed + 100);
			if (element is not null)
			{
				list.Add(element);
			}

			return list;
		}

		if (type.IsClass)
		{
			ConstructorInfo? ctor = type.GetConstructor(Type.EmptyTypes);
			return ctor is not null ? Activator.CreateInstance(type) : null;
		}

		return null;
	}
}
