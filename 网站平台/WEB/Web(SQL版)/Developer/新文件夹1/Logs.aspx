<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logs.aspx.cs" Inherits="WebSite.Web.Logs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>操作日志</title>
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq.datepicker.setDefaults(jq.datepicker.regional['zh-TW']);
            var dates = jq("#starTime,#endTime");
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
            /*删除*/
            jq(li[2]).bind('click', function () {
                var count = jq('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择删除对象.');
                }
                else {
                    dialogConfirm({ el: '#btnDelete', text: '选中的信息将被删除且无法恢复!确定要删除吗?', data: -1 });
                }
            });
            /*清空*/
            jq(li[3]).bind('click', function () {
                dialogConfirm({ el: '#btnDelete', text: '确定要清空所有操作日志吗?', data: -2 });
            });
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-stop-4"></i>操作日志</div>
    <ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
        <li class="ui-state-default" title="清空所有日志"><span class="ui-icon none"><i class="icon0-cancel-circle"></i></span></li>
        <li style="display:none;">
            <asp:Button ID="btnDelete" runat="server" Text="批量删除" onclick="btnDelete_Click" />
        </li>
    </ul>
    <div class="ui-widget-header ui-helper-clearfix" style="background-image:none;">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td>时间:</td>
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
                        <td>日志内容</td>
                        <td width="100px">操作员</td>
                        <td width="120px">日期</td>
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
                    <%#Eval("content")%>
                </td>
                <td>
                    <%#Eval("username")%>
                </td>
                <td>
                    <%#Eval("PubDate","{0:yyyy-MM-dd HH:mm:ss}")%>
                </td>
                <td>
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