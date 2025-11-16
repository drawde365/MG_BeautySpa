<%@ Page Title="Mis Pedidos"
Language="C#"
MasterPageFile="~/Cliente/Perfil/Perfil.Master"
AutoEventWireup="true"
CodeBehind="Pedidos.aspx.cs"
Inherits="MGBeautySpaWebAplication.Cliente.Perfil.Pedidos" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <link rel="stylesheet" href="Pedidos.css" />

    <div class="orders-container">
        <h2 class="orders-title">Historial de Pedidos</h2>

        <asp:Repeater ID="rptPedidos" runat="server" OnItemCommand="btnDetalles_Command">
            <ItemTemplate>
                <div class="order-card">
                    <div class="order-info">
                        <h3 class="order-number">Pedido N.º <%# Eval("NumeroPedido") %></h3>
                        <p class="order-detail">Fecha: <%# Eval("FechaCompra") %></p>
                        <p class="order-detail">Subtotal: <%# Eval("Subtotal") %></p>
                    </div>
                    <div class="order-actions">
                        <asp:Button ID="btnDetalles" runat="server" 
                                    Text="Detalles"
                                    CssClass="btn-detalles"
                                    CommandArgument='<%# Eval("NumeroPedido") %>'
                                    OnCommand="btnDetalles_Command" />
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <%-- ▼▼▼ MENSAJE DE PEDIDOS VACÍOS AÑADIDO ▼▼▼ --%>
        <asp:Panel ID="pnlNoPedidos" runat="server" CssClass="no-pedidos-mensaje" Visible="false">
            Aún no tienes pedidos en tu historial.
        </asp:Panel>
        <%-- ▲▲▲ FIN DEL MENSAJE ▲▲▲ --%>

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