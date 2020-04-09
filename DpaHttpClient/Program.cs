using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DpaHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Authorized client.
            var dpaClient = Examples.Login(login: "admin", password: "admin", serverUrlAddress: "http://localhost:6216");

            // Equipment list.
            var equipments = Examples.GetEquipments(dpaClient);
            ViewResult("Список оборудования", equipments);

            // Downtime List.
            var downtimes = Examples.GetDowntimes(dpaClient, equipments.First().Id, false, FilterPeriodType.LastNDays);
            ViewResult("Список простоев", downtimes);

            // Machine Status List.
            var machinesStates = Examples.GetMachineStatus(dpaClient);
            ViewResult("Список состояний станков", machinesStates);

            // List of programs executed on equipments.
            var processingProgramRecords = Examples.GetProcessingProgramRecord(dpaClient);
            ViewResult("Список программ выполняемых на станке", processingProgramRecords);

            // List of orders.
            var orders = Examples.GetOrders(dpaClient);
            ViewResult("Список заданий", orders);

            // List of completed orders.
            var completedOrders = Examples.GetCompletedOrders(dpaClient);
            ViewResult("Список выполненных заданий", completedOrders);

            // List of completed orders for last month and for specific equipment.
            var completedOrdersForLastMonth = Examples.GetCompletedOrdersForLastMonth(dpaClient);
            ViewResult("Список выполненных заданий за последний месяц", completedOrdersForLastMonth);

            // Adds ticket for dispatcher
            if (equipments.Any())
            {
                Examples.AddTicket(dpaClient, equipments.FirstOrDefault().Name);
            }

            dpaClient.SetPreviousOperationCompleted("1000016132436", true);

            Console.ReadKey();
        }

        private static void ViewResult<T>(string name, IEnumerable<T> elements)
        {
            Console.WriteLine($"{name}:");
            foreach (T element in elements)
            {
                Console.WriteLine(JsonConvert.SerializeObject(element));
            }
            Console.WriteLine(Environment.NewLine);
        }
    }

}
