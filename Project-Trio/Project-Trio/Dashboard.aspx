<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Project_Trio.Dashboard" %>
<<<<<<< HEAD
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
=======

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Activity Dashboard</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }
        .dashboard-container {
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .dashboard-header {
            background-color: #007bff;
            color: white;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
            text-align: center;
        }
        .filters {
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }
        .gridview-container {
            overflow-x: auto;
        }
        .gridview {
            width: 100%;
            border-collapse: collapse;
            background-color: white;
        }
        .gridview th {
            background-color: #343a40;
            color: white;
            padding: 12px;
            text-align: left;
            border: 1px solid #ddd;
        }
        .gridview td {
            padding: 10px;
            border: 1px solid #ddd;
        }
        .gridview tr:nth-child(even) {
            background-color: #f8f9fa;
        }
        .gridview tr:hover {
            background-color: #e9ecef;
        }
        .btn {
            padding: 8px 16px;
            margin: 5px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-primary {
            background-color: #007bff;
            color: white;
        }
        .btn-success {
            background-color: #28a745;
            color: white;
        }
        .stats-cards {
            display: flex;
            flex-wrap: wrap;
            gap: 15px;
            margin-bottom: 20px;
        }
        .stat-card {
            flex: 1;
            min-width: 200px;
            background-color: #007bff;
            color: white;
            padding: 20px;
            border-radius: 5px;
            text-align: center;
        }
        .stat-number {
            font-size: 24px;
            font-weight: bold;
        }
        .stat-label {
            font-size: 14px;
            margin-top: 5px;
>>>>>>> 49d0aaac94ea8cf8269bcfe79740c03dc204c3ce
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
<<<<<<< HEAD
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
=======
        <div class="dashboard-container">
            <div class="dashboard-header">
                <h1>User Activity Dashboard</h1>
                <p>Welcome, <asp:Label ID="lblWelcome" runat="server"></asp:Label>!</p>
            </div>

            <!-- Statistics Cards -->
            <div class="stats-cards">
                <div class="stat-card">
                    <div class="stat-number"><asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Total Users</div>
                </div>
                <div class="stat-card">
                    <div class="stat-number"><asp:Label ID="lblActiveToday" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Active Today</div>
                </div>
                <div class="stat-card">
                    <div class="stat-number"><asp:Label ID="lblTotalSessions" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Total Sessions</div>
                </div>
                <div class="stat-card">
                    <div class="stat-number"><asp:Label ID="lblAvgSessionTime" runat="server" Text="0"></asp:Label></div>
                    <div class="stat-label">Avg Session (min)</div>
                </div>
            </div>

            <!-- Filters -->
            <div class="filters">
                <h3>Filters</h3>
                <asp:Label runat="server" Text="Select User: "></asp:Label>
                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control" style="display: inline-block; width: 200px; margin-right: 10px;"></asp:DropDownList>
                
                <asp:Label runat="server" Text="Date From: "></asp:Label>
                <asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" CssClass="form-control" style="display: inline-block; width: 150px; margin-right: 10px;"></asp:TextBox>
                
                <asp:Label runat="server" Text="Date To: "></asp:Label>
                <asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" CssClass="form-control" style="display: inline-block; width: 150px; margin-right: 10px;"></asp:TextBox>
                
                <asp:Button ID="btnFilter" runat="server" Text="Apply Filters" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh All Data" CssClass="btn btn-success" OnClick="btnRefresh_Click" />
                <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CssClass="btn btn-success" OnClick="btnExport_Click" />
            </div>

            <!-- GridView for User Activity Summary -->
            <div class="gridview-container">
                <h3>User Activity Summary</h3>
                <asp:GridView ID="gvUserSummary" runat="server" CssClass="gridview" AutoGenerateColumns="False" 
                    AllowPaging="True" PageSize="10" OnPageIndexChanging="gvUserSummary_PageIndexChanging"
                    EmptyDataText="No user activity data found.">
                    <Columns>
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="UserCreatedAt" HeaderText="User Created" DataFormatString="{0:MM/dd/yyyy HH:mm}" />
                        <asp:BoundField DataField="RoleName" HeaderText="Role" />
                        <asp:BoundField DataField="LoginPageVisits" HeaderText="Login Visits" />
                        <asp:BoundField DataField="SignupPageVisits" HeaderText="Signup Visits" />
                        <asp:BoundField DataField="HomePageVisits" HeaderText="Home Visits" />
                        <asp:BoundField DataField="DashboardPageVisits" HeaderText="Dashboard Visits" />
                        <asp:BoundField DataField="LastLoginVisit" HeaderText="Last Login" DataFormatString="{0:MM/dd/yyyy HH:mm}" />
                        <asp:BoundField DataField="LastHomeVisit" HeaderText="Last Home Visit" DataFormatString="{0:MM/dd/yyyy HH:mm}" />
                        <asp:BoundField DataField="TotalLoginTime" HeaderText="Total Login Time (min)" />
                        <asp:BoundField DataField="TotalHomeTime" HeaderText="Total Home Time (min)" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnViewDetails" runat="server" Text="View Details" 
                                    CommandName="ViewDetails" CommandArgument='<%# Eval("UserId") %>' 
                                    CssClass="btn btn-primary" style="font-size: 12px; padding: 5px 10px;" 
                                    OnClick="btnViewDetails_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pager" />
                </asp:GridView>
            </div>

            <!-- Detailed Activity GridView (Initially Hidden) -->
            <div id="detailsSection" runat="server" visible="false" style="margin-top: 30px;">
                <h3>Detailed Activity Log for: <asp:Label ID="lblSelectedUser" runat="server"></asp:Label></h3>
                <asp:GridView ID="gvDetailedActivity" runat="server" CssClass="gridview" AutoGenerateColumns="False" 
                    AllowPaging="True" PageSize="15" OnPageIndexChanging="gvDetailedActivity_PageIndexChanging"
                    EmptyDataText="No detailed activity found for this user.">
                    <Columns>
                        <asp:BoundField DataField="PageName" HeaderText="Page" />
                        <asp:BoundField DataField="EntryTime" HeaderText="Entry Time" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="ExitTime" HeaderText="Exit Time" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="TimeSpentMinutes" HeaderText="Time Spent (min)" />
                        <asp:BoundField DataField="SessionId" HeaderText="Session ID" />
                    </Columns>
                    <PagerStyle CssClass="pager" />
                </asp:GridView>
                <asp:Button ID="btnHideDetails" runat="server" Text="Hide Details" CssClass="btn btn-primary" OnClick="btnHideDetails_Click" style="margin-top: 10px;" />
            </div>

            <!-- Message Label -->
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false" style="margin-top: 15px; display: block;"></asp:Label>
        </div>
    </form>
</body>
</html>
>>>>>>> 49d0aaac94ea8cf8269bcfe79740c03dc204c3ce
