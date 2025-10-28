<%@ Page Title="Tu Carrito" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Carrito" %>

<asp:Content ID="ctScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:ScriptManager ID="smCarrito" runat="server" /> 

    <link rel="stylesheet" href="<%: ResolveUrl("CarritoCliente.css") %>" />
    
    <div class="cart-page-content">

        <nav class="breadcrumbs">
            <a href="<%: ResolveUrl("~/Cliente/InicioCliente.aspx") %>">Inicio</a> /
            <strong>Carrito</strong>
        </nav> 
        <div class="cart-header">
            <h1 class="cart-title">Tu Carrito</h1>
        </div>

        <asp:UpdatePanel ID="upCartContent" runat="server">
            <ContentTemplate>
                
                <div class="products-list-container">
                    <asp:Repeater ID="rpCartItems" runat="server">
                        <ItemTemplate>
                            <div class="product-item-card">
    
                                <div class="product-details-group">
                                    <img src='<%# Eval("ImagenUrl") %>' alt='<%# Eval("Nombre") %>' class="product-image-small" />
                                    <div class="product-text-group">
                                        <h2 class="item-name"><%# Eval("Nombre") %></h2>
                                        <p class="item-variant">Tamaño: <%# Eval("Tamano") %></p>
                                        <p class="item-variant">Tipo de piel: <%# Eval("TipoPiel") %></p>
                                    </div>
                                </div>

                                <div class="quantity-control">
                                    <asp:LinkButton ID="btnDecrement" runat="server" CssClass="qty-button" Text="-" CommandName="Decrement" CommandArgument='<%# Eval("ProductId") + "|" + Eval("TipoPiel") %>' OnClick="Quantity_Click" />
                                    <span class="qty-display"><%# Eval("Cantidad") %></span>
                                    <asp:LinkButton ID="btnIncrement" runat="server" CssClass="qty-button" Text="+" CommandName="Increment" CommandArgument='<%# Eval("ProductId") + "|" + Eval("TipoPiel") %>' OnClick="Quantity_Click" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div class="summary-detail-container">
                    <h2 class="summary-title">Resumen del Pedido</h2>
    
                    <div class="summary-row">
                        <span class="summary-label">Subtotal</span>
                        <span class="summary-value">S/ <asp:Literal ID="litSubtotal" runat="server"></asp:Literal></span>
                    </div>
    
                    <div class="summary-row">
                        <span class="summary-label">IGV</span>
                        <span class="summary-value">S/ <asp:Literal ID="litImpuestos" runat="server"></asp:Literal></span>
                    </div>
    
                    <div class="summary-row total-value-row">
                        <span class="summary-label">Total</span>
                        <span class="summary-value">S/ <asp:Literal ID="litTotal" runat="server"></asp:Literal></span>
                    </div>
                </div>

                <div class="checkout-button-wrapper">
                    <asp:Button ID="btnCheckout" runat="server" Text="Proceder al Pago" CssClass="checkout-button" OnClick="btnCheckout_Click" />
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <div class="modal fade" id="paymentModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content payment-interface">
                
                <div class="modal-header-custom">
                    <h5 class="modal-title-custom">Realiza fácil tus pagos con EzPay</h5>
                </div>

                <div class="payment-body">
                    
                    <div class="input-group-field">
                        <label for="txtCardNumber" class="input-label">Ingrese el número de la tarjeta</label>
                        <asp:TextBox ID="txtCardNumber" runat="server" 
                                     CssClass="input-text-style" 
                                     placeholder="XXXX XXXX XXXX XXXX" 
                                     TextMode="Number" />
                    </div>

                    <div class="input-group-field">
                        <label for="txtNameOnCard" class="input-label">Nombre y Apellido</label>
                        <asp:TextBox ID="txtNameOnCard" runat="server" 
                                     CssClass="input-text-style" 
                                     placeholder="Nombre en la tarjeta" />
                    </div>

                    <div class="input-group-row">
                        <div class="input-group-field half-width">
                            <label for="txtExpiryDate" class="input-label">Fecha de vencimiento</label>
                            <asp:TextBox ID="txtExpiryDate" runat="server" 
                                         CssClass="input-text-style" 
                                         placeholder="MM/AA" />
                        </div>

                        <div class="input-group-field half-width">
                            <label for="txtCVV" class="input-label">CVV</label>
                            <asp:TextBox ID="txtCVV" runat="server" 
                                         CssClass="input-text-style" 
                                         TextMode="Password" 
                                         MaxLength="4" 
                                         placeholder="123" />
                        </div>
                    </div>
                    
                </div>
                
                <div class="modal-footer-custom">
                    <asp:Button ID="btnProcessPayment" runat="server" 
                                Text="Aceptar" 
                                CssClass="payment-button"
                                OnClick="btnProcessPayment_Click" />
                </div>
                
                </div>
        </div>
    </div>
</asp:Content>