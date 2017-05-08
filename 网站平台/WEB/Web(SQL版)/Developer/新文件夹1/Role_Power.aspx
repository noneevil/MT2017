<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Role_Power.aspx.cs" Inherits="WebSite.Web.Role_Power" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色权限设置</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq("#tabs").tabs();
        });
        function selectAll(obj, index) {
            var table = $(obj).getParent('table');
            var td = table.getElements('tr td:nth-child(' + index + ') input');
            if (td.length > 0) {
                td.each(function (item) {
                    item.checked = true;
                });
            }
        }
        function unSelected(obj, index) {
            var table = $(obj).getParent('table');
            var td = table.getElements('tr td:nth-child(' + index + ') input');
            if (td.length > 0) {
                td.each(function (item) {
                    item.checked = !item.checked;
                });
            }
        }
    </script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <div id="tabs">
        <ul style="background-image:none; border-left:none; border-right:none; border-top:none;">
		    <li><a href="#tabs-1">功能权限</a></li>
            <li><a href="#tabs-2">新闻权限</a></li>
	    </ul>
        <div id="tabs-1" style="height: 370px; padding: 3px; overflow-y: auto;">
            <asp:Repeater ID="Repeater1" runat="server" onitemdatabound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <table class="table" width="100%" border="0" cellpadding="4" cellspacing="0">
                        <thead>
                            <tr>
                                <td>功能名称</td>
                                <td width="90">上级分类</td>
                                <td width="60px">允许访问</td>
                            </tr>
                            <tr style="font-weight:normal; background:none; background-color:#fff; line-height:normal;">
                                <td colspan="2">
                                </td>
                                <td>
                                    <a href="javascript:" onclick="selectAll(this,3);">全</a> / <a href="javascript:" onclick="unSelected(this,3);">反</a>
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr style="text-align:center;">
                            <td align="left"><%#Eval("MenuName") %>
                                <asp:HiddenField ID="ID" Value='<%#Eval("id") %>' runat="server" />
                                <asp:HiddenField ID="Url" Value='<%#Eval("action") %>' runat="server" />
                            </td>
                            <td><%#Eval("pName") %></td>
                            <td><asp:CheckBox ID="CheckBox1" runat="server" /></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div id="tabs-2" style="display:none;height: 370px; padding: 3px; overflow-y: auto;">
            <asp:Repeater ID="Repeater2" runat="server" onitemdatabound="Repeater2_ItemDataBound">
                <HeaderTemplate>
                    <table class="table" width="100%" border="0" cellpadding="4" cellspacing="0">
                        <thead>
                            <tr>
                                <td>新闻分类</td>
                                <td>上级分类</td>
                                <td width="60px">创建</td>
                                <td width="60px">查看</td>
                                <td width="60px">编辑</td>
                                <td width="60px">删除</td>
                                <td width="60px">设置</td>
                            </tr>
                            <tr style="font-weight:normal; background:none; background-color:#fff; line-height:normal;">
                                <td colspan="2"></td>
                                <td><a href="javascript:" onclick="selectAll(this,3);">全</a> / <a href="javascript:" onclick="unSelected(this,3);">反</a></td>
                                <td><a href="javascript:" onclick="selectAll(this,4);">全</a> / <a href="javascript:" onclick="unSelected(this,4);">反</a></td>
                                <td><a href="javascript:" onclick="selectAll(this,5);">全</a> / <a href="javascript:" onclick="unSelected(this,5);">反</a></td>
                                <td><a href="javascript:" onclick="selectAll(this,6);">全</a> / <a href="javascript:" onclick="unSelected(this,6);">反</a></td>
                                <td><a href="javascript:" onclick="selectAll(this,7);">全</a> / <a href="javascript:" onclick="unSelected(this,7);">反</a></td>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr style="text-align:center;">
                            <td align="left"><asp:HiddenField ID="ID" Value='<%#Eval("id") %>' runat="server" /><%#Eval("GroupName")%></td>
                            <td><%#Eval("pName") %></td>
                            <td><asp:CheckBox ID="CheckBox1" runat="server" /></td>
                            <td><asp:CheckBox ID="CheckBox2" runat="server" /></td>
                            <td><asp:CheckBox ID="CheckBox3" runat="server" /></td>
                            <td><asp:CheckBox ID="CheckBox4" runat="server" /></td>
                            <td><asp:CheckBox ID="CheckBox5" runat="server" /></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                        </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div style="padding:5px 0; text-align:right;">
        <label id="Label1" runat="server" style="float:left; padding-right:5px;"></label>
        <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
        <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
    </div>
</form>
</body>
</html>