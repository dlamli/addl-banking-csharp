using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ADDLBankingApp.Views
{
    public partial class frmTransfer : System.Web.UI.Page
    {
        IEnumerable<Transfer> transfers = new ObservableCollection<Transfer>();
        TransferManager transferManager = new TransferManager();

        public string lblGraphic = string.Empty;
        public string bgColorGraphic = string.Empty;
        public string dataGraphic = string.Empty;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    transfers = await transferManager.GetAllTransfer(Session["Token"].ToString());
                    init();
                    getDataGraphic();
                    string connString = ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {

                        using (SqlCommand cmd = new SqlCommand("SELECT [Id], [IBAN] FROM [Account]"))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            conn.Open();
                            ddlAccountOrigin.DataSource = cmd.ExecuteReader();
                            ddlAccountOrigin.DataTextField = "IBAN";
                            ddlAccountOrigin.DataValueField = "Id";
                            ddlAccountOrigin.DataBind();
                        }
                    }
                }
            }
        }

        public void init()
        {
            try
            {
                gvTransfers.DataSource = transfers.ToList();
                gvTransfers.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load transfer list.";
                lblStatus.Visible = true;
            }
        }

        private void getDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var transfer in transfers.GroupBy(e => e.Date.Date)
                  .Select(group => new
                  {
                      Date = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.Date))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", transfer.Date.ToString("MM/dd/yyyy"));
                data.AppendFormat("'{0}',", transfer.Quantity);
                backgroundColor.AppendFormat("'{0}',", color);

                lblGraphic = labels.ToString().Substring(0, labels.Length - 1);
                dataGraphic = data.ToString().Substring(0, data.Length - 1);
                bgColorGraphic = backgroundColor.ToString().Substring(0, backgroundColor.Length - 1);
            }
        }


        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Transfer transfer = new Transfer()
                {
                    AccountOrigin = Convert.ToInt32(ddlAccountOrigin.SelectedValue),
                    AccountDestiny = Convert.ToInt32(txtAccountDestiny.Text),
                    Date = DateTime.Now,
                    Description = txtDescription.Text,
                    Amount = Convert.ToDecimal(txtAmount.Text),
                    Status = ddlStatus.SelectedValue

                };

                Transfer transferInserted = await transferManager.insertTransfer(transfer, Session["Token"].ToString());

                if(!string.IsNullOrEmpty(transferInserted.Description) && !transferInserted.Amount.Equals(0) &&
                    !transferInserted.AccountDestiny.Equals(0))
                {
                    renderModalMessage("Transfer created");
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
                Transfer transfer = new Transfer()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    AccountOrigin = Convert.ToInt32(ddlAccountOrigin.SelectedValue),
                    AccountDestiny = Convert.ToInt32(txtAccountDestiny.Text),
                    Date = DateTime.Now,
                    Description = txtDescription.Text,
                    Amount = Convert.ToInt32(txtAmount.Text),
                    Status = ddlStatus.SelectedValue

                };

                Transfer transferUpdated = await transferManager.updateTransfer(transfer, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(transferUpdated.Description) 
                    && !transferUpdated.Amount.Equals(0) 
                    && !transferUpdated.AccountDestiny.Equals(0))
                {
                    renderModalMessage("Transfer updated");
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
                Transfer transfer = await transferManager.deleteTransfer(lblIdRemove.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(transfer.Id.ToString()))
                {
                    renderModalMessage("Transfer deleted");
                    init();
                }
            }
            catch (Exception ex)
            {
                renderModalMessage("No se puede eliminar la cuenta, porque su información está activa");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmTransfer.aspx",
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
            ltrTitleManagement.Text = "New Transfer";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            txtDescription.Text = string.Empty;
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ddlAccountOrigin.Enabled = true;
            txtAccountDestiny.Visible = true;
            ltrAccountDestiny.Visible = true;
            txtDescription.Visible = true;
            ltrDescription.Visible = true;
            txtAmount.Visible = true;
            ltrAmount.Visible = true;
            ddlStatus.Enabled = true;
            txtIdManagement.Text = string.Empty;
            txtAmount.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvTransfers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTransfers.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editTransfers":
                    ltrTitleManagement.Text = "Edit Transfer";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtAccountDestiny.Text = row.Cells[2].Text.Trim();
                    txtDescription.Text = row.Cells[4].Text.Trim();
                    txtAmount.Text = row.Cells[5].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeTransfers":
                    lblIdRemove.Text = row.Cells[0].Text.Trim();
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure want to remove this transfer#"+lblIdRemove.Text+" ?";
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