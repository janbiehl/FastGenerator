using System;
using System.IO;
using System.Threading.Tasks;
using FastGenerator.Extensions;
using FastGenerator.Utils;
using MiniExcelLibs;
using Siemens.Engineering;

namespace FastGenerator
{
	internal abstract class Program
	{
		public static async Task Main(string[]? args)
		{
			if (args == null || args.Length == 0 || args.Length > 1)
			{
				Console.WriteLine("Usage: FastGenerator.exe requires exactly one argument");
				return;
			}

			var fileName = args[0];
			
			if (!File.Exists(fileName))
			{
				Console.WriteLine($"File {fileName} does not exist");
				return;
			}

			var generatorData = await MiniExcel.QueryAsync<GeneratorData>(fileName);
			
			if (generatorData is null)
			{
				Console.WriteLine($"File {fileName} is empty");
				return;
			}

			var tiaPortal = new TiaPortal();
			Project? project = null;

			try
			{
				string? lastProject = null;
				foreach (var data in generatorData)
				{
					if (lastProject == null || data.TiaProjectPath != lastProject)
					{
						project = tiaPortal.OpenProject(data.TiaProjectPath);
						lastProject = data.TiaProjectPath;
					}

					if (project is null)
						throw new Exception("Project was not opened properly");

					var plcDevices = project.FindAnyPlcDevices();
					
					if (plcDevices is null)
						throw new Exception("No PLC devices found");

					var plcDevice = plcDevices.Find(device => device.plcSoftware.Name == data.Plc);
					
					if (plcDevice.plcSoftware is null)
						throw new Exception($"No PLC device found with name {data.Plc}");

					var plcBlock = PlcSoftwareUtils.FindBlock(plcDevice.plcSoftware, data.BlockName); 

					if (plcBlock is null)
						throw new Exception($"No PLC block found with name {data.BlockName}");
					
					plcBlock.ExportToFile(data.BlockExportPath);
					
					if (!File.Exists(data.BlockExportPath))
						throw new Exception($"Exported PLC block {data.BlockName} to {data.BlockExportPath} does not exist");
					
					Console.WriteLine("Export successful");
				}
			}
			finally
			{
				project?.Close();
				tiaPortal.Dispose();
			}
		}
	}
}