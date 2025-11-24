<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="Productos.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Productos" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Productos | MG Beauty Spa
</asp:Content>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
<!-- CSS específico de Productos -->

    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/ClienteCss/ProductosClienteCss.css") %>" />
</asp:Content>

<asp:Content ID="Breadcrumb1" ContentPlaceHolderID="BreadcrumbContent" runat="server">
<!-- Breadcrumb -->

    <li class="breadcrumb-item"><a href="InicioCliente.aspx">Inicio</a></li>
    <li class="breadcrumb-item active">Productos</li>
</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
<!-- Contenido principal -->


    <div class="products-page-wrapper">
        <div class="product-page-content">

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
                        <a href='<%# ResolveUrl("~/Cliente/DetalleProducto.aspx?id=" + Eval("idProducto")) %>'
                           class="product-card">

                            <div class="product-image-container">
                                <asp:Image ID="imgProd" runat="server"
                                    ImageUrl='<%# Eval("urlImagen") %>'
                                    AlternateText='<%# Eval("nombre") %>'
                                    CssClass="product-image" />
                            </div>

                            <div class="product-details">
                                <h2 class="product-name"><%# Eval("nombre") %></h2>
                                <p class="product-description"><%# Eval("descripcion") %></p>
                                <p class="product-precio">S/ <%# Eval("precio") %></p>
                            </div>

                        </a>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
    </div>

</asp:Content>
