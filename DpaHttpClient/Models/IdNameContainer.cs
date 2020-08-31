using System;
using System.Collections.Generic;
using System.Text;

namespace DpaHttpClient
{
    public class IdNameContainer
    {
        public long Id { get; set; }
        public string Name { get; set; }

		public override string ToString()
		{
			return $"{Id}. {Name}";
		}
	}
}
