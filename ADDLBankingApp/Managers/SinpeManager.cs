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
    public class SinpeManager
    {

        /// <summary>
        /// SinpeM Endpoints
        /// </summary>
        string urlBase = "http://localhost:3000/api/SinpeMs/";

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
        public async Task<IEnumerable<SinpeM>> GetAllSinpeM(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<SinpeM>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SinpeM> GetSinpeMById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<SinpeM>(resp);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="sinpeM"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SinpeM> insertSinpeM(SinpeM sinpeM, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(sinpeM), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<SinpeM>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="sinpeM"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SinpeM> updateSinpeM(SinpeM sinpeM, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(sinpeM), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<SinpeM>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SinpeM> deleteSinpeM(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<SinpeM>(await resp.Content.ReadAsStringAsync());
        }

    }
}