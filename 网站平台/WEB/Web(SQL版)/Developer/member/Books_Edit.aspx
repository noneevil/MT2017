<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Books_Edit.aspx.cs" Inherits="WebSite.Web.Books_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看留言信息</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function () {
            jQuery("#tabs").tabs();
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div id="tabs">
        <ul style="background-image:none;">
		    <li runat="server" id="tab_1"><a href="#tabs_1">留言信息</a></li>
            <li runat="server" id="tab_2"><a href="#tabs_2">回复信息</a></li>
	    </ul>
	    <div id="tabs_1" style="display:none;">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                <tr>
                    <th width="70px">留言标题：</th>
                    <td>
                        <asp:Label ID="Title" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>留言内容：</th>
                    <td>
                        <asp:Label ID="Content" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>留言时间：</th>
                    <td>
                        <aspx:Label ID="BookTime" runat="server" FormatString="{0:yyyy年MM月dd日 HH:mm:ss}"></aspx:Label>
                    </td>
                </tr>
                <tr>
                    <th>用户名称：</th>
                    <td>
                        <asp:Label ID="UserName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>会员名称：</th>
                    <td>
                        <asp:Label ID="MemberName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>联系电话：</th>
                    <td>
                        <asp:Label ID="Telephone" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>IP地址：</th>
                    <td>
                        <asp:Label ID="IpAddress" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs_2">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                <tr>
                    <th width="70px" valign="top">回复内容：</th>
                    <td>
                        <aspx:TextBox ID="ReContent" runat="server" TextMode="MultiLine" Width="480" Height="200"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>回复时间：</th>
                    <td>
                        <aspx:TextBox ID="ReTime" Enabled="false" runat="server" FormatString="{0:yyyy-MM-dd HH:mm:ss}" CssClass="text" Width="480"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td colspan="3">
                        <asp:Label ID="Label1" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;">
                        <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
                        <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</form>
</body>
</html>