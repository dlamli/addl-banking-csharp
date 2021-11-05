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
    public class CustomerManager
    {
        /// <summary>
        /// Customer Enpoints
        /// </summary>
        string urlLogin = "http://localhost:49220/api/login/authenticate/";
        string urlRegister = "http://localhost:49220/api/Customers/";

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public async Task<Customer> Authenticate(LoginRequest loginRequest)
        {
            HttpClient httpClient = new HttpClient();

            var resp = await httpClient.PostAsync(urlLogin,
                new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Customer>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<Customer> Register(Customer customer)
        {
            HttpClient httpClient = new HttpClient();

            var resp = await httpClient.PostAsync(urlRegister,
                new StringContent(JsonConvert.SerializeObject(customer),
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Customer>(await resp.Content.ReadAsStringAsync());
        }

    }
}