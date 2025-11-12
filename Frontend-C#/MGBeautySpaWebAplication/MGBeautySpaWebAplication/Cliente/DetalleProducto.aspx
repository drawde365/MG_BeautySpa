<%@ Page Title="Detalle de Producto" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" 
    AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs" 
    Inherits="MGBeautySpaWebAplication.Cliente.DetalleProducto" %>

<%-- 1. CONTENIDO DEL HEAD: CSS, Fuentes y JS --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptsContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700;800&display=swap" rel="stylesheet">
    
    <link href="DetalleProducto.css" rel="stylesheet" />
    
    <script src="<%: ResolveUrl("~/Scripts/MGBeautySpaScripts/ProductDetail.js?v=1") %>"></script>
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
      
                      <%-- 1. Botón de Tipo de Piel (usa la clase de Figma) --%>
                      <span class="tipo-name-btn"><%# Eval("Tipo") %></span>

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
                    <div class="score-number"><asp:Literal ID="litReviewScore" runat="server" /></div>
                    <%--<div class="score-stars">★ ★ ★ ★ ☆</div>--%>
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
                
                <%-- Panel 'pnlNoComments' del C# 
                <asp:Panel ID="pnlNoComments" runat="server" CssClass="text-muted" Visible="false">
                    Aún no hay reseñas para este producto.
                </asp:Panel>
                    --%>
            </div>

            <div class="add-review-form">
                <img class="review-avatar" src="/avatar-placeholder-user.png" alt="Tu avatar" />
                <div class="add-review-content">
                    <%-- IDs 'txtNombreComent' y 'txtComentario' del C# --%>
                    <asp:TextBox ID="txtNombreComent" runat="server" CssClass="review-textarea" placeholder="Tu nombre (opcional)" Rows="1" style="height: 40px; margin-bottom: 5px;"/>
                    <asp:TextBox ID="txtComentario" runat="server" TextMode="MultiLine" CssClass="review-textarea" placeholder="Escribe tu reseña..." />
                    <div class="review-form-footer">
                        <div class="review-rating-input">
                            <span>☆</span><span>☆</span><span>☆</span><span>☆</span><span>☆</span>
                        </div>
                        <%-- ID 'btnEnviarComent' del C# --%>
                        <asp:Button ID="btnEnviarComent" runat="server" Text="Enviar" CssClass="btn-submit-review" OnClick="btnEnviarComent_Click" />
                    </div>
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
</asp:Content>