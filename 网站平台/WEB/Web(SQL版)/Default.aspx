<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Developer/Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="/Developer/Plugins/mootools/mootools-more-1.4.0.1.js" type="text/javascript"></script>
</head>
<body>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    <form id="form1" runat="server">
    <ul>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <li>
                    <%#Eval("title")%><span><%=DateTime.Now%></span></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    </form>
</body>
</html>
