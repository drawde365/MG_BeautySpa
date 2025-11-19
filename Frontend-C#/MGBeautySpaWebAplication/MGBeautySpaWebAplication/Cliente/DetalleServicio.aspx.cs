using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSComentario;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
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
                MostrarNombreUsuario();

                // Verificar si viene de login con comentario pendiente
                if (Session["ComentarioPendienteS"] != null)
                {
                    var ComentarioPendienteS = Session["ComentarioPendienteS"] as Dictionary<string, object>;
                    if (ComentarioPendienteS != null)
                    {
                        int idServicioPendiente = (int)ComentarioPendienteS["idServicio"];

                        // Verificar que estamos en la página del mismo servicio
                        if (servicio != null && servicio.idServicio == idServicioPendiente)
                        {
                            txtComentario.Text = ComentarioPendienteS["comentario"].ToString();
                            hdnValoracion.Value = ComentarioPendienteS["valoracion"].ToString();

                            // Enviar el comentario automáticamente
                            EnviarComentario();

                            // Limpiar la sesión
                            Session.Remove("ComentarioPendienteS");
                        }
                    }
                }
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

        private void PintarServicio()
        {
            if (servicio == null)
            {
                Response.Redirect("~/Cliente/Servicios.aspx");
                return;
            }
            // --- Encabezado y Breadcrumb ---
            Page.Title = "Servicio: " + servicio.nombre;
            litNombreBreadcrumb.Text = servicio.nombre;
            litNombreServicio.Text = servicio.nombre;

            // --- Sección Principal ---
            imgServicio.ImageUrl = ResolveUrl(servicio.urlImagen);
            imgServicio.AlternateText = servicio.nombre;
            litDescripcionLarga.Text = servicio.descripcion;

            // Precio con formato de moneda local
            litPrecio.Text = servicio.precio.ToString("C", new CultureInfo("es-PE"));

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

        // --- Eventos de Botones ---

        protected void btnEnviarComent_Click(object sender, EventArgs e)
        {
            // Validar que el usuario haya iniciado sesión
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario == null)
            {
                // Guardar comentario y valoración en sesión
                var ComentarioPendienteS = new Dictionary<string, object>
        {
            { "comentario", txtComentario.Text?.Trim() ?? "" },
            { "valoracion", hdnValoracion.Value },
            { "idservicio", servicio.idServicio }
        };

                Session["ComentarioPendienteS"] = ComentarioPendienteS;
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
                int? idComentarioEditando = (int?)Session["ComentarioEditando"];
                if (idComentarioEditando != null && idComentarioEditando != 0)
                {
                    SoftInvBusiness.SoftInvWSComentario.comentarioDTO comentarioExistente = comentarioBO.ObtenerComentarioPorId((int)idComentarioEditando);
                    // ✅ MODO EDICIÓN: Actualizar comentario existente
                    comentarioExistente.comentario = texto;

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
                    // ✅ MODO INSERCIÓN: Crear nuevo comentario
                    comentarioBO.InsertarComentarioDeServicio(
                        usuario.idUsuario,
                        texto,
                        valoracion,
                        servicio.idServicio
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
                var comentarios = comentarioBO.ObtenerComentariosPorServicio(servicio.idServicio);
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
        protected void btnReservarCita_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/SeleccionarEmpleado.aspx?servicioId=" + servicio.idServicio);
        }
    }
}