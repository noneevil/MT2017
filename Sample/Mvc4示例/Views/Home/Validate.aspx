<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    表单验证 - 示例
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/scripts/lib/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/scripts/validation/jquery.validate.js" type="text/javascript"></script>
    <%--<script src="/scripts/validation/jquery.validate.min.js" type="text/javascript"></script>--%>
    <script src="/scripts/validation/additional-methods.min.js" type="text/javascript"></script>
    <script src="/scripts/validation/messages_zh.min.js" type="text/javascript"></script>
    <style type="text/css">
        input.text { width: 150px; height: 25px; line-height: 25px; border: 1px solid #ccc; }
        input.error { background-color: #fff2e9; }
        label.error, label.accept, label.warning, label.loading { float:left; padding-left: 25px; line-height: 25px; display: inline-block; }
        label.error { border: #f60 1px solid; background: url(/skin/dialog/msg_red.gif) #fff2e9 no-repeat 5px center; }
        label.accept { border: #00be00 1px solid; background: url(/skin/dialog/msg_green.gif) #e6ffe6 no-repeat 5px center; }
        label.warning { border: #238CBC 1px solid; background: url(/skin/dialog/msg_blue.gif) #e2f5ff no-repeat 5px center; }
        label.loading { border: #00be00 1px solid; background: url(/skin/dialog/loading.gif) no-repeat 5px center #e6ffe6; }
    </style>

    <script type="text/javascript">
        //http://jqueryvalidation.org/documentation/ 
        /*http://www.w3cschool.cc/jquery/jquery-plugin-validate.html*/
        jQuery.noConflict();
        jQuery(function () {
            /*Jquery 表单序列化为Object对象*/
            jQuery.fn.serializeJson = function () {
                var o = {};
                jQuery(this.serializeArray()).each(function () {
                    o[this.name] = this.value;
                });
                return o;
            };
            var _validate = jQuery('#sys').validate({
                //debug: true,
                /*忽略规则*/
                ignore: ".ignore",
                //onkeyup: false,
                //focusCleanup:true,
                validClass: 'accept',
                errorClass: 'error',//默认
                errorElement: 'label',//默认标签
                //wrapper: 'p',//包裹标签
                /*验证规则*/
                rules: {
                    A01: 'required',
                    A02: {
                        //使用 ajax 方式进行验证，默认会提交当前验证的值到远程地址，如果需要提交其他的值，可以使用 data 选项。
                        required: true,
                        //远程地址只能输出 "true" 或 "false"，不能有其他输出。
                        //服务器数据检查 方式一
                        //remote: '<%:Url.Action("Validate", new { cmd=0 }) %>',
                        //服务器数据检查 方式二
                        remote: {
                            //type: "post",
                            url: "<%:Url.Action("Validate", new { cmd=0 }) %>",
                            data: {
                                //附加数据
                                time: new Date(),
                                A01: function () {
                                    return jQuery("#A01").val();
                                }
                            }
                        }
                    },
                    A03: {
                        required: true,
                        email: true//邮箱地址
                    },
                    A04: {
                        required: true,
                        url: true//网址
                    },
                    A05: {
                        required: true,
                        date: true//日期
                    },
                    A06: {
                        required: true,
                        dateISO: true//日期 2009-06-23，1998/01/22
                    },
                    A07: {
                        required: true,
                        number: true//数字（负数，小数）
                    },
                    A08: {
                        required: true,
                        digits: true//整数
                    },
                    A09: {
                        required: true,
                        creditcard: true//信用卡号
                    },
                    A10: {
                        required: true,
                        equalTo: '#A09'//必须和 #A09 相同
                    },
                    A11: {
                        required: true,
                        accept: 'image/jpg,image/jpeg,image/png,image/gif'
                    },
                    A12: {
                        required: true,
                        maxlength: 5//最多是 5 的字符串
                    },
                    A13: {
                        required: true,
                        minlength: 10//最小是 10 的字符串
                    },
                    A14: {
                        required: true,
                        rangelength: [5, 10]//介于 5 和 10 之间的字符串
                    },
                    A15: {
                        required: true,
                        range: [5, 10]//介于 5 和 10 之间
                    },
                    A16: {
                        required: true,
                        max: 5//不能大于 5
                    },
                    A17: {
                        required: true,
                        min: 10//不能小于 10
                    },
                    A18: {
                        required: true
                    },
                    A19: {
                        required: true,
                        //minlength: 5,
                        //maxlength :10,
                        rangelength: [5, 10]
                    },
                    A20: {
                        required: true
                    }
                },
                /*自定义错误消息*/
                messages: {
                    A01: '不能为空.',
                    A02: {
                        required: '不能为空',
                        remote: '服务器上已经存在了!'
                    },
                    A11: {
                        required: '请选择文件',
                        accept: '请选择有效图片文件'
                    }
                },
                /*指明错误放置的位置，默认情况是：error.appendTo(element.parent());即把错误信息放在验证的元素后面。*/
                errorPlacement: function (error, element) {
                    var td = element.closest('table').closest('td').next('td');
                    if (!td.length) td = element.parent('td').next('td');
                    error.appendTo(td);
                },
                success: function (label, element) {
                    // set &nbsp; as text for IE
                    label.html("&nbsp;").addClass("accept");
                },
                /*数据提交*/
                submitHandler: function (form) {
                    //form.submit();
                    var data = jQuery(form).serializeJson();
                    //alert(JSON.stringify(data));
                    jQuery.ajax({
                        type: "POST",
                        data: JSON.stringify(data),
                        url: '<%:Url.Action("Validate", "Home") %>',
                        dataType: "json",
                        //contentType: "text/json",
                        success: function (result) {
                            alert(JSON.stringify(result));
                        }
                    })
                }
            });
            jQuery(':reset').click(function () {
                _validate.resetForm();
            });
        });
    </script>
    <%
        var items = Enum.GetValues(typeof(System.Web.Razor.Parser.SyntaxTree.SpanKind));
        using (Html.BeginForm("Validate", "Home", FormMethod.Post, new
        {
            id = "sys",
            name = "sys",
            target = "winframe",
            enctype = "application/x-www-form-urlencoded"//multipart/form-data
        }))
        {%>
    <%:Html.AntiForgeryToken()%>
    <table>
        <tr>
            <td>required:</td>
            <td><%:Html.TextBox("A01", "AAAAAAAAA", new { @class="text" }) %></td>
            <td></td>
            <td>remote:</td>
            <td><%:Html.TextBox("A02", "A", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>email:</td>
            <td><%:Html.TextBox("A03", "412541529@qq.com", new { @class="text" }) %></td>
            <td></td>
            <td>url:</td>
            <td><%:Html.TextBox("A04", "http://www.google.com", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>date:</td>
            <td><%:Html.TextBox("A05", "2014", new { @class="text" }) %></td>
            <td></td>
            <td>dateISO:</td>
            <td><%:Html.TextBox("A06", "2014-10-01", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>number:</td>
            <td><%:Html.TextBox("A07", "300.5", new { @class="text" }) %></td>
            <td></td>
            <td>digits:</td>
            <td><%:Html.TextBox("A08", "100", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>creditcard:</td>
            <td><%:Html.TextBox("A09", "8615134152416512841", new { @class="text" }) %></td>
            <td></td>
            <td>equalTo:</td>
            <td><%:Html.TextBox("A10", "8615134152416512841", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>accept:</td>
            <td>
                <input id="A11" name="A11" type="file" /></td>
            <td></td>
            <td>maxlength:</td>
            <td><%:Html.TextBox("A12", "AAAAA", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>minlength:</td>
            <td><%:Html.TextBox("A13", "AAAAAAAAAA", new { @class="text" }) %></td>
            <td></td>
            <td>rangelength:</td>
            <td><%:Html.TextBox("A14", "AAAAAA", new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>range:</td>
            <td><%:Html.TextBox("A15", 6, new { @class="text" }) %></td>
            <td></td>
            <td>max:</td>
            <td><%:Html.TextBox("A16", 3, new { @class="text" }) %></td>
            <td></td>
        </tr>
        <tr>
            <td>min:</td>
            <td><%:Html.TextBox("A17", "22", new { @class="text" }) %></td>
            <td colspan="4"></td>
        </tr>
        <tr>
            <td>RadioButton</td>
            <td>
                <%foreach (var n in items)
                  { %>
                <%:Html.RadioButton("A18", n, true, new { id = n }) %>
                <label for="<%:n %>"><%:n %></label>
                <%} %>
            </td>
            <td></td>
            <td>Select</td>
            <td>
                <%var selectitem = new List<SelectListItem>();
                  foreach (var n in items)
                  {
                      selectitem.Add(new SelectListItem { Text = n.ToString(), Value = Convert.ToString((int)n), Selected = true });
                  } %>
                <%:Html.DropDownList("A20", selectitem, "-- 请选择 --")%>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>checkbox</td>
            <td>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <%foreach (var n in items)
                          { %>
                        <td>
                            <input id="<%:n %>_2" name="A19" type="checkbox" checked="checked" value="<%:n %>" />
                            <label for="<%:n %>_2"><%:n %></label>
                        </td>
                        <%} %>
                    </tr>
                </table>
            </td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td>
                <input type="submit" />
                <input type="reset" />
            </td>
            <td colspan="4"></td>
        </tr>
    </table>
    <%} %>
    <iframe id="winframe" name="winframe"></iframe>
</asp:Content>
