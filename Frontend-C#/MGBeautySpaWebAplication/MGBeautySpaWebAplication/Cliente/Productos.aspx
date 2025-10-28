<%@ Page Title="" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Productos" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="ProductoClienteStyle.css" />

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