<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserActivity.aspx.cs" Inherits="Project_Trio.UserActivity" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Activity Dashboard</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.0/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Bootstrap Icons -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.10.0/font/bootstrap-icons.min.css" rel="stylesheet" />
    
    <style>
        body {
            background-color: #f8f9fa;
        }
        .dashboard-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 2rem 0;
            margin-bottom: 2rem;
        }
        .card {
            box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
            border: none;
        }
        .edit-row {
            background-color: #e7f3ff !important;
        }
        .edit-row td {
            background-color: #e7f3ff !important;
        }
        .error-label {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 0.25rem;
            display: block;
        }
        .table th {
            background-color: #f8f9fa;
            border-top: none;
            font-weight: 600;
        }
        .btn-action {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
            margin: 0 0.125rem;
        }
        .alert {
            border: none;
            border-radius: 0.5rem;
        }
    </style>

    <script type="text/javascript">
        function confirmEdit() {
            return confirm('Do you want to edit this user?');
        }
        function confirmUpdate() {
            return confirm('Do you want to save changes?');
        }
        function confirmDelete() {
            return confirm('Are you sure you want to delete this user?');
        }
    </script>
</head>
<body>
    <!-- Dashboard Header -->
    <div class="dashboard-header">
        <div class="container">
            <div class="row align-items-center">
                <div class="col">
                    <h1 class="mb-0">
                        <i class="bi bi-people-fill me-3"></i>
                        User Activity Dashboard
                    </h1>
                    <p class="mb-0 mt-2 opacity-75">Manage and monitor user accounts</p>
                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <form id="form1" runat="server">
            <!-- Alert Message -->
            <div class="row mb-4">
                <div class="col-12">
                    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-success d-none" />
                </div>
            </div>

            <!-- Main Content Card -->
            <div class="card">
                <div class="card-header bg-white py-3">
                    <div class="row align-items-center">
                        <div class="col">
                            <h5 class="mb-0">
                                <i class="bi bi-table me-2"></i>
                                User Management
                            </h5>
                        </div>
                        <div class="col-auto">
                            <span class="badge bg-primary">Active Users</span>
                        </div>
                    </div>
                </div>
                
                <div class="card-body p-0">
                    <asp:GridView ID="gvUsers" runat="server" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="Id"
                        OnRowEditing="gvUsers_RowEditing"
                        OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                        OnRowUpdating="gvUsers_RowUpdating"
                        OnRowDeleting="gvUsers_RowDeleting"
                        OnPageIndexChanging="gvUsers_PageIndexChanging"
                        OnRowDataBound="gvUsers_RowDataBound"
                        AllowPaging="true" 
                        PageSize="5"
                        CssClass="table table-hover mb-0"
                        HeaderStyle-CssClass="table-header"
                        PagerStyle-CssClass="d-flex justify-content-center py-3"
                        PagerSettings-Mode="NumericFirstLast"
                        PagerSettings-FirstPageText="<i class='bi bi-chevron-double-left'></i>"
                        PagerSettings-LastPageText="<i class='bi bi-chevron-double-right'></i>"
                        PagerSettings-PreviousPageText="<i class='bi bi-chevron-left'></i>"
                        PagerSettings-NextPageText="<i class='bi bi-chevron-right'></i>">
                        
                        <Columns>
                            <asp:TemplateField HeaderText="Username" SortExpression="Username">
                                <ItemTemplate>
                                    <div class="d-flex align-items-center">
                                        <i class="bi bi-person-circle me-2 text-muted"></i>
                                        <strong><%# Eval("Username") %></strong>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUsername" runat="server" 
                                        Text='<%# Bind("Username") %>' 
                                        CssClass="form-control form-control-sm"></asp:TextBox>
                                    <asp:Label ID="lblUsernameError" runat="server" 
                                        CssClass="error-label" 
                                        Visible="false"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Email" SortExpression="Email">
                                <ItemTemplate>
                                    <div class="d-flex align-items-center">
                                        <i class="bi bi-envelope me-2 text-muted"></i>
                                        <%# Eval("Email") %>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEmail" runat="server" 
                                        Text='<%# Bind("Email") %>' 
                                        CssClass="form-control form-control-sm"
                                        type="email"></asp:TextBox>
                                    <asp:Label ID="lblEmailError" runat="server" 
                                        CssClass="error-label" 
                                        Visible="false"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Gender" SortExpression="Gender">
                                <ItemTemplate>
                                    <span class="badge bg-secondary">
                                        <%# Eval("Gender") %>
                                    </span>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGender" runat="server" 
                                        CssClass="form-select form-select-sm">
                                        <asp:ListItem Value="Male">Male</asp:ListItem>
                                        <asp:ListItem Value="Female">Female</asp:ListItem>
                                        <asp:ListItem Value="Other">Other</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Password">
                                <ItemTemplate>
                                    <div class="d-flex align-items-center">
                                        <i class="bi bi-lock me-2 text-muted"></i>
                                        <span class="font-monospace text-muted">••••••••</span>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <div class="btn-group" role="group">
                                        <asp:LinkButton ID="lnkEdit" runat="server" 
                                            CommandName="Edit" 
                                            Text="<i class='bi bi-pencil'></i> Edit" 
                                            CssClass="btn btn-outline-primary btn-action"
                                            OnClientClick="return confirmEdit();"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" 
                                            CommandName="Delete" 
                                            Text="<i class='bi bi-trash'></i> Delete" 
                                            CssClass="btn btn-outline-danger btn-action"
                                            OnClientClick="return confirmDelete();"></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="btn-group" role="group">
                                        <asp:LinkButton ID="lnkUpdate" runat="server" 
                                            CommandName="Update" 
                                            Text="<i class='bi bi-check-lg'></i> Save" 
                                            CssClass="btn btn-success btn-action"
                                            OnClientClick="return confirmUpdate();"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkCancel" runat="server" 
                                            CommandName="Cancel" 
                                            Text="<i class='bi bi-x-lg'></i> Cancel" 
                                            CssClass="btn btn-secondary btn-action"></asp:LinkButton>
                                    </div>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>
                            <div class="text-center py-5">
                                <i class="bi bi-inbox display-1 text-muted"></i>
                                <h5 class="mt-3 text-muted">No users found</h5>
                                <p class="text-muted">There are no users to display at the moment.</p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </form>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.0/js/bootstrap.bundle.min.js"></script>
    
    <script type="text/javascript">
        // Show alert messages with Bootstrap styling
        document.addEventListener('DOMContentLoaded', function() {
            var messageLabel = document.getElementById('<%= lblMessage.ClientID %>');
            if (messageLabel && messageLabel.innerText.trim() !== '') {
                messageLabel.classList.remove('d-none');
                messageLabel.classList.add('alert', 'alert-success');

                // Auto-hide after 5 seconds
                setTimeout(function () {
                    messageLabel.style.transition = 'opacity 0.5s';
                    messageLabel.style.opacity = '0';
                    setTimeout(function () {
                        messageLabel.classList.add('d-none');
                    }, 500);
                }, 5000);
            }
        });
    </script>
</body>
</html>