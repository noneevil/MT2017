<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataBakup.aspx.cs" Inherits="WebSite.Web.DataBakup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>备份数据库</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            var li = jq('#icons li');
            //全选
            jq(li[0]).bind('click', function () {
                jq('td:nth-child(1) input').each(function (index, item) {
                    item.checked = true;
                });
            });
            //反选
            jq(li[1]).bind('click', function () {
                jq('td:nth-child(1) input').each(function (index, item) {
                    item.checked = !item.checked;
                });
            });
            //备份
            jq(li[2]).bind('click', function () {
                var html = '<div><table cellpadding="0" cellspacing="0"><tr><td width="55" valign="middle">';
                html += '<img id="loading-img" src="images/dialog/loading.gif" />';
                html += '</td><td valign="middle" id="loading-text"></td></tr></table></div>';
                var _dialog = jq(html).dialog({
                    modal: true,
                    title: "提示",
                    autoOpen: false,
                    height: 80,
                    resizable: false,
                    closeOnEscape: false,
                    close: function () {
                        jq(this).dialog("destroy");
                    },
                    buttons: {
                        '确定': function () {
                            jq(this).dialog("close");
                        }
                    }
                });

                jq.ajax({
                    cache: false,
                    dataType: 'json',
                    url: 'DataBakup.aspx?action=backup',
                    beforeSend: function () {
                        jq(".ui-dialog-titlebar,.ui-dialog-buttonpane").hide();
                        jq('#loading-text').html('程序正在备份中,请稍后...');
                        _dialog.dialog('open');
                    },
                    success: function (data) {
                        jq(".ui-dialog-titlebar,.ui-dialog-buttonpane").show();
                        jq('#loading-text').html(data.Text);
                        jq('#loading-img').attr('src', 'images/dialog/' + data.Ico + '.gif');
                    },
                    error: function () {
                        jq(".ui-dialog-titlebar,.ui-dialog-buttonpane").show();
                        jq('#loading-text').html('备份出错!');
                        jq('#loading-img').attr('src', 'images/dialog/failure.gif');
                    }
                });
            });
            //删除
            jq(li[3]).bind('click', function () {
                var count = jq('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择文件.');
                }
                else {
                    dialogConfirm({ el: '#btnDelete', text: '选中的文件将被删除且无法恢复!确定要删除吗?', data: -1 });
                }
            });
        });
    </script>
</head>
<body>
<form runat="server">
    <div class="head"><i class="icon0-database"></i>备份数据库</div>
    <ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="备份"><span class="ui-icon ui-icon-disk"></span></li>
        <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
        <li style="display:none;">
            <asp:Button ID="btnDelete" runat="server" Text="批量删除" onclick="btnDelete_Click" />
        </li>
    </ul>
    <asp:Repeater ID="Repeater1" runat="server" 
        OnItemCommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table" border="1" width="100%" bordercolor="#CCCCCC" cellpadding="4" cellspacing="0">
                <thead>
                    <tr>
                        <td width="50px" align="left">编号</td>
                        <td>备份文件</td>
                        <td width="120px">备份日期</td>
                        <td width="100px">大小</td>
                        <td width="60px">操作</td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr style="text-align:center;">
                <td align="left">
                    <input type="checkbox" id="ID" runat="server" value='<%#Server.UrlEncode(Eval("FullName").ToString())%>' />
                    <%#Container.ItemIndex+1 %>
                </td>
                <td align="left">
                    <img align="absmiddle" style="margin-right:5px;" src="images/files/rar_small.png" /><label><%#Eval("name")%></label>
                </td>
                <td><%#Eval("CreationTime")%></td>
                <td><%#FileHelper.FileSizeToStr((long)(Eval("Length")))%></td>
                <td>
                    <asp:ImageButton CssClass="img" ImageUrl="images/icos/disk_min.gif" runat="server" ToolTip="保存" CommandName="save" />
                    <asp:ImageButton CssClass="img" ID="del" ImageUrl="images/icos/del_enabled.gif" runat="server" ToolTip="删除" CommandName="del" />
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
        CustomInfoSectionWidth="0" LayoutType="Table">
    </aspx:AspNetPager>
</form>
</body>
</html>
