using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSComentario;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class DetalleServicio : Page
    {
        private ServicioBO servicioBO;
        private ComentarioBO comentarioBO;
        private SoftInvBusiness.SoftInvWSServicio.servicioDTO servicio;

        public DetalleServicio()
        {
            servicioBO = new ServicioBO();
            comentarioBO = new ComentarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();

            if (!IsPostBack)
            {
                PintarServicio();
                RestaurarComentarioPendiente();
                MostrarMensajeExito();
            }

            MostrarNombreUsuario();
        }

        private void CargarDatos()
        {
            if (!IsPostBack)
            {
                string idservicioStr = Request.QueryString["id"];
                if (int.TryParse(idservicioStr, out int idservicio))
                {
                    servicio = servicioBO.obtenerPorId(idservicio);
                    Session["detalle_servicio"] = servicio;
                }
                else
                {
                    Response.Redirect("~/Cliente/Servicios.aspx");
                }
            }
            else
            {
                servicio = Session["detalle_servicio"] as SoftInvBusiness.SoftInvWSServicio.servicioDTO;
            }
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

        private void RestaurarComentarioPendiente()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null) return;

            var comentarioPendiente = Session["ComentarioPendienteS"] as Dictionary<string, object>;
            if (comentarioPendiente == null) return;

            try
            {
                if (!comentarioPendiente.ContainsKey("idServicio")) return;

                int idServicioPendiente = (int)comentarioPendiente["idServicio"];
                if (servicio == null || servicio.idServicio != idServicioPendiente) return;

                string comentarioTexto = comentarioPendiente["comentario"]?.ToString() ?? "";
                string valoracionTexto = comentarioPendiente["valoracion"]?.ToString() ?? "0";

                if (string.IsNullOrWhiteSpace(comentarioTexto)) return;
                if (!int.TryParse(valoracionTexto, out int val) || val < 1 || val > 5) return;

                txtComentario.Text = comentarioTexto;
                hdnValoracion.Value = valoracionTexto;

                EnviarComentario();

                Session.Remove("ComentarioPendienteS");

                ScriptManager.RegisterStartupScript(this, GetType(), "limpiarEstrellasAuto",
                    "setTimeout(function() { document.querySelectorAll('.rating-star').forEach(s => { s.classList.remove('active'); s.textContent = '☆'; }); }, 100);", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al restaurar comentario pendiente: {ex.Message}");
                Session.Remove("ComentarioPendienteS");
            }
        }

        private void PintarServicio()
        {
            if (servicio == null)
            {
                Response.Redirect("~/Cliente/Servicios.aspx");
                return;
            }

            Page.Title = "Servicio: " + servicio.nombre;
            litNombreBreadcrumb.Text = servicio.nombre;
            litNombreServicio.Text = servicio.nombre;
            imgServicio.ImageUrl = ResolveUrl(servicio.urlImagen);
            imgServicio.AlternateText = servicio.nombre;
            litDescripcionLarga.Text = servicio.descripcion;
            LiteralDuracion.Text = "Duración (horas): " + servicio.duracionHora.ToString();
            litPrecio.Text = servicio.precio.ToString("C", new CultureInfo("es-PE"));

            PintarResenas();
        }

        private void MostrarNombreUsuario()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            litNombreUsuario.Text = usuario?.nombre ?? "Invitado";
        }

        private void PintarResenas()
        {
            var listaComentarios = comentarioBO.ObtenerComentariosPorServicio(servicio.idServicio);

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

                servicio.promedioValoracion = promedioReal;
                servicio.promedioValoracionSpecified = true;
                servicioBO.modificar(servicio);
            }
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
                { "idServicio", servicio.idServicio }
            };
            Session["ComentarioPendienteS"] = comentarioPendiente;
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
                    comentarioBO.InsertarComentarioDeServicio(usuario.idUsuario, texto, valoracion, servicio.idServicio);
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
            Session.Remove("ComentarioPendienteS");

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

                var comentarios = comentarioBO.ObtenerComentariosPorServicio(servicio.idServicio);
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

        protected void btnReservarCita_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/SeleccionarEmpleado.aspx?servicioId=" + servicio.idServicio);
        }
    }
}