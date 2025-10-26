<%@ Page Title="Inicio Administrador" Language="C#" MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true" CodeBehind="InicioAdmin.aspx.cs"
    Inherits="MGBeautySpaWebAplication.InicioAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Aquí puedes agregar estilos o scripts específicos del head -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center mb-4">Bienvenido al panel de administración</h2>
    <p class="text-center">Aquí podrás gestionar los servicios, productos y usuarios del sistema.</p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <!-- Aquí puedes agregar scripts específicos para esta página -->
</asp:Content>
