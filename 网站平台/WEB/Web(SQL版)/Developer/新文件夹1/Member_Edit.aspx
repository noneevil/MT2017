<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Member_Edit.aspx.cs" Inherits="WebSite.Web.Member_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加或修改会员资料</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq.datepicker.setDefaults(jq.datepicker.regional['zh-TW']);
            var dates = jq("#birthday");
            dates.datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'clip',
                dateFormat: "yy-mm-dd"
            });
            var src = 'url(system/Thumbnail.aspx?w=165&h=100&file=' + jq('#picture').attr('value') + ')';
            jq('#photo').css({ 'background-image': src });
        });
    </script>
</head>
<body scroll="no">
<form id="Form1" runat="server">
    <table border="0" width="96%" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">会员名称：</th>
            <td>
                <aspx:TextBox ID="membername" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="membername" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
            <th width="100px"></th>
            <td rowspan="3">
                <asp:Image ID="photo" ToolTip="头像" runat="server" ImageUrl="images/transparent.gif" Width="165" Height="100" />
            </td>
        </tr>
        <tr>
            <th>类型：</th>
            <td>
                <aspx:DropDownList ID="membertype" runat="server" Width="162"></aspx:DropDownList>
            </td>
        </tr>
        <tr>
            <th>头像：</th>
            <td><aspx:FileUpload ID="picture" CssClass="text" Width="161" runat="server" /></td>
        </tr>
        <tr>
            <th>真实姓名：</th>
            <td><aspx:TextBox ID="username" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
            <th>等级：</th>
            <td><aspx:TextBox ID="memberlevel" runat="server" CssClass="text" Width="150" Text="0"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>登录密码：</th>
            <td>
                <aspx:TextBox ID="password" TextMode="Password" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ID="RequiredPassword" ControlToValidate="password" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
            <th>生日：</th>
            <td>
                <aspx:TextBox ID="birthday" runat="server" FormatString="{0:yyyy-MM-dd}" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="birthday" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>坐机：</th>
            <td><aspx:TextBox ID="phone" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
            <th>手机：</th>
            <td><aspx:TextBox ID="mobile" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>邮箱：</th>
            <td>
                <aspx:TextBox ID="email" runat="server" CssClass="text" Width="150"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="email" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
            <th>QQ：</th>
            <td><aspx:TextBox ID="qq" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>身份证：</th>
            <td><aspx:TextBox ID="idcard" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
            <th>联系地址：</th>
            <td><aspx:TextBox ID="address" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>注册日期：</th>
            <td><aspx:TextBox ID="joindate" Enabled="false" FormatString="{0:yyyy-MM-dd HH:mm:ss}" runat="server" CssClass="text" Width="150"></aspx:TextBox>
            </td>
            <th>最近登录时间：</th>
            <td><aspx:TextBox ID="lastsigntime" Enabled="false" FormatString="{0:yyyy-MM-dd HH:mm:ss}" runat="server" CssClass="text" Width="150"></aspx:TextBox></td>
        </tr>
        <tr>
            <th>验证</th>
            <td>
                <aspx:CheckBox ID="ValidateMail" runat="server" Enabled="false" Text="通过邮箱验证"/>
                <aspx:CheckBox ID="ValidateMobile" runat="server" Enabled="false" Text="通过手机验证"/>
            </td>
            <th>性别：</th>
            <td>
                <aspx:RadioButtonList ID="sex" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">男</asp:ListItem>
                    <asp:ListItem Value="0">女</asp:ListItem>
                </aspx:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th>状态</th>
            <td colspan="3">
                <aspx:CheckBox ID="status" runat="server" Text="启用"/>
            </td>
        </tr>
        <tr>
            <th></th>
            <td colspan="3">
                <asp:RequiredFieldValidator ControlToValidate="membername" CssClass="line1px_2" runat="server" ErrorMessage="会员登录名称不能为空！" />
                <asp:Label ID="Label1" runat="server" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="4" style="text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
                <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>