using NUnit.Framework;

namespace DpaHttpClient.Tests
{
	[TestFixture]
	public class DpaTicketService_Is
	{
		private const string dpaHostUrl = "http://dpademo/";
		private const string login = "operator";
		private const string password = "123";
		private const long expectedTicketId = 309280859;
		private const long expectedEquipmentId = 262136686;
		private const string ticketName = "Жду транспорта";
		private const string equipmentName = "Sewing Machine 1";

		[Test]
		public void AbleToGetEquipmentByName()
		{
			using (var client = new DpaClient(dpaHostUrl))
			{
				client.Login(login, password);
				var service = new DpaTicketService(client);
				var equipmentId = service.GetEquipmentByName(equipmentName);

				Assert.AreEqual(expectedEquipmentId, equipmentId);
			}
		}

		[Test]
		public void AbleToGetTicketByName()
		{
			using (var client = new DpaClient(dpaHostUrl))
			{
				client.Login(login, password);
				var service = new DpaTicketService(client);
				var ticketWasFound = service.TryGetTicketByName(ticketName, out var ticketId);

				Assert.IsTrue(ticketWasFound);
				Assert.AreEqual(expectedTicketId, ticketId);
			}
		}

		[Test]
		public void AbleToAddAwaitingForTransportTicket()
		{
			using (var client = new DpaClient(dpaHostUrl))
			{
				client.Login(login, password);

				var service = new DpaTicketService(client);
				service.AddAwaitingForTransportTicket(equipmentName);
			}
		}
	}
}
