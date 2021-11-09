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

namespace ADDLBankingApp.Views
{
    public partial class frmService : System.Web.UI.Page
    {
        IEnumerable<Service> services = new ObservableCollection<Service>();
        ServiceManager serviceManager = new ServiceManager();
        static string _id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else init();
            }
        }

        public async void init()
        {
            try
            {
                services = await serviceManager.GetAllService(Session["Token"].ToString());
                gvService.DataSource = services.ToList();
                gvService.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load Service list.";
                lblStatus.Visible = true;
            }
        }
        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Service service = new Service()
                {
                    Description = txtDescription.Text,
                    Status = ddlStatus.SelectedValue
                };

                Service serviceInserted = await serviceManager.insertService(service, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(serviceInserted.Description))
                {
                    renderModalMessage("Service created");
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
                Service service = new Service()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    Description = txtDescription.Text,
                    Status = ddlStatus.SelectedValue
                };

                Service serviceUpdated = await serviceManager.updateService(service, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(serviceUpdated.Description))
                {
                    renderModalMessage("Service updated");
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
                Service service = await serviceManager.deleteService(_id, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(service.Description))
                {
                    renderModalMessage("Service deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Service table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmService.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Service",
                    Number = 547,
                    Description = "Service table error with foreign key."
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
            ltrTitleManagement.Text = "New Service";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            txtDescription.Visible = true;
            ltrDescription.Visible = true;
            txtIdManagement.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvService.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editService":
                    ltrTitleManagement.Text = "Edit Service";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtDescription.Text = row.Cells[1].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeService":
                    _id = row.Cells[0].Text.Trim();
                    ltrModalMsg.Text = "Are you sure want to remove service #" +_id+" ?";
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