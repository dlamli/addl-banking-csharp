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
    public partial class frmUtility : System.Web.UI.Page
    {
        IEnumerable<Utility> utilities = new ObservableCollection<Utility>();
        UtilityManager utilityManager = new UtilityManager();
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

                        using (SqlCommand cmd = new SqlCommand("SELECT [Id], [IBAN] FROM [Account]"))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            conn.Open();
                            ddlAccount.DataSource = cmd.ExecuteReader();
                            ddlAccount.DataTextField = "IBAN";
                            ddlAccount.DataValueField = "Id";
                            ddlAccount.DataBind();
                        }
                    }
                }
            }
        }

        public async void init()
        {
            try
            {
                utilities = await utilityManager.GetAllUtility(Session["Token"].ToString());
                gvUtility.DataSource = utilities.ToList();
                gvUtility.DataBind();
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
                Utility utility = new Utility()
                {
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    ProfitPercentage = Convert.ToInt32(txtProfitPercentage.Text)

                };

                Utility utilityInserted = await utilityManager.insertUtility(utility, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(utilityInserted.AccountId.ToString()))
                {
                    renderModalMessage("Utility created");
                    init();
                }
                else
                {
                    lblResult.Text = "An error ocurred to do this action.";
                    lblResult.Visible = true;
                    lblResult.ForeColor = Color.Red;
                }
            }
            else // Edit
            {
                Utility utility = new Utility()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    ProfitPercentage = Convert.ToDecimal(txtProfitPercentage.Text)
                };

                Utility utilityUpdated = await utilityManager.updateUtility(utility, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(utilityUpdated.AccountId.ToString()))
                {
                    renderModalMessage("Utility updated");
                    init();
                }
                else
                {
                    lblResult.Text = "An error ocurred to do this action.";
                    lblResult.Visible = true;
                    lblResult.ForeColor = Color.Red;
                }
            }
        }

        protected void btnCancelManagement_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseManagement(); });", true);
        }

        protected async void btnConfirmModal_Click(object sender, EventArgs e)
        {
            try
            {
                Utility utility = await utilityManager.deleteUtility(_id, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(utility.AccountId.ToString()))
                {
                    renderModalMessage("Utility deleted");
                    init();
                }
            }
            catch (Exception ex)
            {
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmUtility.aspx",
                    Action = "btnAceptarModal_Click",
                    Source = ex.Source,
                    Number = ex.HResult,
                    Description = ex.Message
                };
                await errorManager.insertErrorLog(error);
            }
        }

        protected void btnCancelModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Utility";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            txtProfitPercentage.Visible = true;
            ltrProfitPercentage.Visible = true;
            txtIdManagement.Text = string.Empty;
            txtProfitPercentage.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvUtility_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvUtility.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editUtility":
                    ltrTitleManagement.Text = "Edit Utility";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtProfitPercentage.Text = row.Cells[2].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeUtility":
                    _id = row.Cells[0].Text.Trim();
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure want to remove this utility#"+ _id+"?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        public void renderModalMessage(string text)
        {
            ltrModalMessage.Text = text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMsg(); } );", true);
        }

        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }
    }
}