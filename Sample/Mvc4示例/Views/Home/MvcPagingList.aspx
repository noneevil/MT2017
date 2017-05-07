<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IPagedList<T_CityEntities>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    分页数据绑定示例
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <%--<%//foreach (var n in Model)
            for (int i = 0; i < Model.Count; i++)
            {
                var n = Model[i]; %>
       <%if (Convert.ToBoolean(i % 2))
          { %>
            <tr style="background-color:#E8EEF4">
        <%} else{%>
            <tr>
        <%} %>
            <td><%:Html.ActionLink(n.City_Name, "Article", "Home", new {  id = n.City_Id }, new { target = "_blank" })%></td>
            <td>Province_Id:<%:n.Province_Id %></td>
            <td>City_Letter:<%:n.City_Letter %></td>
        </tr>
        <%} %>--%>
        <%foreach (var item in Model) { %>
            <tr>
                <td>
                    <%:item.City_Name %>
                </td>
            </tr>
        <%} %>
    </table>
    <div class="black2">
       <%:Html.Pager(Model.PageSize, Model.PageIndex, Model.TotalItemCount) %>
    </div>
</asp:Content>