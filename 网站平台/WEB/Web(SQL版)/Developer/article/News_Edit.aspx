<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News_Edit.aspx.cs" Inherits="WebSite.Web.News_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发布文章</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/zTree/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <link href="../skin/Plugins/zTree/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-profile"></i>发布文章</div>
    <div id="tabs">
        <ul style="background-image:none; border-left:none; border-right:none; border-top:none;">
		    <li><a href="#tabs-1">基本信息</a></li>
            <li><a href="#tabs-2">详细内容</a></li>
            <li><a href="#tabs-3">SEO信息</a></li>
	    </ul>
        <div id="tabs-1" style="padding: 3px;">
            <table width="100%" cellpadding="4" border="0" cellspacing="0">
                <tr>
                    <th width="75">
                        文章标题：
                    </th>
                    <td>
                        <aspx:TextBox ID="title" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                        <asp:RequiredFieldValidator runat="server" CssClass="line1px_5" Width="16" ControlToValidate="title"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>
                        文章分类：
                    </th>
                    <td>
                        <div style="position:relative;">
                            <input id="groupName" clear="false" runat="server" value="选择分类" readonly class="text" onfocus="showMenu();" style="width:660px;" />
                            <asp:CompareValidator runat="server" CssClass="line1px_5" Width="16" Type="Integer" Operator="GreaterThan" ValueToCompare="0" ControlToValidate="groupid"></asp:CompareValidator>
                            <aspx:ListBox ID="groupid" runat="server" Width="200" Height="200" style="display:none;"></aspx:ListBox>
                            <ul id="tree" class="ztree" style="width:660px;display:none; z-index:10; position: absolute; height:280px;"></ul>
                        </div>
                    </td>
                </tr>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                <tr>
                    <th>缩略图：</th>
                    <td>
                        <aspx:BrowseFile ID="image_url" runat="server" Width="660" CssClass="text"></aspx:BrowseFile>
                    </td>
                </tr>
                <tr>
                    <th>文档附件：</th>
                    <td>
                        <aspx:BrowseFile ID="attac_url" runat="server" Width="660" CssClass="text" ResourceType="文档文件"></aspx:BrowseFile>
                    </td>
                </tr>
                <tr>
                    <th>视频附件：</th>
                    <td>
                        <aspx:BrowseFile ID="video_url" runat="server" Width="660" CssClass="text" ResourceType="音乐视频"></aspx:BrowseFile>
                    </td>
                </tr>
                <tr>
                    <th>外链地址：</th>
                    <td>
                        <aspx:TextBox ID="link_url" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table cellspacing="0" cellpadding="3">
                            <tr>
                                <th width="73">
                                    标题颜色：
                                </th>
                                <td>
                                    <aspx:ColorPicker runat="server" Width="70" CssClass="text" ID="color" />
                                </td>
                                <th>
                                    作者：
                                </th>
                                <td>
                                    <aspx:TextBox ID="author" runat="server" Width="70" CssClass="text"></aspx:TextBox>
                                </td>
                                <th>
                                    来源：
                                </th>
                                <td>
                                    <aspx:TextBox ID="source" runat="server" Width="70" CssClass="text"></aspx:TextBox>
                                </td>
                                <th>浏览量：</th>
                                <th>
                                    <aspx:TextBox ID="click" runat="server" Width="70" CssClass="text" Text="0"></aspx:TextBox>
                                </th>
                                <th>排序：</th>
                                <th>
                                    <aspx:TextBox ID="sortid" runat="server" Width="70" CssClass="text" Text="0"></aspx:TextBox>
                                </th>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>阅读权限：</th>
                    <td>
                        <aspx:CheckBoxList ID="readaccess" runat="server" CellPadding="0" CellSpacing="0" RepeatLayout="Flow" RepeatDirection="Horizontal"></aspx:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <th>参数设置：</th>
                    <td>
                        <aspx:CheckBox ID="isenable" runat="server" Text="启用" Checked="true" />
                        <aspx:CheckBox ID="isnominate" runat="server" Text="推荐" />
                        <aspx:CheckBox ID="ishotspot" runat="server" Text="热门" />
                        <aspx:CheckBox ID="isslide" runat="server" Text="焦点" />
                        <aspx:CheckBox ID="isstick" runat="server" Text="置顶" />
                        <aspx:CheckBox ID="isaudit" runat="server" Text="审核" />
                        <aspx:CheckBox ID="iscomments" runat="server" Text="允许评论" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs-2" style="padding: 3px;">
            <table width="100%" cellpadding="4" border="0" cellspacing="0">
                <tr>
                    <th width="75" valign="top">
                        内容摘要：
                    </th>
                    <td>
                        <aspx:CKEditor ID="abstract" runat="server" ToolBarSet="Min" Width="700" Height="100" CssClass="kama"></aspx:CKEditor>
                    </td>
                </tr>
                <tr>
                    <th valign="top">
                        详细内容：
                    </th>
                    <td>
                        <aspx:CKEditor ID="content" runat="server" ToolBarSet="DIY" Width="700" Height="500" CssClass="kama"></aspx:CKEditor>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs-3" style="padding: 3px;">
            <table width="100%" cellpadding="4" border="0" cellspacing="0">
                <tr>
                    <th width="85">
                        SEO标题：
                    </th>
                    <td>
                        <aspx:TextBox ID="seo_title" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        SEO关健字：
                    </th>
                    <td>
                        <aspx:TextBox ID="seo_keywords" runat="server" Width="660" CssClass="text"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        SEO描述：
                    </th>
                    <td>
                        <aspx:TextBox ID="seo_description" runat="server" Width="660" Height="100" CssClass="text" TextMode="MultiLine"></aspx:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding:5px;">
            <asp:Label ID="Label1" Width="760" runat="server"></asp:Label>
            <asp:RequiredFieldValidator runat="server" CssClass="line1px_2" Width="600" ControlToValidate="title" ErrorMessage="文章标题不能为空！" Display="Static"></asp:RequiredFieldValidator>
            <aspx:HiddenField ID="Url" runat="server" />
        </div>
        <div style="text-align:center; padding:5px;">
            <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" OnClick="btnSave_Click" />&nbsp;
            <input type="button" value="取消" style="width:80px;" onclick="location.replace('news.aspx');" />
        </div>
    </div>
    <script type="text/javascript">
        function beforeClick(treeId, treeNode) {
            var zTree = jQuery.fn.zTree.getZTreeObj("tree");
            if (treeNode.isParent) {
                zTree.expandNode(treeNode);
                return false;
            }
        }
        function onClick(e, treeId, treeNode) {
            jQuery("#groupName").val(treeNode.name);
            jQuery('#groupid').val(treeNode.id).trigger('change');
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
            jQuery('#tabs').tabs();
            var nodes = [];
            jQuery('#groupid option').each(function () {
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
            var id = jQuery('#groupid option:selected').val() | jQuery('#groupid option:first-child').val();
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
            $$('[value="所有会员"]').addEvent('click', function () {
                if (this.checked) {
                    var td = $(this).getParent('td');
                    td.getElements('input[value!="所有会员"]').each(function (item) {
                        item.checked = false;
                    });
                }
            });
        });
    </script>
</form>
</body>
</html>
