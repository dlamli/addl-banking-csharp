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
    public class PaymentManager
    {

        /// <summary>
        /// Payment Endpoints
        /// </summary>
        string urlBase = "http://localhost:49220/api/Payments/";

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
        public async Task<IEnumerable<Payment>> GetAllPayment(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Payment>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Payment> GetPaymentById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Payment>(resp);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Payment> insertPayment(Payment payment, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Payment>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Payment> updatePayment(Payment payment, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Payment>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Payment> deletePayment(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Payment>(await resp.Content.ReadAsStringAsync());
        }

    }
}