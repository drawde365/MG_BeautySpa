using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSComentario;
using SoftInvBusiness.SoftInvWSPedido;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSProductoTipo;
using SoftInvBusiness.SoftInvWSUsuario;
using System.Globalization;
using System.Web.Script.Serialization;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class DetalleProducto : Page
    {
        private ProductoBO productoBO;
        private ComentarioBO comentarioBO;
        private ProductoTipoBO productoTipoBO;
        private PedidoBO pedidoBO;
        private SoftInvBusiness.SoftInvWSProductos.productoDTO producto;
        private IList<SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO> tipos;

        public DetalleProducto()
        {
            productoBO = new ProductoBO();
            productoTipoBO = new ProductoTipoBO();
            comentarioBO = new ComentarioBO();
            pedidoBO = new PedidoBO(); // <-- 2. AÑADIDO
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();

            if (!IsPostBack)
            {
                PintarProducto();
                MostrarNombreUsuario();

                // ✅ VERIFICAR SI VIENE DE LOGIN CON COMENTARIO PENDIENTE
                VerificarYEnviarComentarioPendiente();
            }
            else
            {
                MostrarNombreUsuario();
            }
        }

        private void CargarDatos()
        {
            if (!IsPostBack)
            {
                string idProductoStr = Request.QueryString["id"];
                if (int.TryParse(idProductoStr, out int idProducto))
                {
                    producto = productoBO.buscarPorId(idProducto);
                    tipos = productoTipoBO.ObtenerPorIdProductoActivo(idProducto);

                    Session["detalle_producto"] = producto;
                    Session["detalle_tipos"] = tipos;
                }
                else
                {
                    Response.Redirect("~/Cliente/Productos.aspx");
                }
            }
            else
            {
                producto = Session["detalle_producto"] as SoftInvBusiness.SoftInvWSProductos.productoDTO;
                tipos = Session["detalle_tipos"] as IList<SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO>;
            }
        }

        private void VerificarYEnviarComentarioPendiente()
        {
            // Verificar que el usuario esté logueado
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario == null)
            {
                return;
            }

            // Verificar si hay un comentario pendiente
            var comentarioPendiente = Session["ComentarioPendiente"] as Dictionary<string, object>;

            if (comentarioPendiente == null)
            {
                return;
            }

            try
            {
                // ✅ VALIDAR QUE SEA EL PRODUCTO CORRECTO **ANTES** DE LIMPIAR
                if (!comentarioPendiente.ContainsKey("idProducto"))
                {
                    Session.Remove("ComentarioPendiente");
                    return;
                }

                int idProductoPendiente = (int)comentarioPendiente["idProducto"];

                // ✅ SI NO ES EL PRODUCTO CORRECTO, NO HACER NADA (mantener sesión)
                if (producto == null || producto.idProducto != idProductoPendiente)
                {
                    return;
                }

                // ✅ AHORA SÍ - Es el producto correcto, limpiar sesión
                Session.Remove("ComentarioPendiente");

                // Validar datos antes de procesar
                string comentarioTexto = comentarioPendiente["comentario"]?.ToString() ?? "";
                string valoracionTexto = comentarioPendiente["valoracion"]?.ToString() ?? "0";

                if (string.IsNullOrWhiteSpace(comentarioTexto))
                {
                    System.Diagnostics.Debug.WriteLine("Comentario pendiente vacío, se descarta.");
                    return;
                }

                if (!int.TryParse(valoracionTexto, out int val) || val < 1 || val > 5)
                {
                    System.Diagnostics.Debug.WriteLine("Valoración pendiente inválida, se descarta.");
                    return;
                }

                // Restaurar en el formulario
                txtComentario.Text = comentarioTexto;
                hdnValoracion.Value = valoracionTexto;

                // Enviar automáticamente
                EnviarComentario();

                // Limpiar estrellas visuales
                string scriptEstrellas = @"
            setTimeout(function() {
                var stars = document.querySelectorAll('.rating-star');
                stars.forEach(function(s) {
                    s.classList.remove('active');
                    s.textContent = '☆';
                });
            }, 100);
        ";
                ScriptManager.RegisterStartupScript(this, GetType(), "limpiarEstrellasAuto", scriptEstrellas, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al verificar comentario pendiente: {ex.Message}");
                Session.Remove("ComentarioPendiente");
            }
        }

        private void PintarProducto()
        {
            if (producto == null)
            {
                Response.Redirect("~/Cliente/Productos.aspx");
                return;
            }

            Page.Title = producto.nombre;
            litNombreProd.Text = producto.nombre;
            litNombre.Text = producto.nombre;
            litDescripcion.Text = producto.descripcion;
            litPrecio.Text = producto.precio.ToString("C", new CultureInfo("es-PE"));
            imgProducto.ImageUrl = ResolveUrl(producto.urlImagen);

            litTamano.Text = producto.tamanho.ToString() + "ml";
            litComoUsar.Text = producto.modoUso;

            rpPresentaciones.DataSource = tipos;
            rpPresentaciones.DataBind();

            PintarResenas();
        }

        private void MostrarNombreUsuario()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario != null)
            {
                litNombreUsuario.Text = usuario.nombre ?? "Usuario";
            }
            else
            {
                litNombreUsuario.Text = "Invitado";
            }
        }

        private void PintarResenas()
        {
            var listaComentarios = comentarioBO.ObtenerComentariosPorProducto(producto.idProducto);

            rpComentarios.DataSource = listaComentarios;
            rpComentarios.DataBind();

            if (listaComentarios == null || listaComentarios.Count == 0)
            {
                pnlNoComments.Visible = true;
                litReviewScore.Text = "0.0";
                litReviewCount.Text = "0 reseñas";
            }
            else
            {
                pnlNoComments.Visible = false;

                double sumaValoraciones = 0;
                int totalComentarios = listaComentarios.Count;

                foreach (var comentario in listaComentarios)
                {
                    sumaValoraciones += comentario.valoracion;
                }

                double promedioReal = totalComentarios > 0 ? sumaValoraciones / totalComentarios : 0;

                litReviewScore.Text = promedioReal.ToString("0.0");
                litReviewCount.Text = $"{totalComentarios} reseñas";
            }
        }

        protected void rpPresentaciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var btn = (LinkButton)e.Item.FindControl("btnIngredientes");
                btn.CommandArgument = e.Item.ItemIndex.ToString();
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(btn);
            }
        }

        protected void btnIngredientes_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            if (!int.TryParse(btn.CommandArgument, out var idx)) return;

            var pres = tipos[idx];
            if (pres == null) return;

            litIngredientes.Text = Server.HtmlEncode(pres.ingredientes).Replace("\n", "<br/>");

            upModalIng.Update();

            ScriptManager.RegisterStartupScript(
                this, GetType(), "openIng", "var modal = new bootstrap.Modal(document.getElementById('modalIng')); modal.show();", true);
        }

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            SoftInvBusiness.SoftInvWSPedido.pedidoDTO carrito = Session["Carrito"] as SoftInvBusiness.SoftInvWSPedido.pedidoDTO;

            if (carrito == null)
            {
                pedidoDTO carro = new pedidoDTO();
                carro.cliente = new SoftInvBusiness.SoftInvWSPedido.clienteDTO();
                carro.cliente.idUsuario = usuario.idUsuario;
                carro.cliente.idUsuarioSpecified = true;
                carro.total = 0;
                carro.totalSpecified = true;
                carro.estadoPedido = estadoPedido.EnCarrito;
                carro.estadoPedidoSpecified = true;
                carro.detallesPedido = new detallePedidoDTO[0];
                carro.idPedido = pedidoBO.Insertar(carro);

                carrito = carro;
            }
            else if (carrito.idPedido == 0)
            {
                pedidoDTO carro = new pedidoDTO();
                carro.cliente = new SoftInvBusiness.SoftInvWSPedido.clienteDTO();
                carro.cliente.idUsuario = usuario.idUsuario;
                carro.cliente.idUsuarioSpecified = true;
                carro.total = 0;
                carro.totalSpecified = true;
                carro.estadoPedido = estadoPedido.EnCarrito;
                carro.estadoPedidoSpecified = true;
                carro.detallesPedido = new detallePedidoDTO[0];
                carro.idPedido = pedidoBO.Insertar(carro);

                carrito = carro;
            }

            var listaDetalles = new List<SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO>(carrito.detallesPedido ?? new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO[0]);
            int totalItemsAdded = 0;

            foreach (RepeaterItem item in rpPresentaciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtCantidad = (TextBox)item.FindControl("txtCantidad");
                    int itemIndex = item.ItemIndex;

                    if (txtCantidad != null &&
                        int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
                    {
                        SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO presentacion = tipos[itemIndex];

                        var existingItem = listaDetalles.FirstOrDefault(d =>
                            d.producto != null &&
                            d.producto.producto != null &&
                            d.producto.tipo != null &&
                            d.producto.producto.idProducto == this.producto.idProducto &&
                            d.producto.tipo.id == presentacion.tipo.id
                        );

                        if (existingItem != null)
                        {
                            existingItem.cantidad += cantidad;
                            existingItem.subtotal = (double)this.producto.precio * existingItem.cantidad;

                            // ----- 4. LLAMA AL BO PARA MODIFICAR EL DETALLE EN DB -----
                            pedidoBO.ModificarDetalle(existingItem, carrito.idPedido);
                        }
                        else
                        {
                            var nuevoDetalle = new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO
                            {
                                producto = new SoftInvBusiness.SoftInvWSPedido.productoTipoDTO
                                {
                                    producto = new SoftInvBusiness.SoftInvWSPedido.productoDTO
                                    {
                                        idProducto = this.producto.idProducto,
                                        idProductoSpecified = true,
                                        urlImagen = this.producto.urlImagen,
                                        tamanho = this.producto.tamanho,
                                        tamanhoSpecified = true,
                                        nombre = this.producto.nombre,
                                        precio = this.producto.precio,
                                        precioSpecified = true
                                    },
                                    tipo = new SoftInvBusiness.SoftInvWSPedido.tipoProdDTO { id = presentacion.tipo.id, nombre = presentacion.tipo.nombre },
                                    ingredientes = presentacion.ingredientes,
                                    stock_fisico = presentacion.stock_fisico,
                                    stock_fisicoSpecified = presentacion.stock_fisicoSpecified
                                },
                                cantidad = cantidad,
                                cantidadSpecified = true,
                                subtotal = (double)this.producto.precio * cantidad,
                                subtotalSpecified = true
                            };
                            pedidoBO.InsertarDetalle(nuevoDetalle, carrito.idPedido);
                            listaDetalles.Add(nuevoDetalle);
                        }
                        totalItemsAdded += cantidad;
                        txtCantidad.Text = "0";
                    }
                }
            }

            if (totalItemsAdded > 0)
            {
                if (listaDetalles[0].producto.producto.idProducto == 0)
                    listaDetalles.Remove(listaDetalles[0]);
                carrito.detallesPedido = listaDetalles.ToArray();
                carrito.total = carrito.detallesPedido.Sum(d => d.subtotal);
                carrito.totalSpecified = true;

                Session["Carrito"] = carrito;

                int currentCartCount = (Session["CartCount"] as int?) ?? 0;
                Session["CartCount"] = currentCartCount + totalItemsAdded;

                Cliente masterPage = this.Master as Cliente;
                if (masterPage != null)
                {
                    masterPage.UpdateCartDisplay();
                }

                lblCartMessage.Text = "¡Producto(s) añadido(s) al carrito!";
                lblCartMessage.Visible = true;
            }
            else
            {
                lblCartMessage.Text = "Por favor, selecciona una cantidad.";
                lblCartMessage.Visible = true;
            }
        }

        protected void btnEnviarComent_Click(object sender, EventArgs e)
        {
            // Validar que el usuario haya iniciado sesión
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario == null)
            {
                // Guardar comentario y valoración en sesión
                var comentarioPendiente = new Dictionary<string, object>
                {
                    { "comentario", txtComentario.Text?.Trim() ?? "" },
                    { "valoracion", hdnValoracion.Value },
                    { "idProducto", producto.idProducto }
                };

                Session["ComentarioPendiente"] = comentarioPendiente;
                Session["ReturnUrl"] = Request.RawUrl;

                // Redirigir a login
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Si está logueado, enviar el comentario
            EnviarComentario();
        }

        private void EnviarComentario()
        {
            string texto = txtComentario.Text?.Trim();

            // Validar que haya texto
            if (string.IsNullOrEmpty(texto))
            {
                lblComentarioMessage.Text = "Por favor, escribe un comentario.";
                lblComentarioMessage.CssClass = "comment-message error";
                lblComentarioMessage.Visible = true;
                return;
            }

            // Validar valoración
            if (!int.TryParse(hdnValoracion.Value, out int valoracion) || valoracion < 1 || valoracion > 5)
            {
                lblComentarioMessage.Text = "Por favor, selecciona una valoración de 1 a 5 estrellas.";
                lblComentarioMessage.CssClass = "comment-message error";
                lblComentarioMessage.Visible = true;
                return;
            }

            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            try
            {
                // ✅ VERIFICAR SI ESTAMOS EDITANDO O INSERTANDO
                int? idComentarioEditando = Session["ComentarioEditando"] as int?;

                if (idComentarioEditando.HasValue && idComentarioEditando.Value > 0)
                {
                    // ✅ MODO EDICIÓN
                    SoftInvBusiness.SoftInvWSComentario.comentarioDTO comentarioExistente =
                        comentarioBO.ObtenerComentarioPorId(idComentarioEditando.Value);

                    if (comentarioExistente == null)
                    {
                        lblComentarioMessage.Text = "El comentario que intentas editar no existe.";
                        lblComentarioMessage.CssClass = "comment-message error";
                        lblComentarioMessage.Visible = true;
                        Session.Remove("ComentarioEditando");
                        btnEnviarComent.Text = "Enviar";
                        return;
                    }

                    comentarioExistente.comentario = texto;
                    comentarioExistente.valoracion = valoracion;
                    comentarioExistente.valoracionSpecified = true;

                    comentarioBO.ModificarComentario(comentarioExistente);

                    lblComentarioMessage.Text = "¡Tu reseña se ha actualizado exitosamente!";
                    lblComentarioMessage.CssClass = "comment-message success";

                    // Limpiar sesión de edición
                    Session.Remove("ComentarioEditando");

                    // Restaurar texto del botón
                    btnEnviarComent.Text = "Enviar";
                }
                else
                {
                    // ✅ MODO INSERCIÓN - Crear nuevo comentario
                    comentarioBO.InsertarComentarioDeProducto(
                        usuario.idUsuario,
                        texto,
                        valoracion,
                        producto.idProducto
                    );

                    lblComentarioMessage.Text = "¡Tu reseña se ha publicado exitosamente!";
                    lblComentarioMessage.CssClass = "comment-message success";
                }

                lblComentarioMessage.Visible = true;

                // Limpiar formulario
                txtComentario.Text = "";
                hdnValoracion.Value = "0";

                // Actualizar la lista de reseñas
                PintarResenas();

                // Registrar script para limpiar las estrellas
                ScriptManager.RegisterStartupScript(this, GetType(), "limpiarEstrellas",
                    "document.querySelectorAll('.rating-star').forEach(s => { s.classList.remove('active'); s.textContent = '☆'; });", true);
            }
            catch (Exception ex)
            {
                lblComentarioMessage.Text = "Error al procesar tu reseña. Inténtalo nuevamente.";
                lblComentarioMessage.CssClass = "comment-message error";
                lblComentarioMessage.Visible = true;

                System.Diagnostics.Debug.WriteLine($"Error al procesar comentario: {ex.Message}");
            }
        }
        protected void rpComentarios_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Obtener el comentario actual
                var comentario = e.Item.DataItem as SoftInvBusiness.SoftInvWSComentario.comentarioDTO;

                if (comentario == null) return;

                // ✅ AGREGAR ESTRELLAS DEL COMENTARIO
                var litEstrellas = (Literal)e.Item.FindControl("litEstrellas");
                if (litEstrellas != null)
                {
                    string estrellas = "";
                    int valoracion = comentario.valoracion;

                    for (int i = 0; i < 5; i++)
                    {
                        if (i < valoracion)
                        {
                            estrellas += "★"; // Estrella llena
                        }
                        else
                        {
                            estrellas += "☆"; // Estrella vacía
                        }
                    }

                    litEstrellas.Text = estrellas;
                }

                // Obtener el usuario de la sesión
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

                // Verificar si el usuario es el autor del comentario
                bool esAutor = false;

                if (usuario != null && comentario.cliente != null)
                {
                    esAutor = (usuario.idUsuario == comentario.cliente.idUsuario);
                }

                // Mostrar u ocultar los botones según sea el autor
                var btnEditar = (LinkButton)e.Item.FindControl("btnEditarComentario");
                var btnEliminar = (LinkButton)e.Item.FindControl("btnEliminarComentario");

                if (btnEditar != null)
                {
                    btnEditar.Visible = esAutor;
                    btnEditar.CommandArgument = comentario.idComentario.ToString();
                }

                if (btnEliminar != null)
                {
                    btnEliminar.Visible = esAutor;
                    btnEliminar.CommandArgument = comentario.idComentario.ToString();
                }
            }
        }
        protected void btnEditarComentario_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;

            if (!int.TryParse(btn.CommandArgument, out int idComentario))
            {
                return;
            }

            try
            {
                // Obtener el comentario completo
                var comentario = comentarioBO.ObtenerComentarioPorId(idComentario);

                if (comentario == null) return;

                // Verificar que el usuario sea el autor
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

                if (usuario == null || comentario.cliente.idUsuario != usuario.idUsuario)
                {
                    lblComentarioMessage.Text = "No tienes permisos para editar este comentario.";
                    lblComentarioMessage.CssClass = "comment-message error";
                    lblComentarioMessage.Visible = true;
                    return;
                }

                // Cargar el comentario en el formulario para editarlo
                txtComentario.Text = comentario.comentario;
                hdnValoracion.Value = comentario.valoracion.ToString();

                // Guardar el ID del comentario que se está editando
                Session["ComentarioEditando"] = idComentario;

                // Cambiar el texto del botón
                btnEnviarComent.Text = "Actualizar";

                // Mostrar mensaje informativo
                lblComentarioMessage.Text = "Editando tu comentario. Modifica el texto o las estrellas y presiona 'Actualizar'.";
                lblComentarioMessage.CssClass = "comment-message";
                lblComentarioMessage.Style["background-color"] = "#fff3cd";
                lblComentarioMessage.Style["color"] = "#856404";
                lblComentarioMessage.Style["border"] = "1px solid #ffeaa7";
                lblComentarioMessage.Visible = true;

                // Scroll al formulario
                ScriptManager.RegisterStartupScript(this, GetType(), "scrollToForm",
                    "document.querySelector('.add-review-form').scrollIntoView({ behavior: 'smooth', block: 'center' });", true);

                // Restaurar las estrellas visuales
                string scriptEstrellas = $@"
            setTimeout(function() {{
                var stars = document.querySelectorAll('.rating-star');
                var valor = {comentario.valoracion};
                
                stars.forEach(function(s) {{
                    s.classList.remove('active');
                    s.textContent = '☆';
                }});
                
                for(var i = 0; i < valor; i++) {{
                    stars[i].classList.add('active');
                    stars[i].textContent = '★';
                }}
            }}, 100);
        ";
                ScriptManager.RegisterStartupScript(this, GetType(), "restaurarEstrellasEdicion", scriptEstrellas, true);
            }
            catch (Exception ex)
            {
                lblComentarioMessage.Text = "Error al cargar el comentario para editar.";
                lblComentarioMessage.CssClass = "comment-message error";
                lblComentarioMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error al editar comentario: {ex.Message}");
            }
        }

        protected void btnEliminarComentario_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;

            if (!int.TryParse(btn.CommandArgument, out int idComentario))
            {
                return;
            }

            try
            {
                // Verificar que el usuario sea el autor
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                // Obtener el comentario para verificar autoría
                var comentarios = comentarioBO.ObtenerComentariosPorProducto(producto.idProducto);
                var comentario = comentarios.FirstOrDefault(c => c.idComentario == idComentario);

                if (comentario != null && comentario.cliente.idUsuario == usuario.idUsuario)
                {
                    // Eliminar el comentario
                    SoftInvBusiness.SoftInvWSComentario.comentarioDTO comentarioEliminar = new SoftInvBusiness.SoftInvWSComentario.comentarioDTO
                    {
                        idComentario = idComentario,
                        idComentarioSpecified = true
                    };
                    comentarioBO.EliminarComentario(comentarioEliminar);

                    // Mostrar mensaje de éxito
                    lblComentarioMessage.Text = "Tu comentario ha sido eliminado exitosamente.";
                    lblComentarioMessage.CssClass = "comment-message success";
                    lblComentarioMessage.Visible = true;

                    // Actualizar la lista
                    PintarResenas();
                }
                else
                {
                    lblComentarioMessage.Text = "No tienes permisos para eliminar este comentario.";
                    lblComentarioMessage.CssClass = "comment-message error";
                    lblComentarioMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblComentarioMessage.Text = "Error al eliminar el comentario.";
                lblComentarioMessage.CssClass = "comment-message error";
                lblComentarioMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error al eliminar comentario: {ex.Message}");
            }
        }
    }
}