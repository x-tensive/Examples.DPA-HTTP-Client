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
			Console.InputEncoding = Encoding.ASCII;

			var menuExplorer = BuildMenu();
			menuExplorer.Run();

			while (true) { }
		}

		private static MenuExplorer BuildMenu()
		{
			var menuRoot = new SubMenuItem("root");
			var menu = new Menu(menuRoot);
			var menuExplorer = new MenuExplorer(menu);

			menuRoot.AddItem(new CommandMenuItem<IEnumerable<Equipment>>("Get workcenters list", (p, v) => { return Examples.GetEquipments(menuExplorer.Client); }));
			menuRoot.AddItem(new CommandMenuItem<IEnumerable<Downtime>>("Get downtime for workcenter by last day", (p, v) =>
			{
				var equipment = Examples.GetEquipments(menuExplorer.Client).FirstOrDefault();
				if (equipment == null)
					return Enumerable.Empty<Downtime>();
				return Examples.GetDowntimes(menuExplorer.Client, equipment.Id, false, FilterPeriodType.LastNDays);
			}));

			var journalsSubMenu = new SubMenuItem("Journal samples", new MenuItem[] {
				new CommandMenuItem<IEnumerable<MachineStateRecord>>("Get last 20 records from journal of Machine States", (p, v) => { return Examples.GetMachineStatus(menuExplorer.Client); }),
				new CommandMenuItem<IEnumerable<ProcessingProgramRecord>>("Get last 20 records from journal of Processing programs", (p, v) => { return Examples.GetProcessingProgramRecord(menuExplorer.Client); })
			});
			menuRoot.AddItem(journalsSubMenu);

			var ordersSubMenu = new SubMenuItem("Order samples", new MenuItem[] {
				new CommandMenuItem<IEnumerable<GridOrder>>("Get orders top 20 list", (p, v) => { return Examples.GetOrders(menuExplorer.Client); }),
				new CommandMenuItem<IEnumerable<GridOrder>>("Get completed orders top 20 list", (p, v) => { return Examples.GetCompletedOrders(menuExplorer.Client); }),
				new CommandMenuItem<IEnumerable<GridOrder>>("Get completed orders for last month by workcenter", (p, v) => {
					return Examples.GetCompletedOrdersForLastMonthByEquipment(menuExplorer.Client, v[0]);
				}, new CommandParameter("Workcenter name", CommandParameterType.String))
			});
			menuRoot.AddItem(ordersSubMenu);

			var operatorSubMenu = new SubMenuItem("Operator samples", new MenuItem[] {
				new CommandMenuItem<bool>("Set 'previous operation completed' attribute", (p, v) => {
					Examples.SetPreviousOperationCompleted(menuExplorer.Client, v[0], true);
					return true;
				}, new CommandParameter("Job identifier in external system", CommandParameterType.String)),
				new CommandMenuItem<bool>("Add 'Awaiting for transport' ticket", (p, v) => {
					Examples.AddTicket(menuExplorer.Client, v[0]);
					return true;
				}, new CommandParameter("Workcenter name", CommandParameterType.String))
			});
			menuRoot.AddItem(operatorSubMenu);

			var indicatorsSubMenu = new SubMenuItem("Indicator samples",
				new MenuItem[] {
					new CommandMenuItem<IdNameContainer[]>("Get indicators list",
						(p, v) => {
							var equipmentName = v[0];
							return Examples.GetIndicatorList(menuExplorer.Client, equipmentName);
						},
						new CommandParameter("Workcenter name", CommandParameterType.String)
					),
					new CommandMenuItem<IndicatorPoint[]>("Get indicator values",
						(p, v) => {
							var equipmentName = v[0];
							var indicatorName = v[1];
							var start = DateTimeOffset.Parse(v[2]);
							var end = DateTimeOffset.Parse(v[3]);
							return Examples.GetIndicatorValues(menuExplorer.Client, equipmentName, indicatorName, start, end);
						},
						new CommandParameter("Workcenter name", CommandParameterType.String),
						new CommandParameter("Indicator name", CommandParameterType.String),
						new CommandParameter("Start", CommandParameterType.DateTime),
						new CommandParameter("end", CommandParameterType.DateTime))
				});
			menuRoot.AddItem(indicatorsSubMenu);
			return menuExplorer;
		}
	}
}
