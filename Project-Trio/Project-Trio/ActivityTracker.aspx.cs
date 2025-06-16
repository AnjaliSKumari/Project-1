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
        /// Private helper to update the ExitTime for a given tracking ID.
        /// </summary>
        private static void UpdateExistingActivityExitTime(int trackingId, DateTime exitTime)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();
                    // Only update if ExitTime is NULL to prevent overwriting genuine exit times
                    string updateQuery = "UPDATE UserActivityTracking SET ExitTime = @ExitTime WHERE Id = @TrackingId AND ExitTime IS NULL";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@ExitTime", exitTime);
                    updateCmd.Parameters.AddWithValue("@TrackingId", trackingId);
                    updateCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating ExitTime for TrackingId {trackingId}: {ex.Message}");
                // In a production app, you'd log this error more robustly.
            }
        }

        /// <summary>
        /// This is the primary method to call on Page_Load.
        /// It closes the previous page's activity and starts a new one for the current page.
        /// </summary>
        public static void TrackPageActivity(string currentPageName)
        {
            HttpContext currentContext = HttpContext.Current;

            if (currentContext.Session["UserId"] == null)
                return; // Not logged in, no tracking

            // Skip tracking for Dashboard and Admin roles
            if (currentPageName.Equals("Dashboard", StringComparison.OrdinalIgnoreCase))
                return;

            int userId = Convert.ToInt32(currentContext.Session["UserId"]);
            int roleId = currentContext.Session["RoleId"] != null ? Convert.ToInt32(currentContext.Session["RoleId"]) : 1;

            if (roleId == 2) // Skip tracking for admin users (RoleId = 2)
                return;

            string sessionId = currentContext.Session.SessionID;
            DateTime now = DateTime.Now; // Capture current time once for consistency

            try
            {
                // STEP 1: Handle ExitTime for the PREVIOUSLY visited page (if any)
                // We'll store the ID of the last active tracking entry in a single session variable
                object lastActivityIdObj = currentContext.Session["CurrentActivityTrackingId"];

                if (lastActivityIdObj != null)
                {
                    int lastActivityId = Convert.ToInt32(lastActivityIdObj);
                    UpdateExistingActivityExitTime(lastActivityId, now); // Set previous page's ExitTime
                }

                // STEP 2: Insert new tracking record for the CURRENT page entry
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Fetch UserCreatedAt (existing logic, kept as is)
                    string getUserQuery = "SELECT CreatedAt FROM UserDetails WHERE Id = @UserId";
                    SqlCommand getUserCmd = new SqlCommand(getUserQuery, conn);
                    getUserCmd.Parameters.AddWithValue("@UserId", userId);
                    DateTime userCreatedAt = DateTime.Now;
                    object result = getUserCmd.ExecuteScalar();
                    if (result != null)
                    {
                        userCreatedAt = Convert.ToDateTime(result);
                    }

                    // --- IMPORTANT CHANGE HERE: ADD ExitTime WITH NULL VALUE ---
                    string insertQuery = @"
                        INSERT INTO UserActivityTracking 
                        (UserId, Username, Email, UserCreatedAt, PageName, EntryTime, ExitTime, SessionId)
                        VALUES (@UserId, @Username, @Email, @UserCreatedAt, @PageName, @EntryTime, NULL, @SessionId);
                        SELECT SCOPE_IDENTITY(); -- Get the ID of the newly inserted row";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@UserId", userId);
                    insertCmd.Parameters.AddWithValue("@Username", currentContext.Session["UserName"] ?? "Unknown");
                    insertCmd.Parameters.AddWithValue("@Email", currentContext.Session["Email"] ?? "Unknown@example.com");
                    insertCmd.Parameters.AddWithValue("@UserCreatedAt", userCreatedAt);
                    insertCmd.Parameters.AddWithValue("@PageName", currentPageName);
                    insertCmd.Parameters.AddWithValue("@EntryTime", now);
                    insertCmd.Parameters.AddWithValue("@SessionId", sessionId);

                    // Get the ID of the newly inserted record
                    int newActivityId = Convert.ToInt32(insertCmd.ExecuteScalar());

                    // Store this new ID in session for the next page load (to become the 'previous' one)
                    currentContext.Session["CurrentActivityTrackingId"] = newActivityId;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error tracking page activity for {currentPageName}: {ex.Message}");
            }
        }

        /// <summary>
        /// This method is specifically for Session_End or explicit Logout.
        /// It ensures the ExitTime for the very last active page is set.
        /// </summary>
        public static void TrackCurrentPageExitOnSessionEnd()
        {
            HttpContext currentContext = HttpContext.Current;
            if (currentContext.Session["CurrentActivityTrackingId"] != null)
            {
                int trackingId = Convert.ToInt32(currentContext.Session["CurrentActivityTrackingId"]);
                UpdateExistingActivityExitTime(trackingId, DateTime.Now);
                currentContext.Session.Remove("CurrentActivityTrackingId"); // Clear after use
            }
        }

        // --- REMOVE THESE METHODS IF THEY STILL EXIST ---
        // public static void TrackPageExit(string pageName) { ... }
        // public static void TrackAllPageExits() { ... }
        // private static int GetLastTrackingId(...) { ... }
    }
}