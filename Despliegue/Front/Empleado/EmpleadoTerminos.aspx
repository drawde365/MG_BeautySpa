<%@ Page Title="Términos y Condiciones" Language="C#" 
    MasterPageFile="Empleado.Master" 
    AutoEventWireup="true" 
    CodeBehind="EmpleadoTerminos.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Empleado.EmpleadoTerminos" %>

<%-- 1. Registra tu Control de Usuario para poder usarlo --%>
<%@ Register Src="~/Shared/TerminosControl.ascx" TagPrefix="uc" TagName="Terminos" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Términos y Condiciones
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- 2. Inserta el control. Todo el HTML del .ascx aparecerá aquí --%>
    <uc:Terminos runat="server" ID="TerminosContent" />

</asp:Content>