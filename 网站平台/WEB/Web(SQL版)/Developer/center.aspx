<%@ Page Language="C#" AutoEventWireup="true" CodeFile="center.aspx.cs" Inherits="Developer_center" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>管理中心</title>
    <link href="skin/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="skin/Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvent('domready', function () {

        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-sitemap"></i>管理中心</div>
    <%--<ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="添加"><span class="ui-icon ui-icon-plus"></span></li>
        <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
        <li style="display:none;">
            <asp:Button ID="btnCommand" runat="server" Text="批量删除" onclick="btnCommand_Click" />
        </li>
    </ul>--%>
    
</form>
</body>
</html>