using DpaHttpClient;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace DPA.API.SampleApp
{
	public static class AuthMenuBuilder
	{
		public static DpaClient LogIn(string serverAddress, string userName, string password)
		{
			var client = new DpaClient(serverAddress);
			if (!client.Login(userName, password))
			{
				client = null;
			}

			return client;
		}
	}
}
