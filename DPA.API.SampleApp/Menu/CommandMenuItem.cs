using DpaHttpClient;
using System;
using System.Linq;
using System.Text;

namespace DPA.API.SampleApp
{
	public abstract class CommandMenuItem : MenuItem
	{
		protected readonly CommandParameter[] parameters;

		private ActionResult EnsureParametersAreValid(string[] parameterValues)
		{
			if (parameterValues.Length != parameters.Length)
			{
				return FailResult.Get($"Not all parameters are specified");
			}

			var errors = new StringBuilder();
			var hasErrors = false;
			for (var ix = 0; ix < parameters.Length; ix++)
			{
				if (!parameters[ix].TryValidate(parameterValues[ix], out var error))
				{
					errors.AppendLine(error);
					hasErrors = true;
				}
			}

			if (hasErrors)
			{
				return FailResult.Get(errors.ToString());
			}

			return OkMenuResult.Get();
		}

		protected abstract ActionResult InnerExecute(CommandParameter[] parameters, string[] values);

		public CommandParameter[] GetParameters() => parameters;

		public ActionResult Execute(string[] parameterValues)
		{
			var result = EnsureParametersAreValid(parameterValues);
			if (!result.IsOK)
			{
				return result;
			}

			return InnerExecute(parameters, parameterValues);
		}

		protected CommandMenuItem(string name, MenuItem owner, CommandParameter[] parameters)
			: base(name, owner, null)
		{
			this.parameters = parameters;
		}
	}


	public class CommandMenuItem<T> : CommandMenuItem
	{
		private readonly Func<CommandParameter[], string[], T> command;

		protected override ActionResult InnerExecute(CommandParameter[] parameters, string[] parameterValues)
		{
			try { return new CommandResult(command(parameters, parameterValues)); }
			catch(Exception ex) { return FailResult.Get(ex.Message); }
		}

		public override MenuItem Clone()
		{
			return new CommandMenuItem<T>(this.Name, this.Owner, this.command, this.parameters);
		}

		public override string ToString()
		{
			var parameterTypes = string.Join(", ", base.parameters.Select(prm => prm.Type));
			return $"{Name}({parameterTypes})";
		}

		public CommandMenuItem(string name, Func<CommandParameter[], string[], T> command)
			: base(name, null, Array.Empty<CommandParameter>())
		{
			this.command = command;
		}

		public CommandMenuItem(string name, Func<CommandParameter[], string[], T> command, params CommandParameter[] parameters)
			: base(name, null, parameters)
		{
			this.command = command;
		}

		public CommandMenuItem(string name, MenuItem owner, Func<CommandParameter[], string[], T> command, params CommandParameter[] parameters)
			: base(name, owner, parameters)
		{
			this.command = command;
		}
	}
}
