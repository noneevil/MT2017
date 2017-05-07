<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <title>日历控件</title>
    <meta name="viewport" content="width=device-width" />
    <script src="/scripts/lib/jquery-1.8.3.js"></script>
    <style type="text/css">
        body { font-size: 12px; }
        dl.calendar { margin: 0; padding: 0; }
        dl.calendar dt, dl.calendar dd { margin: 0; padding: 0; }
        dl.calendar dt { float: left; width: 60px; height: 125px; padding-top: 75px; text-align: center; color: #fff; position: relative; background-color: #96D2FA; }
        dl.calendar dt input { position: absolute; left: 0; top: 0; width: 60px; height: 23px; border: none; cursor: pointer; background: url(/content/icons/icon_arrow.png) no-repeat center top; }
        dl.calendar dt input.down { background-position: center bottom; bottom: 0; top: auto; }
        dl.calendar dt input.disable { background-position: center center; cursor: default; }

        dl.calendar dd { float: right; width: 350px; height: 200px; overflow: hidden; background-color: #F7F7F7; }
        dl.calendar table { height: 200px; border: none; border-collapse: collapse; empty-cells: show; color: #B8B8B8; }
        dl.calendar table textarea { display: none; }
        dl.calendar table span { display: block; }
        dl.calendar table label { display: block; text-align: right; }
        dl.calendar table td { border: 1px solid #fff; padding: 0 5px; height: 28px; font-size: 10px; line-height: 13px; vertical-align: middle; }
        dl.calendar table td.none { background-color: #FCFCFC; }
        dl.calendar table td.on { background-color: #FFF9E6; color: #94928A; }
        dl.calendar table td.on label { color: #ff574a; }
        dl.calendar table td.today { background-color: #46D0D6; color: #fff; }
        dl.calendar table td.today label { color: #fff; }
        dl.calendar table th { text-align: center; height: 22px; vertical-align: middle; font-size: 11px; color: #517c99; background-color: #D4ECFC; border: 1px solid #fff; border-top: 1px solid #D4ECFC; }
        /*table.calendar thead td { text-align: center; height: 22px; vertical-align: middle; font-size: 11px; color: #517c99; background-color: #D4ECFC; border-top: 1px solid #D4ECFC; }*/
    </style>
    <script type="text/javascript">
        var GetCalendar = function (time) {
            var div = $('#test');
            var data = { dateTime: time };

            $.post('<%:Url.Action("Calendar", "Calendar")%>', data, function (result) {
                div.html(result);
            });
        };
    </script>
</head>
<body>
    <div id="test" style="width: 410px; margin: 0 auto; overflow: hidden;">
        <%Html.RenderAction("Calendar", "Calendar");%>
    </div>
</body>
</html>
