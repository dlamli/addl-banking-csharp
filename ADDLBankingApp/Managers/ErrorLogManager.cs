using ADDLBankingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ADDLBankingApp.Managers
{
    public class ErrorLogManager
    {

        /// <summary>
        /// ErrorLog Endpoints
        /// </summary>
        string urlBase = "http://localhost:49220/api/ErrorLogs/";

        HttpClient GetErrorLog()
        {
            HttpClient httpClient = new HttpClient();

            return httpClient;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ErrorLog>> GetAllErrorLog()
        {
            HttpClient httpClient = GetErrorLog();

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<ErrorLog>>(resp);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="errorLog"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ErrorLog> insertErrorLog(ErrorLog errorLog)
        {
            HttpClient httpClient = new HttpClient();

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(errorLog), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<ErrorLog>(await resp.Content.ReadAsStringAsync());
        }

    }
}