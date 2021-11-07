using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using ADDLBankingApp.Models;
using ADDLBankingApp.Managers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace ADDLBankingApp.Views
{
    public partial class frmCustomerManager : System.Web.UI.Page
    {
        IEnumerable<Customer> customer = new ObservableCollection<Customer>();
        CustomerManager customerManager = new CustomerManager();


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

                        using (SqlCommand cmd = new SqlCommand("SELECT [Id], [Name] FROM [Role]"))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            conn.Open();
                            ddlRole.DataSource = cmd.ExecuteReader();
                            ddlRole.DataTextField = "Name";
                            ddlRole.DataValueField = "Id";
                            ddlRole.DataBind();
                        }
                    }
                }
            }
        }


        public async void init()
        {
            try
            {
                customer = await customerManager.GetAllCustomer(Session["Token"].ToString());
                gvCustomer.DataSource = customer.ToList();
                gvCustomer.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load customer list.";
                lblStatus.Visible = true;
            }
        }

        protected void gvCard_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Customer";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ltrUsername.Visible = true;
            txtUsername.Visible = true;
            ltrName.Visible = true;
            txtName.Visible = true;
            ltrMiddleName.Visible = true;
            txtMiddleName.Visible = true;
            ltrLastName.Visible = true;
            txtLastName.Visible = true;
            ltrPassword.Visible = true;
            txtPassword.Visible = true;
            ltrEmail.Visible = true;
            txtEmail.Visible = true;
            ltrBirthdate.Visible = true;
            txtBirthDate.Visible = true;
            ddlStatus.Enabled = true;
            ddlRole.Enabled = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Customer customer = new Customer()
                {
                    Identification = txtIdentification.Text,
                    Username = txtUsername.Text,
                    Name = txtName.Text,
                    MiddleName = txtMiddleName.Text,
                    LastName = txtLastName.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    Birthdate = Convert.ToDateTime(txtBirthDate.Text),
                    Status = ddlStatus.SelectedValue,
                    RoleId = Convert.ToInt32(ddlRole.SelectedValue)
                };

                Customer customerInserted = await customerManager.Register(customer);

                if (!string.IsNullOrEmpty(customerInserted.Name))
                {
                    lblResult.Text = "Customer created";
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
                Customer customer = new Customer()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    Identification = txtIdentification.Text,
                    Username = txtUsername.Text,
                    Name = txtName.Text,
                    MiddleName = txtMiddleName.Text,
                    LastName = txtLastName.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    Birthdate = Convert.ToDateTime(txtBirthDate.Text),
                    Status = ddlStatus.SelectedValue,
                    RoleId = Convert.ToInt32(ddlRole.SelectedValue)
                };

                Customer customerUpdated = await customerManager.updateCustomer(customer, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(customerUpdated.Name))
                {
                    lblResult.Text = "Customer updated";
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
                Customer customerDeleted = await customerManager.deleteCustomer(lblIdRemove.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(customerDeleted.Name))
                {
                    ltrModalMsg.Text = "Customer deleted";
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
                    Page = "frmCustomer.aspx",
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

        protected void gvCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCustomer.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editCustomer":
                    ltrTitleManagement.Text = "Edit Customer";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtIdentification.Text = row.Cells[1].Text.Trim();
                    txtUsername.Text = row.Cells[2].Text.Trim();
                    txtName.Text = row.Cells[3].Text.Trim();
                    txtMiddleName.Text = row.Cells[4].Text.Trim();
                    txtLastName.Text = row.Cells[5].Text.Trim();
                    txtPassword.Text = row.Cells[6].Text.Trim();
                    txtEmail.Text = row.Cells[7].Text.Trim();
                    txtBirthDate.Text = row.Cells[8].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeCustomer":
                    lblIdRemove.Text = row.Cells[0].Text;
                    ltrModalMsg.Text = "Are you sure you want to delete customer #" + lblIdRemove.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}