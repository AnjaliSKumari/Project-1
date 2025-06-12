<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Project_Trio.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navbar -->
<div style="background-color:#007bff; padding:10px; color:white; display:flex; justify-content:space-between; align-items:center;">
    <div>
        Welcome, <asp:Label ID="lblWelcome" runat="server" Text="User"></asp:Label>
    </div>
    <div>
        <asp:ImageButton ID="imgUserIcon" runat="server" ImageUrl="~/Images/user.png" 
            OnClick="imgUserIcon_Click" ToolTip="View Profile" Style="cursor:pointer; width:30px; height:30px;" />
    </div>
</div>

<!-- Profile Panel, hidden by default -->
<asp:Panel ID="pnlProfile" runat="server" Visible="false" Style="border:1px solid #ccc; padding:15px; margin-top:15px; max-width:400px;">
    <h3>Your Profile</h3>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>

    <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label><br />
    <asp:TextBox ID="txtUsername" runat="server"  /><br /><br />

    <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label><br />
    <asp:TextBox ID="txtEmail" runat="server" /><br /><br />

    <asp:Label ID="lblGender" runat="server" Text="Gender:"></asp:Label><br />
    <asp:DropDownList ID="ddlGender" runat="server">
        <asp:ListItem Text="Male" Value="Male" />
        <asp:ListItem Text="Female" Value="Female" />
    </asp:DropDownList><br /><br />

    <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" />
    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Visible="false" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
</asp:Panel>

    </form>
</body>
</html>
