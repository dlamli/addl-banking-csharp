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
    public partial class frmStadistic : System.Web.UI.Page
    {
        IEnumerable<Stadistic> stadistics = new ObservableCollection<Stadistic>();
        StadisticManager stadisticManager = new StadisticManager();
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
                stadistics = await stadisticManager.GetAllStadistic();
                gvStadistic.DataSource = stadistics.ToList();
                gvStadistic.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load stadistic list.";
                lblStatus.Visible = true;
            }
        }
    }
}