<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Project_Trio.Login" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login Page</title>
    <link href="style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="form-container">
        <h2>Login</h2>

        <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqUsername" runat="server"
            ControlToValidate="txtUsername" ErrorMessage="* Username is required"></asp:RequiredFieldValidator>

        <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqPassword" runat="server"
            ControlToValidate="txtPassword" ErrorMessage="* Password is required"></asp:RequiredFieldValidator>

        <br /><br />
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
        <br /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

        <div class="signup-link-container">
            <p>Don't have an account? <a href="SignUp.aspx">Sign up here</a></p>
        </div>
    </form>
</body>
</html>

