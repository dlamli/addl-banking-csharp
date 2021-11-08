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
using System.Globalization;

namespace ADDLBankingApp.Views
{
    public partial class frmTimeDeposit : System.Web.UI.Page
    {
        CultureInfo cultures = new CultureInfo("en-US");
        IEnumerable<TimeDeposit> timeDeposit = new ObservableCollection<TimeDeposit>();
        TimeDepositManager timeDepositManager = new TimeDepositManager();

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

        private void clearFields()
        {
            txtIdManagement.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            txtPercentage.Text = string.Empty;
        }

        public async void init()
        {
            try
            {
                timeDeposit = await timeDepositManager.GetAllTimeDeposit(Session["Token"].ToString());
                gvTimeDeposit.DataSource = timeDeposit.ToList();
                gvTimeDeposit.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load card request list.";
                lblStatus.Visible = true;
            }
        }

        protected void gvTimeDeposit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTimeDeposit.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editTimeDeposit":
                    ltrTitleManagement.Text = "Edit Time Deposit Request";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtAmount.Text = row.Cells[2].Text.Trim();
                    txtStartDate.Text = row.Cells[3].Text.Trim();
                    txtEndDate.Text = row.Cells[4].Text.Trim();
                    txtPercentage.Text = row.Cells[5].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeTimeDeposit":
                    lblIdRemove.Text = row.Cells[0].Text.Trim();
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure want to remove Time Deposit#"+ lblIdRemove.Text + " ?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Time Deposit";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            clearFields();
            btnConfirmManagement.Visible = true;
            txtIdManagement.Visible = true;
            txtPercentage.Visible = true;
            txtStartDate.Visible = true;
            txtEndDate.Visible = true;
            txtStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd").ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            var dateCompare = DateTime.Compare(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                TimeDeposit timeDeposit = new TimeDeposit()
                {
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    Amount = Convert.ToDecimal(txtAmount.Text),
                    StartDate = Convert.ToDateTime(txtStartDate.Text, cultures),
                    EndDate = Convert.ToDateTime(txtEndDate.Text),
                    Percentage = Convert.ToDecimal(txtPercentage.Text)
                };
                

                if (dateCompare > 0) renderModalMessage("Date start must be earlier than Expiration date");
                else
                {

                    TimeDeposit timeDepositInserted = await timeDepositManager.insertTimeDeposit(timeDeposit, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(timeDepositInserted.AccountId.ToString()))
                    {
                        renderModalMessage("Time Deposit created");
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
            else // Edit
            {
                if (dateCompare > 0) renderModalMessage("Date start must be earlier than Expiration date");
                else
                {
                    TimeDeposit timeDeposit = new TimeDeposit()
                    {
                        Id = Convert.ToInt32(txtIdManagement.Text),
                        AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                        Amount = Convert.ToDecimal(txtAmount.Text),
                        StartDate = Convert.ToDateTime(txtStartDate.Text),
                        EndDate = Convert.ToDateTime(txtEndDate.Text),
                        Percentage = Convert.ToDecimal(txtPercentage.Text)
                    };

                    TimeDeposit timeDepositUpdate = await timeDepositManager.updateTimeDeposit(timeDeposit, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(timeDepositUpdate.AccountId.ToString()))
                    {
                        renderModalMessage("Time Deposit updated");
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
        }

        protected void btnCancelManagement_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseManagement(); });", true);
        }

        protected async void btnConfirmModal_Click(object sender, EventArgs e)
        {
            try
            {
                TimeDeposit timeDeposit = await timeDepositManager.deleteTimeDeposit(lblIdRemove.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(timeDeposit.AccountId.ToString()))
                {
                    renderModalMessage("Time deposit deleted");
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
                    Page = "frmTimeDeposit.aspx",
                    Action = "btnConfirmModal_Click",
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