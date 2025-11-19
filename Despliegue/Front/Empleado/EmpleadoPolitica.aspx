<%@ Page Title="Politica de Privacidad" Language="C#" 
    MasterPageFile="Empleado.Master" 
    AutoEventWireup="true" 
    CodeBehind="EmpleadoPolitica.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Empleado.EmpleadoPolitica" %>

<%-- 1. Registra tu Control de Usuario para poder usarlo --%>
<%@ Register Src="~/Shared/PoliticaPrivacidad.ascx" TagPrefix="uc" TagName="Politica"%>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Política de Privacidad
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- 2. Inserta el control. Todo el HTML del .ascx aparecerá aquí --%>
    <uc:Politica runat="server" ID="PoliticaContent" />

</asp:Content>