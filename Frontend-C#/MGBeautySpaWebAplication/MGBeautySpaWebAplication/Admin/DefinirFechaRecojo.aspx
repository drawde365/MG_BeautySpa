<%@ Page Title="Definir fecha de recojo"
    Language="C#"
    MasterPageFile="~/Admin/Admin.Master"
    AutoEventWireup="true"
    CodeBehind="DefinirFechaRecojo.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Admin.DefinirFechaRecojo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Definir fecha de recojo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .page-title {
            font-family: 'ZCOOL XiaoWei', serif;
            font-size: 32px;
            margin-bottom: 4px;
            color: #1A0F12;
        }

        .page-subtitle {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 15px;
            color: #4B5563;
            margin-bottom: 16px;
        }

        .tabla-detalles-container {
            margin-top: 10px;
            background-color: #FAFAFA;
            border-radius: 12px;
            border: 1px solid #D3D4D9;
            overflow: hidden;
        }

        .tabla-detalles {
            width: 100%;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 14px;
        }

        .tabla-detalles thead th {
            background-color: #F3F4F6;
            padding: 8px 12px;
            font-weight: 600;
            border-bottom: none;
        }

        .tabla-detalles tbody td {
            padding: 8px 12px;
            border-top: 1px solid #E5E7EB;
            vertical-align: middle;
        }

        .faltante-alerta {
            color: #B91C1C;
            font-size: 13px;
        }

        .faltante-ok {
            color: #047857;
            font-size: 13px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hfIdPedido" runat="server" />

    <div>
        <h1 class="page-title">Definir fecha de recojo</h1>
        <div class="page-subtitle">
            <asp:Label ID="lblInfoPedido" runat="server" />
        </div>
    </div>

    <!-- Tabla de detalles con validación de stock -->
    <div class="tabla-detalles-container">
        <asp:Repeater ID="rptDetalles" runat="server" OnItemCommand="rptDetalles_ItemCommand">
            <HeaderTemplate>
                <table class="tabla-detalles">
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
            </HeaderTemplate>

            <ItemTemplate>
                <tr>
                    <td><%# Eval("NombreProducto") %></td>


                     <!-- NUEVA COLUMNA: Tipo de piel -->
                    <td><%# Eval("TipoPiel") %></td>

                    <td><%# Eval("Cantidad") %></td>
                    <td><%# Eval("StockFisico") %></td>
                    <td><%# Eval("StockDespacho") %></td>
                    <td>
                        <asp:Label ID="lblFaltante" runat="server"
                                   Text='<%# Eval("FaltanteTexto") %>'
                                   CssClass='<%# (Convert.ToInt32(Eval("Faltante")) < 0 ? "faltante-alerta" : "faltante-ok") %>' />
                    </td>
                    <td>
                        <div class="d-flex align-items-center">
                            <asp:TextBox ID="txtAgregar" runat="server"
                                         CssClass="form-control form-control-sm me-2"
                                         Width="80" />
                            <asp:LinkButton ID="btnAgregar" runat="server"
                                            CssClass="btn btn-sm btn-outline-primary"
                                            CommandName="AgregarStock"
                                            CommandArgument='<%# Eval("Index") %>'>
                                Añadir
                            </asp:LinkButton>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>

            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <div class="mt-3">
        <asp:Label ID="lblResumenStock" runat="server" />
    </div>

    <div class="mt-4">
        <label class="form-label">Fecha de recojo programada</label>
        <asp:TextBox ID="txtFechaRecojo" runat="server"
                     CssClass="form-control"
                     TextMode="Date" />
        <small class="text-muted">
            La fecha debe ser mayor a la fecha actual.
        </small>
    </div>

    <div class="mt-3">
        <asp:Button ID="btnGuardarFecha" runat="server"
                    CssClass="btn btn-primary"
                    Text="Confirmar fecha de recojo"
                    OnClick="btnGuardarFecha_Click" />
        <asp:HyperLink ID="lnkVolver" runat="server"
                       CssClass="btn btn-secondary ms-2"
                       NavigateUrl="AdmPedidos.aspx"
                       Text="Volver" />
    </div>

</asp:Content>
