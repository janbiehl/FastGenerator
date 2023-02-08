using System;
using System.Collections.Generic;
using FastGenerator.Extensions;
using Siemens.Engineering.SW;
using Siemens.Engineering.SW.Blocks;
using Siemens.Engineering.SW.Tags;
using PlcSystemConstant = FastGenerator.Models.PlcSystemConstant;
using PlcTag = FastGenerator.Models.PlcTag;
using PlcTagTable = FastGenerator.Models.PlcTagTable;
using PlcUserConstant = FastGenerator.Models.PlcUserConstant;

namespace FastGenerator.Utils
{
	public static class PlcSoftwareUtils
	{
#region TagTables

		public static IList<PlcTagTable> GetAllTags(PlcSoftware plcSoftware)
		{
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			var tagTables = new List<PlcTagTable>();

			EnumerateTagTables(plcSoftware.TagTableGroup.TagTables, ref tagTables);
			EnumerateTagTableGroups(plcSoftware.TagTableGroup.Groups, ref tagTables);

			return tagTables;
		}

		private static void EnumerateTagTables(PlcTagTableComposition tagTableComposition,
			ref List<PlcTagTable> tagTables)
		{
			if (tagTableComposition == null) throw new ArgumentNullException(nameof(tagTableComposition));

			foreach (var tagTable in tagTableComposition)
			{
				var tagTableInfo = new PlcTagTable()
				{
					Guid = Guid.NewGuid(),
					Name = tagTable.Name,
					Tags = new List<PlcTag>(),
					SystemConstants = new List<PlcSystemConstant>(),
					UserConstants = new List<PlcUserConstant>()
				};

				foreach (var plcTag in tagTable.Tags)
				{
					var plcTagInfo = new PlcTag()
					{
						Name = plcTag.Name,
						DataTypeName = plcTag.DataTypeName,
						LogicalAddress = plcTag.LogicalAddress,
						Comment = plcTag.Comment.GetDefaultText(),
						ExternalAccessible = plcTag.ExternalAccessible,
						ExternalVisible = plcTag.ExternalVisible,
						ExternalWritable = plcTag.ExternalWritable
					};

					tagTableInfo.Tags.Add(plcTagInfo);
				}

				foreach (var systemConstant in tagTable.SystemConstants)
				{
					var systemConstantInfo = new PlcSystemConstant()
					{
						Name = systemConstant.Name,
						DataTypeName = systemConstant.DataTypeName,
						Value = systemConstant.Value
					};

					tagTableInfo.SystemConstants.Add(systemConstantInfo);
				}

				foreach (var userConstant in tagTable.UserConstants)
				{
					var userConstantInfo = new PlcUserConstant()
					{
						Name = userConstant.Name,
						DataTypeName = userConstant.DataTypeName,
						Comment = userConstant.Comment.GetDefaultText(),
						Value = userConstant.Value
					};

					tagTableInfo.UserConstants.Add(userConstantInfo);
				}

				tagTables.Add(tagTableInfo);
			}
		}

		private static void EnumerateTagTableGroups(PlcTagTableUserGroupComposition tagTableUserGroupComposition,
			ref List<PlcTagTable> tagTables)
		{
			if (tagTableUserGroupComposition == null)
				throw new ArgumentNullException(nameof(tagTableUserGroupComposition));

			foreach (var tagTableUserGroup in tagTableUserGroupComposition)
			{
				EnumerateTagTables(tagTableUserGroup.TagTables, ref tagTables);
				EnumerateTagTableGroups(tagTableUserGroup.Groups, ref tagTables);
			}
		}

		/*public static void EnumerateTagTables(PlcSoftware plcSoftware)
		{
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));
			
			Console.WriteLine("Tag tables:");
			EnumerateTagTables(plcSoftware.TagTableGroup.TagTables);
			
			EnumerateTagTableGroups(plcSoftware.TagTableGroup.Groups);
		}

		private static void EnumerateTagTables(PlcTagTableComposition tagTableComposition)
		{
			if (tagTableComposition == null) throw new ArgumentNullException(nameof(tagTableComposition));
			
			foreach (var tagTable in tagTableComposition)
			{
				Console.WriteLine($"{Environment.NewLine}- {tagTable.Name}");

				foreach (var plcTag in tagTable.Tags)
				{
					Console.WriteLine($" + {plcTag.Name}, {plcTag.DataTypeName}, {plcTag.LogicalAddress}, {plcTag.Comment.GetDefaultText()}");
				}

				Console.WriteLine("System constants");
				foreach (var systemConstant in tagTable.SystemConstants)
				{
					Console.WriteLine($" + {systemConstant.Name}, {systemConstant.DataTypeName}, {systemConstant.Value}");
				}

				Console.WriteLine("user constants");
				foreach (var userConstant in tagTable.UserConstants)
				{
					Console.WriteLine($" + {userConstant.Name}, {userConstant.DataTypeName}, {userConstant.Comment.GetDefaultText()}, {userConstant.Value}");
				}
			}
		}
		
		private static void EnumerateTagTableGroups(PlcTagTableUserGroupComposition tagTableUserGroupComposition)
		{
			if (tagTableUserGroupComposition == null)
				throw new ArgumentNullException(nameof(tagTableUserGroupComposition));
			
			foreach (var tagTableUserGroup in tagTableUserGroupComposition)
			{
				Console.WriteLine($"{Environment.NewLine}Group: {tagTableUserGroup.Name}");
				
				EnumerateTagTables(tagTableUserGroup.TagTables);
				
				EnumerateTagTableGroups(tagTableUserGroup.Groups);
			}
		}*/

#endregion


#region Blocks

		public static IList<Models.PlcBlock> GetAllBlocks(PlcSoftware plcSoftware)
		{
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));

			var blocks = new List<Models.PlcBlock>();

			EnumerateBlocks(plcSoftware.BlockGroup.Blocks, string.Empty, ref blocks);
			EnumerateBlockGroup(plcSoftware.BlockGroup.Groups, string.Empty, ref blocks);

			return blocks;
		}

		private static void EnumerateBlockGroup(PlcBlockUserGroupComposition userGroupComposition, string path,
			ref List<Models.PlcBlock> blocks)
		{
			if (userGroupComposition == null) throw new ArgumentNullException(nameof(userGroupComposition));

			foreach (var blockUserGroup in userGroupComposition)
			{
				string tmpPath;

				if (string.IsNullOrEmpty(path))
				{
					tmpPath = blockUserGroup.Name;
				}
				else
				{
					tmpPath = path;
					tmpPath += $"/{blockUserGroup.Name}";
				}

				EnumerateBlocks(blockUserGroup.Blocks, tmpPath, ref blocks);
				EnumerateBlockGroup(blockUserGroup.Groups, tmpPath, ref blocks);
			}
		}

		private static void EnumerateBlocks(PlcBlockComposition blockComposition, string path,
			ref List<Models.PlcBlock> blocks)
		{
			if (blockComposition == null) throw new ArgumentNullException(nameof(blockComposition));

			foreach (var block in blockComposition)
			{
				var plcBlock = new Models.PlcBlock()
				{
					Guid = Guid.NewGuid(),
					Name = block.Name,
					Number = block.Number,
					AutoNumber = block.AutoNumber,
					Language = block.ProgrammingLanguage,
					BlockType = block.GetBlockType(),
					Path = path
				};

				blocks.Add(plcBlock);
			}
		}

		/*
		public static void EnumerateBlocks(PlcSoftware plcSoftware)
		{
			if (plcSoftware == null) throw new ArgumentNullException(nameof(plcSoftware));
			
			Console.WriteLine("Blocks:");
			EnumerateBlocks(plcSoftware.BlockGroup.Blocks);
			EnumerateBlockGroup(plcSoftware.BlockGroup.Groups);
		}

		private static void EnumerateBlockGroup(PlcBlockUserGroupComposition userGroupComposition)
		{
			if (userGroupComposition == null) throw new ArgumentNullException(nameof(userGroupComposition));

			foreach (var blockUserGroup in userGroupComposition)
			{				
				Console.WriteLine($"{Environment.NewLine}Group: {blockUserGroup.Name}");
				
				EnumerateBlocks(blockUserGroup.Blocks);
				EnumerateBlockGroup(blockUserGroup.Groups);
			}
		}

		private static void EnumerateBlocks(PlcBlockComposition blockComposition)
		{
			if (blockComposition == null) throw new ArgumentNullException(nameof(blockComposition));

			foreach (var block in blockComposition)
			{
				var blockIdentifier = GetBlockTypeShortName(block.GetBlockType());
				Console.WriteLine($"{Environment.NewLine}- {block.Name}[{blockIdentifier}{block.Number}]");
			}
		}
		*/

		public static string GetBlockTypeShortName(BlockType blockType) =>
			blockType switch
			{
				BlockType.Ob => "OB",
				BlockType.Fb => "FB",
				BlockType.Fc => "FC",
				BlockType.Db => "DB",
				BlockType.Idb => "iDB",
				BlockType.Adb => "aDB",
				_ => throw new ArgumentOutOfRangeException(nameof(blockType), blockType, null)
			};

		/// <summary>
		/// Get the blocks information in short format
		/// </summary>
		/// <remarks>
		///	Example: BlockName FB[25]
		/// </remarks>
		/// <param name="plcBlock">The block to get the information for</param>
		/// <returns>The blocks short information</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static string GetBlockShortInformation(PlcBlock plcBlock)
		{
			if (plcBlock == null) throw new ArgumentNullException(nameof(plcBlock));

			var blockType = plcBlock.GetBlockType();

			var blockTypeShortName = GetBlockTypeShortName(blockType);

			return $"{plcBlock.Name} {blockTypeShortName}[{plcBlock.Number}]";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plcSoftware"></param>
		/// <param name="name"></param>
		public static PlcBlock? FindBlock(PlcSoftware plcSoftware, string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			var plcBlock = plcSoftware.BlockGroup.Blocks.Find(name) 
			               ?? FindBlockRecursive(plcSoftware.BlockGroup.Groups, name);

			return plcBlock;
		}

		private static PlcBlock? FindBlockRecursive(PlcBlockUserGroupComposition blockGroupGroups, string name)
		{
			PlcBlock? plcBlock = null;
			
			foreach (var plcBlockUserGroup in blockGroupGroups)
			{
				plcBlock = plcBlockUserGroup.Blocks.Find(name) 
				           ?? FindBlockRecursive(plcBlockUserGroup.Groups, name);

				if (plcBlock != null)
					break;
			}

			return plcBlock;
		}

#endregion
	}
}