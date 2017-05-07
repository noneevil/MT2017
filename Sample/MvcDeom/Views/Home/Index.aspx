<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<T_Province>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    主页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%:ViewData["Message"]%>
    <%using (Html.BeginForm("SaveForm", "Home", FormMethod.Post, new { id = "sys", name = "sys" }))
      { %>
    <%foreach (var item in Model)
      { %>
    <table width="90%" align="center" border="0" style="margin: 10px;">
        <tr>
            <td>
                Province_Name
            </td>
            <td>
                <%: Html.TextBox("Name", item.Province_Name)%>
            </td>
            <td>
                City_Id
            </td>
            <td>
                <%: Html.TextBox("Id", item.City.City_Id)%>
            </td>
        </tr>
        <tr>
            <td>
                City_Letter
            </td>
            <td>
                <%: Html.TextBox("Letter", item.City.City_Letter)%>
            </td>
            <td>
                City_Name
            </td>
            <td>
                <%: Html.TextBox("Name", item.City.City_Name)%>
            </td>
        </tr>
    </table>
    <hr style="border-top: 1px solid #ccc;" />
    <%} %>
    <input type="button" value="提交" />
    <%} %>
    <script type="text/javascript">
        window.addEvent('domready', function () {
            $$('input[type=button]').addEvent('click', function () {
                var val = $('sys').toQueryString();

                var arr = [];
                $$('table').each(function (item) {
                    var _item = {};
                    item.getElements('input[type=text]').each(function (text) {
                        _item[text.get('name')] = text.get('value');
                    });
                    arr.push(_item);
                });
                var data = JSON.encode(arr);
                new Request.JSON({
                    data: data,
                    url: '/home/saveform',
                    onSuccess: function (result) {
                        //alert(result.state);
                    }
                }).send();
            });
        });
    </script>
</asp:Content>
