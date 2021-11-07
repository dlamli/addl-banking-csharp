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
                    lblResult.Text = "Service created";
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
                Service service = new Service()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    Description = txtDescription.Text,
                    Status = ddlStatus.SelectedValue
                };

                Service serviceUpdated = await serviceManager.updateService(service, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(serviceUpdated.Description))
                {
                    lblResult.Text = "Service updated";
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
                Service service = await serviceManager.deleteService(_id, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(service.Description))
                {
                    ltrModalMsg.Text = "Service deleted";
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
                    Page = "frmService.aspx",
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
                    ltrModalMsg.Text = "Are you sure want to remove this Service?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}