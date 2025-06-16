//using System;
//using System.Configuration;
//using System.Data.SqlClient;

//namespace Project_Trio
//{
//    public partial class Home : System.Web.UI.Page
//    {
//        string connStr = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

//        protected void Page_Load(object sender, EventArgs e)
//        {


//            if (Session["UserId"] == null)
//            {
//                Response.Redirect("Login.aspx");
//            }

//            if (!IsPostBack)
//            {
//                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";
//            }
//        }

//        protected void imgUserIcon_Click(object sender, EventArgs e)
//        {
//            LoadUserProfile();
//            pnlProfile.Visible = true;
//        }

//        private void LoadUserProfile()
//        {
//            int userId = Convert.ToInt32(Session["UserId"]);
//            using (SqlConnection conn = new SqlConnection(connStr))
//            {
//                conn.Open();
//                SqlCommand cmd = new SqlCommand("SELECT Username, Email, Gender FROM UserDetails WHERE Id = @UserId", conn);
//                cmd.Parameters.AddWithValue("@UserId", userId);

//                SqlDataReader reader = cmd.ExecuteReader();
//                if (reader.Read())
//                {
//                    txtUsername.Text = reader["Username"].ToString();
//                    txtEmail.Text = reader["Email"].ToString();
//                    string gender = reader["Gender"].ToString();
//                    ddlGender.SelectedValue = gender == "Male" || gender == "Female" ? gender : "";
//                }
//                reader.Close();
//            }

//            SetEditing(false);
//            lblMessage.Visible = false;
//        }

//        private void SetEditing(bool isEditing)
//        {    txtUsername.ReadOnly = !isEditing;
//            txtEmail.ReadOnly = !isEditing;
//            ddlGender.Enabled = isEditing;
//            btnSave.Visible = isEditing;
//            btnCancel.Visible = isEditing;
//            btnEdit.Visible = !isEditing;
//        }

//        protected void btnEdit_Click(object sender, EventArgs e)
//        {
//            SetEditing(true);
//        }

//        protected void btnCancel_Click(object sender, EventArgs e)
//        {
//            LoadUserProfile(); // reload original data and disable editing
//        }

//        protected void btnSave_Click(object sender, EventArgs e)
//        {
//            int userId = Convert.ToInt32(Session["UserId"]);
//            string newUsername = txtUsername.Text.Trim();
//            string newEmail = txtEmail.Text.Trim();
//            string newGender = ddlGender.SelectedValue;

//            using (SqlConnection conn = new SqlConnection(connStr))
//            {
//                conn.Open();

//                // Check if username is already taken by another user
//                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE Username = @Username AND Id <> @UserId", conn);
//                checkCmd.Parameters.AddWithValue("@Username", newUsername);
//                checkCmd.Parameters.AddWithValue("@UserId", userId);
//                int existingCount = (int)checkCmd.ExecuteScalar();

//                if (existingCount > 0)
//                {
//                    lblMessage.Text = "Username already taken, please choose another.";
//                    lblMessage.ForeColor = System.Drawing.Color.Red;
//                    lblMessage.Visible = true;
//                    return; // Stop further execution
//                }

//                // Update the user details including username
//                SqlCommand cmd = new SqlCommand("UPDATE UserDetails SET Username = @Username, Email = @Email, Gender = @Gender WHERE Id = @UserId", conn);
//                cmd.Parameters.AddWithValue("@Username", newUsername);
//                cmd.Parameters.AddWithValue("@Email", newEmail);
//                cmd.Parameters.AddWithValue("@Gender", newGender);
//                cmd.Parameters.AddWithValue("@UserId", userId);

//                int rows = cmd.ExecuteNonQuery();

//                if (rows > 0)
//                {
//                    lblMessage.Text = "Profile updated successfully.";
//                    lblMessage.ForeColor = System.Drawing.Color.Green;

//                    // Update Session username and welcome label
//                    Session["Username"] = newUsername;
//                    lblWelcome.Text = newUsername;
//                }
//                else
//                {
//                    lblMessage.Text = "Failed to update profile.";
//                    lblMessage.ForeColor = System.Drawing.Color.Red;
//                }

//                lblMessage.Visible = true;
//            }

//            SetEditing(false);
//        }


//        // Override btnSave to add confirmation popup with JS
//        protected override void OnPreRender(EventArgs e)
//        {
//            base.OnPreRender(e);
//            if (btnSave.Visible)
//            {
//                btnSave.Attributes["onclick"] = "return confirm('Do you want to save the changes?');";
//            }
//        }
//    }
//}

//new code

using System;
using System.Configuration;
using System.Data.SqlClient;

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

            // Track home page entry
            ActivityTracker.TrackPageEntry("Home");

            if (!IsPostBack)
            {
                lblWelcome.Text = Session["Username"]?.ToString() ?? "User";
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // Track home page exit
            ActivityTracker.TrackPageExit("Home");
        }

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

                // Check if username is already taken by another user
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM UserDetails WHERE Username = @Username AND Id <> @UserId", conn);
                checkCmd.Parameters.AddWithValue("@Username", newUsername);
                checkCmd.Parameters.AddWithValue("@UserId", userId);
                int existingCount = (int)checkCmd.ExecuteScalar();

                if (existingCount > 0)
                {
                    lblMessage.Text = "Username already taken, please choose another.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    return; // Stop further execution
                }

                // Update the user details including username
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

                    // Update Session username and welcome label
                    Session["Username"] = newUsername;
                    lblWelcome.Text = newUsername;
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

        // Override btnSave to add confirmation popup with JS
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (btnSave.Visible)
            {
                btnSave.Attributes["onclick"] = "return confirm('Do you want to save the changes?');";
            }
        }

        // New method: Logout functionality with activity tracking
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Track all page exits before logout
            ActivityTracker.TrackAllPageExits();

            // Clear session
            Session.Clear();
            Session.Abandon();

            // Redirect to login
            Response.Redirect("Login.aspx");
        }

        // New method: Navigation to Dashboard with activity tracking
        protected void btnGoToDashboard_Click(object sender, EventArgs e)
        {
            // Track exit from home page
            ActivityTracker.TrackPageExit("Home");

            // Redirect to dashboard
            Response.Redirect("Dashboard.aspx");
        }
    }
}