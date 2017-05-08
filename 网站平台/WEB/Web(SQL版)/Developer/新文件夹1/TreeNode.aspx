<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TreeNode.aspx.cs" Inherits="Admin_TreeNode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Plugins/JQuery%20zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet"
        type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <ul class="ztree">
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </ul>
    <asp:Repeater ID="Repeater1" runat="server" 
        onitemcreated="Repeater1_ItemCreated" onitemdatabound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <ul class="ztree">
        </HeaderTemplate>
        <ItemTemplate>
            <%--<li id="treeDemo_1" class="level0">
                <span id="treeDemo_1_switch" class="button level0 switch root_open"></span>
                <a id="treeDemo_1_a" class="level0" title="父节点1 - 展开">
                    <span id="treeDemo_1_ico" class="button ico_open"></span>
                    <span id="treeDemo_1_span">父节点1 - 展开</span>
                </a>
            </li>--%>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    </form>
    <ul class="ztree">
        <li id="treeDemo_1" class="level0" tabindex="0" treenode="">
            <span id="treeDemo_1_switch" class="button level0 switch root_open" title="" treenode_switch=""></span>
            <a id="treeDemo_1_a" class="level0" title="父节点1 - 展开" onclick="" target="_blank" treenode_a="">
                <span id="treeDemo_1_ico" class="button ico_open" title="" treenode_ico=""></span>
                <span id="treeDemo_1_span">父节点1 - 展开</span>
            </a>
            <ul style="display: block;" id="treeDemo_1_ul" class="level0 ">
                <li id="treeDemo_2" class="level1" tabindex="0" treenode="">
                    <span id="treeDemo_2_switch" class="button level1 switch bottom_open" title="" treenode_switch=""></span>
                    <a id="treeDemo_2_a" class="level1" title="父节点11 - 折叠" onclick="" target="_blank" treenode_a="">
                        <span id="treeDemo_2_ico" class="button ico_open" title="" treenode_ico=""></span>
                        <span id="treeDemo_2_span">父节点11 - 折叠</span>
                    </a>
                    <ul id="treeDemo_2_ul" class="level1 ">
                        <li id="treeDemo_3" class="level2" tabindex="0" treenode="">
                            <span id="treeDemo_3_switch" class="button level2 switch bottom_docu" title="" treenode_switch=""></span>
                            <a id="treeDemo_3_a" class="level2" title="叶子节点114" onclick="" target="_blank" treenode_a="">
                            <span id="treeDemo_3_ico" class="button ico_docu" title="" treenode_ico=""></span>
                            <span id="treeDemo_3_span">叶子节点114</span></a>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>
    </ul>
</body>
</html>
