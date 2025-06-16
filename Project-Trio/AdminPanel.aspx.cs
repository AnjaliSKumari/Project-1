using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Project_Trio
{
	public partial class AdminPanel : System.Web.UI.Page
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (Session["RoleId"] == null || Convert.ToInt32(Session["RoleId"]) != 2)
            {
                Response.Redirect("Home.aspx");
            }

            if (!IsPostBack)
            {
                lblAdminUsername.Text = Session["Username"]?.ToString() ?? "Admin";
                pnlAdminMenu.Visible = false;
            }
        }

        protected void imgAdminIcon_Click(object sender, EventArgs e)
        {
            // Toggle visibility of admin menu panel
            pnlAdminMenu.Visible = !pnlAdminMenu.Visible;
        }

        protected void lnkDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }

        protected void lnkUserActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserActivity.aspx");
        }

    }
}