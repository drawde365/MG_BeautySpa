<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="InicioCliente.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.InicioCliente" %>

<%-- 1. CONTENIDO DEL HEAD: Estilos y Fuentes --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    
    <%-- 
        NOTA: Tu CSS original 'InicioClienteStyle.css' tenía anchos fijos (width: 960px).
        He movido y adaptado esos estilos aquí abajo para que funcionen con el carrusel 
        y sean responsivos. Puedes borrar el link a 'InicioClienteStyle.css' 
        y usar solo este bloque <style>.
    --%>
    
    <link href="../Content/Fonts/css/all.min.css" rel="stylesheet" />

    <%-- ▼▼▼ CSS ADAPTADO PARA EL CARRUSEL ▼▼▼ --%>
    <style>
        :root {
            --primary-color: #148C76;
            --accent-color: #1EC3B6;
            --text-primary: #171214;
            --text-secondary: #757575;
            --border-color: #E3D4D9;
            --bg-white: #FFFFFF;
            --bg-light: #F9FAFB;
            --radius-md: 12px;
        }

        .content-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center; /* Centra el contenido */
            width: 100%;
            padding: 0;
            margin: 0 auto;
            max-width: 960px; /* Ancho máximo de tu diseño */
        }

        /* --- Hero Section --- */
        .hero-section {
            width: 100%;
            padding: 16px;
            box-sizing: border-box;
        }
        .hero-banner {
            width: 100%;
            height: 480px; /* Altura fija */
            min-height: 300px; /* Altura mínima en móviles */
            position: relative;
            border-radius: var(--radius-md);
            overflow: hidden; /* Asegura que la imagen respete el borde */
        }
        .hero-banner img {
            width: 100%;
            height: 100%;
            object-fit: cover; /* Cubre el contenedor */
            object-position: center;
        }
        .hero-text {
            position: absolute;
            bottom: 1.5rem;
            right: 1.5rem;
            left: 1.5rem;
            color: white;
            text-align: right;
            text-shadow: 0 2px 4px rgba(0,0,0,0.5);
        }
        .hero-text h1 {
            font-family: 'ZCOOL XiaoWei', serif;
            font-weight: 400;
            font-size: 48px;
            letter-spacing: -2px;
            margin: 0;
        }
        .hero-text p {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            max-width: 600px; /* Limita el ancho del texto */
            margin-left: auto; /* Alinea a la derecha */
        }

        /* --- Títulos de Sección --- */
        .section-title {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
            padding: 20px 16px 12px 16px;
            box-sizing: border-box;
        }
        .section-title h2 {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 22px;
            color: var(--text-primary);
        }
        .section-title a { text-decoration: none; }

        /* --- Estructura del Carrusel --- */
        .carousel-container {
            display: flex;
            align-items: center;
            justify-content: space-between;
            width: 100%;
            box-sizing: border-box;
            padding: 0 16px;
        }
        .carousel-button {
            background-color: transparent;
            border: 2px solid var(--primary-color);
            color: var(--primary-color);
            border-radius: 50%;
            width: 40px;
            height: 40px;
            font-size: 20px;
            font-weight: 700;
            cursor: pointer;
            flex-shrink: 0;
            transition: all 0.2s ease;
            z-index: 10;
        }
        .carousel-button:hover {
            background-color: var(--accent-color);
            color: white;
            border-color: var(--accent-color);
        }
        .carousel-button:disabled {
            border-color: #ccc;
            color: #ccc;
            cursor: not-allowed;
        }
        
        /* La "ventana" que recorta el grid */
        .carousel-viewport {
            width: 100%;
            overflow: hidden;
            margin: 0 12px;
        }
        
        /* El 'div' que se mueve */
        .product-grid, .services-grid {
            display: flex;
            flex-wrap: nowrap; /* ¡Importante! */
            gap: 12px;
            padding: 16px 0;
            /* La magia del carrusel */
            transition: transform 0.5s cubic-bezier(0.25, 0.8, 0.25, 1);
        }

        /* --- Tarjetas (Items del Carrusel) --- */
        .product-card-link, .service-card-link {
            text-decoration: none;
            display: block;
            width: 301px; /* Ancho fijo de tu diseño */
            flex-shrink: 0; /* Evita que la tarjeta se encoja */
        }
        
        .product-card, .service-card {
            display: flex;
            flex-direction: column;
            gap: 12px;
            width: 100%;
        }

        .product-card-image {
            width: 100%;
            height: 251px;
            background-color: var(--bg-light);
            background-size: cover;
            background-position: center;
            border-radius: var(--radius-md);
        }
        .service-card-image {
            width: 100%;
            height: 301px;
            background-color: var(--bg-light);
            background-size: cover;
            background-position: center;
            border-radius: var(--radius-md);
        }

        .product-card-body h3, .service-card-body h3 {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 600;
            font-size: 16px;
            line-height: 24px;
            color: var(--text-primary);
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        .product-card-body p, .service-card-body p {
            margin: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            color: var(--text-secondary);
        }
        
        /* Ajuste para móvil */
        @media (max-width: 768px) {
            .hero-text h1 { font-size: 32px; }
            .hero-text p { font-size: 14px; }
            
            /* En móvil, el carrusel muestra menos items */
            .carousel-viewport { margin: 0 8px; }
            .carousel-button { width: 32px; height: 32px; font-size: 16px; }
            .product-card-link, .service-card-link { width: 240px; }
        }

    </style>
    <%-- ▲▲▲ FIN CSS CORREGIDO ▲▲▲ --%>
</asp:Content>


<%-- 2. CONTENIDO PRINCIPAL: HTML --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="content-wrapper">

        <section class="hero-section">
            <div class="hero-banner">
                <%-- Usamos la imagen real --%>
                <img src="<%: ResolveUrl("~/Content/images/Cliente/Bienvenida.jpg") %>" alt="MG Beauty Spa" />
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

        <%-- ▼▼▼ SECCIÓN DE PRODUCTOS DINÁMICA (CARRUSEL) ▼▼▼ --%>
        <section class="carousel-container">
            <button class="carousel-button" id="prod-prev" disabled>&lt;</button>
            
            <div class="carousel-viewport" id="prod-viewport">
                <asp:Repeater ID="rptProductosDestacados" runat="server">
                    <HeaderTemplate>
                        <div class="product-grid" id="product-grid-inner">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href='<%# Eval("UrlDetalle") %>' class="product-card-link">
                            <article class="product-card">
                                <div class="product-card-image" style="background-image: url('<%# Eval("ImageUrl") %>');"></div>
                                <div class="product-card-body">
                                    <h3><%# Eval("Nombre") %></h3>
                                    <p><%# Eval("Descripcion") %></p>
                                </div>
                            </article>
                        </a>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            
            <button class="carousel-button" id="prod-next">&gt;</button>
        </section>
        <%-- ▲▲▲ FIN SECCIÓN PRODUCTOS ▲▲▲ --%>


        <div class="section-title">
            <h2>Nuestros servicios</h2>
            <a href="<%: ResolveUrl("~/Cliente/Servicios.aspx") %>" title="Ir a mi perfil">
                <i class="fa-solid fa-angle-right" style="color: #000000;"></i>
            </a>
        </div>
        
        <%-- ▼▼▼ SECCIÓN DE SERVICIOS DINÁMICA (CARRUSEL) ▼▼▼ --%>
        <section class="carousel-container">
             <button class="carousel-button" id="serv-prev" disabled>&lt;</button>
             <div class="carousel-viewport" id="serv-viewport">
                 <asp:Repeater ID="rptServiciosDestacados" runat="server">
                    <HeaderTemplate>
                        <div class="services-grid" id="service-grid-inner">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <a href='<%# Eval("UrlDetalle") %>' class="service-card-link">
                            <article class="service-card">
                                <div class="service-card-image" style="background-image: url('<%# Eval("ImageUrl") %>');"></div>
                                <div class="service-card-body">
                                    <h3><%# Eval("Nombre") %></h3>
                                    <p><%# Eval("Descripcion") %></p>
                                </div>
                            </article>
                        </a>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                 </asp:Repeater>
             </div>
             <button class="carousel-button" id="serv-next">&gt;</button>
         </section>
         <%-- ▲▲▲ FIN SECCIÓN SERVICIOS ▲▲▲ --%>

    </div>
    
</asp:Content>


<%-- 3. CONTENIDO DE SCRIPTS (CON JAVASCRIPT) --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    
    <%-- ▼▼▼ JAVASCRIPT DEL CARRUSEL ▼▼▼ --%>
    <script type="text/javascript">
        window.addEventListener('load', function () {
            
            // Configura el carrusel de PRODUCTOS
            setupCarousel({
                gridSelector: '#product-grid-inner',
                viewportSelector: '#prod-viewport',
                cardSelector: '.product-card-link',
                prevButtonSelector: '#prod-prev',
                nextButtonSelector: '#prod-next',
                itemsToShow: 3 // Cuántos items mostrar a la vez
            });
            
            // Configura el carrusel de SERVICIOS
            setupCarousel({
                gridSelector: '#service-grid-inner',
                viewportSelector: '#serv-viewport',
                cardSelector: '.service-card-link',
                prevButtonSelector: '#serv-prev',
                nextButtonSelector: '#serv-next',
                itemsToShow: 3
            });

        });

        /**
         * Función genérica para inicializar un carrusel
         * @param {object} config - Objeto de configuración
         */
        function setupCarousel(config) {
            const grid = document.querySelector(config.gridSelector);
            const viewport = document.querySelector(config.viewportSelector);
            const prevButton = document.querySelector(config.prevButtonSelector);
            const nextButton = document.querySelector(config.nextButtonSelector);

            if (!grid || !grid.children.length) {
                if(prevButton) prevButton.style.display = 'none';
                if(nextButton) nextButton.style.display = 'none';
                return; 
            }

            const cards = grid.querySelectorAll(config.cardSelector);
            const totalItems = cards.length;
            let itemsToShow = config.itemsToShow || 3;
            
            // --- CÁLCULO DE DESLIZAMIENTO ---
            // 1. Ancho total del grid
            const gridWidth = grid.scrollWidth;
            // 2. Ancho visible del viewport
            const viewportWidth = viewport.offsetWidth;
            // 3. Ancho de 1 item (asumiendo que todos son iguales)
            const cardWidth = cards[0].offsetWidth;
            // 4. Gap (12px)
            const gap = 12;
            
            // 5. Calcula cuántos items caben realmente en la ventana
            // Esto hace que sea responsivo si los items cambian de tamaño
            itemsToShow = Math.max(1, Math.floor(viewportWidth / (cardWidth + gap)));
            
            // 6. Ancho de un solo "paso" (slide)
            const slideWidth = (cardWidth + gap);
            
            let currentIndex = 0;
            const maxIndex = totalItems - itemsToShow; 

            // Oculta el botón 'next' si no hay suficientes items
            if(totalItems <= itemsToShow){
                nextButton.disabled = true;
            }

            nextButton.addEventListener('click', function (e) {
                e.preventDefault();
                if (currentIndex < maxIndex) {
                    currentIndex++;
                    grid.style.transform = `translateX(-${currentIndex * slideWidth}px)`;
                }
                updateButtons();
            });

            prevButton.addEventListener('click', function (e) {
                e.preventDefault();
                if (currentIndex > 0) {
                    currentIndex--;
                    grid.style.transform = `translateX(-${currentIndex * slideWidth}px)`;
                }
                updateButtons();
            });

            function updateButtons() {
                prevButton.disabled = (currentIndex === 0);
                nextButton.disabled = (currentIndex >= maxIndex);
            }
        }

    </script>
</asp:Content>