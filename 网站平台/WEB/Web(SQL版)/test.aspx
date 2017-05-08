<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <aspx:TextBox ID="TextBox1" TextMode="SingleLine" Width="300" FormatString="{0:yyyy-MM-dd HH:mm:ss ffff}"
        runat="server"></aspx:TextBox>

        <script runat="server">
     public void IAjax() {
         int country_id;
         int.TryParse(Request.Form["country_id"], out country_id);
         
         Response.Write("[");
         //List<CityInfo> citys = City.GetItemsByCountry_id(country_id);
         //foreach (CityInfo city in citys) {
         //    Response.Write(string.Format("['{0}',{1},'{2}'],", city.Code, city.Id, city.Name); //这里我没有处理 js 问题，你自己处理
         //}
         Response.Write("]");
     }
 </script>
 
<input type="text" id="country_id">
 <input type="button" value="GO" onclick="getCitys()">
 <%--<script type="text/javascript">
 function getCitys() {
     var receive = function(rt) {
         alert(rt);
     };
     var args = 'country_id=' + $('country_id').value;
     <%= Ajax.Register(this, "receive", "args") %>
 }
 </script>--%>
 
    </form>
</body>
</html>
