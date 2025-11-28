<%@ Page Title="Términos y Condiciones" Language="C#" 
    MasterPageFile="Admin.Master" 
    AutoEventWireup="true" 
    CodeBehind="AdminTerminos.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Admin.AdminTerminos" %>

<%@ Register Src="~/Shared/TerminosControl.ascx" TagPrefix="uc" TagName="Terminos" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Términos y Condiciones
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <uc:Terminos runat="server" ID="TerminosContent" />

</asp:Content>