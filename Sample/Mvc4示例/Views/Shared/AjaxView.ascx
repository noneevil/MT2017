<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<T_City>>" %>
<%if(Model!=null){ %>
<table width="100%">
<%
    //foreach (var n in Model)
  for(int i=0;i<Model.Count;i++)
  {
      var n = Model[i];
      %>
    <%if (Convert.ToBoolean(i % 2))
      { %>
        <tr style="background-color:#E8EEF4">
    <%} else{%>
        <tr>
    <%} %>
    <td><%:Html.ActionLink(n.City_Name, "Article", "Home", new {  id = n.City_Id }, new { target = "_blank" })%></td>
    <td>Province_Id:<%:n.Province_Id %></td>
    <td>City_Letter:<%:n.City_Letter %></td>
    <td>Province_Id:<%:n.Province_Id %></td>
    <td width="100" align="center"><%:Html.ActionLink("编辑", "Edit", new { id = n.City_Id }, new { target="_blank" })%></td>
</tr>
<%} %>
</table>
<div class="page">
    <%=ViewData["分页字符"] %>
</div>
<%}else{ %>
    没有数据!
<%} %>