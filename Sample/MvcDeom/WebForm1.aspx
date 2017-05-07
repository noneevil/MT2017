<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="MvcSite.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post">
    <%-- action="/Upload.axd"--%>
    <div>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="100%" Height="300"></asp:TextBox>
    </div>
    <div style="padding: 10px">
        <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Width="100%" Height="50"></asp:TextBox>
    </div>
    <asp:Button ID="登录" runat="server" Text="登录" OnClick="登录_Click" />
    <%--<div>
        <p><input type="file" id="File1" runat="server" /></p>       
        <p><input type="file" id="File2" runat="server" /></p>
        <p><input type="text" id="Text1" runat="server" /></p>
        <p><textarea id="Textarea1" runat="server"></textarea></p>
        <p><input type="submit" /></p>
    </div>--%>
    </form>
</body>
</html>
