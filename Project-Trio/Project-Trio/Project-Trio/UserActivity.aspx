<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserActivity.aspx.cs" Inherits="Project_Trio.UserActivity" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>User Activity - Project Trio</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">

    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet">

    <style>
        :root {
            --primary-color: #007bff; /* Bootstrap primary blue */
            --secondary-color: #6c757d; /* Bootstrap secondary grey */
            --success-color: #28a745; /* Bootstrap success green */
            --danger-color: #dc3545; /* Bootstrap danger red */
            --info-color: #17a2b8; /* Bootstrap info blue */
            --light-grey: #f8f9fa;
            --dark-grey: #343a40;
            --font-family: 'Poppins', sans-serif;
        }

        body {
            font-family: var(--font-family);
            background-color: var(--light-grey);
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }

        .container-custom {
            max-width: 1200px;
            margin: 0 auto;
            padding: 0 15px;
            flex-grow: 1; /* Allow content to grow */
        }

        /* Navbar Enhancements */
        .navbar {
            background: linear-gradient(to right, #6a11cb 0%, #2575fc 100%); /* Vibrant gradient */
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }

        .navbar-text strong {
            color: white; /* Welcome text */
            font-weight: 500;
        }

        .dropdown-toggle {
            background-color: rgba(255, 255, 255, 0.2); /* Transparent background for dropdown button */
            border: 1px solid rgba(255, 255, 255, 0.3);
            color: white;
            transition: background-color 0.3s ease, border-color 0.3s ease;
        }

        .dropdown-toggle:hover {
            background-color: rgba(255, 255, 255, 0.3);
            border-color: rgba(255, 255, 255, 0.5);
        }

        .dropdown-menu {
            border-radius: 0.5rem;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
            animation: fadeIn 0.2s ease-out;
        }

        .dropdown-item {
            font-size: 0.95rem;
            color: var(--dark-grey);
        }

        .dropdown-item:hover {
            background-color: var(--primary-color);
            color: white;
        }

        /* Main Content Header */
        h2 {
            color: #343a40; /* Darker text for headings */
            font-weight: 600;
            padding-top: 20px; /* Space from navbar */
        }

        /* Card Styling */
        .card {
            border: none;
            border-radius: 0.75rem;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.08);
            margin-bottom: 30px;
        }

        .card-header {
            background-color: white;
            border-bottom: 1px solid rgba(0, 0, 0, 0.05);
            font-weight: 600;
            font-size: 1.1rem;
            color: #343a40;
            padding: 1.25rem 1.5rem;
            border-top-left-radius: 0.75rem;
            border-top-right-radius: 0.75rem;
        }

        /* GridView / Table Styling */
        .table {
            margin-bottom: 0; /* Remove default margin from last element in card-body */
        }

        .table thead th {
            background-color: var(--primary-color);
            color: white;
            border-color: var(--primary-color);
            font-weight: 500;
            vertical-align: middle; /* Center text vertically */
        }

        .table tbody tr {
            transition: background-color 0.2s ease;
        }

        .table tbody tr:hover {
            background-color: rgba(0, 123, 255, 0.05); /* Light blue hover */
        }

        .table td, .table th {
            padding: 1rem; /* Increase padding for better readability */
            vertical-align: middle; /* Center text vertically */
        }

        .table-striped tbody tr:nth-of-type(odd) {
            background-color: rgba(0, 0, 0, 0.02); /* Very subtle stripe */
        }

        /* Form Control Styling */
        .form-control {
            border-radius: 0.3rem;
            border: 1px solid #ced4da;
            padding: 0.6rem 0.75rem;
            font-size: 0.95rem;
            transition: border-color 0.2s ease, box-shadow 0.2s ease;
        }

        .form-control:focus {
            border-color: var(--primary-color);
            box-shadow: 0 0 0 0.25rem rgba(0, 123, 255, 0.25);
        }

        .error-label {
            color: var(--danger-color);
            font-size: 0.85rem;
            margin-top: 5px;
            display: block; /* Ensure it takes full width */
        }

        /* Pagination Styling */
        .pagination .page-item .page-link {
            border-radius: 0.3rem !important; /* Override Bootstrap's default for a unified look */
            margin: 0 3px;
            border: 1px solid var(--primary-color);
            color: var(--primary-color);
            transition: background-color 0.2s ease, color 0.2s ease, border-color 0.2s ease;
        }

        .pagination .page-item .page-link:hover {
            background-color: var(--primary-color);
            color: white;
            border-color: var(--primary-color);
        }

        .pagination .page-item.active .page-link {
            background-color: var(--primary-color);
            border-color: var(--primary-color);
            color: white;
        }

        .pagination .page-item.disabled .page-link {
            color: #6c757d;
            border-color: #dee2e6;
            background-color: #e9ecef;
        }

        /* Button Styling (standard Bootstrap is good, but enhance) */
        .btn {
            border-radius: 0.3rem;
            font-weight: 500;
            padding: 0.5rem 1rem;
            transition: all 0.2s ease;
        }

        .btn-primary {
            background-color: var(--primary-color);
            border-color: var(--primary-color);
        }
        .btn-primary:hover {
             background-color: #0056b3; /* Darker shade on hover */
             border-color: #0056b3;
        }

        .btn-danger {
            background-color: var(--danger-color);
            border-color: var(--danger-color);
        }
        .btn-danger:hover {
            background-color: #c82333; /* Darker shade on hover */
            border-color: #c82333;
        }

        .btn-success {
            background-color: var(--success-color);
            border-color: var(--success-color);
        }
        .btn-success:hover {
             background-color: #218838; /* Darker shade on hover */
             border-color: #218838;
        }

        .btn-secondary {
            background-color: var(--secondary-color);
            border-color: var(--secondary-color);
        }
        .btn-secondary:hover {
             background-color: #545b62; /* Darker shade on hover */
             border-color: #545b62;
        }

        /* Alert Messages */
        .alert {
            border-radius: 0.5rem;
            padding: 1rem 1.25rem;
            margin-bottom: 1.5rem;
            font-size: 0.95rem;
            animation: slideInFromTop 0.5s ease-out; /* Optional: subtle entry animation */
        }

        .alert-success {
            color: #155724;
            background-color: #d4edda;
            border-color: #c3e6cb;
        }

        .alert-danger {
            color: #721c24;
            background-color: #f8d7da;
            border-color: #f5c6cb;
        }

        /* Animations */
        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(-10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @keyframes slideInFromTop {
            from { opacity: 0; transform: translateY(-20px); }
            to { opacity: 1; transform: translateY(0); }
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg mb-4">
            <div class="container-custom d-flex justify-content-between align-items-center">
                <div class="navbar-text">
                    Welcome <strong><asp:Label ID="lblUsername" runat="server" /></strong>
                </div>

                <div class="dropdown">
                    <a class="btn btn-secondary dropdown-toggle d-flex align-items-center" href="#" role="button" id="userDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                        <img src="https://cdn-icons-png.flaticon.com/512/149/149071.png" alt="Profile" class="rounded-circle" style="width:32px; height:32px; margin-right:8px;" />
                        Account
                    </a>
                    <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
                        <li><a class="dropdown-item" href="UserActivity.aspx">User Activity</a></li>
                        <li><a class="dropdown-item" href="Dashboard.aspx">Dashboard</a></li>
                        <li>
                            <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click" CssClass="dropdown-item">Logout</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-custom">
            <h2 class="mb-4">User Activity Management</h2>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" />
            
            <div class="card">
                <div class="card-header">
                    <h5>User List</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvUsers" runat="server" 
                        AutoGenerateColumns="false"
                        DataKeyNames="Id,Username,Email,Gender"
                        OnRowEditing="gvUsers_RowEditing"
                        OnRowUpdating="gvUsers_RowUpdating"
                        OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                        OnRowDeleting="gvUsers_RowDeleting"
                        OnPageIndexChanging="gvUsers_PageIndexChanging"
                        OnRowDataBound="gvUsers_RowDataBound"
                        AllowPaging="true"
                        PageSize="10"
                        CssClass="table table-striped table-hover table-responsive">
                        
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" />
                            
                            <asp:TemplateField HeaderText="Username">
                                <ItemTemplate>
                                    <asp:Label ID="lblUsername" runat="server" Text='<%# Bind("Username") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUsername" runat="server" Text='<%# Bind("Username") %>' CssClass="form-control" />
                                    <asp:Label ID="lblUsernameError" runat="server" CssClass="error-label" Visible="false" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control" />
                                    <asp:Label ID="lblEmailError" runat="server" CssClass="error-label" Visible="false" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Gender">
                                <ItemTemplate>
                                    <asp:Label ID="lblGender" runat="server" Text='<%# Bind("Gender") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Select Gender" Value="" />
                                        <asp:ListItem Text="Male" Value="Male" />
                                        <asp:ListItem Text="Female" Value="Female" />
                                        <asp:ListItem Text="Other" Value="Other" />
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="CreatedAt" HeaderText="Created Date" 
                                ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
                            
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" 
                                        CommandName="Edit" Text="Edit" CssClass="btn btn-primary btn-sm me-2" />
                                    <asp:LinkButton ID="lnkDelete" runat="server" 
                                        CommandName="Delete" Text="Delete" CssClass="btn btn-danger btn-sm"
                                        OnClientClick="return confirm('Are you sure you want to delete this user?');" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" runat="server" 
                                        CommandName="Update" Text="Update" CssClass="btn btn-success btn-sm me-2" />
                                    <asp:LinkButton ID="lnkCancel" runat="server" 
                                        CommandName="Cancel" Text="Cancel" CssClass="btn btn-secondary btn-sm" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        
                        <EmptyDataTemplate>
                            <div class="text-center p-4 text-muted">
                                No users found.
                            </div>
                        </EmptyDataTemplate>
                        
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" />
                        <PagerStyle CssClass="pagination justify-content-center mt-3" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="bg-primary text-white" />
                        <RowStyle CssClass="table-light" />
                        <AlternatingRowStyle CssClass="table-secondary" />
                        <SelectedRowStyle CssClass="table-info" />
                        <SortedAscendingHeaderStyle CssClass="bg-primary text-white" />
                        <SortedDescendingHeaderStyle CssClass="bg-primary text-white" />
                        <SortedAscendingCellStyle BackColor="#e1e1e1" />
                        <SortedDescendingCellStyle BackColor="#e1e1e1" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>

</body>
</html>

