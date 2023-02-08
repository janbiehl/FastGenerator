using System;
using System.Collections.Generic;
using FastGenerator.Utils;

namespace FastGenerator.Models
{
	[Serializable]
	public class PlcTagTable
	{
		public string Name { get; set; } = Constants.NotAssigned;
		public Guid Guid { get; set; }
		public List<PlcTag>? Tags { get; set; }
		public List<PlcSystemConstant>? SystemConstants { get; set; }
		public List<PlcUserConstant>? UserConstants { get; set; }

		public override string ToString() => Name;
	}
}