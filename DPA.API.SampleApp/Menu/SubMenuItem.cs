using System;
using System.Linq;

namespace DPA.API.SampleApp
{
	public class SubMenuItem : MenuItem
	{
		public ActionResult NavigateTo(MenuItem menuItem)
		{
			if (Items.Contains(menuItem))
			{
				return OkMenuResult.Get();
			}

			var message = $"Wrong menu item '{menuItem.Name}'";
			return FailResult.Get(message);
		}

		public override MenuItem Clone()
		{
			return new SubMenuItem(this.Name, this.Owner, this.Items.Select(itm => itm.Clone()).ToArray());
		}

		public override string ToString()
		{
			return Name;
		}

		public SubMenuItem(string name, MenuItem owner, MenuItem[] items)
			: base(name, owner, items)
		{
		}

		public SubMenuItem(string name) 
			: base(name)
		{
		}

		public SubMenuItem(string name, MenuItem[] items) 
			: base(name,  items)
		{
		}
	}
}
