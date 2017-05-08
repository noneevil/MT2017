<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="WebSite.Web.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>管理系统</title>
    <meta name="Robots" content="none" />
    <meta http-equiv="Window-target" content="_top" />
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="skin/favicon.ico" rel="Shortcut Icon" />
    <script src="Plugins/jQuery/jquery-2.1.0.min.js" type="text/javascript"></script>
    <script src="Plugins/encrypt.js" type="text/javascript"></script>
    <link href="skin/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        if (window != top) top.location.href = "login.aspx";
        $(function () {
            $('input[type="text"],input[type="password"]').on({
                focus: function () {
                    $(this).css('background-color', '#FC9631');
                },
                blur: function () {
                    $(this).css('background-color', '');

                }
            });
        });
        function CheckSafe(str) {
            var bads = "' &<>?%,;:()`~!#$^*{}[]|+-=\"";
            for (var i = 0; i < bads.length; i++) {
                if (str.indexOf(bads.substring(i, i + 1)) != -1) {
                    return false;
                    break
                }
            };
            return true
        };
        function login() {
            if (!CheckSafe($('#user').val()) || $('#user').val().length < 5) {
                $('#user').focus();
                alert('用户名格式错误！');
                return false
            }
            if ($('#pwd').val().trim().length < 3) {
                $('#pwd').focus();
                alert('密码格式错误！');
                return false
            }
            if ($('#num').val().trim().length < 5) {
                $('#num').focus();
                alert('验证码错误！');
                return false
            }
            $('#pwd').val(MD5($('#pwd').val()));
            return true
        };
    </script>
</head>
<body scroll="no">
<form id="sys" runat="server" onsubmit="return login();">
  <div id="top"></div>
  <div id="center">
    <div id="center_left"></div>
    <div id="center_middle">
        <table width="162px" height="84px" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td style="width: 53px;">用户名：</td>
            <td><input type="text" id="user" runat="server" maxlength="15" value="administrator" /></td>
          </tr>
          <tr>
            <td>密　码：</td>
            <td><input type="password" id="pwd" runat="server" maxlength="32" value="vs2010" /></td>
          </tr>
          <tr>
            <td>验证码：</td>
            <td>
                <input type="text" id="num" runat="server" maxlength="5" style="width:46px;*width:50px;" />
                <img id="AuthCode" align="absmiddle" alt="点击刷新验证码" class="img" src="Plug-in/AuthCode.aspx" onclick="this.src+='?';" />
            </td>
          </tr>
        </table>
    </div>
    <div id="center_middle_right"></div>
    <div id="center_submit">
      <div class="button"><asp:Button ID="btnSubmit" runat="server" CssClass="btnsub" Text="" onclick="btnSubmit_Click" /></div>
      <div class="button"><input type="reset" value="" class="btnrest" /></div>
    </div>
    <div id="center_right"></div>
  </div>
  <div id="footer"></div>
</form>
</body>
</html>