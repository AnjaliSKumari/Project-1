<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Use.aspx.cs" Inherits="Project_Trio.Use" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>User Management</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />

    <style>
        :root {
            --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            --edit-gradient: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
            --success-gradient: linear-gradient(135deg, #84fab0 0%, #8fd3f4 100%);
            --danger-gradient: linear-gradient(135deg, #ff9a9e 0%, #fecfef 100%);
            --warning-gradient: linear-gradient(135deg, #ffeaa7 0%, #fab1a0 100%);
            --info-gradient: linear-gradient(135deg, #74b9ff 0%, #0984e3 100%);
            --card-shadow: 0 15px 35px rgba(0, 0, 0, 0.1);
            --card-shadow-hover: 0 20px 40px rgba(0, 0, 0, 0.15);
            --transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 50%, #f093fb 100%);
            min-height: 100vh;
            margin: 0;
            padding: 20px;
            position: relative;
        }

        body::before {
            content: '';
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: 
                radial-gradient(circle at 25% 25%, rgba(255, 255, 255, 0.1) 0%, transparent 50%),
                radial-gradient(circle at 75% 75%, rgba(255, 255, 255, 0.05) 0%, transparent 50%);
            pointer-events: none;
            z-index: -1;
        }

        .container {
            max-width: 1200px;
            margin: 40px auto;
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(20px);
            border: 1px solid rgba(255, 255, 255, 0.2);
            padding: 40px;
            border-radius: 20px;
            box-shadow: var(--card-shadow);
            position: relative;
            transition: var(--transition);
            animation: slideUp 0.6s ease-out;
        }

        .container:hover {
            box-shadow: var(--card-shadow-hover);
        }

        .container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 4px;
            background: var(--primary-gradient);
            border-radius: 20px 20px 0 0;
        }

        @keyframes slideUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        h2 {
            text-align: center;
            font-size: 32px;
            font-weight: 700;
            background: var(--primary-gradient);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            margin-bottom: 40px;
            letter-spacing: -0.5px;
            position: relative;
        }

        h2::after {
            content: '';
            position: absolute;
            bottom: -10px;
            left: 50%;
            transform: translateX(-50%);
            width: 60px;
            height: 3px;
            background: var(--primary-gradient);
            border-radius: 2px;
        }

        /* Table Styling */
        .table-container {
            background: white;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0, 0, 0, 0.08);
            border: 1px solid rgba(0, 0, 0, 0.05);
        }

        .table {
            width: 100%;
            margin: 0;
            border-collapse: separate;
            border-spacing: 0;
        }

        .table th {
            background: var(--primary-gradient);
            color: white;
            font-weight: 600;
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            padding: 20px 15px;
            border: none;
            text-align: center;
            position: relative;
        }

        .table th:first-child {
            border-radius: 0;
        }

        .table th:last-child {
            border-radius: 0;
        }

        .table td {
            padding: 18px 15px;
            border-bottom: 1px solid #f1f3f4;
            text-align: center;
            font-weight: 500;
            color: #374151;
            transition: var(--transition);
            position: relative;
        }

        .table tbody tr {
            transition: var(--transition);
        }

        .table tbody tr:hover {
            background: rgba(102, 126, 234, 0.05);
            transform: scale(1.01);
        }

        .table tbody tr:last-child td {
            border-bottom: none;
        }

        /* Edit Row Styling */
        .edit-row {
            background: var(--edit-gradient) !important;
            box-shadow: 0 5px 20px rgba(255, 165, 0, 0.2);
            animation: highlightEdit 0.5s ease-out;
        }

        .edit-row td {
            border-bottom: 1px solid rgba(255, 165, 0, 0.3);
            color: #8b4513;
            font-weight: 600;
        }

        @keyframes highlightEdit {
            0% {
                background: rgba(255, 165, 0, 0.3);
                transform: scale(1.02);
            }
            100% {
                background: var(--edit-gradient);
                transform: scale(1);
            }
        }

        /* Column-specific coloring when in edit mode */
        .edit-row td:nth-child(1) { /* ID Column */
            background: linear-gradient(135deg, #e3f2fd 0%, #bbdefb 100%);
            color: #1976d2;
        }

        .edit-row td:nth-child(2) { /* Username Column */
            background: linear-gradient(135deg, #f3e5f5 0%, #e1bee7 100%);
            color: #7b1fa2;
        }

        .edit-row td:nth-child(3) { /* Email Column */
            background: linear-gradient(135deg, #e8f5e8 0%, #c8e6c9 100%);
            color: #388e3c;
        }

        .edit-row td:nth-child(4) { /* Password Column */
            background: linear-gradient(135deg, #fff3e0 0%, #ffcc02 100%);
            color: #f57c00;
        }

        .edit-row td:nth-child(5) { /* Actions Column */
            background: linear-gradient(135deg, #ffebee 0%, #ffcdd2 100%);
            color: #d32f2f;
        }

        /* Input fields in edit mode */
        .edit-row input[type="text"] {
            border: 2px solid rgba(102, 126, 234, 0.3);
            border-radius: 8px;
            padding: 8px 12px;
            background: rgba(255, 255, 255, 0.9);
            font-weight: 500;
            transition: var(--transition);
            width: 100%;
        }

        .edit-row input[type="text"]:focus {
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
            outline: none;
            background: white;
        }

        /* Button Styling */
        .table a {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 8px 16px;
            margin: 2px;
            text-decoration: none;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            transition: var(--transition);
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            position: relative;
            overflow: hidden;
        }

        .table a::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
            transition: left 0.5s;
        }

        .table a:hover::before {
            left: 100%;
        }

        /* Edit Button */
        .table a[href*="Edit"] {
            background: var(--info-gradient);
            color: white;
            border: 2px solid transparent;
        }

        .table a[href*="Edit"]:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(116, 185, 255, 0.4);
        }

        .table a[href*="Edit"]::after {
            content: '\f044';
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
            margin-left: 4px;
        }

        /* Update Button */
        .table a[href*="Update"] {
            background: var(--success-gradient);
            color: white;
            border: 2px solid transparent;
        }

        .table a[href*="Update"]:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(132, 250, 176, 0.4);
        }

        .table a[href*="Update"]::after {
            content: '\f00c';
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
            margin-left: 4px;
        }

        /* Delete Button */
        .table a[href*="Delete"] {
            background: var(--danger-gradient);
            color: white;
            border: 2px solid transparent;
        }

        .table a[href*="Delete"]:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(255, 154, 158, 0.4);
        }

        .table a[href*="Delete"]::after {
            content: '\f2ed';
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
            margin-left: 4px;
        }

        /* Cancel Button */
        .table a[href*="Cancel"] {
            background: var(--warning-gradient);
            color: white;
            border: 2px solid transparent;
        }

        .table a[href*="Cancel"]:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(255, 234, 167, 0.4);
        }

        .table a[href*="Cancel"]::after {
            content: '\f00d';
            font-family: 'Font Awesome 6 Free';
            font-weight: 900;
            margin-left: 4px;
        }

        /* Button active state */
        .table a:active {
            transform: translateY(0);
        }

        /* Password field styling */
        .password-dots {
            font-family: monospace;
            font-size: 16px;
            color: #6c757d;
            letter-spacing: 2px;
        }

        /* Responsive Design */
        @media screen and (max-width: 768px) {
            .container {
                margin: 20px auto;
                padding: 20px;
            }

            h2 {
                font-size: 24px;
            }

            .table-container {
                overflow-x: auto;
            }

            .table {
                min-width: 600px;
            }

            .table a {
                padding: 6px 12px;
                font-size: 11px;
                margin: 1px;
            }
        }

        /* Loading Animation */
        .loading-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.9);
            display: none;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        }

        .loading-spinner {
            width: 50px;
            height: 50px;
            border: 4px solid #f3f3f3;
            border-top: 4px solid #667eea;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

        /* Confirmation Modal Styling */
        .modal-content {
            border-radius: 16px;
            border: none;
            box-shadow: var(--card-shadow);
        }

        .modal-header {
            background: var(--primary-gradient);
            color: white;
            border-radius: 16px 16px 0 0;
            border-bottom: none;
        }

        .modal-footer .btn {
            border-radius: 8px;
            padding: 10px 20px;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <div class="loading-overlay" id="loadingOverlay">
        <div class="loading-spinner"></div>
    </div>

    <form id="form1" runat="server">
        <div class="container">
            <h2>User Management Dashboard</h2>
            <div class="table-container">
                <asp:GridView ID="GridViewUse" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table"
                    DataKeyNames="Id"
                    OnRowEditing="GridViewUse_RowEditing"
                    OnRowCancelingEdit="GridViewUse_RowCancelingEdit"
                    OnRowUpdating="GridViewUse_RowUpdating"
                    OnRowDeleting="GridViewUse_RowDeleting"
                    OnRowDataBound="GridViewUse_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="Email" HeaderText="Email Address" />
                        <asp:TemplateField HeaderText="Password">
                            <ItemTemplate>
                                <span class="password-dots">••••••••</span>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span class="password-dots">••••••••</span>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField HeaderText="Actions" ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>

    <!-- Bootstrap & JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            const grid = document.getElementById("<%= GridViewUse.ClientID %>");
            const loadingOverlay = document.getElementById("loadingOverlay");

            // Enhanced button click handling with modern confirmations
            grid.querySelectorAll("a").forEach(btn => {
                btn.addEventListener("click", function (e) {
                    let text = btn.innerText.toLowerCase();
                    let confirmMsg = "";
                    let confirmTitle = "";
                    let confirmType = "";

                    if (text.includes("edit")) {
                        confirmMsg = "Do you want to edit this user's information?";
                        confirmTitle = "Edit User";
                        confirmType = "info";
                    } else if (text.includes("update")) {
                        confirmMsg = "Save all changes to this user?";
                        confirmTitle = "Save Changes";
                        confirmType = "success";
                    } else if (text.includes("delete")) {
                        confirmMsg = "This action cannot be undone. Are you sure you want to delete this user?";
                        confirmTitle = "Delete User";
                        confirmType = "danger";
                    } else if (text.includes("cancel")) {
                        confirmMsg = "All unsaved changes will be lost. Continue?";
                        confirmTitle = "Cancel Edit";
                        confirmType = "warning";
                    }

                    if (confirmMsg) {
                        if (confirm(`${confirmTitle}\n\n${confirmMsg}`)) {
                            // Show loading overlay
                            loadingOverlay.style.display = "flex";

                            // Add a small delay to show the loading animation
                            setTimeout(() => {
                                // The postback will happen automatically
                            }, 100);
                        } else {
                            e.preventDefault();
                        }
                    }
                });
            });

            // Add row highlighting on hover
            const rows = grid.querySelectorAll("tbody tr");
            rows.forEach(row => {
                row.addEventListener("mouseenter", function () {
                    if (!this.classList.contains("edit-row")) {
                        this.style.transform = "scale(1.005)";
                        this.style.boxShadow = "0 5px 15px rgba(0,0,0,0.1)";
                    }
                });

                row.addEventListener("mouseleave", function () {
                    if (!this.classList.contains("edit-row")) {
                        this.style.transform = "scale(1)";
                        this.style.boxShadow = "none";
                    }
                });
            });

            // Hide loading overlay after page loads
            window.addEventListener("load", function () {
                loadingOverlay.style.display = "none";
            });
        });
    </script>
</body>
</html>