<%@ Page Title="Administrar Servicios" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdmServicios.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.AdmServicios" %>

<%-- 
    1. BLOQUE DE ESTILOS
    Este CSS se insertará en el "HeadContent" de tu Master Page.
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- Cargamos Bootstrap Icons (si no lo has hecho en el master) --%>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">

    <style>
        /* Título de la página (Administrar Servicios) */
        .h1-admin-title {
            font-family: 'ZCOOL XiaoWei', serif;
            font-size: 48px; [cite_start]/* [cite: 1619] */
            line-height: 40px; [cite_start]/* [cite: 1619] */
            color: #1A0F12; [cite_start]/* [cite: 1619] */
        }

        /* Subtítulo (Servicios disponibles) */
        .h2-admin-subtitle {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700; [cite_start]/* [cite: 1624] */
            font-size: 22px; [cite_start]/* [cite: 1624] */
            color: #1A0F12; [cite_start]/* [cite: 1624] */
        }
        
        /* Contenedor de la tabla */
        .table-container {
            background: #FAFAFA; [cite_start]/* [cite: 1632] */
            border: 1px solid #E3D4D9; [cite_start]/* [cite: 1632] */
            border-radius: 12px; [cite_start]/* [cite: 1632] */
            overflow: hidden;
            margin-top: 12px;
        }

        /* La tabla de servicios */
        .service-list-table {
            margin-bottom: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            width: 100%;
        }

        /* Cabecera de la tabla */
        .service-list-table thead th {
            background: #FAFAFA; [cite_start]/* [cite: 1637] */
            font-weight: 500; [cite_start]/* [cite: 1640] */
            font-size: 14px; [cite_start]/* [cite: 1640] */
            color: #1A0F12; [cite_start]/* [cite: 1641] */
            padding: 12px 16px; [cite_start]/* [cite: 1638] */
            text-align: left;
            border-bottom: 0;
        }

        /* Filas de la tabla */
        .service-list-table tbody tr {
            border-top: 1px solid #E6E8EB; [cite_start]/* [cite: 1661] */
            background-color: #FFFFFF; /* Fondo blanco para filas */
        }

        .service-list-table tbody td {
            font-size: 14px;
            font-weight: 400; [cite_start]/* [cite: 1668, 1672] */
            color: #1A0F12; [cite_start]/* [cite: 1673] */
            padding: 8px 16px; [cite_start]/* [cite: 1662, 1667] */
            vertical-align: middle;
            text-align: left;
        }

        /* Columna de Imagen (Servicio) */
        .service-image-thumb {
            width: 62px; [cite_start]/* [cite: 1664] */
            height: 62px; [cite_start]/* [cite: 1664] */
            object-fit: cover;
            border-radius: 12px; [cite_start]/* [cite: 1664] */
        }
        
        /* Nombre del Servicio (link) */
        .service-name-link {
            font-weight: 400;
            color: #107369; [cite_start]/* [cite: 1669] */
            text-decoration: none;
        }
        .service-name-link:hover {
            text-decoration: underline;
        }

        /* Columna Precio */
        .service-price {
            font-weight: 800; [cite_start]/* [cite: 1680] */
            color: #148C76; [cite_start]/* [cite: 1681] */
        }

        /* Columna Valoración (Estrellas) */
        .rating-stars {
            color: #148C76; [cite_start]/* [cite: 1689] */
            font-size: 1.1rem; /* Ajustado de 20px */
            letter-spacing: 2px; [cite_start]/* [cite: 1684] */
        }
        .rating-stars .star-empty {
            color: #78D5CD; /* (color de estrella vacía de diseño anterior) */
        }

        /* Columna Acciones (Botones) */
        .btn-action {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 39px;  [cite_start]/* [cite: 1713] */
            height: 45px; [cite_start]/* [cite: 1713] */
            border-radius: 15px; [cite_start]/* [cite: 1713] */
            border: none;
            margin: 0 4px;
        }
        .btn-edit-admin {
            background-color: #1EC3B6; [cite_start]/* [cite: 1714] */
        }
        .btn-delete-admin {
            background-color: #C31E1E; [cite_start]/* [cite: 1721] */
        }
        .btn-action i {
            font-size: 1.25rem;
            color: #FFFFFF; /* Iconos blancos para mejor contraste */
        }
        
        /* Botón de Añadir Servicio (+) */
        .btn-add-product {
            color: #107369; /* [cite: 1594] */
            font-size: 50px; /*  */
            line-height: 1;
            text-decoration: none;
        }
        .btn-add-service:hover {
            color: #148C76;
        }

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
</asp:Content>


<%-- 
    2. CONTENIDO PRINCIPAL
    Este es el contenido que se insertará en el "MainContent"
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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


    <div class="table-container">
        
        <%-- 
            Usamos un Repeater para construir la tabla dinámicamente.
        --%>
        <asp:Repeater ID="rptServicios" runat="server">
            
            <HeaderTemplate>
                <table class="table service-list-table align-middle">
                    <thead>
                        <tr>
                            <%-- 
                                [cite_start]NOTA: El diseño decía "Producto" [cite: 1640] aquí, 
                                lo he corregido a "Servicio"
                            --%>
                            <th colspan="2">Servicio</th> <%-- Colspan 2 para imagen y nombre --%>
                            <th>Codigo</th> <th>Tipo</th> <th>Precio</th> <th>Valoración</th> <th class="text-center">Acciones</th>
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
                        <div class="rating-stars">
                            [cite_start]<%-- Ejemplo estático basado en el diseño [cite: 1684-1710] --%>
                            <i class="bi bi-star-fill"></i>
                            <i class="bi bi-star-fill"></i>
                            <i class="bi bi-star-fill"></i>
                            <i class="bi bi-star-fill"></i>
                            <i class="bi bi-star-fill"></i>
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

</asp:Content>


<%-- 
    3. SCRIPTS (Opcional)
--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <%-- Tus scripts aquí --%>
</asp:Content>