<%@ Page Title="Detalle de Producto" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" 
    AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Cliente.DetalleProducto" %>

<%-- 1. CONTENIDO DEL HEAD: CSS, Fuentes y JS --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptsContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700;800&display=swap" rel="stylesheet">
    
    <link href="DetalleProducto.css" rel="stylesheet" />
    
</asp:Content>


<%-- 2. CONTENIDO PRINCIPAL (Tu diseño deseado) --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- El ID 'smDetalle' coincide con tu C# --%>
    <asp:ScriptManager ID="smDetalle" runat="server" EnablePartialRendering="true" />

    <div class="page-container">
        
        <nav class="breadcrumb-nav">
            <a href="<%: ResolveUrl("~/Cliente/InicioCliente.aspx") %>">Inicio</a>
            <span>/</span>
            <a href="<%: ResolveUrl("~/Cliente/Productos.aspx") %>">Productos</a>
            <span>/</span>
            <strong><asp:Literal ID="litNombreProd" runat="server" /></strong> <%-- ID del C# --%>
        </nav>

        <section class="product-main-layout">
            
            <div class="product-image-column">
                <asp:Image ID="imgProducto" runat="server" CssClass="product-image-css" />
            </div>

            <div class="product-details-column">
                <h1><asp:Literal ID="litNombre" runat="server" /></h1>
                <p><asp:Literal ID="litDescripcion" runat="server" /></p>
                
                <div class="price">
                    <asp:Literal ID="litPrecio" runat="server" />
                </div>
                
                <h5 class="options-title">Seleccionar Tipo de Piel y Cantidad</h5>
                
                <%-- REPEATER (Con IDs del C#) --%>
                <asp:Repeater ID="rpPresentaciones" runat="server" OnItemDataBound="rpPresentaciones_ItemDataBound">
                  <ItemTemplate>
                    <%-- Este es el contenedor de fila de tu diseño de Figma --%>
                    <div class="tipo-item-row">
      
                      <span class="tipo-name-btn"><%# Eval("tipo.nombre") %></span>

                      <%-- 2. Selector de Cantidad (usa las clases de Figma) --%>
                      <div class="quantity-picker">
                        <button type="button" class="qty-btn-minus">-</button>
        
                        <asp:TextBox ID="txtCantidad" runat="server" Text="0" 
                            CssClass="qty-display" />
        
                        <button type="button" class="qty-btn-plus">+</button>
                      </div> 

                      <%-- 3. Botón de Ingredientes (usa la clase de Figma) --%>
                      <asp:LinkButton ID="btnIngredientes" runat="server"
                            CommandName="ver"
                            CssClass="add-button-small"
                            OnClick="btnIngredientes_Click">
                        Ingredientes
                      </asp:LinkButton>
                    </div>
                </ItemTemplate>
                </asp:Repeater>
                
                <asp:Button ID="btnAddCart" runat="server" CssClass="add-button-main"
                    Text="Añadir al carrito" OnClick="btnAddCart_Click" />

                <asp:Label ID="lblCartMessage" runat="server" Visible="false" CssClass="cart-message"></asp:Label>
            </div>
        </section>

        <section class="product-info-section">
            <h3 class="section-title">Detalles del Producto</h3>
            <div class="details-grid">
                <div class="detail-item">
                    <span class="detail-item-label">Tamaño</span>
                    <span class="detail-item-value"><asp:Literal ID="litTamano" runat="server" /></span>
                </div>
                <div class="detail-item">
                    <span class="detail-item-label">Beneficios</span>
                    <span class="detail-item-value"><asp:Literal ID="litBeneficios" runat="server" /></span>
                </div>
                <div class="detail-item">
                    <span class="detail-item-label">Cómo Usar</span>
                    <span class="detail-item-value"><asp:Literal ID="litComoUsar" runat="server" /></span>
                </div>
            </div>
        </section>

        <section class="product-reviews-section">

            <h3 class="section-title">Reseñas</h3>

            <div class="review-summary-grid">
                <div class="review-score">
                    <div class="score-number-container">
                        <span class="score-number">
                            <asp:Literal ID="litReviewScore" runat="server" />
                        </span>
                        <div class="score-stars" id="scoreStars">
                            <span class="star">★</span>
                            <span class="star">★</span>
                            <span class="star">★</span>
                            <span class="star">★</span>
                            <span class="star">★</span>
                        </div>
                    </div>
                    <div class="score-count"><asp:Literal ID="litReviewCount" runat="server" /></div>
                </div>
                <div class="review-bars">
                    <asp:Repeater ID="rpRatingBars" runat="server">
                        <ItemTemplate>
                            <div class="bar-row">
                                <span><%# Eval("Stars") %></span>
                                <div class="bar-container">
                                    <div class="bar-fill" style="width: <%# Eval("Percentage") %>%;"></div>
                                </div>
                                <span><%# Eval("Count") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="review-list">
                <%-- ID 'rpComentarios' adaptado a 'rpResenas' para que C# funcione --%>
                <asp:Repeater ID="rpComentarios" runat="server">
                    <ItemTemplate>
                        <article class="review-item">
                            <div class="review-header">
                                <asp:Image ID="imgAvatar" runat="server" ImageUrl='<%# Eval("cliente.urlFotoPerfil", "~{0}") %>' CssClass="review-avatar" />
                                <div class="review-author-info">
                                    <span class="review-author-name"><%# Eval("cliente.nombre") %></span>
                                    <!--<span class="review-date">
                                        <# ((DateTime)Eval("Fecha")).ToString("dd/MM/yyyy") >
                                        </span>-->
                                </div>
                            </div>
                            <%--<div class="review-stars">★ ★ ★ ★ ★</div>--%>
                            <p class="review-body"><%# Eval("comentario") %></p>
                            <%-- Los botones de Like/Dislike de tu diseño (necesitarían C# adicional) --%>

                        </article>
                    </ItemTemplate>
                </asp:Repeater>
                
                <%--Panel 'pnlNoComments' del C# --%>
                <asp:Panel ID="pnlNoComments" runat="server" CssClass="text-muted" Visible="false">
                    Aún no hay reseñas para este producto.
                </asp:Panel>
                    
            </div>
            
            <div class="add-review-form">
                <img class="review-avatar" src="/avatar-placeholder-user.png" alt="Tu avatar" />
                <div class="add-review-content">
                    <%-- Nombre del usuario o "Invitado" --%>
                    <div class="review-user-name">
                        <asp:Literal ID="litNombreUsuario" runat="server" />
                    </div>
        
                    <%-- Campo oculto para guardar la valoración seleccionada --%>
                    <asp:HiddenField ID="hdnValoracion" runat="server" Value="0" />
        
                    <%-- Campo de texto para el comentario --%>
                    <asp:TextBox ID="txtComentario" runat="server" TextMode="MultiLine" 
                        CssClass="review-textarea" placeholder="Escribe tu reseña..." />
        
                    <div class="review-form-footer">
                        <div class="review-rating-input" id="reviewRatingInput">
                            <span class="rating-star" data-value="1">☆</span>
                            <span class="rating-star" data-value="2">☆</span>
                            <span class="rating-star" data-value="3">☆</span>
                            <span class="rating-star" data-value="4">☆</span>
                            <span class="rating-star" data-value="5">☆</span>
                        </div>
                        <%-- ID 'btnEnviarComent' del C# --%>
                        <asp:Button ID="btnEnviarComent" runat="server" Text="Enviar" 
                            CssClass="btn-submit-review" OnClick="btnEnviarComent_Click" />
                    </div>
        
                    <%-- Mensaje de validación --%>
                    <asp:Label ID="lblComentarioMessage" runat="server" Visible="false" 
                        CssClass="comment-message"></asp:Label>
                </div>
            </div>
        </section>

    </div> <%-- Fin de .page-container --%>


    <div class="modal fade" id="modalIng" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
        <asp:UpdatePanel ID="upModalIng" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title">Ingredientes</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
              </div>
              <div class="modal-body">
                <div class="p-3 bg-light rounded">
                  <asp:Literal ID="litIngredientes" runat="server" />
                </div>
              </div>
            </div>
          </ContentTemplate>
        </asp:UpdatePanel>
      </div>
    </div>

<%-- SCRIPT PARA ESTRELLAS INTERACTIVAS DEL FORMULARIO --%>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var ratingStars = document.querySelectorAll('.rating-star');
            var hdnValoracion = document.getElementById('<%= hdnValoracion.ClientID %>');

            if (!ratingStars || ratingStars.length === 0) return;

            // Hover effect
            ratingStars.forEach(function (star, index) {
                star.addEventListener('mouseenter', function () {
                    // Limpiar todas las estrellas
                    ratingStars.forEach(s => s.classList.remove('hover-preview'));

                    // Iluminar hasta la estrella actual
                    for (var i = 0; i <= index; i++) {
                        ratingStars[i].classList.add('hover-preview');
                    }
                });

                // Click para seleccionar
                star.addEventListener('click', function () {
                    var valor = parseInt(this.getAttribute('data-value'));
                    hdnValoracion.value = valor;

                    // Limpiar todas
                    ratingStars.forEach(s => {
                        s.classList.remove('active');
                        s.textContent = '☆';
                    });

                    // Activar hasta la seleccionada
                    for (var i = 0; i < valor; i++) {
                        ratingStars[i].classList.add('active');
                        ratingStars[i].textContent = '★';
                    }
                });
            });

            // Limpiar hover al salir del contenedor
            var ratingContainer = document.getElementById('reviewRatingInput');
            if (ratingContainer) {
                ratingContainer.addEventListener('mouseleave', function () {
                    ratingStars.forEach(s => s.classList.remove('hover-preview'));
                });
            }
        });

    </script>

<%-- SCRIPT PARA CALCULAR PROMEDIO DE VALORACIONES --%>
    <script type="text/javascript">
        // Función para pintar las estrellas según el promedio
        function pintarEstrellas(promedio) {
            console.log('Pintando estrellas para promedio:', promedio);

            var stars = document.querySelectorAll('#scoreStars .star');
            if (!stars || stars.length === 0) {
                console.error('No se encontraron las estrellas');
                return;
            }

            // Limpiar todas las clases primero
            stars.forEach(function (star) {
                star.classList.remove('filled', 'half-filled');
            });

            var promedioNum = parseFloat(promedio);
            console.log('Promedio numérico:', promedioNum);

            if (isNaN(promedioNum) || promedioNum <= 0) {
                console.log('Promedio inválido o cero');
                return; // No pintar nada si es 0 o inválido
            }

            var parteEntera = Math.floor(promedioNum);
            var parteDecimal = promedioNum - parteEntera;

            console.log('Parte entera:', parteEntera, 'Parte decimal:', parteDecimal);

            // Pintar estrellas completas
            for (var i = 0; i < parteEntera && i < 5; i++) {
                stars[i].classList.add('filled');
                console.log('Estrella', i, 'pintada completa');
            }

            // Pintar media estrella si el decimal es >= 0.3
            if (parteDecimal >= 0.3 && parteEntera < 5) {
                if (parteDecimal >= 0.8) {
                    // Si es >= 0.8, pintar la siguiente estrella completa
                    stars[parteEntera].classList.add('filled');
                    console.log('Estrella', parteEntera, 'pintada completa (por decimal alto)');
                } else {
                    // Si es entre 0.3 y 0.7, pintar media estrella
                    stars[parteEntera].classList.add('half-filled');
                    console.log('Estrella', parteEntera, 'pintada a medias');
                }
            }
        }

        // Función para calcular y mostrar el promedio de valoraciones
        function actualizarPromedioValoraciones(idProducto) {
            if (!idProducto || idProducto === 0) {
                console.log('ID de producto no válido');
                return;
            }

            console.log('Obteniendo comentarios para producto:', idProducto);

            // Construir URL del servicio web
            var serviceUrl = '<%= ResolveUrl("~/Services/WSComentario.asmx/ObtenerComentariosPorProducto") %>';

            // Realizar petición AJAX
            fetch(serviceUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ idProducto: idProducto })
            })
                .then(function (response) {
                    if (!response.ok) throw new Error('Error en la respuesta del servidor');
                    return response.json();
                })
                .then(function (data) {
                    console.log('Respuesta del servicio:', data);

                    // ASP.NET Web Services envuelven la respuesta en .d
                    var comentarios = data.d;
                    console.log('Comentarios obtenidos:', comentarios);

                    if (!comentarios || comentarios.length === 0) {
                        console.log('No hay comentarios para este producto');
                        pintarEstrellas(0);
                        return;
                    }

                    // Calcular el promedio de valoraciones
                    var sumaValoraciones = 0;
                    var totalComentarios = comentarios.length;

                    for (var i = 0; i < comentarios.length; i++) {
                        console.log('Comentario', i, 'valoración:', comentarios[i].valoracion);
                        if (comentarios[i].valoracion) {
                            sumaValoraciones += comentarios[i].valoracion;
                        }
                    }

                    console.log('Suma total:', sumaValoraciones, 'Total comentarios:', totalComentarios);
                    var promedio = sumaValoraciones / totalComentarios;
                    console.log('Promedio calculado:', promedio);

                    // Actualizar el número en pantalla
                    var numeroElement = document.querySelector('.score-number');
                    if (numeroElement) {
                        var currentText = numeroElement.textContent;
                        numeroElement.childNodes[0].textContent = promedio.toFixed(1) + ' ';
                    }

                    // Pintar las estrellas según el promedio
                    pintarEstrellas(promedio);
                })
                .catch(function (error) {
                    console.error('Error al obtener valoraciones:', error);
                    pintarEstrellas(0);
                });
        }

        // Ejecutar cuando el DOM esté listo
        window.addEventListener('DOMContentLoaded', function () {
            var idProducto = <%= Request.QueryString["id"] ?? "0" %>;
            console.log('Página cargada, ID producto:', idProducto);

            // Leer el promedio que viene del servidor
            var scoreElement = document.querySelector('.score-number');
            if (scoreElement) {
                // Obtener solo el texto del Literal (primer nodo de texto)
                var primerTexto = '';
                for (var i = 0; i < scoreElement.childNodes.length; i++) {
                    if (scoreElement.childNodes[i].nodeType === 3) { // Node.TEXT_NODE
                        primerTexto = scoreElement.childNodes[i].textContent.trim();
                        break;
                    }
                }

                console.log('Texto del score inicial:', primerTexto);
                var promedioInicial = parseFloat(primerTexto);
                console.log('Promedio inicial parseado:', promedioInicial);

                if (!isNaN(promedioInicial) && promedioInicial > 0) {
                    pintarEstrellas(promedioInicial);
                }
            }

        });
    </script>

</asp:Content>