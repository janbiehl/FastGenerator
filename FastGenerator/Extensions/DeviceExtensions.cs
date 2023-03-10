using System;
using FastGenerator.Models;
using FastGenerator.Utils;
using Siemens.Engineering.CustomIdentity;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;

namespace FastGenerator.Extensions
{
	public static class DeviceExtensions
	{
		public static string? Identifier(this Device device)
		{
			if (device == null) throw new ArgumentNullException(nameof(device));
            
			var identityProvider = device.GetService<CustomIdentityProvider>();

			return identityProvider?.Get(DeviceUtils.DeviceIdentifierKey);
		}

		public static PlcDevice? GetPlcDeviceInformation(this Device device)
		{
			if (device == null) throw new ArgumentNullException(nameof(device));
			
			// Iterate over the device items and search for a plc software
			DeviceItem? deviceItem = null;
			PlcSoftware? plcSoftware = null;
			
			foreach (var tmpDeviceItem in device.DeviceItems)
			{
				var softwareContainer = tmpDeviceItem.GetService<SoftwareContainer>();

				if (softwareContainer?.Software is PlcSoftware tmpPlcSoftware)
				{
					deviceItem = tmpDeviceItem;
					plcSoftware = tmpPlcSoftware;
					break;
				}
			}
			
			// Exit when it is not a plc device
			if (deviceItem is null || plcSoftware is null)
				return null;

			var plcDevice = new PlcDevice
			{
				Guid = Guid.NewGuid(),
				DeviceName = device.Name,
				DeviceItemName = deviceItem.Name,
				PlcSoftwareName = plcSoftware.Name,
				DeviceIdentifier = device.TypeIdentifier,
				TypeIdentifier = deviceItem.TypeIdentifier,
				Blocks = plcSoftware.GetBlockInformation(),
				TagTables = plcSoftware.GetTagTableInformation()
			};

			return plcDevice;
		}
	}
}