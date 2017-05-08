<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="WebSite.Web.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>后台管理系统</title>
    <meta http-equiv="Window-target" content="_top" />
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="skin/favicon.ico" rel="Shortcut Icon" />
    <link href="skin/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="skin/Plugins/fontsicon/lte-ie7.js" type="text/javascript"></script><![endif]-->
    <script src="skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="skin/Plugins/mootools-more-1.4.0.1.js" type="text/javascript"></script>
    <script src="skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/public.js" type="text/javascript"></script>
    <script src="skin/Plugins/time.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvents({
            'domready': function () {
                resize();
                new DateTimeShow('time');
                /*setTimeout("KeepLink();", 2000);*/
            },
            'resize': function () {
                resize();
            }
        });
        /*主框架高度修正*/
        function resize() {
            var h = $(window).getSize().y;
            $('main').setStyles({ height: h - 143 });
            $('leftFrame').setStyles({ height: h - 158 });
            $('winFrame').setStyles({ height: h - 138 });
        }
        /*显示隐藏左侧菜单*/
        function ShowMenu(obj) {
            $('menulist').toggleClass('hide');
            $$('#footer .left').toggleClass('left2');
            $(obj).toggleClass('menuhide');
            $('leftFrame').setStyles({ height: 0 });
            resize();
        }
        /*显示隐藏顶部导航*/
        function mainTop(obj) {
            $('mainTop').toggleClass('hide');
            $(obj).set('src', $('mainTop').get('class') == 'hide' ? 'images/arrow_down.gif' : 'images/arrow_up.gif');
            resize();
        }
        /*退出时提示*/
        function SingOut() {
            $$('html').setStyle('filter', 'progid:DXImageTransform.Microsoft.BasicImage(grayscale=1)');
            if (confirm('你确定要退出？')) {
                location.href = 'Login.aspx?action=exit';
            }
            else {
                $$('html').setStyle('filter', '');
            }
        }
    </script>
</head>
<body scroll="no" style="padding:0;">
    <div id="header">
        <div class="left"></div>
        <div class="right">
            <input type="hidden" id="SelectFiles" name="SelectFiles" />
            <div>
                <a href="center.aspx" target="winFrame" class="nav"></a>
                <a href="user/PassWord_Edit.aspx" target="winFrame" class="pwd"></a>
                <a href="javascript:" onclick="SingOut();" class="quit"></a>
            </div>
        </div>
    </div>
    <div id="toolbar">
        <div class="left"></div>
        <div class="right" id="time"></div>
    </div>
    <div id="infobar">
        <div class="left">
            <span class="icon0-double-angle-right"></span>
            当前登录用户：<%=Admin.UserName %>
            <span class="icon0-double-angle-right"></span>
            用户角色：<%=Admin.RoleName %>
        </div>
        <div class="right">
            <span class="icon0-location-8"></span>
            <%=Utils.GetIp()%>
        </div>
    </div>
    <table id="main" width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td class="spac"></td>
        <td id="menulist">
            <iframe id="leftFrame" name="leftFrame" width="100%" height="100%" border="0" scrolling="no" frameborder="0" src="left.aspx"></iframe>
            <div>
                版本：2013 v2.0
            </div>
        </td>
        <td class="close">
            <div onclick="ShowMenu(this);" title="打开/关闭左侧栏"></div>
        </td>
        <td valign="top">
            <iframe id="winFrame" name="winFrame"  runat="server" frameborder="0" src="center.aspx"></iframe>
        </td>
        <td class="spac"></td>
      </tr>
    </table>
    <div id="footer">
        <div class="left"></div>
        <div class="right"></div>
    </div>
</body>
</html>