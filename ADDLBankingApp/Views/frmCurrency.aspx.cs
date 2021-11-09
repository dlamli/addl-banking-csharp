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

namespace ADDLBankingApp.Views
{

    public partial class frmCurrencyManager : System.Web.UI.Page
    {
        IEnumerable<Currency> currency = new ObservableCollection<Currency>();
        CurrencyManager currencyManager = new CurrencyManager();
        static string _id = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
            else init();
        }
        //
        //inicializer method that obtains all currency and adds it to table as datasource
        public async void init()
        {
            try
            {
                currency = await currencyManager.GetAllCurrency(Session["Token"].ToString());
                gvCurrency.DataSource = currency.ToList();
                gvCurrency.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load account list.";
                lblStatus.Visible = true;
            }
        }
        //
        //Button to add a new currency
        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Currency";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            txtIdManagement.Text = string.Empty;
            txtDescription.Text = string.Empty;
            btnConfirmManagement.Visible = true;
            txtDescription.Visible = true;
            ltrDescription.Visible = true;
            txtDescription.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }
        //
        //Cancel the management window
        protected void btnCancelManagement_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseManagement(); });", true);
        }
        //
        //Confirm the management insert or change
        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Currency currency = new Currency()
                {
                    Description = txtDescription.Text,
                    Status = ddlStatusManagement.SelectedValue
                };

                Currency currencyInserted = await currencyManager.insertCurrency(currency, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(currencyInserted.Description))
                {
                    renderModalMessage("Currency Created");
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
                Currency currency = new Currency()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    Description = txtDescription.Text,
                    Status = ddlStatusManagement.SelectedValue
                };

                Currency currencyUpdated = await currencyManager.updateCurrency(currency, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(currencyUpdated.Description))
                {
                    renderModalMessage("Currency Updated");
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
        protected async void btnConfirmModal_Click(object sender, EventArgs e)
        {
            try
            {
                Currency currency = await currencyManager.deleteCurrency(lblRemoveCode.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(currency.Description))
                {
                    renderModalMessage("Currency Deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Currency table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmCurrency.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Currency",
                    Number = 547,
                    Description = "Currency table error with foreign key."
                };
                await errorManager.insertErrorLog(error);
            }
        }

        protected void btnCancelModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void gvCurrency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCurrency.Rows[index];

            switch (e.CommandName)
            {
                case "editCurrency":
                    ltrIdManagement.Text = "Edit Currency";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtDescription.Text = row.Cells[1].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
                    break;
                case "removeCurrency":
                    lblRemoveCode.Text = row.Cells[0].Text;
                    lblRemoveCode.Visible = false;
                    ltrModalMessage.Text = "Are you sure you want to delete currency #" + lblRemoveCode.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() { openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        public void renderModalMessage(string text)
        {
            ltrCurrencyMessage.Text = text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMsg(); } );", true);
        }

        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }
    }
}