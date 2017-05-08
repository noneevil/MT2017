<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Role_Power.aspx.cs" Inherits="WebSite.Web.Role_Power" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色权限设置</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <style type="text/css">
        td{ white-space:nowrap; word-break:keep-all;}
    </style>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            jQuery('#tabs').tabs();
            $$('tbody td:nth-child(3) a').addEvent('click', function () {
                var input = this.getParent('td').getPrevious('td').getElements('input');
                if (input.length > 0) {
                    if (this.get('text') == '全') {
                        input.each(function (item, index) {
                            item.checked = true;
                        });
                    }
                    else {
                        input.each(function (item, index) {
                            item.checked = !item.checked;
                        });
                    }
                }
            });
            $$('thead a').addEvent('click', function () {
                var self = $(this);
                var table = $(this).getParent('table');
                var td = table.getElements('tr td:nth-child(2) input');
                if (td.length > 0) {
                    td.each(function (item) {
                        item.checked = self.get('text') == '全选' ? true : !item.checked;
                    });
                }
            });
            var width = 0;
            $$('tbody td:nth-child(2) span').each(function (item, index) {
                var w = $(item).getSize().x;
                if (w > width) width = w;
            });
            $$('tbody tr:nth-child(1) td:nth-child(2)').setStyle('width', width);
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-feather"></i>用户权限设置</div>
    <div id="tabs">
        <ul style="background-image:none; border-left:none; border-right:none; border-top:none;">
		    <li><a href="#tabs-1">功能权限</a></li>
            <li><a href="#tabs-2">内容权限</a></li>
	    </ul>
        <div id="tabs-1" style="padding: 3px;">
            <asp:Repeater ID="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <table class="table" border="0" width="100%" border="0" cellpadding="4" cellspacing="0">
                        <thead>
                            <tr>
                                <td>功能名称</td>
                                <td colspan="2">权限</td>
                            </tr>
                            <tr style="font-weight:normal; background:none; background-color:#fff; line-height:normal;">
                                <td>
                                </td>
                                <td colspan="2">
                                    <a href="javascript:">全选</a> / <a href="javascript:">反选</a>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr style="text-align:center;">
                            <td align="left">
                                <asp:Literal ID="LitLayer" runat="server"></asp:Literal>
                                <%#Eval("title") %>
                                <asp:HiddenField ID="ID" Value='<%#Eval("id") %>' runat="server" />
                                <asp:HiddenField ID="link_url" Value='<%#Eval("link_url") %>' runat="server" />
                            </td>
                            <td align="left">
                                <asp:CheckBoxList ID="cbaction" RepeatLayout="Flow" BorderStyle="None" CellPadding="0" CellSpacing="0" RepeatColumns="13" runat="server"></asp:CheckBoxList>
                            </td>
                            <td width="60px">
                                <a href="javascript:">全</a> / <a href="javascript:">反</a>
                            </td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div id="tabs-2" style="padding: 3px;">
            <asp:Repeater ID="Repeater2" runat="server" onitemdatabound="Repeater2_ItemDataBound">
                <HeaderTemplate>
                    <table class="table" border="0" width="100%" border="0" cellpadding="4" cellspacing="0">
                        <thead>
                            <thead>
                                <tr>
                                    <td>新闻分类</td>
                                    <td colspan="2">权限</td>
                                </tr>
                                <tr style="font-weight:normal; background:none; background-color:#fff; line-height:normal;">
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <a href="javascript:">全选</a> / <a href="javascript:">反选</a>
                                    </td>
                                </tr>
                            </thead>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="text-align:center;">
                        <td align="left">
                            <asp:Literal ID="LitLayer" runat="server"></asp:Literal>
                            <%#Eval("GroupName")%>
                            <asp:HiddenField ID="ID" Value='<%#Eval("id") %>' runat="server" />
                            <asp:HiddenField ID="link_url" Value="" runat="server" />
                        </td>
                        <td align="left">
                            <asp:CheckBoxList ID="cbaction" RepeatLayout="Flow" BorderStyle="None" CellPadding="0" CellSpacing="0" RepeatColumns="13" runat="server"></asp:CheckBoxList>
                        </td>
                        <td width="60px">
                            <a href="javascript:">全</a> / <a href="javascript:">反</a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div style="padding:5px 0; text-align:center;">
        <label id="Label1" runat="server" style="float:left; padding-right:5px;"></label>
        <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
        <input type="button" onclick="javascript:history.back(-1);" value="取消" style="width:80px;" />
    </div>
</form>
</body>
</html>