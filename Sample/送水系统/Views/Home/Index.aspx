<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<!DOCTYPE html>
<html>
<head>
    <title>送水系统</title>
    <meta http-equiv="Window-target" content="_top" />
    <meta name="Author" content="http://www.wieui.com" />
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Content/favicon.ico" rel="Shortcut Icon" />
    <link href="/Content/css.css" rel="stylesheet" type="text/css" />
    <script src="/Plugins/mootools-core-1.5.1.min.js"></script>
    <script src="/Plugins/mootools-more-1.5.1.min.js"></script>
    <script src="/Plugins/jquery-1.8.3.min.js"></script>
    <script src="/Plugins/jQuery-ui/jquery-ui-1.10.3.min.js"></script>
    <script src="/Plugins/jQuery-ui/jquery.ui.datepicker-zh-CN.min.js"></script>
    <script src="/Plugins/SlideList.js"></script>
    <script type="text/javascript">
        window.addEvents(
        {
            domready: function () {
                new SlideList('head');
                resize();
                var Task = function () {
                    new Request.JSON({
                        url: 'Task.ashx',
                        onSuccess: function (data) {
                            var el = $$('#head b');
                            el[0].set('text', data.stock);
                            el[1].set('text', data.orders0);
                            el[2].set('text', data.orders1);
                            el[3].set('text', data.sales);
                            el[4].set('text', '￥' + data.money);
                        },
                        onFailure: function () {

                        },
                        onRequest: function () {

                        }
                    }).send();
                };
                Task.periodical(2000);
            },
            beforeunload: function () {
                /*window.event.returnValue = '确定要离开此页吗?';*/
            },
            resize: resize
        });
        /*框架高度修正*/
        function resize() {
            var h1 = $('head').getCoordinates().height;
            var h2 = $('menu').getCoordinates().height;
            var h = document.getHeight();
            $('winFrame').setStyle('height', h - (h1 + h2 + 5));
        }
        /*退出时提示*/
        function SingOut() {
            ht = document.getElementsByTagName("html");
            ht[0].style.filter = "progid:DXImageTransform.Microsoft.BasicImage(grayscale=1)";
            if (confirm('你确定要退出？')) {
                location.href = 'Login.aspx?cmd=exit';
            }
            else {
                ht[0].style.filter = "";
            }
        }
    </script>
</head>
<body>
    <div id="head">
        <div>
            <p>今日库存:<b></b>/份</p>
            <p>未配送订单:<b></b> 未对账订单:<b>30</b></p>
            <p>今日销售量:<b>300</b>/份 金额:<b>￥10000.00</b></p>
        </div>
        <ul>
            <li class="on"><img src="/Content/ico_01.png" /><p>客户管理</p></li>
            <li><img src="/Content/ico_04.png" /><p>日程管理</p></li>
            <li><img src="/Content/ico_07.png" /><p>商品管理</p></li>
            <li><img src="/Content/ico_08.png" /><p>订单管理</p></li>
            <%--<li><img src="/Content/ico_06.png" /><p>当前任务</p></li>--%>
            <li><img src="/Content/ico_03.png" /><p>报表管理</p></li>
            <li><img src="/Content/ico_02.png" /><p>系统设置</p></li>
            <li onclick="SingOut();"><img src="/Content/ico_05.png" /><p>退出系统</p></li>
        </ul>
    </div>
    
    <div id="menu">
        <ul>
            <li><a href="Members.aspx" target="winFrame">客户管理</a></li>
        </ul>
        <ul class="hide">
            <li><a href="Schedule.aspx?tabs=-1" target="winFrame">上周排程</a></li>
            <li class="separate"></li>
            <li><a href="Schedule.aspx?tabs=0" target="winFrame">本周排程</a></li>
            <li class="separate"></li>
            <li><a href="Schedule.aspx?tabs=1" target="winFrame">下周排程</a></li>
        </ul>
         <ul class="hide">
            <li><a href="Product.aspx" target="winFrame">商品管理</a></li>
            <li class="separate"></li>
            <li><a href="Product_Edit.aspx" target="winFrame">添加商品</a></li>
        </ul>
        <ul class="hide">
            <li><a href="Order_Create.aspx" target="winFrame">创建订单</a></li>
            <li class="separate"></li>
            <li><a href="Order.aspx" target="winFrame">订单管理</a></li>
            <li class="separate"></li>
            <li><a href="Order_Transport.aspx" target="winFrame">未配送订单</a></li>
            <li class="separate"></li>
            <li><a href="Order_Check.aspx" target="winFrame">未对账订单</a></li>
        </ul>
        <%--<ul class="hide">
            
        </ul>--%>
        <ul class="hide">
            <li><a href="#">日报表</a></li>
            <li class="separate"></li>
            <li><a href="#">周报表</a></li>
            <li class="separate"></li>
            <li><a href="#">月报表</a></li>
            <li class="separate"></li>
            <li><a href="#">员工个人报表</a></li>
            <li class="separate"></li>
            <li><a href="#">用户消费排行榜</a></li>
        </ul>
        <ul class="hide">
            <li><a href="#">员工管理</a></li>
            <li class="separate"></li>
            <li><a href="#">密码修改</a></li>
            <li class="separate"></li>
            <li><a href="#">权限设置</a></li>
            <li class="separate"></li>
            <li><a href="#">数据备份</a></li>
            <li class="separate"></li>
            <li><a href="#">操作日志</a></li>
        </ul>
    </div>
    <iframe id="winFrame" name="winFrame" width="100%" border="0" frameborder="0" src="AddUser.aspx"></iframe>
</body>
</html>