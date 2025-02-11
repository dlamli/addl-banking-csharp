﻿using System;
using ADDLBankingApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ADDLBankingApp.Managers;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Text;

namespace ADDLBankingApp.Views
{
    public partial class Marchamo : System.Web.UI.Page
    {
        IEnumerable<Models.Marchamo> marchamo = new ObservableCollection<Models.Marchamo>();
        MarchamoManager marchamoManager = new MarchamoManager();
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
                marchamo = await marchamoManager.GetAllMarchamo(Session["Token"].ToString());
                gvMarchamo.DataSource = marchamo.ToList();
                gvMarchamo.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred to load marchamo list.";
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

            marchamo = await marchamoManager.GetAllMarchamo(Session["Token"].ToString());

            foreach (var insurance in marchamo.GroupBy(t => t.VehicleType).Select(v => v.First()).Distinct())
            {
                labels.Append(String.Format("'{0}',", insurance.VehicleType));
                data.Append(String.Format("'{0}',", marchamo.Where(v => v.VehicleType == insurance.VehicleType).Count()));
                backgroundColors.Append(String.Format("'{0}',", String.Format("#{0:X6}", random.Next(0x1000000))));

                graphLabels = labels.ToString().Substring(0, labels.Length - 1);
                graphData = data.ToString().Substring(0, data.Length - 1);
                graphBackgroundColors = backgroundColors.ToString().Substring(0, backgroundColors.Length - 1);
            }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Marchamo";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            txtIdManagement.Text = string.Empty;
            txtName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtNumberPlate.Text = string.Empty;
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ltrAccountId.Visible = true;
            ddlAccount.Enabled = true;
            ltrAmount.Visible = true;
            txtAmount.Visible = true;
            ltrNumberPlate.Visible = true;
            txtNumberPlate.Visible = true;
            ltrVehicleType.Visible = true;
            ddlVehicleType.Enabled = true;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Models.Marchamo marchamo = new Models.Marchamo()
                {
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    Name = txtName.Text,
                    Amount = Convert.ToInt32(txtAmount.Text),
                    NumberPlate = txtNumberPlate.Text,
                    VehicleType = ddlVehicleType.SelectedValue.ToString()
                };

                Models.Marchamo marchamoInserted = await marchamoManager.insertMarchamo(marchamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(marchamoInserted.Id.ToString()))
                {
                    renderModalMessage("Marchamo created");
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
                Models.Marchamo marchamo = new Models.Marchamo()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    Name = txtName.Text,
                    Amount = Convert.ToInt32(txtAmount.Text),
                    NumberPlate = txtNumberPlate.Text,
                    VehicleType = ddlVehicleType.SelectedValue.ToString()
                };

                Models.Marchamo marchamoUpdated = await marchamoManager.updateMarchamo(marchamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(marchamoUpdated.Id.ToString()))
                {
                    renderModalMessage("Marchamo updated");
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
                Models.Marchamo marchamo = await marchamoManager.deleteMarchamo(lblRemoveCode.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(marchamo.Id.ToString()))
                {
                    renderModalMessage("Marchamo deleted");
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

        protected void gvMarchamo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvMarchamo.Rows[index];

            switch (e.CommandName)
            {
                case "editMarchamo":
                    ltrIdManagement.Text = "Update Loan";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtName.Text = row.Cells[2].Text.Trim();
                    txtAmount.Text = row.Cells[3].Text.Trim();
                    txtNumberPlate.Text = row.Cells[4].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
                    break;
                case "removeMarchamo":
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
            ltrMarchamoMessage.Text = text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMsg(); } );", true);
        }


        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }
    }
}