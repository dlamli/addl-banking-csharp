using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp.Views
{
    public partial class frmAccount : System.Web.UI.Page
    {
        IEnumerable<Account> accounts = new ObservableCollection<Account>();
        AccountManager accountManager = new AccountManager();
        string connString = ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString;

        public string lblGraphic = string.Empty;
        public string bgColorGraphic = string.Empty;
        public string dataGraphic = string.Empty;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else
                {
                    accounts = await accountManager.GetAllAccount(Session["Token"].ToString());
                    init();
                    getDataGraphic();

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


        private void clearFields()
        {
            txtIdManagement.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtBalance.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
        }
        public async void init()
        {
            try
            {
                gvAccount.DataSource = accounts.ToList();
                gvAccount.DataBind();
            }
            catch (Exception ex)
            {

                lblStatus.Text = "An error ocurred  to load account list.";
                lblStatus.Visible = true;
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Source = ex.Source,
                    Number = ex.HResult,
                    Description = ex.Message,
                    Page = "frmAccount.aspx",
                    Action = "init",
                };
                await errorManager.insertErrorLog(error);
            }
        }

        private void getDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var account in accounts.GroupBy(e => e.Status)
                  .Select(group => new
                  {
                      Status = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.Status))
            {

                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                data.AppendFormat("'{0}',", account.Quantity);
                backgroundColor.AppendFormat("'{0}',", color);

                //labels.AppendFormat("'{0}',", account.Status);
                if (account.Status.Equals("0"))
                {
                    labels.AppendFormat("'{0}',", "Inactive");
                }
                else
                {
                    labels.AppendFormat("'{0}',", "Active");
                }

                lblGraphic = labels.ToString().Substring(0, labels.Length - 1);
                dataGraphic = data.ToString().Substring(0, data.Length - 1);
                bgColorGraphic = backgroundColor.ToString().Substring(0, backgroundColor.Length - 1);
            }
        }

        public bool checkExistAccount(string IBAN, int cardId)
        {
            bool accountExist = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("CheckAccountCreation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IBAN", IBAN.Trim());
                    cmd.Parameters.AddWithValue("@cardId", cardId);
                    conn.Open();
                    accountExist = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return accountExist;

        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
                {
                    if (checkExistAccount(txtIban.Text, Convert.ToInt32(ddlCard.SelectedValue)))
                    {
                        Account account = new Account()
                        {
                            UserId = Convert.ToInt32(ddlUser.SelectedValue),
                            CurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue),
                            CardId = Convert.ToInt32(ddlCard.SelectedValue),
                            Description = txtDescription.Text,
                            IBAN = txtIban.Text,
                            Balance = Convert.ToDecimal(txtBalance.Text),
                            Status = ddlStatus.SelectedValue,
                            PhoneNumber = txtPhoneNumber.Text
                        };

                        Account accountInserted = await accountManager.insertAccount(account, Session["Token"].ToString());

                        if (!string.IsNullOrEmpty(accountInserted.Description))
                        {
                            renderModalMessage("Account created");
                            init();

                        }
                        else
                        {
                            lblResult.Text = "An error ocurred to do this action.";
                            lblResult.Visible = true;
                            lblResult.ForeColor = Color.Red;
                        }
                    }
                    else // Account exist
                    {
                        renderModalMessage("Account already exist");
                    }
                }
                else // Edit
                {
                    Account account = new Account()
                    {
                        Id = Convert.ToInt32(txtIdManagement.Text),
                        UserId = Convert.ToInt32(ddlUser.SelectedValue),
                        CurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue),
                        CardId = Convert.ToInt32(ddlCard.SelectedValue),
                        Description = txtDescription.Text,
                        IBAN = txtIban.Text,
                        Balance = Convert.ToDecimal(txtBalance.Text),
                        Status = ddlStatus.SelectedValue,
                        PhoneNumber = txtPhoneNumber.Text
                    };

                    Account accountUpdated = await accountManager.updateAccount(account, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(accountUpdated.Description))
                    {

                        renderModalMessage("Account Updated");
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
            catch (Exception)
            {
                renderModalMessage("Missing fields. Compelte them please");
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
                    renderModalMessage("Account deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Account table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmAccount.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Account",
                    Number = 547,
                    Description = "Account table error with foreign key."
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
            clearFields();
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
            txtIban.Text = generateIBAN();
            txtIban.Enabled = false;
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
                    txtIban.Enabled = false;
                    ddlCard.SelectedValue = row.Cells[3].Text.Trim();
                    ddlUser.SelectedValue = row.Cells[1].Text.Trim();
                    ddlCurrency.SelectedValue = row.Cells[2].Text.Trim();
                    ddlCard.Enabled = false;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeAccount":
                    lblIdRemove.Text = row.Cells[0].Text;
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure you want to delete account #" + lblIdRemove.Text + "?";
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
            clearFields();
        }

        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }

        private string generateIBAN()
        {
            string IBAN = "";
            Random rnd = new Random();

            for (int i = 0; i < 22; i++)
            {
                IBAN += rnd.Next(0, 9);
            }

            return IBAN;
        }

    }
}