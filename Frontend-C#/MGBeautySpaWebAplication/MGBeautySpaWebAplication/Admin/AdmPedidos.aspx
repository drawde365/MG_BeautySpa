<%@ Page Title="Administrar pedidos"
    Language="C#"
    MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="AdmPedidos.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Admin.AdmPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Administrar pedidos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" />

    <style>
        .admin-title-main {
            font-family: 'ZCOOL XiaoWei', serif;
            font-size: 40px;
            color: #1A0F12;
            margin-bottom: 8px;
        }

        .admin-title-sub {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 20px;
            color: #1A0F12;
            margin-bottom: 12px;
        }

        .top-bar-pedidos {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        @media (min-width: 992px) {
            .top-bar-pedidos {
                flex-direction: row;
                justify-content: space-between;
                align-items: center;
            }
        }

        .search-and-filters {
            display: flex;
            flex-direction: column;
            gap: 8px;
        }

        @media (min-width: 768px) {
            .search-and-filters {
                flex-direction: row;
                align-items: center;
                gap: 12px;
            }
        }

        .search-box {
            max-width: 420px;
            position: relative;
            flex: 1;
        }

        .search-box input {
            width: 100%;
            border-radius: 999px;
            border: 1px solid #D3D4D9;
            padding: 8px 16px;
            font-size: 14px;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        .filter-select {
            min-width: 180px;
            border-radius: 12px;
            border: 1px solid #D3D4D9;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 13px;
            padding: 6px 10px;
        }

        .tabla-pedidos-container {
            background-color: #FAFAFA;
            border: 1px solid #D3D4D9;
            border-radius: 12px;
            overflow: hidden;
            margin-top: 10px;
        }

        .tabla-pedidos {
            width: 100%;
            margin-bottom: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 14px;
        }

        .tabla-pedidos thead th {
            padding: 10px 14px;
            background-color: #F3F4F6;
            font-weight: 600;
            color: #1A0F12;
            border-bottom: none;
            white-space: nowrap;
        }

        .tabla-pedidos tbody td {
            padding: 9px 14px;
            border-top: 1px solid #E5E7EB;
            vertical-align: middle;
        }

        .badge-estado {
            display: inline-block;
            border-radius: 999px;
            padding: 4px 10px;
            font-size: 12px;
            font-weight: 600;
        }

        .badge-confirmado {
            background-color: #FFF4D8;
            color: #8A5B00;
        }

        .badge-entregado {
            background-color: #D8FBE7;
            color: #006C3F;
        }

        .badge-norecogido {
            background-color: #FFE0E0;
            color: #A30021;
        }

        /* Botones de acciones en fila (estilo productos) */
        .acciones-pedido {
            text-align: center;
            white-space: nowrap;
        }

        .action-icon {
            width: 40px;
            height: 40px;
            border-radius: 999px;
            border: none;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            margin-left: 5px;
            margin-right: 5px;
            cursor: pointer;
        }

        .action-definir-fecha { background-color: #0C7C59; color: white; }      /* calendario */
        .action-marcar-recogido { background-color: #1E88E5; color: white; }    /* bolsa check */
        .action-cancelar { background-color: #DC3545; color: white; }           /* papelera */

        .texto-sin-pedidos {
            padding: 20px;
            text-align: center;
            color: #6B7280;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }
    </style>

    <script type="text/javascript">
        // Filtro de texto (búsqueda por código, cliente, etc.)
        function filtrarPedidos() {
            var input = document.getElementById('<%= txtBuscarPedidos.ClientID %>');
            var filtro = input.value.toLowerCase();
            var filas = document.querySelectorAll('.fila-pedido');

            for (var i = 0; i < filas.length; i++) {
                var texto = filas[i].textContent || filas[i].innerText;
                texto = texto.toLowerCase();
                filas[i].style.display = texto.indexOf(filtro) !== -1 ? '' : 'none';
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <!-- TÍTULO + BUSCADOR + FILTROS -->
    <div class="top-bar-pedidos mb-3">
        <div>
            <h1 class="admin-title-main">Administrar pedidos</h1>
            <div class="admin-title-sub">Pedidos realizados</div>
        </div>

        <div class="search-and-filters">
            <div class="search-box">
                <asp:TextBox ID="txtBuscarPedidos" runat="server"
                    CssClass="form-control"
                    placeholder="Buscar por código, cliente, estado..."
                    onkeyup="filtrarPedidos()" />
            </div>

            <!-- Filtro por fecha para recoger -->
            <asp:DropDownList ID="ddlFiltroFechaLista" runat="server"
                CssClass="filter-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="Filtros_SelectedIndexChanged">
            </asp:DropDownList>

            <!-- Filtro por estado de recojo -->
            <asp:DropDownList ID="ddlFiltroRecojo" runat="server"
                CssClass="filter-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="Filtros_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>

    <!-- TABLA DE PEDIDOS -->
    <div class="tabla-pedidos-container">

        <asp:Repeater ID="rptPedidos" runat="server" OnItemCommand="rptPedidos_ItemCommand">
            <HeaderTemplate>
                <table class="table tabla-pedidos align-middle">
                    <thead>
                        <tr>
                            <th>Pedido</th>
                            <th>Cliente</th>
                            <th>Estado</th>
                            <th>Fecha pago</th>
                            <th>Fecha lista para recoger</th>
                            <th>Fecha de recojo</th>
                            <th>Total</th>
                            <th style="text-align:center;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>

            <ItemTemplate>
                <tr class="fila-pedido">
                    <td>
                        <strong>#<%# Eval("PedidoId") %></strong><br />
                        <small>CODTR: <%# Eval("CodTr") %></small>
                    </td>

                    <td><%# Eval("NombreCliente") %></td>

                    <td>
                        <span class='<%# "badge-estado " + ObtenerClaseEstado(Eval("Estado")) %>'>
                            <%# Eval("Estado") %>
                        </span>
                    </td>

                    <td><%# Eval("FechaPago", "{0:dd/MM/yyyy}") %></td>
                    <td><%# Eval("FechaListaParaRecoger", "{0:dd/MM/yyyy}") %></td>
                    <td><%# Eval("FechaRecojo", "{0:dd/MM/yyyy}") %></td>

                    <td>S/. <%# Eval("Total", "{0:F2}") %></td>

                    <!-- ACCIONES CON ICONOS (como productos) -->
                    <td class="acciones-pedido">

                        <!-- Definir fecha "lista para recoger" -->
                        <asp:LinkButton ID="btnDefinirFecha" runat="server"
                            CssClass="action-icon action-definir-fecha"
                            CommandName="DefinirFecha"
                            CommandArgument='<%# Eval("PedidoId") %>'
                            ToolTip="Definir fecha para recoger">
                            <i class="bi bi-calendar-check"></i>
                        </asp:LinkButton>

                        <!-- Marcar pedido como recogido (solo debe funcionar si ya tiene fecha para recoger) -->
                        <asp:LinkButton ID="btnMarcarRecogido" runat="server"
                            CssClass="action-icon action-marcar-recogido"
                            CommandName="MarcarRecogido"
                            CommandArgument='<%# Eval("PedidoId") %>'
                            ToolTip="Marcar como recogido">
                            <i class="bi bi-bag-check"></i>
                        </asp:LinkButton>

                        <!-- Cancelar pedido (usa ícono de papelera como en productos) -->
                        <asp:LinkButton ID="btnCancelar" runat="server"
                            CssClass="action-icon action-cancelar"
                            CommandName="CancelarPedido"
                            CommandArgument='<%# Eval("PedidoId") %>'
                            ToolTip="Cancelar pedido">
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

        <asp:Panel ID="pnlSinPedidos" runat="server" Visible="false" CssClass="texto-sin-pedidos">
            No hay pedidos para mostrar.
        </asp:Panel>

    </div>

    <!-- MODAL: DEFINIR FECHA PARA RECOGER -->
    <asp:HiddenField ID="hfPedidoSeleccionado" runat="server" />

    <div class="modal fade" id="modalFechaRecojo" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Definir fecha de recojo</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:Label ID="lblInfoPedido" runat="server" />
                    <div class="mt-3">
                        <label class="form-label">Fecha de recojo</label>
                        <asp:TextBox ID="txtFechaRecojo" runat="server"
                                     CssClass="form-control"
                                     TextMode="Date" />
                    </div>
                    <p class="mt-3 small text-muted">
                        Antes de guardar se valida el stock físico contra el stock por despachar
                        y la cantidad solicitada de cada producto del pedido.
                    </p>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnConfirmarFecha" runat="server"
                        CssClass="btn btn-primary"
                        Text="Guardar fecha"
                        OnClick="btnConfirmarFecha_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        Cerrar
                    </button>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
