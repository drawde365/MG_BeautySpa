<%@ Page Title="Cambiar Contraseña"
    Language="C#"
    MasterPageFile="~/Cliente/Perfil/Perfil.Master"
    AutoEventWireup="true"
    CodeBehind="CambiarPassword.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Perfil.CambiarPassword" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <div class="password-change-container" style="max-width:600px; margin:2rem auto; padding:1rem 2rem;">
        <h2 style="margin-bottom:1rem;">Cambio de contraseña</h2>

        <div style="margin-bottom:1rem;">
            <label for="<%= txtAntigua.ClientID %>">Contraseña anterior</label><br />
            <asp:TextBox ID="txtAntigua" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         style="width:100%; padding:6px; border:1px solid #ccc; border-radius:4px;" />
        </div>

        <div style="margin-bottom:1rem;">
            <label for="<%= txtNueva.ClientID %>">Nueva contraseña</label><br />
            <asp:TextBox ID="txtNueva" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         style="width:100%; padding:6px; border:1px solid #ccc; border-radius:4px;" />
        </div>

        <div style="margin-bottom:1.5rem;">
            <label for="<%= txtVerificar.ClientID %>">Repite nueva contraseña</label><br />
            <asp:TextBox ID="txtVerificar" runat="server"
                         TextMode="Password"
                         CssClass="form-control"
                         style="width:100%; padding:6px; border:1px solid #ccc; border-radius:4px;" />
        </div>

        <div style="margin-top:1rem;">
            <asp:Button ID="btnGuardar" runat="server"
                        Text="Guardar cambios"
                        CssClass="btn btn-primary"
                        OnClick="btnGuardar_Click"
                        style="background-color:#107369; color:white; border:none; padding:8px 16px; border-radius:6px; margin-right:10px;" />

            <asp:Button ID="btnCancelar" runat="server"
                        Text="Cancelar"
                        CssClass="btn btn-secondary"
                        OnClick="btnCancelar_Click"
                        style="background-color:#aaa; color:white; border:none; padding:8px 16px; border-radius:6px;" />
        </div>

        <div style="margin-top:1rem;">
            <asp:Label ID="lblInfo" runat="server" CssClass="text-muted" ForeColor="Red" />
        </div>
    </div>

</asp:Content>