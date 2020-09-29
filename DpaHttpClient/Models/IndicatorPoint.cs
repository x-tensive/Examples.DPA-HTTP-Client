using System;
using System.Collections.Generic;
using System.Text;

namespace DpaHttpClient
{
	public class IndicatorPoint
	{
		public DateTimeOffset Time { get; set; }
		public double Value { get; set; }

		public string Text { get; set; }
		public IndicatorDataType Type { get; set; }

		public override string ToString()
		{
			return $"[{Time}]: {Text}";
		}
	}

	public enum IndicatorDataType
	{
		Numeric,
		Logical,
		Literal
	}
}
