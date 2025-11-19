<%@ Page Title="Detalle del Pedido"
Language="C#"
MasterPageFile="~/Cliente/Perfil/Perfil.Master"
AutoEventWireup="true"
CodeBehind="DetallePedido.aspx.cs"
Inherits="MGBeautySpaWebAplication.Cliente.Perfil.DetallePedido" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <link rel="stylesheet" href="DetallePedido.css" />

    <div class="detalle-container">
        <h2 class="detalle-numero">Pedido N.º <asp:Literal ID="litNumeroPedido" runat="server" /></h2>
        <p class="detalle-fecha">Fecha: <asp:Literal ID="litFecha" runat="server" /></p>

        <div class="productos-lista">
            <asp:Repeater ID="rptProductos" runat="server">
                <ItemTemplate>
                    <div class="producto-item">
                        <img src='<%# Eval("Imagen") %>' alt="Producto" class="producto-img" />
                        <div class="producto-info">
                            <p class="producto-nombre"><%# Eval("Nombre") %></p>
                            <p class="producto-desc"><%# Eval("Descripcion") %></p>
                        </div>
                        <div class="producto-precio">
                            <p>Cant: <%# Eval("Cantidad") %></p>
                            <p>Subtotal: S/ <%# Eval("Subtotal") %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <hr class="linea-total" />

        <div class="detalle-total">
            <span>Total:</span>
            <strong>S/ <asp:Literal ID="litTotal" runat="server" /></strong>
        </div>

        <div class="detalle-footer">
            <asp:Button ID="btnRegresar" runat="server"
                        Text="Regresar"
                        CssClass="btn-regresar"
                        OnClick="btnRegresar_Click" />
        </div>
    </div>

</asp:Content>