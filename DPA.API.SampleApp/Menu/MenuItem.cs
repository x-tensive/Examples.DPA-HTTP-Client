using System;
using System.Collections.Generic;
using System.Linq;

namespace DPA.API.SampleApp
{
	public abstract class MenuItem
	{
		private List<MenuItem> items = new List<MenuItem>();

		public string Name { get; }

		public MenuItem Owner { get; private set; }

		public MenuItem[] Items { get { return items.ToArray(); } }

		public MenuItem AddItem(MenuItem item)
		{
			var newItem = item.Clone();
			newItem.Owner = this;
			items.Add(newItem);

			return newItem;
		}

		public string GetPath()
		{
			var path = ">";
			var item = this;
			while(item.Owner != null && item.Owner != item)
			{
				path = item.Owner + ">" + path;
				item = item.Owner;
			}
			return path;
		}

		public abstract MenuItem Clone();

		protected MenuItem(string name, MenuItem owner, MenuItem[] items)
		{
			Name = name;
			Owner = owner;
			items = items ?? Array.Empty<MenuItem>();
			foreach (var item in items)
			{
				AddItem(item);
			}
		}

		protected MenuItem(string name)
			: this(name, null, null)
		{
		}

		protected MenuItem(string name, MenuItem[] items)
			: this(name, null, items)
		{
		}
	}
}
