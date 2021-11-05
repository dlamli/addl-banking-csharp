using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp.CustomErrors
{
    public partial class frmError : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            Exception err = Session["LastError"] as Exception;

            if(err != null)
            {
                err = err.GetBaseException(); 
                lblError.Text = err.Message;
                Session["LastError"] = null;

                ErrorLogManager errorLogManager = new ErrorLogManager();
                ErrorLog errorApi = new ErrorLog()
                {
                    UserId = 0,
                    Date = DateTime.Now,
                    Page = "frmError.aspx",
                    Action = "Page_load",
                    Source = err.Source,
                    Number = err.HResult,
                    Description = err.Message
                };

                ErrorLog errorInserted = await errorLogManager.insertErrorLog(errorApi);
            }
        }
    }
}