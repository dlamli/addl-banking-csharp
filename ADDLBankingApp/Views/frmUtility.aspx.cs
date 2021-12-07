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
    public partial class frmUtility : System.Web.UI.Page
    {
        IEnumerable<Utility> utilities = new ObservableCollection<Utility>();
        UtilityManager utilityManager = new UtilityManager();
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
                    utilities = await utilityManager.GetAllUtility(Session["Token"].ToString());

                    init();
                    getDataGraphic();

                    

                }
            }
        }

        public  void init()
        {
            try
            {
                
                gvUtility.DataSource = utilities.ToList();
                gvUtility.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load Utility list.";
                lblStatus.Visible = true;
            }
        }

        private void getDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var utility in utilities.GroupBy(e => e.ProfitPercentage)
                  .Select(group => new
                  {
                      ProfitPercentage = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.ProfitPercentage))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", utility.ProfitPercentage);
                data.AppendFormat("'{0}',", utility.Quantity);
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