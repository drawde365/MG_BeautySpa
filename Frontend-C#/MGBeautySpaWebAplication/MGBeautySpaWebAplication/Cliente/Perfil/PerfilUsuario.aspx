<%@ Page Title="Mi Perfil"
    Language="C#"
    MasterPageFile="~/Cliente/Perfil/Perfil.Master"
    AutoEventWireup="true"
    CodeBehind="PerfilUsuario.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.PerfilUsuario" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">
    <head>
        <style>
            .btn-custom-teal {
                /* Estilo de los botones 'Depth 5, Frame 1' */
                background-color: #1EC3B6;
                color: #FCF7FA;
                font-family: 'Plus Jakarta Sans', sans-serif;
                font-weight: 700;
                font-size: 14px;
                line-height: 21px;
                /* Dimensiones y alineación del botón */
                width: 143px;
                height: 40px;
                display: inline-flex;
                align-items: center;
                justify-content: center;
                text-decoration: none;
            }
        </style>
    </head>
    <div class="profile-info-wrapper">
        <div class="profile-greeting-header">
            <h1 class="greeting-title">¡Hola,
                <asp:Literal ID="litUserNameGreeting" runat="server" />!
            </h1>
            <p class="greeting-subtitle">
                Aquí puedes revisar y actualizar tus datos personales.
            </p>
        </div>

        <div class="password-change-container" style="max-width: 600px; margin: 2rem auto; padding: 1rem 2rem; background: #fff; border-radius: 8px; box-shadow: 0 2px 6px rgba(0,0,0,0.03);">
            <h2 style="margin-bottom: 1rem;">Datos personales</h2>

            <div style="margin-bottom: 1rem;">
                <label class="field-label">Nombres</label><br />
                <p class="field-value" style="width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 6px; margin: 6px 0 0 0;">
                    <asp:Literal ID="litNombres" runat="server" />
                </p>
            </div>

            <div style="margin-bottom: 1rem;">
                <label class="field-label">Apellidos</label><br />
                <p class="field-value" style="width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 6px; margin: 6px 0 0 0;">
                    <asp:Literal ID="litApellidos" runat="server" />
                </p>
            </div>

            <div style="margin-bottom: 1rem;">
                <label class="field-label">Email</label><br />
                <p class="field-value" style="width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 6px; margin: 6px 0 0 0;">
                    <asp:Literal ID="litEmail" runat="server" />
                </p>
            </div>

            <div style="margin-bottom: 1rem;">
                <label class="field-label">Teléfono</label><br />
                <p class="field-value" style="width: 100%; padding: 8px; border: 1px solid #ccc; border-radius: 6px; margin: 6px 0 0 0;">
                    <asp:Literal ID="litTelefono" runat="server" />
                </p>
            </div>
            
            <div style="margin-top: 1rem; display: flex; gap: 10px;">
                <asp:Button ID="btnEditarPerfil" runat="server" Text="Editar" CssClass="btn"
                    Style="background-color: #107369; color: white; border: none; padding: 8px 16px; border-radius: 6px;" />
                <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn"
                    Style="background-color: #aaa; color: white; border: none; padding: 8px 16px; border-radius: 6px;" />
            </div>
        </>

        <div class="button-section">
            <a href="<%: ResolveUrl("~/Cuenta/ModificarCuenta.aspx") %>"
                class="btn btn-custom-teal rounded-pill">Editar pefil
            </a>
        </div>
    </div>

</asp:Content>
