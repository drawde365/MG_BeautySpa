<%@ Page Title="" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Carrito" %>
<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        .cart-page-content {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            width: 100%;
            max-width: 960px;
        }

        /* Título del Carrito */
        .cart-header {
            padding: 16px;
            width: 100%;
            box-sizing: border-box;
        }
        .cart-title {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 32px;
            line-height: 40px;
            color: #171214;
        }

        /* Contenedor de la lista de productos */
        .products-list-container {
            padding: 0 16px;
            width: 100%;
            box-sizing: border-box;
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        /* Tarjeta de Producto Individual (dentro del Repeater) */
        .product-item-card {
            display: flex;
            flex-direction: row;
            align-items: center;
            padding: 8px 16px;
            width: 100%;
            min-height: 82px;
            background: #FFFFFF;
            border-radius: 32px;
            box-sizing: border-box;
            justify-content: space-between;
        }

        .product-details-group {
            display: flex;
            flex-direction: row;
            align-items: center;
            gap: 16px;
        }

        .product-image-small {
            width: 56px;
            height: 56px;
            border-radius: 8px;
            object-fit: cover;
        }

        .product-text-group {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: flex-start;
        }
        
        .item-name {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            color: #171214;
            margin: 0;
        }

        .item-variant {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #826670;
            margin: 0;
        }

        /* Control de Cantidad */
        .quantity-control {
            display: flex;
            flex-direction: row;
            align-items: center;
            gap: 8px;
        }

        .qty-button {
            display: flex;
            justify-content: center;
            align-items: center;
            width: 28px;
            height: 28px;
            background: #F5F2F2;
            border-radius: 14px;
            border: none;
            cursor: pointer;
            font-weight: 500;
            font-size: 16px;
            color: #171214;
            line-height: 1;
        }
        
        .qty-display {
            width: 32px;
            text-align: center;
            font-weight: 500;
            font-size: 16px;
            color: #171214;
        }

        /* Resumen de Pedido */
        .summary-title {
            padding: 16px 16px 8px;
            width: 100%;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 24px;
            line-height: 23px;
            color: #171214;
            margin: 0;
        }

        .summary-detail-container {
            padding: 0 16px;
            width: 100%;
            box-sizing: border-box;
        }

        .summary-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 8px 0;
            width: 100%;
        }

        .summary-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 20px;
            line-height: 21px;
            color: #148C76;
            margin: 0;
        }
        
        .summary-value {
            display: flex;
            align-items: center;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 20px;
            line-height: 21px;
            color: #171214;
        }
        
        .total-value-row {
            padding-top: 16px;
            border-top: 1px solid #148C76; /* Línea separadora */
        }

        /* Botón de Pago */
        .checkout-button-wrapper {
            padding: 12px 16px;
            width: 100%;
            box-sizing: border-box;
            display: flex;
            justify-content: flex-end;
        }

        .checkout-button {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 0 20px;
            width: 172px;
            height: 48px;
            background: #1EC3B6;
            border-radius: 24px;
            border: none;
            cursor: pointer;
            
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 16px;
            line-height: 24px;
            color: #FFFFFF;
        }
    </style>

    <div class="cart-page-content">

        <nav class="breadcrumbs">
            <a href="<%: ResolveUrl("~/Cliente/InicioCliente.aspx") %>">Inicio</a> /
            <strong>Carrito</strong>
        </nav>

        <div class="cart-header">
            <h1 class="cart-title">Tu Carrito</h1>
        </div>

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
                            <button type="button" class="qty-button" data-action="minus" data-id="<%# Eval("Id") %>">-</button>
                            
                            <span class="qty-display"><%# Eval("Cantidad") %></span>
                            
                            <button type="button" class="qty-button" data-action="plus" data-id="<%# Eval("Id") %>">+</button>
                            
                            </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="summary-detail-container">
            <h2 class="summary-title">Resumen del Pedido</h2>
            
            <div class="summary-row">
                <span class="summary-label">Subtotal</span>
                <span class="summary-value">S/ <asp:Literal ID="litSubtotal" runat="server">105.00</asp:Literal></span>
            </div>
            
            <div class="summary-row">
                <span class="summary-label">Envío</span>
                <span class="summary-value">S/ <asp:Literal ID="litEnvio" runat="server">7.50</asp:Literal></span>
            </div>
            
            <div class="summary-row">
                <span class="summary-label">Impuestos</span>
                <span class="summary-value">S/ <asp:Literal ID="litImpuestos" runat="server">18.90</asp:Literal></span>
            </div>
            
            <div class="summary-row total-value-row">
                <span class="summary-label">Total</span>
                <span class="summary-value">S/ <asp:Literal ID="litTotal" runat="server">123.90</asp:Literal></span>
            </div>
        </div>

        <div class="checkout-button-wrapper">
            <asp:Button ID="btnCheckout" runat="server" Text="Proceder al Pago" CssClass="checkout-button" OnClick="btnCheckout_Click" />
        </div>

    </div>
</asp:Content>