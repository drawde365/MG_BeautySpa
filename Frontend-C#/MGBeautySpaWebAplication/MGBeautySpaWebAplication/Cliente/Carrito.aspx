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
    <%-- ▼▼▼ NUEVO SCRIPT PARA MODALES ▼▼▼ --%>
    <script type="text/javascript">
        var paymentModalInstance = null;
        var successModalInstance = null;

        document.addEventListener("DOMContentLoaded", function () {
            var paymentModalEl = document.getElementById('paymentModal');
            if (paymentModalEl) {
                paymentModalInstance = new bootstrap.Modal(paymentModalEl);
            }

            var successModalEl = document.getElementById('paymentSuccessModal');
            if (successModalEl) {
                successModalInstance = new bootstrap.Modal(successModalEl);
            }
        });

        // Función para mostrar el modal de éxito y ocultar el de pago
        function showSuccessModal() {
            if (paymentModalInstance) {
                paymentModalInstance.hide();
            }
            if (successModalInstance) {
                successModalInstance.show();
            }
        }

        // (Opcional) Función para mostrar el modal de pago
        // Tu botón "Proceder al Pago" (btnCheckout) debería llamar a esto desde C#
        function showPaymentModal() {
            if (paymentModalInstance) {
                paymentModalInstance.show();
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

                                        <div class="quantity-control quantity-picker">
                                            <button type="button" class="qty-button qty-btn-minus">-</button>
    
                                            <asp:TextBox ID="txtCantidad" runat="server" 
                                                CssClass="qty-display" 
                                                Text='<%# Eval("Cantidad") %>' 
                                                TextMode="Number" 
                                                min="0" />
    
                                            <button type="button" class="qty-button qty-btn-plus">+</button>
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
                            
                            <div class="d-flex justify-content-center mb-3">
                                <asp:Button ID="btnActualizarCarrito" runat="server" 
                                    Text="Actualizar Carrito" 
                                    CssClass="checkout-button btn-compact" 
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

            <div class="modal-content payment-modal-content">

                <div class="payment-modal-header">
                    <h5 class="payment-modal-title">Realiza fácil tus pagos con EzPay</h5>
                </div>

                <div class="payment-modal-body">

                    <div class="input-block">
                        <label for="txtCardNumber" class="input-label">Ingrese el número de la tarjeta</label>
                        <asp:TextBox ID="txtCardNumber" runat="server"
                            CssClass="input-field"
                            placeholder="XXXX XXXX XXXX XXXX"
                            TextMode="Number" />
                    </div>

                    <div class="input-block">
                        <label for="txtNameOnCard" class="input-label">Nombre y Apellido</label>
                        <asp:TextBox ID="txtNameOnCard" runat="server"
                            CssClass="input-field"
                            placeholder="Nombre en la tarjeta" />
                    </div>

                    <div class="row-2">
                        <div class="input-block block-half">
                            <label for="txtExpiryDate" class="input-label">Fecha de vencimiento</label>
                            <asp:TextBox ID="txtExpiryDate" runat="server"
                                CssClass="input-field"
                                placeholder="MM/AA" />
                        </div>

                        <div class="input-block block-half">
                            <label for="txtCVV" class="input-label">CVV</label>
                            <asp:TextBox ID="txtCVV" runat="server"
                                CssClass="input-field"
                                TextMode="Password"
                                MaxLength="4"
                                placeholder="123" />
                        </div>
                    </div>

                </div>

                <div class="payment-modal-footer">
                    <asp:Button ID="btnProcessPayment" runat="server"
                        Text="Aceptar"
                        CssClass="payment-button"
                        OnClick="btnProcessPayment_Click" />
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="paymentSuccessModal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content" style="border-radius: 12px; text-align: center; padding: 24px;">
                <div class="modal-body">
                
                    <%-- Icono de Check (opcional, pero recomendado) --%>
                    <svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#1EC3B6" class="bi bi-check-circle-fill" viewBox="0 0 16 16" style="margin-bottom: 16px;">
                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                    </svg>

                    <h4 style="font-family: 'Plus Jakarta Sans', sans-serif; font-weight: 700; color: #171214; margin-bottom: 12px;">
                        ¡Pago Realizado!
                    </h4>
                    <p style="font-family: 'Plus Jakarta Sans', sans-serif; color: #757575; margin-bottom: 24px;">
                        Tu pedido ha sido procesado exitosamente. Recibirás una confirmación por correo.
                    </p>
                
                    <asp:Button ID="btnVolverInicio" runat="server" 
                        Text="Volver al Inicio" 
                        CssClass="checkout-button" 
                        OnClick="btnVolverInicio_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>