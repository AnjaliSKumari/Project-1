using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Project_Trio
{
    public partial class Use : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["WebDbConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Id, Username, Email FROM UserCredentials";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewUse.DataSource = dt;
                GridViewUse.DataBind();
            }
        }

        protected void GridViewUse_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewUse.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridViewUse_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewUse.EditIndex = -1;
            BindGrid();
        }

        protected void GridViewUse_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridViewUse.DataKeys[e.RowIndex].Value);
            GridViewRow row = GridViewUse.Rows[e.RowIndex];

            string username = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();

            // Validate email format
            if (!IsValidEmail(email))
            {
                // Show error message using a Label or JavaScript alert (example here)
                // You can add a Label control in your .aspx page to show errors instead of alert
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid email format. Please enter a valid email.');", true);
                return; // Prevent update
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE UserCredentials SET Username=@Username, Email=@Email WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            GridViewUse.EditIndex = -1;
            BindGrid();
        }

        protected void GridViewUse_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridViewUse.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM UserCredentials WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }

        // Helper method to validate email
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Simple regex for email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        protected void GridViewUse_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Color the row when in edit mode
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(255, 243, 205); // light yellow (Bootstrap warning color)
            }
        }
    }
}
