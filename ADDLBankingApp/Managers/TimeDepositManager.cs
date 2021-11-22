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
    public class TimeDepositManager
    {

        /// <summary>
        /// TimeDeposit Endpoints
        /// </summary>
        string urlBase = "http://localhost:3000/api/TimeDeposits/";

        /// <summary>
        /// Get Client
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        HttpClient GetClient(string token)
        {
            HttpClient httpClient = new HttpClient();


            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            return httpClient;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TimeDeposit>> GetAllTimeDeposit(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<TimeDeposit>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TimeDeposit> GetTimeDepositById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<TimeDeposit>(resp);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="timeDeposit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimeDeposit> insertTimeDeposit(TimeDeposit timeDeposit, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(timeDeposit), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<TimeDeposit>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="timeDeposit"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimeDeposit> updateTimeDeposit(TimeDeposit timeDeposit, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(timeDeposit), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<TimeDeposit>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TimeDeposit> deleteTimeDeposit(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<TimeDeposit>(await resp.Content.ReadAsStringAsync());
        }

    }
}