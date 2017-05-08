<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Left.aspx.cs" Inherits="WebSite.Web.Left" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>功能菜单</title>
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="skin/css.css" rel="stylesheet" type="text/css" />
    <script src="skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function () {
            jQuery('ul ul li a').click(function () {
                jQuery('ul ul li a').removeClass('hover');
                jQuery(this).addClass('hover');
                //top.jQuery('#winFrame').focus();
                parent.frames["winFrame"].focus();
                Cookie.write('navigation_cookie', jQuery(this).attr('href'));
            });
            jQuery('a[class^=head]').click(function () {
                var ul = jQuery(this).next('ul');
                if (ul.css('display') == 'none') {
                    ul.slideDown(300);
                    jQuery(this).attr('class', 'head2');
                } else {
                    ul.slideUp(300);
                    jQuery(this).attr('class', 'head');
                }
            });
            var url = Cookie.read("navigation_cookie");
            if (url) parent.frames["winFrame"].location.href = url;
        });
    </script>
</head>
<body onselectstart="return false;" style="padding:0;">
<ul id="menu">
    <asp:Repeater ID="Repeater1" OnItemDataBound="rpt_ItemDataBound" runat="server">
        <ItemTemplate>
            <li>
                  <a class="<%#Convert.ToBoolean(Eval("isopen")) ? "head2" : "head" %>"><%#Eval("title") %></a>
                  <ul<%#Convert.ToBoolean(Eval("isopen")) ? "" : " class=\"hide\"" %>>
                    <asp:Repeater ID="rpt" runat="server">
                        <ItemTemplate>
                            <li><a href="<%# Eval("link_url") %>" target="winFrame"><%#Eval("title")%></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                  </ul>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
</body>
</html>