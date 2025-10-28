<%@ Page Title="Detalle de producto" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="DetalleProducto.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.DetalleProducto" %>

<asp:Content ID="ctHead" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="ctTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="litTitulo" runat="server" />
</asp:Content>

<asp:Content ID="ctMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="smDetalle" runat="server" EnablePartialRendering="true" />
    <!-- Migas -->
    <nav class="mb-3 small">
        <a href="<%: ResolveUrl("~/Cliente/InicioCliente.aspx") %>">Inicio</a> /
        <a href="<%: ResolveUrl("~/Cliente/Productos.aspx") %>">Productos</a> /
        <strong><asp:Literal ID="litNombreProd" runat="server" /></strong>
    </nav>

    <div class="row g-4 align-items-start">
        <!-- Imagen -->
        <div class="col-lg-6">
            <asp:Image ID="imgProducto" runat="server" CssClass="img-fluid rounded shadow-sm" />
        </div>

        <!-- Info principal -->
        <div class="col-lg-6">
            <h1 class="h2 mb-3"><asp:Literal ID="litNombre" runat="server" /></h1>
            <p class="lead"><asp:Literal ID="litDescripcion" runat="server" /></p>

            <div class="display-6 fw-bold my-3">S/. <asp:Literal ID="litPrecio" runat="server" /></div>

            <h5 class="mb-3">Seleccionar Tipo de Piel y Cantidad</h5>

            <!-- Variantes / tipos -->
            <asp:Repeater ID="rpPresentaciones" runat="server" OnItemDataBound="rpPresentaciones_ItemDataBound">
              <ItemTemplate>
                <div class="d-flex align-items-center gap-3 mb-3 flex-wrap">
                  <span class="btn btn-light px-3" style="min-width: 120px;"><%# Eval("Tipo") %></span>

                  <div class="input-group quantity-control" style="width: 150px;">
                    <button type="button" class="btn btn-outline-secondary minus">-</button>
    
                    <asp:TextBox ID="txtCantidad" runat="server" 
                                 Text="0" 
                                 CssClass="form-control text-center qty" />
                    
                    <button type="button" class="btn btn-outline-secondary plus">+</button>
                </div> 

                  <asp:LinkButton ID="btnIngredientes" runat="server"
                                  CommandName="ver"
                                  CommandArgument='<%# Eval("Tipo") %>'
                                  CssClass="btn btn-primary"
                                  OnClick="btnIngredientes_Click">
                    Ver ingredientes
                  </asp:LinkButton>
                </div>
              </ItemTemplate>
            </asp:Repeater>

            <asp:Button ID="btnAddCart" runat="server" CssClass="btn btn-primary btn-lg mt-2"
                Text="Añadir al carrito" OnClick="btnAddCart_Click" />
        </div>
    </div>

    <!-- Tabs de detalle -->
    <div class="mt-5">
        <ul class="nav nav-tabs" id="tabsProd" role="tablist">
            <li class="nav-item"><button class="nav-link active" data-bs-toggle="tab" data-bs-target="#tab-det">Detalles</button></li>
            <li class="nav-item"><button class="nav-link" data-bs-toggle="tab" data-bs-target="#tab-uso">Modo de uso</button></li>
            <li class="nav-item"><button class="nav-link" data-bs-toggle="tab" data-bs-target="#tab-com">Reseñas</button></li>
        </ul>

        <div class="tab-content p-3 border-bottom border-start border-end rounded-bottom">
            <div class="tab-pane fade show active" id="tab-det">
                <h5>Descripción</h5>
                <p><asp:Literal ID="litDescripcionLarga" runat="server" /></p>
            </div>
            <div class="tab-pane fade" id="tab-uso">
                <h5>Modo de uso</h5>
                <p><asp:Literal ID="litModoUso" runat="server" /></p>
            </div>
            <div class="tab-pane fade" id="tab-com">
                <asp:UpdatePanel ID="upComentarios" runat="server">
                    <ContentTemplate>
                        <h5 class="mb-3">Escribe tu comentario</h5>
                        <div class="row g-2 mb-3">
                            <div class="col-md-4">
                                <asp:TextBox ID="txtNombreComent" runat="server" CssClass="form-control" placeholder="Tu nombre (opcional)"></asp:TextBox>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtComentario" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="¿Qué te pareció el producto?"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Button ID="btnEnviarComent" runat="server" CssClass="btn btn-primary mb-3"
                            Text="Publicar comentario" OnClick="btnEnviarComent_Click" />

                        <asp:Panel ID="pnlNoComments" runat="server" CssClass="text-muted">Aún no hay comentarios.</asp:Panel>

                        <asp:Repeater ID="rpComentarios" runat="server">
                            <ItemTemplate>
                                <div class="border rounded p-3 mb-2">
                                    <div class="small text-muted">
                                        <strong><%# Eval("Autor") %></strong> — <%# ((DateTime)Eval("Fecha")).ToString("dd/MM/yyyy HH:mm") %>
                                    </div>
                                    <div><%# Eval("Texto") %></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Modal Ingredientes -->
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

<asp:Content ID="ctScripts" ContentPlaceHolderID="ScriptsContent" runat="server">
    <!-- JS externo -->
    <script src="<%: ResolveUrl("~/Scripts/MGBeautySpaScripts/ProductDetail.js?v=1") %>"></script>
</asp:Content>
