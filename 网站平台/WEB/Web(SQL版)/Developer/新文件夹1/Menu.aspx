<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="WebSite.Web.Menu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>菜单管理</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            var li = jq('#icons li');
            /*全选*/
            jq(li[0]).bind('click', function () {
                jq('td:nth-child(1) input').each(function (index, item) {
                    item.checked = true;
                });
            });
            /*反选*/
            jq(li[1]).bind('click', function () {
                jq('td:nth-child(1) input').each(function (index, item) {
                    item.checked = !item.checked;
                });
            });
            /*添加*/
            jq(li[2]).bind('click', function () {
                dialogIFrame({ url: 'Menu_Edit.aspx', title: '添加功能' });
            });
            /*启用*/
            jq(li[3]).bind('click', function () {
                var count = jq('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择启用对象.');
                }
                else {
                    CallPostBack('#btnDelete', 1);
                }
            });
            /*禁用*/
            jq(li[4]).bind('click', function () {
                var count = jq('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择禁用对象.');
                }
                else {
                    CallPostBack('#btnDelete', 0);
                }
            });
            /*删除*/
            jq(li[5]).bind('click', function () {
                var count = jq('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择删除对象.');
                }
                else {
                    dialogConfirm({ el: '#btnDelete', text: '选中的信息将被删除且无法恢复!确定要删除吗?', data: -1 });
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
            <asp:Button ID="btnDelete" runat="server" Text="批量删除" onclick="btnDelete_Click" />
        </li>
    </ul>
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table" border="1" width="100%" cellpadding="4" cellspacing="0">
                <thead>
                    <tr>
                        <td width="50px" align="left">编号</td>
                        <td>菜单名称</td>
                        <td width="100px">上级菜单</td>
                        <td width="50px">排序</td>
                        <td width="150px">连接页面</td>
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
                </td>
                <td align="left">
                    <%#Eval("menuname") %>
                </td>
                <td id="Label1" runat="server">
                    <%#Eval("pname") %>
                </td>
                <td>
                    <asp:ImageButton ImageUrl="images/icos/arrow_up.gif" runat="server" CommandName="up" CommandArgument='<%#Eval("ParentID") %>' />
                    <asp:ImageButton ImageUrl="images/icos/arrow_down.gif" runat="server" CommandName="down" CommandArgument='<%#Eval("ParentID") %>' />
                </td>
                <td align="left">
                    <%#Eval("Action")%>
                </td>
                <td>
                    <%#!Convert.ToBoolean(Eval("ParentID")) ? "<i class=\"icon0-eye-" + ((Boolean)Eval("Open") ? "open" : "close") + "\"></i>" : ""%>
                </td>
                <td>
                    <asp:ImageButton ID="status" runat="server" ImageUrl="images/icos/checkbox_no.png" ToolTip="启用/禁用" CommandName="status" />
                </td>
                <td>
                    <input class="img" type="image" src="images/icos/write_enabled.gif" onclick="javascript:dialogIFrame({url:'Menu_Edit.aspx?id=<%#Eval("id") %>',title:'修改 - <%#Eval("menuname") %>'});return false;" title="修改" />
                    <asp:ImageButton ID="del" CssClass="img" ImageUrl="images/icos/del_enabled.gif" runat="server" ToolTip="删除" CommandName="del" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <aspx:AspNetPager ID="Pager1" runat="server" FirstPageText="&#171;" 
    LastPageText="&#187;" NextPageText="&gt;" OnPageChanged="Pager1_PageChanged" PrevPageText="&lt;"
    CustomInfoHTML="共<font color='red'><b>%RecordCount%</b></font>条信息,<b><font color='red'>%CurrentPageIndex%</font>/%PageCount%</b>页,每页%PageSize%条信息" 
        PageSize="15" CssClass="black2" 
        PagingButtonSpacing="0px" ShowCustomInfoSection="Right" 
        ShowNavigationToolTip="True" ShowPageIndexBox="Never" CustomInfoClass="" 
        CustomInfoSectionWidth="" LayoutType="Table">
    </aspx:AspNetPager>
</form>
</body>
</html>
