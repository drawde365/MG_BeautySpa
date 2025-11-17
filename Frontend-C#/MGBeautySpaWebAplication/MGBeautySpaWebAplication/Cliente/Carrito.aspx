<%@ Page Title="Tu Carrito" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Carrito" %>

<asp:Content ID="ctHead" ContentPlaceHolderID="HeadContent" runat="server">
    <!--
    <link href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" rel="stylesheet" /> 
    -->
    <link rel="stylesheet" href="<%: ResolveUrl("~/Cliente/CarritoCliente.css?v=3") %>" />
    
    <%-- Estilo simple para el mensaje de carrito vacío --%>
    <style>
        .carrito-vacio-mensaje {
            text-align: center;
            padding: 60px 20px;
            background-color: #fff;
            border-radius: 12px;
            border: 1px solid #E3D4D9;
            margin: 0 16px 24px 16px;
        }
        .carrito-vacio-mensaje h3 {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            color: #171214;
        }
        .carrito-vacio-mensaje p {
            font-family: 'Plus Jakarta Sans', sans-serif;
            color: #757575;
            margin-bottom: 24px;
        }
        .btn-ver-productos {
            display: inline-block;
            background-color: #1EC3B6; /* Color primario */
            color: #fff;
            padding: 12px 24px;
            text-decoration: none;
            border-radius: 8px;
            font-weight: 700;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }
    </style>
</asp:Content>

<asp:Content ID="ctScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <!--
    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
    -->
    
    <script type="text/javascript">
        function changeQty(button, amount) {
            var parent = button.parentNode;
            var txtQty = parent.querySelector('.qty-display');

            if (txtQty) {
                var currentQty = parseInt(txtQty.value) || 0;
                var newQty = currentQty + amount;

                if (newQty < 0) {
                    newQty = 0;
                }

                txtQty.value = newQty;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:ScriptManager ID="smCarrito" runat="server" /> 

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
                
                <%-- ▼▼▼ PANEL PARA CARRITO VACÍO (OCULTO POR DEFECTO) ▼▼▼ --%>
                <asp:Panel ID="pnlCarritoVacio" runat="server" Visible="false" CssClass="carrito-vacio-mensaje">
                    <h3>Tu carrito está vacío</h3>
                    <p>Parece que aún no has añadido ningún producto. ¡Explora nuestro catálogo!</p>
                    <a href="<%: ResolveUrl("~/Cliente/Productos.aspx") %>" class="btn-ver-productos">Ver Productos</a>
                </asp:Panel>
                
                <%-- ▼▼▼ PANEL PARA CARRITO LLENO (VISIBLE POR DEFECTO) ▼▼▼ --%>
                <asp:Panel ID="pnlCarritoLleno" runat="server" Visible="true">
                    <div class="cart-layout-wrapper">
                        
                        <div class="cart-products-section">
                            <div class="cart-products-header">
                                <span class="cart-vendor-info">Vendido y despachado por <strong>MG Beauty Spa</strong></span>
                                <label class="cart-select-all">
                                    <span>Tus Productos</span>
                                </label>
                            </div>
                            
                            <asp:Repeater ID="rpCartItems" runat="server" OnItemCommand="btnEliminarItem_Click">
                                <ItemTemplate>
                                    <div class="product-item-card">
                                        
                                        <div class="product-details-group">
                                            <asp:HiddenField ID="hdnItemKey" runat="server" 
                                                Value='<%# Eval("ProductId") + "|" + Eval("TipoPiel") %>' />
                                            
                                            <img src='<%# Eval("ImageUrl") %>' 
                                                 alt='<%# Eval("Nombre") %>' 
                                                 class="product-image-small" />
                                            <div class="product-text-group">
                                                <h2 class="item-name"><%# Eval("Nombre") %></h2>
                                                <p class="item-variant">Tamaño: <%# Eval("Tamano") %></p>
                                                <p class="item-variant">Tipo de piel: <%# Eval("TipoPiel") %></p>
                                                <span class="item-unit-price">S/ <%# Eval("PrecioUnitario", "{0:N2}") %></span>
                                            </div>
                                        </div>

                                        <div class="quantity-control">
                                            <button type="button" class="qty-button" onclick="changeQty(this, -1)">-</button>
                                            
                                            <asp:TextBox ID="txtCantidad" runat="server" 
                                                CssClass="qty-display" 
                                                Text='<%# Eval("Cantidad") %>' 
                                                TextMode="Number" 
                                                min="0" />
                                            
                                            <button type="button" class="qty-button" onclick="changeQty(this, 1)">+</button>
                                        </div>

                                        <asp:LinkButton ID="btnEliminarItem" runat="server"
                                            CssClass="qty-button-remove"
                                            Text="&times;"
                                            ToolTip="Eliminar"
                                            CommandName="Remove"
                                            CommandArgument='<%# Eval("ProductId") + "|" + Eval("TipoPiel") %>'
                                            OnClick="btnEliminarItem_Click" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <div class="products-separator"></div>
                            
                            <div class="d-flex justify-content-end mb-3">
                                <asp:Button ID="btnActualizarCarrito" runat="server" 
                                    Text="Actualizar Carrito" 
                                    CssClass="checkout-button" 
                                    OnClick="btnActualizarCarrito_Click" />
                            </div>
                            
                        </div>

                        <div class="cart-summary-section">
                            
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
                                
                                <hr class="summary-divider" />
                                
                                <div class="summary-row total-value-row">
                                    <span class="summary-label total-label">Total</span>
                                    <span class="summary-value total-amount">S/ <asp:Literal ID="litTotal" runat="server"></asp:Literal></span>
                                </div>
                            </div>

                            <div class="checkout-button-wrapper">
                                <a href="<%: ResolveUrl("~/Cliente/Productos.aspx") %>" class="btn-back">
                                    Seguir Comprando
                                </a>
                                <asp:Button ID="btnCheckout" runat="server" 
                                    Text="Proceder al Pago" 
                                    CssClass="checkout-button" 
                                    OnClick="btnCheckout_Click" />
                            </div>
                            
                        </div>
                        
                    </div>
                </asp:Panel>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div> 

    <%-- El Modal de Pago --%>
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