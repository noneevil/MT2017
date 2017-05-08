<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Form_Edit.aspx.cs" Inherits="WebSite.Web.Debug.Form_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建或修改表单</title>
    <link href="../images/css.css" rel="stylesheet" type="text/css" />
    <script src="../Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Plugins/public.js" type="text/javascript"></script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <aspx:DevelopCKEditor ID="Content" runat="server" CssClass="v2" ToolBarSet="Form" Width="740" Height="460"></aspx:DevelopCKEditor>
    <div style="padding:8px 0; text-align:right;">
        <div style="float:left; text-align:left;">
            <asp:Label ID="Label1" runat="server" Width="185"></asp:Label>
        </div>
        <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
        <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
    </div>
</form>
</body>
</html>