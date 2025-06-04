using System;
using System.Web;

namespace Project_Trio
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Application startup code
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session start code
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Track all page exits when session ends
            try
            {
                if (Session["UserId"] != null)
                {
                    ActivityTracker.TrackAllPageExits();
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                System.Diagnostics.Debug.WriteLine($"Error in Session_End: {ex.Message}");
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // Application end code
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Global error handling
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                System.Diagnostics.Debug.WriteLine($"Application Error: {ex.Message}");
            }
        }
    }
}