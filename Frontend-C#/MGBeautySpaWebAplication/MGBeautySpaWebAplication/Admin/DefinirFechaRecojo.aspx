<%@ Page Title="Definir fecha de recojo" Async="true" Language="C#"
    MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="DefinirFechaRecojo.aspx.cs"
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

        /* -------------------------
           NUEVO ESTILO PARA AÑADIR STOCK
           ------------------------- */
        .btn-add-stock {
            background-color: white;
            color: #0B63CE;
            border: 1px solid #0B63CE;
            border-radius: 999px;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 13px;
            font-weight: 600;
            padding: 4px 14px;
            line-height: 1.1;
            display: inline-block;
        }

        .btn-add-stock:hover {
            background-color: #0B63CE;
            color: white;
        }

        .td-add-stock {
            width: 150px;
            text-align: center;
            vertical-align: middle;
        }

        .td-add-stock .form-control {
            height: 32px;
            font-size: 13px;
            padding: 4px 8px;
            margin: auto;
            text-align: center;
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
            <asp:Repeater ID="rptDetalles" runat="server"
                          OnItemCommand="rptDetalles_ItemCommand">
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

                        <td class="td-add-stock">
                            <asp:TextBox ID="txtAgregar" runat="server"
                                         CssClass="form-control"
                                         Style="max-width: 90px;"></asp:TextBox>

                            <asp:LinkButton ID="btnAgregar"
                                runat="server"
                                CommandName="AgregarStock"
                                CommandArgument='<%# Eval("Index") %>'
                                CssClass="btn-add-stock mt-2">
                                Añadir
                            </asp:LinkButton>
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

    <!-- FECHA LISTA PARA RECOGER -->
    <div class="mt-3 mb-2">
        <label class="form-label fw-semibold">Lista para recoger (fecha actual):</label>

        <p class="mb-1">
            <asp:Label ID="lblFechaActual" runat="server" CssClass="fw-bold"></asp:Label>
        </p>

        <small class="text-muted">
            Se utilizará automáticamente la fecha actual.
        </small>
    </div>

    <asp:HiddenField ID="hfFechaActual" runat="server" />

    <!-- BOTONES -->
    <div class="d-flex gap-3 mt-4">
        <asp:Button ID="btnGuardarFecha" runat="server"
            Text="Definir fecha actual"
            CssClass="btn text-white"
            BackColor="#0C7C59"
            OnClick="btnGuardarFecha_Click" />

        <a href="AdmPedidos.aspx" class="btn btn-danger">Cancelar</a>
    </div>

    <asp:HiddenField ID="hfIdPedido" runat="server" />

    <!-- MODAL CONFIRMACIÓN -->
    <div class="modal fade" id="modalFechaOk" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Fecha lista para recoger guardada</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    La fecha lista para recoger se guardó correctamente.
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
