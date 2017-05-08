<%@ Page Language="C#" AutoEventWireup="true" CodeFile="License.aspx.cs" Inherits="WebSite.Web.Debug.License" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建授权文件</title>
    <link href="../images/css.css" rel="stylesheet" type="text/css" />
    <script src="../Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js" type="text/javascript"></script>
    <script src="../Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq("#tabs").tabs();
            jq("#Count").spinner({ min: 100, max: 255 });
            jq.datepicker.setDefaults(jq.datepicker.regional['zh-TW']);
            jq("#EndTime").datepicker({
                changeMonth: true,
                changeYear: true,
                showAnim: 'clip',
                dateFormat: "yy-mm-dd"
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="tabs">
            <ul style="background-image:none;">
		        <li runat="server" id="tab_1"><a href="#tabs_1">授权参数</a></li>
                <li runat="server" id="tab_2"><a href="#tabs_2">查看授权</a></li>
	        </ul>
	        <div id="tabs_1">
                <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                    <tr>
                        <th width="70px">企业名称：</th>
                        <td>
                            <aspx:TextBox CssClass="text" Width="330" runat="server" ID="CompanyName"></aspx:TextBox>
                            <asp:RequiredFieldValidator runat="server" CssClass="line1px_5" Width="16" ControlToValidate="CompanyName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th width="70px">网站域名：</th>
                        <td>
                            <aspx:TextBox CssClass="text" Width="330" runat="server" ID="DomainName"></aspx:TextBox>
                            <asp:RequiredFieldValidator runat="server" CssClass="line1px_5" Width="16" ControlToValidate="DomainName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th width="70px">到期时间：</th>
                        <td>
                            <aspx:TextBox CssClass="text" Width="330" runat="server" FormatString="{0:yyyy-MM-dd}" ID="EndTime"></aspx:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>加密次数：</th>
                        <td>
                            <aspx:TextBox CssClass="text" BorderStyle="None" Width="305" runat="server" ID="Count"></aspx:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>GUID：</th>
                        <td>
                            <aspx:TextBox CssClass="text" Width="330" ReadOnly="true" runat="server" ID="guid"></aspx:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <aspx:Label ID="Label1" runat="server"></aspx:Label>
                            <aspx:Label ID="Label2" runat="server"></aspx:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <asp:Button ID="btnSave" runat="server" Text="生成" Width="80" onclick="btnSave_Click" />
                            <input type="button" onclick="location.reload();" value="刷新" style="width:80px;" />
                            <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="tabs_2" style="display:;">
                 <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                    <tr>
                        <th width="70px">授权文件：</th>
                        <td>
                            <aspx:FileUpload CssClass="text" Width="330" ID="FileUpload1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <aspx:Label ID="Label3" runat="server"></aspx:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <asp:Button ID="btnView" runat="server" Text="查看" Width="80" onclick="btnView_Click" CausesValidation="False" />
                        </td>
                    </tr>
                  </table>
            </div>
        </div>
    </form>
</body>
</html>