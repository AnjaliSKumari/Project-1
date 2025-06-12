<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Project_Trio.Home" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Dashboard - Home</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />
    <style>
        /* Global Styles */
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            padding: 0;
            margin: 0;
            position: relative;
            overflow-x: hidden;
        }

        /* Animated background overlay */
        body::before {
            content: '';
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-image: 
                radial-gradient(circle at 25% 25%, rgba(255,255,255,0.1) 0%, transparent 50%),
                radial-gradient(circle at 75% 75%, rgba(255,255,255,0.1) 0%, transparent 50%);
            animation: backgroundFloat 8s ease-in-out infinite;
            pointer-events: none;
            z-index: 0;
        }

        @keyframes backgroundFloat {
            0%, 100% { transform: translateY(0px); opacity: 0.7; }
            50% { transform: translateY(-20px); opacity: 1; }
        }

        /* Main Container */
        .main-container {
            position: relative;
            z-index: 1;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
        }

        /* Dashboard Card */
        .dashboard-card {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(20px);
            border: 1px solid rgba(255, 255, 255, 0.2);
            border-radius: 24px;
            padding: 40px 35px;
            max-width: 600px;
            width: 100%;
            box-shadow: 
                0 25px 45px rgba(0, 0, 0, 0.1),
                0 15px 35px rgba(0, 0, 0, 0.08),
                inset 0 1px 0 rgba(255, 255, 255, 0.6);
            animation: slideIn 0.8s cubic-bezier(0.25, 0.46, 0.45, 0.94);
            position: relative;
            overflow: hidden;
        }

        .dashboard-card::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 1px;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.8), transparent);
        }

        /* Header Section */
        .header-section {
            text-align: center;
            margin-bottom: 35px;
        }

        .header-title {
            font-size: 32px;
            font-weight: 700;
            background: linear-gradient(135deg, #667eea, #764ba2);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            margin-bottom: 8px;
        }

        .header-subtitle {
            color: #666;
            font-size: 16px;
            font-weight: 400;
        }

        /* Profile Section */
        .profile-section {
            background: linear-gradient(135deg, #f8fafc, #f1f5f9);
            border-radius: 20px;
            padding: 25px;
            margin-bottom: 30px;
            border: 1px solid rgba(0,0,0,0.05);
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        .profile-info {
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .profile-icon {
            width: 70px;
            height: 70px;
            border-radius: 50%;
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            font-size: 28px;
            font-weight: 700;
            display: flex !important;
            align-items: center;
            justify-content: center;
            box-shadow: 
                0 8px 16px rgba(102, 126, 234, 0.3),
                0 4px 8px rgba(102, 126, 234, 0.2);
            position: relative;
            transition: all 0.3s ease;
        }

        .profile-icon::before {
            content: '';
            position: absolute;
            top: -2px;
            left: -2px;
            right: -2px;
            bottom: -2px;
            background: linear-gradient(135deg, #667eea, #764ba2);
            border-radius: 50%;
            z-index: -1;
            opacity: 0;
            transition: opacity 0.3s ease;
        }

        .profile-icon:hover::before {
            opacity: 0.7;
        }

        .profile-details h3 {
            color: #2d3748;
            font-size: 24px;
            font-weight: 600;
            margin: 0 0 5px 0;
            line-height: 1.2;
        }

        .profile-status {
            color: #10b981;
            font-size: 14px;
            font-weight: 500;
            display: flex;
            align-items: center;
            gap: 6px;
        }

        .status-dot {
            width: 8px;
            height: 8px;
            background: #10b981;
            border-radius: 50%;
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0% { opacity: 1; }
            50% { opacity: 0.5; }
            100% { opacity: 1; }
        }

        /* Action Buttons Section */
        .actions-section {
            text-align: center;
        }

        .actions-title {
            color: #4a5568;
            font-size: 18px;
            font-weight: 600;
            margin-bottom: 20px;
        }

        .button-group {
            display: flex;
            gap: 15px;
            justify-content: center;
            flex-wrap: wrap;
        }

        /* Enhanced Buttons */
        .btn-enhanced {
            position: relative;
            padding: 15px 30px;
            font-size: 16px;
            font-weight: 600;
            border: none;
            border-radius: 16px;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.25, 0.46, 0.45, 0.94);
            overflow: hidden;
            text-transform: none;
            letter-spacing: 0.3px;
            min-width: 160px;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
        }

        .btn-enhanced::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.4), transparent);
            transition: left 0.5s;
        }

        .btn-enhanced:hover::before {
            left: 100%;
        }

        /* Admin Button */
        .btn-admin {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            box-shadow: 0 8px 20px rgba(102, 126, 234, 0.4);
        }

        .btn-admin:hover {
            background: linear-gradient(135deg, #5a67d8, #6b46c1);
            transform: translateY(-3px);
            box-shadow: 0 12px 30px rgba(102, 126, 234, 0.6);
            color: white;
        }

        /* User Button */
        .btn-user {
            background: linear-gradient(135deg, #48bb78, #38a169);
            color: white;
            box-shadow: 0 8px 20px rgba(72, 187, 120, 0.4);
        }

        .btn-user:hover {
            background: linear-gradient(135deg, #38a169, #2f855a);
            transform: translateY(-3px);
            box-shadow: 0 12px 30px rgba(72, 187, 120, 0.6);
            color: white;
        }

        .btn-enhanced:active {
            transform: translateY(-1px);
        }

        /* Animations */
        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateY(40px) scale(0.95);
            }
            to {
                opacity: 1;
                transform: translateY(0) scale(1);
            }
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .main-container {
                padding: 15px;
            }
            
            .dashboard-card {
                padding: 30px 25px;
                border-radius: 20px;
            }
            
            .header-title {
                font-size: 28px;
            }
            
            .profile-info {
                flex-direction: column;
                text-align: center;
                gap: 15px;
            }
            
            .profile-icon {
                width: 60px;
                height: 60px;
                font-size: 24px;
            }
            
            .button-group {
                flex-direction: column;
                align-items: center;
            }
            
            .btn-enhanced {
                width: 100%;
                max-width: 280px;
            }
        }

        @media (max-width: 480px) {
            .dashboard-card {
                padding: 25px 20px;
                margin: 10px;
            }
            
            .header-title {
                font-size: 24px;
            }
            
            .profile-section {
                padding: 20px;
            }
        }

        /* Additional Professional Touches */
        .welcome-badge {
            display: inline-block;
            background: rgba(102, 126, 234, 0.1);
            color: #667eea;
            padding: 4px 12px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 15px;
        }

        .divider {
            height: 1px;
            background: linear-gradient(90deg, transparent, rgba(0,0,0,0.1), transparent);
            margin: 25px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-container">
            <div class="dashboard-card">
                <!-- Header Section -->
                <div class="header-section">
                    <div class="welcome-badge">Dashboard</div>
                    <h1 class="header-title">Welcome Back!</h1>
                    <p class="header-subtitle">Choose your access level to continue</p>
                </div>

                <!-- Profile Section -->
                <div class="profile-section">
                    <div class="profile-info">
                        <asp:Image ID="imgProfileIcon" runat="server" CssClass="profile-icon" Visible="false" />
                        <span class="profile-icon">
                            <asp:Literal ID="litGenderInitial" runat="server" />
                        </span>
                        <div class="profile-details">
                            <h3>Hello, <asp:Literal ID="litUserName" runat="server" />!</h3>
                            <div class="profile-status">
                                <span class="status-dot"></span>
                                Online
                            </div>
                        </div>
                    </div>
                </div>

                <div class="divider"></div>

                <!-- Actions Section -->
                <div class="actions-section">
                    <h4 class="actions-title">Select Your Access Level</h4>
                    <div class="button-group">
                        <asp:Button ID="btnAdmin" runat="server" 
                            CssClass="btn-enhanced btn-admin" 
                            Text="👑 Admin Panel" 
                            OnClick="btnAdmin_Click" />
                        <asp:Button ID="btnNormalUser" runat="server" 
                            CssClass="btn-enhanced btn-user" 
                            Text="👤 User Dashboard" 
                            OnClick="btnNormalUser_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>