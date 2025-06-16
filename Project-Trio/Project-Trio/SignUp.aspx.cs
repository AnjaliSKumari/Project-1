using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.IO; // IMPORTANT: Add this for Path.GetFileNameWithoutExtension

namespace Project_Trio
{
    public partial class SignUp : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            // --- START MODIFIED TRACKING LOGIC ---
            // This 'if (!IsPostBack)' block is crucial.
            // It ensures that ActivityTracker.TrackPageActivity is called only on the
            // initial GET request for the page, not on subsequent postbacks (e.g., button clicks).

            // NOTE: The ActivityTracker.TrackPageActivity method already checks if Session["UserId"] is null.
            // If UserId is null (as it would be for a guest on the SignUp page), the tracking call will
            // simply return without logging. This is the desired behavior for unauthenticated pages.
            if (!IsPostBack)
            {
                string currentPageName = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
                ActivityTracker.TrackPageActivity(currentPageName);
            }
            // --- END MODIFIED TRACKING LOGIC ---
        }

        // --- REMOVE THE ENTIRE Page_Unload METHOD ---
        // protected void Page_Unload(object sender, EventArgs e)
        // {
        //     // This method and its content should be completely removed.
        //     // The new ActivityTracker handles page exits automatically when navigating to a new page
        //     // or when the session ends.
        //     if (Session["UserId"] != null)
        //     {
        //         ActivityTracker.TrackPageExit("Signup");
        //     }
        // }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Only proceed if all validations pass
            if (!Page.IsValid)
            {
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string gender = ddlGender.SelectedValue;

            string connStr = ConfigurationManager.ConnectionStrings["UserConn"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Check for existing username or email
                    SqlCommand checkCmd = new SqlCommand("SELECT * FROM UserDetails WHERE Username = @Username OR Email = @Email", conn);
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    SqlDataReader reader = checkCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["Username"].ToString() == username && reader["Email"].ToString() == email)
                            {
                                lblMessage.Text = "This username and email is already taken!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Visible = true;
                            }
                            else if (reader["Username"].ToString() == username)
                            {
                                lblMessage.Text = "Username already exists with a different email!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Visible = true;
                            }
                            else if (reader["Email"].ToString() == email)
                            {
                                lblMessage.Text = "Email already registered with a different username!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                lblMessage.Visible = true;
                            }
                        }
                        reader.Close();
                        return;
                    }

                    reader.Close();

                    // Insert into database with CreatedAt timestamp
                    SqlCommand insertCmd = new SqlCommand("INSERT INTO UserDetails (Username, Email, Password, Gender, CreatedAt) VALUES (@Username, @Email, @Password, @Gender, @CreatedAt)", conn);
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", password); // Password hashing recommended for production
                    insertCmd.Parameters.AddWithValue("@Gender", gender);
                    insertCmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    int result = insertCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "Signup successful! Welcome, " + username;
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Visible = true;

                        // Redirect to Login.aspx with username and password in query string
                        string url = $"Login.aspx?username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}";
                        Response.Redirect(url);

                        // Optional: Clear form after successful signup
                        // ClearForm();
                    }
                    else
                    {
                        lblMessage.Text = "Error during signup. Please try again.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Signup failed. Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            // Clear all input fields
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirm.Text = "";
            ddlGender.SelectedIndex = 0; // Reset to "Select" option
            lblMessage.Text = "";
            lblMessage.Visible = false; // Hide the message label

            // Clear any validation errors
            Page.Validate();
            if (!Page.IsValid)
            {
                foreach (System.Web.UI.WebControls.BaseValidator validator in Page.Validators)
                {
                    validator.IsValid = true;
                }
            }
        }
    }
}