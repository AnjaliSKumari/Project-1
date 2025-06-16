<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Project_Trio.Home" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home - Project Trio</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
        * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
    min-height: 100vh;
    color: #333;
}

.container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 20px;
}

/* Navbar Styles */
.navbar {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    padding: 15px 0;
    position: sticky;
    top: 0;
    z-index: 1000;
}

.navbar-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 20px;
}

.welcome-section {
    color: white;
    font-size: 16px;
    font-weight: 500;
    flex: 1; /* Takes up remaining space */
}

.welcome-section .username {
    font-weight: 600;
    margin-left: 5px;
}

/* Right side container for profile icon and logout */
.navbar-right {
    display: flex;
    align-items: center;
    gap: 15px; /* Space between elements */
}

.user-profile-btn {
    background: rgba(255, 255, 255, 0.2);
    border: 2px solid rgba(255, 255, 255, 0.3);
    border-radius: 50%;
    padding: 8px;
    transition: all 0.3s ease;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
}

.user-profile-btn:hover {
    background: rgba(255, 255, 255, 0.3);
    transform: scale(1.1);
    border-color: rgba(255, 255, 255, 0.5);
}

.user-icon {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    display: block;
}

.btn-logout {
    background: transparent;
    border: 2px solid rgba(255, 255, 255, 0.3);
    border-radius: 8px;
    color: white;
    font-size: 14px;
    cursor: pointer;
    padding: 8px 16px;
    font-weight: 500;
    transition: all 0.3s ease;
}

.btn-logout:hover {
    background: rgba(255, 255, 255, 0.2);
    border-color: rgba(255, 255, 255, 0.5);
    transform: translateY(-1px);
}

/* Main Content */
.main-content {
    padding: 40px 0;
}

/* Profile Panel Styles */
.profile-panel {
    background: white;
    border-radius: 12px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
    padding: 30px;
    margin: 20px auto;
    max-width: 500px;
    border: none;
    animation: slideDown 0.3s ease-out;
}

@keyframes slideDown {
    from {
        opacity: 0;
        transform: translateY(-20px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}

.profile-panel h3 {
    color: #2c3e50;
    font-size: 24px;
    font-weight: 600;
    margin-bottom: 25px;
    text-align: center;
    position: relative;
}

.profile-panel h3::after {
    content: '';
    position: absolute;
    bottom: -10px;
    left: 50%;
    transform: translateX(-50%);
    width: 50px;
    height: 3px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border-radius: 2px;
}

/* Form Styles */
.form-group {
    margin-bottom: 20px;
}

.form-label {
    display: block;
    color: #2c3e50;
    font-weight: 500;
    margin-bottom: 8px;
    font-size: 14px;
}

.form-control {
    width: 100%;
    padding: 12px 15px;
    border: 2px solid #e1e8ed;
    border-radius: 8px;
    font-size: 14px;
    transition: all 0.3s ease;
    background: #f8f9fa;
}

.form-control:focus {
    outline: none;
    border-color: #667eea;
    background: white;
    box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

/* Dropdown Styles */
select.form-control {
    cursor: pointer;
}

/* Button Styles */
.btn {
    padding: 12px 24px;
    border: none;
    border-radius: 8px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.3s ease;
    margin-right: 10px;
    margin-bottom: 10px;
    display: inline-block;
    text-decoration: none;
    text-align: center;
    min-width: 80px;
}

.btn-primary {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
}

.btn-primary:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.btn-success {
    background: linear-gradient(135deg, #56ab2f 0%, #a8e6cf 100%);
    color: white;
}

.btn-success:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 15px rgba(86, 171, 47, 0.4);
}

.btn-secondary {
    background: linear-gradient(135deg, #bdc3c7 0%, #2c3e50 100%);
    color: white;
}

.btn-secondary:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 15px rgba(44, 62, 80, 0.4);
}

.button-group {
    text-align: center;
    margin-top: 25px;
}

/* Success Message */
.success-message {
    background: linear-gradient(135deg, #d4edda 0%, #c3e6cb 100%);
    color: #155724;
    padding: 12px 15px;
    border-radius: 8px;
    border-left: 4px solid #28a745;
    font-weight: 500;
    margin-bottom: 20px;
    animation: fadeOut 9s forwards;
}

@keyframes fadeOut {
    0% { opacity: 1; visibility: visible; }
    80% { opacity: 1; visibility: visible; }
    99% { opacity: 0; visibility: visible; }
    100% { opacity: 0; visibility: hidden; }
}

/* Responsive Design */
@media (max-width: 768px) {
    .navbar-content {
        padding: 0 15px;
        flex-wrap: wrap;
        gap: 10px;
    }
    
    .welcome-section {
        font-size: 14px;
    }
    
    .navbar-right {
        gap: 10px;
    }
    
    .user-icon {
        width: 25px;
        height: 18px;
    }
    
    .user-profile-btn {
        padding: 6px;
    }
    
    .btn-logout {
        padding: 6px 12px;
        font-size: 12px;
    }
    
    .profile-panel {
        margin: 20px 15px;
        padding: 20px;
    }
    
    .profile-panel h3 {
        font-size: 20px;
    }
    
    .container {
        padding: 0 15px;
    }
}

@media (max-width: 480px) {
    .navbar-content {
        flex-direction: column;
        align-items: stretch;
        text-align: center;
    }
    
    .welcome-section {
        margin-bottom: 10px;
    }
    
    .navbar-right {
        justify-content: center;
        margin-top: 10px;
    }
    
    .profile-panel {
        margin: 15px 10px;
        padding: 15px;
    }
    
    .btn {
        width: 100%;
        margin-right: 0;
        margin-bottom: 10px;
    }
    
    .button-group .btn {
        width: auto;
        margin-right: 5px;
        min-width: 100px;
    }
}

/* Additional Utility Styles */
.text-center {
    text-align: center;
}

.mt-3 {
    margin-top: 1rem;
}

.mb-3 {
    margin-bottom: 1rem;
}

.p-3 {
    padding: 1rem;
}

/* Focus states for accessibility */
.user-profile-btn:focus,
.btn:focus,
.form-control:focus {
    outline: 2px solid #667eea;
    outline-offset: 2px;
}

/* Loading state for buttons */
.btn:disabled {
    opacity: 0.6;
    cursor: not-allowed;
    transform: none;
}

.btn:disabled:hover {
    transform: none;
    box-shadow: none;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navbar -->
        <nav class="navbar">
            <div class="navbar-content">
                <div class="welcome-section">
                    Welcome,<span class="username"><asp:Label ID="lblWelcome" runat="server" Text="User"></asp:Label></span>
                </div>
                <div class="user-profile-btn">
                    <asp:ImageButton ID="imgUserIcon" runat="server" 
                        ImageUrl="~/Images/user.png" 
                        OnClick="imgUserIcon_Click" 
                        ToolTip="View Profile" 
                        CssClass="user-icon" />
                </div>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn-logout" OnClick="btnLogout_Click" />
            </div>
        </nav>

        <!-- Main Content -->
        <div class="main-content">
            <div class="container">
                <!-- Profile Panel -->
                <asp:Panel ID="pnlProfile" runat="server" Visible="false" CssClass="profile-panel">
                    <h3>Your Profile</h3>
                    
                    <asp:Label ID="lblMessage" runat="server" 
                        ForeColor="Green" 
                        Visible="false" 
                        CssClass="success-message"></asp:Label>
                    
                    <div class="form-group">
                        <asp:Label ID="lblUsername" runat="server" 
                            Text="Username:" 
                            CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtUsername" runat="server" 
                            CssClass="form-control" 
                            placeholder="Enter your username" />
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblEmail" runat="server" 
                            Text="Email Address:" 
                            CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" 
                            CssClass="form-control" 
                            placeholder="Enter your email address"
                            TextMode="Email" />
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblGender" runat="server" 
                            Text="Gender:" 
                            CssClass="form-label"></asp:Label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Select Gender" Value="" />
                            <asp:ListItem Text="Male" Value="Male" />
                            <asp:ListItem Text="Female" Value="Female" />
                            <asp:ListItem Text="Other" Value="Other" />
                            <asp:ListItem Text="Prefer not to say" Value="PreferNotToSay" />
                        </asp:DropDownList>
                    </div>
                    
                    <div class="button-group">
                        <asp:Button ID="btnEdit" runat="server" 
                            Text="Edit Profile" 
                            OnClick="btnEdit_Click" 
                            CssClass="btn btn-primary" />
                        <asp:Button ID="btnSave" runat="server" 
                            Text="Save Changes" 
                            OnClick="btnSave_Click" 
                            Visible="false" 
                            CssClass="btn btn-success" />
                        <asp:Button ID="btnCancel" runat="server" 
                            Text="Cancel" 
                            OnClick="btnCancel_Click" 
                            Visible="false" 
                            CssClass="btn btn-secondary" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
