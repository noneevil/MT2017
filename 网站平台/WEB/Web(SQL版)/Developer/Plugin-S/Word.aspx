<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Word.aspx.cs" Inherits="WebSite.Web.Word" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Word转换换件</title>
    <link href="/Developer/skin/css.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="4" cellspacing="0">
            <tr>
                <td>选择Word文件:</td>
                <td>
                    <asp:FileUpload ID="FileUpload1" CssClass="file" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" CssClass="line1px_2" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="Button1" runat="server" CssClass="file" Width="80" Height="25" Text="转换" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>
        <textarea id="txt" runat="server" style="display: none;" cols="40"></textarea>
    </form>
</body>
</html>
