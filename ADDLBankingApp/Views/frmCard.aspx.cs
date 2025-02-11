﻿using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp.Views
{
    public partial class frmCard : System.Web.UI.Page
    {

        IEnumerable<Card> cards = new ObservableCollection<Card>();
        CardManager cardManager = new CardManager();

        public string lblGraphic = string.Empty;
        public string bgColorGraphic = string.Empty;
        public string dataGraphic = string.Empty;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else
                {
                    cards = await cardManager.GetAllCard(Session["Token"].ToString());
                    init();
                    GetDataGraphic();
                }
            }
        }

        public void init()
        {
            try
            {
                gvCard.DataSource = cards.ToList();
                gvCard.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load card list.";
                lblStatus.Visible = true;
            }
        }

        private void GetDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var card in cards.GroupBy(e => e.Provider)
                  .Select(group => new
                  {
                      Provider = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.Provider))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", card.Provider);
                data.AppendFormat("'{0}',", card.Quantity);
                backgroundColor.AppendFormat("'{0}',", color);

                lblGraphic = labels.ToString().Substring(0, labels.Length - 1);
                dataGraphic = data.ToString().Substring(0, data.Length - 1);
                bgColorGraphic = backgroundColor.ToString().Substring(0, backgroundColor.Length - 1);
            }
        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            ltrTitleManagement.Text = "New Card";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            txtIdManagement.Visible = true;
            txtCardNumber.Visible = true;
            txtCCV.Visible = true;
            txtProvider.Visible = true;
            btnConfirmManagement.Visible = true;
            txtIdManagement.Text = string.Empty;
            txtCardNumber.Text = txtCardNumber.Text;
            txtCardNumber.Text = string.Empty;
            txtCCV.Text = string.Empty;
            txtProvider.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
            {
                Card card = new Card()
                {
                    CardType = ddlCardType.SelectedValue,
                    CardNumber = txtCardNumber.Text,
                    CCV = txtCCV.Text,
                    DueDate = Convert.ToDateTime(txtDueDate.Text),
                    Provider = txtProvider.Text
                };

                Card cardInserted = await cardManager.insertCard(card, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(cardInserted.CardNumber)))
                {
                    renderModalMessage("Card created");
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
                Card card = new Card()
                {
                    Id = Convert.ToInt32(txtIdManagement.Text),
                    CardType = ddlCardType.SelectedValue,
                    CardNumber = txtCardNumber.Text,
                    CCV = txtCCV.Text,
                    DueDate = Convert.ToDateTime(txtDueDate.Text),
                    Provider = txtProvider.Text
                };

                Card cardUpdated = await cardManager.updateCard(card, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(cardUpdated.CardNumber)))
                {
                    renderModalMessage("Card updated");
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
                Card card = await cardManager.deleteCard(lblIdRemove.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(Convert.ToString(card.CardNumber)))
                {
                    lblIdRemove.Text = string.Empty;
                    renderModalMessage("Card removed");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Card table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmCard.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Card",
                    Number = 547,
                    Description = "Card table error with foreign key."
                };
                await errorManager.insertErrorLog(error);
            }
        }

        protected void btnCancelModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void gvCard_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCard.Rows[index];

            switch (e.CommandName)
            {
                case "editCard":
                    ltrTitleManagement.Text = "Edit Card";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtCardNumber.Text = row.Cells[2].Text.Trim();
                    txtCCV.Text = row.Cells[3].Text.Trim();
                    txtDueDate.Text = row.Cells[4].Text.Trim();
                    txtProvider.Text = row.Cells[5].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModalManagement(); } );", true);
                    break;
                case "removeCard":
                    lblIdRemove.Text = row.Cells[0].Text;
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure you want to delete card #" + lblIdRemove.Text + "?";
                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() { openModal(); } );", true);
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