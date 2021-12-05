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
    public partial class frmPayment : System.Web.UI.Page
    {
        IEnumerable<Payment> payment = new ObservableCollection<Payment>();
        PaymentManager paymentManager = new PaymentManager();

        public string graphLabels = string.Empty;
        public string graphBackgroundColors = string.Empty;
        public string graphData = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    init();
                    createGraph();

                    string connString = ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString;

                    //
                    //Obtiene los servicios 
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (var cmd = new SqlCommand("getServices", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlService.DataSource = cmd.ExecuteReader();
                            ddlService.DataTextField = "Description";
                            ddlService.DataValueField = "Id";
                            ddlService.DataBind();
                        }
                    }
                    //
                    //Obtiene los usuarios 
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

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
                }
            }
        }
        //
        //inicializer method that obtains all currency and adds it to table as datasource
        public async void init()
        {
            try
            {
                payment = await paymentManager.GetAllPayment(Session["Token"].ToString());
                gvPayment.DataSource = payment.ToList();
                gvPayment.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred to load payment list.";
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

            payment = await paymentManager.GetAllPayment(Session["Token"].ToString());

            foreach (var payments in payment.GroupBy(d => d.Date.ToShortDateString()).Select(v => v.First()).OrderBy(d => d.Date).Distinct())
            {
                labels.Append(String.Format("'{0}',", payments.Date.ToShortDateString()));
                data.Append(String.Format("'{0}',", payment.Where(v => v.Date.ToShortDateString() == payments.Date.ToShortDateString()).Count()));
                backgroundColors.Append(String.Format("'{0}',", String.Format("#{0:X6}", random.Next(0x1000000))));

                graphLabels = labels.ToString().Substring(0, labels.Length - 1);
                graphData = data.ToString().Substring(0, data.Length - 1);
                graphBackgroundColors = backgroundColors.ToString().Substring(0, backgroundColors.Length - 1);
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Payment";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            txtIdManagement.Text = string.Empty;
            txtDate.Text = string.Empty;
            txtAmount.Text = string.Empty;           
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ltrServiceId.Visible = true;
            ddlService.Enabled = true;
            ltrUserId.Visible = true;
            ddlUser.Enabled = true;
            ltrDate.Visible = true;
            txtDate.Visible = true;
            txtDate.Text = DateTime.Now.ToString();
            ltrAmount.Visible = true;
            txtAmount.Visible = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Payment payment = new Payment()
                {
                    ServiceId = Convert.ToInt32(ddlService.SelectedValue),
                    UserId = Convert.ToInt32(ddlUser.SelectedValue),
                    Date = Convert.ToDateTime(txtDate.Text),
                    Mount = Convert.ToDecimal(txtAmount.Text)
                };

                Payment paymentInserted = await paymentManager.insertPayment(payment, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(paymentInserted.Id.ToString()))
                {
                    renderModalMessage("Payment Created");
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
                Payment payment = new Payment()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    ServiceId = Convert.ToInt32(ddlService.SelectedValue),
                    UserId = Convert.ToInt32(ddlUser.SelectedValue),
                    Date = Convert.ToDateTime(txtDate.Text),
                    Mount = Convert.ToDecimal(txtAmount.Text)
                };

                Payment paymentUpdated = await paymentManager.updatePayment(payment, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(paymentUpdated.Id.ToString()))
                {
                    renderModalMessage("Payment Updated");
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
                Payment payment = await paymentManager.deletePayment(lblRemoveCode.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(payment.Id.ToString()))
                {
                    renderModalMessage("Payment Deleted");
                    init();
                }
            }
            catch (Exception ex)
            {
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId =
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()),
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

        protected void gvPayment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPayment.Rows[index];

            switch (e.CommandName)
            {
                case "editPayment":
                    ltrIdManagement.Text = "Update Payment";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtDate.Text = DateTime.Now.ToString();
                    txtAmount.Text = row.Cells[4].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
                    break;
                case "removePayment":
                    lblRemoveCode.Text = row.Cells[0].Text;
                    lblRemoveCode.Visible = false;
                    ltrModalMessage.Text = "Are you sure you want to delete payment #" + lblRemoveCode.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() { openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        public void renderModalMessage(string text)
        {
            ltrPaymentMessage.Text = text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMsg(); } );", true);
        }


        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }
    }
}