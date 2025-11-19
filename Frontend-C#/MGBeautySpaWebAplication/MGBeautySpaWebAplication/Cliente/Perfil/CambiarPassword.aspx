<%@ Page Title="Cambiar Contraseña"
    Language="C#"
    MasterPageFile="~/Cliente/Perfil/Perfil.Master"
    AutoEventWireup="true"
    CodeBehind="CambiarPassword.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Perfil.CambiarPassword" %>

<asp:Content ID="ctHead" ContentPlaceHolderID="PerfilHeadContent" runat="server">
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/ClienteCss/CambiarPasswordCss.css") %>" />
</asp:Content>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <!-- AUTOCOMPLETE FIX -->
    <div style="position:absolute; top:-9999px;">
        <asp:TextBox ID="dummyUser" runat="server" autocomplete="username" />
        <asp:TextBox ID="dummyPass" runat="server" TextMode="Password" autocomplete="current-password" />
    </div>

    <div class="password-container">

        <h2>Cambio de contraseña</h2>

        <label class="password-label" for="<%= txtAntigua.ClientID %>">Contraseña actual</label>
        <asp:TextBox ID="txtAntigua" runat="server"
                     CssClass="password-input"
                     TextMode="Password"
                     autocomplete="current-password" />

        <label class="password-label" for="<%= txtNueva.ClientID %>">Nueva contraseña</label>
        <asp:TextBox ID="txtNueva" runat="server"
                     CssClass="password-input"
                     TextMode="Password"
                     autocomplete="new-password" />

        <label class="password-label" for="<%= txtVerificar.ClientID %>">Repite nueva contraseña</label>
        <asp:TextBox ID="txtVerificar" runat="server"
                     CssClass="password-input"
                     TextMode="Password"
                     autocomplete="new-password" />

        <div style="margin-top:1.5rem;">
            <asp:Button ID="btnGuardar"
                        runat="server"
                        CssClass="btn-save"
                        Text="Guardar cambios"
                        OnClick="btnGuardar_Click" />

            <asp:Button ID="btnCancelar"
                        runat="server"
                        CssClass="btn-cancel"
                        Text="Cancelar"
                        OnClick="btnCancelar_Click" />
        </div>

        <asp:Label ID="lblInfo" runat="server" CssClass="password-message" />
    </div>

</asp:Content>
