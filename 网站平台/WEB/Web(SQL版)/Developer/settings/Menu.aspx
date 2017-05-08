<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="WebSite.Web.Menu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单管理</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="../skin/Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            var li = $$('#icons li');
            /*全选*/
            $(li[0]).addEvent('click', function () {
                $$('td:nth-child(1) input').each(function (item, index) {
                    item.checked = true;
                });
            });
            /*反选*/
            $(li[1]).addEvent('click', function () {
                $$('td:nth-child(1) input').each(function (item, index) {
                    item.checked = !item.checked;
                });
            });
            /*添加*/
            $(li[2]).addEvent('click', function () {
                dialogIFrame({ url: 'Menu_Edit.aspx', title: '添加菜单', width: 450, height: 350 });
            });
            /*启用*/
            $(li[3]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择启用对象.');
                }
                else {
                    CallPostBack('#btnCommand', 1);
                }
            });
            /*禁用*/
            $(li[4]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择禁用对象.');
                }
                else {
                    CallPostBack('#btnCommand', 0);
                }
            });
            /*删除*/
            $(li[5]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择删除对象.');
                }
                else {
                    dialogConfirm({ el: '#btnCommand', text: '选中的信息将被删除且无法恢复!确定要删除吗?', data: -1 });
                }
            });
        });
    </script>
</head>
<body>
<form runat="server">
    <div class="head"><i class="icon0-menu-2"></i>功能设置</div>
    <ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="添加"><span class="ui-icon ui-icon-plus"></span></li>
	    <li class="ui-state-default" title="启用"><span class="ui-icon ui-icon-check"></span></li>
	    <li class="ui-state-default" title="禁用"><span class="ui-icon ui-icon-cancel"></span></li>
        <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
        <li style="display:none;">
            <asp:Button ID="btnCommand" runat="server" onclick="btnCommand_Click" />
        </li>
    </ul>
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table" border="0" width="100%" cellpadding="4" cellspacing="0">
                <thead>
                    <tr>
                        <td width="50px" align="left">选择</td>
                        <td>菜单名称</td>
                        <td width="50px">排序</td>
                        <td width="200px">连接页面</td>
                        <td width="60px">是否展开</td>
                        <td width="30px">启用</td>
                        <td width="60px">操作</td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr style="text-align: center;">
                <td align="left">
                    <asp:CheckBox ID="ID" runat="server" Text='<%#Eval("id") %>' />
                    <asp:HiddenField ID="hidTitle" Value='<%#Eval("title") %>' runat="server" />
                </td>
                <td align="left">
                    <asp:Literal ID="LitLayer" runat="server"></asp:Literal>
                    <%#Eval("title") %>
                    <asp:Literal ID="LitLinkUrl" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:ImageButton ImageUrl="../skin/icos/arrow_up.gif" runat="server" CommandName="up" CommandArgument='<%#Eval("ParentID") %>' />
                    <asp:ImageButton ImageUrl="../skin/icos/arrow_down.gif" runat="server" CommandName="down" CommandArgument='<%#Eval("ParentID") %>' />
                </td>
                <td align="left">
                    <%#Eval("link_url")%>
                </td>
                <td>
                    <%#!Convert.ToBoolean(Eval("ParentID")) ? "<i class=\"icon0-eye-" + ((Boolean)Eval("isopen") ? "open" : "close") + "\"></i>" : ""%>
                </td>
                <td>
                    <asp:ImageButton ID="IsEnable" runat="server" ImageUrl="../skin/icos/checkbox_no.png" ToolTip="启用/禁用" CommandName="enable" />
                </td>
                <td>
                    <input class="img" type="image" src="../skin/icos/write_enabled.gif" onclick="javascript:dialogIFrame({url:'Menu_Edit.aspx?id=<%#Eval("id") %>',title:'修改 - <%#Eval("title") %>',width: 450, height: 350});return false;" title="修改" />
                    <asp:ImageButton ID="del" CssClass="img" ImageUrl="../skin/icos/del_enabled.gif" runat="server" ToolTip="删除" CommandName="del" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</form>
</body>
</html>