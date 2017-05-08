<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Field_Edit.aspx.cs" Inherits="WebSite.Web.Debug.Field_Edit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>创建或修改字段</title>
    <link href="../images/css.css" rel="stylesheet" type="text/css" />
    <script src="../Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Plugins/public.js" type="text/javascript"></script>
    <script type="text/javascript">
        jq(function () {
            jq("#tabs").tabs();
        });
    </script>
</head>
<body scroll="no">
<form id="form1" runat="server">
    <div id="tabs">
        <ul style="background-image:none;">
		    <li runat="server" id="tab_1"><a href="#tabs_1">基本配置</a></li>
            <li runat="server" id="tab_2"><a href="#tabs_2">布局设置</a></li>
            <li runat="server" id="tab_3"><a href="#tabs_3">数据源设置</a></li>
	    </ul>
	    <div id="tabs_1" style="display:none;">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                <tr>
                    <th width="100">字段名称:</th>
                    <td>
                        <aspx:TextBox ID="FieldName" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="FieldName" CssClass="line1px_2" Width="150" runat="server" Text="字段名称不能为空!"/>
                    </td>
                </tr>
                <tr>
                    <th>显示名称:</th>
                    <td><aspx:TextBox ID="Description" runat="server" CssClass="text" Width="200"></aspx:TextBox></td>
                </tr>
                <tr>
                    <th>数据类型:</th>
                    <td>
                        <aspx:DropDownList ID="DataType" Width="212" runat="server"></aspx:DropDownList>
                        <aspx:HiddenField ID="DataTypeValue" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>数据长度:</th>
                    <td>
                        <aspx:TextBox runat="server" ID="Length" CssClass="text" Width="200" Text="0"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>默认值:</th>
                    <td>
                        <aspx:TextBox ID="DefaultValue" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>控件类型:</th>
                    <td>
                        <aspx:DropDownList ID="Control" Width="212" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="Control_SelectedIndexChanged">
                        </aspx:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>验证正则:</th>
                    <td>
                        <aspx:TextBox ID="Regex" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>虚拟字段</th>
                    <td>
                        <aspx:RadioButtonList ID="isVirtual" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                        </aspx:RadioButtonList>
                    </td>
                </tr>
            </table>
            <div style="padding:0 30px;">
                <asp:RegularExpressionValidator runat="server" ControlToValidate="FieldName" ValidationExpression="[a-zA-Z0-9_]+" ErrorMessage="字段名称只允许包含字母、数字以及下划线" CssClass="line1px_2"/>
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </div>
        </div>
        <div id="tabs_2" runat="server" style="display:none;">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                <tr>
                    <th width="100">显示列数:</th>
                    <td colspan="3">
                        <aspx:TextBox ID="RepeatColumns" runat="server" CssClass="text" Width="200" Text="1"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>布局方向:</th>
                    <td>
                        <aspx:RadioButtonList ID="RepeatDirection" runat="server" RepeatDirection="Horizontal">
                        </aspx:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th>布局模式:</th>
                    <td>
                        <aspx:RadioButtonList ID="RepeatLayout" runat="server" RepeatDirection="Horizontal">
                        </aspx:RadioButtonList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="tabs_3" runat="server" style="display:none;">
            <table border="0" width="100%" cellpadding="4" align="center" cellspacing="0">
                <tr>
                    <th width="100">数据类型:</th>
                    <td>
                        <aspx:DropDownList ID="DataSourceType" runat="server" Width="212" 
                            onselectedindexchanged="DataSourceType_SelectedIndexChanged" AutoPostBack="true">
                        </aspx:DropDownList>
                    </td>
                </tr>

                <tbody id="_customsql" runat="server">
                    <tr>
                        <th>SQL语句：</th>
                        <td>
                            <aspx:TextBox ID="SQL" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>标题字段：</th>
                        <td>
                            <aspx:TextBox ID="TextField" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>值 字 段：</th>
                        <td>
                            <aspx:TextBox ID="ValueField" runat="server" CssClass="text" Width="200"></aspx:TextBox>
                        </td>
                    </tr>
                </tbody>

                <tbody id="_listitems" runat="server">
                    <tr>
                        <th>选项数目:</th>
                        <td align="left">
                            <aspx:TextBox ID="ListItemCount" runat="server" CssClass="text" Width="135"></aspx:TextBox>
                            <asp:Button ID="Button2" Text="设置" CssClass="button" Width="60" onclick="SetCount_OnClick" CausesValidation="false" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th>列表选项:</th>
                        <td align="left">
                            <div style="height:250px; overflow-y:auto; border:1px solid #ccc;">
                            <asp:Repeater ID="Repeater1" runat="server">
                                <ItemTemplate>
                                    <table border="0" cellspacing="0" cellpadding="4" style="border-bottom:1px dotted #ccc;">
                                        <tr>
                                            <td rowspan="3" style="font-weight:bold; text-align:center;">
                                                <%#Container.ItemIndex + 1 %>
                                            </td>
                                            <td align="right" width="100">选项标题：</td>
                                            <td>
                                                <aspx:TextBox ID="t1" runat="server" CssClass="text" Width="100" Text='<%#Eval("Text") %>'></aspx:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="t1" CssClass="line1px_5"  runat="server" BorderStyle="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">选项值：</td>
                                            <td>
                                                <aspx:TextBox ID="t2" runat="server" CssClass="text" Width="100" Text='<%#Eval("Value") %>'></aspx:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="t2" CssClass="line1px_5"  runat="server" BorderStyle="None" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">默认选定：</td>
                                            <td>
                                                <aspx:CheckBox ID="c1" runat="server" Checked='<%#Eval("Selected") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                            </div>
                        </td>
                    </tr>
                </tbody>                
            </table>
        </div>
        <div style="padding:5px; text-align:center;">
            <asp:Button ID="btnSave" runat="server" Text="保存" Width="80" onclick="btnSave_Click" />
            <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
        </div>
    </div>
</form>
</body>
</html>