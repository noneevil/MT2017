<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Member.aspx.cs" Inherits="WebSite.Web.Member" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员管理</title>
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
            /*添加*/
            jq(li[2]).bind('click', function () {
                dialogIFrame({ url: 'Member_Edit.aspx', title: '添加会员',width:650, height: 530 });
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
<form id="form1" runat="server">
    <div class="head"><i class="icon0-users-3"></i>会员管理</div>
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
    <div class="ui-widget-header ui-helper-clearfix" style="background-image:none;">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td>关键字：</td>
                <td><asp:TextBox ID="txtKey" runat="server" CssClass="text" Width="150"></asp:TextBox></td>
                <td>
                    <asp:DropDownList ID="dropField" runat="server" Width="100">
                    </asp:DropDownList>
                </td>
                <td>注册时间:</td>
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
                        <td>会员名称</td>
                        <td>姓名</td>
                        <td width="80px">会员类型</td>
                        <td width="35px">状态</td>
                        <td width="80px">注册日期</td>
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
                    <%#Eval("MemberName")%>
                    <asp:Image ID="ValidateMail" ToolTip="未通过邮箱验证" ImageUrl="images/icos/email-disable.png" runat="server" />
                    <asp:Image ID="ValidateMobile" ToolTip="未通过手机验证" ImageUrl="images/icos/mobile-disable.png" runat="server" />
                    <asp:HiddenField ID="membername" runat="server" Value='<%#Eval("MemberName")%>' />
                </td>
                <td><%#Eval("username") %></td>
                <td><%#Enum.Parse(typeof(MemberType), Eval("membertype").ToString(), true) %></td>
                <td>
                    <asp:ImageButton ID="status" runat="server" ImageUrl="images/icos/checkbox_no.png" ToolTip="启用" CommandName="status" CommandArgument='<%#Eval("status") %>' />
                </td>
                <td>
                    <%#Eval("JoinDate", "{0:yyyy-MM-dd}")%>
                </td>
                <td>
                    <input class="img" type="image" src="images/icos/info.gif" onclick="javascript:dialogIFrame({url:'Member_Edit.aspx?id=<%#Eval("id") %>',title:'会员资料 - <%#Eval("MemberName") %>',width:650, height: 530});return false;" title="查看详细" />
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
        PageSize="13" CssClass="black2" 
        PagingButtonSpacing="0px" ShowCustomInfoSection="Right" 
        ShowNavigationToolTip="True" ShowPageIndexBox="Never" CustomInfoClass="" 
        CustomInfoSectionWidth="" LayoutType="Table">
    </aspx:AspNetPager>
</form>
</body>
</html>