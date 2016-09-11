<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登录页</title>
    <link href="Content/Css/login.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtPwd").val("<%=pwd%>");

            $("#txtUsername").blur(function () {
                if ($(this).val() == "") {
                    $("#errorTip").html("请输入用户名！");
                    $("#errorTip").css("display", "block");
                }
                else {
                    $("#errorTip").css("display", "none");
                }
            });
            $("#txtPwd").blur(function () {
                if ($(this).val() == "") {
                    $("#errorTip").html("请输入密码！");
                    $("#errorTip").css("display", "block");
                }
                else {
                    $("#errorTip").css("display", "none");
                }
            });
        });

        function CheckData()
        {
            var flag = true;
            
            if ($("#txtUsername").val() == "") {
                flag = false;
            }
            if ($("#txtPwd").val() == "") {
                flag = false;
            }

            if (!flag)
            {
                $("#errorTip").html("用户名与密码不能为空！");
                $("#errorTip").css("display", "block");
            }

            return flag;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="logo"></div>
        <div class="phone"></div>
        <div class="titleOne">让鼓励无处不在</div>
        <div class="titleTwo">校朋，专注于简单高效的教学与课堂管理</div>
        <div class="login">
            <div class="loginTop"><div class="loginImage"></div></div>
            <div class="loginTitle">登陆到校朋</div>
            <div class="errorTip" id="errorTip" runat="server">
                用户名或密码错误！
            </div>
            <div class="loginUserName">
                <input type="text" id="txtUsername" placeholder="账号/邮箱/手机号码" runat="server"/>
            </div>
            <div class="loginPwd">
                <input type="password" id="txtPwd" placeholder="请输入密码" runat="server" />
            </div>
            <div class="loginRem">
                <div class="remDiv"><asp:CheckBox ID="cbRem" runat="server" />记住账号</div>
               <%-- <div class="forgetDiv">忘记密码?</div>--%>
            </div>
            <div class="loginBtn">
                <div class="registerDiv"></div>
                <div class="load">下载手机端注册</div>
                <div class="loginDiv">
                    <asp:Button ID="btnLogin" runat="server" Text="登 陆" OnClick="btnLogin_Click" OnClientClick="return CheckData()"/>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
