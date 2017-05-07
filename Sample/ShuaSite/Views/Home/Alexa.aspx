<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<html>
<head>
    <title>Alexa刷站系统</title>
    <meta http-equiv="refresh" content="3;url=<%:ViewData["url"] %>">
    <style type="text/css">
        body, td, th { font-size: 12px; }
        a:link { font-size: 12px; color: #333333; text-decoration: underline; }
        a:visited { font-size: 12px; color: #333333; }
        a:hover { font-size: 12px; color: #FF0000; text-decoration: underline; }
        div { line-height: 30px; text-align: center; }
        span { color: #FF0000; }
    </style>
</head>
<body>
    <div>
        积分+1<br>
        <span>网站载入中......请稍等,刷站期间请不要<b>关闭.刷新</b>该浏览器. </span>
        <br />
        准备转向URL:<%:ViewData["url"] %>
    </div>
</body>
</html>