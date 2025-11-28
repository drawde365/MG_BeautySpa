<%@ Page Title="Mis Pedidos"
Language="C#"
MasterPageFile="~/Cliente/Perfil/Perfil.Master"
AutoEventWireup="true"
CodeBehind="Pedidos.aspx.cs"
Inherits="MGBeautySpaWebAplication.Cliente.Perfil.Pedidos" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <link rel="stylesheet" href="Pedidos.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">

    <div class="orders-container">
        <h2 class="orders-title">Historial de Pedidos</h2>

        <asp:Repeater ID="rptPedidos" runat="server">
            <ItemTemplate>
                <div class="order-card">
                    <div class="order-info">
                        <h3 class="order-number">Pedido N.º <%# Eval("NumeroPedido") %></h3>
                        <p class="order-detail">Fecha: <%# Eval("FechaCompra") %></p>
                        <p class="order-detail">Subtotal: <%# Eval("Subtotal") %></p>
                    </div>

                    <div class="order-actions">
                        <asp:HyperLink 
                            ID="lnkDetalles" 
                            runat="server"
                            CssClass="btn-detalles"
                            NavigateUrl='<%# "~/Cliente/Perfil/DetallePedido.aspx?pedido=" + Eval("NumeroPedido") %>'>
                            Detalles
                        </asp:HyperLink>
                    </div>

                </div>
            </ItemTemplate>
        </asp:Repeater>

        <!-- Panel de pedidos vacíos -->
        <asp:Panel ID="pnlNoPedidos" runat="server" CssClass="no-pedidos-mensaje" Visible="false">
            Aún no tienes pedidos en tu historial.
        </asp:Panel>

        <div class="orders-footer">
            <asp:Button ID="btnVerMas" runat="server"
                        Text="Ver más"
                        CssClass="btn-vermas"
                        OnClick="btnVerMas_Click" />

            <asp:Button ID="btnRegresar" runat="server"
                        Text="Regresar"
                        CssClass="btn-regresar"
                        OnClick="btnRegresar_Click" />
        </div>
    </div>

</asp:Content>
