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
    public class SessionManager
    {

        /// <summary>
        /// Session Endpoints
        /// </summary>
        string urlBase = "http://localhost:49220/api/Sessions/";

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
        public async Task<IEnumerable<Session>> GetAllSession(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Session>>(resp);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="session"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Session> insertSession(Session session, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(session), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Session>(await resp.Content.ReadAsStringAsync());
        }

    }
}