<%@ Page Title="" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Productos" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        /* Estilos específicos para la página de productos */
        .page-wrapper {
            width: 100%;
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .product-page-content {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            width: 100%;
            max-width: 960px;
        }

        /* Migas de pan */
        .breadcrumbs {
            padding: 16px;
            width: 100%;
            box-sizing: border-box;
            display: flex;
            gap: 8px;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            font-weight: 500;
        }
        .breadcrumbs a {
            color: #BAD0D9; /* Enlace inactivo/gris */
            text-decoration: none;
        }
        .breadcrumbs strong {
            color: #148C76; /* Enlace activo/verde */
        }

        /* Título y Descripción */
        .header-section {
            padding: 16px;
            width: 100%;
            display: flex;
            flex-direction: column;
            gap: 12px;
        }

        .category-title {
            margin: 0;
            font-family: 'ZCOOL XiaoWei', serif;
            font-weight: 400;
            font-size: 48px;
            line-height: 40px;
            color: #148C76;
            border-bottom: 0.5px solid #148C76;
            padding-bottom: 5px;
        }

        .category-description {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #000000;
        }

        /* Navegación por Pestañas/Categorías */
        .category-tabs {
            width: 100%;
            box-sizing: border-box;
            border-bottom: 1px solid #E3DEDE;
            padding: 0 16px 0 16px;
            margin-bottom: 12px; /* Espacio antes de la cuadrícula */
        }

        .category-tabs ul {
            display: flex;
            list-style: none;
            padding: 0;
            margin: 0;
            gap: 32px;
        }

        .tab-button {
            border: none;
            background: none;
            padding: 16px 0 13px 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 14px;
            line-height: 21px;
            color: #757575;
            cursor: pointer;
            border-bottom: 3px solid transparent;
            transition: border-bottom 0.3s;
        }

        .tab-button.active {
            color: #148C76;
            border-bottom-color: #148C76;
        }
        .tab-button:not(.active) {
            border-bottom-color: #E6E8EB; /* Línea gris de pestaña inactiva */
        }
        
        /* Cuadrícula de Productos */
        .products-grid-container {
            padding: 16px;
            width: 100%;
            box-sizing: border-box;
            display: flex;
            flex-wrap: wrap;
            gap: 12px; /* Espacio entre las tarjetas */
        }

        .product-card {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px 0px 12px;
            gap: 12px;
            width: calc(33.333% - 8px); /* Tamaño para 3 columnas (300px * 3 + 12px * 2 = 924px) */
            text-decoration: none;
            color: inherit;
        }

        .product-image-container {
            width: 100%;
            height: 235px;
            background-color: #f0f0f0;
            background-size: cover;
            border-radius: 12px;
            overflow: hidden;
        }

        .product-name {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            color: #171214;
        }

        .product-description {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #826670; /* Color de texto del diseño */
        }
    </style>

    <div class="page-wrapper">
        <div class="product-page-content">
            
            <div class="breadcrumbs">
                <a href="<%: ResolveUrl("~/Cliente/InicioCliente.aspx") %>">Inicio</a>
                /
                <strong>Productos</strong>
            </div>

            <div class="header-section">
                <h1 class="category-title">Productos</h1>
                <p class="category-description">
                    Explora nuestra gama de productos dermocosméticos diseñados para realzar tu belleza natural y promover una piel sana.
                </p>
            </div>
            
            <div class="category-tabs">
                <ul>
                    <li>
                        <asp:LinkButton ID="btnFaciales" runat="server" 
                                        CssClass="tab-button active"  
                                        CommandArgument="facial"
                                        OnClick="FilterProducts_Click">
                            Productos Faciales
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnCorporales" runat="server" 
                                        CssClass="tab-button" 
                                        CommandArgument="corporal"
                                        OnClick="FilterProducts_Click">
                            Productos Corporales
                        </asp:LinkButton>
                    </li>
                    </ul>
            </div>
            
            <div class="products-grid-container">
                
                <asp:Repeater ID="rpProductos" runat="server">
                    <ItemTemplate>
                        <a href='<%# ResolveUrl("~/Cliente/DetalleProducto.aspx?id=" + Eval("Id")) %>' class="product-card">
                            
                            <div class="product-image-container">
                                <asp:Image ID="imgProd" runat="server" 
                                           ImageUrl='<%# Eval("ImagenUrl") %>' 
                                           AlternateText='<%# Eval("Nombre") %>' 
                                           style="width:100%;height:100%;object-fit:cover;" />
                            </div>

                            <div class="product-details">
                                <h2 class="product-name"><%# Eval("Nombre") %></h2>
                                <p class="product-description"><%# Eval("DescripcionCorta") %></p>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
                
                </div>
        </div>
    </div>
</asp:Content>