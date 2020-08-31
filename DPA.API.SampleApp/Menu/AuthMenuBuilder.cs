using DpaHttpClient;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace DPA.API.SampleApp
{
	public static class AuthMenuBuilder
	{
		public static DpaClient LogIn(CommandParameter[] parameters, object[] values)
		{
			string baseAddress = values[0].ToString();
			string userName = values[1].ToString();
			string password = values[2].ToString();

			var client = new DpaClient(baseAddress);
			if (!client.Login(userName, password))
			{
				client = null;
			}

			return client;
		}
	}
}
