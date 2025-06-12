<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Project_Trio.AdminPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<style>
 /* Admin Navbar Base Styles */
.admin-navbar {
    background-color: #333;
    color: white;
    padding: 10px 20px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
    font-family: Arial, sans-serif;
}

.admin-welcome {
    font-size: 16px;
    font-weight: 500;
}

.admin-username {
    color: #4CAF50;
    font-weight: bold;
}

/* Dropdown Container */
.dropdown-container {
    position: relative;
    display: inline-block;
}

/* Admin Icon Button */
.admin-icon {
    cursor: pointer;
    width: 30px;
    height: 30px;
    border-radius: 50%;
    transition: all 0.3s ease;
    border: 2px solid transparent;
}

.admin-icon:hover {
    background-color: rgba(255,255,255,0.1);
    border-color: rgba(255,255,255,0.3);
    transform: scale(1.05);
}

/* Enhanced Admin Menu Dropdown */
.admin-menu {
    position: absolute;
    right: 0;
    top: 100%;
    margin-top: 8px;
    background-color: #ffffff;
    color: #000000;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 0;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    min-width: 180px;
    z-index: 1000;
    overflow: hidden;
    animation: dropdownFadeIn 0.2s ease-out;
}

/* Dropdown Animation */
@keyframes dropdownFadeIn {
    from {
        opacity: 0;
        transform: translateY(-10px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}

/* Arrow pointer for dropdown */
.admin-menu::before {
    content: '';
    position: absolute;
    top: -8px;
    right: 15px;
    width: 0;
    height: 0;
    border-left: 8px solid transparent;
    border-right: 8px solid transparent;
    border-bottom: 8px solid #ddd;
}

.admin-menu::after {
    content: '';
    position: absolute;
    top: -7px;
    right: 15px;
    width: 0;
    height: 0;
    border-left: 8px solid transparent;
    border-right: 8px solid transparent;
    border-bottom: 8px solid #ffffff;
}

/* Menu Header (Optional) */
.menu-header {
    background-color: #f8f9fa;
    padding: 12px 16px;
    border-bottom: 1px solid #eee;
    font-weight: bold;
    color: #333;
    font-size: 14px;
}

/* Enhanced Menu Links */
.admin-menu-link {
    display: block;
    margin: 0;
    padding: 12px 16px;
    line-height: 1.4;
    text-decoration: none;
    color: #333;
    font-size: 14px;
    transition: all 0.2s ease;
    border: none;
    background: none;
    width: 100%;
    text-align: left;
    cursor: pointer;
    position: relative;
}

/* Hover Effects */
.admin-menu-link:hover {
    background-color: #f5f5f5;
    color: #007bff;
    text-decoration: none;
    padding-left: 20px;
}

.admin-menu-link:active {
    background-color: #e9ecef;
}

/* Focus state for accessibility */
.admin-menu-link:focus {
    outline: 2px solid #007bff;
    outline-offset: -2px;
    background-color: #e7f3ff;
}

/* Add icons to menu items (optional) */
.admin-menu-link::before {
    content: '▶';
    opacity: 0;
    margin-right: 8px;
    transition: opacity 0.2s ease;
    color: #007bff;
}

.admin-menu-link:hover::before {
    opacity: 1;
}

/* Specific styling for Dashboard */
.admin-menu-link[href*="Dashboard"]::before {
    content: '📊';
    opacity: 1;
}

/* Specific styling for User Activity */
.admin-menu-link[href*="UserActivity"]::before {
    content: '👥';
    opacity: 1;
}

/* Divider between menu items */
.admin-menu-link:not(:last-child) {
    border-bottom: 1px solid #f0f0f0;
}

/* Responsive Design */
@media (max-width: 768px) {
    .admin-navbar {
        padding: 8px 12px;
        flex-direction: column;
        align-items: stretch;
        gap: 10px;
    }
    
    .admin-menu {
        right: 12px;
        min-width: 160px;
    }
    
    .admin-menu-link {
        padding: 10px 12px;
        font-size: 13px;
    }
}

/* Dark mode support (optional) */
@media (prefers-color-scheme: dark) {
    .admin-menu {
        background-color: #2d3748;
        border-color: #4a5568;
        color: #e2e8f0;
    }
    
    .admin-menu::after {
        border-bottom-color: #2d3748;
    }
    
    .menu-header {
        background-color: #1a202c;
        color: #e2e8f0;
        border-bottom-color: #4a5568;
    }
    
    .admin-menu-link {
        color: #e2e8f0;
    }
    
    .admin-menu-link:hover {
        background-color: #4a5568;
        color: #90cdf4;
    }
    
    .admin-menu-link:not(:last-child) {
        border-bottom-color: #4a5568;
    }
}

/* Loading state for menu items */
.admin-menu-link.loading {
    opacity: 0.6;
    pointer-events: none;
}

.admin-menu-link.loading::after {
    content: '...';
    margin-left: 5px;
    animation: loading 1s infinite;
}

@keyframes loading {
    0%, 20% { opacity: 0; }
    50% { opacity: 1; }
    100% { opacity: 0; }
}
 .admin-menu-link:hover,
 #lnkDashboardBottom:hover,
 #lnkUserActivityBottom:hover {
     background-color: #555 !important;
     color: #fff !important;
     cursor: pointer;
 }
</style>

</head>

<body>
    <form id="form1" runat="server">
        <!-- Admin Navbar (Top) -->
        <div style="background-color:#333; color:white; padding:10px; display:flex; align-items:center; justify-content:space-between;">
            <div>
                Welcome, <asp:Label ID="lblAdminUsername" runat="server" Text="AdminUser"></asp:Label>
            </div>
            <div style="position:relative;">
                <asp:ImageButton ID="imgAdminIcon" runat="server" ImageUrl="~/Images/user-gear.png" 
                    OnClick="imgAdminIcon_Click" ToolTip="Admin Menu" Style="cursor:pointer; width:30px; height:30px;" />

                <asp:Panel ID="pnlAdminMenu" runat="server" Visible="false" CssClass="admin-menu" 
                    style="position:absolute; right:0; background:#444; border-radius:4px; padding:8px; margin-top:5px; box-shadow: 0 2px 6px rgba(0,0,0,0.3);">
                    <asp:LinkButton ID="lnkDashboard" runat="server" OnClick="lnkDashboard_Click" CssClass="admin-menu-link" 
                        style="display:block; color:#fff; padding:6px 12px; text-decoration:none;">
                        Dashboard
                    </asp:LinkButton>

                    <asp:LinkButton ID="lnkUserActivity" runat="server" OnClick="lnkUserActivity_Click" CssClass="admin-menu-link"
                        style="display:block; color:#fff; padding:6px 12px; text-decoration:none;">
                        UserActivity
                    </asp:LinkButton>
                </asp:Panel>
            </div>
        </div>



        <!-- Admin Summary above bottom nav -->
        <div style="background:#222; color:#eee; padding:15px 20px; font-size:14px; text-align:center;">
            <span>System Status: <strong>All systems operational</strong></span> &nbsp;&nbsp;|&nbsp;&nbsp;
            <span>New Users Today: <strong>12</strong></span> &nbsp;&nbsp;|&nbsp;&nbsp;
            <span>Active Sessions: <strong>5</strong></span>
        </div>

        <!-- Fixed Bottom Navigation -->
        <div style="position:fixed; bottom:0; left:0; right:0; background:#333; display:flex; justify-content:center; box-shadow: 0 -2px 6px rgba(0,0,0,0.4); z-index:1000;">
            <asp:LinkButton ID="lnkDashboardBottom" runat="server" OnClick="lnkDashboard_Click" 
                style="color:#fff; padding:15px 25px; display:inline-block; text-decoration:none; border-right:1px solid #555;">
                Dashboard
            </asp:LinkButton>
            <asp:LinkButton ID="lnkUserActivityBottom" runat="server" OnClick="lnkUserActivity_Click" 
                style="color:#fff; padding:15px 25px; display:inline-block; text-decoration:none;">
                User Activity
            </asp:LinkButton>
        </div>

       
        
    </form>
</body>
    </html>
