<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="WebSite.Web.News" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新闻内容管理</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <link href="Plugins/JQuery-zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/JQuery-zTree/js/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        function beforeClick(treeId, treeNode) {
            var zTree = jq.fn.zTree.getZTreeObj("tree");
            if (treeNode.isParent) {
                zTree.expandNode(treeNode);
                return false;
            }
        }
        function onClick(e, treeId, treeNode) {
            jq("#groupName").val(treeNode.name);
            jq('#dropGroup').val(treeNode.id).trigger('change');
            jq("#tree").fadeOut("fast");
            jq("body").unbind("mousedown", onBodyDown);
        }

        function showMenu() {
            var cityObj = jq("#groupName");
            var cityOffset = jq("#groupName").offset();
            jq("#tree").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() - 1 + "px" }).slideDown("fast");
            jq("body").bind("mousedown", onBodyDown);
        }

        function onBodyDown(event) {
            if (!(event.target.id == "groupName" || event.target.id == "tree" || jq(event.target).parents("#tree").length > 0)) {
                jq("#tree").fadeOut("fast");
                jq("body").unbind("mousedown", onBodyDown);
            }
        }

        jq(function () {
            var nodes = [];
            jq('#dropGroup option').each(function () {
                var el = jq(this);
                nodes.push({ id: el.val(), pId: el.attr('pid'), name: el.text() });
            });

            jq.fn.zTree.init(jq("#tree"), {
                view: { dblClickExpand: false, selectedMulti: false },
                data: { simpleData: { enable: true} },
                callback: { beforeClick: beforeClick, onClick: onClick }
            }, nodes);
            var zTree = jq.fn.zTree.getZTreeObj("tree");
            var nodes = zTree.transformToArray(zTree.getNodes());
            var text = [];
            var id = jq('#dropGroup option:selected').val() | jq('#dropGroup option:first-child').val();
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].id == id) {
                    zTree.selectNode(nodes[i], true);
                    zTree.expandNode(nodes[i]);
                    text.push(nodes[i].name);
                    break;
                }
            }
        });
	</script>
    <script type="text/javascript">
        jq(function () {
            jq.datepicker.setDefaults(jq.datepicker.regional['zh-TW']);
            jq("#starTime,#endTime").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'clip',
                dateFormat: "yy-mm-dd"
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
                location.href = 'News_Edit.aspx';
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
    <div class="head"><i class="icon0-newspaper"></i>内容管理</div>
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
                <td><asp:TextBox ID="txtKey" runat="server" CssClass="text" Width="100"></asp:TextBox></td>
                <td>
                    <asp:DropDownList ID="dropField" runat="server" Width="100">
                    </asp:DropDownList>
                </td>
                <td>
                    <input id="groupName" runat="server" value="选择分类" readonly class="text" onfocus="showMenu();" style="width:200px;" />
                    <aspx:ListBox ID="dropGroup" runat="server" Width="200" Height="200" style="display:none;">
                    </aspx:ListBox>
                    <ul id="tree" class="ztree" style="width:200px;display:none; z-index:10; position: absolute; height:280px;"></ul>
                </td>
                <td>时间:</td>
                <td><asp:TextBox ID="starTime" runat="server" CssClass="text" Width="70"></asp:TextBox></td>
                <td>至</td>
                <td><asp:TextBox ID="endTime" runat="server" CssClass="text" Width="70"></asp:TextBox></td>
                <td>
                    <asp:Button ID="btnSearch" Width="60" runat="server" Text="搜索" 
                        onclick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table class="table" border="1" width="100%" cellpadding="4" cellspacing="0">
                <thead>
                    <tr>
                        <td width="80px" align="left">编号</td>
                        <td>标题</td>
                        <td width="100">分类</td>
                        <td width="35">焦点</td>
                        <td width="35">置顶</td>
                        <td width="35">启用</td>
                        <td width="90">发布日期</td>
                        <td width="60">操作</td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="ID" runat="server" Text='<%#Eval("id") %>' />
                        <asp:HiddenField ID="Groupid" runat="server" Value='<%#Eval("groupid") %>' />
                    </td>
                    <td><a href="/a/<%#Eval("groupid") %>/<%#Eval("id") %>.shtml" target="_blank"  style="color:<%#Eval("color") %>"><%#Eval("title") %></a></td>
                    <td align="center"><a href="/a/channels/<%#Eval("groupid") %>.shtml" target="_blank"><%#Eval("GroupName")%></a></td>
                    <td align="center">
                        <asp:ImageButton ID="focus" runat="server" ImageUrl="images/icos/checkbox_no.png" ToolTip="焦点" CommandName="focus" CommandArgument='<%#Eval("focus") %>' />
                    </td>
                    <td align="center">
                        <asp:ImageButton ID="stick" runat="server" ImageUrl="images/icos/checkbox_no.png" ToolTip="置顶" CommandName="stick" CommandArgument='<%#Eval("stick") %>' />
                    </td>
                    <td align="center">
                        <asp:ImageButton ID="status" runat="server" ImageUrl="images/icos/checkbox_no.png" ToolTip="启用" CommandName="status" CommandArgument='<%#Eval("status") %>' />
                    </td>
                    <td align="center"><%#Eval("PubDate","{0:yyyy-MM-dd}")%></td>
                    <td align="center">
                        <asp:ImageButton ID="edit" CssClass="img" ImageUrl="images/icos/write_enabled.gif" runat="server" ToolTip="修改" />
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