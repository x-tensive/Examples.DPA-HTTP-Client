using System;

namespace DPA.API.SampleApp
{
	public class CommandParameter
	{
		public string Name { get; }

		public CommandParameterType Type { get; }

		public bool TryValidate(string value, out string error)
		{
			error = string.Empty;
			switch (Type)
			{
				case CommandParameterType.Number:
					if (double.TryParse(value, out var _))
						return true;
					error = $"Parameter '{Name}'; Value '{value}' is not number";
					return false;
				case CommandParameterType.DateTime:
					if (DateTimeOffset.TryParse(value, out var _))
						return true;
					error = $"Parameter '{Name}'; Value '{value}' is not DateTime";
					return false;
			}
			
			return true;
		}

		public CommandParameter(string name, CommandParameterType type)
		{
			Name = name;
			Type = type;
		}
	}
	
	public enum CommandParameterType
	{
		Number = 0,
		String = 1,
		DateTime = 2
	}
}
