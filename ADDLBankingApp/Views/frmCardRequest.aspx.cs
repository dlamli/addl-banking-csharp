﻿using ADDLBankingApp.Managers;
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
    public partial class CardRequest : System.Web.UI.Page
    {

        IEnumerable<Models.CardRequest> cardRequest = new ObservableCollection<Models.CardRequest>();
        CardRequestManager cardRequestManager = new CardRequestManager();
        static string _id = string.Empty;

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

                        using (SqlCommand cmd = new SqlCommand("SELECT [Id], [IBAN] FROM [Account]"))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = conn;
                            conn.Open();
                            ddlAccount.DataSource = cmd.ExecuteReader();
                            ddlAccount.DataTextField = "IBAN";
                            ddlAccount.DataValueField = "Id";
                            ddlAccount.DataBind();
                        }
                    }
                }
            }
        }


        public async void init()
        {
            try
            {
                cardRequest = await cardRequestManager.GetAllCardRequest(Session["Token"].ToString());
                gvCardRequest.DataSource = cardRequest.ToList();
                gvCardRequest.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load card request list.";
                lblStatus.Visible = true;
            }
        }

        protected void gvCardRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCardRequest.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editCardRequest":
                    ltrTitleManagement.Text = "Edit Card Request";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtRequestDate.Text = row.Cells[2].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeCardRequest":
                    _id = row.Cells[0].Text.Trim();
                    ltrModalMsg.Text = "Are you sure want to remove this utility?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {

            ltrTitleManagement.Text = "New Card Request";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            txtIdManagement.Visible = true;
            txtRequestDate.Visible = true;
            txtRequestDate.Text = DateTime.Now.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Models.CardRequest cardRequest = new Models.CardRequest()
                {
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    RequestDate = DateTime.Now
                };

                Models.CardRequest cardRequestInserted = await cardRequestManager.insertCardRequest(cardRequest, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(cardRequestInserted.AccountId.ToString()))
                {
                    lblResult.Text = "Card Request created";
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
                Models.CardRequest cardRequest = new Models.CardRequest()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    AccountId = Convert.ToInt32(ddlAccount.SelectedValue),
                    RequestDate = Convert.ToDateTime(txtRequestDate.Text)
                };

                Models.CardRequest cardRequestUpdated = await cardRequestManager.updateCardRequest(cardRequest, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(cardRequestUpdated.AccountId.ToString()))
                {
                    lblResult.Text = "CardRequest updated";
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
                Models.CardRequest cardRequest = await cardRequestManager.deleteCardRequest(_id, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(cardRequest.AccountId.ToString()))
                {
                    ltrModalMsg.Text = "CardRequest deleted";
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
                    Page = "frmCardRequest.aspx",
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
    }
}