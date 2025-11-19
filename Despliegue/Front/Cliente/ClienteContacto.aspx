<%@ Page Title="Politica de Privacidad" Language="C#" 
    MasterPageFile="Cliente.Master" 
    AutoEventWireup="true" 
    CodeBehind="ClienteContacto.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Cliente.ClienteContacto" %>

<%-- 1. Registra tu Control de Usuario para poder usarlo --%>
<%@ Register Src="~/Shared/Contacto.ascx" TagPrefix="uc" TagName="Contacto"%>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Contacto
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- 2. Inserta el control. Todo el HTML del .ascx aparecerá aquí --%>
    <uc:Contacto runat="server" ID="ContactoContent" />

</asp:Content>