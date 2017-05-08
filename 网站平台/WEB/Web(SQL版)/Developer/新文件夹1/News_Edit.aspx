<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News_Edit.aspx.cs" Inherits="WebSite.Web.News_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发布文章</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <link href="Plugins/JQuery-zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/JQuery-zTree/js/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
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
            jq('#GroupId').val(treeNode.id).trigger('change');
            jq("#tree").fadeOut("fast");
            jq("body").unbind("mousedown", onBodyDown);
        }

        function showMenu() {
            var cityObj = jq("#groupName");
            var cityOffset = jq("#groupName").offset();
            jq("#tree").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() + 1 + "px" }).slideDown("fast");
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
            jq('#GroupId option').each(function () {
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
            var id = jq('#GroupId option:selected').val() | jq('#GroupId option:first-child').val();
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
</head>
<body>
<form id="form1" runat="server">
    <div class="head"><i class="icon0-profile"></i>发布文章</div>
    <table width="100%" cellpadding="4" border="0" cellspacing="0">
        <tr>
            <th width="100">
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
                <input id="groupName" clear="false" runat="server" value="选择分类" readonly class="text" onfocus="showMenu();" style="width:660px;" />
                <asp:CompareValidator runat="server" CssClass="line1px_5" Width="16" Type="Integer" Operator="GreaterThan" ValueToCompare="0" ControlToValidate="GroupId"></asp:CompareValidator>
                <aspx:ListBox ID="GroupId" runat="server" Width="200" Height="200" style="display:none;"></aspx:ListBox>
                <ul id="tree" class="ztree" style="width:660px;display:none; z-index:10; position: absolute; height:280px;"></ul>
            </td>
        </tr>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        <tr>
            <th>
                缩略图：
            </th>
            <td>
                <aspx:BrowseFile ID="imageurl" runat="server" Width="660" CssClass="text"></aspx:BrowseFile>
            </td>
        </tr>
        <tr>
            <th>
                文档附件：
            </th>
            <td>
                <aspx:BrowseFile ID="attacurl" runat="server" Width="660" CssClass="text" ResourceType="文档文件"></aspx:BrowseFile>
            </td>
        </tr>
        <tr>
            <th>
                视频附件：
            </th>
            <td>
                <aspx:BrowseFile ID="videourl" runat="server" Width="660" CssClass="text" ResourceType="音乐视频"></aspx:BrowseFile>
            </td>
        </tr>
        <tr>
            <th>
                参数设置：
            </th>
            <td>
                <table cellspacing="0">
                    <tr>
                        <th>
                            作者：
                        </th>
                        <td>
                            <aspx:TextBox ID="author" runat="server" Width="80" CssClass="text"></aspx:TextBox>
                        </td>
                        <th>
                            来源：
                        </th>
                        <td>
                            <aspx:TextBox ID="source" runat="server" Width="80" CssClass="text"></aspx:TextBox>
                        </td>
                        <th>
                            标题颜色：
                        </th>
                        <td><aspx:ColorPicker runat="server" Width="80" CssClass="text" ID="color" /></td>
                        <td>
                            <aspx:CheckBox ID="nominate" runat="server" Text="推荐" />
                        </td>
                        <td>
                            <aspx:CheckBox ID="hotspot" runat="server" Text="热门" />
                        </td>
                        <td>
                            <aspx:CheckBox ID="focus" runat="server" Text="焦点" />
                        </td>
                        <td>
                            <aspx:CheckBox ID="stick" runat="server" Text="置顶" />
                        </td>
                        <td>
                            <aspx:CheckBox ID="status" runat="server" Text="启用" Checked="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th valign="top">
                <span id="Span2" runat="server">内容摘要</span>：
            </th>
            <td>
                <aspx:CKEditor ID="abstract" runat="server" ToolBarSet="Min" Width="710" Height="100" CssClass="kama"></aspx:CKEditor>
            </td>
        </tr>
        <tr>
            <th valign="top">
                详细内容：
            </th>
            <td>
                <aspx:CKEditor ID="content" runat="server" ToolBarSet="DIY" Width="710" Height="500" CssClass="kama"></aspx:CKEditor>
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <asp:Label ID="Label1" Width="600" runat="server"></asp:Label>
                <asp:RequiredFieldValidator runat="server" CssClass="line1px_2" Width="600" ControlToValidate="title" ErrorMessage="文章标题不能为空！" Display="Static"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" OnClick="btnSave_Click" />&nbsp;
                <input type="button" value="取消" style="width:80px;" onclick="location.replace('news.aspx');" />
                <aspx:HiddenField ID="Url" runat="server" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>
