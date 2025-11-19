<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="Servicios.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Servicios" %>

<asp:Content ID="HeadServicios" ContentPlaceHolderID="HeadContent" runat="server">
<!-- 1) CSS específico de la página -->

    <link href="<%: ResolveUrl("~/Content/ClienteCss/ServiciosClienteCss.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="BreadcrumbServicios" ContentPlaceHolderID="BreadcrumbContent" runat="server">
<!-- 2) Breadcrumb -->

    <li class="breadcrumb-item">
        <a href="<%: ResolveUrl("~/Cliente/InicioCliente.aspx") %>">Inicio</a>
    </li>
    <li class="breadcrumb-item active">Servicios</li>
</asp:Content>

<asp:Content ID="MainServicios" ContentPlaceHolderID="MainContent" runat="server">
<!-- 3) Contenido principal -->


    <div class="service-page-wrapper">
        <div class="service-page-content">

            <!-- Encabezado -->
            <header class="service-header">
                <h1 class="category-title">Servicios</h1>
                <p class="category-description">
                    Descubre una amplia gama de tratamientos personalizados para realzar tu belleza y bienestar.
                    Desde rejuvenecedores tratamientos faciales hasta relajantes terapias corporales, nuestro
                    equipo de expertos está comprometido a brindarte una experiencia excepcional.
                </p>
            </header>

            <!-- Pestañas (igual estilo que Productos) -->
            <nav class="category-tabs">
                <ul class="category-tabs-list">
                    <li>
                        <asp:LinkButton ID="btnFaciales" runat="server"
                            CssClass="tab-button"
                            CommandArgument="facial"
                            OnClick="FilterServices_Click">
                            Servicios Faciales
                        </asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnCorporales" runat="server"
                            CssClass="tab-button"
                            CommandArgument="corporal"
                            OnClick="FilterServices_Click">
                            Servicios Corporales
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
            </nav>

            <!-- Grid de servicios -->
            <section class="services-grid-container">
                <asp:Repeater ID="rpServicios" runat="server">
                    <ItemTemplate>
                        <a href='<%# ResolveUrl("~/Cliente/DetalleServicio.aspx?id=" + Eval("idServicio")) %>'
                           class="service-card">

                            <div class="service-image-container">
                                <asp:Image ID="imgServicio" runat="server"
                                           ImageUrl='<%# Eval("urlImagen") %>'
                                           AlternateText='<%# Eval("nombre") %>'
                                           CssClass="service-image" />
                            </div>

                            <div class="service-details">
                                <h2 class="service-name"><%# Eval("nombre") %></h2>
                                <p class="service-description"><%# Eval("descripcion") %></p>
                            </div>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </section>

        </div>
    </div>

</asp:Content>
