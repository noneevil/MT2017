<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="WebSite.Web.News" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新闻内容管理</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="../skin/Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <link href="../skin/Plugins/zTree/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/zTree/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        function beforeClick(treeId, treeNode) {
            var zTree = jQuery.fn.zTree.getZTreeObj("tree");
            if (treeNode.isParent) {
                zTree.expandNode(treeNode);
                if (treeNode.id != 0) return false;
            }
        }
        function onClick(e, treeId, treeNode) {
            jQuery("#groupName").val(treeNode.name);
            jQuery('#dropGroup').val(treeNode.id).trigger('change');
            jQuery("#tree").fadeOut("fast");
            jQuery("body").unbind("mousedown", onBodyDown);
        }

        function showMenu() {
            jQuery("#tree").css({ left: 0, top: jQuery("#ParentName").height() }).slideDown("fast");
            jQuery("body").bind("mousedown", onBodyDown);
        }

        function onBodyDown(event) {
            if (!(event.target.id == "groupName" || event.target.id == "tree" || jQuery(event.target).parents("#tree").length > 0)) {
                jQuery("#tree").fadeOut("fast");
                jQuery("body").unbind("mousedown", onBodyDown);
            }
        }

        jQuery(function () {
            jQuery.datepicker.setDefaults(jQuery.datepicker.regional['zh-TW']);
            jQuery("#starTime,#endTime").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'clip',
                dateFormat: "yy-mm-dd"
            });

            var nodes = [];
            jQuery('#dropGroup option').each(function () {
                var el = jQuery(this);
                nodes.push({ id: el.val(), pId: el.attr('pid'), name: el.text() });
            });

            jQuery.fn.zTree.init(jQuery("#tree"), {
                view: { dblClickExpand: false, selectedMulti: false },
                data: { simpleData: { enable: true} },
                callback: { beforeClick: beforeClick, onClick: onClick }
            }, nodes);
            var zTree = jQuery.fn.zTree.getZTreeObj("tree");
            var nodes = zTree.transformToArray(zTree.getNodes());
            var text = [];
            var id = jQuery('#dropGroup option:selected').val() | jQuery('#dropGroup option:first-child').val();
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
                location.href = 'News_Edit.aspx';
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
            <asp:Button ID="btnCommand" runat="server" Text="批量删除" onclick="btnCommand_Click" />
        </li>
    </ul>
    <div class="ui-widget-header ui-helper-clearfix" style="background-image:none;">
        <table cellpadding="4" cellspacing="0">
            <tr>
                <td>关键字：</td>
                <td><asp:TextBox ID="txtKey" runat="server" CssClass="text" Width="100"></asp:TextBox></td>
                <td>
                    <asp:DropDownList ID="dropField" runat="server" Width="100"></asp:DropDownList>
                </td>
                <td>
                    <div style="position:relative;">
                        <input id="groupName" runat="server" value="选择分类" readonly class="text" onfocus="showMenu();" style="width:200px;" />
                        <aspx:ListBox ID="dropGroup" runat="server" Width="200" Height="200" style="display:none;"></aspx:ListBox>
                        <ul id="tree" class="ztree" style="width:200px;display:none; z-index:10; position: absolute; height:280px;"></ul>
                    </div>
                </td>
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
                        <td width="80px" align="left">选择</td>
                        <td>标题</td>
                        <td width="100">分类</td>
                        <td width="90">发布日期</td>
                        <td width="150">属性</td>
                        <td width="60">操作</td>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="ID" runat="server" Text='<%#Eval("id") %>' />
                        <asp:HiddenField ID="hidTitle" runat="server" Value='<%#Eval("title") %>' />
                        <asp:HiddenField ID="hidGroupid" runat="server" Value='<%#Eval("groupid") %>' />
                    </td>
                    <td><a href="/a/<%#Eval("groupid") %>/<%#Eval("id") %>.shtml" target="_blank"  style="color:<%#Eval("color") %>"><%#Eval("title") %></a></td>
                    <td align="center"><a href="/a/channels/<%#Eval("groupid") %>.shtml" target="_blank"><%#Eval("groupname")%></a></td>
                    <td align="center"><%#Eval("PubDate","{0:yyyy-MM-dd}")%></td>
                    <td>
                        <div class="btn-tools">
                            <asp:LinkButton CommandName="iscomments" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsComments")) ? "comments selected" : "comments"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsComments")) ? "取消评论" : "设置评论"%>' CommandArgument='<%#Eval("IsComments") %>' />
                            <asp:LinkButton CommandName="isstick" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsStick")) ? "stick selected" : "stick"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsStick")) ? "取消置顶" : "设置置顶"%>' CommandArgument='<%#Eval("IsStick") %>' />
                            <asp:LinkButton CommandName="isnominate" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsNominate")) ? "nominate selected" : "nominate"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsNominate")) ? "取消推荐" : "设置推荐"%>' CommandArgument='<%#Eval("IsNominate") %>' />
                            <asp:LinkButton CommandName="ishotspot" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsHotspot")) ? "hotspot selected" : "hotspot"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsHotspot")) ? "取消热门" : "设置热门"%>' CommandArgument='<%#Eval("IsHotspot") %>' />
                            <asp:LinkButton CommandName="isslide" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsSlide")) ? "slide selected" : "slide"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsSlide")) ? "取消幻灯片" : "设置幻灯片"%>' CommandArgument='<%#Eval("IsSlide") %>' />
                            <asp:LinkButton CommandName="isaudit" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsAudit")) ? "audit selected" : "audit"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsAudit")) ? "取消审核" : "设置审核"%>' CommandArgument='<%#Eval("IsAudit") %>' />
                            <asp:LinkButton CommandName="isenable" runat="server" CssClass='<%#Convert.ToBoolean(Eval("IsEnable")) ? "enable selected" : "enable"%>' ToolTip='<%#Convert.ToBoolean(Eval("IsEnable")) ? "禁用" : "启用"%>' CommandArgument='<%#Eval("IsEnable") %>' />
                        </div>
                    </td>
                    <td align="center">
                        <asp:ImageButton ID="edit" CssClass="img" ImageUrl="../skin/icos/write_enabled.gif" runat="server" ToolTip="修改" />
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
        PageSize="13" CssClass="black2" PagingButtonSpacing="0px" ShowCustomInfoSection="Right" 
        ShowNavigationToolTip="True" ShowPageIndexBox="Never" CustomInfoClass="" 
        CustomInfoSectionWidth="" LayoutType="Table">
    </aspx:AspNetPager>
</form>
</body>
</html>