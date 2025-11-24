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

        // Caché para las Citas del Empleado
        private IList<SoftInvBusiness.SoftInvWSCita.citaDTO> ListaCompletaReservas
        {
            get { return (IList<SoftInvBusiness.SoftInvWSCita.citaDTO>)Session["ListaReservasEmpleado"]; }
            set { Session["ListaReservasEmpleado"] = value; }
        }

        // ✅ CACHÉ 1: Calendario (Disponibilidad de días)
        // Evita consultar la BD cada vez que validamos un día
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
            // No necesitamos setter público, se llena solo bajo demanda o se limpia al recargar
            set { Session["CalendarioEmpleadoCache"] = value; }
        }

        // ✅ CACHÉ 2: Horario de Trabajo (Configuración semanal)
        // El horario base rara vez cambia, es ideal para cachear
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

        // --------------------------------------------------------------------
        // EVENTOS DEL CICLO DE VIDA
        // --------------------------------------------------------------------

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiamos la caché al entrar por primera vez para asegurar datos frescos
                CalendarioCache = null;
                HorarioCache = null;

                CargarCitas();
            }
            lblErrorFechaHora.Visible = false;
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

            // Recargamos las citas (esto sí conviene que sea fresco)
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

        // Helper para reducir código repetitivo
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
            return citas.Select(c => new {
                CitaId = c.id,
                ClienteNombre = (c.cliente != null) ? $"{c.cliente.nombre} {c.cliente.primerapellido}" : "Cliente",
                ClienteCelular = (c.cliente != null && c.cliente.celular != null) ? c.cliente.celular : "N/A",
                ServicioNombre = (c.servicio != null) ? c.servicio.nombre : "Servicio",
                Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                HoraInicio = !string.IsNullOrEmpty(c.horaIni) ? DateTime.Parse(c.horaIni, culturaES).ToString("hh:mm tt", culturaES) : "N/A",
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
                if (!string.IsNullOrEmpty(cita.horaIni)) txtNuevaHora.Text = TimeSpan.Parse(cita.horaIni).ToString(@"hh\:mm");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModificarModal",
                    "var myModal = new bootstrap.Modal(document.getElementById('modificarCitaModal')); myModal.show();", true);
            }
        }

        protected void btnGuardarCambiosCita_Click(object sender, EventArgs e)
        {
            try
            {
                int citaId = int.Parse(hdnCitaIdModal.Value);
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO; // No verificamos null porque Page_Load ya lo hizo

                if (!DateTime.TryParse(txtNuevaFecha.Text, out DateTime nuevaFecha)) { MostrarErrorJS("Fecha inválida"); return; }
                if (!TimeSpan.TryParse(txtNuevaHora.Text, out TimeSpan nuevaHora)) { MostrarErrorJS("Hora inválida"); return; }

                var citaParaModificar = ListaCompletaReservas.FirstOrDefault(c => c.id == citaId);
                if (citaParaModificar == null) return;

                int duracionMinutos = citaParaModificar.servicio.duracionHoraSpecified ? citaParaModificar.servicio.duracionHora * 60 : 60;

                // 1. VALIDAR: Disponibilidad del día (Usando CACHÉ)
                if (!EsDiaLaborableYDisponible(nuevaFecha))
                {
                    lblErrorFechaHora.Text = "El día seleccionado no está disponible en tu calendario.";
                    lblErrorFechaHora.Visible = true;
                    return;
                }

                // 2. VALIDAR: Horario y Cruce (Usando CACHÉ)
                if (!EsHoraValidaYLibre(nuevaFecha, nuevaHora, duracionMinutos, citaId))
                {
                    lblErrorFechaHora.Text = "La hora seleccionada está fuera del horario laboral o ya está ocupada.";
                    lblErrorFechaHora.Visible = true;
                    return;
                }

                DateTime fechaAnterior = citaParaModificar.fecha;
                // Guardado
                citaParaModificar.fecha = nuevaFecha;
                citaParaModificar.fechaSpecified = true;
                citaParaModificar.horaIni = nuevaHora.ToString();
                citaParaModificar.horaFin = nuevaHora.Add(TimeSpan.FromMinutes(duracionMinutos)).ToString();

                citaBO.ModificarCita(citaParaModificar);

                //Mandamos Correo al cliente avisandole que se modificó la cita
                EnviarCorreoCliente(citaParaModificar, fechaAnterior);

                // Limpiamos las citas para forzar recarga fresca, pero NO limpiamos el CalendarioCache/HorarioCache
                // porque el horario base no cambió.
                ListaCompletaReservas = null;
                CargarCitas();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideModificarModal",
                    "var myModal = bootstrap.Modal.getInstance(document.getElementById('modificarCitaModal')); myModal.hide(); alert('Cita modificada con éxito.');",
                    true);

                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                MostrarErrorJS("Error al modificar: " + ex.Message);
            }
        }

        private void EnviarCorreoCliente(SoftInvBusiness.SoftInvWSCita.citaDTO citaParaModificar,DateTime fechaAnterior)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(citaParaModificar.cliente.correoElectronico);
            mensaje.Subject = "Tu cita ha cambiado | MG Beauty SPA";
            mensaje.Body = "¡Hola, " + citaParaModificar.cliente.nombre + "!\n\n" +
                           "Te escribimos para avisarte que tu cita programada para el día "+fechaAnterior.ToString("dd/MM/yyyy")+" para el servicio "+
                           citaParaModificar.servicio.nombre+" ha sido reprogramada.\n"+ "La nueva fecha y hora es: "+ citaParaModificar.fecha.ToString("dd/MM/yyyy") +" a las "+citaParaModificar.horaIni.ToString() +
                           "\n\nSi necesitas otro horario, solo dinos y con gusto te ayudamos. Te recomendamos comunicarte con el empleado a cargo.\n¡Gracias por tu comprensión!"+"\nMG Beauty SPA";
            mensaje.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;

            smtp.Send(mensaje);
        }

        // --------------------------------------------------------------------
        // ⚡ MÉTODOS DE VALIDACIÓN USANDO CACHÉ ⚡
        // --------------------------------------------------------------------

        private bool EsDiaLaborableYDisponible(DateTime fecha)
        {
            // Usamos la propiedad 'CalendarioCache' que gestiona la sesión automáticamente
            var calendario = this.CalendarioCache;

            if (calendario == null) return false;

            // Buscamos en la lista en memoria
            var dia = calendario.FirstOrDefault(c => c.fecha.Date == fecha.Date);

            // Si no existe el día en el calendario generado o cantLibre <= 0, no está disponible
            if (dia == null || dia.cantLibre <= 0)
            {
                return false;
            }

            return true;
        }

        private bool EsHoraValidaYLibre(DateTime fecha, TimeSpan horaInicio, int duracionMinutos, int citaIdActual)
        {
            TimeSpan horaFin = horaInicio.Add(TimeSpan.FromMinutes(duracionMinutos));

            // A. VALIDAR HORARIO LABORAL (Usando CACHÉ)
            var horarioSemanal = this.HorarioCache;
            if (horarioSemanal == null) return false;

            int diaSemana = (int)fecha.DayOfWeek;
            if (diaSemana == 0) diaSemana = 7;

            bool dentroDeHorario = false;
            foreach (var bloque in horarioSemanal)
            {
                if (bloque.diaSemana == diaSemana)
                {
                    // Convertimos los strings "HH:mm:ss" a TimeSpan
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

            // B. VALIDAR CHOQUE CON OTRAS CITAS (Usamos la lista de citas ya cargada en memoria)
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
                    // Lógica de colisión: (InicioA < FinB) Y (FinA > InicioB)
                    if (horaInicio < otraFin && horaFin > otraIni)
                    {
                        return false; // Hay cruce
                    }
                }
            }

            return true;
        }

        private void MostrarErrorJS(string mensaje)
        {
            string script = $"alert('{mensaje}'); var myModal = new bootstrap.Modal(document.getElementById('modificarCitaModal')); myModal.show();";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowError", script, true);
        }
    }
}