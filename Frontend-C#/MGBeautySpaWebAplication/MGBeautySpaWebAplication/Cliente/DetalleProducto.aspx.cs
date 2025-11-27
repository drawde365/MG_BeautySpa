using System;
using System.Collections.Generic;
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
            pedidoBO = new PedidoBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();

            if (!IsPostBack)
            {
                PintarProducto();
                RestaurarComentarioPendiente();
                MostrarMensajeExito();
            }

            MostrarNombreUsuario();
        }

        private void MostrarMensajeExito()
        {
            if (Session["MensajeExito"] != null)
            {
                string mensaje = Session["MensajeExito"].ToString();
                Session.Remove("MensajeExito");
                MostrarMensaje(mensaje, "success");
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

        private void RestaurarComentarioPendiente()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null) return;

            var comentarioPendiente = Session["ComentarioPendiente"] as Dictionary<string, object>;
            if (comentarioPendiente == null) return;

            try
            {
                if (!comentarioPendiente.ContainsKey("idProducto")) return;

                int idProductoPendiente = (int)comentarioPendiente["idProducto"];
                if (producto == null || producto.idProducto != idProductoPendiente) return;

                string comentarioTexto = comentarioPendiente["comentario"]?.ToString() ?? "";
                string valoracionTexto = comentarioPendiente["valoracion"]?.ToString() ?? "0";

                if (string.IsNullOrWhiteSpace(comentarioTexto)) return;
                if (!int.TryParse(valoracionTexto, out int val) || val < 1 || val > 5) return;

                txtComentario.Text = comentarioTexto;
                hdnValoracion.Value = valoracionTexto;

                EnviarComentario();

                Session.Remove("ComentarioPendiente");

                ScriptManager.RegisterStartupScript(this, GetType(), "limpiarEstrellasAuto",
                    "setTimeout(function() { document.querySelectorAll('.rating-star').forEach(s => { s.classList.remove('active'); s.textContent = '☆'; }); }, 100);", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al restaurar comentario pendiente: {ex.Message}");
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

            litNombreUsuario.Text = usuario?.nombre ?? "Invitado";

            string urlFoto = usuario?.urlFotoPerfil;

            if (!string.IsNullOrEmpty(urlFoto))
            {
                imgAvatarFormulario.ImageUrl = ResolveUrl(urlFoto);
            }
            else
            {
                imgAvatarFormulario.ImageUrl = ResolveUrl("~/Content/images/blank-photo.jpg");
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
                double promedioReal = listaComentarios.Average(c => c.valoracion);
                litReviewScore.Text = promedioReal.ToString("0.0");
                litReviewCount.Text = $"{listaComentarios.Count} reseñas";

                producto.promedioValoracion = promedioReal;
                producto.promedioValoracionSpecified = true;
                productoBO.modificarProducto(producto);
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

            ScriptManager.RegisterStartupScript(this, GetType(), "openIng",
                "var modal = new bootstrap.Modal(document.getElementById('modalIng')); modal.show();", true);
        }

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            var carrito = Session["Carrito"] as SoftInvBusiness.SoftInvWSPedido.pedidoDTO ?? CrearNuevoCarrito(usuario);
            var listaDetalles = new List<SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO>(
                carrito.detallesPedido ?? new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO[0]);
            int totalItemsAdded = 0;

            foreach (RepeaterItem item in rpPresentaciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtCantidad = (TextBox)item.FindControl("txtCantidad");
                    if (txtCantidad != null && int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
                    {
                        var presentacion = tipos[item.ItemIndex];
                        AgregarOActualizarDetalle(listaDetalles, presentacion, cantidad, carrito.idPedido);
                        totalItemsAdded += cantidad;
                        txtCantidad.Text = "0";
                    }
                }
            }

            if (totalItemsAdded > 0)
            {
                ActualizarCarrito(carrito, listaDetalles, totalItemsAdded);
                lblCartMessage.Text = "¡Producto(s) añadido(s) al carrito!";
            }
            else
            {
                lblCartMessage.Text = "Por favor, selecciona una cantidad.";
            }

            lblCartMessage.Visible = true;
        }

        private pedidoDTO CrearNuevoCarrito(SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario)
        {
            var carro = new SoftInvBusiness.SoftInvWSPedido.pedidoDTO
            {
                cliente = new SoftInvBusiness.SoftInvWSPedido.clienteDTO
                {
                    idUsuario = usuario.idUsuario,
                    idUsuarioSpecified = true
                },
                total = 0,
                totalSpecified = true,
                estadoPedido = SoftInvBusiness.SoftInvWSPedido.estadoPedido.EnCarrito,
                estadoPedidoSpecified = true,
                detallesPedido = new SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO[0]
            };
            carro.idPedido = pedidoBO.Insertar(carro);
            return carro;
        }

        private void AgregarOActualizarDetalle(List<SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO> listaDetalles,
            SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO presentacion, int cantidad, int idPedido)
        {
            var existingItem = listaDetalles.FirstOrDefault(d =>
                d.producto?.producto?.idProducto == this.producto.idProducto &&
                d.producto?.tipo?.id == presentacion.tipo.id);

            if (existingItem != null)
            {
                existingItem.cantidad += cantidad;
                existingItem.subtotal = (double)this.producto.precio * existingItem.cantidad;
                pedidoBO.ModificarDetalle(existingItem, idPedido);
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
                        tipo = new SoftInvBusiness.SoftInvWSPedido.tipoProdDTO
                        {
                            id = presentacion.tipo.id,
                            nombre = presentacion.tipo.nombre
                        },
                        ingredientes = presentacion.ingredientes,
                        stock_fisico = presentacion.stock_fisico,
                        stock_fisicoSpecified = presentacion.stock_fisicoSpecified
                    },
                    cantidad = cantidad,
                    cantidadSpecified = true,
                    subtotal = (double)this.producto.precio * cantidad,
                    subtotalSpecified = true
                };
                pedidoBO.InsertarDetalle(nuevoDetalle, idPedido);
                listaDetalles.Add(nuevoDetalle);
            }
        }

        private void ActualizarCarrito(SoftInvBusiness.SoftInvWSPedido.pedidoDTO carrito,
            List<SoftInvBusiness.SoftInvWSPedido.detallePedidoDTO> listaDetalles, int totalItemsAdded)
        {
            carrito.detallesPedido = listaDetalles.ToArray();
            carrito.total = carrito.detallesPedido.Sum(d => d.subtotal);
            carrito.totalSpecified = true;

            Session["Carrito"] = carrito;

            int currentCartCount = (Session["CartCount"] as int?) ?? 0;
            Session["CartCount"] = currentCartCount + totalItemsAdded;

            (this.Master as Cliente)?.UpdateCartDisplay();
        }

        protected void btnEnviarComent_Click(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario == null)
            {
                GuardarComentarioPendiente();
                Session["ReturnUrl"] = Request.RawUrl;
                Response.Redirect("~/Login.aspx");
                return;
            }

            EnviarComentario();
        }

        private void GuardarComentarioPendiente()
        {
            var comentarioPendiente = new Dictionary<string, object>
            {
                { "comentario", txtComentario.Text?.Trim() ?? "" },
                { "valoracion", hdnValoracion.Value },
                { "idProducto", producto.idProducto }
            };
            Session["ComentarioPendiente"] = comentarioPendiente;
        }

        private void EnviarComentario()
        {
            string texto = txtComentario.Text?.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                MostrarMensaje("Por favor, escribe un comentario.", "error");
                return;
            }

            if (!int.TryParse(hdnValoracion.Value, out int valoracion) || valoracion < 1 || valoracion > 5)
            {
                MostrarMensaje("Por favor, selecciona una valoración de 1 a 5 estrellas.", "error");
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
                int? idComentarioEditando = Session["ComentarioEditando"] as int?;

                if (idComentarioEditando.HasValue && idComentarioEditando.Value > 0)
                {
                    EditarComentarioExistente(idComentarioEditando.Value, texto, valoracion);
                    Session["MensajeExito"] = "¡Tu reseña se ha actualizado exitosamente!";
                }
                else
                {
                    comentarioBO.InsertarComentarioDeProducto(usuario.idUsuario, texto, valoracion, producto.idProducto);
                    Session["MensajeExito"] = "¡Tu reseña se ha publicado exitosamente!";
                }

                LimpiarFormularioComentario();

                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al procesar tu reseña. Inténtalo nuevamente.", "error");
                System.Diagnostics.Debug.WriteLine($"Error al procesar comentario: {ex.Message}");
            }

            PintarResenas();
        }

        private void EditarComentarioExistente(int idComentario, string texto, int valoracion)
        {
            var comentarioExistente = comentarioBO.ObtenerComentarioPorId(idComentario);

            if (comentarioExistente == null)
            {
                MostrarMensaje("El comentario que intentas editar no existe.", "error");
                Session.Remove("ComentarioEditando");
                btnEnviarComent.Text = "Enviar";
                return;
            }

            comentarioExistente.comentario = texto;
            comentarioExistente.valoracion = valoracion;
            comentarioExistente.valoracionSpecified = true;

            comentarioBO.ModificarComentario(comentarioExistente);

            Session.Remove("ComentarioEditando");
            btnEnviarComent.Text = "Enviar";
        }

        private void LimpiarFormularioComentario()
        {
            txtComentario.Text = "";
            hdnValoracion.Value = "0";
            Session.Remove("ComentarioPendiente");

            ScriptManager.RegisterStartupScript(this, GetType(), "limpiarEstrellas",
                "document.querySelectorAll('.rating-star').forEach(s => { s.classList.remove('active'); s.textContent = '☆'; });", true);
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            lblComentarioMessage.Text = mensaje;
            lblComentarioMessage.CssClass = $"comment-message {tipo}";
            lblComentarioMessage.Visible = true;
        }

        protected void rpComentarios_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            var comentario = e.Item.DataItem as SoftInvBusiness.SoftInvWSComentario.comentarioDTO;
            if (comentario == null) return;

            var litEstrellas = (Literal)e.Item.FindControl("litEstrellas");
            if (litEstrellas != null)
            {
                litEstrellas.Text = GenerarEstrellas(comentario.valoracion);
            }

            var imgAvatar = (Image)e.Item.FindControl("imgComentarioListado");

            string urlFoto = comentario.cliente?.urlFotoPerfil;

            if (!string.IsNullOrEmpty(urlFoto))
            {
                imgAvatar.ImageUrl = ResolveUrl(urlFoto);
            }
            else
            {
                imgAvatar.ImageUrl = ResolveUrl("~/Content/images/blank-photo.jpg");
            }

            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            bool esAutor = usuario != null && comentario.cliente != null &&
                            usuario.idUsuario == comentario.cliente.idUsuario;

            ConfigurarBotonComentario(e.Item, "btnEditarComentario", esAutor, comentario.idComentario);
            ConfigurarBotonComentario(e.Item, "btnEliminarComentario", esAutor, comentario.idComentario);
        }

        private string GenerarEstrellas(int valoracion)
        {
            string estrellas = "";
            for (int i = 0; i < 5; i++)
            {
                estrellas += i < valoracion ? "★" : "☆";
            }
            return estrellas;
        }

        private void ConfigurarBotonComentario(RepeaterItem item, string controlId, bool visible, int idComentario)
        {
            var btn = (LinkButton)item.FindControl(controlId);
            if (btn != null)
            {
                btn.Visible = visible;
                btn.CommandArgument = idComentario.ToString();
            }
        }

        protected void btnEditarComentario_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            if (!int.TryParse(btn.CommandArgument, out int idComentario)) return;

            try
            {
                var comentario = comentarioBO.ObtenerComentarioPorId(idComentario);
                if (comentario == null) return;

                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario == null || comentario.cliente.idUsuario != usuario.idUsuario)
                {
                    MostrarMensaje("No tienes permisos para editar este comentario.", "error");
                    return;
                }

                CargarComentarioParaEditar(comentario, idComentario);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar el comentario para editar.", "error");
                System.Diagnostics.Debug.WriteLine($"Error al editar comentario: {ex.Message}");
            }
        }

        private void CargarComentarioParaEditar(SoftInvBusiness.SoftInvWSComentario.comentarioDTO comentario, int idComentario)
        {
            txtComentario.Text = comentario.comentario;
            hdnValoracion.Value = comentario.valoracion.ToString();
            Session["ComentarioEditando"] = idComentario;
            btnEnviarComent.Text = "Actualizar";

            lblComentarioMessage.Text = "Editando tu comentario. Modifica el texto o las estrellas y presiona 'Actualizar'.";
            lblComentarioMessage.CssClass = "comment-message";
            lblComentarioMessage.Style["background-color"] = "#fff3cd";
            lblComentarioMessage.Style["color"] = "#856404";
            lblComentarioMessage.Style["border"] = "1px solid #ffeaa7";
            lblComentarioMessage.Visible = true;

            ScriptManager.RegisterStartupScript(this, GetType(), "scrollToForm",
                "document.querySelector('.add-review-form').scrollIntoView({ behavior: 'smooth', block: 'center' });", true);

            RestaurarEstrellasVisuales(comentario.valoracion);
        }

        private void RestaurarEstrellasVisuales(int valoracion)
        {
            string script = $@"
                setTimeout(function() {{
                    var stars = document.querySelectorAll('.rating-star');
                    stars.forEach(s => {{ s.classList.remove('active'); s.textContent = '☆'; }});
                    for(var i = 0; i < {valoracion}; i++) {{
                        stars[i].classList.add('active');
                        stars[i].textContent = '★';
                    }}
                }}, 100);";
            ScriptManager.RegisterStartupScript(this, GetType(), "restaurarEstrellas", script, true);
        }

        protected void btnEliminarComentario_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            if (!int.TryParse(btn.CommandArgument, out int idComentario)) return;

            try
            {
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                var comentarios = comentarioBO.ObtenerComentariosPorProducto(producto.idProducto);
                var comentario = comentarios.FirstOrDefault(c => c.idComentario == idComentario);

                if (comentario != null && comentario.cliente.idUsuario == usuario.idUsuario)
                {
                    var comentarioEliminar = new SoftInvBusiness.SoftInvWSComentario.comentarioDTO
                    {
                        idComentario = idComentario,
                        idComentarioSpecified = true
                    };
                    comentarioBO.EliminarComentario(comentarioEliminar);

                    Session["MensajeExito"] = "Tu comentario ha sido eliminado exitosamente.";
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    MostrarMensaje("No tienes permisos para eliminar este comentario.", "error");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al eliminar el comentario.", "error");
                System.Diagnostics.Debug.WriteLine($"Error al eliminar comentario: {ex.Message}");
            }
        }
    }
}