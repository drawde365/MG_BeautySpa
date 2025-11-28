using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCalendario;
using SoftInvBusiness.SoftInvWSCita;
using SoftInvBusiness.SoftInvWSEmpleado;
using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using SoftInvBusiness.SoftInvWSServicio;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class MisCitas : System.Web.UI.Page
    {
        private CitaBO citaBO;
        private CalendarioBO calendarioBO;
        private HorarioTrabajoBO horarioBO;
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";

        public MisCitas()
        {
            citaBO = new CitaBO();
            calendarioBO = new CalendarioBO();
            horarioBO = new HorarioTrabajoBO();
        }

        private IList<SoftInvBusiness.SoftInvWSCita.citaDTO> ListaCompletaReservas
        {
            get { return (IList<SoftInvBusiness.SoftInvWSCita.citaDTO>)Session["ListaReservasEmpleado"]; }
            set { Session["ListaReservasEmpleado"] = value; }
        }

        private List<calendarioDTO> CalendarioCache
        {
            get
            {
                if (Session["CalendarioEmpleadoCache"] == null)
                {
                    var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                    if (usuario != null)
                    {
                        var lista = calendarioBO.ListarCalendarioDeEmpleado(usuario.idUsuario);
                        Session["CalendarioEmpleadoCache"] = (lista != null) ? lista.ToList() : new List<calendarioDTO>();
                    }
                }
                return (List<calendarioDTO>)Session["CalendarioEmpleadoCache"];
            }
            set { Session["CalendarioEmpleadoCache"] = value; }
        }

        private List<horarioTrabajoDTO> HorarioCache
        {
            get
            {
                if (Session["HorarioTrabajoCache"] == null)
                {
                    var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                    if (usuario != null)
                    {
                        var lista = horarioBO.ListarHorarioDeEmpleado(usuario.idUsuario);
                        Session["HorarioTrabajoCache"] = (lista != null) ? lista.ToList() : new List<horarioTrabajoDTO>();
                    }
                }
                return (List<horarioTrabajoDTO>)Session["HorarioTrabajoCache"];
            }
            set { Session["HorarioTrabajoCache"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CalendarioCache = null;
                HorarioCache = null;

                CargarCitas();
            }
        }

        private void CargarCitas()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            SoftInvBusiness.SoftInvWSCita.usuarioDTO user = new SoftInvBusiness.SoftInvWSCita.usuarioDTO();
            user.idUsuario = usuario.idUsuario;
            user.idUsuarioSpecified = true;
            user.rol = 2;
            user.rolSpecified = true;

            ListaCompletaReservas = citaBO.ListarPorUsuario(user);
            if (ListaCompletaReservas == null) ListaCompletaReservas = new List<SoftInvBusiness.SoftInvWSCita.citaDTO>();

            var todasLasCitas = ListaCompletaReservas;
            DateTime hoy = DateTime.Today;

            var proximas = todasLasCitas.Where(c => c.activo == 1 && c.fechaSpecified && c.fecha.Date >= hoy).OrderBy(c => c.fecha).ThenBy(c => c.horaIni).ToList();
            var pasadas = todasLasCitas.Where(c => (c.activo == 2) || (c.activo == 1 && c.fechaSpecified && c.fecha.Date < hoy)).OrderByDescending(c => c.fecha).ToList();
            var canceladas = todasLasCitas.Where(c => c.activo == 0).OrderByDescending(c => c.fecha).ToList();

            EnlazarRepeater(rptProximas, pnlNoProximas, proximas);
            EnlazarRepeater(rptPasadas, pnlNoPasadas, pasadas);
            EnlazarRepeater(rptCanceladas, pnlNoCanceladas, canceladas);
        }

        private void EnlazarRepeater(Repeater rpt, Panel pnlVacio, List<SoftInvBusiness.SoftInvWSCita.citaDTO> datos)
        {
            if (datos.Any())
            {
                rpt.DataSource = MapearCitas(datos);
                rpt.DataBind();
                pnlVacio.Visible = false;
            }
            else
            {
                rpt.DataSource = null;
                rpt.DataBind();
                pnlVacio.Visible = true;
            }
        }

        private object MapearCitas(List<SoftInvBusiness.SoftInvWSCita.citaDTO> citas)
        {
            var culturaES = new CultureInfo("es-ES");
            return citas.Select(c => new
            {
                CitaId = c.id,
                ClienteNombre = (c.cliente != null) ? $"{c.cliente.nombre} {c.cliente.primerapellido}" : "Cliente",
                ClienteCelular = (c.cliente != null && c.cliente.celular != null) ? c.cliente.celular : "N/A",
                ServicioNombre = (c.servicio != null) ? c.servicio.nombre : "Servicio",
                Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                HoraInicio = !string.IsNullOrEmpty(c.horaIni)
                    ? DateTime.Today.Add(TimeSpan.Parse(c.horaIni))
                                    .ToString("hh:mm tt", new CultureInfo("en-US"))
                                        .Replace("AM", "a. m.")
                                        .Replace("PM", "p. m.")
                                        : "N/A",
                Activo = c.activo,
                FechaCita = c.fecha
            }).ToList();
        }

        protected void rptCitas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cita = e.Item.DataItem;
                var litEstado = (Literal)e.Item.FindControl("litEstado");
                int activo = Convert.ToInt32(DataBinder.Eval(cita, "Activo"));
                DateTime fechaCita = Convert.ToDateTime(DataBinder.Eval(cita, "FechaCita"));

                if (activo == 0) litEstado.Text = "<span class=\"badge rounded-pill text-white\" style=\"background-color: #C31E1E;\">Cancelado</span>";
                else if (activo == 2) litEstado.Text = "<span class=\"badge rounded-pill text-white\" style=\"background-color: #198754;\">Confirmado</span>";
                else if (activo == 1) litEstado.Text = (fechaCita < DateTime.Today) ?
                    "<span class=\"badge rounded-pill text-dark\" style=\"background-color: #ffc107;\">Por Confirmar</span>" :
                    "<span class=\"badge rounded-pill text-white\" style=\"background-color: #A6A6A6;\">Pendiente</span>";

                var parentRepeater = e.Item.Parent as Repeater;
                if (parentRepeater != null && parentRepeater.ID == "rptPasadas")
                {
                    var btnAceptar = (LinkButton)e.Item.FindControl("btnAceptar");
                    var btnCancelar = (LinkButton)e.Item.FindControl("btnCancelar");
                    if (btnAceptar != null && btnCancelar != null)
                    {
                        bool mostrarAcciones = (activo == 1);
                        btnAceptar.Visible = mostrarAcciones;
                        btnCancelar.Visible = mostrarAcciones;
                    }
                }
            }
        }

        protected void rptPasadas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int citaId))
            {
                var cita = ListaCompletaReservas.FirstOrDefault(c => c.id == citaId);
                if (cita != null)
                {
                    if (e.CommandName == "Aceptar") citaBO.AceptarCita(cita);
                    else if (e.CommandName == "Cancelar") citaBO.EliminarCita(cita);
                    CargarCitas();
                }
            }
        }

        protected void rptProximas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                if (!int.TryParse(e.CommandArgument.ToString(), out int citaId)) return;
                var cita = ListaCompletaReservas.FirstOrDefault(c => c.id == citaId);
                if (cita == null) return;

                hdnCitaIdModal.Value = cita.id.ToString();
                txtNuevaFecha.Text = cita.fecha.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(cita.horaIni))
                {
                    var hora = DateTime.Today.Add(TimeSpan.Parse(cita.horaIni));
                    txtNuevaHora.Text = hora.ToString("hh:mm tt", new CultureInfo("en-US"));
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModificarModal",
                    "var myModal = new bootstrap.Modal(document.getElementById('modificarCitaModal')); myModal.show();", true);
            }
        }

        protected void btnGuardarCambiosCita_Click(object sender, EventArgs e)
        {
            try
            {
                int citaId = int.Parse(hdnCitaIdModal.Value);

                if (!DateTime.TryParse(txtNuevaFecha.Text, out DateTime nuevaFecha))
                {
                    MostrarErrorModal("Ingrese una fecha");
                    return;
                }

                if (!DateTime.TryParseExact(txtNuevaHora.Text, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime horaCompleta))
                {
                    MostrarErrorModal("Ingrese una hora");
                    return;
                }

                TimeSpan nuevaHora = horaCompleta.TimeOfDay;

                var citaParaModificar = ListaCompletaReservas.FirstOrDefault(c => c.id == citaId);
                if (citaParaModificar == null) return;

                int duracionMinutos = citaParaModificar.servicio.duracionHoraSpecified
                    ? citaParaModificar.servicio.duracionHora * 60
                    : 60;

                if (!EsDiaLaborableYDisponible(nuevaFecha))
                {
                    lblErrorModal.Text = "El día seleccionado no está disponible.";
                    lblErrorModal.Visible = true;
                    return;
                }
                if (!EsHoraValidaYLibre(nuevaFecha, nuevaHora, duracionMinutos, citaId))
                {
                    lblErrorModal.Text = "La hora seleccionada está fuera del horario laboral o ya está ocupada.";
                    lblErrorModal.Visible = true;
                    return;
                }

                DateTime fechaAnterior = citaParaModificar.fecha;

                citaParaModificar.fecha = nuevaFecha;
                citaParaModificar.fechaSpecified = true;
                citaParaModificar.horaIni = nuevaHora.ToString();
                citaParaModificar.horaFin = nuevaHora.Add(TimeSpan.FromMinutes(duracionMinutos)).ToString();

                citaBO.ModificarCita(citaParaModificar);

                EnviarCorreoCliente(citaParaModificar, fechaAnterior);

                Response.Redirect("~/Empleado/MisCitas.aspx", false);
                Context.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                MostrarErrorModal("Error al modificar: " + ex.Message);
            }
        }
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Empleado/MisCitas.aspx");
        }
        private void EnviarCorreoCliente(SoftInvBusiness.SoftInvWSCita.citaDTO citaParaModificar, DateTime fechaAnterior)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(citaParaModificar.cliente.correoElectronico);
            mensaje.Subject = "Tu cita ha cambiado | MG Beauty SPA";
            mensaje.Body = "¡Hola, " + citaParaModificar.cliente.nombre + "!\n\n" +
                            "Te escribimos para avisarte que tu cita programada para el día " + fechaAnterior.ToString("dd/MM/yyyy") + " para el servicio " +
                            citaParaModificar.servicio.nombre + " ha sido reprogramada.\n" + "La nueva fecha y hora es: " + citaParaModificar.fecha.ToString("dd/MM/yyyy") + " a las " + citaParaModificar.horaIni.ToString() +
                            "\n\nSi necesitas otro horario, solo dinos y con gusto te ayudamos. Te recomendamos comunicarte con el empleado a cargo.\n¡Gracias por tu comprensión!" + "\nMG Beauty SPA";
            mensaje.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;

            smtp.Send(mensaje);
        }

        private bool EsDiaLaborableYDisponible(DateTime fecha)
        {
            var calendario = this.CalendarioCache;

            if (calendario == null) return false;

            var dia = calendario.FirstOrDefault(c => c.fecha.Date == fecha.Date);

            if (dia == null || dia.cantLibre <= 0)
            {
                return false;
            }

            return true;
        }

        private bool EsHoraValidaYLibre(DateTime fecha, TimeSpan horaInicio, int duracionMinutos, int citaIdActual)
        {
            TimeSpan horaFin = horaInicio.Add(TimeSpan.FromMinutes(duracionMinutos));

            var horarioSemanal = this.HorarioCache;
            if (horarioSemanal == null) return false;

            int diaSemana = (int)fecha.DayOfWeek;
            if (diaSemana == 0) diaSemana = 7;

            bool dentroDeHorario = false;
            foreach (var bloque in horarioSemanal)
            {
                if (bloque.diaSemana == diaSemana)
                {
                    if (TimeSpan.TryParse(bloque.horaInicio.ToString(), out TimeSpan inicioLaboral) &&
                        TimeSpan.TryParse(bloque.horaFin.ToString(), out TimeSpan finLaboral))
                    {
                        if (horaInicio >= inicioLaboral && horaFin <= finLaboral)
                        {
                            dentroDeHorario = true;
                            break;
                        }
                    }
                }
            }

            if (!dentroDeHorario) return false;

            var citasDelDia = ListaCompletaReservas.Where(c =>
                c.activo == 1 &&
                c.fechaSpecified &&
                c.fecha.Date == fecha.Date &&
                c.id != citaIdActual
            ).ToList();

            foreach (var otraCita in citasDelDia)
            {
                if (string.IsNullOrEmpty(otraCita.horaIni) || string.IsNullOrEmpty(otraCita.horaFin)) continue;

                if (TimeSpan.TryParse(otraCita.horaIni, out TimeSpan otraIni) &&
                    TimeSpan.TryParse(otraCita.horaFin, out TimeSpan otraFin))
                {
                    if (horaInicio < otraFin && horaFin > otraIni)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void MostrarErrorModal(string mensaje)
        {
            lblErrorModal.Text = mensaje;
            lblErrorModal.Visible = true;
        }
    }
}