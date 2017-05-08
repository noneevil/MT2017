<%@ Page Language="C#" AutoEventWireup="false" %>
<html>
<head>
    <title>广告预览</title>
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../skin/css.css" rel="stylesheet" type="text/css" />
    <script src="../skin/Plugins/mootools-1.4.5.js" type="text/javascript"></script>
    <script src="../skin/Plugins/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../skin/Plugins/mediaelement/mediaelement-and-player.min.js" type="text/javascript"></script>
    <link href="../skin/Plugins/mediaelement/mediaelementplayer.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        jQuery(function () {
            jQuery('audio,video').mediaelementplayer(); 
        });
    </script>
</head>
<body scroll="no">
    <video width="<%:Request["lightbox[width]"] %>" height="<%:Request["lightbox[height]"] %>" src="<%:Request["src"] %>" type="video/flv"
        poster="../Plugins/johndyer-mediaelement/media/echo-hereweare.jpg" controls="controls" preload="none">
    </video>
</body>
</html>