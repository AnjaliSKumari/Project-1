<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Project_Trio.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Styles/Site.css" rel="stylesheet" />
    <style>
        /* Custom styles for the dashboard */
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f8f9fa;
            color: #333;
        }

        .navbar {
            background-color: #ffffff !important;
            border-bottom: 1px solid #e0e0e0;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        }

        .navbar-brand {
            font-weight: bold;
            color: #007bff !important;
        }

        .navbar-text {
            font-size: 1.1em;
            color: #555;
        }

        .container-custom {
            width: 95%; /* Adjust as needed */
            max-width: 1400px; /* Max width for larger screens */
            margin: 0 auto;
            padding: 0 15px;
        }

        .dashboard-container {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            margin-top: 20px;
        }

        .dashboard-header {
            text-align: center;
            margin-bottom: 30px;
        }

            .dashboard-header h1 {
                color: #007bff;
                font-weight: 600;
                margin-bottom: 10px;
            }

            .dashboard-header p {
                font-size: 1.1em;
                color: #666;
            }

        .stats-cards {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 40px;
            text-align: center;
        }

        .stat-card {
            background-color: #e9ecef;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.07);
            transition: transform 0.2s;
        }

            .stat-card:hover {
                transform: translateY(-5px);
            }

        .stat-number {
            font-size: 2.5em;
            font-weight: bold;
            color: #007bff;
            margin-bottom: 5px;
        }

        .stat-label {
            font-size: 0.9em;
            color: #555;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .filters {
            background-color: #f0f8ff;
            padding: 25px;
            border-radius: 8px;
            margin-bottom: 30px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.05);
        }

            .filters h3 {
                color: #007bff;
                margin-bottom: 20px;
            }

        .gridview-container {
            margin-top: 30px;
            background-color: #fff;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

            .gridview-container h3 {
                color: #007bff;
                margin-bottom: 20px;
            }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
        }

            .gridview th, .gridview td {
                padding: 12px 15px;
                text-align: left;
                border-bottom: 1px solid #ddd;
            }

            .gridview th {
                background-color: #f2f2f2;
                color: #333;
                font-weight: 600;
                text-transform: uppercase;
                font-size: 0.9em;
            }

            .gridview tr:nth-child(even) {
                background-color: #f9f9f9;
            }

            .gridview tr:hover {
                background-color: #f1f1f1;
            }

        .pager table {
            width: auto;
            margin: 15px 0 0 auto; /* Align pager to the right */
        }

        .pager td {
            padding: 5px 8px;
            border: none;
        }

        .pager a, .pager span {
            display: inline-block;
            padding: 6px 12px;
            margin: 0 3px;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            text-decoration: none;
            color: #007bff;
            background-color: #fff;
            transition: all 0.2s;
        }

        .pager a:hover {
            background-color: #e9ecef;
            border-color: #0056b3;
            color: #0056b3;
        }

        .pager span {
            background-color: #007bff;
            color: #fff;
            border-color: #007bff;
            font-weight: bold;
        }

        .form-control {
            border-radius: 5px;
            border: 1px solid #ced4da;
            padding: 8px 12px;
        }

        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
            padding: 8px 15px;
            font-size: 0.95em;
            border-radius: 5px;
        }

            .btn-primary:hover {
                background-color: #0056b3;
                border-color: #0056b3;
            }

        .btn-success {
            background-color: #28a745;
            border-color: #28a745;
            padding: 8px 15px;
            font-size: 0.95em;
            border-radius: 5px;
        }

            .btn-success:hover {
                background-color: #218838;
                border-color: #1e7e34;
            }

        .dropdown-menu {
            border-radius: 5px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }

        .dropdown-item {
            padding: 10px 15px;
        }

            .dropdown-item:hover {
                background-color: #f1f1f1;
                color: #007bff;
            }

        .page-visits .badge {
            margin-right: 5px;
            margin-bottom: 5px;
            display: inline-block;
            background-color: #6c757d;
            color: white;
            padding: .35em .65em;
            border-radius: .25rem;
            font-size: 0.85em;
        }

        .badge-info {
            background-color: #17a2b8 !important;
        }

        .badge-secondary {
            background-color: #6c757d !important;
        }

        .badge-success {
            background-color: #28a745 !important;
        }

        .badge-warning {
            background-color: #ffc107 !important;
            color: #343a40 !important;
        }

        .badge-danger {
            background-color: #dc3545 !important;
        }

        .detailed-activity-summary {
            background-color: #e9f7ef; /* Light green background */
            border: 1px solid #d4edda;
            border-radius: 8px;
            padding: 20px;
            margin-top: 20px;
            margin-bottom: 20px;
        }

            .detailed-activity-summary h4 {
                color: #28a745; /* Green header */
                margin-bottom: 15px;
            }

            .detailed-activity-summary p {
                font-size: 1.05em;
                margin-bottom: 8px;
                color: #333;
            }

                .detailed-activity-summary p strong {
                    color: #007bff;
                }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
            <div class="container-custom d-flex justify-content-between align-items-center">
                <div class="navbar-text">
                    Welcome<strong> <asp:Label ID="lblUsername" runat="server" /></strong>
                </div>

                <div class="dropdown">
                    <a class="btn btn-secondary dropdown-toggle d-flex align-items-center" href="#" role="button" id="userDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                        <img src="https://cdn-icons-png.flaticon.com/512/149/149071.png" alt="Profile" class="rounded-circle" style="width: 32px; height: 32px; margin-right: 8px;" />
                        Account
                    </a>
                    <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
                        <li><a class="dropdown-item" href="UserActivity.aspx">User Activity</a></li>
                        <li><a class="dropdown-item" href="Dashboard.aspx">Dashboard</a></li>
                        <li><asp:LinkButton ID="lnkLogout" runat="server" CssClass="dropdown-item" OnClick="lnkLogout_Click">Logout</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="container-custom">
            <div class="dashboard-container">
                <div class="dashboard-header">
                    <h1>User Activity Dashboard</h1>
                    <p>Welcome, <asp:Label ID="lblWelcome" runat="server"></asp:Label>!</p>
                </div>

                <div class="stats-cards">
                    <div class="stat-card">
                        <div class="stat-number"><asp:Label ID="lblTotalUsers" runat="server" Text="0" /></div>
                        <div class="stat-label">Total Registered Users</div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-number"><asp:Label ID="lblUsersToday" runat="server" Text="0" /></div>
                        <div class="stat-label">Users Signed Up Today</div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-number"><asp:Label ID="lblActiveUsers" runat="server" Text="0" /></div>
                        <div class="stat-label">Active Users (Last 24h)</div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-number"><asp:Label ID="lblTotalLogins" runat="server" Text="0" /></div>
                        <div class="stat-label">Total Logins (Overall)</div>
                    </div>
                </div>

                <div class="filters">
                    <h3>Filters</h3>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="mb-3">
                                <label for="<%= ddlUsers.ClientID %>" class="form-label">Select User:</label>
                                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="mb-3">
                                <label for="<%= ddlActivityType.ClientID %>" class="form-label">Activity Type:</label>
                                <asp:DropDownList ID="ddlActivityType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="All Activities" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Logins" Value="Login"></asp:ListItem>
                                    <asp:ListItem Text="Signups" Value="Signup"></asp:ListItem>
                                    <asp:ListItem Text="Page Views" Value="PageView"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4 d-flex align-items-end">
                            <asp:Button ID="btnFilter" runat="server" Text="Apply Filter" CssClass="btn btn-primary me-2" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh All Data" CssClass="btn btn-success me-2" OnClick="btnRefresh_Click" />
                            <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CssClass="btn btn-success" OnClick="btnExport_Click" />
                        </div>
                    </div>
                </div>

                <div class="gridview-container">
                    <h3>User Activity Summary (Last 24 Hours)</h3>
                    <asp:GridView ID="gvUserSummary" runat="server" CssClass="gridview" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvUserSummary_PageIndexChanging"
                        EmptyDataText="No user activity data found." OnRowDataBound="gvUserSummary_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Username" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:TemplateField HeaderText="Pages Visited (24h)">
                                <ItemTemplate>
                                    <div class="page-visits">
                                        <asp:Literal ID="litPagesVisited" runat="server"></asp:Literal>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Time Spent (24h)">
                                <ItemTemplate>
                                    <asp:Literal ID="litTotalTime" runat="server"></asp:Literal> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CreatedAt" HeaderText="User Created At" DataFormatString="{0:MM/dd/yyyy HH:mm}" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:Button ID="btnViewActivity" runat="server" Text="View Activity"
                                        CommandName="ViewActivity" CommandArgument='<%# Eval("UserId") %>'
                                        CssClass="btn btn-primary" Style="font-size: 12px; padding: 5px 10px;"
                                        OnClick="btnViewActivity_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pager" />
                    </asp:GridView>
                </div>

                <div id="detailsSection" runat="server" Visible="false" style="margin-top: 30px;">
                    <h3>Detailed User Activity</h3>
                    <div class="detailed-activity-summary">
                        <h4>Activity Summary for: <asp:Label ID="lblSelectedUserSummary" runat="server" Font-Bold="true"></asp:Label></h4>
                        <p>Total Logins: <strong><asp:Label ID="lblSelectedUserTotalLogins" runat="server"></asp:Label></strong></p>
                        <p>Total Signups: <strong><asp:Label ID="lblSelectedUserTotalSignups" runat="server"></asp:Label></strong></p>
                        <p>Total Page Views: <strong><asp:Label ID="lblSelectedUserTotalPageViews" runat="server"></asp:Label></strong></p>
                    </div>

                    <h4>Page Visit Log</h4>
                    <asp:GridView ID="gvDetailedActivity" runat="server" CssClass="gridview" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvDetailedActivity_PageIndexChanging"
                        EmptyDataText="No detailed activity data found.">
                        <Columns>
                            <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" />
                            <asp:BoundField DataField="PageVisited" HeaderText="Page Visited" />
                            <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                            <asp:TemplateField HeaderText="Entry Time">
                                <ItemTemplate>
                                    <asp:Label ID="lblEntryTime" runat="server" Text='<%# Eval("EntryTime", "{0:HH:mm:ss}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exit Time">
                                <ItemTemplate>
                                    <asp:Label ID="lblExitTime" runat="server" Text='<%# Eval("ExitTime", "{0:HH:mm:ss}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Time Spent (seconds)">
                                <ItemTemplate>
                                    <asp:Label ID="lblTimeSpent" runat="server" Text='<%# Eval("TimeSpentInSeconds") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pager" />
                    </asp:GridView>

                    <h4 style="margin-top: 20px;">Key Page Time Summaries</h4>
                    <div class="detailed-activity-summary">
                        <p>Total Time Spent on Login Page: <strong><asp:Label ID="lblLoginTimeSpent" runat="server"></asp:Label></strong></p>
                        <p>Total Time Spent on Home Page: <strong><asp:Label ID="lblHomeTimeSpent" runat="server"></asp:Label></strong></p>
                        <p>Total Time Spent on Dashboard Page: <strong><asp:Label ID="lblDashboardTimeSpent" runat="server"></asp:Label></strong></p>
                        <p>Total Time Spent on User Activity Page: <strong><asp:Label ID="lblUserActivityTimeSpent" runat="server"></asp:Label></strong></p>
                    </div>

                    <h4 style="margin-top: 20px;">Total Time Spent Per Page</h4>
                    <asp:GridView ID="gvPageTimeSummary" runat="server" CssClass="gridview" AutoGenerateColumns="False"
                        EmptyDataText="No page time summary found.">
                        <Columns>
                            <asp:BoundField DataField="PageName" HeaderText="Page Name" />
                            <asp:BoundField DataField="TotalTimeSpent" HeaderText="Total Time Spent (HH:mm:ss)" />
                        </Columns>
                        <PagerStyle CssClass="pager" />
                    </asp:GridView>

                    <asp:Button ID="btnHideDetails" runat="server" Text="Hide Details" CssClass="btn btn-primary"
                        OnClick="btnHideDetails_Click" Style="margin-top: 10px;" />
                </div>

                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false" Style="margin-top: 15px; display: block;"></asp:Label>
            </div>
        </div>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>