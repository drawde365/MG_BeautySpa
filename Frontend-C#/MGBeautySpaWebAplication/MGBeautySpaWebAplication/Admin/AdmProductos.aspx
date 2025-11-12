<%@ Page Title="Administrar Productos" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdmProductos.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.AdmProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    <style>
        
        /* Título de la página */
        .h1-admin-title { font-family: 'ZCOOL XiaoWei', serif; font-size: 48px; line-height: 40px; color: #1A0F12; }
        /* Subtítulo */
        .h2-admin-subtitle { font-family: 'Plus Jakarta Sans', sans-serif; font-weight: 700; font-size: 22px; color: #1A0F12; }
        /* Contenedor de la tabla */
        .table-container { background: #FAFAFA; border: 1px solid #E3D4D9; border-radius: 12px; overflow: hidden; margin-top: 12px; }
        .product-list-table { margin-bottom: 0; font-family: 'Plus Jakarta Sans', sans-serif; width: 100%; }
        /* Cabecera de la tabla */
        .product-list-table thead th { background: #FAFAFA; font-weight: 500; font-size: 14px; color: #1A0F12; padding: 12px 16px; text-align: left; border-bottom: 0; }
        /* Filas de la tabla */
        .product-list-table tbody tr { border-top: 1px solid #E6E8EB; background-color: #FFFFFF; }
        .product-list-table tbody td { font-size: 14px; font-weight: 400; color: #1A0F12; padding: 8px 16px; vertical-align: middle; text-align: left; }
        /* Imagen */
        .product-image-thumb { width: 62px; height: 62px; object-fit: cover; border-radius: 12px; }
        .product-name-link { font-weight: 400; color: #107369; text-decoration: none; }
        .product-name-link:hover { text-decoration: underline; }
        /* Precio */
        .product-price { font-weight: 800; }
        /* Estrellas */
        .rating-stars { color: #148C76; font-size: 1.1rem; letter-spacing: 2px; }
        .rating-stars .star-empty { color: #78D5CD; }
        /* Botones de Acción */
        .btn-action { display: inline-flex; align-items: center; justify-content: center; width: 39px; height: 45px; border-radius: 15px; border: none; color: #1D1B20; margin: 0 4px; }
        .btn-edit-admin { background-color: #1EC3B6; }
        .btn-delete-admin { background-color: #C31E1E; }
        .btn-action i { font-size: 1.25rem; color: #FFFFFF; }
        /* Botón Añadir */
        .btn-add-product { color: #107369; font-size: 50px; line-height: 1; text-decoration: none; }
        .btn-add-product:hover { color: #148C76; }
        /* Botón 'Ver Más' */
        .btn-custom-teal { background-color: #1EC3B6; color: #FCF7FA; font-family: 'Plus Jakarta Sans', sans-serif; font-weight: 700; font-size: 14px; line-height: 21px; width: 143px; height: 40px; display: inline-flex; align-items: center; justify-content: center; text-decoration: none; }
        /* Badges */
        .badge-estado { padding: 4px 16px; height: 32px; min-width: 84px; font-weight: 500; font-size: 14px; line-height: 24px; border-radius: 16px; display: inline-block; }

        /* ▼▼▼ ESTILO NUEVO PARA EL BUSCADOR ▼▼▼ */
        .admin-search-box .input-group-text {
            border-right: 0;
            background-color: #fff;
            border-color: #E3D4D9;
        }
        .admin-search-box .form-control {
            border-left: 0;
            border-color: #E3D4D9;
        }
        .admin-search-box .form-control:focus {
            box-shadow: none;
            border-color: #1EC3B6; /* Resalta el borde al hacer foco */
        }
        /* ▲▲▲ FIN ESTILO NUEVO ▲▲▲ */

    </style>
</asp:Content>


<%-- 
    2. CONTENIDO PRINCIPAL
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1 class="h1-admin-title">Administrar Productos</h1>
    </div>

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="h2-admin-subtitle">Productos disponibles</h2>
        
        <div class="d-flex gap-2 align-items-center">
            
            

            <asp:HyperLink ID="hlAddProduct" runat="server" NavigateUrl="~/Admin/InsertarProducto.aspx" 
                           CssClass="btn-add-product">
                <i class="bi bi-plus-circle-fill"></i>
            </asp:HyperLink>
        </div>
        
    </div>

    <div class="input-group admin-search-box" style="width: 300px;">
    <span class="input-group-text">
        <i class="bi bi-search"></i>
    </span>
    
    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre, código, ..." ClientIDMode="Static">
    </asp:TextBox>
    </div>

    <div class="table-container">
        
        <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
            
            <HeaderTemplate>
                <table class="table product-list-table align-middle">
                    <thead>
                        <tr>
                            <th colspan="2">Producto</th>
                            <th>Codigo</th> 
                            <th>Precio</th> 
                            <th>Valoración</th> 
                            <th class="text-center">Acciones</th>
                        </tr>
                    </thead>
                    <tbody> <%-- Importante añadir tbody para que el script lo encuentre --%>
            </HeaderTemplate>

            <ItemTemplate>
                <%-- ▼▼▼ CAMBIO 2: Añadí una clase "producto-fila" al <tr> ▼▼▼ --%>
                <tr class="producto-fila">
                    <td>
                        <asp:Image ID="imgProducto" runat="server" 
                                   ImageUrl='<%# Eval("urlImagen", "~{0}") %>' 
                                   CssClass="product-image-thumb" />
                    </td>
                    <td>
                        <asp:HyperLink ID="hlNombre" runat="server"  
                                       Text='<%# Eval("nombre") %>' 
                                       CssClass="product-name-link" />
                    </td>
                    <td>
                        <asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("idProducto") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblPrecio" runat="server" Text='<%# Eval("precio", "S/. {0:F2}") %>' CssClass="product-price" />
                    </td>
                    <td>
                        <div class="rating-stars">
                            <i class="bi bi-star-fill"></i> <i class="bi bi-star-fill"></i> <i class="bi bi-star-fill"></i> <i class="bi bi-star-fill"></i> <i class="bi bi-star-fill star-empty"></i>
                        </div>
                    </td>
                    <td class="text-center">
                        <asp:LinkButton ID="btnEditar" runat="server" 
                                        CssClass="btn btn-action btn-edit-admin" 
                                        CommandName="Editar" 
                                        CommandArgument='<%# Eval("idProducto") %>'
                                        ToolTip="Editar">
                            <i class="bi bi-pencil-fill"></i>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" 
                                        CssClass="btn btn-action btn-delete-admin" 
                                        CommandName="Eliminar" 
                                        CommandArgument='<%# Eval("idProducto") %>'
                                        ToolTip="Eliminar"
                                        OnClientClick="return confirm('¿Estás seguro de que deseas dar de baja a este producto?');">
                            <i class="bi bi-trash-fill"></i>
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>

            <FooterTemplate>
                    </tbody> <%-- Importante cerrar el tbody --%>
                </table>
            </FooterTemplate>

        </asp:Repeater>
    </div>

    <asp:LinkButton ID="btnVerMas" runat="server" CssClass="btn btn-custom-teal rounded-pill"
        OnClick="btnVerMas_Click">Ver más</asp:LinkButton>

</asp:Content>


<%-- 
    3. SCRIPTS
--%>
zz<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    
    <script type="text/javascript">
        // Espera a que la página esté completamente cargada
        $(document).ready(function () {
            
            // Busca el <input> con id "txtBuscar" y escucha el evento "keyup"
            // (se dispara cada vez que levantas una tecla)
            $("#txtBuscar").on("keyup", function () {
                
                // 1. Obtiene el texto del buscador y lo convierte a minúsculas
                var searchTerm = $(this).val().toLowerCase();

                // 2. Itera sobre cada fila (tr) que tenga la clase "producto-fila"
                //    (que están dentro del <tbody> de nuestra tabla)
                $(".product-list-table tbody .producto-fila").each(function () {
                    
                    // 3. Obtiene TODO el texto de esa fila y lo convierte a minúsculas
                    var rowText = $(this).text().toLowerCase();

                    // 4. Comprueba si el texto de la fila INCLUYE el término de búsqueda
                    if (rowText.includes(searchTerm)) {
                        // Si lo incluye, muestra la fila
                        $(this).show();
                    } else {
                        // Si NO lo incluye, oculta la fila
                        $(this).hide();
                    }
                });
            });
        });
    </script>
    </asp:Content>