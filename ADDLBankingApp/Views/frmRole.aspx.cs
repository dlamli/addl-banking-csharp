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
    public partial class frmRole : System.Web.UI.Page
    {
        IEnumerable<Role> roles = new ObservableCollection<Role>();
        RoleManager roleManager = new RoleManager();
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
                roles = await roleManager.GetAllRole(Session["Token"].ToString());
                gvRole.DataSource = roles.ToList();
                gvRole.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load role list.";
                lblStatus.Visible = true;
            }
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Role role = new Role()
                {
                    Name = txtName.Text
                };

                Role roleInserted = await roleManager.insertRole(role, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(roleInserted.Name))
                {
                    lblResult.Text = "Role created";
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
                Role role = new Role()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    Name = txtName.Text
                };

                Role roleUpdated = await roleManager.updateRole(role, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(roleUpdated.Name))
                {
                    lblResult.Text = "Role updated";
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
                Role role = await roleManager.deleteRole(_id, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(role.Name))
                {
                    ltrModalMsg.Text = "Role deleted";
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
                    Page = "frmRole.aspx",
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
            ltrTitleManagement.Text = "New Role";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            txtName.Visible = true;
            ltrName.Visible = true;
            txtIdManagement.Text = string.Empty;
            txtName.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvRole.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editRole":
                    ltrTitleManagement.Text = "Edit Role";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtName.Text = row.Cells[1].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeRole":
                    _id = row.Cells[0].Text.Trim();
                    ltrModalMsg.Text = "Are you sure want to remove this role?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }
    }
}