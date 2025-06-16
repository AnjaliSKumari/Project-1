using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.IO; // IMPORTANT: Add this for Path.GetFileNameWithoutExtension

namespace Project_Trio
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            // --- START MODIFIED TRACKING LOGIC ---
            // This 'if (!IsPostBack)' block is crucial.
            // It ensures that ActivityTracker.TrackPageActivity is called only on the
            // initial GET request for the page, not on subsequent postbacks.

            // IMPORTANT: For the Login page, Session["UserId"] will typically be NULL on initial load.
            // ActivityTracker.TrackPageActivity explicitly checks if Session["UserId"] is null and returns
            // if it is. This means that *unauthenticated* visits to the login page will NOT be tracked,
            // which is usually the desired behavior. Tracking for a user begins once they successfully log in
            // and are redirected to the *first authenticated page* (e.g., Home.aspx).
            if (!IsPostBack)
            {
                string currentPageName = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
                ActivityTracker.TrackPageActivity(currentPageName);
            }
            // --- END MODIFIED TRACKING LOGIC ---

            // If the user was redirected here after a successful signup,
            // pre-fill username and password for convenience.
            if (!IsPostBack && Request.QueryString["username"] != null && Request.QueryString["password"] != null)
            {
                txtUsername.Text = HttpUtility.UrlDecode(Request.QueryString["username"]);
                txtPassword.Text = HttpUtility.UrlDecode(Request.QueryString["password"]);
            }
        }

        // --- REMOVE THE ENTIRE Page_Unload METHOD ---
        // protected void Page_Unload(object sender, EventArgs e)
        // {
        //     // This method and its content should be completely removed.
        //     // The new ActivityTracker handles page exits automatically when navigating to a new page
        //     // or when the session ends.
        //     if (Session["UserId"] != null)
        //     {
        //         ActivityTracker.TrackPageExit("Login");
        //     }
        // }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim(); // For production, use hashed password!

            string connStr = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Query to get user by username and password
                    SqlCommand cmd = new SqlCommand(
                        "SELECT Id, RoleId, Username, Email FROM UserDetails WHERE Username = @Username AND Password = @Password", conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["Id"]);
                        int roleId = Convert.ToInt32(reader["RoleId"]);
                        string userEmail = reader["Email"].ToString(); // Fetch Email

                        reader.Close();

                        // Save user info in Session for future authorization
                        Session["UserId"] = userId;
                        Session["Username"] = username;
                        Session["Email"] = userEmail; // Store Email in session
                        Session["RoleId"] = roleId;

                        // --- IMPORTANT: REMOVE ActivityTracker.TrackPageEntry("Login"); HERE ---
                        // Activity tracking for the *first authenticated page* will automatically
                        // start when the user is redirected to Home.aspx or AdminPanel.aspx.
                        // We don't track login page activity for the logged-in user explicitly here.

                        // Redirect based on RoleId
                        if (roleId == 2) // Admin
                        {
                            Response.Redirect("AdminPanel.aspx");
                        }
                        else // User
                        {
                            Response.Redirect("Home.aspx");
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Invalid username or password.";
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Login failed. Error: " + ex.Message;
                lblMessage.Visible = true;
            }
        }
    }
}