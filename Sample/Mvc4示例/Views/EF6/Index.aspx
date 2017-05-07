<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<T_NewsEntities>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    实体框架示例
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <%foreach (var item in Model) { %>
        <tr>
           <td><%:item.ID %></td>
           <td><%:item.Title %></td>
           <td align="center"><%:item.ImageUrl %></td>
           <td width="150" align="center"><%:item.PubDate.ToString("yyyy-MM-dd HH:mm:ss") %></td>
           <td width="150" align="center"><%:item.EditDate.ToString("yyyy-MM-dd HH:mm:ss") %></td>
        </tr>
        <%} %>
    </table>
</asp:Content>
