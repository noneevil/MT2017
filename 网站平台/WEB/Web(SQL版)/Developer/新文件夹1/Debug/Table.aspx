<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Table.aspx.cs" Inherits="WebSite.Web.Debug.Table" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据库管理工具</title>
    <link href="../images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="../Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="../Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Plugins/public.js" type="text/javascript"></script>
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
                dialogIFrame({ url: 'Table_Edit.aspx', title: '创建数据表', width: 350, height: 250 });
            });
            /*刷新*/
            jq(li[3]).bind('click', function () {
                CallPostBack('#btnDelete', 1);
            });
            /*删除*/
            jq(li[4]).bind('click', function () {
                var count = jq('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择删除对象.');
                }
                else {
                    dialogConfirm({ el: '#btnDelete', text: '选中的数据表将被删除且无法恢复.<br/>请谨慎操作!确定要删除吗?', data: -1 });
                }
            });
            /*授权*/
            jq(li[5]).bind('click', function () {
                dialogIFrame({ url: 'License.aspx', title: '创建授权文件', width: 500, height: 400 });
            });
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-table-2"></i>数据库管理工具</div>
    <ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="添加"><span class="ui-icon ui-icon-plus"></span></li>
        <li class="ui-state-default" title="刷新"><span class="ui-icon none"><i class="icon0-refresh-4"></i></span></li>
        <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
        <li class="ui-state-default" title="授权"><span class="ui-icon icon0-vcard"></span></li>
        <li style="display:none;">
            <asp:Button ID="btnDelete" runat="server" Text="批量删除" onclick="btnDelete_Click" />
        </li>
    </ul>
    <asp:Repeater ID="Repeater1" runat="server" 
        onitemcommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table" border="1" width="100%" cellpadding="4" cellspacing="0">
            <thead>
                <tr>
                    <td width="50px" align="left">编号</td>
                    <td width="120px">表名</td>
                    <td>备注</td>
                    <td width="100px">字段管理</td>
                    <td width="100px">表单管理</td>
                    <td width="60px">操作</td>
                </tr>
            </thead>
            <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><input type="checkbox" id="ID" runat="server" value='<%#Eval("id") %>' /><%#Container.ItemIndex + 1%></td>
                <td><%#Eval("TableName")%></td>
                <td><%#Eval("Description")%></td>
                <td align="center"><a href="Field.aspx?table=<%#Eval("id") %>">字段管理</a></td>
                <td align="center"><a href="Form.aspx?table=<%#Eval("id") %>">表单管理</a></td>
                <td align="center">
                    <input class="img" type="image" src="../images/icos/write_enabled.gif" onclick="javascript:dialogIFrame({url:'Table_Edit.aspx?id=<%#Eval("id") %>',title:'修改 - <%#Eval("TableName") %>', width: 350, height: 250 });return false;" title="修改" />
                    <asp:ImageButton ID="del" CssClass="img" ImageUrl="../images/icos/del_enabled.gif" runat="server" ToolTip="删除" CommandName="del" />
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
