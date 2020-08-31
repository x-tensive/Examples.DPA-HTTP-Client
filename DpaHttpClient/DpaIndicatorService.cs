using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpaHttpClient
{
	public class DpaIndicatorService
	{
		private class TimePeriod
		{
			public DateTimeOffset Start { get; set; }
			public DateTimeOffset End { get; set; }
		}

		private readonly DpaClient dpaClient;

		public long GetIndicatorByName(string equipmentName, string indicatorName)
		{
			var resultObject = GetIndicatorList(equipmentName);

			var indicator = resultObject.FirstOrDefault(ind => ind.Name == indicatorName);
			if (indicator == null)
				throw Examples.NotFoundException("Indicator", indicatorName);
			return indicator.Id;
		}

		public IdNameContainer[] GetIndicatorList(string equipmentName)
		{
			var url = "/api/dashboard/getIndicators";

			var equipmentId = Examples.GetEquipmentByName(dpaClient, equipmentName);

			var equipmentIdListParam = new List<long>() { equipmentId };
			var postParams = JsonConvert.SerializeObject(equipmentIdListParam);
			var postResult = dpaClient.Post(url, postParams);
			if (string.IsNullOrEmpty(postResult))
				throw new Exception("No one indicator found");

			var resultObject = JsonConvert.DeserializeObject<IdNameContainer[]>(postResult);
			return resultObject;
		}

		public IndicatorPoint[] GetIndicatorValues(string equipmentName, string indicatorName, DateTimeOffset start, DateTimeOffset end)
		{
			var indicatorId = GetIndicatorByName(equipmentName, indicatorName);

			var url = $"/api/gantt/getIndicatorData/{indicatorId}";
			var periodParam = new[] { new TimePeriod { Start = start, End = end } };
			var postParams = JsonConvert.SerializeObject(periodParam);
			
			var postResult = dpaClient.Post(url, postParams);
			if (string.IsNullOrEmpty(postResult))
				throw new Exception("No indicator data found");

			var resultObject = JsonConvert.DeserializeObject<IndicatorPoint[]>(postResult);
			return resultObject;
		}

		public DpaIndicatorService(DpaClient dpaClient)
		{
			this.dpaClient = dpaClient;
		}
	}
}
