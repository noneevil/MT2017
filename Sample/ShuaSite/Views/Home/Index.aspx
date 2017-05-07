<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    主页
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="time">
    </div>
    <div>
        <input id="btn_Start" type="button" onclick="T_Start()" value="开始" />
        <input id="btn_Stop" type="button" onclick="T_Stop()" value="停止" />
    </div>
</asp:Content>
