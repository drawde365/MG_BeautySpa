<%@ Page Title="" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Servicios.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Servicios" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        /* Reutiliza estilos de productos */
        .page-wrapper {
            width: 100%;
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .service-page-content {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            width: 100%;
            max-width: 960px;
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

        /* Navegación por Categorías/Pestañas */
        .category-tabs {
            width: 100%;
            box-sizing: border-box;
            border-bottom: 1px solid #E3DEDE;
            padding: 0 16px 0 16px;
            margin-bottom: 12px;
        }

        .category-tabs ul {
            display: flex;
            list-style: none;
            padding: 0;
            margin: 0;
            gap: 32px;
        }

        /* Estilo para los botones de pestañas (debes aplicar estas clases con JS o C#) */
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
            border-bottom-color: #E6E8EB;
        }
        
        /* Cuadrícula de Servicios (3 columnas) */
        .services-grid-container {
            padding: 16px;
            width: 100%;
            box-sizing: border-box;
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
        }

        .service-card {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px 0px 12px;
            gap: 12px;
            width: calc(33.333% - 8px); /* Tamaño para 3 columnas */
            text-decoration: none;
            color: inherit;
        }

        .service-image-container {
            width: 100%;
            height: 301px; /* La imagen es un cuadrado grande */
            background-color: #f0f0f0;
            background-size: cover;
            border-radius: 12px;
            overflow: hidden;
        }

        .service-name {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            color: #171214;
        }

        .service-description {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #826670;
        }
    </style>

    <div class="page-wrapper">
        <div class="service-page-content">
            
            <div class="header-section">
                <h1 class="category-title">Servicios</h1>
                <p class="category-description">
                    Descubre una amplia gama de tratamientos personalizados para realzar tu belleza y bienestar. Desde rejuvenecedores tratamientos faciales hasta relajantes terapias corporales, nuestro equipo de expertos está comprometido a brindarte una experiencia excepcional.
                </p>
            </div>
            <div class="category-tabs">
                <ul>
                    <li>
                        <asp:LinkButton ID="btnFaciales" runat="server" 
                                        CssClass="tab-button active" 
                                        CommandArgument="facial"
                                        OnClick="FilterServices_Click">
                            Faciales
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnCorporales" runat="server" 
                                        CssClass="tab-button" 
                                        CommandArgument="corporal"
                                        OnClick="FilterServices_Click">
                            Corporales
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnTerapias" runat="server" 
                                        CssClass="tab-button" 
                                        CommandArgument="terapias"
                                        OnClick="FilterServices_Click">
                            Terapias Complementarias
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
            
            <div class="services-grid-container">
                
                <asp:Repeater ID="rpServicios" runat="server">
                    <ItemTemplate>
                        <a href='<%# ResolveUrl("~/Cliente/DetalleServicio.aspx?id=" + Eval("Id")) %>' class="service-card">
                            
                            <div class="service-image-container">
                                <asp:Image ID="imgServicio" runat="server" 
                                           ImageUrl='<%# Eval("ImagenUrl") %>' 
                                           AlternateText='<%# Eval("Nombre") %>' 
                                           style="width:100%;height:100%;object-fit:cover;" />
                            </div>

                            <div class="product-details">
                                <h2 class="service-name"><%# Eval("Nombre") %></h2>
                                <p class="service-description"><%# Eval("DescripcionCorta") %></p>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
                
            </div>
        </div>
    </div>
</asp:Content>