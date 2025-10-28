<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="InicioCliente.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.InicioCliente" %>

<%-- 1. CONTENIDO DEL HEAD: Estilos y Fuentes --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/InicioClienteStyle.css") %>" />
    <link href="../Content/Fonts/css/all.min.css" rel="stylesheet" />
</asp:Content>


<%-- 2. CONTENIDO PRINCIPAL: HTML --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <%-- El <main class="container"> ya está en el MasterPage --%>

    <div class="content-wrapper">

        <section class="hero-section">
            <div class="hero-banner">
                <img src="<%: ResolveUrl("~/Content/images/Cliente/Bienvenida.jpg") %>" alt="MG Beauty Spa" style="height:100%">
                <div class="hero-text">
                    <h1>Bienvenido a MG Beauty Spa</h1>
                    <p>Descubre nuestra colección curada de productos dermocosméticos y servicios expertos diseñados para realzar tu belleza natural.</p>
                </div>
            </div>
        </section>

        <div class="section-title">
                <h2>Productos destacados</h2>
                <a href="<%: ResolveUrl("~/Cliente/Productos.aspx") %>">
                    <i class="fa-solid fa-angle-right" style="color: #000000;"></i>
                </a>
        </div>

        <section class="product-grid-container">
            <div class="product-grid">

                <article class="product-card">
                    <div class="product-card-image"></div>
                    <div class="product-card-body">
                        <h3>Suero hidratante</h3>
                        <p>Hidratación intensa para todo tipo de piel</p>
                    </div>
                </article>

                <article class="product-card">
                    <div class="product-card-image"></div>
                    <div class="product-card-body">
                        <h3>Crema antienvejecimiento</h3>
                        <p>Reduce arrugas y líneas finas</p>
                    </div>
                </article>

                <article class="product-card">
                    <div class="product-card-image"></div>
                    <div class="product-card-body">
                        <h3>Mascarilla iluminadora</h3>
                        <p>Unifica el tono y la textura de la piel</p>
                    </div>
                </article>

            </div>
        </section>

        <div class="section-title-wrapper">
            <div class="section-title">
                <h2>Nuestros servicios</h2>
                <a href="<%: ResolveUrl("~/Cliente/Servicios.aspx") %>" title="Ir a mi perfil">
                    <i class="fa-solid fa-angle-right" style="color: #000000;"></i>
                </a>
            </div>
        </div>

        <section class="services-grid-container">
            <div class="services-grid">
                
                <article class="service-card">
                    <div class="service-card-image"></div>
                    <div class="service-card-body">
                        <h3>Tratamientos faciales</h3>
                        <p>Faciales personalizados para una piel radiante</p>
                    </div>
                </article>
                
                <article class="service-card">
                    <div class="service-card-image"></div>
                    <div class="service-card-body">
                        <h3>Tratamientos corporales</h3>
                        <p>Terapias corporales relajantes y rejuvenecedoras</p>
                    </div>
                </article>

                <article class="service-card">
                    <div class="service-card-image"></div>
                    <div class="service-card-body">
                        <h3>Tratamientos láser</h3>
                        <p>Soluciones láser avanzadas para diversas preocupaciones de la piel</p>
                    </div>
                </article>

            </div>
        </section>

    </div>
    
</asp:Content>


<%-- 3. CONTENIDO DE SCRIPTS (Opcional) --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <%-- Puedes poner scripts específicos de esta página aquí --%>
</asp:Content>