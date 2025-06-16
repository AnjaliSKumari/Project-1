using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO; // IMPORTANT: Add this for Path.GetFileNameWithoutExtension
using System.Web.UI; // Already there, but ensures it's explicit

namespace Project_Trio
{
    public partial class Home : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // --- CRITICAL CHANGE: Tracking should be inside !IsPostBack ---
            // This ensures activity is logged only when the page is first loaded (GET request),
            // not on subsequent postbacks (like clicking buttons on the page).
            if (!IsPostBack)
            {
                string currentPageName = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
                ActivityTracker.TrackPageActivity(currentPageName);

                // Your existing initial setup for lblWelcome.Text
                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";
            }
        }

        // --- REMOVE THE ENTIRE Page_Unload METHOD ---
        // The Page_Unload method and any calls to TrackPageExit are no longer needed here.
        // ActivityTracker.TrackPageActivity handles closing the previous page's activity
        // when a new page is loaded, and Session_End in Global.asax handles session expiry.
        // protected void Page_Unload(object sender, EventArgs e)
        // {
        //     // ActivityTracker.TrackPageExit("Home"); // This line caused the 0-second issues
        // }

        protected void imgUserIcon_Click(object sender, EventArgs e)
        {
            LoadUserProfile();
            pnlProfile.Visible = true;
        }

        private void LoadUserProfile()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Username, Email, Gender FROM UserDetails WHERE Id = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtUsername.Text = reader["Username"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    string gender = reader["Gender"].ToString();
                    ddlGender.SelectedValue = gender == "Male" || gender == "Female" ? gender : "";
                }
                reader.Close();
            }

            SetEditing(false);
            lblMessage.Visible = false;
        }

        private void SetEditing(bool isEditing)
        {
            txtUsername.ReadOnly = !isEditing;
            txtEmail.ReadOnly = !isEditing;
            ddlGender.Enabled = isEditing;
            btnSave.Visible = isEditing;
            btnCancel.Visible = isEditing;
            btnEdit.Visible = !isEditing;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            SetEditing(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadUserProfile(); // reload original data and disable editing
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string newUsername = txtUsername.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newGender = ddlGender.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE Username = @Username AND Id <> @UserId", conn);
                checkCmd.Parameters.AddWithValue("@Username", newUsername);
                checkCmd.Parameters.AddWithValue("@UserId", userId);
                int existingCount = (int)checkCmd.ExecuteScalar();

                if (existingCount > 0)
                {
                    lblMessage.Text = "Username already taken, please choose another.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    return;
                }

                SqlCommand cmd = new SqlCommand("UPDATE UserDetails SET Username = @Username, Email = @Email, Gender = @Gender WHERE Id = @UserId", conn);
                cmd.Parameters.AddWithValue("@Username", newUsername);
                cmd.Parameters.AddWithValue("@Email", newEmail);
                cmd.Parameters.AddWithValue("@Gender", newGender);
                cmd.Parameters.AddWithValue("@UserId", userId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    lblMessage.Text = "Profile updated successfully.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;

                    Session["Username"] = newUsername; // Update session with new username
                    lblWelcome.Text = newUsername; // Update welcome label
                }
                else
                {
                    lblMessage.Text = "Failed to update profile.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }

                lblMessage.Visible = true;
            }

            SetEditing(false);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // This tracks the exit of the LAST active page for the current session.
            ActivityTracker.TrackCurrentPageExitOnSessionEnd();

            Session.Clear();
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("Login.aspx");
        }

        protected void btnGoToDashboard_Click(object sender, EventArgs e)
        {
            // No explicit tracking needed here; Dashboard.aspx's Page_Load will handle it.
            Response.Redirect("Dashboard.aspx");
        }
    }
}