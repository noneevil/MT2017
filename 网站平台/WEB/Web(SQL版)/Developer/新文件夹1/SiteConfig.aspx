<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteConfig.aspx.cs" Inherits="WebSite.Web.SiteConfig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>站点配置</title>
    <link href="images/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="Plugins/jQuery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="Plugins/public.js" type="text/javascript"></script>
</head>
<body>
<form runat="server">
    <div class="head"><i class="icon0-cog-2"></i>网站参数设置</div>
    <div id="tabs">
        <ul>
		    <li><a href="#tabs-1">基本配置</a></li>
            <li><a href="#tabs-2">数据库配置</a></li>
            <li><a href="#tabs-3">会员设置</a></li>
            <li><a href="#tabs-4">邮件设置</a></li>
	    </ul>
	    <div id="tabs-1">
            <table border="0" width="96%" align="center" cellpadding="4" cellspacing="0" style="border-collapse: collapse;">
                <tr>
                    <th>站点语言：</th>
                    <td>
                        <aspx:DropDownList ID="Language" runat="server" onselectedindexchanged="language_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem value="CN">中文</asp:ListItem>
                            <asp:ListItem value="US">英文</asp:ListItem>
                            <asp:ListItem value="TW">繁体</asp:ListItem>
                        </aspx:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>站点名称：</th>
                    <td>
                        <aspx:TextBox ID="SiteName" CssClass="text" ToolTip="站点名称" runat="server"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>站点域名：</th>
                    <td>
                        <aspx:TextBox ID="SiteURL" CssClass="text" ToolTip="站点域名" runat="server"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>SEO关键字：</th>
                    <td>
                        <aspx:TextBox ID="KeyWords" ToolTip="提示：用于表示搜索引擎中输入指定的关键字能更容易找到网站信息,(可填写多个,每项用,号分开)。" runat="server" TextMode="MultiLine"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>SEO介绍信息：</th>
                    <td>
                        <aspx:TextBox ID="Description" ToolTip="提示：用于表示搜索引擎搜索到本网站是显示的部分介绍信息。" runat="server" TextMode="MultiLine"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>版权信息：</th>
                    <td>
                        <aspx:TextBox ID="Copyright" ToolTip="版权信息,一般为网站底部信息。" runat="server" TextMode="MultiLine"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td><p>提示：用于表示网站底部显示的版权相关信息及联系电话信息等。</p></td>
                </tr>  
                <tr>
                    <th>私钥：</th>
                    <td>
                        <aspx:TextBox ID="Keys" CssClass="text" ToolTip="私钥" runat="server"></aspx:TextBox>
                    </td>
                </tr>
            </table>
        </div>
	    <div id="tabs-2">
            <table border="0" width="96%" align="center" cellpadding="4" cellspacing="0" style="border-collapse: collapse;">
                <tr>
                    <th>数据库类型：</th>
                    <td>
                        <aspx:DropDownList ID="DataType" runat="server"></aspx:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>连接字符：</th>
                    <td>
                        <aspx:TextBox ID="ConnectionString" CssClass="text" runat="server" ToolTip="数据库连接字符串."></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>视图存储:</th>
                    <td>
                        <aspx:DropDownList ID="ViewStateMode" runat="server"></aspx:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
	    <div id="tabs-3">
            <table border="0" width="96%" align="center" cellpadding="4" cellspacing="0" style="border-collapse: collapse;">
                <tr>
                    <th>留言信息：</th>
                    <td>
                        <aspx:CheckBox ID="AuditBook" Text="审核/不审核" ToolTip="留言信息是否需要审核." runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>会员注册：</th>
                    <td>
                        <aspx:CheckBox ID="AuditReg" Text="审核/不审核" ToolTip="会员注册后是否需要审核." runat="server" />
                    </td>
                </tr>
                <tr>
                    <th valign="top">过滤关键字：</th>
                    <td>
                        <aspx:TextBox ID="BadKeywords" Height="350" ToolTip="提示：用于过滤前台网站上传时提交的不良信息,多项用,号分隔。" runat="server" TextMode="MultiLine"></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>替换字符：</th>
                    <td>
                        <aspx:TextBox ID="SubStitute" CssClass="text" Width="100" runat="server" ToolTip="将过滤关键字替换为该字符."></aspx:TextBox>注：默认为*号为替换。
                    </td>
                </tr>
            </table>    
        </div>
        <div id="tabs-4">
            <table border="0" width="96%" align="center" cellpadding="4" cellspacing="0" style="border-collapse: collapse;">
                <tr>
                    <th>服务器：</th>
                    <td>
                        <aspx:TextBox ID="Smtp" CssClass="text" runat="server" ToolTip="用于发送邮件的服务器地址,如：smtp.163.com."></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>邮件账户：</th>
                    <td>
                        <aspx:TextBox ID="Email" CssClass="text" runat="server" ToolTip="发送邮件时使用的账号,如：abc@163.com."></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>用户名：</th>
                    <td>
                        <aspx:TextBox ID="MailUser" CssClass="text" runat="server" ToolTip="登录邮件服务器时的用户名称."></aspx:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>账户密码：</th>
                    <td>
                        <aspx:TextBox ID="MailPass" CssClass="text" runat="server" ToolTip="登录邮件服务器时的密码."></aspx:TextBox>
                    </td>
                </tr>
            </table>    
        </div>
        <div style="padding:3px; text-align:center;">
            <asp:Button ID="btnSave" hidefocus="hidefocus" Width="100" runat="server" Text="保存" onclick="btnSave_Click" />
        </div>
    </div>
</form>
<script type="text/javascript">
    jq(function () {
        var keywords = <%=ViewState["keywords"] %>;
        jq('#BadKeywords').val(keywords.join(','));

        var options = <%=ViewState["options"] %>;
        jq('#DataType').change(function () {
            var val = jq('#DataType').val();
            jq.each(options,function(i,o){
                if(o.Name==val){
                    jq('#ConnectionString').val(o.ConnectionString);
                }
            });
        });

        jq("#tabs").tabs();
        jq("[title]").tooltip();
    });
</script>
</body>
</html>