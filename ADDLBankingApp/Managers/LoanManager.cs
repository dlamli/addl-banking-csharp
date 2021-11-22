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
    public class LoanManager
    {

        /// <summary>
        /// Loan Endpoints
        /// </summary>
        string urlBase = "http://localhost:3000/api/Loans/";

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
        public async Task<IEnumerable<Loan>> GetAllLoan(string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(urlBase);

            return JsonConvert.DeserializeObject<IEnumerable<Loan>>(resp);
        }

        /// <summary>
        /// GET By Id
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Loan> GetLoanById(string token, string id)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.GetStringAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Loan>(resp);
        }


        /// <summary>
        /// POST
        /// </summary>
        /// <param name="loan"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Loan> insertLoan(Loan loan, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PostAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(loan), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Loan>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <param name="loan"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Loan> updateLoan(Loan loan, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.PutAsync(urlBase,
                new StringContent(JsonConvert.SerializeObject(loan), Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Loan>(await resp.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Loan> deleteLoan(string id, string token)
        {
            HttpClient httpClient = GetClient(token);

            var resp = await httpClient.DeleteAsync(string.Concat(urlBase, id));

            return JsonConvert.DeserializeObject<Loan>(await resp.Content.ReadAsStringAsync());
        }

    }
}