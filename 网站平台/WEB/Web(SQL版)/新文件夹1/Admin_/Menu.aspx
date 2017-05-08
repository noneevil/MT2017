<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Developer_Menu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单管理</title>
    <link href="css/reset.css" rel="stylesheet" type="text/css" />
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/widgets.css" rel="stylesheet" type="text/css" />
    <link href="css/icons.css" rel="stylesheet" type="text/css" />
    <link href="css/uniform.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="css/lte-ie7.js"></script><![endif]-->
    <script src="js/mootools.js" type="text/javascript"></script>
    <script src="js/public.js" type="text/javascript"></script>
    <script src="js/mooniform.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            new Mooniform($$('input[type="radio"],input[type="checkbox"]'));
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="widget">
        <div class="widgettitle"><i class="icon0-cog-2"></i>网站参数设置</div>
        <div class="widgetcontent noborder siteconfig">
            <div id="tabs" class="ui-tabs">
                 <table class="NavTable" cellpadding="4" cellspacing="0">
                    <tr>
                        <td width="50px" align="left"><input onclick="selectAllChkboxs(this);" type="checkbox" id="ChkAll" name="ChkAll" />全选</td>
                        <td>菜单名称</td>
                        <td width="100px">上级菜单</td>
                        <td width="50px">排序</td>
                        <td width="150px">连接页面</td>
                        <td width="60px">是否展开</td>
                        <td width="30px">启用</td>
                        <td width="60px">操作</td>
                    </tr>
                </table>
            </div>
            <div style=" padding:3px; text-align:center;">

            </div>
        </div>
    </div>
</form>
</body>
</html>