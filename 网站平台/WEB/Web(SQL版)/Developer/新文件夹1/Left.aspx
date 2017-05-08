<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Left.aspx.cs" Inherits="WebSite.Web.Left" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>功能菜单</title>
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            $$('ul ul li a').addEvent('click', function () {
                $$('ul ul li a').removeClass('hover');
                this.set('class', 'hover');
                top.$('winFrame').focus();
            });
            $$('a[class^=head]').addEvent('click', function () {
                var tag = this.getNext('ul');
                tag.toggleClass('hide');
                this.set('class', tag.get('class') == 'hide' ? 'head' : 'head2');
            });
        });
    </script>
</head>
<body onselectstart="return false;" style="padding:0;">
<ul id="menu">
    <asp:Repeater ID="Repeater1" OnItemDataBound="rpt_ItemDataBound" runat="server">
        <ItemTemplate>
            <li>
                  <a class="<%#Convert.ToBoolean(Eval("open")) ? "head2" : "head" %>"><%#Eval("menuName") %></a>
                  <ul<%#Convert.ToBoolean(Eval("open")) ? "" : " class=\"hide\"" %>>
                    <asp:Repeater ID="rpt" runat="server">
                        <ItemTemplate>
                            <li><a href="<%# Eval("Action") %>" target="winFrame"><%#Eval("menuName") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                  </ul>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
</body>
</html>