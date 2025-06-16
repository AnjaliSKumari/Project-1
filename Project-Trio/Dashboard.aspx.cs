<<<<<<< HEAD
﻿using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

namespace Project_Trio
{
    public partial class Dashboard : Page
    {
=======
﻿//using System;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.IO;

//namespace Project_Trio
//{
//    public partial class Dashboard : System.Web.UI.Page
//    {
//        private string connectionString = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            // Check if user is logged in
//            if (Session["UserId"] == null)
//            {
//                Response.Redirect("Login.aspx");
//                return;
//            }

//            // Track dashboard page entry
//            ActivityTracker.TrackPageEntry("Dashboard");

//            if (!IsPostBack)
//            {
//                LoadDashboardData();
//                LoadUserDropdown();
//                SetDefaultDates();
//            }
//        }

//        protected void Page_Unload(object sender, EventArgs e)
//        {
//            // Track dashboard page exit
//            ActivityTracker.TrackPageExit("Dashboard");
//        }

//        private void LoadDashboardData()
//        {
//            try
//            {
//                // Set welcome message
//                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";

//                // Load statistics
//                LoadStatistics();

//                // Load user activity summary
//                LoadUserActivitySummary();
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading dashboard data: " + ex.Message, true);
//            }
//        }

//        private void LoadStatistics()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();

//                    // Total Users
//                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM UserDetails", conn);
//                    lblTotalUsers.Text = cmd1.ExecuteScalar().ToString();

//                    // Active Today
//                    SqlCommand cmd2 = new SqlCommand(@"
//                        SELECT COUNT(DISTINCT UserId) 
//                        FROM UserActivityTracking 
//                        WHERE CAST(EntryTime AS DATE) = CAST(GETDATE() AS DATE)", conn);
//                    lblActiveToday.Text = cmd2.ExecuteScalar().ToString();

//                    // Total Sessions
//                    SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM UserActivityTracking", conn);
//                    lblTotalSessions.Text = cmd3.ExecuteScalar().ToString();

//                    // Average Session Time
//                    SqlCommand cmd4 = new SqlCommand(@"
//                        SELECT ISNULL(AVG(CAST(TimeSpentMinutes AS FLOAT)), 0) 
//                        FROM UserActivityTracking 
//                        WHERE ExitTime IS NOT NULL", conn);
//                    object avgTime = cmd4.ExecuteScalar();
//                    lblAvgSessionTime.Text = avgTime != DBNull.Value ?
//                        Math.Round(Convert.ToDouble(avgTime), 1).ToString() : "0";
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading statistics: " + ex.Message, true);
//            }
//        }

//        private void LoadUserActivitySummary(string selectedUser = "", DateTime? dateFrom = null, DateTime? dateTo = null)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();

//                    string query = "SELECT * FROM vw_UserDashboardSummary WHERE 1=1";
//                    SqlCommand cmd = new SqlCommand();
//                    cmd.Connection = conn;

//                    // Add filters
//                    if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All Users")
//                    {
//                        query += " AND Username = @Username";
//                        cmd.Parameters.AddWithValue("@Username", selectedUser);
//                    }

//                    if (dateFrom.HasValue)
//                    {
//                        query += " AND UserCreatedAt >= @DateFrom";
//                        cmd.Parameters.AddWithValue("@DateFrom", dateFrom.Value);
//                    }

//                    if (dateTo.HasValue)
//                    {
//                        query += " AND UserCreatedAt <= @DateTo";
//                        cmd.Parameters.AddWithValue("@DateTo", dateTo.Value.AddDays(1));
//                    }

//                    query += " ORDER BY Username";
//                    cmd.CommandText = query;

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);

//                    gvUserSummary.DataSource = dt;
//                    gvUserSummary.DataBind();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading user activity summary: " + ex.Message, true);
//            }
//        }

//        private void LoadUserDropdown()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand("SELECT DISTINCT Username FROM UserDetails ORDER BY Username", conn);
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    ddlUsers.Items.Clear();
//                    ddlUsers.Items.Add(new ListItem("All Users", "All Users"));

//                    while (reader.Read())
//                    {
//                        ddlUsers.Items.Add(new ListItem(reader["Username"].ToString(), reader["Username"].ToString()));
//                    }
//                    reader.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading users: " + ex.Message, true);
//            }
//        }

//        private void SetDefaultDates()
//        {
//            txtDateFrom.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
//            txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
//        }

//        protected void btnFilter_Click(object sender, EventArgs e)
//        {
//            string selectedUser = ddlUsers.SelectedValue;
//            DateTime? dateFrom = null;
//            DateTime? dateTo = null;

//            if (!string.IsNullOrEmpty(txtDateFrom.Text))
//            {
//                dateFrom = DateTime.Parse(txtDateFrom.Text);
//            }

//            if (!string.IsNullOrEmpty(txtDateTo.Text))
//            {
//                dateTo = DateTime.Parse(txtDateTo.Text);
//            }

//            LoadUserActivitySummary(selectedUser, dateFrom, dateTo);
//            detailsSection.Visible = false; // Hide details when filtering
//        }

//        protected void btnRefresh_Click(object sender, EventArgs e)
//        {
//            LoadDashboardData();
//            detailsSection.Visible = false;
//            ShowMessage("Data refreshed successfully!", false);
//        }

//        protected void btnViewDetails_Click(object sender, EventArgs e)
//        {
//            Button btn = (Button)sender;
//            int userId = Convert.ToInt32(btn.CommandArgument);
//            LoadDetailedActivity(userId);
//        }

//        private void LoadDetailedActivity(int userId)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();

//                    // Get username for display
//                    SqlCommand userCmd = new SqlCommand("SELECT Username FROM UserDetails WHERE Id = @UserId", conn);
//                    userCmd.Parameters.AddWithValue("@UserId", userId);
//                    string username = userCmd.ExecuteScalar()?.ToString() ?? "Unknown User";
//                    lblSelectedUser.Text = username;

//                    // Get detailed activity
//                    string query = @"
//                        SELECT PageName, EntryTime, ExitTime, TimeSpentMinutes, SessionId 
//                        FROM UserActivityTracking 
//                        WHERE UserId = @UserId 
//                        ORDER BY EntryTime DESC";

//                    SqlCommand cmd = new SqlCommand(query, conn);
//                    cmd.Parameters.AddWithValue("@UserId", userId);

//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);

//                    gvDetailedActivity.DataSource = dt;
//                    gvDetailedActivity.DataBind();

//                    detailsSection.Visible = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading detailed activity: " + ex.Message, true);
//            }
//        }

//        protected void btnHideDetails_Click(object sender, EventArgs e)
//        {
//            detailsSection.Visible = false;
//        }

//        protected void btnExport_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                // Create a new DataTable with all user summary data
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand("SELECT * FROM vw_UserDashboardSummary ORDER BY Username", conn);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);

//                    // Prepare the response for Excel download
//                    Response.Clear();
//                    Response.Buffer = true;
//                    Response.AddHeader("content-disposition", "attachment;filename=UserActivityReport.xls");
//                    Response.Charset = "";
//                    Response.ContentType = "application/vnd.ms-excel";

//                    using (StringWriter sw = new StringWriter())
//                    {
//                        // Create HTML table for Excel
//                        sw.WriteLine("<table border='1'>");

//                        // Headers
//                        sw.WriteLine("<tr>");
//                        foreach (DataColumn col in dt.Columns)
//                        {
//                            sw.WriteLine($"<th>{col.ColumnName}</th>");
//                        }
//                        sw.WriteLine("</tr>");

//                        // Data rows
//                        foreach (DataRow row in dt.Rows)
//                        {
//                            sw.WriteLine("<tr>");
//                            foreach (var item in row.ItemArray)
//                            {
//                                sw.WriteLine($"<td>{item}</td>");
//                            }
//                            sw.WriteLine("</tr>");
//                        }

//                        sw.WriteLine("</table>");
//                        Response.Output.Write(sw.ToString());
//                        Response.Flush();
//                        Response.End();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error exporting data: " + ex.Message, true);
//            }
//        }

//        protected void gvUserSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            gvUserSummary.PageIndex = e.NewPageIndex;
//            LoadUserActivitySummary();
//        }

//        protected void gvDetailedActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            gvDetailedActivity.PageIndex = e.NewPageIndex;
//            // Reload the detailed activity for the currently selected user
//            if (detailsSection.Visible)
//            {
//                // Get the user ID from the first row if available
//                if (gvDetailedActivity.DataSource != null)
//                {
//                    DataTable dt = (DataTable)gvDetailedActivity.DataSource;
//                    if (dt.Rows.Count > 0)
//                    {
//                        // Find the user ID based on the username
//                        string username = lblSelectedUser.Text;
//                        using (SqlConnection conn = new SqlConnection(connectionString))
//                        {
//                            conn.Open();
//                            SqlCommand cmd = new SqlCommand("SELECT Id FROM UserDetails WHERE Username = @Username", conn);
//                            cmd.Parameters.AddWithValue("@Username", username);
//                            object result = cmd.ExecuteScalar();
//                            if (result != null)
//                            {
//                                int userId = Convert.ToInt32(result);
//                                LoadDetailedActivity(userId);
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        private void ShowMessage(string message, bool isError = false)
//        {
//            lblMessage.Text = message;
//            lblMessage.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Green;
//            lblMessage.Visible = true;
//        }
//    }
//}

//new code
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Project_Trio
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

>>>>>>> 49d0aaac94ea8cf8269bcfe79740c03dc204c3ce
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
<<<<<<< HEAD
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
=======
                Response.Redirect("Login.aspx");
                return;
            }

            // Remove dashboard tracking - no longer needed
            // ActivityTracker.TrackPageEntry("Dashboard");

            if (!IsPostBack)
            {
                LoadDashboardData();
                LoadUserDropdown();
                SetDefaultDates();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Remove dashboard tracking - no longer needed
            // ActivityTracker.TrackPageExit("Dashboard");
        }

        private void LoadDashboardData()
        {
            try
            {
                // Set welcome message
                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";

                // Load statistics
                LoadStatistics();

                // Load user activity summary
                LoadUserActivitySummary();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading dashboard data: " + ex.Message, true);
            }
        }

        private void LoadStatistics()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Total Users (excluding admins - RoleId = 2)
                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE RoleId != 2", conn);
                    lblTotalUsers.Text = cmd1.ExecuteScalar().ToString();

                    // Active Today (excluding admins)
                    SqlCommand cmd2 = new SqlCommand(@"
                        SELECT COUNT(DISTINCT uat.UserId) 
                        FROM UserActivityTracking uat
                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
                        WHERE CAST(uat.EntryTime AS DATE) = CAST(GETDATE() AS DATE)
                        AND ud.RoleId != 2", conn);
                    lblActiveToday.Text = cmd2.ExecuteScalar().ToString();

                    // Total Sessions (excluding admins)
                    SqlCommand cmd3 = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM UserActivityTracking uat
                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
                        WHERE ud.RoleId != 2", conn);
                    lblTotalSessions.Text = cmd3.ExecuteScalar().ToString();

                    // Average Session Time (excluding admins)
                    SqlCommand cmd4 = new SqlCommand(@"
                        SELECT ISNULL(AVG(CAST(uat.TimeSpentMinutes AS FLOAT)), 0) 
                        FROM UserActivityTracking uat
                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
                        WHERE uat.ExitTime IS NOT NULL AND ud.RoleId != 2", conn);
                    object avgTime = cmd4.ExecuteScalar();
                    lblAvgSessionTime.Text = avgTime != DBNull.Value ?
                        Math.Round(Convert.ToDouble(avgTime), 1).ToString() : "0";
>>>>>>> 49d0aaac94ea8cf8269bcfe79740c03dc204c3ce
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
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
=======
                ShowMessage("Error loading statistics: " + ex.Message, true);
            }
        }

        private void LoadUserActivitySummary(string selectedUser = "", DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Updated query to exclude admin users
                    string query = @"
                        SELECT vw.* FROM vw_UserDashboardSummary vw
                        INNER JOIN UserDetails ud ON vw.Username = ud.Username
                        WHERE ud.RoleId != 2";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    // Add filters
                    if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All Users")
                    {
                        query += " AND vw.Username = @Username";
                        cmd.Parameters.AddWithValue("@Username", selectedUser);
                    }

                    if (dateFrom.HasValue)
                    {
                        query += " AND vw.UserCreatedAt >= @DateFrom";
                        cmd.Parameters.AddWithValue("@DateFrom", dateFrom.Value);
                    }

                    if (dateTo.HasValue)
                    {
                        query += " AND vw.UserCreatedAt <= @DateTo";
                        cmd.Parameters.AddWithValue("@DateTo", dateTo.Value.AddDays(1));
                    }

                    query += " ORDER BY vw.Username";
                    cmd.CommandText = query;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvUserSummary.DataSource = dt;
                    gvUserSummary.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading user activity summary: " + ex.Message, true);
            }
        }

        private void LoadUserDropdown()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Updated query to exclude admin users
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT DISTINCT Username 
                        FROM UserDetails 
                        WHERE RoleId != 2 
                        ORDER BY Username", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlUsers.Items.Clear();
                    ddlUsers.Items.Add(new ListItem("All Users", "All Users"));

                    while (reader.Read())
                    {
                        ddlUsers.Items.Add(new ListItem(reader["Username"].ToString(), reader["Username"].ToString()));
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading users: " + ex.Message, true);
            }
        }

        private void SetDefaultDates()
        {
            txtDateFrom.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedUser = ddlUsers.SelectedValue;
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                dateFrom = DateTime.Parse(txtDateFrom.Text);
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                dateTo = DateTime.Parse(txtDateTo.Text);
            }

            LoadUserActivitySummary(selectedUser, dateFrom, dateTo);
            detailsSection.Visible = false; // Hide details when filtering
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
            detailsSection.Visible = false;
            ShowMessage("Data refreshed successfully!", false);
        }

        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int userId = Convert.ToInt32(btn.CommandArgument);
            LoadDetailedActivity(userId);
        }

        private void LoadDetailedActivity(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the user is not an admin before loading details
                    SqlCommand roleCheckCmd = new SqlCommand("SELECT RoleId FROM UserDetails WHERE Id = @UserId", conn);
                    roleCheckCmd.Parameters.AddWithValue("@UserId", userId);
                    object roleResult = roleCheckCmd.ExecuteScalar();

                    if (roleResult != null && Convert.ToInt32(roleResult) == 2)
                    {
                        ShowMessage("Cannot view admin user details.", true);
                        return;
                    }

                    // Get username for display
                    SqlCommand userCmd = new SqlCommand("SELECT Username FROM UserDetails WHERE Id = @UserId", conn);
                    userCmd.Parameters.AddWithValue("@UserId", userId);
                    string username = userCmd.ExecuteScalar()?.ToString() ?? "Unknown User";
                    lblSelectedUser.Text = username;

                    // Get detailed activity (already filtered by non-admin in previous check)
                    string query = @"
                        SELECT PageName, EntryTime, ExitTime, TimeSpentMinutes, SessionId 
                        FROM UserActivityTracking 
                        WHERE UserId = @UserId 
                        ORDER BY EntryTime DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvDetailedActivity.DataSource = dt;
                    gvDetailedActivity.DataBind();

                    detailsSection.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading detailed activity: " + ex.Message, true);
            }
        }

        protected void btnHideDetails_Click(object sender, EventArgs e)
        {
            detailsSection.Visible = false;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                // Create a new DataTable with all user summary data (excluding admins)
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"
                        SELECT vw.* FROM vw_UserDashboardSummary vw
                        INNER JOIN UserDetails ud ON vw.Username = ud.Username
                        WHERE ud.RoleId != 2
                        ORDER BY vw.Username", conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Prepare the response for Excel download
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=UserActivityReport.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";

                    using (StringWriter sw = new StringWriter())
                    {
                        // Create HTML table for Excel
                        sw.WriteLine("<table border='1'>");

                        // Headers
                        sw.WriteLine("<tr>");
                        foreach (DataColumn col in dt.Columns)
                        {
                            sw.WriteLine($"<th>{col.ColumnName}</th>");
                        }
                        sw.WriteLine("</tr>");

                        // Data rows
                        foreach (DataRow row in dt.Rows)
                        {
                            sw.WriteLine("<tr>");
                            foreach (var item in row.ItemArray)
                            {
                                sw.WriteLine($"<td>{item}</td>");
                            }
                            sw.WriteLine("</tr>");
                        }

                        sw.WriteLine("</table>");
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
>>>>>>> 49d0aaac94ea8cf8269bcfe79740c03dc204c3ce
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
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
=======
                ShowMessage("Error exporting data: " + ex.Message, true);
            }
        }

        protected void gvUserSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserSummary.PageIndex = e.NewPageIndex;
            LoadUserActivitySummary();
        }

        protected void gvDetailedActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetailedActivity.PageIndex = e.NewPageIndex;
            // Reload the detailed activity for the currently selected user
            if (detailsSection.Visible)
            {
                // Get the user ID from the first row if available
                if (gvDetailedActivity.DataSource != null)
                {
                    DataTable dt = (DataTable)gvDetailedActivity.DataSource;
                    if (dt.Rows.Count > 0)
                    {
                        // Find the user ID based on the username
                        string username = lblSelectedUser.Text;
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(@"
                                SELECT Id FROM UserDetails 
                                WHERE Username = @Username AND RoleId != 2", conn);
                            cmd.Parameters.AddWithValue("@Username", username);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                int userId = Convert.ToInt32(result);
                                LoadDetailedActivity(userId);
                            }
                        }
                    }
                }
            }
        }

        private void ShowMessage(string message, bool isError = false)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            lblMessage.Visible = true;
        }
>>>>>>> 49d0aaac94ea8cf8269bcfe79740c03dc204c3ce
    }
}