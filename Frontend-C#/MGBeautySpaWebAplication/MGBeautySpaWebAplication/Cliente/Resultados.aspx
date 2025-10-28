<%@ Page Title="Resultados" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="Resultados.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Resultados" %>
<asp:Content ID="ct2" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        /* CSS DE ESTA PÁGINA: Asegura que el Repeater se muestre en una cuadrícula */
        .products-grid-row {
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            padding: 0px;
            gap: 12px;
            width: 100%;
            flex-wrap: wrap; 
        }

        .product-card {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px 0px 12px;
            gap: 12px;
            width: 223px; /* Ancho de la tarjeta para 4 columnas en 960px */
            text-decoration: none;
            color: inherit;
        }

        .product-image-container {
            width: 100%;
            height: 223px;
            background-color: #f0f0f0;
            background-size: cover;
            border-radius: 12px;
            overflow: hidden;
        }
        
        .product-details {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            width: 100%;
        }

        .product-name {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            color: #1C0D12;
            width: 100%;
        }

        .product-description {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #994D66; /* Color de descripción en el diseño de Figma */
            width: 100%;
        }
    </style>

    <div class="results-header">
        <h1 class="results-title">Resultados para "<asp:Literal ID="litQuery" runat="server" />" 
            <small class="text-muted"><asp:Literal ID="litCount" runat="server" /></small>
        </h1>
    </div>

    <asp:Panel runat="server" ID="pnlNoResults" Visible="false" CssClass="alert alert-light border">
        No hay resultados para "<asp:Literal ID="Literal1" runat="server" />". Intenta con otra palabra (p. ej., <em>crema</em>).
    </asp:Panel>

    <div class="products-grid-container">
        <div class="products-grid-row">

            <asp:Repeater ID="rptProductos" runat="server">
                <ItemTemplate>
                    <a href='<%# ResolveUrl("~/Cliente/DetalleProducto.aspx?id=" + Eval("Id")) %>' class="product-card">
                        
                        <div class="product-image-container">
                            <img src='<%# Eval("ImagenUrl") %>' alt='<%# Eval("Nombre") %>' style="width:100%;height:100%;object-fit:cover;" />
                        </div>

                        <div class="product-details">
                            <h2 class="product-name"><%# Eval("Nombre") %></h2>
                            <p class="product-description">S/ <%# String.Format("{0:0.00}", Eval("Precio")) %></p> 
                        </div>
                    </a>
                </ItemTemplate>
                
                <SeparatorTemplate>
                    <%-- Este Template se puede dejar vacío --%>
                </SeparatorTemplate>
            </asp:Repeater>
            </div>
    </div>

</asp:Content>