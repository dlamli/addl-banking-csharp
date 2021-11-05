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
    public class StadisticManager
    {

        /// <summary>
        /// Stadistic Endpoints
        /// </summary>
        string urlBase = "http://localhost:49220/api/Stadistics/";

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Stadistic>> GetAllStadistic()
        {
            HttpClient httpClient = new HttpClient();

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Stadistic>>(resp);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="stadistic"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Stadistic> insertStadistic(Stadistic stadistic)
        {
            HttpClient httpClient = new HttpClient();

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(stadistic), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Stadistic>(await resp.Content.ReadAsStringAsync());
        }

    }
}