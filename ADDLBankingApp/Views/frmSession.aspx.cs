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
    public partial class frmSession : System.Web.UI.Page
    {
        IEnumerable<Session> sessions = new ObservableCollection<Session>();
        SessionManager sessionManager = new SessionManager();
        static string _id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else
                {
                    init();
                    string connString = ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString;

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        //using (SqlCommand cmd = new SqlCommand("SELECT [Id], [Name] FROM [Customer]"))
                        using (var cmd = new SqlCommand("getCustomer", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlUser.DataSource = cmd.ExecuteReader();
                            ddlUser.DataTextField = "Name";
                            ddlUser.DataValueField = "Id";
                            ddlUser.DataBind();
                        }
                    }
                }
            }
        }

        public async void init()
        {
            try
            {
                sessions = await sessionManager.GetAllSession(Session["Token"].ToString());
                gvSession.DataSource = sessions.ToList();
                gvSession.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load Utility list.";
                lblStatus.Visible = true;
            }
        }

        protected void gvSession_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            init();
            gvSession.PageIndex = e.NewPageIndex;
            gvSession.DataBind();
        }
    }
}