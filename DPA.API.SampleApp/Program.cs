using DpaHttpClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DPA.API.SampleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var menuRoot = new SubMenuItem("root");
			var menu = new Menu(menuRoot);
			var menuExplorer = new MenuExplorer(menu);

			menuRoot.AddItem(new CommandMenuItem<IEnumerable<Equipment>>("Get equipment list", (p, v) => { return Examples.GetEquipments(menuExplorer.Client); }));
			menuRoot.AddItem(new CommandMenuItem<IEnumerable<Downtime>>("Get downtime for first equipmrnt by last day", (p, v) => {
				var equipment = Examples.GetEquipments(menuExplorer.Client).FirstOrDefault();
				if (equipment == null)
					return Enumerable.Empty<Downtime>();
				return Examples.GetDowntimes(menuExplorer.Client, equipment.Id, false, FilterPeriodType.LastNDays);
			}));
			menuRoot.AddItem(new CommandMenuItem<IEnumerable<MachineStateRecord>>("Get last 20 records from journal of Machine States", (p, v) => { return Examples.GetMachineStatus(menuExplorer.Client); }));
			menuRoot.AddItem(new CommandMenuItem<IEnumerable<ProcessingProgramRecord>>("Get last 20 records from journal of Processing programs", (p, v) => { return Examples.GetProcessingProgramRecord(menuExplorer.Client); }));
			menuRoot.AddItem(new CommandMenuItem<IEnumerable<GridOrder>>("Get orders top 20 list", (p, v) => { return Examples.GetOrders(menuExplorer.Client); }));
			menuRoot.AddItem(new CommandMenuItem<IEnumerable<GridOrder>>("Get completed orders top 20 list", (p, v) => { return Examples.GetCompletedOrders(menuExplorer.Client); }));


			menuExplorer.Run();
			while (true) { }
		}
	}
}
