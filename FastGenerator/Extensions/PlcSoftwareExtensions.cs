using System;
using System.Collections.Generic;
using FastGenerator.Models;
using FastGenerator.Utils;
using Siemens.Engineering.SW;

namespace FastGenerator.Extensions
{
	public static class PlcSoftwareExtensions
	{
		// public static void EnumerateTagTables(this PlcSoftware plcSoftware)
		// {
		// 	PlcSoftwareUtils.EnumerateTagTables(plcSoftware);
		// }
		//
		// public static void EnumerateBlocks(this PlcSoftware plcSoftware)
		// {
		// 	PlcSoftwareUtils.EnumerateBlocks(plcSoftware);
		// }

		public static IList<PlcBlock> GetBlockInformation(this PlcSoftware plcSoftware)
		{
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			return PlcSoftwareUtils.GetAllBlocks(plcSoftware);
		}

		public static IList<PlcTagTable> GetTagTableInformation(this PlcSoftware plcSoftware)
		{
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			return PlcSoftwareUtils.GetAllTags(plcSoftware);
		}
	}
}