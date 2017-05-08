<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Table_Edit.aspx.cs" Inherits="WebSite.Web.Debug.Table_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建或修改数据表</title>
    <link href="../images/css.css" rel="stylesheet" type="text/css" />
    <script src="../Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Plugins/public.js" type="text/javascript"></script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <table border="0" width="330" cellpadding="4" align="center" cellspacing="0">
        <tr>
            <th width="70px">数据表名：</th>
            <td>
                <aspx:TextBox ID="TableName" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="TableName" CssClass="line1px_5"  runat="server" BorderStyle="None" />
            </td>
        </tr>
        <tr>
            <th>备注信息：</th>
            <td><aspx:TextBox ID="Description" runat="server" CssClass="text" Width="200"></aspx:TextBox></td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:RequiredFieldValidator ControlToValidate="TableName" CssClass="line1px_2" runat="server" ErrorMessage="表名称不能为空！"  Width="135"/>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="TableName" ValidationExpression="[a-zA-Z0-9_]+" ErrorMessage="只允许包含字母、数字以及下划线" CssClass="line1px_2"  Width="185"/>
                <asp:Label ID="Label1" runat="server" Width="185"></asp:Label>
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