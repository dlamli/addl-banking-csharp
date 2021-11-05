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
    public class TransferManager
    {

        /// <summary>
        /// Transfer Endpoints
        /// </summary>
        string urlBase = "http://localhost:49220/api/Transfers/";

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
        public async Task<IEnumerable<Transfer>> GetAllTransfer(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Transfer>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Transfer> GetTransferById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Transfer>(resp);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Transfer> insertTransfer(Transfer transfer, string token)
        {
            HttpClient httpClient = GetClient(token);
                
            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(transfer), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Transfer>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Transfer> updateTransfer(Transfer transfer, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(transfer), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Transfer>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Transfer> deleteTransfer(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Transfer>(await resp.Content.ReadAsStringAsync());
        }

    }
}