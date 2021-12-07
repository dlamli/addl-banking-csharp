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
using System.Text;

namespace ADDLBankingApp.Views
{
    public partial class frmSession : System.Web.UI.Page
    {
        IEnumerable<Session> sessions = new ObservableCollection<Session>();
        SessionManager sessionManager = new SessionManager();
        static string _id = string.Empty;
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
                    sessions = await sessionManager.GetAllSession(Session["Token"].ToString());
                    init();
                    getDataGraphic();

                }
            
            }
        }

        public  void init()
        {
            try
            {
                
                gvSession.DataSource = sessions.ToList();
                gvSession.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "An error ocurred  to load Utility list.";
                lblStatus.Visible = true;
            }
        }


        private void getDataGraphic()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColor = new StringBuilder();
            var random = new Random();

            foreach (var session in sessions.GroupBy(e => e.UserId)
                  .Select(group => new
                  {
                      UserId = group.Key,
                      Quantity = group.Count()
                  }).OrderBy(c => c.UserId))
            {
                string color = String.Format("#{0:X}", random.Next(0, 0x1000000));
                labels.AppendFormat("'{0}',", session.UserId);
                data.AppendFormat("'{0}',", session.Quantity);
                backgroundColor.AppendFormat("'{0}',", color);

                lblGraphic = labels.ToString().Substring(0, labels.Length - 1);
                dataGraphic = data.ToString().Substring(0, data.Length - 1);
                bgColorGraphic = backgroundColor.ToString().Substring(0, backgroundColor.Length - 1);
            }
        }


    }
}