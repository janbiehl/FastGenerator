using System;
using FastGenerator.Utils;

namespace FastGenerator.Models
{
	[Serializable]
	public class PlcUserConstant
	{
		public string Name { get; set; } = Constants.NotAssigned;
		public string DataTypeName { get; set; } = Constants.NotAssigned;
		public string Value { get; set; } = Constants.NotAssigned;
		public string Comment { get; set; } = string.Empty;
	
		public override string ToString() => Name;
	}
}