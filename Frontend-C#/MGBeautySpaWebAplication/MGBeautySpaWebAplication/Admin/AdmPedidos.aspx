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
            margin-left: 5px;
            margin-right: 5px;
            cursor: pointer;
        }

        .action-definir-fecha   { background-color: #0C7C59; color: white; }
        .action-marcar-recogido { background-color: #1E88E5; color: white; }
        .action-cancelar        { background-color: #DC3545; color: white; }

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

    <!-- TÍTULO + BUSCADOR + FILTROS (CLIENT-SIDE) -->
    <div class="top-bar-pedidos mb-3">
        <div>
            <h1 class="admin-title-main">Administrar pedidos</h1>
            <div class="admin-title-sub">Pedidos realizados</div>
        </div>

        <div class="search-and-filters">

            <!-- BUSCADOR -->
            <div class="search-box">
                <input type="text"
                       id="searchPedidos"
                       class="form-control"
                       placeholder="Buscar por código, cliente, estado..." />
            </div>

            <!-- FILTRO: PEDIDOS CON / SIN FECHA LISTA PARA RECOGER -->
            <select id="filterFecha" class="filter-select">
                <option value="">Todos (con y sin fecha)</option>
                <option value="sin">Sin fecha de recojo</option>
                <option value="con">Con fecha de recojo</option>
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
                    data-tienefecha='<%# (Eval("FechaListaParaRecojo") == null ? "0" : "1") %>'>

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

                        <!-- Definir / ver fecha lista para recoger -->
                        <asp:LinkButton ID="btnDefinirFecha" runat="server"
                            CssClass="action-icon action-definir-fecha"
                            CommandName="DefinirFecha"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Definir / ver fecha 'lista para recoger'">
                            <i class="bi bi-calendar-check"></i>
                        </asp:LinkButton>

                        <!-- Registrar fecha de recojo (modal) -->
                        <asp:LinkButton ID="btnMarcarRecogido" runat="server"
                            CssClass="action-icon action-marcar-recogido"
                            CommandName="MarcarRecogido"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Registrar recojo">
                            <i class="bi bi-bag-check"></i>
                        </asp:LinkButton>

                        <!-- Marcar como NO recogido (modal confirmación) -->
                        <asp:LinkButton ID="btnCancelar" runat="server"
                            CssClass="action-icon action-cancelar"
                            CommandName="CancelarPedido"
                            CommandArgument='<%# Eval("IdPedido") %>'
                            ToolTip="Marcar como no recogido">
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

    <!-- ========================= -->
    <!-- MODAL: REGISTRAR RECOJO -->
    <!-- ========================= -->
    <asp:HiddenField ID="hfIdPedidoRecojo" runat="server" />
    <div class="modal fade" id="modalRecojo" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Registrar recojo</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:Label ID="lblInfoRecojo" runat="server"
                               CssClass="fw-semibold d-block mb-2"></asp:Label>

                    <div class="mb-3">
                        <label class="form-label">Fecha de recojo</label>
                        <asp:TextBox ID="txtFechaRecojoModal" runat="server"
                                     TextMode="Date"
                                     CssClass="form-control"
                                     Style="max-width: 260px;"></asp:TextBox>
                        <small class="text-muted">
                            Debe ser mayor o igual a la fecha &quot;lista para recoger&quot; y no puede ser futura.
                        </small>
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnGuardarFechaRecojo" runat="server"
                                CssClass="btn btn-primary"
                                Text="Guardar"
                                OnClick="btnGuardarFechaRecojo_Click" />
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        Cancelar
                    </button>
                </div>

            </div>
        </div>
    </div>

    <!-- ====================================== -->
    <!-- MODAL: CONFIRMAR MARCAR NO RECOGIDO  -->
    <!-- ====================================== -->
    <asp:HiddenField ID="hfIdPedidoCancelar" runat="server" />
    <div class="modal fade" id="modalCancelarPedido" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Marcar pedido como no recogido</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:Label ID="lblCancelarInfo" runat="server"
                               CssClass="d-block mb-2"></asp:Label>
                    <p class="mb-0">
                        ¿Estás seguro de marcar este pedido como <strong>NO RECOGIDO</strong>? Esta acción
                        actualizará el estado del pedido y no podrá revertirse desde esta pantalla.
                    </p>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnConfirmarCancelar" runat="server"
                                CssClass="btn btn-danger"
                                Text="Sí, marcar como no recogido"
                                OnClick="btnConfirmarCancelar_Click" />
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
                        Volver
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
            const filterFecha = document.getElementById("filterFecha");
            const filterEstado = document.getElementById("filterEstado");
            const rows = document.querySelectorAll("#tbodyPedidos tr[data-id]");

            function filtrarPedidos() {
                const text = (searchInput?.value || "").toLowerCase();
                const fFecha = filterFecha ? filterFecha.value : "";
                const fEstado = filterEstado ? filterEstado.value : "";

                rows.forEach(row => {
                    const estado = row.dataset.estado;        // CONFIRMADO / LISTO_PARA_RECOGER / ...
                    const tieneFecha = row.dataset.tienefecha; // "1" / "0"
                    const contenido = row.innerText.toLowerCase();

                    const okTexto = contenido.includes(text);

                    let okFecha = true;
                    if (fFecha === "con") {
                        okFecha = (tieneFecha === "1");
                    } else if (fFecha === "sin") {
                        okFecha = (tieneFecha === "0");
                    }

                    let okEstado = true;
                    if (fEstado !== "") {
                        okEstado = (estado === fEstado);
                    }

                    row.style.display = (okTexto && okFecha && okEstado) ? "" : "none";
                });
            }

            if (searchInput) searchInput.addEventListener("input", filtrarPedidos);
            if (filterFecha) filterFecha.addEventListener("change", filtrarPedidos);
            if (filterEstado) filterEstado.addEventListener("change", filtrarPedidos);

            filtrarPedidos(); // filtro inicial
        })();
    </script>
</asp:Content>
