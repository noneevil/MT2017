<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MVC JSON 日期格式
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/scripts/lib/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/scripts/jquery-dateformat.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery.noConflict();
        jQuery(function () {
            /*MVC JSON Jquery日期处理*/
            jQuery.fn.JsonDateFormate = function (jsonDateTime) {
                var value = new Date(parseInt(jsonDateTime.substr(6)));
                //var value = eval('new ' + jsonDateTime.slice(1, -1));
                return value;
            };
            /* test */
            var _timedate =<%=ViewData["data"]%>;
            var _value = jQuery.fn.JsonDateFormate(_timedate.date);
            jQuery('#result').html(JSON.stringify(_timedate) + '<br/>' + jQuery.format.date(_value, 'yyyy年MM月dd HH:mm:ss'));
        });
    </script>
    <h1 id="result"></h1>
</asp:Content>
