<%@ Page Title="Administrar Servicios" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AdmServicios.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.AdmServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    <style>
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
        .btn-add-service { color: #107369; font-size: 50px; line-height: 1; text-decoration: none; }
        .btn-add-service:hover { color: #148C76; }
        
        .btn-custom-teal { 
            background-color: #1EC3B6; 
            color: #FCF7FA; 
            font-family: 'Plus Jakarta Sans', sans-serif; 
            font-weight: 700; 
            font-size: 14px; 
            line-height: 21px; 
            width: 143px; 
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
            border-color: #1EC3B6; 
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1 class="h1-admin-title">Administrar Servicios</h1>
    </div>

    <div class="d-flex justify-content-between align-items-center mb-2">
        <h2 class="h2-admin-subtitle">Servicios disponibles</h2>
        <asp:HyperLink ID="hlAddService" runat="server" NavigateUrl="~/Admin/InsertarServicio.aspx" 
                       CssClass="btn-add-service" ToolTip="Añadir nuevo servicio">
            <i class="bi bi-plus-circle-fill"></i>
        </asp:HyperLink>
    </div>

    <div class="input-group admin-search-box" style="width: 300px;">
        <span class="input-group-text">
            <i class="bi bi-search"></i>
        </span>
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre, código, tipo..." ClientIDMode="Static"></asp:TextBox>
    </div>

    <div class="table-container">
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
                <tr class="servicio-fila">
                    <td>
                        <asp:Image ID="imgServicio" runat="server" 
                                   ImageUrl='<%# Eval("RutaImagen") %>' 
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
                            <%# RenderStars(Eval("Valoracion")) %>
                        </div>
                    </td>
                    <td class="text-center">
                        <asp:LinkButton ID="btnEditar" runat="server" 
                                        CssClass="btn btn-action btn-edit-admin" 
                                        CommandName="Editar" 
                                        CommandArgument='<%# Eval("IDServicio") %>'
                                        PostBackUrl='<%# Eval("IDServicio", "~/Admin/InsertarServicio.aspx?id={0}") %>'
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

    <div class="d-flex justify-content-center align-items-center gap-3 mt-4">
        <button id="btnPagPrev" class="btn btn-custom-teal">&laquo; Anterior</button>
        <span id="lblPaginaActual" class="fw-bold">Página 1</span>
        <button id="btnPagNext" class="btn btn-custom-teal">Siguiente &raquo;</button>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            
            let currentPage = 1;
            const itemsPerPage = 10;
            let $filas = $(".service-list-table tbody .servicio-fila");

            function actualizarVista() {
                var searchTerm = $("#txtBuscar").val().toLowerCase();

                var $filasFiltradas = $filas.filter(function () {
                    var rowText = $(this).text().toLowerCase();
                    return rowText.includes(searchTerm);
                });

                var totalPages = Math.ceil($filasFiltradas.length / itemsPerPage);
                if (totalPages === 0) totalPages = 1;
                if (currentPage > totalPages) {
                    currentPage = 1;
                }

                $filas.hide();

                var startIndex = (currentPage - 1) * itemsPerPage;
                var endIndex = startIndex + itemsPerPage;

                $filasFiltradas.slice(startIndex, endIndex).show();

                $("#lblPaginaActual").text(`Página ${currentPage} de ${totalPages}`);
                
                $("#btnPagPrev").prop("disabled", currentPage === 1);
                $("#btnPagNext").prop("disabled", currentPage === totalPages);
            }

            $("#txtBuscar").on("keyup", function () {
                currentPage = 1;
                actualizarVista();
            });

            $("#btnPagNext").on("click", function () {
                currentPage++;
                actualizarVista();
            });

            $("#btnPagPrev").on("click", function () {
                currentPage--;
                actualizarVista();
            });

            $filas = $(".service-list-table tbody .servicio-fila");
            actualizarVista(); 
        });
    </script>
</asp:Content>