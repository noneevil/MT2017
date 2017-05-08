<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Template.aspx.cs" Inherits="Developer_Template" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="css/reset.css" rel="stylesheet" type="text/css" />
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/widgets.css" rel="stylesheet" type="text/css" />
    <link href="css/icons.css" rel="stylesheet" type="text/css" />
    <link href="css/uniform.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="css/lte-ie7.js"></script><![endif]-->
    <script src="js/mootools.js" type="text/javascript"></script>
    <script src="js/public.js" type="text/javascript"></script>
    <script src="js/mooniform.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            new Mooniform($$('input[type="radio"],input[type="checkbox"]'));
        });
    </script>
</head>
<body>
<form runat="server">
<div class="widget">
    <div class="widgettitle"><i class="icon0-cog-2"></i>网站参数设置</div>
    <div class="widgetcontent" style="padding:10px;">
        <div id="tabs" class="ui-tabs">
            <ul class="ui-tabs-nav">
                <li class="ui-state-active"><a href="#tabs-1">Tab 1</a></li>
                <li><a href="#tabs-2">Tab 2</a></li>
                <li><a href="#tabs-3">Tab 3</a></li>
            </ul>
            <div id="tabs-1" class="ui-tabs-panel">
                <select style="width:300px;">
                    <option>aaaaaaaaaaaaaaa</option>
                    <option>aaaaaaaaaaaaaaa</option>
                    <option>aaaaaaaaaaaaaaa</option>
                    <option>aaaaaaaaaaaaaaa</option>
                    <option>aaaaaaaaaaaaaaa</option>
                </select>
                <br /><br />
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="测试1" Value="1" Enabled="false"></asp:ListItem>
                    <asp:ListItem Text="测试2" Value="2" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="测试3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="测试4" Value="4"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="测试1" Value="1" Enabled="false"></asp:ListItem>
                    <asp:ListItem Text="测试2" Value="2" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="测试3" Value="3"></asp:ListItem>
                    <asp:ListItem Text="测试4" Value="4"></asp:ListItem>
                </asp:CheckBoxList>
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <br /><br />
                <input type="submit" class="ui-btn" />
                <input type="reset" class="ui-btn" />
                <input type="button" class="ui-btn" value="提交" />
                <button class="ui-btn">提交</button>
            </div>
            <div id="tabs-2" class="ui-tabs-panel ui-tabs-hide">
                Your content goes here for tab 2
            </div>
            <div id="tabs-3" class="ui-tabs-panel ui-tabs-hide">
                Your content goes here for tab 3
            </div>
        </div>
    </div>
</div>
</form>
</body>
</html>