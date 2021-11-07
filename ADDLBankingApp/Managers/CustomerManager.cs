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
        string urlBase = "http://localhost:49220/api/Customers/";


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

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(customer),
                Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Customer>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Customer>> GetAllCustomer(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Customer>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Customer>(resp);
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="account"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Customer> updateCustomer(Customer customer, string token)
        {
            HttpClient httpClient = GetClient(token);

            var response = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Customer> deleteCustomer(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Customer>(await resp.Content.ReadAsStringAsync());
        }

    }
}