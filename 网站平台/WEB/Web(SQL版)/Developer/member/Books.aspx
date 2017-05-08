<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Books.aspx.cs" Inherits="WebSite.Web.Books" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>留言管理</title>
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="../skin/Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            jQuery.datepicker.setDefaults(jQuery.datepicker.regional['zh-TW']);
            var dates = jQuery("#starTime,#endTime");
            dates.datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'clip',
                dateFormat: "yy-mm-dd",
                onSelect: function (selectedDate) {
                    var option = this.id == "starTime" ? "minDate" : "maxDate";
                    dates.not(this).datepicker("option", option, selectedDate);
                }
            });

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
            /*审核*/
            $(li[2]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择审核对象.');
                }
                else {
                    CallPostBack('#btnDelete', 1);
                }
            });
            /*禁用*/
            $(li[3]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择审核对象.');
                }
                else {
                    CallPostBack('#btnDelete', 0);
                }
            });
            /*删除*/
            $(li[4]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
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
<form id="form1" runat="server">
    <div class="head"><i class="icon0-email"></i>留言管理</div>
    <ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="批量通过"><span class="ui-icon ui-icon-check"></span></li>
	    <li class="ui-state-default" title="批量未通过"><span class="ui-icon ui-icon-cancel"></span></li>
        <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
        <li style="display:none;">
            <asp:Button ID="btnDelete" runat="server" Text="批量删除" onclick="btnDelete_Click" />
        </li>
    </ul>
    <div class="ui-widget-header ui-helper-clearfix" style="background-image:none;">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td>关键字：</td>
                <td><asp:TextBox ID="txtKey" runat="server" CssClass="text" Width="150"></asp:TextBox></td>
                <td>
                    <asp:DropDownList ID="dropField" runat="server" Width="100"></asp:DropDownList>
                </td>
                <td>留言时间:</td>
                <td><asp:TextBox ID="starTime" runat="server" CssClass="text" Width="70"></asp:TextBox></td>
                <td>至</td>
                <td><asp:TextBox ID="endTime" runat="server" CssClass="text" Width="70"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnSearch" Width="60" runat="server" Text="搜索" onclick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table" border="1" width="100%" cellpadding="4" cellspacing="0">
                <thead>
                    <tr>
                        <td width="50px" align="left">编号</td>
                        <td>留言标题</td>
                        <td width="80px">留言时间</td>
                        <td width="80px">回复时间</td>
                        <td width="35px">审核</td>
                        <td width="60px">操作</td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr style="text-align:center;">
                <td align="left">
                    <asp:CheckBox ID="ID" runat="server" Text='<%#Eval("id") %>' />
                </td>
                <td align="left">
                    <%#Eval("Title")%>
                    <asp:HiddenField ID="title" runat="server" Value='<%#Eval("Title")%>' />
                </td>
                <td><%#Eval("BookTime", "{0:yyyy-MM-dd}")%></td>
                <td><%#Eval("ReTime", "{0:yyyy-MM-dd}")%></td>
                <td>
                    <asp:ImageButton ID="status" runat="server" ImageUrl="../skin/icos/checkbox_no.png" ToolTip="审核" CommandName="status" CommandArgument='<%#Eval("status") %>' />
                </td>
                <td>
                    <input id="detail" runat="server" class="img" type="image" src="../skin/icos/mail_close.png" title="查看详细" />
                    <asp:ImageButton ID="del" CssClass="img" ImageUrl="../skin/icos/del_enabled.gif" runat="server" ToolTip="删除" CommandName="del" />
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
        PageSize="13" CssClass="black2" 
        PagingButtonSpacing="0px" ShowCustomInfoSection="Right" 
        ShowNavigationToolTip="True" ShowPageIndexBox="Never" CustomInfoClass="" 
        CustomInfoSectionWidth="" LayoutType="Table">
    </aspx:AspNetPager>
</form>
</body>
</html>