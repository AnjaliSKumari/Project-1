<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProfilePage.aspx.cs" Inherits="Project_Trio.ProfilePage" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>User Profile</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        :root {
            --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            --secondary-gradient: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
            --success-gradient: linear-gradient(135deg, #4facfe 0%, #00f2fe 100%);
            --warning-gradient: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
            --card-shadow: 0 20px 60px rgba(0, 0, 0, 0.1);
            --card-shadow-hover: 0 30px 80px rgba(0, 0, 0, 0.15);
            --border-radius: 16px;
            --transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 50%, #f093fb 100%);
            min-height: 100vh;
            padding: 20px;
            margin: 0;
            position: relative;
            overflow-x: hidden;
        }

        body::before {
            content: '';
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: 
                radial-gradient(circle at 20% 80%, rgba(120, 119, 198, 0.3) 0%, transparent 50%),
                radial-gradient(circle at 80% 20%, rgba(255, 119, 198, 0.3) 0%, transparent 50%),
                radial-gradient(circle at 40% 40%, rgba(120, 219, 226, 0.2) 0%, transparent 50%);
            pointer-events: none;
            z-index: -1;
        }

        .profile-container {
            max-width: 480px;
            margin: 40px auto;
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(20px);
            border: 1px solid rgba(255, 255, 255, 0.2);
            padding: 40px;
            border-radius: var(--border-radius);
            box-shadow: var(--card-shadow);
            position: relative;
            transition: var(--transition);
            animation: slideUp 0.6s ease-out;
        }

        .profile-container:hover {
            box-shadow: var(--card-shadow-hover);
            transform: translateY(-5px);
        }

        .profile-container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 4px;
            background: var(--primary-gradient);
            border-radius: var(--border-radius) var(--border-radius) 0 0;
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

        .profile-header {
            text-align: center;
            margin-bottom: 40px;
        }

        .profile-header h3 {
            font-size: 28px;
            font-weight: 700;
            background: var(--primary-gradient);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            margin-bottom: 8px;
            letter-spacing: -0.5px;
        }

        .profile-header p {
            color: #64748b;
            font-size: 16px;
            margin: 0;
            font-weight: 400;
        }

        .profile-avatar-container {
            text-align: center;
            margin-bottom: 32px;
            position: relative;
        }

        .profile-icon {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            font-size: 32px;
            font-weight: 700;
            color: white;
            background: var(--primary-gradient);
            box-shadow: 0 10px 30px rgba(102, 126, 234, 0.4);
            position: relative;
            transition: var(--transition);
            animation: pulse 2s infinite;
        }

        .profile-icon:hover {
            transform: scale(1.1);
            box-shadow: 0 15px 40px rgba(102, 126, 234, 0.6);
        }

        @keyframes pulse {
            0%, 100% {
                box-shadow: 0 10px 30px rgba(102, 126, 234, 0.4);
            }
            50% {
                box-shadow: 0 15px 40px rgba(102, 126, 234, 0.6);
            }
        }

        .profile-icon::after {
            content: '';
            position: absolute;
            top: -3px;
            left: -3px;
            right: -3px;
            bottom: -3px;
            border-radius: 50%;
            border: 2px solid rgba(102, 126, 234, 0.3);
            animation: rotate 3s linear infinite;
        }

        @keyframes rotate {
            from { transform: rotate(0deg); }
            to { transform: rotate(360deg); }
        }

        .form-group {
            margin-bottom: 24px;
            position: relative;
        }

        .form-label {
            font-weight: 600;
            color: #374151;
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 8px;
            display: block;
        }

        .form-control {
            border: 2px solid #e5e7eb;
            border-radius: 12px;
            padding: 16px 20px;
            font-size: 16px;
            font-weight: 500;
            background: rgba(255, 255, 255, 0.8);
            transition: var(--transition);
            width: 100%;
        }

        .form-control:focus {
            border-color: #667eea;
            box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
            background: white;
            outline: none;
        }

        .form-control[readonly] {
            background: #f8fafc;
            color: #64748b;
            cursor: default;
        }

        .form-control[readonly]:focus {
            border-color: #e5e7eb;
            box-shadow: none;
        }

        .btn-group {
            display: flex;
            gap: 12px;
            margin-top: 32px;
            flex-wrap: wrap;
        }

        .btn {
            padding: 14px 28px;
            font-size: 16px;
            font-weight: 600;
            border-radius: 12px;
            border: none;
            cursor: pointer;
            transition: var(--transition);
            text-transform: uppercase;
            letter-spacing: 0.5px;
            flex: 1;
            min-width: 120px;
            position: relative;
            overflow: hidden;
        }

        .btn::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
            transition: left 0.5s;
        }

        .btn:hover::before {
            left: 100%;
        }

        .btn-warning {
            background: var(--warning-gradient);
            color: white;
            box-shadow: 0 4px 15px rgba(250, 112, 154, 0.4);
        }

        .btn-warning:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(250, 112, 154, 0.6);
        }

        .btn-success {
            background: var(--success-gradient);
            color: white;
            box-shadow: 0 4px 15px rgba(79, 172, 254, 0.4);
        }

        .btn-success:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(79, 172, 254, 0.6);
        }

        .btn-secondary {
            background: linear-gradient(135deg, #6c757d 0%, #495057 100%);
            color: white;
            box-shadow: 0 4px 15px rgba(108, 117, 125, 0.4);
        }

        .btn-secondary:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 25px rgba(108, 117, 125, 0.6);
        }

        .btn:active {
            transform: translateY(0);
        }

        .status-message {
            margin-top: 24px;
            padding: 16px 20px;
            border-radius: 12px;
            font-weight: 500;
            text-align: center;
            opacity: 0;
            transform: translateY(10px);
            transition: var(--transition);
        }

        .status-message.show {
            opacity: 1;
            transform: translateY(0);
        }

        .text-success {
            background: linear-gradient(135deg, rgba(79, 172, 254, 0.1) 0%, rgba(0, 242, 254, 0.1) 100%);
            color: #0ea5e9;
            border: 1px solid rgba(79, 172, 254, 0.2);
        }

        .floating-elements {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            pointer-events: none;
            z-index: -1;
        }

        .floating-circle {
            position: absolute;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.1);
            animation: float 6s ease-in-out infinite;
        }

        .floating-circle:nth-child(1) {
            width: 80px;
            height: 80px;
            top: 20%;
            left: 10%;
            animation-delay: 0s;
        }

        .floating-circle:nth-child(2) {
            width: 60px;
            height: 60px;
            top: 60%;
            right: 10%;
            animation-delay: 2s;
        }

        .floating-circle:nth-child(3) {
            width: 100px;
            height: 100px;
            bottom: 20%;
            left: 20%;
            animation-delay: 4s;
        }

        @keyframes float {
            0%, 100% {
                transform: translateY(0px) rotate(0deg);
                opacity: 0.5;
            }
            50% {
                transform: translateY(-20px) rotate(180deg);
                opacity: 0.8;
            }
        }

        @media (max-width: 576px) {
            .profile-container {
                margin: 20px auto;
                padding: 30px 20px;
            }

            .profile-header h3 {
                font-size: 24px;
            }

            .btn-group {
                flex-direction: column;
            }

            .btn {
                min-width: auto;
            }
        }

        /* Loading animation for form transitions */
        .form-loading {
            position: relative;
            overflow: hidden;
        }

        .form-loading::after {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(102, 126, 234, 0.1), transparent);
            animation: loading 1.5s infinite;
        }

        @keyframes loading {
            0% { left: -100%; }
            100% { left: 100%; }
        }
    </style>
</head>
<body>
    <div class="floating-elements">
        <div class="floating-circle"></div>
        <div class="floating-circle"></div>
        <div class="floating-circle"></div>
    </div>
    
    <form id="form1" runat="server">
        <div class="profile-container">
            <div class="profile-header">
                <h3>Your Profile</h3>
                <p>Manage your personal information</p>
            </div>
            
            <div class="profile-avatar-container">
                <span id="genderCircle" runat="server" class="profile-icon">
                    <asp:Literal ID="litGenderInitial" runat="server" />
                </span>
            </div>
            
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtUsername" Text="Username" CssClass="form-label" />
                <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" ReadOnly="true" />
            </div>
            
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtEmail" Text="Email Address" CssClass="form-label" />
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" ReadOnly="true" />
            </div>
            
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="txtGender" Text="Gender" CssClass="form-label" />
                <asp:TextBox runat="server" ID="txtGender" CssClass="form-control" ReadOnly="true" />
            </div>
            
            <div class="btn-group">
                <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-warning" Text="✏️ Edit" OnClick="btnEdit_Click" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-success" Text="💾 Save" OnClick="btnSave_Click" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary" Text="✖️ Cancel" OnClick="btnCancel_Click" Visible="false" />
            </div>
            
            <asp:Label ID="lblStatus" runat="server" CssClass="text-success status-message"></asp:Label>
        </div>
    </form>

    <script>
        // Add smooth transitions and interactions
        document.addEventListener('DOMContentLoaded', function() {
            // Show status message with animation
            const statusMessage = document.querySelector('.status-message');
            if (statusMessage && statusMessage.textContent.trim()) {
                statusMessage.classList.add('show');
                setTimeout(() => {
                    statusMessage.classList.remove('show');
                }, 5000);
            }
            
            // Add loading effect to form when buttons are clicked
            const buttons = document.querySelectorAll('.btn');
            buttons.forEach(button => {
                button.addEventListener('click', function() {
                    const formGroups = document.querySelectorAll('.form-group');
                    formGroups.forEach(group => {
                        group.classList.add('form-loading');
                    });
                });
            });
        });
    </script>
</body>
</html>