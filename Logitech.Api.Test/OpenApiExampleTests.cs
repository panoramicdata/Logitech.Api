using Logitech.Api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Logitech.Api.Test;

public sealed class OpenApiExampleTests
{
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	[Fact]
	public void GetPlacesExample_DeserializesExpectedRoomAndDeskShape()
	{
		string json = """
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

		PlaceResponse response = JsonSerializer.Deserialize<PlaceResponse>(json, JsonOptions) ?? throw new Xunit.Sdk.XunitException("Response should deserialize.");

		Assert.Equal("next-page-token", response.Continuation);
		Assert.Equal(2, response.Places.Count);

		JsonElement room = response.Places[0];
		Assert.Equal("Room", room.GetProperty("type").GetString());
		Assert.Equal("Large Room 5", room.GetProperty("name").GetString());
		Assert.Equal(12, room.GetProperty("seatCount").GetInt32());
		Assert.Equal(3, room.GetProperty("devices").GetArrayLength());

		JsonElement desk = response.Places[1];
		Assert.Equal("Desk", desk.GetProperty("type").GetString());
		Assert.Equal("Desk 13", desk.GetProperty("name").GetString());
		Assert.Equal(2, desk.GetProperty("devices").GetArrayLength());
	}

	[Fact]
	public void DeviceExample_DeserializesNestedModelShapes()
	{
		string json = """
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

		LogitechDevice device = JsonSerializer.Deserialize<LogitechDevice>(json, JsonOptions) ?? throw new Xunit.Sdk.XunitException("Device should deserialize.");

		Assert.Equal("logitech-1", device.Id);
		Assert.Equal("Logitech", device.Type);
		Assert.Equal("Rally", device.Name);
		Assert.Equal("1.2.69", device.Version);
		Assert.Equal("ABC123", device.Serial);
		Assert.Equal("Online", device.Status);
		Assert.Equal("Warning", device.HealthStatus);
		Assert.Equal(1717200121230, device.LastSeen);
		Assert.NotNull(device.Peripherals);
		Assert.Equal(1, device.Peripherals!.Camera!.Count.Actual);
		Assert.Equal(2, device.Peripherals.Speaker!.Count.Actual);
		Assert.NotNull(device.Network);
		Assert.Equal("DockFlex-30VV78", device.Network!.HostName);
		Assert.NotNull(device.Sensors);
		Assert.Equal(650, device.Sensors!.Co2);
		Assert.NotNull(device.Warranty);
		Assert.Equal("SelectDesk", device.Warranty!.Type);
	}
}
