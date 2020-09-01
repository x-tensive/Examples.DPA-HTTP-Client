using DpaHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPA.API.SampleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.InputEncoding = Encoding.Unicode;

			var menuExplorer = BuildMenu();
			menuExplorer.Run();

			while (true) { }
		}

		private static MenuExplorer BuildMenu()
		{
			var menuRoot = new SubMenuItem("root");
			var menu = new Menu(menuRoot);
			var menuExplorer = new MenuExplorer(menu);

			menuRoot.AddItem(new CommandMenuItem<IEnumerable<Equipment>>("Get workcenters list", () => { return Examples.GetEquipments(menuExplorer.Client); }));
			menuRoot.AddItem(new CommandMenuItem<string, IEnumerable<Downtime>>("Get downtime for workcenter by last day", (equipmentName) =>
			{
				var equipmentId = Examples.GetEquipmentIdByName(menuExplorer.Client, equipmentName);
				return Examples.GetDowntimes(menuExplorer.Client, equipmentId, false, FilterPeriodType.LastNDays);
			}, new CommandParameter<string>("Workcenter name")));

			var journalsSubMenu = new SubMenuItem("Journal samples", new MenuItem[] {
				new CommandMenuItem<IEnumerable<MachineStateRecord>>("Get last 20 records from journal of Machine States", () => { return Examples.GetMachineStatus(menuExplorer.Client); }),
				new CommandMenuItem<IEnumerable<ProcessingProgramRecord>>("Get last 20 records from journal of Processing programs", () => { return Examples.GetProcessingProgramRecord(menuExplorer.Client); })
			});
			menuRoot.AddItem(journalsSubMenu);

			var ordersSubMenu = new SubMenuItem("Order samples", new MenuItem[] {
				new CommandMenuItem<IEnumerable<GridOrder>>("Get orders top 20 list", () => { return Examples.GetOrders(menuExplorer.Client); }),
				new CommandMenuItem<IEnumerable<GridOrder>>("Get completed orders top 20 list", () => { return Examples.GetCompletedOrders(menuExplorer.Client); }),
				new CommandMenuItem<string, IEnumerable<GridOrder>>("Get completed orders for last month by workcenter", (equipmentName) => {
					return Examples.GetCompletedOrdersForLastMonthByEquipment(menuExplorer.Client, equipmentName);
				}, new CommandParameter<string>("Workcenter name"))
			});
			menuRoot.AddItem(ordersSubMenu);

			var operatorSubMenu = new SubMenuItem("Operator samples", new MenuItem[] {
				new CommandMenuItem<string, bool>("Set 'previous operation completed' attribute", (jobExternalIdentifier) => {
					Examples.SetPreviousOperationCompleted(menuExplorer.Client, jobExternalIdentifier, true);
					return true;
				}, new CommandParameter<string>("Job identifier in external system")),
				new CommandMenuItem<string, bool>("Add 'Awaiting for transport' ticket", (equipmentName) => {
					Examples.AddTicket(menuExplorer.Client, equipmentName);
					return true;
				}, new CommandParameter<string>("Workcenter name"))
			});
			menuRoot.AddItem(operatorSubMenu);

			var indicatorsSubMenu = new SubMenuItem("Indicator samples",
				new MenuItem[] {
					new CommandMenuItem<string, IdNameContainer[]>("Get indicators list",
						(equipmentName) => {
							return Examples.GetIndicatorList(menuExplorer.Client, equipmentName);
						},
						new CommandParameter<string>("Workcenter name")
					),
					new CommandMenuItem<string, string, DateTimeOffset, DateTimeOffset, IndicatorPoint[]>("Get indicator values",
						(equipmentName, indicatorName, start, end) => {
							return Examples.GetIndicatorValues(menuExplorer.Client, equipmentName, indicatorName, start, end);
						},
						new CommandParameter<string>("Workcenter name"),
						new CommandParameter<string>("Indicator name"),
						new CommandParameter<DateTimeOffset>("Start"),
						new CommandParameter<DateTimeOffset>("end"))
				});
			menuRoot.AddItem(indicatorsSubMenu);
			return menuExplorer;
		}
	}
}
