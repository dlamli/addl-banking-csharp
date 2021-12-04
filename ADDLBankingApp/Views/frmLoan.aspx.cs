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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp.Views
{
    public partial class frmLoan : System.Web.UI.Page
    {
        IEnumerable<Loan> loan = new ObservableCollection<Loan>();
        LoanManager loanManager = new LoanManager();

        public string graphLabels = string.Empty;
        public string graphBackgroundColors = string.Empty;
        public string graphData = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else
                {
                    init();
                    createGraph();
                    string connString = ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString;

                    //
                    //Obtiene los usuarios 
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (var cmd = new SqlCommand("getAccount", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlAccount.DataSource = cmd.ExecuteReader();
                            ddlAccount.DataTextField = "IBAN";
                            ddlAccount.DataValueField = "Id";
                            ddlAccount.DataBind();
                        }
                    }
                }
            }
        }

        //
        //inicializer method that obtains all currency and adds it to table as datasource
        public async void init()
        {
            try
            {
                loan = await loanManager.GetAllLoan(Session["Token"].ToString());
                gvLoan.DataSource = loan.ToList();
                gvLoan.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred to load loan list.";
                lblStatus.Visible = true;
            }
        }
        //
        //Creates the graph
        private async void createGraph()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColors = new StringBuilder();

            var random = new Random();

            loan = await loanManager.GetAllLoan(Session["Token"].ToString());

            foreach (var loans in loan.GroupBy(t => t.Type).Select(v => v.First()).Distinct())
            {
                labels.Append(String.Format("'{0}',", loans.Type));
                data.Append(String.Format("'{0}',", loan.Where(v => v.Type == loans.Type).Count()));
                backgroundColors.Append(String.Format("'{0}',", String.Format("#{0:X6}", random.Next(0x1000000))));

                graphLabels = labels.ToString().Substring(0, labels.Length - 1);
                graphData = data.ToString().Substring(0, data.Length - 1);
                graphBackgroundColors = backgroundColors.ToString().Substring(0, backgroundColors.Length - 1);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Loan";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            txtIdManagement.Text=string.Empty;
            txtAmount.Text = string.Empty;
            txtType.Text = string.Empty;
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ltrType.Visible = true;
            ltrAmount.Visible = true;
            txtAmount.Visible = true;
            ddlAccount.Enabled = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Loan loan = new Loan()
                {
                    Type = txtType.Text,
                    Amount = Convert.ToInt32(txtAmount.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue)
                };

                Loan loanInserted = await loanManager.insertLoan(loan, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(loanInserted.Id.ToString()))
                {
                    renderModalMessage("Loan Created");
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
                Loan loan = new Loan()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    Type = txtType.Text,
                    Amount = Convert.ToInt32(txtAmount.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue)
                };

                Loan loanUpdated = await loanManager.updateLoan(loan, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(loanUpdated.Id.ToString()))
                {
                    renderModalMessage("Loan Updated");
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
                Loan loan = await loanManager.deleteLoan(lblRemoveCode.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(loan.Id.ToString()))
                {
                    renderModalMessage("Loan Deleted");
                    init();
                }
            }
            catch (Exception ex)
            {
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId =
                        Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmLoan.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = ex.Source,
                    Number = ex.HResult,
                    Description = ex.Message
                };
                ErrorLog errorIngresado = await errorManager.insertErrorLog(error);
            }
        }

        protected void btnCancelModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void gvLoan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvLoan.Rows[index];

            switch (e.CommandName)
            {
                case "editLoan":
                    ltrIdManagement.Text = "Update Loan";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtType.Text = row.Cells[1].Text.Trim();
                    txtAmount.Text = row.Cells[2].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
                    break;
                case "removeLoan":
                    lblRemoveCode.Text = row.Cells[0].Text;
                    lblRemoveCode.Visible = false;
                    ltrModalMessage.Text = "Are you sure you want to delete Loan #" + lblRemoveCode.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() { openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        public void renderModalMessage(string text)
        {
            ltrModalMessages.Text = text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMsg(); } );", true);
        }

        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }
    }
}