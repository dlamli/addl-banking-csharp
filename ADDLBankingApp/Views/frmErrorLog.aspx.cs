using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ADDLBankingApp.Views
{
    public partial class frmErrorLog : System.Web.UI.Page
    {
        IEnumerable<ErrorLog> errorLogs = new ObservableCollection<ErrorLog>();
        ErrorLogManager errorlogManager = new ErrorLogManager();
        static string _id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else init();
            }
        }

        public async void init()
        {
            try
            {
                errorLogs = await errorlogManager.GetAllErrorLog();
                gvErrorlog.DataSource = errorLogs.ToList();
                gvErrorlog.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load role list.";
                lblStatus.Visible = true;
            }
        }

        protected void gvErrorlog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            init();
            gvErrorlog.PageIndex = e.NewPageIndex;
            gvErrorlog.DataBind();
        }
    }
}