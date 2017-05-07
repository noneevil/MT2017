<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<T_City>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    局部视图的用法和区别
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        h1 { line-height: 51px; display: block; margin: 0; padding: 0; margin-bottom: 10px; border-bottom: 1px solid #ccc; }
        #loading { clear:both; display:none; height:16px; text-align:center; background:url(/content/icons/loading.gif) no-repeat center center;}
    </style>
    <h6>局部视图的用法和区别参考:<a href="http://www.cnblogs.com/meen/p/3145696.html" target="_blank">Html.RenderPartial & Html.Partial | Html.RenderAction & Html.Action</a></h6>
    <h1>Server 调用</h1>
    <div>
        <h5>不执行Controller中的Action方法,可传递数据; &lt;%Html.RenderPartial("AjaxView", Model);%&gt;</h5>
        <%Html.RenderPartial("AjaxView", ViewData["data"]); %>

        <h5>不执行Controller中的Action方法,可传递数据; &lt;%=Html.Partial("AjaxView", Model)%&gt;</h5>
        <%=Html.Partial("AjaxView",ViewData["data"]) %>

        <h5>执行Controller中的Action方法,可传递参数;&lt; %Html.RenderAction("AjaxView", new { page = 15, pagesize = 5 });%&gt;</h5>
        <%Html.RenderAction("AjaxView", new { page = 15, pagesize = 5 }); %>

        <h5>执行Controller中的Action方法,可传递参数;&lt; %=Html.Action("AjaxView", new { page = 15, pagesize = 5 })%&gt;</h5>
        <%=Html.Action("AjaxView", new { page = 15, pagesize = 5 }) %>
    </div>
    <h1>Ajax 调用</h1>
    <div id="loading"></div>
    <div id="list"></div>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            var LoadData = function (url) {
                if (!url) url = Cookie.read('lastUrl');
                if (!url) url = '<%:Url.Action("AjaxView", "Home")%>';
                Cookie.write('lastUrl', url);
                new Request({
                    url: url,
                    onRequest: function () {
                        $('loading').setStyle('display', 'block');
                    },
                    onSuccess: function (result) {
                        $('loading').setStyle('display', 'none');
                        $('list').set('html', result);
                        $$('#list .page a').addEvent('click', function (e) {
                            e.stop();
                            LoadData($(this).get('href'));
                        });
                    },
                    onFailure: function () {
                        $('loading').setStyle('display', 'none');
                        Cookie.dispose('lastUrl');
                    }
                }).send();
            };
            LoadData();
        });
    </script>
</asp:Content>