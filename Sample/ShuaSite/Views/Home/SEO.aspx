<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SEO>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>SEO外链工具-网站自动化宣传机器/超级外链工具可批量增加外链</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="Keywords" content="网站SEO宣传，超级外链工具，自动宣传网站，网站自动宣传工具,网站自动化宣传机器,网站外链群发" />
    <meta name="Description" content="网站自动化宣传机器(超级外链工具)可以轻轻松松提高网站曝光率,可以批量增加外链,快速增高世界排名，全部实现自动化外链群发，无需人工操作，网络推广必备工具。" />
    <script src="<%:Url.Content("/scripts/mootools.js")%>" type="text/javascript"></script>
    <script src="<%:Url.Content("/scripts/timespan.js")%>" type="text/javascript"></script>
    <%if (!String.IsNullOrEmpty(Request["SiteUrl"]))
      {%>
    <style type="text/css">
        DIV.black2 { padding-right: 7px; padding-left: 7px; padding-bottom: 7px; margin: 3px; padding-top: 7px; text-align: center; }
        DIV.black2 A { border-right: #000000 1px solid; padding-right: 5px; border-top: #000000 1px solid; padding-left: 5px; padding-bottom: 2px; margin: 2px; border-left: #000000 1px solid; color: #000000; padding-top: 2px; border-bottom: #000000 1px solid; text-decoration: none; }
        DIV.black2 A:hover { border-right: #000000 1px solid; border-top: #000000 1px solid; border-left: #000000 1px solid; color: #fff; border-bottom: #000000 1px solid; background-color: #000; }
        DIV.black2 A:active { border-right: #000000 1px solid; border-top: #000000 1px solid; border-left: #000000 1px solid; color: #fff; border-bottom: #000000 1px solid; background-color: #000; }
        DIV.black2 SPAN.current { border-right: #000000 1px solid; padding-right: 5px; border-top: #000000 1px solid; padding-left: 5px; font-weight: bold; padding-bottom: 2px; margin: 2px; border-left: #000000 1px solid; color: #fff; padding-top: 2px; border-bottom: #000000 1px solid; background-color: #000000; }
        DIV.black2 SPAN.disabled { border-right: #eee 1px solid; padding-right: 5px; border-top: #eee 1px solid; padding-left: 5px; padding-bottom: 2px; margin: 2px; border-left: #eee 1px solid; color: #ddd; padding-top: 2px; border-bottom: #eee 1px solid; }
    </style>
    <%} %>
</head>
<body>
    <%if (!String.IsNullOrEmpty(Request["SiteUrl"]))
      {
    %>
        <div id="time"></div>
        <ol>
            <%foreach (DataRow dr in ((DataTable)ViewData["Table"]).Rows)
              {
                %>
                    <li><a href="<%:dr["url"] %>" target="_blank">宣传 <%:ViewData["Host"]%> </a>
                    </li>
                    <br />
                    <iframe src="/seo.html?<%:dr["url"] %>" height="50" width="90%" marginwidth="0" 
                    marginheight="0" hspace="0" vspace="0" frameborder="0" scrolling="no"></iframe>
                <%
              }
            %>
        </ol>
        <%=ViewData["PageString"]%>
        <script type="text/javascript">
            window.addEvent('domready', function () {
                var next = $$('[title="下一页"]')[0];
                if (next) {
                    new TimeSpan($('time'),
                    {
                        servertime: "<%:(DateTime.Now) %>", endtime: "<%:(DateTime.Now.AddSeconds(30)) %>", onEnd:
                        function () {
                            location.href = next;
                        }
                    });
                }
            });
        </script>
        <%
      }
      else
      {
          using (Html.BeginForm("seo", "home", new { Page = 1, auto = true, time = 100 }, FormMethod.Get, new { target = "_blank" }))
          {
            %>
                <%--<%:Html.AntiForgeryToken()%>--%>
                <input type="text" id="SiteUrl" name="SiteUrl" value="http://www.wieui.com" />
                <input type="submit" value="开始" />
            <%
          }
      }
    %>
    <script type="text/javascript">
        
    </script>
</body>
</html>
