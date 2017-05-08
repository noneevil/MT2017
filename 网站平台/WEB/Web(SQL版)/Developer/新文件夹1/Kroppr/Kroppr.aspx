<%@ Page Language="C#" AutoEventWireup="false" %>
<%--<?import namespace="v" urn="urn:schemas-microsoft-com:vml" implementation="#default#VML" declareNamespace />--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml"><%-- xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office"--%>
<head>
    <meta http-equiv="imagetoolbar" content="no"/>
    <meta http-equiv="imagetoolbar" content="false"/>
    <%--<meta http-equiv="X-UA-Compatible" content="IE=7" />--%>
    <link href="k_style.css" rel="stylesheet" type="text/css" />
    <script src="../Plugins/mootools/mootools-core-1.4.5.js" type="text/javascript"></script>
    <script src="../Plugins/mootools/mootools-more-1.4.0.1.js" type="text/javascript"></script>
    <script src="kroppr.js" type="text/javascript" id="s1"></script>
    <script type="text/javascript" id="sin">
        window.addEvent('load', function () {
            var img = $('kroppr');
            var kimg = new Element(img);
            var coords = kimg.getCoordinates();
            var el = new Element('div#a', {
                styles: {
                    width: coords.width,
                    height: coords.height,
                    position: 'relative',
                    background: '#000000'
                }
            }).inject(document.body);
            var kropr = new Kropr({ el: 'a', 'img': img.src, top: 10 });
            img.destroy();
        });
    </script>
</head>
<body scroll="no">
    <div id="kdiv_rotate" class="tip" title="旋转">
        <span>&nbsp;0&deg;</span>
        <div id="slider_rotate">
            <img src="knob_v.gif" id="knob_rotate" />
        </div>
        <span>360&deg;</span>
    </div>
    <div id="kdiv_zoom" class="tip" title="放大">
        <span>+</span>
        <div id="slider_zoom">
            <img src="knob_v.gif" id="knob_zoom" />
        </div>
        <span >-</span>
    </div>
    <span id="k_accept" class="menu">裁剪</span>
    <span id="k_reset" class="menu">重置</span>
    <div id="k_load">
        <span id="k_message">Kropping...</span>
        <div id="k_download"></div>
        <span id="k_load_cancel" class="menu" onclick="$('k_load').setStyle('display','none');">Crop again </span>
    </div>
   
    <div id="result" style="position:absolute; left:0; bottom:0; padding:10px;">
        <img src="<%:Request["file"] %>" class="kroppr" id="kroppr" />
    </div>
</body>
</html>