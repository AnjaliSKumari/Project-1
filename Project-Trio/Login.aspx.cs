//using System;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.Web;
//using System.Web.UI;

//namespace Project_Trio
//{
//    public partial class Login : Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
//        }

//        protected void btnLogin_Click(object sender, EventArgs e)
//        {
//            if (!Page.IsValid)
//                return;

//            string username = txtUsername.Text.Trim();
//            string password = txtPassword.Text.Trim(); // For production, use hashed password!

//            string connStr = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connStr))
//                {
//                    conn.Open();

//                    // Query to get user by username and password
//                    SqlCommand cmd = new SqlCommand(
//                        "SELECT Id, RoleId FROM UserDetails WHERE Username = @Username AND Password = @Password", conn);
//                    cmd.Parameters.AddWithValue("@Username", username);
//                    cmd.Parameters.AddWithValue("@Password", password);

//                    SqlDataReader reader = cmd.ExecuteReader();

//                    if (reader.Read())
//                    {
//                        int userId = Convert.ToInt32(reader["Id"]);
//                        int roleId = Convert.ToInt32(reader["RoleId"]);

//                        reader.Close();

//                        // Save user info in Session for future authorization
//                        Session["UserId"] = userId;
//                        Session["Username"] = username;
//                        Session["RoleId"] = roleId;

//                        // Redirect based on RoleId
//                        if (roleId == 2) // Admin
//                        {
//                            Response.Redirect("AdminPanel.aspx");
//                        }
//                        else // User
//                        {
//                            Response.Redirect("Home.aspx");
//                        }
//                    }
//                    else
//                    {
//                        lblMessage.Text = "Invalid username or password.";
//                        lblMessage.Visible = true;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                lblMessage.Text = "Login failed. Error: " + ex.Message;
//                lblMessage.Visible = true;
//            }
//        }
//    }
//}



//----new code
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace Project_Trio
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            // Track page entry only for logged-in users (not for initial login page visit)
            if (Session["UserId"] != null)
            {
                ActivityTracker.TrackPageEntry("Login");
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Track page exit
            if (Session["UserId"] != null)
            {
                ActivityTracker.TrackPageExit("Login");
            }
        }

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
                        "SELECT Id, RoleId FROM UserDetails WHERE Username = @Username AND Password = @Password", conn);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["Id"]);
                        int roleId = Convert.ToInt32(reader["RoleId"]);

                        reader.Close();

                        // Save user info in Session for future authorization
                        Session["UserId"] = userId;
                        Session["Username"] = username;
                        Session["RoleId"] = roleId;

                        // Track successful login
                        ActivityTracker.TrackPageEntry("Login");

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