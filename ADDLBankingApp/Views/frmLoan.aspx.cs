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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp.Views
{
    public partial class frmLoan : System.Web.UI.Page
    {
        IEnumerable<Loan> loan = new ObservableCollection<Loan>();
        LoanManager loanManager = new LoanManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else
                {
                    init();

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

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Loan";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
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
                    lblResult.Text = "Loan created";
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
                    lblResult.Text = "Loan updated";
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
                Loan loan = await loanManager.deleteLoan(lblRemoveCode.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(loan.Id.ToString()))
                {
                    lblRemoveCode.Text = string.Empty;
                    ltrModalMessage.Text = "Loan deleted";
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
                    UserId =
                        Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmServicio.aspx",
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
                    ltrModalMessage.Text = "Are you sure you want to delete Loan #" + lblRemoveCode.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() { openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}