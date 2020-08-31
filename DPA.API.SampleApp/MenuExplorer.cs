using DpaHttpClient;
using Newtonsoft.Json;
using System;
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

		private MenuResult TrySelectMenuItem()
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

		private void ShowResult(MenuResult selecteionResult)
		{
			Console.WriteLine();
			switch (selecteionResult)
			{
				case CommandResult cmdResult:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Operation result is:");
					var resultJson = JsonConvert.SerializeObject(cmdResult.Result);
					Console.WriteLine(resultJson);
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
			logInMenuItem = new CommandMenuItem<bool>("Log in", (p, v) => { dpaClient = AuthMenuBuilder.LogIn(p, v); return dpaClient != null; }, new[] {
				new CommandParameter ("server", CommandParameterType.String),
				new CommandParameter ("user", CommandParameterType.String),
				new CommandParameter ("password", CommandParameterType.String)
			});
			logOutMenuItem = new CommandMenuItem<bool>("Log out", (p, v) => { dpaClient = null; return true; });
			exitMenuItem = new CommandMenuItem<bool>("Exit", (p, v) => { Environment.Exit(-1); return true; });
			mainMenuMenuItem = new CommandMenuItem<bool>("To main menu", (p, v) => { mainMenu.NavigateToMainMenu(); return true; });
			levelUpMenuItem = new CommandMenuItem<bool>("Level up", (p, v) => { mainMenu.NavigateToLevelUp(); return true; });

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
