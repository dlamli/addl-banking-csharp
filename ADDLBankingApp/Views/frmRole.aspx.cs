using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
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
                    roles = await roleManager.GetAllRole(Session["Token"].ToString());
                    init();
                    getDataGraphic();
                }
            }
        }


        public void init()
        {
            try
            {
                gvRole.DataSource = roles.ToList();
                gvRole.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load role list.";
                lblStatus.Visible = true;
            }
        }

        private void getDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var role in roles.GroupBy(e => e.Name)
                  .Select(group => new
                  {
                      Name = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.Name))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", role.Name);
                data.AppendFormat("'{0}',", role.Quantity);
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
                Role role = new Role()
                {
                    Name = txtName.Text
                };

                Role roleInserted = await roleManager.insertRole(role, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(roleInserted.Name))
                {
                    renderModalMessage("Role created");
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
                    renderModalMessage("Role Updated");
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
                    renderModalMessage("Role deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Role table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmRole.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Role",
                    Number = 547,
                    Description = "Role table error with foreign key."
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