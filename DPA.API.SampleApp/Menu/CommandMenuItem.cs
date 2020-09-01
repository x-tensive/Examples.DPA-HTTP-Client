using DpaHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPA.API.SampleApp
{
	public abstract class CommandMenuItem : MenuItem
	{
		protected readonly CommandParameter[] parameters;
		public CommandParameter[] GetParameters() => parameters;

		private CommandParameterValue[] BuildParameterValues(string[] values, out string errorMessages)
		{
			var result = new List<CommandParameterValue>();
			if (values.Length != parameters.Length)
			{
				errorMessages = "Not all parameters are specified";
				return result.ToArray();
			}

			var errors = new StringBuilder();
			var hasErrors = false;
			for (var ix = 0; ix < parameters.Length; ix++)
			{
				CommandParameterValue value = null;
				try { 
					value = parameters[ix].CreateParameterValue(values[ix]);
					result.Add(value);
				}
				catch (Exception ex)
				{
					errors.AppendLine(ex.Message);
					hasErrors = true;
				}
			}

			errorMessages = hasErrors ? errors.ToString() : "";

			return result.ToArray();
		}

		protected abstract ActionResult InnerExecute(CommandParameterValue[] parameters);

		public ActionResult Execute(string[] values)
		{
			var prmValues = BuildParameterValues(values, out var errorMessages);
			if (!string.IsNullOrWhiteSpace(errorMessages))
				return FailResult.Get(errorMessages);

			return InnerExecute(prmValues);
		}

		public override string ToString()
		{
			var parameterTypes = "(" + string.Join(", ", parameters.Select(prm => prm.Name)) + ")";
			return $"{Name}{parameterTypes}";
		}

		public CommandMenuItem(string name, MenuItem owner, params CommandParameter[] parameters)
			:base(name, owner, null)
		{
			this.parameters = parameters;
		}
	}

	public class CommandMenuItem<TResult> : CommandMenuItem
	{
		private Func<TResult> command;

		protected override ActionResult InnerExecute(CommandParameterValue[] parameters)
		{
			try { return new CommandResult(command()); }
			catch (Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			return new CommandMenuItem<TResult>(this.Name, this.Owner, this.command);
		}

		public CommandMenuItem(string name, Func<TResult> command)
			:this(name, null, command)
		{
		}

		private CommandMenuItem(string name, MenuItem owner, Func<TResult> command)
			:base(name, owner, Array.Empty<CommandParameter>())
		{
			this.command = command;
		}
	}

	public class CommandMenuItem<TIn1, TResult> : CommandMenuItem
	{
		private readonly Func<TIn1, TResult> command;

		protected override ActionResult InnerExecute(CommandParameterValue[] paramValues)
		{
			try
			{
				var inPrm1 = (CommandParameterValue<TIn1>)paramValues[0];
				return new CommandResult(command(inPrm1.Value));
			}
			catch (Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			var inPrm1 = (CommandParameter<TIn1>)parameters[0];
			return new CommandMenuItem<TIn1, TResult>(this.Name, this.Owner, this.command, inPrm1);
		}

		public CommandMenuItem(string name, Func<TIn1, TResult> command)
			: base(name, null, Array.Empty<CommandParameter>())
		{
			this.command = command;
		}

		public CommandMenuItem(string name, Func<TIn1, TResult> command, CommandParameter<TIn1> prm1)
			: base(name, null, prm1)
		{
			this.command = command;
		}

		public CommandMenuItem(string name, MenuItem owner, Func<TIn1, TResult> command, CommandParameter<TIn1> prm1)
			: base(name, owner, prm1)
		{
			this.command = command;
		}
	}

	public class CommandMenuItem<TIn1, TIn2, TResult> : CommandMenuItem
	{
		private readonly Func<TIn1, TIn2, TResult> command;

		protected override ActionResult InnerExecute(CommandParameterValue[] paramValues)
		{
			try
			{
				var inPrm1 = (CommandParameterValue<TIn1>)paramValues[0];
				var inPrm2 = (CommandParameterValue<TIn2>)paramValues[1];
				return new CommandResult(command(inPrm1.Value, inPrm2.Value));
			}
			catch (Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			var inPrm1 = (CommandParameter<TIn1>)parameters[0];
			var inPrm2 = (CommandParameter<TIn2>)parameters[1];
			return new CommandMenuItem<TIn1, TIn2, TResult>(this.Name, this.Owner, this.command, inPrm1, inPrm2);
		}

		public override string ToString()
		{
			var parameterTypes = "(" + string.Join(", ", base.parameters.Select(prm => prm.Name)) + ")";
			return $"{Name}{parameterTypes}";
		}

		public CommandMenuItem(string name, Func<TIn1, TIn2, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2)
			: base(name, null, prm1)
		{
			this.command = command;
		}

		public CommandMenuItem(string name, MenuItem owner, Func<TIn1, TIn2, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2)
			: base(name, owner, prm1)
		{
			this.command = command;
		}
	}

	public class CommandMenuItem<TIn1, TIn2, TIn3, TResult> : CommandMenuItem
	{
		private readonly Func<TIn1, TIn2, TIn3, TResult> command;

		protected override ActionResult InnerExecute(CommandParameterValue[] paramValues)
		{
			try
			{
				var inPrm1 = (CommandParameterValue<TIn1>)paramValues[0];
				var inPrm2 = (CommandParameterValue<TIn2>)paramValues[1];
				var inPrm3 = (CommandParameterValue<TIn3>)paramValues[2];
				return new CommandResult(command(inPrm1.Value, inPrm2.Value, inPrm3.Value));
			}
			catch (Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			var inPrm1 = (CommandParameter<TIn1>)parameters[0];
			var inPrm2 = (CommandParameter<TIn2>)parameters[1];
			var inPrm3 = (CommandParameter<TIn3>)parameters[2];
			return new CommandMenuItem<TIn1, TIn2, TIn3, TResult>(this.Name, this.Owner, this.command, inPrm1, inPrm2, inPrm3);
		}

		public override string ToString()
		{
			var parameterTypes = "(" + string.Join(", ", base.parameters.Select(prm => prm.Name)) + ")";
			return $"{Name}{parameterTypes}";
		}

		public CommandMenuItem(string name, Func<TIn1, TIn2, TIn3, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2, CommandParameter<TIn3> prm3)
			: base(name, null, prm1, prm2, prm3)
		{
			this.command = command;
		}

		public CommandMenuItem(string name, MenuItem owner, Func<TIn1, TIn2, TIn3, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2, CommandParameter<TIn3> prm3)
			: base(name, owner, prm1, prm2, prm3)
		{
			this.command = command;
		}
	}

	public class CommandMenuItem<TIn1, TIn2, TIn3, TIn4, TResult> : CommandMenuItem
	{
		private readonly Func<TIn1, TIn2, TIn3, TIn4, TResult> command;

		protected override ActionResult InnerExecute(CommandParameterValue[] paramValues)
		{
			try
			{
				var inPrm1 = (CommandParameterValue<TIn1>)paramValues[0];
				var inPrm2 = (CommandParameterValue<TIn2>)paramValues[1];
				var inPrm3 = (CommandParameterValue<TIn3>)paramValues[2];
				var inPrm4 = (CommandParameterValue<TIn4>)paramValues[3];
				return new CommandResult(command(inPrm1.Value, inPrm2.Value, inPrm3.Value, inPrm4.Value));
			}
			catch (Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			var inPrm1 = (CommandParameter<TIn1>)parameters[0];
			var inPrm2 = (CommandParameter<TIn2>)parameters[1];
			var inPrm3 = (CommandParameter<TIn3>)parameters[2];
			var inPrm4 = (CommandParameter<TIn4>)parameters[3];
			return new CommandMenuItem<TIn1, TIn2, TIn3, TIn4, TResult>(this.Name, this.Owner, this.command, inPrm1, inPrm2, inPrm3, inPrm4);
		}

		public override string ToString()
		{
			var parameterTypes = "(" + string.Join(", ", base.parameters.Select(prm => prm.Name)) + ")";
			return $"{Name}{parameterTypes}";
		}

		public CommandMenuItem(string name, Func<TIn1, TIn2, TIn3, TIn4, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2, CommandParameter<TIn3> prm3, CommandParameter<TIn4> prm4)
			: base(name, null, prm1, prm2, prm3, prm4)
		{
			this.command = command;
		}

		public CommandMenuItem(string name, MenuItem owner, Func<TIn1, TIn2, TIn3, TIn4, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2, CommandParameter<TIn3> prm3, CommandParameter<TIn4> prm4)
			: base(name, owner, prm1, prm2, prm3, prm4)
		{
			this.command = command;
		}
	}

	public class CommandMenuItem<TIn1, TIn2, TIn3, TIn4, TIn5, TResult> : CommandMenuItem
	{
		private readonly Func<TIn1, TIn2, TIn3, TIn4, TIn5, TResult> command;

		protected override ActionResult InnerExecute(CommandParameterValue[] paramValues)
		{
			try
			{
				var inPrm1 = (CommandParameterValue<TIn1>)paramValues[0];
				var inPrm2 = (CommandParameterValue<TIn2>)paramValues[1];
				var inPrm3 = (CommandParameterValue<TIn3>)paramValues[2];
				var inPrm4 = (CommandParameterValue<TIn4>)paramValues[3];
				var inPrm5 = (CommandParameterValue<TIn5>)paramValues[4];
				return new CommandResult(command(inPrm1.Value, inPrm2.Value, inPrm3.Value, inPrm4.Value, inPrm5.Value));
			}
			catch (Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			var inPrm1 = (CommandParameter<TIn1>)parameters[0];
			var inPrm2 = (CommandParameter<TIn2>)parameters[1];
			var inPrm3 = (CommandParameter<TIn3>)parameters[2];
			var inPrm4 = (CommandParameter<TIn4>)parameters[3];
			var inPrm5 = (CommandParameter<TIn5>)parameters[4];
			return new CommandMenuItem<TIn1, TIn2, TIn3, TIn4, TIn5, TResult>(this.Name, this.Owner, this.command, inPrm1, inPrm2, inPrm3, inPrm4, inPrm5);
		}

		public override string ToString()
		{
			var parameterTypes = "(" + string.Join(", ", base.parameters.Select(prm => prm.Name)) + ")";
			return $"{Name}{parameterTypes}";
		}

		public CommandMenuItem(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2, CommandParameter<TIn3> prm3, CommandParameter<TIn4> prm4, CommandParameter<TIn5> prm5)
			: base(name, null, prm1, prm2, prm3, prm4, prm5)
		{
			this.command = command;
		}

		public CommandMenuItem(string name, MenuItem owner, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TResult> command, CommandParameter<TIn1> prm1, CommandParameter<TIn2> prm2, CommandParameter<TIn3> prm3, CommandParameter<TIn4> prm4, CommandParameter<TIn5> prm5)
			: base(name, owner, prm1, prm2, prm3, prm4, prm5)
		{
			this.command = command;
		}
	}
}
