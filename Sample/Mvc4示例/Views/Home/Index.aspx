<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MVC4 - 示例
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        li:hover, li:hover a { color: red; }
    </style>
    <ol>
        <li><%:Html.ActionLink("13种数据提交方式", "SaveForm", "Home")%></li>
        <li><%:Html.ActionLink("分页数据绑定", "List", "Home")%></li>
        <li><%:Html.ActionLink("MvcPaging分页数据绑定", "MvcPagingList", "Home")%></li>
        <li><%:Html.ActionLink("PartialView示例", "PartialView1", "Home")%></li>
        <li><%:Html.ActionLink("数据压缩示例", "Compress", "Home")%></li>
        <li><%:Html.ActionLink("网络收音机", "Radios", "Home")%></li>
        <li><%:Html.ActionLink("表单验证示例", "Validate", "Home")%></li>
        <li><%:Html.ActionLink("Uploadify示例", "Uploadify", "Home")%></li>
        <li><%:Html.ActionLink("MVC JSON 日期格式", "DateTime", "Home")%></li>
        <li><%:Html.ActionLink("实体框架示例", "Index", "EF6")%></li>
        <li><%:Html.ActionLink("日历控件", "Index", "Calendar")%></li>
        <li><%:Html.ActionLink("图片裁剪", "Kroppr", "Home")%></li>
    </ol>
</asp:Content>
