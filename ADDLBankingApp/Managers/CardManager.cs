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
    public class CardManager
    {

        /// <summary>
        /// Card Endpoints
        /// </summary>
        string urlBase = "http://localhost:3000/api/Cards/";

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
        public async Task<IEnumerable<Card>> GetAllCard(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Card>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Card> GetCardById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Card>(resp);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="card"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Card> insertCard(Card card, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Card>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="card"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Card> updateCard(Card card, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(card), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Card>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Card> deleteCard(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Card>(await resp.Content.ReadAsStringAsync());
        }

    }
}