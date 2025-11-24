using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCita;
using SoftInvBusiness.SoftInvWSEmpleado;
using SoftInvBusiness.SoftInvWSServicio;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Reservas : Page
    {
        private CitaBO citaBO;
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";

        private int LimiteReservas
        {
            get { return (int)(ViewState["LimiteReservas"] ?? 3); }
            set { ViewState["LimiteReservas"] = value; }
        }

        private List<SoftInvBusiness.SoftInvWSCita.citaDTO> ListaCompletaReservas
        {
            get { return (List<SoftInvBusiness.SoftInvWSCita.citaDTO>)Session["ListaReservasCliente"]; }
            set { Session["ListaReservasCliente"] = value; }
        }

        public Reservas()
        {
            citaBO = new CitaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LimiteReservas = 3;
                CargarReservas();
            }
        }

        private void CargarReservas()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            if (ListaCompletaReservas == null)
            {
                SoftInvBusiness.SoftInvWSCita.usuarioDTO user = new SoftInvBusiness.SoftInvWSCita.usuarioDTO();
                user.idUsuario = usuario.idUsuario;
                user.idUsuarioSpecified = true;
                user.rol = 1; // Rol Cliente
                user.rolSpecified = true;

                var reservas = citaBO.ListarPorUsuario(user);
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<SoftInvBusiness.SoftInvWSCita.citaDTO>();
            }
            // Forzar recarga si la lista está vacía para evitar falsos positivos de caché
            else if (ListaCompletaReservas.Count == 0)
            {
                SoftInvBusiness.SoftInvWSCita.usuarioDTO user = new SoftInvBusiness.SoftInvWSCita.usuarioDTO();
                user.idUsuario = usuario.idUsuario;
                user.idUsuarioSpecified = true;
                user.rol = 1;
                user.rolSpecified = true;

                var reservas = citaBO.ListarPorUsuario(user);
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<SoftInvBusiness.SoftInvWSCita.citaDTO>();
            }

            var listaCompleta = ListaCompletaReservas;

            if (listaCompleta == null || !listaCompleta.Any())
            {
                rptReservas.Visible = false;
                btnVerMas.Visible = false;
                pnlNoReservas.Visible = true;
            }
            else
            {
                rptReservas.Visible = true;
                pnlNoReservas.Visible = false;

                // Ordenamos: Próximas primero, luego por fecha descendente
                var listaOrdenada = listaCompleta
                    .OrderByDescending(c => c.fecha)
                    .ThenByDescending(c => c.horaIni);

                var listaMapeada = listaOrdenada.Select(c => new {
                    CitaId = c.id,
                    NumeroReserva = c.id.ToString("D3"),
                    Servicio = c.servicio.nombre,
                    Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                    Empleado = $"{c.empleado.nombre} {c.empleado.primerapellido}",
                    Total = c.servicio.precioSpecified ?
                            c.servicio.precio.ToString("C", new CultureInfo("es-PE"))
                            : "S/ 0.00",
                    HoraInicio = !string.IsNullOrEmpty(c.horaIni)
                                 ? DateTime.Parse(c.horaIni, new CultureInfo("es-ES")).ToString("hh:mm tt")
                                 : "N/A",
                    // Datos para lógica interna
                    Activo = c.activo,
                    FechaReal = c.fecha,
                    HoraRealStr = c.horaIni,
                    EmpleadoCorreo = c.empleado.correoElectronico,
                    EmpleadoCelular = c.empleado.celular,
                    Estado = c.activo==1? "Pendiente" : (c.activo==2 ? "Atendido" : "Cancelado")
                });

                var listaLimitada = listaMapeada.Take(LimiteReservas).ToList();
                rptReservas.DataSource = listaLimitada;
                rptReservas.DataBind();

                btnVerMas.Visible = (LimiteReservas < listaCompleta.Count);
            }
        }

        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            LimiteReservas += 3;
            CargarReservas();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }

        // ✅ LÓGICA CORREGIDA DE ESTADOS Y BOTÓN CANCELAR
        protected void rptReservas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var data = (dynamic)e.Item.DataItem;

                // Controles
                LinkButton btnCancelar = (LinkButton)e.Item.FindControl("btnCancelarCita");

                // Datos
                int activo = data.Activo;
                DateTime fechaReserva = data.FechaReal;
                string horaString = data.HoraRealStr;
                int citaId = data.CitaId;

                DateTime fullDateTime = fechaReserva.Date;
                if (!string.IsNullOrEmpty(horaString) && TimeSpan.TryParse(horaString, out TimeSpan hora))
                {
                    fullDateTime = fullDateTime.Add(hora);
                }

                bool esCancelable = (activo != 0) && (fullDateTime > DateTime.Now);

                if (esCancelable)
                {
                    btnCancelar.Visible = true;
                    btnCancelar.CommandArgument = citaId.ToString();
                    // 👉 ya no usamos confirm(), ahora el modal
                    btnCancelar.OnClientClick = "";
                    btnCancelar.Attributes["data-citaid"] = citaId.ToString();
                }
                else
                {
                    btnCancelar.Visible = false;
                }
            }
        }

        protected void btnConfirmarCancelacion_Click(object sender, EventArgs e)
        {
            if (int.TryParse(hfCitaIdCancelar.Value, out int citaIdParaEliminar))
            {
                var citaAEliminar = ListaCompletaReservas?
                    .FirstOrDefault(r => r.id == citaIdParaEliminar);

                if (citaAEliminar != null)
                {
                    citaBO.EliminarCita(citaAEliminar);

                    // Enviar correo al empleado
                    EnviarCorreoEmpleado(citaAEliminar);

                    // Forzar recarga desde BD para refrescar estados
                    ListaCompletaReservas = null;
                    CargarReservas();
                }
            }
        }



        protected void rptReservas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Cancelar")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int citaIdParaEliminar))
                {
                    var citaAEliminar = ListaCompletaReservas.FirstOrDefault(r => r.id == citaIdParaEliminar);

                    if (citaAEliminar != null)
                    {
                        // EliminarCita cambia el estado a 0 (inactivo) en la BD
                        citaBO.EliminarCita(citaAEliminar);

                        //Enviamos correo a empleado sobre la cancelacion
                        EnviarCorreoEmpleado(citaAEliminar);

                        ListaCompletaReservas = null; // Forzamos la recarga desde la BD
                        CargarReservas();
                    }
                }
            }
        }

        private void EnviarCorreoEmpleado(SoftInvBusiness.SoftInvWSCita.citaDTO citaAEliminar)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(citaAEliminar.empleado.correoElectronico);
            mensaje.Subject = "Cancelación de reserva | MG Beauty SPA";
            mensaje.Body = "¡Hola, " + citaAEliminar.empleado.nombre + "!\n\n" +
                           "Te informamos que la reserva programada por el cliente " + usuario.nombre + " " + usuario.primerapellido + " " + usuario.segundoapellido + " para el servicio " + citaAEliminar.servicio.nombre +
                           ", con fecha " + citaAEliminar.fecha.ToString("dd/MM/yyyy")+" "+citaAEliminar.horaIni.ToString() + ", ha sido cancelada.\n" + "Por favor, toma en cuenta este cambio en tu agenda. ¡Gracias!";
            mensaje.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;

            smtp.Send(mensaje);
        }
    }
}