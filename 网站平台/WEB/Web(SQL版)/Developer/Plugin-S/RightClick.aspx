<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RightClick.aspx.cs" Inherits="WebSite.Web.RightClick" %>
<html>
<head>
    <title>通过IE右键菜单收藏文章</title>
</head>
<body>
    <form id="sys" name="sys" target="_blank" method="post" action="/Developer/article/News_Edit.aspx" enctype="multipart/form-data">
        <input type="hidden" id="title" name="title" />
        <input type="hidden" id="content" name="content" />
        <input type="hidden" id="url" name="url" />
        <input type="hidden" id="by" name="by" value="ie" />
    </form>
    <script type="text/javascript">
        window.onerror = ignoreError;
        function ignoreError() {
            return true
        }
        function PostSelectHtml() {
            var strText = GetSelectHtml();
            var strTitle = external.menuArguments.document.title;
            var strURL = external.menuArguments.document.URL;
            if (strURL.indexOf('<%:Request.Url.Host %>') >= 0) {
                alert("提示：不能收藏当前站点下的文章！");
                return
            }
            if (strText.length == 0) {
                alert('请选中网页上欲保存的文字和图片后再收藏！');
                return
            }
            document.getElementById("url").value = encodeURIComponent(strURL);
            document.getElementById("title").value = encodeURIComponent(strTitle);
            document.getElementById("content").value = encodeURIComponent(strText);

            try {
                document.sys.submit()
            }
            catch (e) {
                alert(e)
            }
        }
        function GetSelectHtml() {
            var range;
            var spanNode;
            if (isBrowser() == 'IE') {
                var doc = external.menuArguments.document;
                if (doc.selection.type == "None" || doc.selection.type == "none") {
                    return ""
                }
                range = doc.selection.createRange();
                spanNode = doc.createElement("span");
                spanNode.innerHTML = range.htmlText
            } else {
                var selection = window.getSelection();
                if (selection.rangeCount == 0) return "";
                range = selection.getRangeAt(0);
                var startContainer = range.startContainer;
                spanNode = startContainer.ownerexternal.menuArguments.document.createElement("span");
                spanNode.appendChild(range.cloneContents())
            }
            spanNode.style.display = "none";
            external.menuArguments.document.body.appendChild(spanNode);
            dom(spanNode);
            var selecthtml = spanNode.innerHTML;
            spanNode.parentNode.removeChild(spanNode);
            return selecthtml
        }
        function dom(obj) {
            if (!obj.hasChildNodes) {
                return
            }
            var nodes = obj.childNodes;
            for (var i = 0; i < nodes.length; i++) {
                var curNode = nodes[i];
                var attrs = curNode.attributes;
                var nodename = curNode.nodeName.toLowerCase();
                if (nodename == "script" || nodename == "iframe" || nodename == "link" || nodename == "meta" || !isVisibleNode(curNode)) {
                    curNode.parentNode.removeChild(curNode)
                }
                else if (nodename == "a" || nodename == "img" || nodename == "embed") {
                    if (attrs != null) {
                        for (var j = 0; j < attrs.length; j++) {
                            var a = attrs[j].nodeName.toLowerCase();
                            var v = attrs[j].nodeValue;
                            if (a == "href" || a == "src") {
                                if (v.toLowerCase().indexOf("javascript:") == 0 || v.indexOf("#") == 0) {
                                    attrs[j].nodeValue = ""
                                }
                                else {
                                    /*v = replaceURL(v);*/
                                    attrs[j].nodeValue = v
                                }
                            }
                        }
                    }
                }
                if (curNode != null && curNode.hasChildNodes) {
                    dom(curNode)
                }
            }
        }
        function replaceURL(url) {
            if (!window.location) {
                return url
            }
            var match = null;
            url = trim(url);
            var host = window.location.host;
            var proto = window.location.protocol;
            var base = window.location.href.split("?")[0].split('#')[0];
            base = base.substr(0, base.lastIndexOf('/')) + "/";
            rbase = proto + "//" + host;
            if ((match = url.match(/^(https?):/i)) != null) {
                return url
            }
            else {
                if (url.indexOf("/") == 0) {
                    return rbase + url
                } else {
                    return base + url
                }
            }
        }
        function trim(str) {
            if (typeof str != "string") {
                return str
            } else {
                return str.replace(/^\s+/, '').replace(/\s+$/, '')
            }
        }
        function isBrowser() {
            if (navigator.appVersion.indexOf("MSIE", 0) != -1) return 'IE';
            if (navigator.appVersion.indexOf("WebKit", 0) != -1) return 'Safari';
            if (navigator.userAgent.indexOf("Firefox", 0) != -1) return 'Firefox';
            if (navigator.userAgent.indexOf("WebKit") > 0 && navigator.userAgent.indexOf("iPad") > 0) return 'Ipad';
            if (navigator.userAgent.indexOf("WebKit") > 0 && navigator.userAgent.indexOf("iPhone") > 0) return 'Iphone';
            if (navigator.userAgent.indexOf("WebKit") > 0 && navigator.userAgent.indexOf("Chrome") > 0) return 'Chrome'
        }
        function isVisibleNode(node) {
            if (node.nodeType) {
                if (node.nodeType == 3) {
                    return true
                }
                if (isBrowser() == 'IE') {
                    if (node.currentStyle != null && node.currentStyle['display'] == "none") {
                        return false
                    }
                } else {
                    try {
                        if (window.getComputedStyle(node, null)['display'] == "none") {
                            return false
                        }
                    } catch (e) {
                        return false
                    }
                }
                return true
            } else {
                return false
            }
        }
        PostSelectHtml();
    </script>
</body>
</html>