<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    数据压缩示例
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ol>
        <li><%:Html.ActionLink("HTML压缩", "clearHtmlCompress", "Compress", null, new { target = "_blank" })%></li>
        <li><%:Html.ActionLink("7z压缩", "z7Compress", "Compress", null, new { target = "_blank" })%></li>
        <li><%:Html.ActionLink("7z+base64压缩", "base64Compress", "Compress", null, new { target = "_blank" })%></li>
    </ol>
</asp:Content>
