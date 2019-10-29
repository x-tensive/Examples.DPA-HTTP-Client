using System.Linq;

namespace DpaHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Authorized client.
            var dpaClient = Examples.Login("login", "password", "http://localhost:6216");

            // Equipment list.
            var equipments = Examples.GetEquipments(dpaClient);

            // Downtime List.
            var downtimes = Examples.GetDowntimes(dpaClient, equipments.First().Id, false, FilterPeriodType.LastNDays);

            // Machine Status List.
            var machinesStates = Examples.GetMachineStatus(dpaClient);

            // List of programs executed on equipments.
            var processingProgramRecords = Examples.GetProcessingProgramRecord(dpaClient);

            // List of orders.
            var orders = Examples.GetOrders(dpaClient);

            // List of completed orders.
            var completedOrders = Examples.GetCompletedOrders(dpaClient);

            // List of completed orders for last month and for specific equipment.
            var completedOrdersForLastMonth = Examples.GetCompletedOrdersForLastMonth(dpaClient);
        }
    }
}
