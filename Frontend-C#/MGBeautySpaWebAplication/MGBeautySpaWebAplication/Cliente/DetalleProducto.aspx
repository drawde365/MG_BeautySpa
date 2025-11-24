<%@ Page Title="Detalle de Producto" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" 
    AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Cliente.DetalleProducto" %>

<asp:Content ID="Content0" ContentPlaceHolderID="TitleContent" runat="server">
     Detalle Producto | MG Beauty Spa
</asp:Content>

<%-- 1. CONTENIDO DEL HEAD: CSS, Fuentes y JS --%>
<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/ClienteCss/DetalleProductoCss.css") %>" />
</asp:Content>

<%-- 2. BREADCRUMB --%>
<asp:Content ID="Breadcrumb1" ContentPlaceHolderID="BreadcrumbContent" runat="server">
    <li class="breadcrumb-item"><a href="InicioCliente.aspx">Inicio</a></li>
    <li class="breadcrumb-item"><a href="Productos.aspx">Productos</a></li>
    <li class="breadcrumb-item active"><asp:Literal ID="litNombreProd" runat="server" /></li>
</asp:Content>

<%-- 3. CONTENIDO PRINCIPAL --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="smDetalle" runat="server" EnablePartialRendering="true" />

    <div class="detalle-page-wrapper">
        <div class="detalle-page-content">

            <section class="product-main-layout">
                
                <div class="product-image-column">
                    <asp:Image ID="imgProducto" runat="server" CssClass="product-image-css" />
                </div>

                <div class="product-details-column">
                    <h1 class="product-title"><asp:Literal ID="litNombre" runat="server" /></h1>
                    <p class="product-description-text"><asp:Literal ID="litDescripcion" runat="server" /></p>
                    
                    <div class="price">
                        <asp:Literal ID="litPrecio" runat="server" />
                    </div>
                    
                    <h5 class="options-title">Seleccionar Tipo de Piel y Cantidad</h5>
                    
                    <asp:Repeater ID="rpPresentaciones" runat="server" OnItemDataBound="rpPresentaciones_ItemDataBound">
                      <ItemTemplate>
                        <div class="tipo-item-row">
          
                          <span class="tipo-name-btn"><%# Eval("tipo.nombre") %></span>

                          <div class="quantity-picker">
                            <button type="button" class="qty-btn-minus">-</button>
            
                            <asp:TextBox ID="txtCantidad" runat="server" Text="0" 
                                CssClass="qty-display" />
            
                            <button type="button" class="qty-btn-plus">+</button>
                          </div> 

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
                    <asp:Repeater ID="rpComentarios" runat="server" OnItemDataBound="rpComentarios_ItemDataBound">
                        <ItemTemplate>
                            <article class="review-item">
                                <div class="review-header">
                                    <asp:Image ID="imgAvatar" runat="server" 
                                        ImageUrl='<%# Eval("cliente.urlFotoPerfil", "~{0}") %>' 
                                        CssClass="review-avatar" />
                                    <div class="review-author-info">
                                        <span class="review-author-name"><%# Eval("cliente.nombre") %></span>
                                        <div class="review-item-stars">
                                            <asp:Literal ID="litEstrellas" runat="server" />
                                        </div>
                                    </div>
                                </div>
                
                                <p class="review-body"><%# Eval("comentario") %></p>
                
                                <div class="review-actions">
                                    <asp:LinkButton ID="btnEditarComentario" runat="server" 
                                        CssClass="btn-review-action btn-edit" 
                                        ToolTip="Editar comentario"
                                        OnClick="btnEditarComentario_Click"
                                        Visible="false">
                                        <i class="fas fa-edit"></i> Editar
                                    </asp:LinkButton>
                    
                                    <asp:LinkButton ID="btnEliminarComentario" runat="server" 
                                        CssClass="btn-review-action btn-delete" 
                                        ToolTip="Eliminar comentario"
                                        OnClick="btnEliminarComentario_Click"
                                        OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este comentario?');"
                                        Visible="false">
                                        <i class="fas fa-trash-alt"></i> Eliminar
                                    </asp:LinkButton>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>                
                    <asp:Panel ID="pnlNoComments" runat="server" CssClass="text-muted" Visible="false">
                        Aún no hay reseñas para este producto.
                    </asp:Panel>
                        
                </div>
                
                <div class="add-review-form">
                    <img class="review-avatar" src="/avatar-placeholder-user.png" alt="Tu avatar" />
                    <div class="add-review-content">
                        <div class="review-user-name">
                            <asp:Literal ID="litNombreUsuario" runat="server" />
                        </div>
            
                        <asp:HiddenField ID="hdnValoracion" runat="server" Value="0" />
            
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
                            <asp:Button ID="btnEnviarComent" runat="server" Text="Enviar" 
                                CssClass="btn-submit-review" OnClick="btnEnviarComent_Click" />
                        </div>
            
                        <asp:Label ID="lblComentarioMessage" runat="server" Visible="false" 
                            CssClass="comment-message"></asp:Label>
                    </div>
                </div>
            </section>

        </div>
    </div>

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

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var ratingStars = document.querySelectorAll('.rating-star');
            var hdnValoracion = document.getElementById('<%= hdnValoracion.ClientID %>');

            if (!ratingStars || ratingStars.length === 0) return;

            ratingStars.forEach(function (star, index) {
                star.addEventListener('mouseenter', function () {
                    ratingStars.forEach(s => s.classList.remove('hover-preview'));
                    for (var i = 0; i <= index; i++) {
                        ratingStars[i].classList.add('hover-preview');
                    }
                });

                star.addEventListener('click', function () {
                    var valor = parseInt(this.getAttribute('data-value'));
                    hdnValoracion.value = valor;

                    ratingStars.forEach(s => {
                        s.classList.remove('active');
                        s.textContent = '☆';
                    });

                    for (var i = 0; i < valor; i++) {
                        ratingStars[i].classList.add('active');
                        ratingStars[i].textContent = '★';
                    }
                });
            });

            var ratingContainer = document.getElementById('reviewRatingInput');
            if (ratingContainer) {
                ratingContainer.addEventListener('mouseleave', function () {
                    ratingStars.forEach(s => s.classList.remove('hover-preview'));
                });
            }
        });

        function pintarEstrellas(promedio) {
            console.log('Pintando estrellas para promedio:', promedio);

            var stars = document.querySelectorAll('#scoreStars .star');
            if (!stars || stars.length === 0) {
                console.error('No se encontraron las estrellas');
                return;
            }

            stars.forEach(function (star) {
                star.classList.remove('filled', 'half-filled');
            });

            var promedioNum = parseFloat(promedio);
            console.log('Promedio numérico:', promedioNum);

            if (isNaN(promedioNum) || promedioNum <= 0) {
                console.log('Promedio inválido o cero');
                return;
            }

            var parteEntera = Math.floor(promedioNum);
            var parteDecimal = promedioNum - parteEntera;

            console.log('Parte entera:', parteEntera, 'Parte decimal:', parteDecimal);

            for (var i = 0; i < parteEntera && i < 5; i++) {
                stars[i].classList.add('filled');
                console.log('Estrella', i, 'pintada completa');
            }

            if (parteDecimal >= 0.3 && parteEntera < 5) {
                if (parteDecimal >= 0.8) {
                    stars[parteEntera].classList.add('filled');
                    console.log('Estrella', parteEntera, 'pintada completa (por decimal alto)');
                } else {
                    stars[parteEntera].classList.add('half-filled');
                    console.log('Estrella', parteEntera, 'pintada a medias');
                }
            }
        }

        function actualizarPromedioValoraciones(idProducto) {
            if (!idProducto || idProducto === 0) {
                console.log('ID de producto no válido');
                return;
            }

            console.log('Obteniendo comentarios para producto:', idProducto);

            var serviceUrl = '<%= ResolveUrl("~/Services/WSComentario.asmx/ObtenerComentariosPorProducto") %>';

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

                    var comentarios = data.d;
                    console.log('Comentarios obtenidos:', comentarios);

                    if (!comentarios || comentarios.length === 0) {
                        console.log('No hay comentarios para este producto');
                        pintarEstrellas(0);
                        return;
                    }

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

                    var numeroElement = document.querySelector('.score-number');
                    if (numeroElement) {
                        var currentText = numeroElement.textContent;
                        numeroElement.childNodes[0].textContent = promedio.toFixed(1) + ' ';
                    }

                    pintarEstrellas(promedio);
                })
                .catch(function (error) {
                    console.error('Error al obtener valoraciones:', error);
                    pintarEstrellas(0);
                });
        }

        window.addEventListener('DOMContentLoaded', function () {
            var idProducto = <%= Request.QueryString["id"] ?? "0" %>;
            console.log('Página cargada, ID producto:', idProducto);

            var scoreElement = document.querySelector('.score-number');
            if (scoreElement) {
                var primerTexto = '';
                for (var i = 0; i < scoreElement.childNodes.length; i++) {
                    if (scoreElement.childNodes[i].nodeType === 3) {
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