<%@ Page Title="Contacto" Language="C#" 
    MasterPageFile="Admin.Master" 
    AutoEventWireup="true" 
    CodeBehind="AdminContacto.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Admin.AdminContacto" %>

<%@ Register Src="~/Shared/Contacto.ascx" TagPrefix="uc" TagName="Contacto"%>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Contacto
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <uc:Contacto runat="server" ID="ContactoContent" />

</asp:Content>