using System;

namespace DPA.API.SampleApp
{
	public abstract class CommandParameter
	{
		public string Name { get; }

		public abstract string GetValueType();

		public abstract CommandParameterValue CreateParameterValue(string value);

		protected CommandParameter(string name)
		{
			Name = name;
		}
	}

	public class CommandParameter<TParam>:CommandParameter
	{
		public override string GetValueType() => typeof(TParam).Name;

		public override CommandParameterValue CreateParameterValue(string value)
		{
			return CommandParameterValue<TParam>.Create<TParam>(Name, value);
		}

		public CommandParameter(string name)
			:base(name)
		{
		}
	}

	public abstract class CommandParameterValue
	{
		public string Name { get; set; }
	}

	public class CommandParameterValue<T> : CommandParameterValue
	{
		public T Value { get; }

		public static CommandParameterValue<TParam> Create<TParam>(string parameterName, string value)
		{
			object convertedValue = null;
			if (typeof(TParam) == typeof(string))
				convertedValue = value;

			if (convertedValue == null && value is TParam)
				convertedValue = value;

			if (convertedValue == null)
			{
				if (typeof(TParam) == typeof(long))
				{
					if (long.TryParse(value, out var number))
						convertedValue = number;
				}
				else if (typeof(TParam) == typeof(int))
				{
					if (int.TryParse(value, out var number))
						convertedValue = number;
				}
				else if (typeof(TParam) == typeof(decimal))
				{
					if (decimal.TryParse(value, out var number))
						convertedValue = number;
				}
				else if (typeof(TParam) == typeof(double))
				{
					if (double.TryParse(value, out var number))
						convertedValue = number;
				}
				else if (typeof(TParam) == typeof(DateTimeOffset))
				{
					if (DateTimeOffset.TryParse(value, out var date))
						convertedValue = date;
				}
				else if (typeof(TParam) == typeof(DateTime))
				{
					if (DateTime.TryParse(value, out var date))
						convertedValue = date;
				}
				else if (typeof(TParam) == typeof(bool))
				{
					if (bool.TryParse(value, out var flag))
						convertedValue = flag;
				}
			}

			if (convertedValue == null)
			{
				try { convertedValue = Convert.ChangeType(value, typeof(TParam)); }
				catch { throw new Exception($"Can't convert value '{value}' to {typeof(TParam).Name}"); }
			}

			return new CommandParameterValue<TParam>(parameterName, (TParam)convertedValue);
		}

		private CommandParameterValue(string name, T value)
		{
			Name = name;
			Value = value;
		}
	}
}
