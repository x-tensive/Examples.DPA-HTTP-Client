using Newtonsoft.Json;
using System;
using System.Linq;

namespace DpaHttpClient
{
	public class DpaTicketService
	{
		private class IdNameContainer
		{
			public long Id { get; set; }
			public string Name { get; set; }
		}

		private const string AWAITING_FOR_TRANSPORT_TICKET_NAME_RUS = "Жду транспорта";
		private const string AWAITING_FOR_TRANSPORT_TICKET_NAME_ENG = "Waiting for transport";
		private const string COMMENTARY = "From MES";
		private readonly DpaClient client;

		public DpaTicketService(DpaClient client)
		{
			this.client = client;
		}

		private void AddTicket(long equipmentId, long ticketId)
		{
			var url = "/api/ticket/createTicket/";
			var postParams = JsonConvert.SerializeObject(new
			{
				EquipmentId = equipmentId,
				TicketTypeId = ticketId,
				Comment = COMMENTARY
			});
			client.Post(url, postParams);
		}

		private Exception NotFoundException(string entity, string name)
		{
			return new Exception($"{entity} does not found by name {name}");
		}

		public bool TryGetTicketByName(string ticketName, out long id)
		{
			id = -1;
			var result = client.Get("/api/ticket/getTicketTypes/");
			var resultTickets = JsonConvert.DeserializeObject<IdNameContainer[]>(result);
			var resultTicket = resultTickets
				.Where(x => x.Name == ticketName)
				.SingleOrDefault();
			if (resultTicket == null)
				return false;
			id = resultTicket.Id;
			return true;
		}

		public long GetEquipmentByName(string equipmentName)
		{
			var filter = new GridRequestOptions()
			{
				Filter = new[] {
					"Name",
					"contains",
					equipmentName
				}
			};
			var url = "/api/dpaEnterpriseStrusture/getEquipments/";

			var postParams = JsonConvert.SerializeObject(filter);
			var result = client.Post(url, postParams);

			if (string.IsNullOrEmpty(result))
				throw NotFoundException("Equipment", equipmentName);

			var resultObject = JsonConvert.DeserializeObject<IdNameContainer[]>(result);
			if (resultObject.Length == 0)
				throw NotFoundException("Equipment", equipmentName);

			return resultObject[0].Id;
		}

		public void AddAwaitingForTransportTicket(string equipmentName)
		{
			var equipmentId = GetEquipmentByName(equipmentName);
			if (!TryGetTicketByName(AWAITING_FOR_TRANSPORT_TICKET_NAME_RUS, out long ticketId))
			{
				if (!TryGetTicketByName(AWAITING_FOR_TRANSPORT_TICKET_NAME_ENG, out ticketId))
				{
					throw NotFoundException("Ticket", AWAITING_FOR_TRANSPORT_TICKET_NAME_RUS);
				}
			}

			AddTicket(equipmentId, ticketId);
		}
	}
}