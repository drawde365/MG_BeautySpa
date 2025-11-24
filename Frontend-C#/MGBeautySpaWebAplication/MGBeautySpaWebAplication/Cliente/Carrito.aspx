<%@ Page Title="Tu Carrito" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Carrito" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Carrito | MG Beauty Spa
</asp:Content>

<asp:Content ID="ctHead" ContentPlaceHolderID="HeadContent" runat="server">
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
            background-color: #148C76; /* Color primario */
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
                                    Text="Recalcular Resumen de Pedido" 
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

<%-- =====================================================
     MODAL DE PAGO CON VALIDACIONES - MG BEAUTY SPA
     ===================================================== --%>
<div class="modal fade" id="paymentModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content payment-modal-content">

            <%-- Header del Modal --%>
            <div class="payment-modal-header">
                <h5 class="payment-modal-title">Finalizar Compra</h5>
                <p class="payment-modal-subtitle">Ingresa los datos de tu tarjeta de forma segura</p>
            </div>

            <%-- Body del Modal --%>
            <div class="payment-modal-body">

                <%-- Mensaje de Error --%>
                <div id="paymentErrorMessage" class="alert alert-danger" style="display: none; margin-bottom: 20px; padding: 12px; border-radius: 8px; font-size: 14px;">
                    <strong>Error:</strong> <span id="errorText"></span>
                </div>

                <%-- Número de Tarjeta --%>
                <div class="input-block">
                    <label for="txtCardNumber" class="input-label">
                        Número de tarjeta <span style="color: #C31E1E;">*</span>
                    </label>
                    <asp:TextBox ID="txtCardNumber" runat="server"
                        CssClass="input-field"
                        placeholder="1234 5678 9012 3456"
                        TextMode="SingleLine"
                        MaxLength="19"
                        onkeyup="formatCardNumber(this)"
                        onkeypress="return isNumberKey(event)" />
                </div>

                <%-- Nombre en la Tarjeta --%>
                <div class="input-block">
                    <label for="txtNameOnCard" class="input-label">
                        Titular de la tarjeta <span style="color: #C31E1E;">*</span>
                    </label>
                    <asp:TextBox ID="txtNameOnCard" runat="server"
                        CssClass="input-field"
                        placeholder="Ingrese su nombre completo"
                        onkeypress="return isLetterKey(event)" />
                </div>

                <%-- Fecha y CVV (lado a lado) --%>
                <div class="row-2-inputs">
                    <div class="input-block input-half">
                        <label for="txtExpiryDate" class="input-label">
                            Vencimiento <span style="color: #C31E1E;">*</span>
                        </label>
                        <asp:TextBox ID="txtExpiryDate" runat="server"
                            CssClass="input-field"
                            placeholder="MM/AA"
                            MaxLength="5"
                            onkeyup="formatExpiryDate(this)"
                            onkeypress="return isNumberKey(event)" />
                    </div>

                    <div class="input-block input-half">
                        <label for="txtCVV" class="input-label">
                            CVV <span style="color: #C31E1E;">*</span>
                        </label>
                        <asp:TextBox ID="txtCVV" runat="server"
                            CssClass="input-field"
                            TextMode="Password"
                            MaxLength="3"
                            placeholder="123"
                            onkeypress="return isNumberKey(event)" />
                    </div>
                </div>

            </div>

            <%-- Footer del Modal --%>
            <div class="payment-modal-footer">
                <button type="button" class="btn-cancel-payment" data-bs-dismiss="modal">
                    Cancelar
                </button>
                <asp:Button ID="btnProcessPayment" runat="server"
                    Text="Confirmar Pago"
                    CssClass="btn-confirm-payment"
                    OnClientClick="return validatePaymentForm();"
                    OnClick="btnProcessPayment_Click" />
            </div>

        </div>
    </div>
</div>

<%-- Script de Validaciones --%>
<script type="text/javascript">
    // Función para validar todo el formulario antes de enviar
    function validatePaymentForm() {
        // Obtener valores
        var cardNumber = document.getElementById('<%= txtCardNumber.ClientID %>').value.replace(/\s/g, '');
        var cardName = document.getElementById('<%= txtNameOnCard.ClientID %>').value.trim();
        var expiryDate = document.getElementById('<%= txtExpiryDate.ClientID %>').value.trim();
        var cvv = document.getElementById('<%= txtCVV.ClientID %>').value.trim();

        // Validar que todos los campos estén llenos
        if (!cardNumber || !cardName || !expiryDate || !cvv) {
            showError('Todos los campos son obligatorios');
            return false;
        }

        // Validar número de tarjeta (16 dígitos)
        if (cardNumber.length !== 16 || !/^\d+$/.test(cardNumber)) {
            showError('El número de tarjeta debe tener exactamente 16 dígitos');
            return false;
        }

        // Validar nombre (solo letras y espacios)
        if (!/^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/.test(cardName)) {
            showError('El nombre del titular solo puede contener letras');
            return false;
        }

        // Validar CVV (3 dígitos)
        if (cvv.length !== 3 || !/^\d+$/.test(cvv)) {
            showError('El CVV debe tener exactamente 3 dígitos');
            return false;
        }

        // Validar formato de fecha (MM/AA)
        if (!/^\d{2}\/\d{2}$/.test(expiryDate)) {
            showError('La fecha debe tener el formato MM/AA');
            return false;
        }

        // Validar que la fecha sea válida y no esté vencida
        var parts = expiryDate.split('/');
        var month = parseInt(parts[0], 10);
        var year = parseInt('20' + parts[1], 10);

        if (month < 1 || month > 12) {
            showError('El mes debe estar entre 01 y 12');
            return false;
        }

        var today = new Date();
        var currentYear = today.getFullYear();
        var currentMonth = today.getMonth() + 1;

        if (year < currentYear || (year === currentYear && month < currentMonth)) {
            showError('La tarjeta está vencida');
            return false;
        }

        // Si todo está bien, ocultar mensaje de error
        hideError();
        return true;
    }

    // Mostrar mensaje de error
    function showError(message) {
        var errorDiv = document.getElementById('paymentErrorMessage');
        var errorText = document.getElementById('errorText');
        errorText.textContent = message;
        errorDiv.style.display = 'block';

        // Scroll al mensaje de error
        errorDiv.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }

    // Ocultar mensaje de error
    function hideError() {
        var errorDiv = document.getElementById('paymentErrorMessage');
        errorDiv.style.display = 'none';
    }

    // Formatear número de tarjeta (añade espacios cada 4 dígitos)
    function formatCardNumber(input) {
        var value = input.value.replace(/\s/g, '');
        var formattedValue = value.match(/.{1,4}/g)?.join(' ') || value;
        input.value = formattedValue;
    }

    // Formatear fecha de vencimiento (añade / automáticamente)
    function formatExpiryDate(input) {
        var value = input.value.replace(/\D/g, '');
        if (value.length >= 2) {
            input.value = value.substring(0, 2) + '/' + value.substring(2, 4);
        } else {
            input.value = value;
        }
    }

    // Solo permitir números
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    // Solo permitir letras y espacios
    function isLetterKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        // Permitir espacios, letras mayúsculas, minúsculas y caracteres con tilde
        if ((charCode >= 65 && charCode <= 90) || // A-Z
            (charCode >= 97 && charCode <= 122) || // a-z
            charCode === 32 || // espacio
            charCode === 209 || charCode === 241 || // Ñ, ñ
            (charCode >= 192 && charCode <= 255)) { // caracteres con tilde
            return true;
        }
        return false;
    }

    // Limpiar errores cuando el usuario empieza a escribir
    document.addEventListener('DOMContentLoaded', function () {
        var inputs = document.querySelectorAll('.input-field');
        inputs.forEach(function (input) {
            input.addEventListener('input', function () {
                hideError();
            });
        });
    });
</script>

<%-- =====================================================
     MODAL DE ÉXITO - ESTILO MG BEAUTY SPA
     ===================================================== --%>
<div class="modal fade" id="paymentSuccessModal" tabindex="-1" aria-hidden="true" 
     data-bs-backdrop="static" data-bs-keyboard="false">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content success-modal-content">
            <div class="modal-body success-modal-body">
             
                <%-- Icono de Éxito --%>
                <div class="success-icon-wrapper">
                    <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" fill="currentColor" 
                         class="success-icon" viewBox="0 0 16 16">
                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                    </svg>
                </div>

                <%-- Mensaje de Éxito --%>
                <h3 class="success-title">¡Pago Exitoso!</h3>
                <p class="success-message">
                    Tu pedido ha sido procesado correctamente.<br />
                    Recibirás un correo con los datos de tu compra.
                </p>
            
                <%-- Botón de Acción --%>
                <asp:Button ID="btnVolverInicio" runat="server" 
                    Text="Volver al Inicio" 
                    CssClass="btn-success-action" 
                    OnClick="btnVolverInicio_Click" />
            </div>
        </div>
    </div>
</div>
</asp:Content>