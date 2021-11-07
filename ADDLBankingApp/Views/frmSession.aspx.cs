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
        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Session session = new Session()
                {
                    UserId = Convert.ToInt32(ddlUser.SelectedValue),
                    DateStart = DateTime.Now,
                    DateExpiration = DateTime.Now,
                    Status = ddlStatus.SelectedValue

                };

                var dateCompare = DateTime.Compare(Convert.ToDateTime(txtDateStart.Text),Convert.ToDateTime( txtDateExpiration.Text));
                if (dateCompare > 0) lblDatemsg.Text = "Date start must be earlier than Expiration date";
                else
                {
                    Session sessionInserted = await sessionManager.insertSession(session, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(sessionInserted.UserId.ToString()))
                    {
                        lblResult.Text = "Session created";
                        lblResult.Visible = true;
                        lblResult.ForeColor = Color.Green;
                        btnConfirmManagement.Visible = false;
                        init();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    }
                    else
                    {
                        lblResult.Text = "An error ocurred to do this action.";
                        lblResult.Visible = true;
                        lblResult.ForeColor = Color.Red;
                    }
                }
            }
        }

        protected void btnCancelManagement_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseManagement(); });", true);
        }



        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Session";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ltrUserId.Visible = true;
            txtDateStart.Visible = true;
            ltrDateStart.Visible = true;
            txtDateExpiration.Visible = true;
            ltrDateExpiration.Visible = true;
            ltrStatus.Visible = true;
            txtIdManagement.Text = string.Empty;
            txtDateStart.Text = string.Empty;
            txtDateExpiration.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }


    }
}