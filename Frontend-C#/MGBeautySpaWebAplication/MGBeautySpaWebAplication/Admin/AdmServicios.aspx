<%@ Page Title="Administrar Servicios" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdmServicios.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.AdmServicios" %>

<%-- 1. BLOQUE DE ESTILOS --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    <style>
        /* [Estilos CSS... (sin cambios, omitidos por brevedad)] */
        .h1-admin-title { font-family: 'ZCOOL XiaoWei', serif; font-size: 48px; line-height: 40px; color: #1A0F12; }
        .h2-admin-subtitle { font-family: 'Plus Jakarta Sans', sans-serif; font-weight: 700; font-size: 22px; color: #1A0F12; }
        .table-container { background: #FAFAFA; border: 1px solid #E3D4D9; border-radius: 12px; overflow: hidden; margin-top: 12px; }
        .service-list-table { margin-bottom: 0; font-family: 'Plus Jakarta Sans', sans-serif; width: 100%; }
        .service-list-table thead th { background: #FAFAFA; font-weight: 500; font-size: 14px; color: #1A0F12; padding: 12px 16px; text-align: left; border-bottom: 0; }
        .service-list-table tbody tr { border-top: 1px solid #E6E8EB; background-color: #FFFFFF; }
        .service-list-table tbody td { font-size: 14px; font-weight: 400; color: #1A0F12; padding: 8px 16px; vertical-align: middle; text-align: left; }
        .service-image-thumb { width: 62px; height: 62px; object-fit: cover; border-radius: 12px; }
        .service-name-link { font-weight: 400; color: #107369; text-decoration: none; }
        .service-name-link:hover { text-decoration: underline; }
        .service-price { font-weight: 800; color: #148C76; }
        .rating-stars { color: #148C76; font-size: 1.1rem; letter-spacing: 2px; }
        .rating-stars .star-empty { color: #78D5CD; }
        .btn-action { display: inline-flex; align-items: center; justify-content: center; width: 39px; height: 45px; border-radius: 15px; border: none; margin: 0 4px; }
        .btn-edit-admin { background-color: #1EC3B6; }
        .btn-delete-admin { background-color: #C31E1E; }
        .btn-action i { font-size: 1.25rem; color: #FFFFFF; }
        .btn-add-product { color: #107369; font-size: 50px; line-height: 1; text-decoration: none; }
        .btn-add-service:hover { color: #148C76; }
        .btn-custom-teal { background-color: #1EC3B6; color: #FCF7FA; font-family: 'Plus Jakarta Sans', sans-serif; font-weight: 700; font-size: 14px; line-height: 21px; width: 143px; height: 40px; display: inline-flex; align-items: center; justify-content: center; text-decoration: none; }
    </style>
</asp:Content>


<%-- 2. CONTENIDO PRINCIPAL --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- (NOTA: Tu MasterPage DEBE tener un <asp:ScriptManager> 
         para que el UpdatePanel funcione) --%>

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1 class="h1-admin-title">Administrar Servicios</h1>
    </div>

    <div class="d-flex justify-content-between align-items-center mb-2">
        <h2 class="h2-admin-subtitle">Servicios disponibles</h2>
        <asp:HyperLink ID="hlAddService" runat="server" NavigateUrl="~/Admin/InsertarServicio.aspx" 
                       CssClass="btn-add-product" ToolTip="Añadir nuevo servicio">
            <i class="bi bi-plus-circle-fill"></i>
        </asp:HyperLink>
    </div>

    <%-- ▼▼▼ 1. ENVUELVE EL CONTENIDO EN UN UPDATEPANEL ▼▼▼ --%>
    <asp:UpdatePanel ID="upServicios" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="table-container">
                
                <%-- ▼▼▼ 2. AÑADE EL OnItemCommand AL REPEATER ▼▼▼ --%>
                <asp:Repeater ID="rptServicios" runat="server" 
                              OnItemCommand="rptServicios_ItemCommand"> 
                    <HeaderTemplate>
                        <table class="table service-list-table align-middle">
                            <thead>
                                <tr>
                                    <th colspan="2">Servicio</th>
                                    <th>Codigo</th>
                                    <th>Tipo</th>
                                    <th>Precio</th>
                                    <th>Valoración</th>
                                    <th class="text-center">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Image ID="imgServicio" runat="server" 
                                           ImageUrl='<%# Eval("RutaImagen", "~{0}") %>' 
                                           CssClass="service-image-thumb" />
                            </td>
                            <td>
                                <asp:HyperLink ID="hlNombre" runat="server" 
                                               NavigateUrl='<%# Eval("UrlEditar", "~{0}") %>' 
                                               Text='<%# Eval("NombreServicio") %>' 
                                               CssClass="service-name-link" />
                            </td>
                            <td>
                                <asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("Codigo") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("Tipo") %>' />
                            </td>
                            <td>
                                <asp:Label ID="lblPrecio" runat="server" Text='<%# Eval("Precio", "S/. {0:F2}") %>' CssClass="service-price" />
                            </td>
                            <td>
                                <%-- ▼▼▼ 3. HACE LAS ESTRELLAS DINÁMICAS ▼▼▼ --%>
                                <div class="rating-stars">
                                    <%# RenderStars(Eval("Valoracion")) %>
                                </div>
                            </td>
                            <td class="text-center">
                                <asp:LinkButton ID="btnEditar" runat="server" 
                                                CssClass="btn btn-action btn-edit-admin" 
                                                CommandName="Editar" 
                                                CommandArgument='<%# Eval("IDServicio") %>'
                                                ToolTip="Editar">
                                    <i class="bi bi-pencil-fill"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnEliminar" runat="server" 
                                                CssClass="btn btn-action btn-delete-admin" 
                                                CommandName="Eliminar" 
                                                CommandArgument='<%# Eval("IDServicio") %>'
                                                ToolTip="Eliminar"
                                                OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este servicio?');">
                                    <i class="bi bi-trash-fill"></i>
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>

                    <FooterTemplate>
                        </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <asp:LinkButton ID="btnVerMas" runat="server" CssClass="btn btn-custom-teal rounded-pill"
                            OnClick="btnVerMas_Click">Ver más</asp:LinkButton>

        </ContentTemplate>
        
        <%-- ▼▼▼ 4. AÑADE LOS TRIGGERS PARA EL UPDATEPANEL ▼▼▼ --%>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnVerMas" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="rptServicios" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>


<%-- 3. SCRIPTS --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <%-- jQuery es necesario para que los scripts de UpdatePanel funcionen 
         correctamente, aunque tu MasterPage ya debería tenerlo si usa Bootstrap. --%>
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
</asp:Content>