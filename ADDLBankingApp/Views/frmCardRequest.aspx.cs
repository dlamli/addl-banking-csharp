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
using System.Text;

namespace ADDLBankingApp.Views
{
    public partial class CardRequest : System.Web.UI.Page
    {

        IEnumerable<Models.CardRequest> cardRequests = new ObservableCollection<Models.CardRequest>();
        CardRequestManager cardRequestManager = new CardRequestManager();
        static string _id = string.Empty;

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
                    cardRequests = await cardRequestManager.GetAllCardRequest(Session["Token"].ToString());
                    init();
                    GetDataGraphic();
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
                gvCardRequest.DataSource = cardRequests.ToList();
                gvCardRequest.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load card request list.";
                lblStatus.Visible = true;
            }
        }

        private void GetDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var cardRequest in cardRequests.GroupBy(e => e.RequestDate)
                  .Select(group => new
                  {
                      Provider = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.Provider))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", cardRequest.Provider);
                data.AppendFormat("'{0}',", cardRequest.Quantity);
                backgroundColor.AppendFormat("'{0}',", color);

                lblGraphic = labels.ToString().Substring(0, labels.Length - 1);
                dataGraphic = data.ToString().Substring(0, data.Length - 1);
                bgColorGraphic = backgroundColor.ToString().Substring(0, backgroundColor.Length - 1);
            }
        }

        protected void gvCardRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCardRequest.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editCardRequest":
                    ltrTitleManagement.Text = "Edit Card Request";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtRequestDate.Enabled = true;
                    txtRequestDate.ReadOnly = false;
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtRequestDate.Text = row.Cells[2].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeCardRequest":
                    _id = row.Cells[0].Text.Trim();
                    ltrModalMsg.Text = "Are you sure want to remove card request #" + _id + " ?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {

            ltrTitleManagement.Text = "New Card Request";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            txtIdManagement.Text = string.Empty;
            txtIdManagement.Visible = true;
            txtRequestDate.Visible = true;
            txtRequestDate.Text = DateTime.Now.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Models.CardRequest cardRequest = new Models.CardRequest()
                {
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    RequestDate = DateTime.Now
                };

                Models.CardRequest cardRequestInserted = await cardRequestManager.insertCardRequest(cardRequest, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(cardRequestInserted.AccountId.ToString()))
                {
                    renderModalMessage("Card request created");
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
                Models.CardRequest cardRequest = new Models.CardRequest()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    RequestDate = Convert.ToDateTime(txtRequestDate.Text)
                };

                Models.CardRequest cardRequestUpdated = await cardRequestManager.updateCardRequest(cardRequest, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(cardRequestUpdated.AccountId.ToString()))
                {
                    renderModalMessage("Card Request updated");
                    init();
                    txtRequestDate.Enabled = false;
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
                Models.CardRequest cardRequest = await cardRequestManager.deleteCardRequest(_id, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(cardRequest.AccountId.ToString()))
                {
                    renderModalMessage("Card Request deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("CardRequest table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmCardRequest.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "CardRequest",
                    Number = 547,
                    Description = "CardRequest table error with foreign key."
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