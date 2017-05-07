<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<T_RadiosEntity>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    网络收音机
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        ol { margin: 0; padding: 0; overflow: hidden; list-style-type: none; }
        ol li { float: left; width: 145px; height: 145px; margin: 5px; overflow: hidden; border: 1px solid #ccc; o}
    </style>
    <ol>
        <%foreach (var item in Model)
          { %>
        <li>
            <img src="<%:item.image %>" />
        </li>
        <%} %>
    </ol>
    <div>
        <%=ViewData["分页字符"] %>
    </div>
    <script type="text/javascript">
        window.addEvent('domready', function () {
          
        });
    </script>
</asp:Content>
