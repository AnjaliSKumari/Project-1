//using System;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.Web;

//namespace Project_Trio
//{
//    public partial class ActivityTracker
//    {
//        private static string GetConnectionString()
//        {
//            return ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;
//        }

//        /// <summary>
//        /// Track when user enters a page
//        /// </summary>
//        public static void TrackPageEntry(string pageName)
//        {
//            if (HttpContext.Current.Session["UserId"] == null)
//                return;

//            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
//            string username = HttpContext.Current.Session["Username"]?.ToString() ?? "";
//            string sessionId = HttpContext.Current.Session.SessionID;

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
//                {
//                    conn.Open();

//                    // First, get user details
//                    string getUserQuery = "SELECT Email, CreatedAt FROM UserDetails WHERE Id = @UserId";
//                    SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);
//                    getUserCmd.Parameters.AddWithValue("@UserId", userId);

//                    string email = "";
//                    DateTime userCreatedAt = DateTime.Now;

//                    SqlDataReader reader = getUserCmd.ExecuteReader();
//                    if (reader.Read())
//                    {
//                        email = reader["Email"].ToString();
//                        userCreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
//                    }
//                    reader.Close();

//                    // Insert tracking record
//                    string insertQuery = @"
//                        INSERT INTO UserActivityTracking 
//                        (UserId, Username, Email, UserCreatedAt, PageName, EntryTime, SessionId)
//                        VALUES (@UserId, @Username, @Email, @UserCreatedAt, @PageName, @EntryTime, @SessionId)";

//                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
//                    insertCmd.Parameters.AddWithValue("@UserId", userId);
//                    insertCmd.Parameters.AddWithValue("@Username", username);
//                    insertCmd.Parameters.AddWithValue("@Email", email);
//                    insertCmd.Parameters.AddWithValue("@UserCreatedAt", userCreatedAt);
//                    insertCmd.Parameters.AddWithValue("@PageName", pageName);
//                    insertCmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);
//                    insertCmd.Parameters.AddWithValue("@SessionId", sessionId);

//                    insertCmd.ExecuteNonQuery();

//                    // Store the tracking ID in session for exit tracking
//                    HttpContext.Current.Session[$"TrackingId_{pageName}"] = GetLastTrackingId(conn, userId, pageName);
//                }
//            }
//            catch (Exception ex)
//            {
//                // Log error (implement logging as needed)
//                System.Diagnostics.Debug.WriteLine($"Error tracking page entry: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Track when user exits a page
//        /// </summary>
//        public static void TrackPageExit(string pageName)
//        {
//            if (HttpContext.Current.Session["UserId"] == null)
//                return;

//            string trackingIdKey = $"TrackingId_{pageName}";
//            if (HttpContext.Current.Session[trackingIdKey] == null)
//                return;

//            int trackingId = Convert.ToInt32(HttpContext.Current.Session[trackingIdKey]);

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
//                {
//                    conn.Open();

//                    string updateQuery = "UPDATE UserActivityTracking SET ExitTime = @ExitTime WHERE Id = @TrackingId";
//                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
//                    updateCmd.Parameters.AddWithValue("@ExitTime", DateTime.Now);
//                    updateCmd.Parameters.AddWithValue("@TrackingId", trackingId);

//                    updateCmd.ExecuteNonQuery();
//                }

//                // Clear from session
//                HttpContext.Current.Session.Remove(trackingIdKey);
//            }
//            catch (Exception ex)
//            {
//                // Log error (implement logging as needed)
//                System.Diagnostics.Debug.WriteLine($"Error tracking page exit: {ex.Message}");
//            }
//        }

//        /// <summary>
//        /// Track page exit for all pages when session ends or user logs out
//        /// </summary>
//        public static void TrackAllPageExits()
//        {
//            string[] pages = { "Login", "Signup", "Home", "Dashboard" };
//            foreach (string page in pages)
//            {
//                TrackPageExit(page);
//            }
//        }

//        private static int GetLastTrackingId(SqlConnection conn, int userId, string pageName)
//        {
//            string query = @"
//                SELECT TOP 1 Id FROM UserActivityTracking 
//                WHERE UserId = @UserId AND PageName = @PageName 
//                ORDER BY EntryTime DESC";

//            SqlCommand cmd = new SqlCommand(query, conn);
//            cmd.Parameters.AddWithValue("@UserId", userId);
//            cmd.Parameters.AddWithValue("@PageName", pageName);

//            object result = cmd.ExecuteScalar();
//            return result != null ? Convert.ToInt32(result) : 0;
//        }
//    }
//}


//new code
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace Project_Trio
{
    public partial class ActivityTracker
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;
        }

        /// <summary>
        /// Track when user enters a page (excludes admin users and dashboard)
        /// </summary>
        public static void TrackPageEntry(string pageName)
        {
            if (HttpContext.Current.Session["UserId"] == null)
                return;

            // Skip tracking for Dashboard page
            if (pageName.Equals("Dashboard", StringComparison.OrdinalIgnoreCase))
                return;

            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            int roleId = HttpContext.Current.Session["RoleId"] != null ?
                        Convert.ToInt32(HttpContext.Current.Session["RoleId"]) : 1;

            // Skip tracking for admin users (RoleId = 2)
            if (roleId == 2)
                return;

            string username = HttpContext.Current.Session["Username"]?.ToString() ?? "";
            string sessionId = HttpContext.Current.Session.SessionID;

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // First, get user details
                    string getUserQuery = "SELECT Email, CreatedAt FROM UserDetails WHERE Id = @UserId";
                    SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);
                    getUserCmd.Parameters.AddWithValue("@UserId", userId);

                    string email = "";
                    DateTime userCreatedAt = DateTime.Now;

                    SqlDataReader reader = getUserCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        email = reader["Email"].ToString();
                        userCreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                    }
                    reader.Close();

                    // Insert tracking record
                    string insertQuery = @"
                        INSERT INTO UserActivityTracking 
                        (UserId, Username, Email, UserCreatedAt, PageName, EntryTime, SessionId)
                        VALUES (@UserId, @Username, @Email, @UserCreatedAt, @PageName, @EntryTime, @SessionId)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@UserCreatedAt", userCreatedAt);
                    insertCmd.Parameters.AddWithValue("@PageName", pageName);
                    insertCmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                    insertCmd.Parameters.AddWithValue("@SessionId", sessionId);

                    insertCmd.ExecuteNonQuery();

                    // Store the tracking ID in session for exit tracking
                    HttpContext.Current.Session[$"TrackingId_{pageName}"] = GetLastTrackingId(conn, userId, pageName);
                }
            }
            catch (Exception ex)
            {
                // Log error (implement logging as needed)
                System.Diagnostics.Debug.WriteLine($"Error tracking page entry: {ex.Message}");
            }
        }

        /// <summary>
        /// Track when user exits a page (excludes admin users and dashboard)
        /// </summary>
        public static void TrackPageExit(string pageName)
        {
            if (HttpContext.Current.Session["UserId"] == null)
                return;

            // Skip tracking for Dashboard page
            if (pageName.Equals("Dashboard", StringComparison.OrdinalIgnoreCase))
                return;

            int roleId = HttpContext.Current.Session["RoleId"] != null ?
                        Convert.ToInt32(HttpContext.Current.Session["RoleId"]) : 1;

            // Skip tracking for admin users (RoleId = 2)
            if (roleId == 2)
                return;

            string trackingIdKey = $"TrackingId_{pageName}";
            if (HttpContext.Current.Session[trackingIdKey] == null)
                return;

            int trackingId = Convert.ToInt32(HttpContext.Current.Session[trackingIdKey]);

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string updateQuery = "UPDATE UserActivityTracking SET ExitTime = @ExitTime WHERE Id = @TrackingId";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@ExitTime", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("@TrackingId", trackingId);

                    updateCmd.ExecuteNonQuery();
                }

                // Clear from session
                HttpContext.Current.Session.Remove(trackingIdKey);
            }
            catch (Exception ex)
            {
                // Log error (implement logging as needed)
                System.Diagnostics.Debug.WriteLine($"Error tracking page exit: {ex.Message}");
            }
        }

        /// <summary>
        /// Track page exit for all pages when session ends or user logs out
        /// Dashboard removed from tracking list
        /// </summary>
        public static void TrackAllPageExits()
        {
            // Removed "Dashboard" from the array
            string[] pages = { "Login", "Signup", "Home" };
            foreach (string page in pages)
            {
                TrackPageExit(page);
            }
        }

        private static int GetLastTrackingId(SqlConnection conn, int userId, string pageName)
        {
            string query = @"
                SELECT TOP 1 Id FROM UserActivityTracking 
                WHERE UserId = @UserId AND PageName = @PageName 
                ORDER BY EntryTime DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@PageName", pageName);

            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}