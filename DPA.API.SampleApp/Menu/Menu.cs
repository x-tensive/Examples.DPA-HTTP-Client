using DpaHttpClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DPA.API.SampleApp
{
	public class Menu
	{
		private readonly MenuItem rootItem;

		private MenuItem currentItem;
		private MenuItem[] additionalItems=Array.Empty<MenuItem>();

		private string[] ReadParams(CommandParameter[] cmdParams)
		{
			if (cmdParams.Length > 0)
			{
				Console.WriteLine($"Please specify parameters ");
			}

			var values = new string[cmdParams.Length];
			for(var ix=0; ix<cmdParams.Length;ix++)
			{
				var prm = cmdParams[ix];
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"	{prm.Name} ({prm.Type}): ");
				Console.ResetColor();
				values[ix] = Console.ReadLine();
			}
			return values;
		}

		public IEnumerable<MenuItem> GetCurrentItems()
		{
			return currentItem.Items
				.Union(additionalItems);
		}

		public void Extend(params MenuItem[] additionalItems)
		{
			this.additionalItems = additionalItems;
		}

		public void NavigateToMainMenu()
		{
			currentItem = rootItem;
		}

		public void NavigateToLevelUp()
		{
			if (currentItem.Owner != null)
			{
				currentItem = currentItem.Owner;
			}
		}

		public ActionResult SelectMenuItem(MenuItem menuItem)
		{
			var errorMessage = string.Empty;
			if (!GetCurrentItems().Contains(menuItem))
				return FailResult.Get($"Menu item not found");

			try
			{
				switch (menuItem)
				{
					case CommandMenuItem command:
						var cmdParams = command.GetParameters();
						var paramValues = ReadParams(cmdParams);
						return command.Execute(paramValues);
					default:
						currentItem = menuItem;
						return OkMenuResult.Get();
				}
			}
			catch(Exception ex)
			{
				errorMessage = ex.Message;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Something goes wrong");
				Console.ResetColor();
			}

			return FailResult.Get(errorMessage);
		}

		public Menu(MenuItem rootItem)
		{
			this.rootItem = rootItem;
			this.currentItem = rootItem;
		}
	}
}
