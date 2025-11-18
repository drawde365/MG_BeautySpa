<%@ Page Title="Mi Perfil"
    Language="C#"
    MasterPageFile="~/Cliente/Perfil/Perfil.Master"
    AutoEventWireup="true"
    CodeBehind="PerfilUsuario.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.PerfilUsuario" %>

<%-- NO HAY NADA AQUÍ ARRIBA --%>

<%-- Solo hay UN bloque de contenido para "ProfileBodyContent" --%>
<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">
    
    <%-- 1. El <style> va AQUÍ, dentro del Content --%>
    <style>
        .btn-custom-teal {
            /* Estilo de los botones 'Depth 5, Frame 1' */

           
            background-color:#107369; 
            color:white; 
            border:none; 
            border-radius:6px; 
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 14px;            
            align-items: center;
            justify-content: center;
            line-height: 21px;
            /* Dimensiones y alineación del botón */
            width: 143px;
            height: 40px;
            display: inline-flex;
            text-decoration: none;
        }
        .btn-custom-teal:hover {
            background-color: #107369; /* Un tono más oscuro para el hover */
            color: white;
        }
        
        .btn-regresar {
             background-color: #7b7b7b; 
             color: white; 
             border: none; 
             padding: 8px 16px; 
             border-radius: 6px;
             text-decoration: none;
             font-family: 'Plus Jakarta Sans', sans-serif;
             font-size: 14px;
             display: inline-flex;
             align-items: center;
             justify-content: center;
             height: 40px;
        }
        .btn-regresar:hover {
            background-color: #888;
            color: white;
        }

        /* Estilos para los campos y validadores */
        .field-label { font-weight: 600; }
        .field-value, .edit-field { 
            width: 100%; padding: 8px; border: 1px solid #ccc; 
            border-radius: 6px; margin: 6px 0 0 0; box-sizing: border-box; 
        }
        .field-value { background-color: #f9f9f9; } /* Fondo gris para campos de solo lectura */
        .edit-field { background-color: #fff; } /* Fondo blanco para campos editables */
        
        .validator-error { 
            color: #D9534F; font-size: 0.9em; 
            display: block; margin-top: 4px;
        }
    </style>

    <%-- 2. El HTML va a continuación, en el MISMO Content --%>
    <div class="profile-info-wrapper">
        <div class="profile-greeting-header">
            <h1 class="greeting-title">¡Hola,
                <asp:Literal ID="litUserNameGreeting" runat="server" />!
            </h1>
            <p class="greeting-subtitle">
                Aquí puedes revisar y actualizar tus datos personales.
            </p>
        </div>

        <%-- Contenedor principal de los datos --%>
        <div class="password-change-container" style="max-width: 600px; margin: 2rem auto; padding: 1rem 2rem; background: #fff; border-radius: 8px; box-shadow: 0 2px 6px rgba(0,0,0,0.03);">
            <h2 style="margin-bottom: 1rem;">Datos personales</h2>

            <asp:Panel ID="pnlReadOnly" runat="server" Visible="true">
                <div style="margin-bottom: 1rem;">
                    <label class="field-label">Nombres</label><br />
                    <p class="field-value">
                        <asp:Literal ID="litNombres" runat="server" />
                    </p>
                </div>
                <div style="margin-bottom: 1rem;">
                    <label class="field-label">Apellidos</label><br />
                    <p class="field-value">
                        <asp:Literal ID="litApellidos" runat="server" />
                    </p>
                </div>
                <div style="margin-bottom: 1rem;">
                    <label class="field-label">Email</label><br />
                    <p class="field-value">
                        <asp:Literal ID="litEmail" runat="server" />
                    </p>
                </div>
                <div style="margin-bottom: 1rem;">
                    <label class="field-label">Teléfono</label><br />
                    <p class="field-value">
                        <asp:Literal ID="litTelefono" runat="server" />
                    </p>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlEdit" runat="server" Visible="false">
                <div style="margin-bottom: 1rem;">
                    <label class="field-label" for="<%= txtNombres.ClientID %>">Nombres</label><br />
                    <asp:TextBox ID="txtNombres" runat="server" CssClass="edit-field" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvNombres" runat="server" 
                        ControlToValidate="txtNombres" ErrorMessage="El nombre es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                </div>
                
                <div style="margin-bottom: 1rem;">
                    <label class="field-label" for="<%= txtPrimerApellido.ClientID %>">Primer Apellido</label><br />
                    <asp:TextBox ID="txtPrimerApellido" runat="server" CssClass="edit-field" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvPrimerApellido" runat="server" 
                        ControlToValidate="txtPrimerApellido" ErrorMessage="El primer apellido es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                </div>
                
                <div style="margin-bottom: 1rem;">
                    <label class="field-label" for="<%= txtSegundoApellido.ClientID %>">Segundo Apellido</label><br />
                    <asp:TextBox ID="txtSegundoApellido" runat="server" CssClass="edit-field" MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvSegundoApellido" runat="server" 
                        ControlToValidate="txtSegundoApellido" ErrorMessage="El segundo apellido es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                </div>

                <div style="margin-bottom: 1rem;">
                    <label class="field-label">Email (No se puede editar)</label><br />
                    <p class="field-value">
                        <asp:Literal ID="litEmailEdit" runat="server" />
                    </p>
                </div>

                <div style="margin-bottom: 1rem;">
                    <label class="field-label" for="<%= txtTelefono.ClientID %>">Teléfono</label><br />
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="edit-field" MaxLength="12" />
                    <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" 
                        ControlToValidate="txtTelefono" ErrorMessage="El teléfono es obligatorio." 
                        CssClass="validator-error" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revTelefono" runat="server"
                        ControlToValidate="txtTelefono" ErrorMessage="Debe ingresar solo números (máx 12)."
                        ValidationExpression="^[0-9]{1,12}$" CssClass="validator-error" Display="Dynamic" />
                </div>
            </asp:Panel>
            
            <div style="margin-top: 1.5rem; display: flex; gap: 10px;">
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