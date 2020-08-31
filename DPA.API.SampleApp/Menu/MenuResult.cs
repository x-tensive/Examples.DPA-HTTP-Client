using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DPA.API.SampleApp
{
	public abstract class MenuResult
	{
		public abstract bool IsOK { get; }

		public abstract string Message { get; }
	}

	public class OkMenuResult : MenuResult
	{
		private static readonly OkMenuResult instance = new OkMenuResult();

		public override bool IsOK => true;

		public override string Message => string.Empty;

		public static OkMenuResult Get() => instance;

		protected OkMenuResult()
		{
		}
	}

	public class FailResult : MenuResult
	{
		private readonly string errorMessage;

		public override bool IsOK => false;

		public override string Message => errorMessage;

		public static FailResult Get(string errorMessage) => new FailResult(errorMessage);

		private FailResult(string errorMessage)
		{
			this.errorMessage = errorMessage;
		}
	}

	public class CommandResult : OkMenuResult
	{
		public object Result { get; }

		public CommandResult(object result)
		{
			Result = result;
		}
	}
}
