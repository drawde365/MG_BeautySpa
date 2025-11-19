<%@ Page Title="Contacto" Language="C#" 
    MasterPageFile="Empleado.Master" 
    AutoEventWireup="true" 
    CodeBehind="EmpleadoContacto.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Empleado.EmpleadoContacto" %>

<%-- 1. Registra tu Control de Usuario para poder usarlo --%>
<%@ Register Src="~/Shared/Contacto.ascx" TagPrefix="uc" TagName="Contacto" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Contacto
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- 2. Inserta el control. Todo el HTML del .ascx aparecerá aquí --%>
    <uc:Contacto runat="server" ID="ContactoContent" />

</asp:Content>