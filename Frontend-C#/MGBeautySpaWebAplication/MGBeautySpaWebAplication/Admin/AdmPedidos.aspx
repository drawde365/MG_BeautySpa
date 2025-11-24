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

        .badge-confirmado { background-color: #FFF4D8; color: #8A5B00; }
        .badge-listo       { background-color: #D8FBE7; color: #006C3F; }
        .badge-recogido    { background-color: #C7E1FF; color: #084298; }
        .badge-norecogido  { background-color: #FFE0E0; color: #A30021; }

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
            margin-left: 4px;
            margin-right: 4px;
            cursor: pointer;
        }

        .action-ver            { background-color: #E5E7EB; color: #111827; }
        .action-definir-fecha  { background-color: #0C7C59; color: white; }
        .action-marcar-recogido{ background-color: #1E88E5; color: white; }
        .action-cancelar       { background-color: #DC3545; color: white; }

        .action-icon[disabled="disabled"] {
            opacity: 0.4;
            cursor: default;
        }

        .texto-sin-pedidos {
            padding: 20px;
            text-align: center;
            color: #6B7280;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }
    </style>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <!-- TÍTULO + BUSCADOR + FILTROS -->
    <div class="top-bar-pedidos mb-3">
        <div>
            <h1 class="admin-title-main">Administrar pedidos</h1>
            <div class="admin-title-sub">Pedidos realizados</div>
        </div>

        <div class="search-and-filters">

            <!-- BUSCADOR (por nombre de cliente) -->
            <div class="search-box">
                <input type="text"
                       id="searchPedidos"
                       class="form-control"
                       placeholder="Buscar por nombre de cliente..." />
            </div>

            <!-- ORDENAR POR TOTAL -->
            <select id="orderTotal" class="filter-select">
                <option value="">Sin ordenar</option>
                <option value="desc">Mayor total de dinero</option>
            </select>

            <!-- FILTRO: ESTADO DEL PEDIDO -->
            <select id="filterEstado" class="filter-select">
                <option value="">Todos los estados</option>
                <option value="CONFIRMADO">Confirmado</option>
                <option value="LISTO_PARA_RECOGER">Listo para recoger</option>
                <option value="RECOGIDO">Recogido</option>
                <option value="NO_RECOGIDO">No recogido</option>
            </select>

        </div>
    </div>

    <!-- TABLA DE PEDIDOS -->
    <div class="tabla-pedidos-container">

        <asp:Repeater ID="rptPedidos" runat="server"
                      OnItemCommand="rptPedidos_ItemCommand"
                      OnItemDataBound="rptPedidos_ItemDataBound">
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
                    <tbody id="tbodyPedidos">
            </HeaderTemplate>

            <ItemTemplate>
                <tr data-id='<%# Eval("IdPedido") %>'
                    data-estado='<%# Eval("Estado") %>'
                    data-tienefecha='<%# (Eval("FechaListaParaRecojo") == null ? "0" : "1") %>'
                    data-cliente='<%# Eval("NombreCliente") %>'
                    data-total='<%# Eval("Total", "{0:F2}") %>'>

                    <td>
                        <strong>#<%# Eval("IdPedido") %></strong><br />
                        <small>CODTR: <%# Eval("CodigoTransaccion") %></small>
                    </td>

                    <td><%# Eval("NombreCliente") %></td>

                    <td>
                        <span class='<%# "badge-estado " + ObtenerClaseEstado(Eval("Estado")) %>'>
                            <%# Eval("Estado") %>
                        </span>
                    </td>

                    <td><%# Eval("FechaPago", "{0:dd/MM/yyyy}") %></td>
                    <td><%# Eval("FechaListaParaRecojo", "{0:dd/MM/yyyy}") %></td>
                    <td><%# Eval("FechaRecojo", "{0:dd/MM/yyyy}") %></td>

                    <td>S/. <%# Eval("Total", "{0:F2}") %></td>

                    <!-- ACCIONES -->
                    <td class="acciones-pedido">

                        <!-- Ver pedido (siempre) -->
                        <asp:LinkButton ID="btnVerPedido" runat="server"
                            CssClass="action-icon action-ver"
                            CommandName="VerPedido"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Ver detalle del pedido">
                            <i class="bi bi-eye"></i>
                        </asp:LinkButton>

                        <!-- Asignar fecha (CONFIRMADO sin fecha lista para recoger) -->
                        <asp:LinkButton ID="btnDefinirFecha" runat="server"
                            CssClass="action-icon action-definir-fecha"
                            CommandName="DefinirFecha"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Definir fecha de recojo">
                            <i class="bi bi-calendar-check"></i>
                        </asp:LinkButton>

                        <!-- Marcar como recogido (LISTO_PARA_RECOGER) -->
                        <asp:LinkButton ID="btnMarcarRecogido" runat="server"
                            CssClass="action-icon action-marcar-recogido"
                            CommandName="MarcarRecogido"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Marcar como recogido">
                            <i class="bi bi-bag-check"></i>
                        </asp:LinkButton>

                        <!-- Marcar como no recogido (LISTO_PARA_RECOGER) -->
                        <asp:LinkButton ID="btnCancelar" runat="server"
                            CssClass="action-icon action-cancelar"
                            CommandName="CancelarPedido"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Marcar como no recogido">
                            <i class="bi bi-x-lg"></i>
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

    <!-- MODAL DETALLES DE PEDIDO -->
    <div class="modal fade" id="modalDetallesPedido" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">
                        Detalle del pedido
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <div class="mb-3">
                        <p class="mb-1">
                            <strong>Pedido:</strong>
                            <asp:Literal ID="litDetPedido" runat="server"></asp:Literal>
                        </p>
                        <p class="mb-1">
                            <strong>Cliente:</strong>
                            <asp:Literal ID="litDetCliente" runat="server"></asp:Literal>
                        </p>
                        <p class="mb-1">
                            <strong>Código transacción:</strong>
                            <asp:Literal ID="litDetCodTr" runat="server"></asp:Literal>
                        </p>
                        <p class="mb-0">
                            <strong>Estado:</strong>
                            <asp:Literal ID="litDetEstado" runat="server"></asp:Literal>
                        </p>
                    </div>

                    <div class="table-responsive">
                        <table class="table table-sm table-bordered align-middle">
                            <thead class="table-light">
                                <tr>
                                    <th>Producto</th>
                                    <th>Tipo de piel</th>
                                    <th class="text-end">Cantidad</th>
                                    <th class="text-end">Subtotal (S/.)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptDetallesPedido" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("NombreProducto") %></td>
                                            <td><%# Eval("TipoPiel") %></td>
                                            <td class="text-end"><%# Eval("Cantidad") %></td>
                                            <td class="text-end"><%# Eval("Subtotal", "{0:F2}") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <div class="text-end">
                        <strong>Total: S/. </strong>
                        <asp:Literal ID="litDetTotal" runat="server"></asp:Literal>
                    </div>

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        Cerrar
                    </button>
                </div>

            </div>
        </div>
    </div>

    <!-- MODAL: MARCAR COMO RECOGIDO -->
    <div class="modal fade" id="modalMarcarRecogido" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Marcar pedido como recogido</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:HiddenField ID="hfldPedidoRecogido" runat="server" />

                    <p class="mb-1">
                        <strong>Pedido:</strong>
                        <asp:Literal ID="litRecogidoPedido" runat="server"></asp:Literal>
                    </p>
                    <p class="mb-1">
                        <strong>Cliente:</strong>
                        <asp:Literal ID="litRecogidoCliente" runat="server"></asp:Literal>
                    </p>
                    <p class="mb-3">
                        <strong>Fecha lista para recoger:</strong>
                        <asp:Literal ID="litRecogidoFechaProgramada" runat="server"></asp:Literal>
                    </p>

                    <div class="mb-3">
                        <label for="txtFechaRecojoReal" class="form-label">
                            Fecha real de recojo
                        </label>
                        <asp:TextBox ID="txtFechaRecojoReal" runat="server"
                                     CssClass="form-control"
                                     TextMode="Date"></asp:TextBox>
                        <small class="text-muted">
                            Debe ser una fecha pasada o de hoy, y mayor o igual a la fecha lista para recoger.
                        </small>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        Cancelar
                    </button>
                    <asp:LinkButton ID="btnConfirmarRecogido" runat="server"
                                    CssClass="btn btn-primary"
                                    OnClick="btnConfirmarRecogido_Click">
                        Guardar
                    </asp:LinkButton>
                </div>

            </div>
        </div>
    </div>

    <!-- MODAL: MARCAR COMO NO RECOGIDO -->
    <div class="modal fade" id="modalNoRecogido" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Marcar pedido como NO recogido</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:HiddenField ID="hfldPedidoNoRecogido" runat="server" />

                    <p class="mb-1">
                        <strong>Pedido:</strong>
                        <asp:Literal ID="litNoRecogidoPedido" runat="server"></asp:Literal>
                    </p>
                    <p class="mb-3">
                        <strong>Cliente:</strong>
                        <asp:Literal ID="litNoRecogidoCliente" runat="server"></asp:Literal>
                    </p>

                    <p class="text-danger mb-0">
                        ¿Está seguro de marcar este pedido como <strong>NO_RECOGIDO</strong>?
                    </p>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        Cancelar
                    </button>
                    <asp:LinkButton ID="btnConfirmarNoRecogido" runat="server"
                                    CssClass="btn btn-danger"
                                    OnClick="btnConfirmarNoRecogido_Click">
                        Sí, marcar como no recogido
                    </asp:LinkButton>
                </div>

            </div>
        </div>
    </div>

    <!-- MODAL: MENSAJE DE ACCIÓN (CONFIRMACIÓN BONITA) -->
    <div class="modal fade" id="modalMensajeAccion" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Acción realizada</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:Literal ID="litMensajeAccion" runat="server"></asp:Literal>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-bs-dismiss="modal">
                        Aceptar
                    </button>
                </div>

            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script>
        (function () {
            const searchInput = document.getElementById("searchPedidos");
            const filterEstado = document.getElementById("filterEstado");
            const orderTotal = document.getElementById("orderTotal");
            const tbody = document.getElementById("tbodyPedidos");

            function getRows() {
                return Array.from(tbody.querySelectorAll("tr[data-id]"));
            }

            function filtrarPedidos() {
                const text = (searchInput?.value || "").toLowerCase();
                const fEstado = filterEstado ? filterEstado.value : "";

                getRows().forEach(row => {
                    const estado = row.dataset.estado;
                    const cliente = (row.dataset.cliente || "").toLowerCase();

                    const okTexto = cliente.includes(text);
                    let okEstado = true;

                    if (fEstado !== "") {
                        okEstado = (estado === fEstado);
                    }

                    row.style.display = (okTexto && okEstado) ? "" : "none";
                });
            }

            function ordenarPorTotal() {
                const modo = orderTotal ? orderTotal.value : "";
                if (!modo) return;

                const filas = getRows();
                filas.sort((a, b) => {
                    const ta = parseFloat(a.dataset.total || "0");
                    const tb = parseFloat(b.dataset.total || "0");
                    return modo === "desc" ? (tb - ta) : (ta - tb);
                });

                filas.forEach(f => tbody.appendChild(f));
            }

            if (searchInput) searchInput.addEventListener("input", filtrarPedidos);
            if (filterEstado) filterEstado.addEventListener("change", filtrarPedidos);
            if (orderTotal) orderTotal.addEventListener("change", ordenarPorTotal);

            filtrarPedidos();
        })();
    </script>
</asp:Content>
