namespace Logitech.Api.Test;

public sealed class OpenApiExampleTests
{
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	private static readonly string _placesResponseJson = """
	{
	  "places": [
		{
		  "id": "room-1",
		  "type": "Room",
		  "name": "Large Room 5",
		  "group": "/US/California/San Francisco/Headquarters/",
		  "location": "/Main Campus/Building A/Floor 13/Green Area/",
		  "seatCount": 12,
		  "occupancy": 0,
		  "contract": {
			"service": "SelectRoom",
			"name": "Select Room Contract (#987654)",
			"number": 987654,
			"expiresAt": 1748832000000
		  },
		  "devices": [
			{
			  "id": "computer-1",
			  "type": "Computer",
			  "name": "DESKTOP-PW8ZH",
			  "version": "3.3.176",
			  "status": "Online",
			  "healthStatus": "NoIssues",
			  "network": {
				"ip": "192.168.1.50",
				"mac": "AA:BB:CC:DD:EE:FF"
			  },
			  "lastSeen": 1717200000000,
			  "createdAt": 1717200000000
			},
			{
			  "id": "logitech-1",
			  "type": "Logitech",
			  "name": "Rally",
			  "version": "1.2.69",
			  "status": "Online",
			  "healthStatus": "Warning",
			  "peripherals": {
				"camera": { "count": { "actual": 1, "expected": 1 } },
				"micPod": { "count": { "actual": 1, "expected": 2 } },
				"micPodHub": { "count": { "actual": 1, "expected": 1 } },
				"speaker": { "count": { "actual": 2, "expected": 2 } },
				"tableHub": { "count": { "actual": 1, "expected": 1 } },
				"displayHub": { "count": { "actual": 1, "expected": 1 } }
			  },
			  "lastSeen": 1717200000000,
			  "createdAt": 1717200000000
			},
			{
			  "id": "generic-1",
			  "type": "Generic",
			  "name": "USB Keyboard",
			  "status": "Offline",
			  "healthStatus": "Error",
			  "lastSeen": 1717200000000,
			  "createdAt": 1717200000000
			}
		  ],
		  "createdAt": 1717200000000
		},
		{
		  "id": "desk-1",
		  "type": "Desk",
		  "name": "Desk 13",
		  "location": "/Main Campus/Building A/Floor 13/Green Area/",
		  "contract": {
			"service": "SelectDesk",
			"name": "Select Desk Contract (#987654)",
			"number": 987654,
			"expiresAt": 1748832000000
		  },
		  "devices": [
			{
			  "id": "dock-1",
			  "type": "Logitech",
			  "name": "Logi Dock Flex",
			  "version": "1.12.252",
			  "status": "Online",
			  "healthStatus": "NoIssues",
			  "warranty": {
				"type": "SelectDesk",
				"expiresAt": 1759852800000
			  },
			  "network": {
				"ip": "10.11.16.237",
				"mac": "44:73:d6:b4:97:90",
				"hostName": "DockFlex-30VV78",
				"wired": {
				  "mode": "dhcp",
				  "address": "10.11.16.237",
				  "gateway": "10.11.16.1",
				  "subnetMask": "255.255.254.0",
				  "dns": ["10.11.16.7", "1.1.1.1"]
				},
				"wireless": null
			  },
			  "lastSeen": 1717200121230,
			  "createdAt": 1717200000000
			},
			{
			  "id": "generic-2",
			  "type": "Generic",
			  "name": "USB Keyboard",
			  "status": "Offline",
			  "healthStatus": "Error",
			  "lastSeen": 1717200000000,
			  "createdAt": 1717200000000
			}
		  ],
		  "createdAt": 1717200000000
		}
	  ],
	  "continuation": "next-page-token"
	}
	""";

	[Fact]
	public void GetPlacesExample_DeserializesExpectedRoomAndDeskShape()
	{
		var response = JsonSerializer.Deserialize<PlaceResponse>(_placesResponseJson, _jsonOptions) 
			?? throw new Xunit.Sdk.XunitException("Response should deserialize.");

		response.Continuation.Should().Be("next-page-token");
		response.Places.Should().HaveCount(2);

		AssertRoomPlaceData(response.Places[0]);
		AssertDeskPlaceData(response.Places[1]);
	}

	private static void AssertRoomPlaceData(JsonElement room)
	{
		room.GetProperty("type").GetString().Should().Be("Room");
		room.GetProperty("name").GetString().Should().Be("Large Room 5");
		room.GetProperty("seatCount").GetInt32().Should().Be(12);
		room.GetProperty("devices").GetArrayLength().Should().Be(3);
	}

	private static void AssertDeskPlaceData(JsonElement desk)
	{
		desk.GetProperty("type").GetString().Should().Be("Desk");
		desk.GetProperty("name").GetString().Should().Be("Desk 13");
		desk.GetProperty("devices").GetArrayLength().Should().Be(2);
	}

	private static readonly string _deviceJson = """
	{
	  "id": "logitech-1",
	  "type": "Logitech",
	  "name": "Rally",
	  "version": "1.2.69",
	  "serial": "ABC123",
	  "status": "Online",
	  "healthStatus": "Warning",
	  "peripherals": {
		"camera": { "count": { "actual": 1, "expected": 1 } },
		"speaker": { "count": { "actual": 2, "expected": 2 } }
	  },
	  "network": {
		"ip": "10.11.16.237",
		"mac": "44:73:d6:b4:97:90",
		"hostName": "DockFlex-30VV78",
		"wired": {
		  "mode": "dhcp",
		  "address": "10.11.16.237",
		  "gateway": "10.11.16.1",
		  "subnetMask": "255.255.254.0",
		  "dns": ["10.11.16.7", "1.1.1.1"]
		},
		"wireless": null
	  },
	  "sensors": {
		"latestTs": 1717200120000,
		"co2": 650,
		"pressure": 101325,
		"temperature": 22.5,
		"humidity": 45.2,
		"tvoc": 180,
		"pm10": 5,
		"pm25": 3,
		"presence": 1
	  },
	  "warranty": {
		"type": "SelectDesk",
		"expiresAt": 1759852800000
	  },
	  "lastSeen": 1717200121230,
	  "createdAt": 1717200000000
	}
	""";

	[Fact]
	public void DeviceExample_DeserializesNestedModelShapes()
	{
		var device = JsonSerializer.Deserialize<LogitechDevice>(_deviceJson, _jsonOptions) 
			?? throw new Xunit.Sdk.XunitException("Device should deserialize.");

		AssertDeviceBasicProperties(device);
		AssertDevicePeripherals(device);
		AssertDeviceNetwork(device);
		AssertDeviceSensors(device);
		AssertDeviceWarranty(device);
	}

	private static void AssertDeviceBasicProperties(LogitechDevice device)
	{
		device.Id.Should().Be("logitech-1");
		device.Type.Should().Be("Logitech");
		device.Name.Should().Be("Rally");
		device.Version.Should().Be("1.2.69");
		device.Serial.Should().Be("ABC123");
		device.Status.Should().Be("Online");
		device.HealthStatus.Should().Be("Warning");
		device.LastSeen.Should().Be(1717200121230);
	}

	private static void AssertDevicePeripherals(LogitechDevice device)
	{
		device.Peripherals.Should().NotBeNull();
		device.Peripherals!.Camera!.Count.Actual.Should().Be(1);
		device.Peripherals.Speaker!.Count.Actual.Should().Be(2);
	}

	private static void AssertDeviceNetwork(LogitechDevice device)
	{
		device.Network.Should().NotBeNull();
		device.Network!.HostName.Should().Be("DockFlex-30VV78");
	}

	private static void AssertDeviceSensors(LogitechDevice device)
	{
		device.Sensors.Should().NotBeNull();
		device.Sensors!.Co2.Should().Be(650);
	}

	private static void AssertDeviceWarranty(LogitechDevice device)
	{
		device.Warranty.Should().NotBeNull();
		device.Warranty!.Type.Should().Be("SelectDesk");
	}
}
