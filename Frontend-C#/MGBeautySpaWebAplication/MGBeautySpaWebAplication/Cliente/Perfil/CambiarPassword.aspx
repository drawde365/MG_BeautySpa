<%@ Page Title="Cambiar Contraseña"
    Language="C#"
    MasterPageFile="~/Cliente/Perfil/Perfil.Master"
    AutoEventWireup="true"
    CodeBehind="CambiarPassword.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Perfil.CambiarPassword" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <div class="password-change-container mx-auto my-4 p-4 shadow-sm bg-white rounded-3" style="max-width:480px;">
        <h3 class="text-center mb-4">Cambiar contraseña</h3>

        <div class="mb-3">
            <label for="<%= txtAntigua.ClientID %>" class="form-label">Contraseña actual</label>
            <asp:TextBox ID="txtAntigua" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         placeholder="Ingresa tu contraseña actual" />
        </div>

        <div class="mb-3">
            <label for="<%= txtNueva.ClientID %>" class="form-label">Nueva contraseña</label>
            <asp:TextBox ID="txtNueva" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         placeholder="Ingresa una nueva contraseña" />
        </div>

        <div class="mb-4">
            <label for="<%= txtVerificar.ClientID %>" class="form-label">Confirmar nueva contraseña</label>
            <asp:TextBox ID="txtVerificar" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         placeholder="Vuelve a escribir la nueva contraseña" />
        </div>

        <div class="d-flex justify-content-between">
            <asp:Button ID="btnGuardar" runat="server"
                        Text="Guardar cambios"
                        CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click" />

            <asp:Button ID="btnCancelar" runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-secondary"
                        OnClick="btnCancelar_Click" />
        </div>

        <div class="mt-3">
            <asp:Label ID="lblInfo" runat="server" CssClass="text-muted" />
        </div>
    </div>

</asp:Content>