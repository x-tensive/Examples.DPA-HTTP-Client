﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DpaHttpClient
{
    public static class Examples
    {
        public static Exception NotFoundException(string entity, string name)
        {
            return new Exception($"{entity} does not found by name '{name}'");
        }

        /// <summary>
        /// Login.
        /// Cookies are stored in dpaClient.
        /// </summary>
        /// <returns>Returns an authorized client</returns>
        public static DpaClient Login(string login, string password, string serverUrlAddress)
        {
            var dpaClient = new DpaClient(serverUrlAddress);
            dpaClient.Login(login, password);
            return dpaClient;
        }

        /// <summary>
        /// The method of obtaining equipment available to the operator.
        /// Operator - the user under whom it is necessary to authorize the client.
        /// [HttpGet("getEquipments")]
        /// </summary>
        /// <param name="dpaClient">Authorized client</param>
        /// <returns>Returns a collection of equipment</returns>
        public static IEnumerable<Equipment> GetEquipments(DpaClient dpaClient)
        {
            var filter = new GridRequestOptions();

            var url = "/api/dpaEnterpriseStrusture/getEquipments/";

            var postParams = JsonConvert.SerializeObject(filter);
            var postResult = dpaClient.Post(url, postParams);
            return JsonConvert.DeserializeObject<IEnumerable<Equipment>>(postResult);
        }


        /// <summary>
        /// Returns workcenter id by workcenter name
        /// </summary>
        /// <param name="dpaClient">Authorized client</param>
        /// <param name="equipmentName">Workcenter name</param>
        /// <returns></returns>
        public static long GetEquipmentIdByName(DpaClient dpaClient, string equipmentName)
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
            var result = dpaClient.Post(url, postParams);

            if (string.IsNullOrEmpty(result))
                throw NotFoundException("Equipment", equipmentName);

            var resultObject = JsonConvert.DeserializeObject<IdNameContainer[]>(result);
            if (resultObject.Length == 0)
                throw NotFoundException("Equipment", equipmentName);

            return resultObject[0].Id;
        }

        /// <summary>
        /// The method of obtaining downtime.
        /// [HttpGet("getDowntimes/{equipmentId}/{showUnclassifiedOnly}/{periodType}/{periodNumber}/{skip}/{take}")]
        /// </summary>
        /// <param name = "dpaClient"> Authorized client </param>
        /// <param name = "equipmentId"> Equipment Id </param>
        /// <param name = "showUnclassifiedOnly"> Only unclassified downtime </param>
        /// <param name = "filterPeriodType"> Type of period for which we receive downtime </param>
        /// <param name = "periodNumber"> Number of months, weeks, days, hours of the selected period type </param>
        /// <param name = "skipQuantity"> Skip the first skipQuantity downtime </param>
        /// <param name = "takeQuantity"> Take takeQuantity downtime for a given period </param>
        /// <returns> Returns a collection of downtimes </returns>
        public static IEnumerable<Downtime> GetDowntimes(DpaClient dpaClient, long equipmentId, bool showUnclassifiedOnly, FilterPeriodType filterPeriodType, long periodNumber = 1, int skipQuantity = 0, int takeQuantity = 10)
        {
            var downtimesJson = dpaClient.Get($"/api/OperatorV5Job/getDowntimes/{equipmentId}/{showUnclassifiedOnly.ToString()}/{filterPeriodType}/{periodNumber}/{skipQuantity}/{takeQuantity}");
            var downtimesData = JObject.Parse(downtimesJson)["data"].ToString();
            return JsonConvert.DeserializeObject<List<Downtime>>(downtimesData);
        }

        /// <summary>
        /// Method for obtaining a list of machine statuses.
        /// </summary>
        /// <param name = "dpaClient"> Authorized client </param>
        /// <returns> Returns a list of machine states </return
        public static IEnumerable<MachineStateRecord> GetMachineStatus(DpaClient dpaClient)
        {
            var filter = new GridRequestOptions()
            {
                Skip = 0,
                Take = 20,
            };
            var gridOptions = new EventLogBaseGridFilter()
            {
                GridOptions = filter,
                DateTimeOffsetFrom = DateTimeOffset.Now.AddDays(-1),    // Beginning of period.
                DateTimeOffsetUntil = DateTimeOffset.Now,               // End of period.
                ItemIds = { }                                           // List of id equipmets (machines).
            };
            var processingProgramRecordJson = dpaClient.GetJournalDatas("MachineStateRecord", gridOptions);
            return JsonConvert.DeserializeObject<List<MachineStateRecord>>(processingProgramRecordJson);
        }

        /// <summary>
        /// Method for obtaining a list of programs executed on machines.
        /// </summary>
        /// <param name = "dpaClient"> Authorized client </param>
        /// <returns> Returns a list of running programs on machines </returns>
        public static IEnumerable<ProcessingProgramRecord> GetProcessingProgramRecord(DpaClient dpaClient)
        {
            var filter = new GridRequestOptions()
            {
                Skip = 0,
                Take = 20,
            };
            var gridOptions = new EventLogBaseGridFilter()
            {
                GridOptions = filter,
                DateTimeOffsetFrom = DateTimeOffset.Now.AddDays(-1),
                DateTimeOffsetUntil = DateTimeOffset.Now,
                ItemIds = { }
            };
            var processingProgramRecordJson = dpaClient.GetJournalDatas("ProcessingProgramRecord", gridOptions);
            return JsonConvert.DeserializeObject<List<ProcessingProgramRecord>>(processingProgramRecordJson);
        }

        /// <summary>
        /// Method for getting the list of tasks.
        /// </summary>
        public static IEnumerable<GridOrder> GetOrders(DpaClient dpaClient)
        {
            var filter = new GridRequestOptions()
            {
                Skip = 0,
                Take = 20,
            };

            var processingProgramRecordJson = dpaClient.GetOrders(filter);
            return JsonConvert.DeserializeObject<List<GridOrder>>(processingProgramRecordJson);
        }

        /// <summary>
        /// Method for obtaining a list of completed tasks.
        /// </summary>
        public static IEnumerable<GridOrder> GetCompletedOrders(DpaClient dpaClient)
        {
            var filter = new GridRequestOptions()
            {
                Skip = 0,
                Take = 20,
                Filter = new object[]
                {
                    new string[]
                    {
                        "status",
                        "=",
                        ((int)JobStatus.Completed).ToString()
                    }
                }
            };

            var processingProgramRecordJson = dpaClient.GetOrders(filter);
            return JsonConvert.DeserializeObject<List<GridOrder>>(processingProgramRecordJson);
        }

        /// <summary>
        /// Method for obtaining the list of completed tasks for the last month for a specific equipment.
        /// </summary>
        public static IEnumerable<GridOrder> GetCompletedOrdersForLastMonthByEquipment(DpaClient dpaClient, string equipmentName)
        {
            var filter = new GridRequestOptions()
            {
                Skip = 0,
                Take = 20,
                Filter = new object[]
                {
                    new object[]
                    {
                        nameof(GridOrder.Status),
                        "=",
                        JobStatus.Completed
                    },
                    "and",
                    new object[]
                    {
                        nameof(GridOrder.ActualEnd),
                        ">=",
                        DateTime.Now.AddMonths(-1)
                    },
                    "and",
                    new object[]
                    {
                        nameof(GridOrder.Equipment),
                        "contains",
                        equipmentName // Name of the equipment.
                    },
                }
            };

            var processingProgramRecordJson = dpaClient.GetOrders(filter);
            return JsonConvert.DeserializeObject<List<GridOrder>>(processingProgramRecordJson);
        }

        /// <summary>
        /// Method for adding ticket for dispatcher
        /// </summary>
        public static void AddTicket(DpaClient dpaClient, string equipmentName)
        {
            var ticketService = new DpaTicketService(dpaClient);
            ticketService.AddAwaitingForTransportTicket(equipmentName);
        }

        /// <summary>
        /// Returns collection of indicator values by period
        /// </summary>
        /// <param name="client">Authorized client</param>
        /// <param name="equipmentName">Workcenter name</param>
        /// <param name="indicatorName">Indicator name</param>
        /// <param name="start">Period start</param>
        /// <param name="end">Period end</param>
        /// <returns></returns>
        public static IndicatorPoint[] GetIndicatorValues(DpaClient client, string equipmentName, string indicatorName, DateTimeOffset start, DateTimeOffset end)
        {
            var indicatorService = new DpaIndicatorService(client);
            return indicatorService.GetIndicatorValues(equipmentName, indicatorName, start, end);
        }

        /// <summary>
        /// Returns collection of indicators by workcenter
        /// </summary>
        /// <param name="client">Authorized client</param>
        /// <param name="equipmentName">Workcenter name</param>
        /// <returns></returns>
        public static IdNameContainer[] GetIndicatorList(DpaClient client, string equipmentName)
        {
            var indicatorService = new DpaIndicatorService(client);
            return indicatorService.GetIndicatorList(equipmentName);
        }

        public static void SetPreviousOperationCompleted(DpaClient dpaClient, string jobExternalIdentifier, bool completed)
        {
            dpaClient.SetPreviousOperationCompleted(jobExternalIdentifier, completed);
        }
    }
}
