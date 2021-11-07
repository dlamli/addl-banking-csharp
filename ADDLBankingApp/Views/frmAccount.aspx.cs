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
    public partial class frmAccount : System.Web.UI.Page
    {
        IEnumerable<Account> accounts = new ObservableCollection<Account>();
        AccountManager accountManager = new AccountManager();


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

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (var cmd = new SqlCommand("getAllCurrencies", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlCurrency.DataSource = cmd.ExecuteReader();
                            ddlCurrency.DataTextField = "Description";
                            ddlCurrency.DataValueField = "Id";
                            ddlCurrency.DataBind();
                        }

                    }

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("getAllCard", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlCard.DataSource = cmd.ExecuteReader();
                            ddlCard.DataTextField = "CardNumber";
                            ddlCard.DataValueField = "Id";
                            ddlCard.DataBind();
                        }
                    }
                    

                }

            }
        }

        public async void init()
        {
            try
            {
                accounts = await accountManager.GetAllAccount(Session["Token"].ToString());
                gvAccount.DataSource = accounts.ToList();
                gvAccount.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load account list.";
                lblStatus.Visible = true;
            }
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Account account = new Account()
                {
                    UserId = Convert.ToInt32(ddlUser.SelectedValue),
                    CurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue),
                    CardId = Convert.ToInt32(ddlCard.SelectedValue),
                    Description = txtDescription.Text,
                    IBAN = txtIban.Text,
                    Balance = Convert.ToInt32(txtBalance.Text),
                    Status = ddlStatus.SelectedValue,
                    PhoneNumber = txtPhoneNumber.Text
                };

                Account accountInserted = await accountManager.insertAccount(account, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(accountInserted.Description))
                {
                    lblResult.Text = "Account created";
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
            else // Edit
            {
                Account account = new Account()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    UserId =Convert.ToInt32(ddlUser.SelectedValue),
                    CurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue),
                    CardId = Convert.ToInt32(ddlCard.SelectedValue),
                    Description = txtDescription.Text,
                    IBAN = txtIban.Text,
                    Balance = Convert.ToInt32(txtBalance.Text),
                    Status = ddlStatus.SelectedValue,
                    PhoneNumber = txtPhoneNumber.Text
                };

                Account accountUpdated = await accountManager.updateAccount(account, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(accountUpdated.Description))
                {
                    lblResult.Text = "Account updated";
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

        protected void btnCancelManagement_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseManagement(); });", true);
        }

        protected async void btnConfirmModal_Click(object sender, EventArgs e)
        {
            try
            {
                Account account = await accountManager.deleteAccount(lblIdRemove.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(account.Description))
                {
                    ltrModalMsg.Text = "Account deleted";
                    btnConfirmModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
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
                    Page = "frmAccount.aspx",
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
            ltrTitleManagement.Text = "New Account";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ddlUser.Enabled = true;
            ddlCurrency.Enabled = true;
            ddlCard.Enabled = true;
            ltrDescription.Visible = true;
            txtDescription.Visible = true;
            ddlStatus.Enabled = true;
            ddlCurrency.Enabled = true;
            txtIdManagement.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAccount.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editAccount":
                    ltrTitleManagement.Text = "Edit Account";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtDescription.Text = row.Cells[4].Text.Trim();
                    txtIban.Text = row.Cells[5].Text.Trim();
                    txtBalance.Text = row.Cells[6].Text.Trim();
                    txtPhoneNumber.Text = row.Cells[7].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeAccount":
                    lblIdRemove.Text = row.Cells[0].Text;
                    ltrModalMsg.Text = "Are you sure you want to delete account #" + lblIdRemove.Text + "?";

                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}