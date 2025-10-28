<%@ Page Title="Mi Perfil" 
    Language="C#" 
    MasterPageFile="~/Cliente/Perfil/Perfil.Master" 
    AutoEventWireup="true" 
    CodeBehind="PerfilUsuario.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Cliente.PerfilUsuario" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <div class="profile-info-wrapper">
        <div class="profile-greeting-header">
            <h1 class="greeting-title">
                ¡Hola, <asp:Literal ID="litUserNameGreeting" runat="server" />!
            </h1>
            <p class="greeting-subtitle">
                Aquí puedes revisar y actualizar tus datos personales.
            </p>
        </div>

        <div class="data-grid">
            <div class="data-card">
                <span class="data-label">Nombres</span>
                <p class="data-value"><asp:Literal ID="litNombres" runat="server" /></p>
            </div>

            <div class="data-card">
                <span class="data-label">Apellidos</span>
                <p class="data-value"><asp:Literal ID="litApellidos" runat="server" /></p>
            </div>

            <div class="data-card">
                <span class="data-label">Email</span>
                <p class="data-value"><asp:Literal ID="litEmail" runat="server" /></p>
            </div>

            <div class="data-card">
                <span class="data-label">Teléfono</span>
                <p class="data-value"><asp:Literal ID="litTelefono" runat="server" /></p>
            </div>

            <div class="data-card">
                <span class="data-label">Fecha de nacimiento</span>
                <p class="data-value"><asp:Literal ID="litFechaNac" runat="server" /></p>
            </div>

            <div class="data-card">
                <span class="data-label">Sexo</span>
                <p class="data-value"><asp:Literal ID="litSexo" runat="server" /></p>
            </div>

            <div class="button-section">
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="edit-button" />
            </div>
        </div>
    </div>

</asp:Content>