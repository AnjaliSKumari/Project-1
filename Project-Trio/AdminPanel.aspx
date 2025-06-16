<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Project_Trio.AdminPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <!-- Admin Navbar -->
<div style="background-color:#333; color:white; padding:10px; display:flex; align-items:center; justify-content:space-between;">
    <div>
        Welcome, <asp:Label ID="lblAdminUsername" runat="server" Text="AdminUser"></asp:Label>
    </div>
    <div style="position:relative;">
        <asp:ImageButton ID="imgAdminIcon" runat="server" ImageUrl="~/Images/user-gear.png" 
            OnClick="imgAdminIcon_Click" ToolTip="Admin Menu" Style="cursor:pointer; width:30px; height:30px;" />

        <asp:Panel ID="pnlAdminMenu" runat="server" Visible="false" 
            Style="position:absolute; right:0; background:white; color:black; border:1px solid #ccc; padding:10px;">
            <asp:LinkButton ID="lnkDashboard" runat="server" OnClick="lnkDashboard_Click" Style="display:block; margin-bottom:5px;">Dashboard</asp:LinkButton>
            <asp:LinkButton ID="lnkUserActivity" runat="server" OnClick="lnkUserActivity_Click" Style="display:block;">User Activity</asp:LinkButton>
        </asp:Panel>
    </div>
</div>

    </form>
</body>
</html>
