<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TestSite.Models.T_User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewPage1
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ViewPage1</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ID) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ID) %>
                <%: Html.ValidationMessageFor(model => model.ID) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.UserName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.UserName) %>
                <%: Html.ValidationMessageFor(model => model.UserName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PassWord) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.PassWord) %>
                <%: Html.ValidationMessageFor(model => model.PassWord) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ConfirmPassword) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ConfirmPassword) %>
                <%: Html.ValidationMessageFor(model => model.ConfirmPassword) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.PlusScore) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.PlusScore) %>
                <%: Html.ValidationMessageFor(model => model.PlusScore) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.CountScore) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.CountScore) %>
                <%: Html.ValidationMessageFor(model => model.CountScore) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.GiftScore) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.GiftScore) %>
                <%: Html.ValidationMessageFor(model => model.GiftScore) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Email) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Email) %>
                <%: Html.ValidationMessageFor(model => model.Email) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Status) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Status) %>
                <%: Html.ValidationMessageFor(model => model.Status) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteName) %>
                <%: Html.ValidationMessageFor(model => model.SiteName) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteLogo) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteLogo) %>
                <%: Html.ValidationMessageFor(model => model.SiteLogo) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteUrl) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.SiteUrl) %>
                <%: Html.ValidationMessageFor(model => model.SiteUrl) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Mode) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Mode) %>
                <%: Html.ValidationMessageFor(model => model.Mode) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.JoinTime) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.JoinTime) %>
                <%: Html.ValidationMessageFor(model => model.JoinTime) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.LastLoginTime) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.LastLoginTime) %>
                <%: Html.ValidationMessageFor(model => model.LastLoginTime) %>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

