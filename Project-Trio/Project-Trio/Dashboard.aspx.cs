using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace Project_Trio
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Role-based access control - Only allow roles 1 and 2 to access admin dashboard
            if (Session["UserRole"] != null)
            {
                int userRole = Convert.ToInt32(Session["UserRole"]);
                if (userRole != 1 && userRole != 2)
                {
                    // Redirect unauthorized users to a different page
                    Response.Redirect("~/Unauthorized.aspx");
                    return;
                }
            }
            else
            {
                // If no role is set, redirect to login
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Check login time for session expiry
            if (Session["LoginTime"] != null)
            {
                DateTime loginTime = (DateTime)Session["LoginTime"];
                if ((DateTime.Now - loginTime).TotalHours >= 24)
                {
                    Session.Clear();
                    Response.Redirect("~/OlderPage.aspx");
                    return;
                }
            }

            if (!IsPostBack)
            {
                LoadUserActivity();
                LoadDashboardStats();
            }
        }

        protected void lnkUserManagement_Click(object sender, EventArgs e)
        {
            // Redirect to User Management page
            Response.Redirect("~/UserManagement.aspx");
        }

        protected void btnUserManagement_Click(object sender, EventArgs e)
        {
            // Alternative redirect method for the button
            Response.Redirect("~/UserManagement.aspx");
        }

        // Method to populate dashboard statistics
        protected void LoadDashboardStats()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["WebDbConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Get total users count
                    string userCountQuery = "SELECT COUNT(*) FROM UserCredentials";
                    using (SqlCommand cmd = new SqlCommand(userCountQuery, conn))
                    {
                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                        totalUsers.InnerText = userCount.ToString();
                    }

                    // Get total activities count
                    string activitiesCountQuery = "SELECT COUNT(*) FROM UserActivity";
                    using (SqlCommand cmd = new SqlCommand(activitiesCountQuery, conn))
                    {
                        int activitiesCount = Convert.ToInt32(cmd.ExecuteScalar());
                        totalActivities.InnerText = activitiesCount.ToString();
                    }

                    // Get total page visits count
                    string visitsCountQuery = "SELECT SUM(CAST(DurationSeconds AS BIGINT)) FROM UserActivity WHERE DurationSeconds IS NOT NULL";
                    using (SqlCommand cmd = new SqlCommand(visitsCountQuery, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        int totalDuration = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        totalVisits.InnerText = totalDuration.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                // Log error or show default values
                totalUsers.InnerText = "0";
                totalActivities.InnerText = "0";
                totalVisits.InnerText = "0";

                // Optional: Log the exception
                // System.Diagnostics.Debug.WriteLine($"Error loading dashboard stats: {ex.Message}");
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Check role again before allowing export
            if (Session["UserRole"] != null)
            {
                int userRole = Convert.ToInt32(Session["UserRole"]);
                if (userRole != 1 && userRole != 2)
                {
                    Response.Write("<script>alert('You do not have permission to export data.');</script>");
                    return;
                }
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=UserActivityDashboard.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                // Prepare the GridView for export (remove controls)
                PrepareControlForExport(GridViewUserActivity);

                GridViewUserActivity.RenderControl(hw);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        private void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is Button)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as Button).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }

        // Add this override to your page class to avoid runtime error during export
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET
        }

        private void LoadUserActivity()
        {
            string connStr = ConfigurationManager.ConnectionStrings["WebDbConnectionString"].ConnectionString;
            string query = @"
                SELECT 
                    ua.ActivityId,
                    uc.Id AS UserId,
                    uc.Email,
                    uc.Gender,
                    ua.Username,
                    ua.CreatedAt,
                    ua.LastLoginAt,
                    ua.PageVisited,
                    ua.VisitTime,
                    ua.DurationSeconds
                FROM UserActivity ua
                INNER JOIN UserCredentials uc ON ua.UserId = uc.Id
                ORDER BY ua.ActivityId ASC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);  // Fill the DataTable

                    GridViewUserActivity.DataSource = dt;
                    GridViewUserActivity.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Handle database connection errors
                // Optional: Show user-friendly error message
                // Response.Write($"<script>alert('Error loading data: {ex.Message}');</script>");
            }
        }

        protected void GridViewUserActivity_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridViewUserActivity.PageIndex = e.NewPageIndex;
            LoadUserActivity();
        }

        // Helper method to get user role description for display
        private string GetRoleDescription(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return "Super Admin";
                case 2:
                    return "Admin";
                default:
                    return "User";
            }
        }
    }
}