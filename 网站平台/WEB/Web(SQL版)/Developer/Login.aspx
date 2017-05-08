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
    <link href="skin/login.css" rel="stylesheet" type="text/css" />
    <script src="skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="skin/Plugins/mootools-more-1.4.0.1.js" type="text/javascript"></script>
    <script src="skin/Plugins/encrypt.js" type="text/javascript"></script>
    <script type="text/javascript">
        if (window != top) top.location.href = "login.aspx";
        window.addEvent('domready', function () {
            /*检测IE*/
            if (Browser.ie6 || Browser.ie7) {
                window.location.href = 'skin/ie6update.html';
            }
            $$('[type=text],[type=password]').addEvents({
                focus: function () {
                    $(this).setStyle('background-color', '#FC9631');
                },
                blur: function () {
                    $(this).setStyle('background-color', '');
                }
            });
        });
        /*特殊符号检查*/
        function CheckSafe(str) {
            var words = "' &<>?%,;:()`~!#$^*{}[]|+-=\"";
            for (var i = 0; i < words.length; i++) {
                if (str.indexOf(words.substring(i, i + 1)) != -1) {
                    return false;
                    break
                }
            };
            return true
        };
        /*登录事件*/
        function onLogin() {
            if (!CheckSafe($('user').value) || $('user').value.trim().length < 5) {
                $('user').focus();
                alert('用户名格式错误！');
                return false
            }
            if ($('pwd').value.trim().length < 3) {
                $('pwd').focus();
                alert('密码格式错误！');
                return false
            }
            if ($('num').value.trim().length < 5) {
                $('num').focus();
                alert('验证码错误！');
                return false
            }
            var val = $('pwd').value;
            $('pwd').value = MD5(val);

            new Request.JSON({
                url: 'Plugin-S/Ajax.ashx',
                headers: { Action: 'admin_login' },
                data: $("sys").toQueryString(),
                onRequest: function () {
                    $$('input').set('disabled', true);
                },
                onSuccess: function (result) {
                    if (result.Status) {
                        location.replace('/Developer');
                    }
                    else {
                        alert(result.Text);
                        $('pwd').value = val;
                        $$('input').set('disabled', false);
                    }
                }
            }).send();
            return false
        };
    </script>
</head>
<body scroll="no">
<form id="sys" name="sys" action="" method="post" onsubmit="return onLogin();">
  <div id="top"></div>
  <div id="center">
    <div id="center_left"></div>
    <div id="center_middle">
        <table width="162px" height="84px" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td style="width: 53px;">用户名：</td>
            <td><input type="text" id="user" name="user" maxlength="15" value="administrator" /></td>
          </tr>
          <tr>
            <td>密　码：</td>
            <td><input type="password" id="pwd" name="pwd" maxlength="32" value="vs2010" /></td>
          </tr>
          <tr>
            <td>验证码：</td>
            <td>
                <input type="text" id="num" name="num" maxlength="5" style="width:46px;*width:40px;" />
                <img id="AuthCode" align="absmiddle" alt="点击刷新验证码" class="img" src="Plugin-S/AuthCode.aspx" onclick="this.src+='?';" />
            </td>
          </tr>
        </table>
    </div>
    <div id="center_middle_right"></div>
    <div id="center_submit">
      <div class="button"><input type="submit" value="" class="btnsub"  /></div>
      <div class="button"><input type="reset" value="" class="btnrest" /></div>
    </div>
    <div id="center_right"></div>
  </div>
  <div id="footer"></div>
</form>
</body>
</html>