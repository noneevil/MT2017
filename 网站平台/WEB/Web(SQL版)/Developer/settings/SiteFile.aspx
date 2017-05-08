<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SiteFile.aspx.cs" Inherits="WebSite.Web.SiteFile" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>文件管理</title>
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <!--[if lte IE 7]><script src="../skin/Plugins/fontsicon/lte-ie7.js"></script><![endif]-->
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/public.js" type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
    <asp:MultiView ID="MultiView1" runat="server">
    <asp:View ID="View1" runat="server">
        <div class="head"><i class="icon0-file"></i>文件管理</div>
        <ul id="icons" class="ui-widget-header ui-helper-clearfix">
            <li class="ui-state-default" title="后退" url="<%:BackUrl%>"><span class="ui-icon ui-icon-arrowthick-1-w"></span></li>
            <li class="ui-state-default" title="前进"><span class="ui-icon ui-icon-arrowthick-1-e"></span></li>
            <li class="ui-state-default" title="全选"><span class="ui-icon ui-icon-arrow-4-diag"></span></li>
            <li class="ui-state-default" title="反选"><span class="ui-icon ui-icon-arrow-4"></span></li>
            <li class="ui-state-default" title="上传"><span class="ui-icon none"><i class="icon0-upload-2"></i></span></li>
            <li class="ui-state-default" title="新建文件夹"><span class="ui-icon ui-icon-folder-open"></span></li>
            <li class="ui-state-default" title="解压文件"><span class="ui-icon none"><i class="icon0-file-zip"></i></span></li>
            <li class="ui-state-default" title="打包"><span class="ui-icon none"><i class="icon0-bag"></i></span></li>
            <li class="ui-state-default" title="删除"><span class="ui-icon ui-icon-trash"></span></li>
            <li class="ui-state-default" title="列表"><span class="ui-icon none"><i class="icon0-list-3"></i></span></li>
            <li class="ui-state-default" title="图标"><span class="ui-icon none"><i class="icon0-grid"></i></span></li>
            <li class="ui-state-default" title="文件类型"><span class="ui-icon none"><i class="icon0-stack"></i></span></li>
            <li class="ui-state-default" title="刷新"><span class="ui-icon none"><i class="icon0-refresh-2"></i></span></li>
            <li class="ui-state-default" title="剪切"><span class="ui-icon none"><i class="icon0-cut"></i></span></li>
            <li class="ui-state-default" title="复制"><span class="ui-icon none"><i class="icon0-copy"></i></span></li>
            <li class="ui-state-default" title="粘贴"><span class="ui-icon none"><i class="icon0-paste-3"></i></span></li>
            <li style="display:none;">
                <asp:Button ID="btnCommand" runat="server" onclick="btnCommand_Click" />
            </li>
        </ul>
        <asp:Repeater ID="Repeater1" runat="server" onitemcommand="Repeater1_ItemCommand" onitemdatabound="Repeater1_ItemDataBound">
            <HeaderTemplate>
                <table class="table" width="100%" border="0" bordercolor="#add2da" cellpadding="4" cellspacing="0">
                    <thead>
                        <tr>
                            <td width="50px" align="left">选择</td>
                            <td>名称</td>
                            <td width="120px">创建日期</td>
                            <td width="120px">修改日期</td>
                            <td width="100px">大小</td>
                            <td width="60px">操作</td>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id="row" runat="server" style="text-align:center;">
                        <td align="left">
                            <input id="ID" class="checkbox" runat="server" type="checkbox" value='<%#Server.UrlEncode(Eval("FullName").ToString())%>' />
                            <%#Container.ItemIndex+1 %>
                        </td>
                        <td align="left"></td>
                        <td><%#Eval("CreationTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                        <td><%#Eval("LastWriteTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                        <td></td>
                        <td>
                            <asp:ImageButton CssClass="img" ID="save" ImageUrl="../skin/icos/disk_min.gif" runat="server" ToolTip="保存" CommandName="save" Visible="false" />
                            <asp:ImageButton CssClass="img" ID="edit" ImageUrl="../skin/icos/write_enabled.gif" runat="server" ToolTip="重命名" />
                            <asp:ImageButton CssClass="img" ID="del" ImageUrl="../skin/icos/del_enabled.gif" runat="server" ToolTip="删除" CommandName="del" />
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Repeater ID="Repeater2" runat="server" onitemcommand="Repeater1_ItemCommand" onitemdatabound="Repeater2_ItemDataBound">
            <HeaderTemplate><ul id="file"></HeaderTemplate>
            <ItemTemplate>
                <li>
                    <div>
                        <asp:ImageButton ID="del" runat="server" ImageUrl="../skin/icos/del.gif" ToolTip="删除" CommandName="del" />
                        <asp:ImageButton ID="edit" runat="server" ImageUrl="../skin/icos/edit.gif" ToolTip="重命名" />
                        <asp:ImageButton ID="cut" runat="server" ImageUrl="../skin/icos/cut.gif" ToolTip="剪切" CommandName="del" Visible="false" />
                        <asp:ImageButton ID="save" runat="server" ImageUrl="../skin/icos/save.gif" ToolTip="保存" CommandName="save" Visible="false" />
                    </div>
                    <asp:HyperLink ID="Link" runat="server"></asp:HyperLink>
                    <i>
                        <input id="ID" class="checkbox" runat="server" type="checkbox" value='<%#Server.UrlEncode(Eval("FullName").ToString())%>' style="display:none;" />
                        <%#Eval("Name") %>
                    </i>
                    <b></b>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
                <div class="clear"></div>
            </FooterTemplate>
        </asp:Repeater>
        <link href="../skin/Plugins/lightbox/themes/default/jquery.lightbox.css" rel="stylesheet" type="text/css" />
        <script src="../skin/Plugins/lightbox/jquery.lightbox.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            var view;
            window.addEvent('domready', function () {
                jQuery('.lightbox').lightbox();
                var li = $$('#icons li');
                /*后退*/
                $(li[0]).addEvent('click', function () {
                    if ($(this).get('url') != '') history.go(-1);
                });
                /*前进*/
                $(li[1]).addEvent('click', function () {
                    history.forward();
                });
                /*全选*/
                $(li[2]).addEvent('click', function () {
                    $$('.checkbox').each(function (item, index) {
                        item.checked = true;
                        var li = $(item).getParent('li');
                        if (li) li.addClass('checked');
                    });
                });
                /*反选*/
                $(li[3]).addEvent('click', function () {
                    $$('.checkbox').each(function (item, index) {
                        item.checked = !item.checked;
                        var li = $(item).getParent('li');
                        if (li) item.checked ? li.addClass('checked') : li.removeClass('checked');
                    });
                });
                /*上传*/
                $(li[4]).addEvent('click', function () {
                    dialogIFrame({ url: 'SiteFile.aspx?action=upload&dir=<%:RelativeUrl %>', title: '上传文件', width: 507, height: 365 });
                });
                /*新建文件夹*/
                $(li[5]).addEvent('click', function () {
                    dialogIFrame({ url: 'SiteFile.aspx?action=add&dir=<%:RelativeUrl %>', title: '新建文件夹', width: 380, height: 220 });
                });
                /*解压文件*/
                $(li[6]).addEvent('click', function () {
                    dialogIFrame({ url: 'SiteFile.aspx?action=unrar&dir=<%:RelativeUrl %>', title: '解压文件', width: 380, height: 220 });
                });
                /*打包*/
                $(li[7]).addEvent('click', function () {
                    var html = '<div><table cellpadding="0" cellspacing="0"><tr><td width="55" valign="middle">';
                    html += '<img id="loading-img" src="../skin/dialog/loading.gif" />';
                    html += '</td><td valign="middle" id="loading-text"></td></tr></table></div>';
                    var _dialog = jQuery(html).dialog({
                        modal: true,
                        title: "提示",
                        autoOpen: false,
                        height: 80,
                        resizable: false,
                        closeOnEscape: false,
                        close: function () {
                            jQuery(this).dialog("destroy");
                        },
                        buttons: {
                            '确定': function () {
                                jQuery(this).dialog("close");
                            }
                        }
                    });

                    new Request.JSON({
                        url: '../Plugin-S/Ajax.ashx',
                        headers: { Action: 'backup_sitefile' },
                        onRequest: function () {
                            jQuery(".ui-dialog-titlebar,.ui-dialog-buttonpane").hide();
                            jQuery('#loading-text').html('程序正在打包中,请稍后...');
                            _dialog.dialog('open');
                        },
                        onSuccess: function (result) {
                            jQuery(".ui-dialog-titlebar,.ui-dialog-buttonpane").show();
                            jQuery('#loading-text').html(result.Text);
                            jQuery('#loading-img').attr('src', '../skin/dialog/' + result.Ico + '.gif');
                        },
                        onFailure: function () {
                            jQuery(".ui-dialog-titlebar,.ui-dialog-buttonpane").show();
                            jQuery('#loading-text').html('打包出错!');
                            jQuery('#loading-img').attr('src', '../skin/dialog/failure.gif');
                        }
                    }).send();
                });
                /*删除*/
                $(li[8]).addEvent('click', function () {
                    var count = $$('.checkbox:checked').length;
                    if (count == 0) {
                        dialogMessage('未选择删除对象.');
                    }
                    else {
                        dialogConfirm({ el: '#btnCommand', text: '确定要删除选中的文件吗?', data: -1 });
                    }
                });
                /*列表*/
                $(li[9]).addEvent('click', function () {
                    Cookie.write('display_mode', '1');
                    location.href = location.href;
                });
                /*图标*/
                $(li[10]).addEvent('click', function () {
                    Cookie.write('display_mode', '0');
                    location.href = location.href;
                });
                /*设置文件类型*/
                $(li[11]).addEvent('click', function () {
                    dialogIFrame({ url: 'SiteFile.aspx?action=filetype', title: '文件类型', width: 300, height: 180 });
                });
                /*刷新*/
                $(li[12]).addEvent('click', function () {
                    location.href = location.href;
                });
                /*剪切*/
                $(li[13]).addEvent('click', function () {
                    var count = $$('.checkbox:checked').length;
                    if (count == 0) {
                        dialogMessage('未选择剪切对象.');
                    }
                    else {
                        var SelectFiles = { cmd: 'cut', Files: [] };
                        $$('.checkbox:checked').each(function (item) {
                            SelectFiles.Files.push(item.get('value'));
                        });
                        top.$('SelectFiles').set('value', JSON.encode(SelectFiles));
                    }
                });
                /*复制*/
                $(li[14]).addEvent('click', function () {
                    var count = $$('.checkbox:checked').length;
                    if (count == 0) {
                        dialogMessage('未选择复制对象.');
                    }
                    else {
                        var SelectFiles = { cmd: 'copy', Files: [] };
                        $$('.checkbox:checked').each(function (item) {
                            SelectFiles.Files.push(item.get('value'));
                        });
                        top.$('SelectFiles').set('value', JSON.encode(SelectFiles));
                    }
                });
                /*粘贴*/
                $(li[15]).addEvent('click', function () {
                    var val = top.$('SelectFiles').get('value');
                    if (val == '') return;
                    var SelectFiles = JSON.decode(val);
                    new Request.JSON({
                        url: 'SiteFile.aspx?dir=<%:Server.UrlEncode(Request["dir"]) %>&action=' + SelectFiles.cmd,
                        data: val,
                        onSuccess: function (result) {
                            dialogMessage(result.Text, result.Ico);
                            top.$('SelectFiles').set('value', '');
                        },
                        onFailure: function () {
                            dialogMessage('服务器错误!', 'failure');
                        }
                    }).send();
                });
                /*图标视图<%if (displayMode == DisplayMode.图标){ %>*/
                var elements = $$('#file li');
                var checkboxs = $$('#file [type=checkbox]');
                elements.addEvents({
                    'mouseenter': function () {
                        this.addClass('hover');
                    },
                    'mouseleave': function () {
                        this.removeClass('hover');
                    },
                    'click': function (event) {
                        event = new DOMEvent(event);
                        if (event.target.get('tag') == 'i') {
                            var chk = this.getElement('.checkbox').checked;
                            chk ? this.removeClass('checked') : this.addClass('checked');
                            this.getElement('.checkbox').checked = !chk;
                        }
                    }
                });
                /*<%}%>*/
            });
        </script>
    </asp:View>
    <%--上传文件--%>
    <asp:View ID="View2" runat="server">
        <aspx:FlashUpload ID="FlashUpload1" runat="server" FileTypeDescription="所有文件" FileTypes="*.*" OnCancel="Close();">
        </aspx:FlashUpload>
        <script type="text/javascript">
            function Close() {
                parent.view.dialog('close');
            }
        </script>
    </asp:View>
    <%--新建文件夹--%>
    <asp:View ID="View3" runat="server">
        <table align="center" border="0" cellpadding="4" cellspacing="0">
            <tr>
                <th width="100">文件夹名称：</th>
                <td>
                    <asp:TextBox ID="txtFolder" CssClass="text" runat="server" Width="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtFolder" CssClass="line1px_5" runat="server" BorderStyle="None" />
                </td>
            </tr>
            <tr>
                <th>文件夹位置：</th>
                <td>
                    <asp:TextBox ID="txtPath" CssClass="text" runat="server" Width="200" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <asp:RequiredFieldValidator ControlToValidate="txtFolder" CssClass="line1px_2" runat="server" ErrorMessage="文件夹名称不能为空！"  Width="185"/>
                    <asp:Label ID="Label2" runat="server" Width="185"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <asp:Button ID="btnAddFolder" Width="80" runat="server" Text="确定" onclick="btnAddFolder_Click" />
                    <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
                </td>
            </tr>
        </table>
    </asp:View>
    <%--解压文件--%>
    <asp:View ID="View4" runat="server">
        <div id="norarfile" class="line1px_2" runat="server" visible="false" style="margin:3px">当前目录下未发现压缩文件!</div>
        <table id="rartable" runat="server" align="center" border="0" cellpadding="4" cellspacing="0">
            <tr>
                <th width="100">文件名称：</th>
                <td>
                    <asp:DropDownList ID="DropRar" runat="server" CssClass="text" Width="213"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>解压到目录：</th>
                <td>
                    <asp:TextBox ID="txtToPath" CssClass="text" runat="server" Width="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtToPath"  BorderStyle="None" CssClass="line1px_5" runat="server"/>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <asp:RequiredFieldValidator ControlToValidate="txtToPath" ErrorMessage="解压目录不能为空！" CssClass="line1px_2" Width="185" runat="server"/>
                    <asp:Label ID="Label3" runat="server" Width="178"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <asp:Button ID="btnUnRar" runat="server" Text="确定" onclick="btnUnRar_Click"  Width="80"/>
                    <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
                </td>
            </tr>
        </table>
    </asp:View>
    <%--编辑文件--%>
    <asp:View ID="View5" runat="server">
        <textarea id="code" runat="server" style="width:98%; height:415px; border:1px solid #add2da;" class="c#"></textarea>
        <div style="text-align:right; padding:5px 0;">
            <div style="float:left;"><label id="Label4" runat="server"></label></div>
            <asp:Button ID="btnEdit" runat="server" Text="保存" onclick="btnEdit_Click" Width="80" />
            <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
        </div>
    </asp:View>
    <%--重命名--%>
    <asp:View ID="View6" runat="server">
        <table align="center" border="0" cellpadding="4" cellspacing="0">
            <tr>
                <th width="80px">原始文件名:</th>
                <td>
                    <asp:TextBox ID="txtsourceFileName" runat="server" CssClass="text" Width="200" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>新文件名称:</th>
                <td>
                    <asp:TextBox ID="txtdestFileName" runat="server" CssClass="text" Width="200"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="txtdestFileName" CssClass="line1px_5" runat="server" BorderStyle="None" />
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <asp:RequiredFieldValidator ControlToValidate="txtdestFileName" CssClass="line1px_2" runat="server" ErrorMessage="新文件名称不能为空！"  Width="185"/>
                    <asp:Label ID="Label1" runat="server" Width="185"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnRename" runat="server" Width="80" OnClick="btnRename_Click" Text="保存" />
                    <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
                </td>
            </tr>
        </table>
    </asp:View>
    <%--设置文件类型--%>
    <asp:View ID="View7" runat="server">
        <table border="0" cellpadding="4" cellspacing="0">
            <tr>
                <th>快速选择:</th>
                <td>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="*.jpg;*.jpeg;*.bmp;*.gif;*.png">图片文件</asp:ListItem>
                        <asp:ListItem Value="*.flv;*.rm;*.rmi;*.rmvb;*.mid;*.mov;*.mp3;*.mp4;*.mpc;*.mpeg;*.mpg;*.wav;*.wma;*.wmv">音乐视频</asp:ListItem>
                        <asp:ListItem Value="*.*">所有文件</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>文件类型:</th>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="text" Width="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <input type="button" onclick="setFileType();" value="确定" style="width:80px;"/>
                    <input type="button" onclick="parent.view.dialog('close');" value="取消" style="width:80px;" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            window.addEvent('domready', function () {
                $$('[type=radio]').addEvent('click', function () {
                    $('TextBox1').value = $(this).value;
                });
            });
            function setFileType() {
                Cookie.write('filetype', $('TextBox1').get('value'));
                parent.location.href = parent.location.href;
            }
        </script>
    </asp:View>
    </asp:MultiView>
</form>
</body>
</html>