using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ADDLBankingApi.Models;
namespace API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest();

            Customer customer = new Customer();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                                 ,[Identification]
                                                                 ,[Username]
                                                                 ,[Name]
                                                                 ,[MiddleName]
                                                                 ,[LastName]
                                                                 ,[Password]
                                                                 ,[Email]
                                                                 ,[Birthdate]
                                                                 ,[Status]
                                                                 ,[RoleId]
                                                            FROM [dbo].[Customer] Where [Username] = @Username and [Password] = @Password ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Username", loginRequest.Username);
                    sqlCommand.Parameters.AddWithValue("@Password", loginRequest.Password);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        customer.Id = sqlDataReader.GetInt32(0);
                        customer.Identification = sqlDataReader.GetString(1);
                        customer.Username = sqlDataReader.GetString(2);
                        customer.Name = sqlDataReader.GetString(3);
                        customer.MiddleName = sqlDataReader.GetString(4);
                        customer.LastName = sqlDataReader.GetString(5);
                        customer.Password = sqlDataReader.GetString(6);
                        customer.Email = sqlDataReader.GetString(7);
                        customer.Birthdate = sqlDataReader.GetDateTime(8);
                        customer.Status = sqlDataReader.GetString(9);
                        customer.RoleId = sqlDataReader.GetInt32(10);

                        var token =
                            TokenGenerator.GenerateTokenJwt(customer.Identification);
                        customer.Token = token;
                    }

                    sqlConnection.Close();

                    if (!string.IsNullOrEmpty(customer.Token))
                        return Ok(customer);
                    else
                        return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

    }
}