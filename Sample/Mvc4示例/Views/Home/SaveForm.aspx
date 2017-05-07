<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<T_Province>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    13种数据提交方式
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        td input[type='button'] { margin:0; padding:5px; border:1px solid #ccc; cursor:pointer;}
        td input[type='button']:hover { background-color:#E8EEF4; border-color:#ffd800;}
    </style>
    <%using (Html.BeginForm("Deom01", "Home", FormMethod.Post, new { id = "sys", name = "sys" }))
      { %>
        <table width="100%">
            <tr>
                <td>提交方式</td>
                <td>MVC原始方式</td>
                <td>Newtonsoft.Json 别名</td>
                <td>JSONP</td>
            </tr>
            <tr>
                <td>
                    <input type="radio" id="r1" name="sendtype" value="get" /> <label for="r1">GET</label>
                    <input type="radio" id="r2" name="sendtype" value="post" checked="checked" /> <label for="r2">POST</label>
                </td>
                <td>
                    <input id="Button0" type="button" value="ArrayModel" />
                    <input id="Button1" type="button" value="Model 1" />
                    <input id="Button11" type="button" value="Model 2" />
                    <input id="Button2" type="button" value="Query String" />
                    <input id="Button3" type="button" value="FormCollection" />
                    <input id="Button4" type="button" value="Request" />
                    <input id="Button5" type="button" value="POST 2 List<T>" />
                </td>
                <td>
                    <input id="Button6" type="button" value="POST 2 Stream" />
                    <input id="Button7" type="button" value="POST 2 JObject" />
                    <input id="Button8" type="button" value="POST 2 JArray" />
                    <input id="Button12" type="button" value="POST 2 Model" />
                </td>
                <td>
                    <input id="Button9" type="button" value="JSONP Object" />
                    <input id="Button10" type="button" value="JSONP HTML" />
                </td>
            </tr>
        </table>

        <table width="100%">
            <tr>
                <td width="49%">
                    Post Data
                </td>
                <td>
                    Request
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <textarea id="value"></textarea>
                </td>
                <td>
                    <textarea id="resultHeader"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Result
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <textarea id="result"></textarea>
                </td>
            </tr>
        </table>
    <%} %>
    <script type="text/javascript">
        var db = <%=ViewData["data"]%>;
        window.addEvent('domready', function () {
            /*绑定选择提交方式事件*/
            var method = 'post';
            $$('input[name="sendtype"]').addEvent('click', function () {
                method = $(this).get('value');
            });

            /*MVC ArrayModel*/
            $('Button0').addEvent('click', function () {
                /*构造数据*/
                var data = {
                    array1: [1, 2, 3],
                    array2: [
                        [100, 200, 250],
                        [300, 400, 450]
                    ],
                    array3: [
                       ['A', 'B', 'C'],
                       ['D', 'E', 'F']
                    ],
                    array4: [0x12, 0x20, 0x34, 0x40],
                    array5: [1, 2, 3],
                    array6: { A: 65, B: 66, C: 67, D: 68, E: 69, F: 70 },
                    array7: [0x12, 0x20, 0x34, 0x40],
                    array8: [{ key: '65', value: 'A' }]

                };

                new Request.JSON({
                    noCache: true,
                    data: data,
                    method: method,
                    url: '<%:Url.Action("SaveForm0","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*MVC Model 1*/
            $('Button1').addEvent('click', function () {
                /*构造数据*/
                var data = { Province_Id: 100000, Province_Name: '贵州省1', Province_Letter: 'G' };
                data.city = {};
                data.city.City_Id = '嵌套实体';
                data.city.City_Name = '嵌套实体';
                data.city.City_Letter = '嵌套实体';

                new Request.JSON({
                    noCache: true,
                    data: data,
                    method: method,
                    url: '<%:Url.Action("SaveForm1","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*MVC Model 2*/
            $('Button11').addEvent('click', function () {
                /*构造数据*/
                var data = { "Province_Id": "110000", "Province_Name": "北京市", "Province_Letter": "B", "city": { "city_id": 1000, "city_pid": 1000, "city_name": "贵阳市", "city_Letter": "G" } };

                new Request.JSON({
                    noCache: true,
                    data: JSON.encode(data),
                    method: method,
                    headers: { 'Content-Type': 'application/json' },
                    url: '<%:Url.Action("SaveForm1_2","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*MVC Query String*/
            $('Button2').addEvent('click', function () {
                /*构造数据*/
                var data = { id: 100000, name: '贵州省2', letter: 'G' };

                new Request.JSON({
                    noCache: true,
                    data: data,
                    method: method,
                    url: '<%:Url.Action("SaveForm2","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*MVC FormCollection*/
            $('Button3').addEvent('click', function () {
                /*构造数据*/
                var data = { id: 100000, name: '贵州省3', letter: 'G' };
                data.city = {};
                data.city.City_Id = '嵌套实体';
                data.city.City_Name = '嵌套实体';
                data.city.City_Letter = '嵌套实体';

                new Request.JSON({
                    noCache: true,
                    data: data,
                    method: method,
                    url: '<%:Url.Action("SaveForm3","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*Request*/
            $('Button4').addEvent('click', function () {
                /*构造数据*/
                var data = { id: 100000, name: '贵州省4', letter: 'G' };

                new Request.JSON({
                    noCache: true,
                    data: data,
                    method: method,
                    url: '<%:Url.Action("SaveForm4","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*POST 2 List<T>*/
            $('Button5').addEvent('click', function () {
                /*构造数据*/
                var arr = [];
                Object.each(db, function (o, i) {
                    Object.map(o, function (value, key) {
                        if (typeof (value) == 'object') {
                            /*嵌套成员*/
                            Object.map(value, function (v, k) {
                                arr.push('data[' + i + '].' + key + '.City_' + k + '=' + encodeURI(v));
                            });
                        }
                        else {
                            arr.push('data[' + i + '].Province_' + key + '=' + encodeURI(value));
                        }
                    });
                });

                new Request.JSON({
                    noCache: true,
                    data: arr.join('&'),
                    method: method,
                    url: '<%:Url.Action("SaveForm5","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = arr.join('&');
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });
            /*POST 2 Stream*/
            $('Button6').addEvent('click', function () {
                /*构造数据*/
                var data = db;
                Object.each(db, function (o, i) {
                    //o.city.pid=1111;/*别名 pid 受 JsonIgnore 影响将不接收*/
                    o.City = o.City || {};
                    o.City.pid = 111;
                });

                new Request.JSON({
                    noCache: true,
                    data: JSON.encode(data),
                    method: method,
                    url: '<%:Url.Action("SaveForm6","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*POST 2 JObject*/
            $('Button7').addEvent('click', function () {
                /*构造数据*/
                var data = { "Id": "110000", "Name": "北京市", "Letter": "B", "city": { "id": 1000, "pid": 1000, "name": "贵阳市", "Letter": "G" } };/*别名 pid 受 JsonIgnore 影响将不接收*/

                new Request.JSON({
                    noCache: true,
                    data: JSON.encode(data),
                    method: method,
                    //headers: { 'Content-Type': 'text/json' },
                    url: '<%:Url.Action("SaveForm7","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*POST 2 JArray*/
            $('Button8').addEvent('click', function () {
                /*构造数据*/
                var data = db;
                Object.each(db, function (o, i) {
                    /*别名 pid 受 JsonIgnore 影响将不接收*/
                    o.City = o.City || {};
                    o.City.pid = 1111;
                });

                new Request.JSON({
                    noCache: true,
                    data: JSON.encode(data),
                    method: method,
                    //headers: { 'Content-Type': 'text/json' },
                    url: '<%:Url.Action("SaveForm8","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*POST 2 Model */
            $('Button12').addEvent('click', function () {
                /*构造数据*/
                var data = [{ "Id": "110000", "Name": "北京市", "Letter": "B", "city": { "id": 1000, "pid": 1000, "name": "贵阳市", "Letter": "G" } }];/*别名 pid 受 JsonIgnore 影响将不接收*/

                new Request.JSON({
                    noCache: true,
                    data: JSON.encode(data),
                    method: method,
                    headers: { 'Content-Type': 'text/json' },
                    url: '<%:Url.Action("SaveForm8_2","SaveForm") %>',
                    onRequest: function () {
                        $('value').value = JSON.encode(data);
                    },
                    onSuccess: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*JSONP Object*/
            $('Button9').addEvent('click', function () {
                /*构造数据*/
                var data = { City_Id: 110000, id: 1000000000 };

                new Request.JSONP({
                    url: '<%:Url.Action("JsonpObject","SaveForm") %>',
                    //callbackKey: 'callback',
                    data: data,
                    method: method,
                    onRequest: function (url) {
                        $('value').value = JSON.encode(data);
                    },
                    onComplete: function (result) {
                        $("resultHeader").value = result.header;
                        $('result').value = JSON.encode(result.data);
                    }
                }).send();
            });

            /*JSONP HTML*/
            $('Button10').addEvent('click', function () {
                /*构造数据*/
                var data = { id: 1000000000 };

                new Request.JSONP({
                    url: '<%:Url.Action("JsonpPartialView1","SaveForm") %>',
                    //callbackKey: 'callback',
                    data: data,
                    method: method,
                    onRequest: function (url) {
                        $('value').value = JSON.encode(data);
                    },
                    onComplete: function (result) {
                        //$("resultHeader").value = result.header;
                        //$('result').value = JSON.encode(result.data);
                        var div = new Element('div', { html: result });
                        var textarea = div.getElement('textarea');
                        var _request = textarea.get('html');
                        textarea.destroy();

                        $("resultHeader").value = _request;
                        $('result').value = div.get('html');
                    }
                }).send();
            });
        });
    </script>
</asp:Content>
