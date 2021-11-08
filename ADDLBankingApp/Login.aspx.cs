using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnSignIn_Click(object sender, EventArgs e)
        {
            try
            {

                if (Page.IsValid)
                {
                    LoginRequest loginRequest = new LoginRequest() { Username = txtUsername.Text, Password = txtPassword.Text };
                    CustomerManager customerManager = new CustomerManager();
                    Customer customer = new Customer();

                    customer = await customerManager.Authenticate(loginRequest);

                    if(customer != null)
                    {
                        Session["Id"] = customer.Id;
                        Session["Identification"] = customer.Identification;
                        Session["Name"] = customer.Name;
                        Session["Status"] = customer.Status;
                        Session["Token"] = customer.Token;

                        FormsAuthentication.RedirectFromLoginPage(customer.Username, false);
                        SessionManager sessionManager = new SessionManager();
                        Session session = new Session()
                        {
                            UserId = Convert.ToInt32(Session["Id"].ToString()),
                            DateStart = DateTime.Now,
                            DateExpiration = DateTime.Now,
                            Status = Convert.ToString(1)
                        };

                        await sessionManager.insertSession(session, Session["Token"].ToString());
                    }
                    else
                    {
                        lblStatus.Text = "Invalid Credentials";
                        lblStatus.Visible = true;
                    }

                }
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred to the application. Contact the administrator.";
                lblStatus.Visible = true;
            }

        }
    }
}