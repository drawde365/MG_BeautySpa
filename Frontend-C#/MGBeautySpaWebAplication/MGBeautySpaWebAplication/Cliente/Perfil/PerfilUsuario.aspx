<%@ Page Title="Mi Perfil"
    Language="C#"
    MasterPageFile="~/Cliente/Perfil/Perfil.Master"
    AutoEventWireup="true"
    CodeBehind="PerfilUsuario.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.PerfilUsuario" %>

<%-- CSS específico de Mi Perfil --%>
<asp:Content ID="ctHeadPerfil" ContentPlaceHolderID="PerfilHeadContent" runat="server">
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/ClienteCss/PerfilUsuarioCss.css") %>" />
</asp:Content>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <div class="profile-info-wrapper">
        <div class="profile-greeting-header">
            <h1 class="greeting-title">
                ¡Hola,
                <asp:Literal ID="litUserNameGreeting" runat="server" />!
            </h1>
            <p class="greeting-subtitle">
                Aquí puedes revisar y actualizar tus datos personales.
            </p>
        </div>

        <div class="profile-card">
            <h2 class="profile-card-title">Datos personales</h2>

            <asp:Panel ID="pnlReadOnly" runat="server" Visible="true">
                <div class="profile-field-group">
                    <label class="field-label">Nombres</label>
                    <p class="field-value">
                        <asp:Literal ID="litNombres" runat="server" />
                    </p>
                </div>

                <div class="profile-field-group">
                    <label class="field-label">Apellidos</label>
                    <p class="field-value">
                        <asp:Literal ID="litApellidos" runat="server" />
                    </p>
                </div>

                <div class="profile-field-group">
                    <label class="field-label">Email</label>
                    <p class="field-value">
                        <asp:Literal ID="litEmail" runat="server" />
                    </p>
                </div>

                <div class="profile-field-group">
                    <label class="field-label">Teléfono</label>
                    <p class="field-value">
                        <asp:Literal ID="litTelefono" runat="server" />
                    </p>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                <div class="profile-field-group">
                    <label class="field-label" for="<%= txtNombres.ClientID %>">Nombres</label>
                    <asp:TextBox ID="txtNombres" runat="server" CssClass="edit-field" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvNombres" runat="server" 
                        ControlToValidate="txtNombres" ErrorMessage="El nombre es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                </div>
                
                <div class="profile-field-group">
                    <label class="field-label" for="<%= txtPrimerApellido.ClientID %>">Primer Apellido</label>
                    <asp:TextBox ID="txtPrimerApellido" runat="server" CssClass="edit-field" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvPrimerApellido" runat="server" 
                        ControlToValidate="txtPrimerApellido" ErrorMessage="El primer apellido es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                </div>
                
                <div class="profile-field-group">
                    <label class="field-label" for="<%= txtSegundoApellido.ClientID %>">Segundo Apellido</label>
                    <asp:TextBox ID="txtSegundoApellido" runat="server" CssClass="edit-field" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvSegundoApellido" runat="server" 
                        ControlToValidate="txtSegundoApellido" ErrorMessage="El segundo apellido es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                </div>

                <div class="profile-field-group">
                    <label class="field-label">Email (no se puede editar)</label>
                    <p class="field-value">
                        <asp:Literal ID="litEmailEdit" runat="server" />
                    </p>
                </div>

                <div class="profile-field-group">
                    <label class="field-label" for="<%= txtTelefono.ClientID %>">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="edit-field" MaxLength="12" />
                    <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" 
                        ControlToValidate="txtTelefono" ErrorMessage="El teléfono es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revTelefono" runat="server"
                        ControlToValidate="txtTelefono" ErrorMessage="Debe ingresar solo números (máx 12)."
                        ValidationExpression="^[0-9]{1,12}$" CssClass="validator-error" Display="Dynamic" />
                </div>
            </asp:Panel>
            
            <div class="profile-actions-row">
                <asp:Button ID="btnEditarPerfil" runat="server" Text="Editar Perfil" 
                    CssClass="btn-custom-teal" 
                    OnClick="btnEditarPerfil_Click" 
                    Visible="true" />
                
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" 
                    CssClass="btn-custom-teal" 
                    OnClick="btnGuardar_Click" 
                    Visible="false" />
                    
                <asp:Button ID="btnVolver" runat="server" Text="Cancelar" 
                    CssClass="btn-regresar" 
                    OnClick="btnVolver_Click"
                    CausesValidation="false" 
                    Visible="false" />
            </div>
        </div>
    </div>

</asp:Content>
