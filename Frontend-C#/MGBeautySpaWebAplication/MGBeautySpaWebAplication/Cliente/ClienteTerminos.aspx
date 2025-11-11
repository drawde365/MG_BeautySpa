<%@ Page Title="Términos y Condiciones" Language="C#" 
    MasterPageFile="Cliente.Master" 
    AutoEventWireup="true" 
    CodeBehind="ClienteTerminos.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Cliente.ClienteTerminos" %>

<%-- 1. Registra tu Control de Usuario --%>
<%@ Register Src="~/Shared/TerminosControl.ascx" TagPrefix="uc" TagName="Terminos" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Términos y Condiciones
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- 2. Inserta el mismo control --%>
    <uc:Terminos runat="server" ID="TerminosContent" />

</asp:Content>