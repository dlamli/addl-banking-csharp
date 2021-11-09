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
    public partial class frmSinpe : System.Web.UI.Page
    {
        IEnumerable<SinpeM> sinpes = new ObservableCollection<SinpeM>();
        SinpeManager sinpeManager = new SinpeManager();
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
                sinpes = await sinpeManager.GetAllSinpeM(Session["Token"].ToString());
                gvSinpe.DataSource = sinpes.ToList();
                gvSinpe.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load sinpe list.";
                lblStatus.Visible = true;
            }
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                SinpeM sinpe = new SinpeM()
                {
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    Amount = Convert.ToDecimal(txtAmount.Text),
                    AccountTarget = txtAccountTarget.Text,
                    TransactionDate = DateTime.Now
                };

                SinpeM sinpeInserted = await sinpeManager.insertSinpeM(sinpe, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(sinpeInserted.AccountTarget) && !sinpeInserted.Amount.Equals(0))
                {
                    renderModalMessage("Sinpe created");
                    btnConfirmManagement.Visible = false;
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
                SinpeM sinpe = new SinpeM()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    AccountTarget = txtAccountTarget.Text,
                    Amount = Convert.ToDecimal(txtAmount.Text),
                    TransactionDate = DateTime.Now
                };

                SinpeM sinpeUpdated = await sinpeManager.updateSinpeM(sinpe, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(sinpeUpdated.AccountTarget) && !sinpeUpdated.Amount.Equals(0))
                {
                    renderModalMessage("Sinpe Updated");
                    btnConfirmManagement.Visible = false;
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
                SinpeM sinpe = await sinpeManager.deleteSinpeM(lblIdRemove.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(sinpe.AccountTarget))
                {
                    renderModalMessage("Sinpe deleted");
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
                    Page = "frmSinpe.aspx",
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
            ltrTitleManagement.Text = "New Sinpe";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ddlAccount.Enabled = true;
            txtAmount.Visible = true;
            ltrAmount.Visible = true;
            txtAccountTarget.Visible = true;
            ltrAccountTarget.Visible = true;
            txtTransactionDate.Visible = false;
            ltrTransactionDate.Visible = false;
            txtIdManagement.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtAccountTarget.Text = string.Empty;
            txtTransactionDate.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvSinpe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSinpe.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editSinpe":
                    ltrTitleManagement.Text = "Edit Sinpe";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtAmount.Text = row.Cells[2].Text.Trim();
                    txtAccountTarget.Text = row.Cells[3].Text.Trim();
                    txtTransactionDate.Text = row.Cells[4].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeSinpe":
                    lblIdRemove.Text = row.Cells[0].Text.Trim();
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure want to remove this sinpe# " + lblIdRemove.Text + "?";
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