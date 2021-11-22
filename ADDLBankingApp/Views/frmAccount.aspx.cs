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
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;

namespace ADDLBankingApp.Views
{
    public partial class frmAccount : System.Web.UI.Page
    {
        IEnumerable<Account> accounts = new ObservableCollection<Account>();
        AccountManager accountManager = new AccountManager();
        string connString = ConfigurationManager.ConnectionStrings["ADDL-BANKING"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Id"] == null) Response.Redirect("~/Login.aspx");
                else
                {
                    init();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        //using (SqlCommand cmd = new SqlCommand("SELECT [Id], [Name] FROM [Customer]"))
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

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        using (var cmd = new SqlCommand("getAllCurrencies", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlCurrency.DataSource = cmd.ExecuteReader();
                            ddlCurrency.DataTextField = "Description";
                            ddlCurrency.DataValueField = "Id";
                            ddlCurrency.DataBind();
                        }

                    }

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand("getAllCard", conn) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;
                            ddlCard.DataSource = cmd.ExecuteReader();
                            ddlCard.DataTextField = "CardNumber";
                            ddlCard.DataValueField = "Id";
                            ddlCard.DataBind();
                        }
                    }

                }

            }
        }


        private void clearFields()
        {
            txtIdManagement.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtBalance.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
        }
        public async void init()
        {
            try
            {
                accounts = await accountManager.GetAllAccount(Session["Token"].ToString());
                gvAccount.DataSource = accounts.ToList();
                gvAccount.DataBind();
            }
            catch (Exception ex)
            {

                lblStatus.Text = "An error ocurred  to load account list.";
                lblStatus.Visible = true;
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Source = ex.Source,
                    Number = ex.HResult,
                    Description = ex.Message,
                    Page = "frmAccount.aspx",
                    Action = "init",
                };
                await errorManager.insertErrorLog(error);
            }
        }

        public bool checkExistAccount(string IBAN, int cardId)
        {
            bool accountExist = false;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("CheckAccountCreation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IBAN", IBAN.Trim());
                    cmd.Parameters.AddWithValue("@cardId", cardId);
                    conn.Open();
                    accountExist = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }

            return accountExist;

        }

        protected async void btnConfirmManagement_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtIdManagement.Text)) //Insert
                {
                    if (checkExistAccount(txtIban.Text, Convert.ToInt32(ddlCard.SelectedValue)))
                    {
                        Account account = new Account()
                        {
                            UserId = Convert.ToInt32(ddlUser.SelectedValue),
                            CurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue),
                            CardId = Convert.ToInt32(ddlCard.SelectedValue),
                            Description = txtDescription.Text,
                            IBAN = txtIban.Text,
                            Balance = Convert.ToDecimal(txtBalance.Text),
                            Status = ddlStatus.SelectedValue,
                            PhoneNumber = txtPhoneNumber.Text
                        };

                        Account accountInserted = await accountManager.insertAccount(account, Session["Token"].ToString());

                        if (!string.IsNullOrEmpty(accountInserted.Description))
                        {
                            renderModalMessage("Account created");
                            init();

                        }
                        else
                        {
                            lblResult.Text = "An error ocurred to do this action.";
                            lblResult.Visible = true;
                            lblResult.ForeColor = Color.Red;
                        }
                    }
                    else // Account exist
                    {
                        renderModalMessage("Account already exist");
                    }
                }
                else // Edit
                {
                    Account account = new Account()
                    {
                        Id = Convert.ToInt32(txtIdManagement.Text),
                        UserId = Convert.ToInt32(ddlUser.SelectedValue),
                        CurrencyId = Convert.ToInt32(ddlCurrency.SelectedValue),
                        CardId = Convert.ToInt32(ddlCard.SelectedValue),
                        Description = txtDescription.Text,
                        IBAN = txtIban.Text,
                        Balance = Convert.ToDecimal(txtBalance.Text),
                        Status = ddlStatus.SelectedValue,
                        PhoneNumber = txtPhoneNumber.Text
                    };

                    Account accountUpdated = await accountManager.updateAccount(account, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(accountUpdated.Description))
                    {

                        renderModalMessage("Account Updated");
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
            catch (Exception)
            {
                renderModalMessage("Missing fields. Compelte them please");
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
                Account account = await accountManager.deleteAccount(lblIdRemove.Text, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(account.Description))
                {
                    renderModalMessage("Account deleted");
                    init();
                }
            }
            catch (Exception)
            {
                renderModalMessage("Account table error with foreign key.");
                ErrorLogManager errorManager = new ErrorLogManager();
                ErrorLog error = new ErrorLog()
                {
                    UserId = Convert.ToInt32(Session["Id"].ToString()),
                    Date = DateTime.Now,
                    Page = "frmAccount.aspx",
                    Action = "btnConfirmModal_Click",
                    Source = "Account",
                    Number = 547,
                    Description = "Account table error with foreign key."
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
            clearFields();
            ltrTitleManagement.Text = "New Account";
            btnConfirmManagement.ControlStyle.CssClass = "btn btn-sucess";
            btnConfirmManagement.Visible = true;
            ltrIdManagement.Visible = true;
            txtIdManagement.Visible = true;
            ddlUser.Enabled = true;
            ddlCurrency.Enabled = true;
            ddlCard.Enabled = true;
            ltrDescription.Visible = true;
            txtDescription.Visible = true;
            ddlStatus.Enabled = true;
            ddlCurrency.Enabled = true;
            txtIban.Text = generateIBAN();
            txtIban.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
        }

        protected void gvAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAccount.Rows[index];
            init();
            switch (e.CommandName)
            {
                case "editAccount":
                    ltrTitleManagement.Text = "Edit Account";
                    btnConfirmManagement.ControlStyle.CssClass = "btn btn-primary";
                    txtIdManagement.Text = row.Cells[0].Text.Trim();
                    txtDescription.Text = row.Cells[4].Text.Trim();
                    txtIban.Text = row.Cells[5].Text.Trim();
                    txtBalance.Text = row.Cells[6].Text.Trim();
                    txtPhoneNumber.Text = row.Cells[7].Text.Trim();
                    btnConfirmManagement.Visible = true;
                    txtIban.Enabled = false;
                    ddlCard.SelectedValue = row.Cells[3].Text.Trim();
                    ddlUser.SelectedValue = row.Cells[1].Text.Trim();
                    ddlCurrency.SelectedValue = row.Cells[2].Text.Trim();
                    ddlCard.Enabled = false;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalManagement(); } );", true);
                    break;

                case "removeAccount":
                    lblIdRemove.Text = row.Cells[0].Text;
                    lblIdRemove.Visible = false;
                    ltrModalMsg.Text = "Are you sure you want to delete account #" + lblIdRemove.Text + "?";
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
            clearFields();
        }

        protected void btnModalMessage_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModalMsg(); });", true);
        }

        private string generateIBAN()
        {
            string IBAN = "";
            Random rnd = new Random();

            for (int i = 0; i < 22; i++)
            {
                IBAN += rnd.Next(0, 9);
            }

            return IBAN;
        }

        protected void btnExportXls_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            HideColumns(true);
            Response.ContentType = "application/ms-excel";
            Response.AddHeader("content-disposition", $"attachment; filename=Account-{DateTime.Now}.xls");
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gvAccount.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.End();
            HideColumns(false);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        public void HideColumns(bool show)
        {
            gvAccount.Columns[9].Visible = !show;
            gvAccount.Columns[10].Visible = !show;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }

        protected void btnExportPdf_Click(object sender, EventArgs e)
        {

            int columnsCount = gvAccount.HeaderRow.Cells.Count;
            // Create the PDF Table specifying the number of columns
            PdfPTable pdfTable = new PdfPTable(columnsCount);

            // Loop thru each cell in GrdiView header row
            foreach (TableCell gridViewHeaderCell in gvAccount.HeaderRow.Cells)
            {
                // Create the Font Object for PDF document
                iTextSharp.text.Font font = new iTextSharp.text.Font();
                // Set the font color to GridView header row font color
                font.Color = new BaseColor(gvAccount.HeaderStyle.ForeColor);

                // Create the PDF cell, specifying the text and font
                PdfPCell pdfCell = new PdfPCell(new Phrase(gridViewHeaderCell.Text, font));

                // Set the PDF cell backgroundcolor to GridView header row BackgroundColor color
                pdfCell.BackgroundColor = new BaseColor(gvAccount.HeaderStyle.BackColor);

                // Add the cell to PDF table
                pdfTable.AddCell(pdfCell);
            }

            // Loop thru each datarow in GrdiView
            foreach (GridViewRow gridViewRow in gvAccount.Rows)
            {
                if (gridViewRow.RowType == DataControlRowType.DataRow)
                {
                    // Loop thru each cell in GrdiView data row
                    foreach (TableCell gridViewCell in gridViewRow.Cells)
                    {
                        iTextSharp.text.Font font = new iTextSharp.text.Font();

                        PdfPCell pdfCell = new PdfPCell(new Phrase(gridViewCell.Text, font));

                        pdfCell.BackgroundColor = new BaseColor(gvAccount.RowStyle.BackColor);

                        pdfTable.AddCell(pdfCell);
                    }
                }
            }

            // Create the PDF document specifying page size and margins
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            // Roate page using Rotate() function, if you want in Landscape
            pdfDocument.SetPageSize(PageSize.A4.Rotate());

            // Using PageSize.A4_LANDSCAPE may not work as expected
            // Document pdfDocument = new Document(PageSize.A4_LANDSCAPE, 10f, 10f, 10f, 10f);

            PdfWriter.GetInstance(pdfDocument, Response.OutputStream);

            pdfDocument.Open();
            pdfDocument.Add(pdfTable);
            pdfDocument.Close();

            Response.ContentType = "application/pdf";
            Response.AppendHeader("content-disposition",
                $"attachment;filename=Account-{DateTime.Now}.pdf");
            Response.Write(pdfDocument);
            Response.Flush();
            Response.End();
        }

        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             $"attachment;filename=Account-{DateTime.Now}.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";

            gvAccount.AllowPaging = false;

            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < gvAccount.Columns.Count - 2; k++)
            {
                //add separator
                sb.Append(gvAccount.Columns[k].HeaderText + ',');
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < gvAccount.Rows.Count; i++)
            {
                for (int k = 0; k < gvAccount.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(gvAccount.Rows[i].Cells[k].Text + ',');
                }
                //append new line
                sb.Append("\r\n");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {

        }
    }
}