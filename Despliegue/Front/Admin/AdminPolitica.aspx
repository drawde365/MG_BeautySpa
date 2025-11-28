<%@ Page Title="Politica de Privacidad" Language="C#" 
    MasterPageFile="Admin.Master" 
    AutoEventWireup="true" 
    CodeBehind="AdminPolitica.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Admin.AdminPolitica" %>

<%@ Register Src="~/Shared/PoliticaPrivacidad.ascx" TagPrefix="uc" TagName="Politica"%>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Política de Privacidad
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <uc:Politica runat="server" ID="PoliticaContent" />

</asp:Content>