<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ViewPage2</title>
</head>
<body>
    <%using (Html.BeginForm("SaveForm", "Home", FormMethod.Post, new { target = "winframe", enctype = "multipart/form-data" }))
      {%>
    <div>
        <%:Html.Editor("User") %></div>
    <div>
        <%:Html.Editor("PassWord")%></div>
    <input type="file" id="UserHeadImg" name="UserHeadImg" />
    <input type="submit" />
    <iframe id="winframe" name="winframe" style="width:500px; height:500px;"></iframe>
    <%} %>
</body>
</html>
