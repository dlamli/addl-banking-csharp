using ADDLBankingApp.Managers;
using ADDLBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADDLBankingApp
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    CustomerManager customerManager = new CustomerManager();



                    Customer customer = new Customer()
                    {
                        Identification = txtIdentification.Text,
                        Name = txtName.Text,
                        MiddleName = txtMiddleName.Text,
                        LastName = txtLastName.Text,
                        Email = txtEmail.Text,
                        Birthdate = DateTime.ParseExact(txtFechaNacimiento.Text, "dd/MM/yyyy", null),
                        Username = txtUsername.Text,
                        Password = txtPassword.Text,
                        Status = "A",
                        RoleId = 1
                    };



                    Customer customerRegistrado = await customerManager.Register(customer);



                    if (!string.IsNullOrEmpty(customer.Identification))
                        Response.Redirect("Login.aspx");
                    else
                    {
                        lblStatus.Text = "Hubo un error al registrar el usuario.";
                        lblStatus.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogManager errorManager = new ErrorLogManager();
                    ErrorLog error = new ErrorLog()
                    {
                        UserId = 0,
                        Date = DateTime.Now,
                        Page = "Registro.aspx",
                        Action = "btnAceptar_Click",
                        Source = ex.Source,
                        Number = ex.HResult,
                        Description = ex.Message
                    };
                    ErrorLog errorIngresado = await errorManager.insertErrorLog(error);
                    lblStatus.Text = "Hubo un error al registrar el usuario.";
                    lblStatus.Visible = true;
                }
            }
        }

        protected void btnFechaNac_Click(object sender, EventArgs e)
        {
            cldFechaNacimiento.Visible = true;
        }

        protected void cldFechaNacimiento_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaNacimiento.Text = cldFechaNacimiento.SelectedDate.ToString("dd/MM/yyyy");
            cldFechaNacimiento.Visible = false;
        }
    }
}