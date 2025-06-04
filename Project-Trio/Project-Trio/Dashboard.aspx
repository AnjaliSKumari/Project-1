<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Project_Trio.Dashboard" %>

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
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
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
