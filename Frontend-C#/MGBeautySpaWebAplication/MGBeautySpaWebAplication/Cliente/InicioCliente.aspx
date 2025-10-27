<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="InicioCliente.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.InicioCliente" %>

<%-- 1. CONTENIDO DEL HEAD: Estilos y Fuentes --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">

    <style>
        /* --- Estilos basados en tu diseño --- */

        /* Depth 3, Frame 0 (Columna de contenido) 
           Se omite el ancho; el ".container" del MasterPage lo maneja.
        */
        .content-wrapper {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            flex-grow: 1; 
        }

        /* Depth 5, Frame 0 (Sección Hero) */
        .hero-section {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 16px;
            
            width: 960px; /* Ancho del diseño original */
            height: 512px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 0;
            
            /* Ajuste para centrarlo si el container es más ancho */
            margin-left: auto;
            margin-right: auto;
        }

        /* Depth 6, Frame 0 (Banner de Hero) */
        .hero-banner {
            width: 928px;
            height: 480px;
            min-height: 480px;
            
            /* TODO: Reemplaza esta URL de marcador de posición */
            background: linear-gradient(90deg, rgba(0, 0, 0, 0.1) 0%, rgba(0, 0, 0, 0.4) 100%), url(https://via.placeholder.com/928x480.png);
            background-size: cover;
            background-position: center;
            border-radius: 12px;
            
            position: relative; /* Necesario para el texto absoluto */
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 0;
        }

        /* Depth 7, Frame 0 (Contenedor de texto de Hero) */
        .hero-text {
            /* Auto layout */
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            gap: 8px;
            
            /* Posicionamiento absoluto */
            position: absolute;
            width: 695px;
            height: 116px;
            left: 216px; 
            top: 345px;
        }

        /* Depth 8, Frame 0 (Título) */
        .hero-text h1 {
            margin: 0;
            width: 695px;
            height: 60px;
            
            font-family: 'ZCOOL XiaoWei';
            font-style: normal;
            font-weight: 400;
            font-size: 48px;
            line-height: 60px;
            text-align: right;
            letter-spacing: -2px;
            color: #FFFFFF;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            flex-grow: 0;
        }
        
        /* Depth 8, Frame 1 (Subtítulo) */
        .hero-text p {
            margin: 0;
            width: 695px;
            height: 48px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            text-align: right;
            color: #FFFFFF;
            
            /* Inside auto layout */
            flex: none;
            order: 0; 
            align-self: stretch;
            flex-grow: 0;
        }

        /* Frame 20 (Título de sección "Productos") */
        .section-title {
            display: flex;
            flex-direction: row;
            align-items: center;
            padding: 6px 16px;
            gap: 10px;
            
            width: 306px;
            height: 40px;
            
            /* Inside auto layout */
            flex: none;
            order: 1; 
            flex-grow: 0;
        }
        
        .section-title h2 {
            margin: 0;
            width: 254px;
            height: 28px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 22px;
            line-height: 28px;
            color: #171214;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            flex-grow: 0;
        }
        
        .section-title .icon {
            width: 18px;
            height: 10px;
            background: #1E1E1E;
            transform: rotate(-90deg);
            
            /* Inside auto layout */
            flex: none;
            order: 1;
            flex-grow: 0;
        }

        /* Depth 4, Frame 2 (Contenedor Grid Productos) */
        .product-grid-container {
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            padding: 0px;
            
            width: 960px;
            height: 344px;
            
            /* Inside auto layout */
            flex: none;
            order: 2;
            align-self: stretch;
            flex-grow: 0;
            
            /* Ajuste para centrarlo si el container es más ancho */
            margin-left: auto;
            margin-right: auto;
        }

        /* Depth 5, Frame 0 (Grid Productos) */
        .product-grid {
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            padding: 16px;
            gap: 12px;
            
            width: 960px;
            height: 344px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            flex-grow: 0;
        }

        /* Depth 6, Frames 0, 1, 2 (Tarjeta de Producto) */
        .product-card {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            gap: 16px;
            
            width: 301.33px;
            min-width: 240px;
            height: 312px;
            border-radius: 8px;
            
            /* Inside auto layout */
            flex: none;
            order: 0; 
            align-self: stretch;
            flex-grow: 1;
        }

        /* Depth 7, Frame 0 (Imagen de Producto) */
        .product-card-image {
            width: 301.33px;
            height: 251px;
            
            /* TODO: Reemplaza esta URL de marcador de posición */
            background: url(https://via.placeholder.com/301x251.png);
            background-size: cover;
            border-radius: 12px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 0;
        }

        /* Depth 7, Frame 1 (Cuerpo de Tarjeta) */
        .product-card-body {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            
            width: 301.33px;
            height: 45px;
            
            /* Inside auto layout */
            flex: none;
            order: 1;
            align-self: stretch;
            flex-grow: 0;
        }
        
        /* Depth 8, Frame 0 (Título de Producto) */
        .product-card-body h3 {
            margin: 0;
            width: 301.33px;
            height: 24px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 600;
            font-size: 16px;
            line-height: 24px;
            color: #000000;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 0;
        }
        
        /* Depth 8, Frame 1 (Descripción de Producto) */
        .product-card-body p {
            margin: 0;
            width: 301.33px;
            height: 21px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #757575;
            
            /* Inside auto layout */
            flex: none;
            order: 1;
            align-self: stretch;
            flex-grow: 0;
        }
        
        /* Título de sección "Servicios" */
        /* Depth 4, Frame 3 */
        .section-title-wrapper {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 20px 16px 12px;
            
            width: 960px;
            height: 60px;
            
            /* Inside auto layout */
            flex: none;
            order: 3;
            align-self: stretch;
            flex-grow: 0;
            
            /* Ajuste para centrarlo si el container es más ancho */
            margin-left: auto;
            margin-right: auto;
        }
        
        /* Frame 17 */
        .section-title-services {
            display: flex;
            flex-direction: row;
            align-items: center;
            padding: 0px;
            gap: 10px;
            
            width: 219.77px;
            height: 28px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            flex-grow: 0;
        }
        
        .section-title-services h2 {
            margin: 0;
            width: 200.1px;
            height: 28px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 22px;
            line-height: 28px;
            color: #171214;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            flex-grow: 0;
        }

        .section-title-services .icon {
            width: 18px;
            height: 9.67px;
            background: #1E1E1E;
            transform: rotate(-90deg);
            
            /* Inside auto layout */
            flex: none;
            order: 1;
            flex-grow: 0;
        }
        
        /* Grid de Servicios */
        /* Depth 4, Frame 4 */
        .services-grid-container {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 16px;
            gap: 12px;
            
            width: 960px;
            
            /* Inside auto layout */
            flex: none;
            order: 4;
            align-self: stretch;
            flex-grow: 0;
            
            /* Ajuste para centrarlo si el container es más ancho */
            margin-left: auto;
            margin-right: auto;
        }

        /* Depth 5, Frame 0 */
        .services-grid {
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            padding: 0px;
            gap: 12px;
            
            width: 928px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 1;
        }

        /* Depth 6, Frames 0, 1, 2 (Tarjeta de Servicio) */
        .service-card {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px 0px 12px;
            gap: 12px;
            
            width: 301px;
            
            /* Inside auto layout */
            flex: none;
            order: 0; 
            align-self: stretch;
            flex-grow: 0;
        }

        /* Depth 7, Frame 0 (Imagen de Servicio) */
        .service-card-image {
            width: 301px;
            height: 301px;
            
            /* TODO: Reemplaza esta URL de marcador de posición */
            background: url(https://via.placeholder.com/301x301.png);
            background-size: cover;
            border-radius: 12px;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 0;
        }

        /* Depth 7, Frame 1 (Cuerpo de Tarjeta) */
        .service-card-body {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 0px;
            
            width: 301px;
            
            /* Inside auto layout */
            flex: none;
            order: 1;
            align-self: stretch;
            flex-grow: 0;
        }

        /* Depth 8, Frame 0 (Título de Servicio) */
        .service-card-body h3 {
            margin: 0;
            width: 301px;
            height: 24px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 600;
            font-size: 16px;
            line-height: 24px;
            color: #000000;
            
            /* Inside auto layout */
            flex: none;
            order: 0;
            align-self: stretch;
            flex-grow: 0;
        }

        /* Depth 8, Frame 1 (Descripción de Servicio) */
        .service-card-body p {
            margin: 0;
            width: 301px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: #757575;
            
            /* Inside auto layout */
            flex: none;
            order: 1;
            align-self: stretch;
            flex-grow: 0;
        }
    </style>
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
            <div class="icon"></div>
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
            <div class="section-title-services">
                <h2>Nuestros servicios</h2>
                <div class="icon"></div>
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