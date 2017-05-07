<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Mvc4Example.Models.T_City>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Edit</h2>

<% using (Html.BeginForm()) { %>
    <%: Html.AntiForgeryToken() %>
    <%: Html.ValidationSummary(true) %>

    <fieldset>
        <legend>T_City</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.City_Id) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.City_Id) %>
            <%: Html.ValidationMessageFor(model => model.City_Id) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.City_Name) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.City_Name) %>
            <%: Html.ValidationMessageFor(model => model.City_Name) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Province_Id) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Province_Id) %>
            <%: Html.ValidationMessageFor(model => model.Province_Id) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.City_Letter) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.City_Letter) %>
            <%: Html.ValidationMessageFor(model => model.City_Letter) %>
        </div>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div style="color:red; overflow:hidden; clear:both;">
    <%if(ViewData["data"]!=null){ %>
        保存成功!<br />
        <%:ViewData["data"] %>
    <%} %>
</div>

</asp:Content>
