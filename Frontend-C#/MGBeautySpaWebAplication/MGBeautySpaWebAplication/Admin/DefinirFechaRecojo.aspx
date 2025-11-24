<%@ Page Title="Definir fecha de recojo" Language="C#" MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true" CodeBehind="DefinirFechaRecojo.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Admin.DefinirFechaRecojo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Definir fecha de recojo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .tabla-detalles th {
            background-color: #F3F4F6;
            padding: 10px;
            font-weight: 600;
            border-bottom: 1px solid #ddd;
        }

        .tabla-detalles td {
            padding: 10px;
            border-bottom: 1px solid #eee;
        }

        /* Botón confirmar cuando está disabled */
        #btnGuardarFecha[disabled],
        #btnGuardarFecha:disabled {
            background-color: #C5C5C5 !important;
            color: #6C6C6C !important;
            cursor: not-allowed !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="admin-title-main">Definir fecha lista para recoger</h1>

    <div class="mb-3">
        <asp:Label ID="lblInfoPedido" runat="server" CssClass="fw-bold fs-5"></asp:Label>
    </div>

    <!-- TABLA DETALLES -->
    <table class="table tabla-detalles align-middle text-center">
        <thead>
            <tr>
                <th>Producto</th>
                <th>Tipo de piel</th>
                <th>Cant.</th>
                <th>Stock físico</th>
                <th>Stock por despachar</th>
                <th>Estado stock</th>
                <th>Agregar stock físico</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptDetalles" runat="server" OnItemCommand="rptDetalles_ItemCommand">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("NombreProducto") %></td>
                        <td><%# Eval("TipoPiel") %></td>
                        <td><%# Eval("Cantidad") %></td>
                        <td><%# Eval("StockFisico") %></td>
                        <td><%# Eval("StockDespacho") %></td>

                        <td>
                            <%# Convert.ToInt32(Eval("Faltante")) < 0
                                ? "<span style='color:#B91C1C;'>Faltan " + (-Convert.ToInt32(Eval("Faltante"))) + " unid.</span>"
                                : "<span style='color:#047857;'>OK</span>" %>
                        </td>

                        <td>
                            <asp:TextBox ID="txtAgregar" runat="server" CssClass="form-control" Style="max-width: 90px;" />
                            <asp:LinkButton ID="btnAgregar" runat="server"
                                Text="Añadir"
                                CssClass="btn btn-outline-primary mt-1"
                                CommandName="AgregarStock"
                                CommandArgument='<%# Eval("Index") %>'></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <!-- RESUMEN STOCK -->
    <div class="mb-4">
        <asp:Label ID="lblResumenStock" runat="server" CssClass="fw-semibold"></asp:Label>
    </div>

    <!-- FECHA DE RECOJO -->
    <div class="mt-3 mb-2">
        <label class="form-label fw-semibold">Lista para recoger:</label>

        <div class="input-group" style="max-width: 260px;">
            <asp:TextBox ID="txtFechaRecojo" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            <span class="input-group-text"><i class="bi bi-calendar-event"></i></span>
        </div>

        <small class="text-muted">La fecha debe ser mayor a la fecha actual.</small>
    </div>

    <!-- BOTONES -->
    <div class="d-flex gap-3 mt-4">
        <asp:Button ID="btnGuardarFecha" runat="server"
            Text="Confirmar fecha de recojo"
            CssClass="btn text-white"
            BackColor="#0C7C59"
            OnClick="btnGuardarFecha_Click" />

        <a href="AdmPedidos.aspx" class="btn btn-danger">Cancelar</a>
    </div>

    <asp:HiddenField ID="hfIdPedido" runat="server" />

    <!-- MODAL CONFIRMACIÓN FECHA GUARDADA -->
    <div class="modal fade" id="modalFechaOk" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Fecha de recojo guardada</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    La fecha de recojo se guardó correctamente.
                </div>

                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            onclick="window.location='AdmPedidos.aspx';">
                        Aceptar
                    </button>
                </div>

            </div>
        </div>
    </div>


</asp:Content>
