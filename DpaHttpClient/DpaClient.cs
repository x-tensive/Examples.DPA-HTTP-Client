using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DpaHttpClient
{
    public class DpaClient
    {
        private CookieContainer CookieContainer { get; set; }

        private HttpClient Client { get; set; }

        public DpaClient(string baseAddress)
        {
            HttpClientHandler handler = new HttpClientHandler();
            CookieContainer = new CookieContainer();
            handler.CookieContainer = CookieContainer;

            Client = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        /// <summary>
        /// User login. Cookies are automatically stored in the client.
        /// </summary>
        public void Login(string userName, string password)
        {
            string payload = JsonConvert.SerializeObject(new
            {
                userName = userName,
                password = password,
            });

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = Client.PostAsync("/api/Account/Login", content).Result;
        }

        public string Get(string url)
        {
            var response = Client.GetAsync(url).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        public string Post(string url, string postParams)
        {
            var response = Client.PostAsync(url, new StringContent(postParams, Encoding.UTF8, "application/json")).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// The method returns log data filtered by the specified filter.
        /// [HttpPost ("getJournalDatas/{journalTypeName}")]
        /// </summary>
        /// <returns> Returns log data filtered by the specified filter</returns>
        public string GetJournalDatas(string journalTypeName, EventLogBaseGridFilter filter)
        {
            var postParams = JsonConvert.SerializeObject(filter);
            var url = $"/api/Journals/getJournalDatas/{journalTypeName}";
            return Post(url, postParams);
        }

        /// <summary>
        /// The method returns a list of tasks (jobs)
        /// [HttpPost ("getJobDatas")]
        /// </summary>
        public string GetOrders(GridRequestOptions filter)
        {
            var postParams = JsonConvert.SerializeObject(filter);
            var url = $"/api/Jobs/getJobDatas/";
            return Post(url, postParams);
        }
    }
}
