<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%foreach (var item in Model)
  { %>
<li>
    <img src="/Content/159130308.h264_1.jpg" />
    <div>
        <%:item.ID%>:<%:item.Title%>
        <p>
            <%:item.PubDate%></p>
    </div>
</li>
<%} %>
