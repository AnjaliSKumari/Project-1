<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Project_Trio.SignUp" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <link href="style.css" rel="stylesheet" />
    <title>Signup Page</title>
</head>
<body>
    <form id="form1" runat="server" class="form-container">
        <h2>Signup</h2>
        
        <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqUsername" runat="server" 
            ControlToValidate="txtUsername" ErrorMessage="* Username is required"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regUsername" runat="server"
            ControlToValidate="txtUsername"
            ValidationExpression="^.{4,}$"
            ErrorMessage="Username must be at least 4 characters long"></asp:RegularExpressionValidator>
        
        <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
        <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqEmail" runat="server" 
            ControlToValidate="txtEmail" ErrorMessage="* Email is required"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regEmail" runat="server" 
            ControlToValidate="txtEmail"
            ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
            ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
        
        <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqPassword" runat="server" 
            ControlToValidate="txtPassword" ErrorMessage="* Password is required"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regPassword" runat="server"
            ControlToValidate="txtPassword"
            ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$"
            ErrorMessage="Password must be at least 6 chars and contain letters & numbers"></asp:RegularExpressionValidator>
        
        <asp:Label ID="lblConfirm" runat="server" Text="Confirm Password:"></asp:Label>
        <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="reqConfirm" runat="server" 
            ControlToValidate="txtConfirm" ErrorMessage="* Please confirm your password"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="compPassword" runat="server"
            ControlToCompare="txtPassword" ControlToValidate="txtConfirm"
            ErrorMessage="Passwords do not match"></asp:CompareValidator>
        
        <asp:Label ID="lblGender" runat="server" Text="Gender:"></asp:Label>
        <asp:DropDownList ID="ddlGender" runat="server">
            <asp:ListItem Text="Select" Value="" />
            <asp:ListItem Text="Male" Value="Male" />
            <asp:ListItem Text="Female" Value="Female" />
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="reqGender" runat="server" 
            ControlToValidate="ddlGender" ErrorMessage="* Please select your gender"
            InitialValue=""></asp:RequiredFieldValidator>
        
        <br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Signup" OnClick="btnSubmit_Click" />
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" 
            CausesValidation="false" />
        <br /><br />
        
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

         <div class="login-link-container">
            <p>Already have an account? <a href="Login.aspx">Login here</a></p>
        </div>
    </form>
</body>
</html>