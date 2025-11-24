<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="InicioCliente.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.InicioCliente" %>


<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
    Inicio | MG Beauty Spa
</asp:Content>

<%-- 1. CONTENIDO DEL HEAD: Estilos y Fuentes --%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;600;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet" />

    <link href="<%: ResolveUrl("~/Content/Fonts/css/all.min.css") %>" rel="stylesheet" />

    <!-- 👇 NUEVO CSS SOLO PARA INICIO -->
    <link href="<%: ResolveUrl("~/Content/ClienteCss/InicioClienteCss.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Breadcrumb1" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    <li class="breadcrumb-item active">Inicio</li>
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