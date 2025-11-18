using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCalendario;
using SoftInvBusiness.SoftInvWSCita;
using SoftInvBusiness.SoftInvWSServicio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace MGBeautySpaWebAplication.Cliente
{
    public class DiaCalendario
    {
        public int Day { get; set; }
        public int Status { get; set; }
        public DateTime FechaCompleta { get; set; }
    }

    public partial class Calendario : System.Web.UI.Page
    {
        private CalendarioBO calendarioBO;
        private ServicioBO servicioBO;
        private CitaBO citaBO;

        private Dictionary<DateTime, int> DisponibilidadCache
        {
            get { return (Dictionary<DateTime, int>)Session["EmpleadoAvailabilityData"]; }
            set { Session["EmpleadoAvailabilityData"] = value; }
        }

        public int EmpleadoId
        {
            get { return (int)(ViewState["EmpleadoId"] ?? 0); }
            set { ViewState["EmpleadoId"] = value; }
        }

        public int ServicioId
        {
            get { return (int)(ViewState["ServicioId"] ?? 0); }
            set { ViewState["ServicioId"] = value; }
        }

        public int DuracionServicio
        {
            get { return (int)(ViewState["DuracionServicio"] ?? 60); }
            set { ViewState["DuracionServicio"] = value; }
        }

        private DateTime CurrentDisplayMonth
        {
            get
            {
                if (ViewState["CurrentDisplayMonth"] == null)
                {
                    ViewState["CurrentDisplayMonth"] = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                }
                return (DateTime)ViewState["CurrentDisplayMonth"];
            }
            set { ViewState["CurrentDisplayMonth"] = value; }
        }

        public Calendario()
        {
            calendarioBO = new CalendarioBO();
            servicioBO = new ServicioBO();
            citaBO = new CitaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int.TryParse(Request.QueryString["empleadoId"], out int empId);
                int.TryParse(Request.QueryString["servicioId"], out int servId);

                if (empId == 0 || servId == 0)
                {
                    Response.Redirect("SeleccionarEmpleado.aspx");
                    return;
                }

                EmpleadoId = empId;
                ServicioId = servId;

                var servicioDto = servicioBO.obtenerPorId(servId);
                if (servicioDto != null)
                {
                    DuracionServicio = servicioDto.duracionHora * 60;
                }

                hdnEmpleadoId.Value = empId.ToString();
                hdnServicioId.Value = servId.ToString();
                hdnDuracionServicio.Value = DuracionServicio.ToString();

                CargarDisponibilidadEnCache();
                BindCalendar();
                ClearHoursDropDown(true);
            }
        }

        // ... (Los métodos auxiliares del calendario CargarDisponibilidadEnCache, CargarDatosRealesCalendario, BindCalendar, rpCalendarDays_ItemDataBound, GetAvailableHours, btnPrevMonth_Click, btnNextMonth_Click, ClearHoursDropDown SE MANTIENEN IGUAL) ...
        private void CargarDisponibilidadEnCache()
        {
            var calendarioEmpleado = calendarioBO.ListarCalendarioDeEmpleado(this.EmpleadoId);
            if (calendarioEmpleado != null)
                DisponibilidadCache = calendarioEmpleado.ToDictionary(c => c.fecha, c => c.cantLibre);
            else
                DisponibilidadCache = new Dictionary<DateTime, int>();
        }

        private List<DiaCalendario> CargarDatosRealesCalendario()
        {
            List<DiaCalendario> dias = new List<DiaCalendario>();
            DateTime primerDiaDelMes = CurrentDisplayMonth;
            int diasEnMes = DateTime.DaysInMonth(primerDiaDelMes.Year, primerDiaDelMes.Month);
            int primerDiaSemana = (int)primerDiaDelMes.DayOfWeek;
            DateTime hoy = DateTime.Today;
            var calendarioEmpleado = this.DisponibilidadCache;

            for (int i = 0; i < primerDiaSemana; i++)
                dias.Add(new DiaCalendario { Day = 0, Status = -1 });

            for (int i = 1; i <= diasEnMes; i++)
            {
                var dia = new DiaCalendario { Day = i };
                DateTime fechaActual = new DateTime(primerDiaDelMes.Year, primerDiaDelMes.Month, i);
                dia.FechaCompleta = fechaActual;
                if (fechaActual < hoy) dia.Status = 0;
                else if (calendarioEmpleado.ContainsKey(fechaActual.Date) && calendarioEmpleado[fechaActual.Date] > 0) dia.Status = 1;
                else dia.Status = 0;
                dias.Add(dia);
            }
            return dias;
        }

        private void BindCalendar()
        {
            litMonthYear.Text = CurrentDisplayMonth.ToString("MMMM yyyy").Replace("de", "").Replace(".", "");
            DateTime primerDiaMesActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (CurrentDisplayMonth <= primerDiaMesActual) { btnPrevMonth.Enabled = false; btnPrevMonth.CssClass = "nav-arrow nav-arrow--disabled"; }
            else { btnPrevMonth.Enabled = true; btnPrevMonth.CssClass = "nav-arrow"; }
            var dias = CargarDatosRealesCalendario();
            rpCalendarDays.DataSource = dias;
            rpCalendarDays.DataBind();
        }

        protected void rpCalendarDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dia = (DiaCalendario)e.Item.DataItem;
                var btnDay = (LinkButton)e.Item.FindControl("btnDay");
                if (dia.Status == -1) { btnDay.CssClass = "calendar-day--blank"; btnDay.Enabled = false; }
                else
                {
                    btnDay.Text = dia.Day.ToString();
                    btnDay.CssClass = "calendar-day";
                    if (dia.Status == 0) { btnDay.CssClass += " calendar-day--unavailable"; btnDay.Enabled = false; }
                    else btnDay.OnClientClick = $"handleDayClick(this, '{dia.FechaCompleta.ToString("yyyy-MM-dd")}'); return false;";

                    if (dia.FechaCompleta.ToString("yyyy-MM-dd") == hdnSelectedDay.Value) btnDay.CssClass += " calendar-day--selected";
                }
            }
        }

        [WebMethod]
        public static List<string> GetAvailableHours(string dateString, int empleadoId, int duracionServicio)
        {
            try
            {
                CalendarioBO bo = new CalendarioBO();
                DateTime fechaSeleccionada = DateTime.Parse(dateString);
                var bloques = bo.CalcularBloquesDisponibles(empleadoId, fechaSeleccionada, duracionServicio);
                return (bloques != null) ? bloques.ToList() : new List<string>();
            }
            catch { return new List<string>(); }
        }

        protected void btnPrevMonth_Click(object sender, EventArgs e) { CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(-1); hdnSelectedDay.Value = ""; ClearHoursDropDown(true); BindCalendar(); }
        protected void btnNextMonth_Click(object sender, EventArgs e) { CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(1); hdnSelectedDay.Value = ""; ClearHoursDropDown(true); BindCalendar(); }

        protected void btnCancelar_Click(object sender, EventArgs e) { Response.Redirect($"SeleccionarEmpleado.aspx?servicioId={this.ServicioId}"); }

        private void ClearHoursDropDown(bool estadoInicial) { ddlHorarios.Items.Clear(); ddlHorarios.Items.Add(new ListItem(estadoInicial ? "Seleccione una fecha primero" : "No hay horas disponibles", "")); ddlHorarios.Enabled = false; }

        private void MostrarAlerta(string mensaje) { ScriptManager.RegisterStartupScript(this, GetType(), "ReservaAlerta", $"alert('{mensaje.Replace("'", "\\'")}');", true); }

        // ===========================================================
        // 🔥 NUEVA LÓGICA DE PAGO Y RESERVA 🔥
        // ===========================================================

        // 1. Al hacer clic en "Reservar Cita", solo validamos y abrimos el Modal
        protected void btnReservar_Click(object sender, EventArgs e)
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
                return;
            }

            if (string.IsNullOrEmpty(hdnSelectedDay.Value)) { MostrarAlerta("Por favor, seleccione una fecha."); return; }
            if (string.IsNullOrEmpty(hdnSelectedHour.Value)) { MostrarAlerta("Por favor, seleccione una hora."); return; }

            // Si pasa las validaciones, mostramos el modal de pago
            ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenPayment", "var myModal = new bootstrap.Modal(document.getElementById('paymentModal')); myModal.show();", true);
        }

        // 2. Este evento se dispara cuando el usuario confirma el pago en el Modal
        protected void btnProcessPayment_Click(object sender, EventArgs e)
        {
            // Recoger valores del formulario (Pago)
            string cardNumber = Request.Form[txtCardNumber.UniqueID]?.Trim().Replace(" ", "") ?? "";

            // Simulación de pago (igual que en Carrito)
            if (cardNumber.EndsWith("0"))
            {
                // Fallo simulado
                string failScript = @"alert('El pago no pudo ser procesado. Verifica los datos de tu tarjeta.');
                                      setTimeout(function() { var modal = new bootstrap.Modal(document.getElementById('paymentModal')); modal.show(); }, 100);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PaymentFail", failScript, true);
                return;
            }

            // === PAGO EXITOSO: PROCEDEMOS A RESERVAR ===
            try
            {
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                DateTime fechaSeleccionada = DateTime.Parse(hdnSelectedDay.Value);
                DateTime horaSeleccionada = DateTime.Parse(hdnSelectedHour.Value, new CultureInfo("es-ES"));

                SoftInvBusiness.SoftInvWSCalendario.citaDTO nuevaCita = new SoftInvBusiness.SoftInvWSCalendario.citaDTO();
                nuevaCita.empleado = new SoftInvBusiness.SoftInvWSCalendario.empleadoDTO { idUsuario = this.EmpleadoId, idUsuarioSpecified = true };
                nuevaCita.servicio = new SoftInvBusiness.SoftInvWSCalendario.servicioDTO { idServicio = this.ServicioId, idServicioSpecified = true };
                nuevaCita.cliente = new SoftInvBusiness.SoftInvWSCalendario.clienteDTO { idUsuario = usuario.idUsuario, idUsuarioSpecified = true };

                nuevaCita.fecha = fechaSeleccionada;
                nuevaCita.fechaSpecified = true;
                nuevaCita.horaIni = horaSeleccionada.ToString("HH:mm:ss");
                nuevaCita.horaFin = horaSeleccionada.AddMinutes(this.DuracionServicio).ToString("HH:mm:ss");
                nuevaCita.activo = 1;
                nuevaCita.activoSpecified = true;
                nuevaCita.IGV = nuevaCita.servicio.precio * (0.18);
                nuevaCita.IGVSpecified = true;

                // Marca de pago (opcional, si tu citaDTO soporta código de transacción)
                // nuevaCita.codigoTransaccion = "PAY-" + Guid.NewGuid().ToString().Substring(0, 8);

                int resultado = calendarioBO.ReservarBloqueYCita(nuevaCita);

                // Mostrar modal de éxito y cerrar el de pago
                string successScript = @"
                    var paymentModal = bootstrap.Modal.getInstance(document.getElementById('paymentModal'));
                    if (paymentModal) { paymentModal.hide(); }
                    setTimeout(function() {
                        var successModal = new bootstrap.Modal(document.getElementById('paymentSuccessModal'));
                        successModal.show();
                    }, 300);";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", successScript, true);
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error al reservar: " + ex.Message);
            }
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            // Redirigir al perfil/reservas para ver la nueva cita
            Response.Redirect("~/Cliente/Perfil/Reservas.aspx");
        }
    }
}