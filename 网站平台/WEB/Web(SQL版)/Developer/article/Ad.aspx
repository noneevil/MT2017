<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ad.aspx.cs" Inherits="WebSite.Web.Ad" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>广告管理</title>
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <link href="../skin/Plugins/lightbox/themes/default/jquery.lightbox.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="../skin/Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/lightbox/jquery.lightbox.min.js" type="text/javascript"></script>
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
                dialogIFrame({ url: 'Ad_Edit.aspx', title: '添加广告', width: 400, height: 380 });
            });
            /*删除*/
            $(li[3]).addEvent('click', function () {
                var count = $$('td:nth-child(1) input:checked').length;
                if (count == 0) {
                    dialogMessage('未选择删除对象.');
                }
                else {
                    dialogConfirm({ el: '#btnDelete', text: '选中的信息将被删除且无法恢复!确定要删除吗?', data: -1 });
                }
            });

            jQuery('.lightbox').lightbox();
        });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-bar-chart"></i>广告管理</div>
    <ul id="icons" class="ui-widget-header ui-helper-clearfix">
        <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
        <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
        <li class="ui-state-default" title="添加"><span class="ui-icon ui-icon-plus"></span></li>
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
                        <td>广告名称</td>
                        <td width="300px">连接地址</td>
                        <td width="50px">预览</td>
                        <td width="50px">点击</td>
                        <td width="50px">展示</td>
                        <td width="50px">分类</td>
                        <td width="60px">操作</td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:CheckBox ID="ID" runat="server" Text='<%#Eval("id") %>' />
                </td>
                <td>
                    <%#Eval("title")%>
                    <asp:HiddenField ID="title" runat="server" Value='<%#Eval("title")%>' />
                </td>
                <td>
                    <%#Eval("linkurl")%>
                </td>
                <td align="center">
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                            <a href="<%#Eval("LinkSrc") %>?lightbox[width]=<%#Eval("width") %>&lightbox[height]=<%#Eval("height") %>" rel="group1" class="lightbox">
                                <img src="../skin/icos/ico_preview.png" />
                            </a>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="false">
                            <a href="../Plugin-S/AdPreview.aspx?src=<%#Eval("LinkSrc") %>&lightbox[iframe]=true&lightbox[width]=<%#Eval("width") %>&lightbox[height]=<%#Eval("height") %>" class="lightbox">
                                <img src="../skin/icos/ico_preview.png" />
                            </a>
                        </asp:PlaceHolder>
                </td>
                <td align="center"><%#Eval("Hits") %></td>
                <td align="center"><%#Eval("ShowNum")%></td>
                <td align="center">
                    <%#Enum.Parse(typeof(AdCategory), Eval("groupid").ToString(), true)%>
                </td>
                <td align="center">
                    <input class="img" type="image" src="../skin/icos/write_enabled.gif" onclick="javascript:dialogIFrame({url:'Ad_Edit.aspx?id=<%#Eval("id") %>',title:'修改 - <%#Eval("title") %>',width:400,height:380});return false;" title="修改" />
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
        PageSize="12" CssClass="black2" 
        PagingButtonSpacing="0px" ShowCustomInfoSection="Right" 
        ShowNavigationToolTip="True" ShowPageIndexBox="Never" CustomInfoClass="" 
        CustomInfoSectionWidth="" LayoutType="Table">
    </aspx:AspNetPager>
</form>
</body>
</html>