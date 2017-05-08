<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User_Edit.aspx.cs" Inherits="WebSite.Web.User_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建或修改用户</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/Validform/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function () {
            var form = jQuery('#form1').Validform({
                tiptype: 3,
                datatype: {
                    maxzero: function (gets, obj, curform, regxp) {
                        return gets > 0;
                    }
                }
            });
            form.tipmsg.c = ' ';
            form.addRule([
                /*<%if (!IsEdit){ %>*/
                {
                    ele: "#UserName",
                    datatype: "/^[a-zA-Z0-9\-\_]{2,50}$/",
                    ajaxurl: "../Plugin-S/Ajax.ashx?action=validate_user",
                    nullmsg: " ",
                    sucmsg: " ",
                    errormsg: " "
                },
                {
                    ele: "#txtpwd",
                    datatype: "*6-16",
                    nullmsg: " ",
                    sucmsg: " ",
                    errormsg: " "
                },
                {
                    ele: "#UserPass",
                    datatype: "*6-16",
                    recheck: "txtpwd",
                    nullmsg: " ",
                    sucmsg: " ",
                    errormsg: " "
                },
                /*<%} %>*/
                {
                    ele: "#RoleID",
                    datatype: "maxzero",
                    nullmsg: " ",
                    sucmsg: " ",
                    errormsg: " "
                }
            ]);
        });
    </script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <table border="0" align="center" cellpadding="4" cellspacing="0">
        <tr>
            <th>登录名称：</th>
            <td>
                <aspx:TextBox ID="UserName" runat="server" CssClass="text" Width="280"></aspx:TextBox>
                <%--<asp:RequiredFieldValidator ControlToValidate="UserName" CssClass="line1px_5" runat="server" BorderStyle="None" />--%>
                <em class="Validform_checktip">*</em>
            </td>
        </tr>
        <tr>
            <th>用户昵称：</th>
            <td>
                <aspx:TextBox ID="Nickname" runat="server" CssClass="text" Width="280"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>登录密码：</th>
            <td>
                <aspx:TextBox ID="txtpwd" runat="server" CssClass="text" Width="280" TextMode="Password"></aspx:TextBox>
                <em class="Validform_checktip">*</em>
                <%--<asp:RequiredFieldValidator ID="Req1" ControlToValidate="txtpwd" CssClass="line1px_5" runat="server" BorderStyle="None" />--%>
            </td>
        </tr>
        <tr>
            <th>确认密码：</th>
            <td>
                <aspx:TextBox ID="UserPass" runat="server" CssClass="text" Width="280" TextMode="Password"></aspx:TextBox>
                <em class="Validform_checktip">*</em>
                <%--<asp:RequiredFieldValidator ID="Req2" ControlToValidate="UserPass" CssClass="line1px_5" runat="server" BorderStyle="None" />--%>
            </td>
        </tr>
        <tr>
            <th>用户角色：</th>
            <td>
                <aspx:DropDownList ID="RoleID" runat="server" Width="292"></aspx:DropDownList>
                <em class="Validform_checktip">*</em>
                <%--<asp:CompareValidator runat="server" ControlToValidate="RoleID" CssClass="line1px_5" BorderStyle="None" Type="Integer" Operator="GreaterThan" ValueToCompare="0"></asp:CompareValidator>--%>
            </td>
        </tr>
        <tr>
            <th>联系邮箱：</th>
            <td>
                <aspx:TextBox ID="Email" runat="server" CssClass="text"  Width="280"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>联系电话：</th>
            <td>
                <aspx:TextBox ID="TelePhone" runat="server" CssClass="text"  Width="280"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>创建日期：</th>
            <td>
                <aspx:TextBox ID="JoinDate" runat="server" FormatString="{0:yyyy-MM-dd HH:mm:ss}" Enabled="false" CssClass="text" Width="280"></aspx:TextBox>
            </td>
        </tr>
        <tr>
            <th>用户状态：</th>
            <td>
                <aspx:CheckBox ID="IsLock" runat="server" Text="锁定" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CompareValidator ID="Req3" runat="server" CssClass="line1px_2" ErrorMessage="两次密码输入不一致！" ControlToValidate="UserPass"  ControlToCompare="txtpwd"></asp:CompareValidator>
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
                <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
            </td>
        </tr>
    </table>
</form>
</body>
</html>