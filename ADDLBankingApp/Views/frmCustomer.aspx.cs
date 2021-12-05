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
using System.Text;

namespace ADDLBankingApp.Views
{
    public partial class frmCustomerManager : System.Web.UI.Page
    {
        IEnumerable<Customer> customers = new ObservableCollection<Customer>();
        CustomerManager customerManager = new CustomerManager();

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
                    customers = await customerManager.GetAllCustomer(Session["Token"].ToString());
                    init();
                    getDataGraphic();
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

        private void getDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var customer in customers.GroupBy(e => e.Status)
                  .Select(group => new
                  {
                      Provider = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.Provider))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", customer.Provider);
                data.AppendFormat("'{0}',", customer.Quantity);
                backgroundColor.AppendFormat("'{0}',", color);

                lblGraphic = labels.ToString().Substring(0, labels.Length - 1);
                dataGraphic = data.ToString().Substring(0, data.Length - 1);
                bgColorGraphic = backgroundColor.ToString().Substring(0, backgroundColor.Length - 1);
            }
        }




        public async void init()
        {
            try
            {
                gvCustomer.DataSource = customers.ToList();
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

        private void clearFields()
        {
            txtIdManagement.Text = string.Empty;
            txtIdentification.Text = string.Empty;
            txtUsername.Text = string.Empty;
            txtName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtBirthDate.Text = string.Empty;
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Customer";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            clearFields();
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
                    renderModalMessage("Customer created");
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
                    renderModalMessage("Customer updated");
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
                    renderModalMessage("Customer deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Customer table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmCustomer.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Customer",
                    Number = 547,
                    Description = "Customer table error with foreign key."
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
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure you want to delete customer #" + lblIdRemove.Text + "?";
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