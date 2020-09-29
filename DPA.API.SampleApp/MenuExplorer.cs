using DpaHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPA.API.SampleApp
{
	public class MenuExplorer
	{
		private readonly Menu loginMenu;
		private readonly Menu mainMenu;

		private Menu currentMenu;

		private readonly MenuItem logInMenuItem;
		private readonly MenuItem logOutMenuItem;
		private readonly MenuItem exitMenuItem;
		private readonly MenuItem mainMenuMenuItem;
		private readonly MenuItem levelUpMenuItem;

		private DpaClient dpaClient;

		public DpaClient Client => dpaClient;

		private void PreselectMenu()
		{
			if (dpaClient == null)
			{
				currentMenu = loginMenu;
			}
			else
			{
				currentMenu = mainMenu;
			}
		}

		private void ShowMenu()
		{
			Console.WriteLine();
			Console.WriteLine();

			var menuItems = currentMenu.GetCurrentItems().ToArray();
			Console.WriteLine(menuItems[0].GetPath());

			for (var ix = 0; ix < menuItems.Length; ix++)
			{
				var item = menuItems[ix];
				Console.WriteLine($"	{ix+1}. {item}");
			}
		}

		private ActionResult TrySelectMenuItem()
		{
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.Write("Select: ");
			Console.ResetColor();

			var value = Console.ReadLine();
			MenuItem selectedItem = null;
			var menuItems = currentMenu.GetCurrentItems().ToArray();
			if (int.TryParse(value, out var index) && index >= 1 && index <= menuItems.Length)
			{
				selectedItem = menuItems[index-1];
			}

			return currentMenu.SelectMenuItem(selectedItem);
		}

		private void ShowResult(ActionResult selecteionResult)
		{
			Console.WriteLine();
			switch (selecteionResult)
			{
				case CommandResult cmdResult:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Operation result is:");
					//var resultJson = JsonConvert.SerializeObject(cmdResult.Result);
					var resultList = (cmdResult.Result as IEnumerable)?.Cast<object>()?.ToList();
					if (resultList != null)
					{
						foreach (var item in resultList)
						{
							Console.WriteLine(item);
						}
					}
					else
					{
						Console.WriteLine(cmdResult.Result);
					}
					break;
				case FailResult failResult:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(failResult.Message);
					break;
			}

			Console.ResetColor();
		}

		public void Run()
		{
			while (true)
			{
				PreselectMenu();
				ShowMenu();
				var selecteionResult = TrySelectMenuItem();
				if (selecteionResult is FailResult || selecteionResult is CommandResult)
				{
					ShowResult(selecteionResult);
				}
			}
		}

		public MenuExplorer(Menu menu)
		{
			logInMenuItem = new CommandMenuItem<string, string, string, bool>("Log in", (server, user, password) => { dpaClient = AuthMenuBuilder.LogIn(server, user, password); return dpaClient != null; },
				new CommandParameter<string>("server"),
				new CommandParameter<string> ("user"),
				new CommandParameter<string> ("password"));
			logOutMenuItem = new CommandMenuItem<bool>("Log out", () => { dpaClient = null; return true; });
			exitMenuItem = new CommandMenuItem<bool>("Exit", () => { Environment.Exit(-1); return true; });
			mainMenuMenuItem = new CommandMenuItem<bool>("To main menu", () => { mainMenu.NavigateToMainMenu(); return true; });
			levelUpMenuItem = new CommandMenuItem<bool>("Level up", () => { mainMenu.NavigateToLevelUp(); return true; });

			loginMenu = new Menu(new SubMenuItem("root", new[] { logInMenuItem }));
			loginMenu.Extend(exitMenuItem);

			mainMenu = menu;
			mainMenu.Extend(new[] {
					mainMenuMenuItem,
					levelUpMenuItem,
					logOutMenuItem,
					exitMenuItem
				});

			currentMenu = loginMenu;
		}
	}
}
