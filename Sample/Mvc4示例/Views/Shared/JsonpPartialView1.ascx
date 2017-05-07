<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<T_City>>" %>
<table width="100%">
<%foreach (var n in Model)
  { %>
<tr>
    <td><%:Html.ActionLink(n.City_Name, "Article", "Home", new {  id = n.City_Id }, new { target = "_blank" })%></td>
    <td>Province_Id:<%:n.Province_Id %></td>
    <td>City_Letter:<%:n.City_Letter %></td>
    <td>Province_Id:<%:n.Province_Id %></td>
</tr>
<%} %>
</table>
<div class="page">
    <%=ViewData["测试"] %>
</div>
<textarea><%:ViewData["Request"] %></textarea>