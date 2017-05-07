<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateTime>" %>
<dl class="calendar">
    <dt>
        <%if (Model > DateTime.Now.Date.AddDays(-(DateTime.Now.Day) + 1))
            {  %>
            <input type="button" onclick="GetCalendar('<%:Model.AddMonths(-1).ToString("yyyy-MM-dd") %>');" />
        <%}else{ %>
            <input type="button" class="disable" />
        <%} %>
        <%:Model.ToString("yyyy年MM月") %>
        <input type="button" class="down" onclick="GetCalendar('<%:Model.AddMonths(1).ToString("yyyy-MM-dd") %>');" />
    </dt>
    <dd>
        <%=ViewData["Calendar"] %>
    </dd>
</dl>