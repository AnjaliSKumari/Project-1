<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Project_Trio.Dashboard" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Admin Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        body {
            padding: 0;
            background-color: #f0f2f5;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        
        /* Navigation Styles */
        .navbar-custom {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            padding: 15px 0;
        }
        
        .navbar-brand {
            font-weight: bold;
            font-size: 1.5rem;
            color: white !important;
        }
        
        .nav-item {
            margin: 0 10px;
        }
        
        .nav-link {
            color: white !important;
            font-weight: 500;
            padding: 10px 20px !important;
            border-radius: 25px;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        
        .nav-link:hover {
            background-color: rgba(255, 255, 255, 0.2);
            color: white !important;
            transform: translateY(-2px);
        }
        
        .nav-link.active {
            background-color: rgba(255, 255, 255, 0.3);
            color: white !important;
        }
        
        /* Main Content */
        .main-content {
            padding: 40px;
        }
        
        h2 {
            margin-bottom: 30px;
            color: #343a40;
            font-weight: bold;
        }
        
        .table {
            background-color: white;
            border-radius: 6px;
            overflow: hidden;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }
        
        .table th {
            background-color: #343a40 !important;
            color: white;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .table td {
            vertical-align: middle;
        }
        
        .table-striped tbody tr:nth-of-type(odd) {
            background-color: #f8f9fa;
        }
        
        .table-hover tbody tr:hover {
            background-color: #dee2e6;
            transition: background-color 0.2s ease-in-out;
        }
        
        .dashboard-wrapper {
            max-width: 1200px;
            margin: auto;
        }
        
        .card-shadow {
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            border-radius: 10px;
            padding: 20px;
            background-color: #ffffff;
            margin-bottom: 30px;
        }
        
        /* User Management Button */
        .user-management-btn {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            color: white;
            padding: 12px 25px;
            border-radius: 25px;
            font-weight: 500;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
        }
        
        .user-management-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
            color: white;
        }
        
        /* Stats Cards */
        .stats-card {
            background: white;
            border-radius: 10px;
            padding: 20px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            margin-bottom: 20px;
            transition: transform 0.3s ease;
        }
        
        .stats-card:hover {
            transform: translateY(-5px);
        }
        
        .stats-icon {
            width: 50px;
            height: 50px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
            color: white;
            margin-bottom: 15px;
        }
        
        .stats-icon.users {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        }
        
        .stats-icon.activity {
            background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
        }
        
        .stats-icon.visits {
            background: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
        }
        
        @media (max-width: 768px) {
            .table {
                font-size: 14px;
            }
            h2 {
                font-size: 24px;
            }
            .main-content {
                padding: 20px;
            }
            .nav-link {
                padding: 8px 15px !important;
                font-size: 14px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navigation -->
        <nav class="navbar navbar-expand-lg navbar-custom">
            <div class="container">
                <a class="navbar-brand" href="#">
                    <i class="fas fa-tachometer-alt me-2"></i>Admin Dashboard
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        <li class="nav-item">
                            <a class="nav-link active" href="Dashboard.aspx">
                                <i class="fas fa-chart-line"></i>Dashboard
                            </a>
                        </li>
                        <li class="nav-item">
                            <asp:LinkButton ID="lnkUserManagement" runat="server" CssClass="nav-link" 
                                OnClick="lnkUserManagement_Click">
                                <i class="fas fa-users"></i>User Management
                            </asp:LinkButton>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#" onclick="showReports()">
                                <i class="fas fa-file-alt"></i>Reports
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#" onclick="showSettings()">
                                <i class="fas fa-cog"></i>Settings
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <!-- Main Content -->
        <div class="main-content">
            <div class="container dashboard-wrapper">
                <!-- Stats Cards Row -->
                <div class="row mb-4">
                    <div class="col-md-4">
                        <div class="stats-card">
                            <div class="stats-icon users">
                                <i class="fas fa-users"></i>
                            </div>
                            <h5>Total Users</h5>
                            <h3 id="totalUsers" runat="server">0</h3>
                            <small class="text-muted">Active registered users</small>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="stats-card">
                            <div class="stats-icon activity">
                                <i class="fas fa-chart-line"></i>
                            </div>
                            <h5>Total Activities</h5>
                            <h3 id="totalActivities" runat="server">0</h3>
                            <small class="text-muted">User activities logged</small>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="stats-card">
                            <div class="stats-icon visits">
                                <i class="fas fa-eye"></i>
                            </div>
                            <h5>Page Visits</h5>
                            <h3 id="totalVisits" runat="server">0</h3>
                            <small class="text-muted">Total page visits</small>
                        </div>
                    </div>
                </div>

                <!-- Header Section -->
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <h2>User Activity Dashboard</h2>
                    <div class="d-flex gap-3">
                        <asp:Button ID="btnUserManagement" runat="server" 
                            CssClass="btn user-management-btn" 
                            Text="Manage Users" 
                            OnClick="btnUserManagement_Click" />
                        <asp:Button ID="btnExport" runat="server" 
                            CssClass="btn btn-success" 
                            Text="Export to Excel" 
                            OnClick="btnExport_Click" />
                    </div>
                </div>

                <!-- Data Grid -->
                <div class="card-shadow">
                    <asp:GridView ID="GridViewUserActivity" runat="server" 
                        AutoGenerateColumns="False" 
                        AllowPaging="true" 
                        PageSize="10"
                        OnPageIndexChanging="GridViewUserActivity_PageIndexChanging"
                        CssClass="table table-striped table-bordered table-hover">
                        <Columns>
                            <asp:BoundField DataField="ActivityId" HeaderText="Activity ID" />
                            <asp:BoundField DataField="UserId" HeaderText="User ID" />
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Gender" HeaderText="Gender" />
                            <asp:BoundField DataField="CreatedAt" HeaderText="Created At" />
                            <asp:BoundField DataField="LastLoginAt" HeaderText="Last Login" />
                            <asp:BoundField DataField="PageVisited" HeaderText="Page Visited" />
                            <asp:BoundField DataField="VisitTime" HeaderText="Visit Time" />
                            <asp:BoundField DataField="DurationSeconds" HeaderText="Duration (s)" />
                        </Columns>
                        <PagerStyle CssClass="pagination justify-content-center" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function showReports() {
            alert('Reports section - Coming Soon!');
        }

        function showSettings() {
            alert('Settings section - Coming Soon!');
        }
    </script>
</body>
</html>