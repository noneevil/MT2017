<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Group_Edit.aspx.cs" Inherits="WebSite.Web.Group_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改分类</title>
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
                return true;
            }
        }
        function onClick(e, treeId, treeNode) {
            jq("#ParentName").val(treeNode.name);
            jq('#ParentID').val(treeNode.id).trigger('change');
        }

        function showMenu() {
            var cityObj = jq("#ParentName");
            var cityOffset = jq("#ParentName").offset();
            jq("#tree").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() - 1 + "px" }).slideDown("fast");
            jq("body").bind("mousedown", onBodyDown);
        }

        function onBodyDown(event) {
            if (!(event.target.id == "treeBtn" || event.target.id == "tree" || jq(event.target).parents("#tree").length > 0)) {
                jq("#tree").fadeOut("fast");
                jq("body").unbind("mousedown", onBodyDown);
            }
        }

        jq(function () {
            var nodes = [];
            jq('#ParentID option').each(function () {
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
            var id = jq('#ParentID option:selected').val() | jq('#ParentID option:first-child').val();
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
        function selectAll(obj, index) {
            var input = $('Field').getElements('input');
            if (input.length > 0) {
                input.each(function (item) {
                    item.checked = true;
                });
            }
        }
        function unSelected(obj, index) {
            var input = $('Field').getElements('input');
            if (input.length > 0) {
                input.each(function (item) {
                    item.checked = !item.checked;
                });
            }
        }
    </script>
</head>
<body scroll="no">
<form id="Form1" runat="server">
    <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">分类名称：</th>
            <td>
                <aspx:TextBox ID="GroupName" runat="server" CssClass="text" Width="350"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="GroupName" CssClass="line1px_5" runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>上级分类：</th>
            <td>
                <input id="ParentName" runat="server" value="顶级分类" readonly class="text" style="width:350px;" />
                <a id="treeBtn" href="javascript:" onclick="showMenu();return false;">选择</a>
                <aspx:ListBox ID="ParentID" runat="server" AutoPostBack="True" style="display:none;" onselectedindexchanged="ParentID_SelectedIndexChanged" Width="362" Height="200"></aspx:ListBox>
                <ul id="tree" class="ztree" style="width:350px;display:none; z-index:10; position: absolute; height:280px;"></ul>
            </td>
        </tr>
        <tr>
            <th>展示模板：</th>
            <td>
                <aspx:DropDownList ID="Template" runat="server" Width="362"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>数据表：</th>
            <td><aspx:TextBox ID="TableName" runat="server" CssClass="text" Width="350"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>字段设置：</th>
            <td align="left">
                <aspx:CheckBoxList ID="Field" runat="server" RepeatDirection="Horizontal" RepeatColumns="5" CellPadding="4"></aspx:CheckBoxList>
                <div style="padding:5px; width:350px; line-height:23px; height:23px; vertical-align:middle; background-color:#ADD2DA; font-weight:bold;">
                    <a href="javascript:" onclick="selectAll();">全选</a> |
                    <a href="javascript:" onclick="unSelected();">反选</a>
                </div>
            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:RequiredFieldValidator ControlToValidate="GroupName" CssClass="line1px_2" runat="server" ErrorMessage="分类名称名称不能为空！" Width="335"/>
                <asp:Label ID="Label1" runat="server" Width="335"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
                <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>