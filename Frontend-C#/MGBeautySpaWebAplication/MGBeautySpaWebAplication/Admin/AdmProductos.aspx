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
        
        /* Botones de Acción (Base) */
        .btn-action { display: inline-flex; align-items: center; justify-content: center; width: 39px; height: 45px; border-radius: 15px; border: none; color: #1D1B20; margin: 0 4px; transition: opacity 0.2s; }
        
        /* Corrección: Iconos blancos siempre */
        .btn-action:hover { opacity: 0.85; text-decoration: none; }
        .btn-action:hover i { color: #FFFFFF !important; }
        .btn-action i { font-size: 1.25rem; color: #FFFFFF; }

        /* Colores Específicos */
        .btn-edit-admin { background-color: #1EC3B6; }
        .btn-delete-admin { background-color: #C31E1E; }
        .btn-stock-admin { background-color: #F59E0B; color: #fff; } /* Naranja para Stock */

        /* Botón Añadir */
        .btn-add-product { color: #107369; font-size: 50px; line-height: 1; text-decoration: none; }
        .btn-add-product:hover { color: #148C76; }
        
        /* Botón 'Ver Más' y Generales */
        .btn-custom-teal { 
            background-color: #1EC3B6; 
            color: #FCF7FA; 
            font-family: 'Plus Jakarta Sans', sans-serif; 
            font-weight: 700; 
            font-size: 14px; 
            line-height: 21px; 
            padding: 0 20px;
            height: 40px; 
            display: inline-flex; 
            align-items: center; 
            justify-content: center; 
            text-decoration: none; 
            border-radius: 20px; 
            border: none; 
            cursor: pointer; 
        }
        .btn-custom-teal:disabled { 
            background-color: #E3D4D9; 
            cursor: not-allowed; 
        }

        /* Estilos Buscador */
        .admin-search-box .input-group-text { border-right: 0; background-color: #fff; border-color: #E3D4D9; }
        .admin-search-box .form-control { border-left: 0; border-color: #E3D4D9; }
        .admin-search-box .form-control:focus { box-shadow: none; border-color: #1EC3B6; }

        /* Estilos Modal Stock */
        .stock-row { display: flex; align-items: center; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #eee; }
        .stock-row:last-child { border-bottom: none; }
        .stock-label { font-weight: 600; color: #148C76; width: 35%; font-size: 0.95rem; }
        .stock-info { font-size: 0.85rem; color: #666; width: 25%; text-align: center; line-height: 1.2; }
        .stock-input-group { width: 35%; display: flex; align-items: center; gap: 5px; }
        .stock-input-group input { text-align: center; font-weight: bold; }

    </style>
</asp:Content>

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
        <span class="input-group-text"><i class="bi bi-search"></i></span>
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre, código..." ClientIDMode="Static"></asp:TextBox>
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
                    <tbody>
            </HeaderTemplate>

            <ItemTemplate>
                <tr class="producto-fila">
                    <td>
                        <asp:Image ID="imgProducto" runat="server" 
                                   ImageUrl='<%# Eval("urlImagen") %>' 
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
                            <%# GenerarHtmlEstrellas(Eval("promedioValoracion")) %>
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

                        <button type="button" class="btn btn-action btn-stock-admin"
                                onclick="abrirModalStock(this)"
                                data-id='<%# Eval("idProducto") %>'
                                data-nombre='<%# Eval("nombre") %>'
                                data-stock-json='<%# ObtenerDatosStockJSON(Eval("idProducto")) %>'
                                title="Gestionar Stock">
                            <i class="bi bi-box-seam-fill"></i>
                        </button>

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
                    </tbody>
                </table>
            </FooterTemplate>

        </asp:Repeater>
    </div>

    <div class="d-flex justify-content-center align-items-center gap-3 mt-4">
        <button id="btnPagPrev" class="btn btn-custom-teal">&laquo; Anterior</button>
        <span id="lblPaginaActual" class="fw-bold">Página 1</span>
        <button id="btnPagNext" class="btn btn-custom-teal">Siguiente &raquo;</button>
    </div>

    <div class="modal fade" id="modalStock" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #1EC3B6; color: white;">
                    <h5 class="modal-title">Gestionar Stock: <span id="lblModalProdName" class="fw-light"></span></h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <p class="small text-muted mb-3 text-center">
                        Ingrese el <strong>Stock Físico</strong> actual. No puede ser menor que el stock comprometido en pedidos pendientes.
                    </p>
                    
                    <div id="containerStockInputs" style="max-height: 300px; overflow-y: auto; padding-right: 5px;"></div>

                    <asp:HiddenField ID="hdnIdProductoStock" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnJsonStockGuardar" runat="server" ClientIDMode="Static" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-custom-teal" onclick="guardarStock()">Guardar Cambios</button>
                    <asp:Button ID="btnSubmitStock" runat="server" style="display:none;" OnClick="btnGuardarStock_Click" ClientIDMode="Static" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    
    <script type="text/javascript">
        // ==========================================
        // LÓGICA MODAL STOCK
        // ==========================================

        function abrirModalStock(btn) {
            var id = $(btn).data('id');
            var nombre = $(btn).data('nombre');
            var jsonStock = $(btn).data('stock-json'); // Array de objetos del C#

            // Llenar datos base
            $('#hdnIdProductoStock').val(id);
            $('#lblModalProdName').text(nombre);
            
            var $container = $('#containerStockInputs');
            $container.empty();

            if (jsonStock && jsonStock.length > 0) {
                jsonStock.forEach(function(item) {
                    // item = { idTipo, nombreTipo, stockFisico, stockDespacho }
                    
                    // El mínimo permitido es lo que ya está comprometido (stockDespacho)
                    var minStock = item.stockDespacho; 

                    var html = `
                        <div class="stock-row">
                            <div class="stock-label">${item.nombreTipo}</div>
                            <div class="stock-info">
                                <small class="d-block text-muted" style="font-size: 0.75rem;">Por Despachar</small>
                                <strong class="text-danger">${item.stockDespacho}</strong>
                            </div>
                            <div class="stock-input-group">
                                <input type="number" class="form-control input-stock-fisico" 
                                       value="${item.stockFisico}" 
                                       min="${minStock}"
                                       data-tipo-id="${item.idTipo}"
                                       onchange="validarMinimo(this, ${minStock})"
                                       onkeypress="return (event.charCode >= 48 && event.charCode <= 57)">
                            </div>
                        </div>
                    `;
                    $container.append(html);
                });
            } else {
                $container.html('<div class="alert alert-warning text-center">Este producto no tiene configuración de tipos de piel.</div>');
            }

            var myModal = new bootstrap.Modal(document.getElementById('modalStock'));
            myModal.show();
        }

        function validarMinimo(input, min) {
            var val = parseInt(input.value) || 0;
            if (val < min) {
                alert("No puedes reducir el stock físico por debajo del stock comprometido (" + min + ").");
                input.value = min; 
            }
        }

        function guardarStock() {
            var listaCambios = [];
            
            // Recolectar datos de los inputs generados
            $('.input-stock-fisico').each(function() {
                var $inp = $(this);
                listaCambios.push({
                    idTipo: $inp.data('tipo-id'),
                    nuevoStock: parseInt($inp.val()) || 0
                });
            });

            // Poner JSON en HiddenField y enviar
            $('#hdnJsonStockGuardar').val(JSON.stringify(listaCambios));
            $('#btnSubmitStock').click();
        }

        // ==========================================
        // LÓGICA DE BÚSQUEDA Y PAGINACIÓN (EXISTENTE)
        // ==========================================
        $(document).ready(function () {
            let currentPage = 1;
            const itemsPerPage = 10;
            let $filas = $(".product-list-table tbody .producto-fila");

            function actualizarVista() {
                var searchTerm = $("#txtBuscar").val().toLowerCase();
                var $filasFiltradas = $filas.filter(function () {
                    var rowText = $(this).text().toLowerCase();
                    return rowText.includes(searchTerm);
                });

                var totalPages = Math.ceil($filasFiltradas.length / itemsPerPage);
                if (totalPages === 0) totalPages = 1;
                if (currentPage > totalPages) currentPage = 1;

                $filas.hide();
                var startIndex = (currentPage - 1) * itemsPerPage;
                var endIndex = startIndex + itemsPerPage;
                $filasFiltradas.slice(startIndex, endIndex).show();

                $("#lblPaginaActual").text(`Página ${currentPage} de ${totalPages}`);
                $("#btnPagPrev").prop("disabled", currentPage === 1);
                $("#btnPagNext").prop("disabled", currentPage === totalPages);
            }

            $("#txtBuscar").on("keyup", function () { currentPage = 1; actualizarVista(); });
            $("#btnPagNext").on("click", function () { currentPage++; actualizarVista(); });
            $("#btnPagPrev").on("click", function () { currentPage--; actualizarVista(); });

            $filas = $(".product-list-table tbody .producto-fila");
            actualizarVista();
        });
    </script>
</asp:Content>