<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Group_Edit.aspx.cs" Inherits="WebSite.Web.Group_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改分类</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <link href="../skin/Plugins/zTree/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/zTree/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function beforeClick(treeId, treeNode) {
            var zTree = jQuery.fn.zTree.getZTreeObj("tree");
            if (treeNode.isParent) {
                zTree.expandNode(treeNode);
                return true;
            }
        }
        function onClick(e, treeId, treeNode) {
            jQuery("#ParentName").val(treeNode.name);
            jQuery('#ParentID').val(treeNode.id).trigger('change');
        }

        function showMenu() {
            jQuery("#tree").css({ left: 0, top: jQuery("#ParentName").height() }).slideDown("fast");
            jQuery("body").bind("mousedown", onBodyDown);
        }

        function onBodyDown(event) {
            if (!(event.target.id == "treeBtn" || event.target.id == "tree" || jQuery(event.target).parents("#tree").length > 0)) {
                jQuery("#tree").fadeOut("fast");
                jQuery("body").unbind("mousedown", onBodyDown);
            }
        }

        jQuery(function () {
            jQuery('#tabs').tabs();
            var nodes = [];
            jQuery('#ParentID option').each(function () {
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
            var id = jQuery('#ParentID option:selected').val() | jQuery('#ParentID option:first-child').val();
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
            /*绑定 (全选 反选) 事件*/
            $$('td div a').addEvent('click', function () {
                var chkFields = $(this).getParent('td').getElements('input');
                if (chkFields.length > 0) {
                    if ($(this).get('text') == '全选') {
                        chkFields.each(function (item) {
                            item.checked = true;
                        });
                    }
                    else {
                        chkFields.each(function (item) {
                            item.checked = !item.checked;
                        });
                    }
                }
            });
        });
    </script>
</head>
<body>
<form id="Form1" runat="server">
    <div class="head"><i class="icon0-sitemap"></i>分类设置</div>
    <div id="tabs">
        <ul style="background-image:none; border-left:none; border-right:none; border-top:none;">
		    <li><a href="#tabs-1">基本信息</a></li>
            <li><a href="#tabs-2">扩展信息</a></li>
	    </ul>
        <div id="tabs-1" style="padding: 3px;">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                    <tr>
                        <th width="70px">分类名称：</th>
                        <td>
                            <aspx:TextBox ID="GroupName" runat="server" CssClass="text" Width="600"></aspx:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="GroupName" CssClass="line1px_5" runat="server" BorderStyle="None" />
                        </td>
                    </tr>
                    <tr>
                        <th>上级分类：</th>
                        <td>
                            <div style="position:relative;">
                                <input id="ParentName" runat="server" value="顶级分类" readonly class="text" style="width:600px;" />
                                <a id="treeBtn" href="javascript:" onclick="showMenu();return false;">选择</a>
                                <aspx:ListBox ID="ParentID" runat="server" style="display:none;" AutoPostBack="true" onselectedindexchanged="ParentID_SelectedIndexChanged" Width="612" Height="200"></aspx:ListBox>
                                <ul id="tree" class="ztree" style="width:600px;display:none; z-index:10; position: absolute; height:280px;"></ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>展示模板：</th>
                        <td>
                            <aspx:DropDownList ID="Template" runat="server" Width="612"></aspx:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>数据表：</th>
                        <td><aspx:TextBox ID="TableName" runat="server" CssClass="text" Width="600"></aspx:TextBox></td>
                    </tr>
                    <tr>
                        <th valign="top">权限资源:</th>
                        <td>
                            <div style="padding:5px; width:600px; line-height:19px; height:19px; vertical-align:middle; background-color:#ADD2DA; font-weight:bold;">
                                <a href="javascript:">全选</a> |
                                <a href="javascript:">反选</a>
                            </div>
                            <aspx:CheckBoxList ID="actiontype" runat="server" RepeatDirection="Horizontal" RepeatColumns="7" CellPadding="0"></aspx:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <th valign="top">字段设置：</th>
                        <td align="left">
                            <div style="padding:5px; width:600px; line-height:19px; height:19px;vertical-align:middle; background-color:#ADD2DA; font-weight:bold;">
                                <a href="javascript:">全选</a> |
                                <a href="javascript:">反选</a>
                            </div>
                            <aspx:CheckBoxList ID="Field" runat="server" RepeatDirection="Horizontal" RepeatColumns="7" CellPadding="0"></aspx:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td>
                            <aspx:HiddenField ID="Layer" Value="1" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="GroupName" CssClass="line1px_2" runat="server" ErrorMessage="分类名称名称不能为空！" Width="585"/>
                            <asp:Label ID="Label1" runat="server" Width="335"></asp:Label>
                        </td>
                    </tr>
                </table>
        </div>
        <div id="tabs-2" style="padding: 3px;">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                <tr>
                    <th width="100">SEO标题:</th>
                    <td>
                        <aspx:TextBox ID="seo_title" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th width="100">SEO关键字:</th>
                    <td>
                        <aspx:TextBox ID="seo_keyword" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th width="100">SEO说明:</th>
                    <td>
                        <aspx:TextBox ID="seo_description" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th width="100">连接地址:</th>
                    <td>
                        <aspx:TextBox ID="link_url" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>分类图片:</th>
                    <td>
                         <aspx:BrowseFile ID="image_url" runat="server" Width="660" CssClass="text"></aspx:BrowseFile>
                    </td>
                </tr>
                <tr>
                    <th valign="top">分类内容:</th>
                    <td>
                        <aspx:CKEditor ID="content" runat="server" ToolBarSet="DIY" Width="710" Height="250" CssClass="kama"></aspx:CKEditor>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="text-align:center; padding:5px;">
        <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
        <input type="button" onclick="javascript:history.back(-1);" value="取消" style="width:80px;" />
    </div>
</form>
</body>
</html>