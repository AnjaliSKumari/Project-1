//using System;
//using System.Configuration;
//using System.Data;
//using System.Data.SqlClient;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.IO;
//using System.Text;
//using System.Globalization;

//namespace Project_Trio
//{
//    public partial class Dashboard : System.Web.UI.Page
//    {
//        private string connectionString = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (Session["UserId"] == null)
//            {
//                Response.Redirect("Login.aspx");
//                return;
//            }

//            if (!IsPostBack)
//            {
//                lblUsername.Text = Session["Username"]?.ToString() ?? "Guest";
//                LoadUserDropdown();
//                LoadDashboardData();
//            }
//        }

//        protected void lnkLogout_Click(object sender, EventArgs e)
//        {
//            Session.Clear();
//            Session.Abandon();
//            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
//            Response.Redirect("Login.aspx");
//        }

//        private void LoadDashboardData()
//        {
//            try
//            {
//                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";
//                LoadStatistics();
//                LoadUserActivitySummary(ddlUsers.SelectedValue, ddlActivityType.SelectedValue);
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

//                    SqlCommand cmdTotalUsers = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE RoleId != 2", conn);
//                    lblTotalUsers.Text = cmdTotalUsers.ExecuteScalar().ToString();

//                    SqlCommand cmdSignUpToday = new SqlCommand(@"
//                        SELECT COUNT(*)
//                        FROM UserDetails
//                        WHERE RoleId != 2 AND CONVERT(DATE, CreatedAt) = CONVERT(DATE, GETDATE())", conn);
//                    lblUsersSignedUpToday.Text = cmdSignUpToday.ExecuteScalar().ToString();

//                    SqlCommand cmdLoggedInToday = new SqlCommand(@"
//                        SELECT COUNT(DISTINCT uat.UserId)
//                        FROM UserActivityTracking uat
//                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
//                        WHERE ud.RoleId != 2
//                          AND CONVERT(DATE, uat.EntryTime) = CONVERT(DATE, GETDATE())
//                          AND uat.PageName = 'Login'", conn);
//                    lblUsersLoggedInToday.Text = cmdLoggedInToday.ExecuteScalar().ToString();

//                    SqlCommand cmdActiveToday = new SqlCommand(@"
//                        SELECT COUNT(DISTINCT uat.UserId)
//                        FROM UserActivityTracking uat
//                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
//                        WHERE uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())
//                        AND ud.RoleId != 2", conn);
//                    lblActiveToday.Text = cmdActiveToday.ExecuteScalar().ToString();

//                    SqlCommand cmdTotalSessions = new SqlCommand(@"
//                        SELECT COUNT(*)
//                        FROM UserActivityTracking uat
//                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
//                        WHERE uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())
//                        AND ud.RoleId != 2", conn);
//                    lblTotalSessions.Text = cmdTotalSessions.ExecuteScalar().ToString();

//                    // For Average Session Time on dashboard stat,
//                    // still using DATEDIFF(MINUTE) as it's an average and less sensitive to sub-minute events than sums.
//                    SqlCommand cmdAvgSessionTime = new SqlCommand(@"
//                        SELECT ISNULL(AVG(CAST(
//                            CASE
//                                WHEN uat.ExitTime IS NOT NULL THEN
//                                    DATEDIFF(MINUTE, uat.EntryTime, uat.ExitTime)
//                                ELSE
//                                    CASE
//                                        WHEN uat.EntryTime >= DATEADD(HOUR, -24, GETDATE()) THEN
//                                            DATEDIFF(MINUTE, uat.EntryTime, GETDATE())
//                                        ELSE 0
//                                    END
//                            END AS FLOAT)), 0)
//                        FROM UserActivityTracking uat
//                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
//                        WHERE uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())
//                        AND ud.RoleId != 2", conn);

//                    object avgTime = cmdAvgSessionTime.ExecuteScalar();
//                    lblAvgSessionTime.Text = avgTime != DBNull.Value ?
//                        Math.Round(Convert.ToDouble(avgTime), 1).ToString() : "0";
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading statistics: " + ex.Message, true);
//            }
//        }

//        private void LoadUserActivitySummary(string selectedUser = "All Users", string activityType = "All")
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();

//                    string query = @"
//                        WITH UserPageSummary AS (
//                            SELECT
//                                ud.Id AS UserId,
//                                ud.Username,
//                                ud.Email,
//                                ud.CreatedAt,
//                                uat.PageName,
//                                -- Sum of DATEDIFF in SECONDS for accurate aggregation
//                                SUM(
//                                    CASE
//                                        WHEN uat.ExitTime IS NOT NULL THEN DATEDIFF(SECOND, uat.EntryTime, uat.ExitTime)
//                                        ELSE DATEDIFF(SECOND, uat.EntryTime, GETDATE())
//                                    END
//                                ) AS PageTimeSeconds 
//                            FROM UserDetails ud
//                            INNER JOIN UserActivityTracking uat ON ud.Id = uat.UserId
//                            WHERE
//                                ud.RoleId != 2
//                                AND uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())";

//                    if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All Users")
//                    {
//                        query += " AND ud.Username = @Username";
//                    }

//                    if (!string.IsNullOrEmpty(activityType) && activityType != "All")
//                    {
//                        if (activityType == "Login")
//                        {
//                            query += " AND uat.PageName = 'Login'";
//                        }
//                        else if (activityType == "Signup")
//                        {
//                            query += " AND uat.PageName = 'Signup'";
//                        }
//                        else if (activityType == "PageView")
//                        {
//                            query += " AND uat.PageName NOT IN ('Login', 'Signup')";
//                        }
//                    }

//                    query += @"
//                            GROUP BY ud.Id, ud.Username, ud.Email, ud.CreatedAt, uat.PageName
//                        )
//                        SELECT
//                            ups.UserId,
//                            ups.Username,
//                            ups.Email,
//                            STUFF((
//                                SELECT '|' + ups2.PageName
//                                FROM UserPageSummary ups2
//                                WHERE ups2.UserId = ups.UserId
//                                FOR XML PATH('')), 1, 1, '') AS PagesVisitedList,
//                            -- Sum of PageTimeSeconds for total time
//                            SUM(ups.PageTimeSeconds) AS TotalTimeSeconds, 
//                            MIN(ups.CreatedAt) AS CreatedAt
//                        FROM UserPageSummary ups
//                        GROUP BY ups.UserId, ups.Username, ups.Email, ups.CreatedAt
//                        ORDER BY ups.Username;";

//                    SqlCommand cmd = new SqlCommand(query, conn);

//                    if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All Users")
//                    {
//                        cmd.Parameters.AddWithValue("@Username", selectedUser);
//                    }

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

//        protected void btnExport_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    string query = @"
//                        WITH UserPageSummary AS (
//                            SELECT
//                                ud.Id AS UserId,
//                                ud.Username,
//                                ud.Email,
//                                ud.CreatedAt,
//                                uat.PageName,
//                                -- Sum of DATEDIFF in SECONDS for export consistency
//                                SUM(
//                                    CASE
//                                        WHEN uat.ExitTime IS NOT NULL THEN DATEDIFF(SECOND, uat.EntryTime, uat.ExitTime)
//                                        ELSE DATEDIFF(SECOND, uat.EntryTime, GETDATE())
//                                    END
//                                ) AS PageTimeSeconds 
//                            FROM UserDetails ud
//                            INNER JOIN UserActivityTracking uat ON ud.Id = uat.UserId
//                            WHERE
//                                ud.RoleId != 2
//                                AND uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())
//                            GROUP BY ud.Id, ud.Username, ud.Email, ud.CreatedAt, uat.PageName
//                        )
//                        SELECT
//                            ups.Username,
//                            ups.Email,
//                            STUFF((
//                                SELECT ', ' + ups2.PageName
//                                FROM UserPageSummary ups2
//                                WHERE ups2.UserId = ups.UserId
//                                FOR XML PATH('')), 1, 2, '') AS PagesVisited,
//                            -- Sum of PageTimeSeconds for export total
//                            SUM(ups.PageTimeSeconds) AS TotalTimeSeconds, 
//                            MIN(ups.CreatedAt) AS UserCreatedAt
//                        FROM UserPageSummary ups
//                        GROUP BY ups.UserId, ups.Username, ups.Email, ups.CreatedAt
//                        ORDER BY ups.Username";

//                    SqlCommand cmd = new SqlCommand(query, conn);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);

//                    Response.Clear();
//                    Response.Buffer = true;
//                    Response.AddHeader("content-disposition", "attachment;filename=UserActivity24Hours.xls");
//                    Response.Charset = "";
//                    Response.ContentType = "application/vnd.ms-excel";

//                    using (StringWriter sw = new StringWriter())
//                    {
//                        sw.WriteLine("<table border='1'>");
//                        sw.WriteLine("<tr>");
//                        foreach (DataColumn dc in dt.Columns)
//                        {
//                            sw.WriteLine($"<th>{dc.ColumnName}</th>");
//                        }
//                        sw.WriteLine("</tr>");

//                        foreach (DataRow row in dt.Rows)
//                        {
//                            sw.WriteLine("<tr>");
//                            foreach (DataColumn dc in dt.Columns)
//                            {
//                                string columnValue = row[dc].ToString();
//                                // Pass TotalTimeSeconds to FormatTime
//                                if (dc.ColumnName == "TotalTimeSeconds" && row[dc] != DBNull.Value)
//                                {
//                                    columnValue = FormatTime(Convert.ToInt32(row[dc]));
//                                }
//                                else if (dc.ColumnName == "UserCreatedAt" && row[dc] != DBNull.Value)
//                                {
//                                    columnValue = Convert.ToDateTime(row[dc]).ToString("MM/dd/yyyy HH:mm");
//                                }
//                                sw.WriteLine($"<td>{HttpUtility.HtmlEncode(columnValue)}</td>");
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
//            LoadUserActivitySummary(ddlUsers.SelectedValue, ddlActivityType.SelectedValue);
//        }

//        protected void gvUserSummary_RowDataBound(object sender, GridViewRowEventArgs e)
//        {
//            if (e.Row.RowType == DataControlRowType.DataRow)
//            {
//                DataRowView dr = (DataRowView)e.Row.DataItem;

//                // Format "Pages Visited" column with colored tags
//                Literal litPagesVisited = (Literal)e.Row.FindControl("litPagesVisited");
//                if (litPagesVisited != null)
//                {
//                    string pagesVisitedList = dr["PagesVisitedList"].ToString();
//                    if (!string.IsNullOrEmpty(pagesVisitedList))
//                    {
//                        string[] pages = pagesVisitedList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
//                        StringBuilder sb = new StringBuilder();
//                        foreach (string page in pages)
//                        {
//                            string cssClass = "page-tag";
//                            switch (page.ToLower())
//                            {
//                                case "home": cssClass += " home"; break;
//                                case "login": cssClass += " login"; break;
//                                case "dashboard": cssClass += " dashboard"; break;
//                                case "profile": cssClass += " profile"; break;
//                                case "signup": cssClass += " signup"; break;
//                                default: cssClass += " other"; break;
//                            }
//                            sb.Append($"<span class='{cssClass}'>{HttpUtility.HtmlEncode(page)}</span>");
//                        }
//                        litPagesVisited.Text = sb.ToString();
//                    }
//                }

//                // Format "Total Time Spent" column
//                Literal litTotalTime = (Literal)e.Row.FindControl("litTotalTime");
//                if (litTotalTime != null)
//                {
//                    if (dr["TotalTimeSeconds"] != DBNull.Value)
//                    {
//                        int totalTimeSeconds = Convert.ToInt32(dr["TotalTimeSeconds"]);
//                        litTotalTime.Text = FormatTime(totalTimeSeconds);
//                    }
//                    else
//                    {
//                        litTotalTime.Text = "N/A";
//                    }
//                }
//            }
//        }

//        protected void btnFilter_Click(object sender, EventArgs e)
//        {
//            LoadUserActivitySummary(ddlUsers.SelectedValue, ddlActivityType.SelectedValue);
//            detailsSection.Visible = false; // Hide detailed view when applying new filters
//        }

//        protected void btnRefresh_Click(object sender, EventArgs e)
//        {
//            ddlUsers.SelectedValue = "All Users";
//            ddlActivityType.SelectedValue = "All";
//            LoadDashboardData();
//            detailsSection.Visible = false; // Hide detailed view when refreshing all data
//            ShowMessage("All dashboard data refreshed.", false);
//        }

//        private void ShowMessage(string message, bool isError)
//        {
//            lblMessage.Text = message;
//            lblMessage.Visible = true;
//            lblMessage.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Green;
//            lblMessage.CssClass = isError ? "alert alert-danger" : "alert alert-success"; // Add Bootstrap alert classes
//        }

//        private void LoadUserDropdown()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand("SELECT Username FROM UserDetails WHERE RoleId != 2 ORDER BY Username", conn);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);

//                    ddlUsers.DataSource = dt;
//                    ddlUsers.DataTextField = "Username";
//                    ddlUsers.DataValueField = "Username";
//                    ddlUsers.DataBind();

//                    ddlUsers.Items.Insert(0, new ListItem("All Users", "All Users")); // Add "All Users" option
//                    ddlUsers.SelectedValue = "All Users";
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading user dropdown: " + ex.Message, true);
//            }
//        }

//        protected void btnViewActivity_Click(object sender, EventArgs e)
//        {
//            Button btn = (Button)sender;
//            int userId = Convert.ToInt32(btn.CommandArgument);

//            // Get the username of the selected user for display
//            string username = "";
//            foreach (GridViewRow row in gvUserSummary.Rows)
//            {
//                if (row.RowType == DataControlRowType.DataRow)
//                {
//                    DataRowView dr = (DataRowView)row.DataItem;
//                    if (Convert.ToInt32(dr["UserId"]) == userId)
//                    {
//                        username = dr["Username"].ToString();
//                        break;
//                    }
//                }
//            }

//            lblSelectedUser.Text = username;
//            lblSelectedUserSummary.Text = username; // Update for the second GridView title
//            LoadDetailedActivity(userId);
//            LoadPageTimeSummary(userId);
//            detailsSection.Visible = true;
//        }

//        private void LoadDetailedActivity(int userId)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    string query = @"
//                        SELECT
//                            PageName,
//                            VisitDate,
//                            EntryTime,
//                            ExitTime,
//                            -- Calculate TimeSpentSeconds here for detailed view
//                            ISNULL(DATEDIFF(SECOND, EntryTime, ExitTime), DATEDIFF(SECOND, EntryTime, GETDATE())) AS TimeSpentSeconds,
//                            SessionId
//                        FROM UserActivityTracking
//                        WHERE UserId = @UserId AND EntryTime >= DATEADD(HOUR, -24, GETDATE())
//                        ORDER BY EntryTime DESC";
//                    SqlCommand cmd = new SqlCommand(query, conn);
//                    cmd.Parameters.AddWithValue("@UserId", userId);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);
//                    gvDetailedActivity.DataSource = dt;
//                    gvDetailedActivity.DataBind();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading detailed activity: " + ex.Message, true);
//            }
//        }

//        private void LoadPageTimeSummary(int userId)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connectionString))
//                {
//                    conn.Open();
//                    string query = @"
//                        SELECT
//                            PageName,
//                            -- Sum of TimeSpentSeconds for total time on each page
//                            SUM(ISNULL(DATEDIFF(SECOND, EntryTime, ExitTime), DATEDIFF(SECOND, EntryTime, GETDATE()))) AS TotalTimeSeconds
//                        FROM UserActivityTracking
//                        WHERE UserId = @UserId AND EntryTime >= DATEADD(HOUR, -24, GETDATE())
//                        GROUP BY PageName
//                        ORDER BY PageName";
//                    SqlCommand cmd = new SqlCommand(query, conn);
//                    cmd.Parameters.AddWithValue("@UserId", userId);
//                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//                    DataTable dt = new DataTable();
//                    adapter.Fill(dt);
//                    gvPageTimeSummary.DataSource = dt;
//                    gvPageTimeSummary.DataBind();

//                    // Update specific page total times
//                    lblLoginTotalTime.Text = GetTotalTimeForPage(dt, "Login");
//                    lblHomeTotalTime.Text = GetTotalTimeForPage(dt, "Home");
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowMessage("Error loading page time summary: " + ex.Message, true);
//            }
//        }

//        private string GetTotalTimeForPage(DataTable dt, string pageName)
//        {
//            DataRow[] rows = dt.Select($"PageName = '{pageName}'");
//            if (rows.Length > 0)
//            {
//                int totalSeconds = Convert.ToInt32(rows[0]["TotalTimeSeconds"]);
//                return FormatTime(totalSeconds);
//            }
//            return "N/A";
//        }


//        // Public static method to format time (accessible from ASPX)
//        public static string FormatTime(int totalSeconds)
//        {
//            if (totalSeconds < 0) return "N/A"; // Handle potential negative values

//            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
//            if (time.TotalHours >= 1)
//            {
//                return $"{(int)time.TotalHours}h {time.Minutes}m {time.Seconds}s";
//            }
//            else if (time.TotalMinutes >= 1)
//            {
//                return $"{time.Minutes}m {time.Seconds}s";
//            }
//            else
//            {
//                return $"{time.Seconds}s";
//            }
//        }

//        protected void gvDetailedActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            gvDetailedActivity.PageIndex = e.NewPageIndex;
//            if (Session["SelectedUserIdForDetails"] != null)
//            {
//                int userId = Convert.ToInt32(Session["SelectedUserIdForDetails"]);
//                LoadDetailedActivity(userId);
//            }
//        }

//        protected void btnHideDetails_Click(object sender, EventArgs e)
//        {
//            detailsSection.Visible = false;
//            // Optionally clear selection or reset dropdown
//            // ddlUsers.SelectedValue = "All Users";
//            // LoadUserActivitySummary();
//        }
//    }
//}
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Globalization;

namespace Project_Trio
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblUsername.Text = Session["Username"]?.ToString() ?? "Guest";
                LoadUserDropdown();
                LoadDashboardData();
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("Login.aspx");
        }

        private void LoadDashboardData()
        {
            try
            {
                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";
                LoadStatistics();
                LoadUserActivitySummary(ddlUsers.SelectedValue, ddlActivityType.SelectedValue);
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

                    // Total Users
                    SqlCommand cmdTotalUsers = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE RoleId != 2", conn);
                    lblTotalUsers.Text = cmdTotalUsers.ExecuteScalar().ToString();

                    // Users Signed Up Today
                    SqlCommand cmdSignUpToday = new SqlCommand(@"
                        SELECT COUNT(*)
                        FROM UserDetails
                        WHERE RoleId != 2 AND CONVERT(DATE, CreatedAt) = CONVERT(DATE, GETDATE())", conn);
                    lblUsersToday.Text = cmdSignUpToday.ExecuteScalar().ToString();

                    // Active Users Last 24h
                    SqlCommand cmdActiveToday = new SqlCommand(@"
                        SELECT COUNT(DISTINCT uat.UserId)
                        FROM UserActivityTracking uat
                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
                        WHERE uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())
                        AND ud.RoleId != 2", conn);
                    lblActiveUsers.Text = cmdActiveToday.ExecuteScalar().ToString();

                    // Total Logins
                    SqlCommand cmdTotalLogins = new SqlCommand(@"
                        SELECT COUNT(*)
                        FROM UserActivityTracking uat
                        INNER JOIN UserDetails ud ON uat.UserId = ud.Id
                        WHERE uat.PageName = 'Login'
                        AND ud.RoleId != 2", conn);
                    lblTotalLogins.Text = cmdTotalLogins.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading statistics: " + ex.Message, true);
            }
        }

        private void LoadUserDropdown()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Username FROM UserDetails WHERE RoleId != 2 ORDER BY Username";
                    SqlCommand cmd = new SqlCommand(query, conn);
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
                ShowMessage("Error loading user dropdown: " + ex.Message, true);
            }
        }

        private void LoadUserActivitySummary(string selectedUser = "All Users", string activityType = "All")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        WITH UserPageSummary AS (
                            SELECT
                                ud.Id AS UserId,
                                ud.Username,
                                ud.Email,
                                ud.CreatedAt,
                                uat.PageName,
                                -- Sum of DATEDIFF in SECONDS for accurate aggregation
                                SUM(
                                    CASE
                                        WHEN uat.ExitTime IS NOT NULL THEN DATEDIFF(SECOND, uat.EntryTime, uat.ExitTime)
                                        ELSE DATEDIFF(SECOND, uat.EntryTime, GETDATE())
                                    END
                                ) AS PageTimeSeconds 
                            FROM UserDetails ud
                            INNER JOIN UserActivityTracking uat ON ud.Id = uat.UserId
                            WHERE
                                ud.RoleId != 2
                                AND uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())";

                    if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All Users")
                    {
                        query += " AND ud.Username = @Username";
                    }

                    if (!string.IsNullOrEmpty(activityType) && activityType != "All")
                    {
                        if (activityType == "Login")
                        {
                            query += " AND uat.PageName = 'Login'";
                        }
                        else if (activityType == "Signup")
                        {
                            query += " AND uat.PageName = 'Signup'";
                        }
                        else if (activityType == "PageView")
                        {
                            query += " AND uat.PageName NOT IN ('Login', 'Signup')";
                        }
                    }

                    query += @"
                            GROUP BY ud.Id, ud.Username, ud.Email, ud.CreatedAt, uat.PageName
                        )
                        SELECT
                            ups.UserId,
                            ups.Username,
                            ups.Email,
                            STUFF((
                                SELECT '|' + ups2.PageName
                                FROM UserPageSummary ups2
                                WHERE ups2.UserId = ups.UserId
                                FOR XML PATH('')), 1, 1, '') AS PagesVisitedList,
                            -- Sum of PageTimeSeconds for total time
                            SUM(ups.PageTimeSeconds) AS TotalTimeSeconds, 
                            MIN(ups.CreatedAt) AS CreatedAt
                        FROM UserPageSummary ups
                        GROUP BY ups.UserId, ups.Username, ups.Email, ups.CreatedAt
                        ORDER BY ups.Username;";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(selectedUser) && selectedUser != "All Users")
                    {
                        cmd.Parameters.AddWithValue("@Username", selectedUser);
                    }

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadUserActivitySummary(ddlUsers.SelectedValue, ddlActivityType.SelectedValue);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        WITH UserPageSummary AS (
                            SELECT
                                ud.Id AS UserId,
                                ud.Username,
                                ud.Email,
                                ud.CreatedAt,
                                uat.PageName,
                                -- Sum of DATEDIFF in SECONDS for export consistency
                                SUM(
                                    CASE
                                        WHEN uat.ExitTime IS NOT NULL THEN DATEDIFF(SECOND, uat.EntryTime, uat.ExitTime)
                                        ELSE DATEDIFF(SECOND, uat.EntryTime, GETDATE())
                                    END
                                ) AS PageTimeSeconds 
                            FROM UserDetails ud
                            INNER JOIN UserActivityTracking uat ON ud.Id = uat.UserId
                            WHERE
                                ud.RoleId != 2
                                AND uat.EntryTime >= DATEADD(HOUR, -24, GETDATE())
                            GROUP BY ud.Id, ud.Username, ud.Email, ud.CreatedAt, uat.PageName
                        )
                        SELECT
                            ups.Username,
                            ups.Email,
                            STUFF((
                                SELECT ', ' + ups2.PageName
                                FROM UserPageSummary ups2
                                WHERE ups2.UserId = ups.UserId
                                FOR XML PATH('')), 1, 2, '') AS PagesVisited,
                            -- Sum of PageTimeSeconds for export total
                            SUM(ups.PageTimeSeconds) AS TotalTimeSeconds, 
                            MIN(ups.CreatedAt) AS UserCreatedAt
                        FROM UserPageSummary ups
                        GROUP BY ups.UserId, ups.Username, ups.Email, ups.CreatedAt
                        ORDER BY ups.Username";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=UserActivity24Hours.xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";

                    using (StringWriter sw = new StringWriter())
                    {
                        sw.WriteLine("<table border='1'>");
                        sw.WriteLine("<tr>");
                        foreach (DataColumn dc in dt.Columns)
                        {
                            sw.WriteLine($"<th>{dc.ColumnName}</th>");
                        }
                        sw.WriteLine("</tr>");

                        foreach (DataRow dr in dt.Rows)
                        {
                            sw.WriteLine("<tr>");
                            foreach (object item in dr.ItemArray)
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
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error exporting data: " + ex.Message, true);
            }
        }

        protected void gvUserSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserSummary.PageIndex = e.NewPageIndex;
            LoadUserActivitySummary(ddlUsers.SelectedValue, ddlActivityType.SelectedValue);
        }

        protected void gvUserSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Handle Pages Visited
                Literal litPagesVisited = (Literal)e.Row.FindControl("litPagesVisited");
                if (litPagesVisited != null)
                {
                    string pagesVisitedList = DataBinder.Eval(e.Row.DataItem, "PagesVisitedList")?.ToString();
                    if (!string.IsNullOrEmpty(pagesVisitedList))
                    {
                        string[] pages = pagesVisitedList.Split('|');
                        StringBuilder sb = new StringBuilder();
                        foreach (string page in pages)
                        {
                            if (!string.IsNullOrEmpty(page))
                            {
                                string badgeClass = GetBadgeClass(page);
                                sb.Append($"<span class='badge {badgeClass}'>{page}</span> ");
                            }
                        }
                        litPagesVisited.Text = sb.ToString();
                    }
                }

                // Handle Total Time
                Literal litTotalTime = (Literal)e.Row.FindControl("litTotalTime");
                if (litTotalTime != null)
                {
                    object totalTimeSecondsObj = DataBinder.Eval(e.Row.DataItem, "TotalTimeSeconds");
                    if (totalTimeSecondsObj != null && totalTimeSecondsObj != DBNull.Value)
                    {
                        int totalTimeSeconds = Convert.ToInt32(totalTimeSecondsObj);
                        litTotalTime.Text = FormatTimeFromSeconds(totalTimeSeconds);
                    }
                    else
                    {
                        litTotalTime.Text = "00:00:00";
                    }
                }
            }
        }

        protected void btnViewActivity_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int userId = Convert.ToInt32(btn.CommandArgument);
            LoadDetailedActivity(userId);
            detailsSection.Visible = true;
        }

        protected void btnHideDetails_Click(object sender, EventArgs e)
        {
            detailsSection.Visible = false;
        }

        protected void gvDetailedActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetailedActivity.PageIndex = e.NewPageIndex;
            // Get the current user ID from ViewState or re-extract from the last clicked button
            if (ViewState["CurrentUserId"] != null)
            {
                int userId = (int)ViewState["CurrentUserId"];
                LoadDetailedActivity(userId);
            }
        }

        private void LoadDetailedActivity(int userId)
        {
            try
            {
                ViewState["CurrentUserId"] = userId;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get user information and summary
                    string userQuery = @"
                        SELECT Username, Email FROM UserDetails WHERE Id = @UserId";
                    SqlCommand userCmd = new SqlCommand(userQuery, conn);
                    userCmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader userReader = userCmd.ExecuteReader();

                    if (userReader.Read())
                    {
                        lblSelectedUserSummary.Text = userReader["Username"].ToString();
                    }
                    userReader.Close();

                    // Get activity counts
                    string summaryQuery = @"
                        SELECT 
                            SUM(CASE WHEN PageName = 'Login' THEN 1 ELSE 0 END) as TotalLogins,
                            SUM(CASE WHEN PageName = 'Signup' THEN 1 ELSE 0 END) as TotalSignups,
                            SUM(CASE WHEN PageName NOT IN ('Login', 'Signup') THEN 1 ELSE 0 END) as TotalPageViews
                        FROM UserActivityTracking 
                        WHERE UserId = @UserId AND EntryTime >= DATEADD(HOUR, -24, GETDATE())";

                    SqlCommand summaryCmd = new SqlCommand(summaryQuery, conn);
                    summaryCmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader summaryReader = summaryCmd.ExecuteReader();

                    if (summaryReader.Read())
                    {
                        lblSelectedUserTotalLogins.Text = summaryReader["TotalLogins"].ToString();
                        lblSelectedUserTotalSignups.Text = summaryReader["TotalSignups"].ToString();
                        lblSelectedUserTotalPageViews.Text = summaryReader["TotalPageViews"].ToString();
                    }
                    summaryReader.Close();

                    // Get detailed activity log
                    string detailQuery = @"
                        SELECT 
                            CASE 
                                WHEN PageName = 'Login' THEN 'Login'
                                WHEN PageName = 'Signup' THEN 'Signup'
                                ELSE 'Page View'
                            END as ActivityType,
                            PageName as PageVisited,
                            EntryTime as Timestamp,
                            EntryTime,
                            ExitTime,
                            CASE 
                                WHEN ExitTime IS NOT NULL THEN DATEDIFF(SECOND, EntryTime, ExitTime)
                                ELSE DATEDIFF(SECOND, EntryTime, GETDATE())
                            END as TimeSpentInSeconds
                        FROM UserActivityTracking 
                        WHERE UserId = @UserId AND EntryTime >= DATEADD(HOUR, -24, GETDATE())
                        ORDER BY EntryTime DESC";

                    SqlCommand detailCmd = new SqlCommand(detailQuery, conn);
                    detailCmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataAdapter detailAdapter = new SqlDataAdapter(detailCmd);
                    DataTable detailDt = new DataTable();
                    detailAdapter.Fill(detailDt);

                    gvDetailedActivity.DataSource = detailDt;
                    gvDetailedActivity.DataBind();

                    // Load page time summaries
                    LoadPageTimeSummaries(userId, conn);
                    LoadPageTimeSummaryGrid(userId, conn);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading detailed activity: " + ex.Message, true);
            }
        }

        private void LoadPageTimeSummaries(int userId, SqlConnection conn)
        {
            try
            {
                string pageTimeQuery = @"
                    SELECT 
                        PageName,
                        SUM(CASE 
                            WHEN ExitTime IS NOT NULL THEN DATEDIFF(SECOND, EntryTime, ExitTime)
                            ELSE DATEDIFF(SECOND, EntryTime, GETDATE())
                        END) as TotalSeconds
                    FROM UserActivityTracking 
                    WHERE UserId = @UserId AND EntryTime >= DATEADD(HOUR, -24, GETDATE())
                    GROUP BY PageName";

                SqlCommand pageTimeCmd = new SqlCommand(pageTimeQuery, conn);
                pageTimeCmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader pageTimeReader = pageTimeCmd.ExecuteReader();

                // Initialize all labels to "00:00:00"
                lblLoginTimeSpent.Text = "00:00:00";
                lblHomeTimeSpent.Text = "00:00:00";
                lblDashboardTimeSpent.Text = "00:00:00";
                lblUserActivityTimeSpent.Text = "00:00:00";

                while (pageTimeReader.Read())
                {
                    string pageName = pageTimeReader["PageName"].ToString();
                    int totalSeconds = Convert.ToInt32(pageTimeReader["TotalSeconds"]);
                    string formattedTime = FormatTimeFromSeconds(totalSeconds);

                    switch (pageName.ToLower())
                    {
                        case "login":
                            lblLoginTimeSpent.Text = formattedTime;
                            break;
                        case "home":
                            lblHomeTimeSpent.Text = formattedTime;
                            break;
                        case "dashboard":
                            lblDashboardTimeSpent.Text = formattedTime;
                            break;
                        case "useractivity":
                            lblUserActivityTimeSpent.Text = formattedTime;
                            break;
                    }
                }
                pageTimeReader.Close();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading page time summaries: " + ex.Message, true);
            }
        }

        private void LoadPageTimeSummaryGrid(int userId, SqlConnection conn)
        {
            try
            {
                string pageTimeSummaryQuery = @"
                    SELECT 
                        PageName,
                        CONVERT(VARCHAR(8), DATEADD(SECOND, 
                            SUM(CASE 
                                WHEN ExitTime IS NOT NULL THEN DATEDIFF(SECOND, EntryTime, ExitTime)
                                ELSE DATEDIFF(SECOND, EntryTime, GETDATE())
                            END), 0), 108) as TotalTimeSpent
                    FROM UserActivityTracking 
                    WHERE UserId = @UserId AND EntryTime >= DATEADD(HOUR, -24, GETDATE())
                    GROUP BY PageName
                    ORDER BY PageName";

                SqlCommand pageTimeSummaryCmd = new SqlCommand(pageTimeSummaryQuery, conn);
                pageTimeSummaryCmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataAdapter pageTimeSummaryAdapter = new SqlDataAdapter(pageTimeSummaryCmd);
                DataTable pageTimeSummaryDt = new DataTable();
                pageTimeSummaryAdapter.Fill(pageTimeSummaryDt);

                gvPageTimeSummary.DataSource = pageTimeSummaryDt;
                gvPageTimeSummary.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading page time summary grid: " + ex.Message, true);
            }
        }

        private string GetBadgeClass(string pageName)
        {
            switch (pageName.ToLower())
            {
                case "login":
                    return "badge-success";
                case "signup":
                    return "badge-info";
                case "dashboard":
                    return "badge-warning";
                case "useractivity":
                    return "badge-danger";
                default:
                    return "badge-secondary";
            }
        }

        private string FormatTimeFromSeconds(int totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                (int)time.TotalHours, time.Minutes, time.Seconds);
        }

        private void ShowMessage(string message, bool isError = false)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Green;
            lblMessage.Visible = true;
        }
    }
}