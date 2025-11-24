using iTextSharp.text;
using iTextSharp.text.pdf;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSCalendario;
using SoftInvBusiness.SoftInvWSCita;
using SoftInvBusiness.SoftInvWSServicio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        private EmpleadoBO empleadoBO;
        private CitaBO citaBO;
        private const double TASA_IGV = 0.18;
        private const string correoEmpresa = "mgbeautyspa2025@gmail.com";
        private const string contraseñaApp = "beprxkazzucjiwom";

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
            empleadoBO = new EmpleadoBO();
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
                    DuracionServicio = servicioDto.duracionHora;
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

                if (fechaActual < hoy)
                {
                    dia.Status = 0;
                }
                else if (calendarioEmpleado.ContainsKey(fechaActual.Date))
                {
                    int cantidadLibreMinutos = calendarioEmpleado[fechaActual.Date];

                    if ((cantidadLibreMinutos - this.DuracionServicio) >= 0)
                    {
                        dia.Status = 1;
                    }
                    else
                    {
                        dia.Status = 0;
                    }
                }
                else
                {
                    dia.Status = 0;
                }

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

        private void ClearHoursDropDown(bool estadoInicial) { ddlHorarios.Items.Clear(); ddlHorarios.Items.Add(new System.Web.UI.WebControls.ListItem(estadoInicial ? "Seleccione una fecha primero" : "No hay horas disponibles", "")); ddlHorarios.Enabled = false; }

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

                SoftInvBusiness.SoftInvWSServicio.servicioDTO servicio = servicioBO.obtenerPorId(this.ServicioId);

                nuevaCita.fecha = fechaSeleccionada;
                nuevaCita.fechaSpecified = true;
                nuevaCita.horaIni = horaSeleccionada.ToString("HH:mm:ss");
                nuevaCita.horaFin = horaSeleccionada.AddMinutes(this.DuracionServicio*60).ToString("HH:mm:ss");
                nuevaCita.activo = 1;
                nuevaCita.activoSpecified = true;
                nuevaCita.IGV = servicio.precio * TASA_IGV;
                nuevaCita.IGVSpecified = true;
                nuevaCita.codigoTransaccion = "PAY-" + Guid.NewGuid().ToString().Substring(0, 8);

                
                int resultado = calendarioBO.ReservarBloqueYCita(nuevaCita);
                nuevaCita.id = resultado;
                nuevaCita.idSpecified = true;

                //Enviamos correo al cliente, boleta con datos de la reserva
                
                SoftInvBusiness.SoftInvWSEmpleado.empleadoDTO empleado = empleadoBO.ObtenerEmpleadoPorId(this.EmpleadoId);
                byte[] pdf = GenerarPdfReserva(nuevaCita,servicio,empleado);
                EnviarCorreoConPdf(
                    usuario.correoElectronico,
                    "Comprobante de tu reserva - MG Beauty SPA",
                    "¡Hola, " + usuario.nombre + "!\n¡Gracias por tu reserva! Adjuntamos el comprobante en PDF.",
                    pdf
                );

                //Enviamos correo al empleado asignado
                EnviarCorreoEmpleado(usuario,empleado,servicio,nuevaCita);

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

        private void EnviarCorreoEmpleado(SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario, SoftInvBusiness.SoftInvWSEmpleado.empleadoDTO empleado, SoftInvBusiness.SoftInvWSServicio.servicioDTO servicio, SoftInvBusiness.SoftInvWSCalendario.citaDTO nuevaCita)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(empleado.correoElectronico);
            mensaje.Subject = "Nueva Cita Registrada | MG Beauty SPA";
            mensaje.Body = "¡Hola, " + empleado.nombre + "!\n\n" +
                           "Un nuevo cliente ha registrado la siguiente cita:\n" + "Cliente: "+usuario.nombre+" "+usuario.primerapellido+" "+usuario.segundoapellido+"\nCorreo Electrónico: "+usuario.correoElectronico+
                           "\nCelular: "+usuario.celular+"\nServicio: "+servicio.nombre+"\nFecha: "+nuevaCita.fecha.ToString("D") +"\nHora Inicio: "+nuevaCita.horaIni+"\nHora Fin: "+nuevaCita.horaFin+"\n¡Conáctate si es necesario!";
            mensaje.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;

            smtp.Send(mensaje);
        }

        private byte[] GenerarPdfReserva(SoftInvBusiness.SoftInvWSCalendario.citaDTO cita, SoftInvBusiness.SoftInvWSServicio.servicioDTO servicio, SoftInvBusiness.SoftInvWSEmpleado.empleadoDTO empleado)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SoftInvBusiness.SoftInvWSUsuario.usuarioDTO usuario = (SoftInvBusiness.SoftInvWSUsuario.usuarioDTO)Session["UsuarioActual"];
                // Crear documento
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // === COLORES ===
                BaseColor verde = new BaseColor(0x14, 0x8C, 0x76);   // #148C76
                BaseColor blancoFondo = new BaseColor(0xF4, 0xFB, 0xF8); // #F4FBF8

                // === LOGO ===
                string rutaLogo = HttpContext.Current.Server.MapPath("~/Content/images/MGFavicon.png");
                if (File.Exists(rutaLogo))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                    logo.ScaleToFit(120, 120);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);
                }

                // Título
                Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, verde);
                Paragraph titulo = new Paragraph("Comprobante de Reserva - MG BEAUTY SPA", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingBefore = 10;
                titulo.SpacingAfter = 20;
                doc.Add(titulo);

                // LÍNEA SEPARADORA
                PdfPTable linea = new PdfPTable(1);
                linea.WidthPercentage = 100;
                PdfPCell cellSep = new PdfPCell(new Phrase(""))
                {
                    BackgroundColor = verde,
                    FixedHeight = 3,
                    Border = Rectangle.NO_BORDER
                };
                linea.AddCell(cellSep);
                doc.Add(linea);

                doc.Add(new Paragraph("\n"));

                // === INFORMACIÓN DEL CLIENTE ===
                Font label = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, verde);
                Font texto = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                doc.Add(new Paragraph("Cliente:", label));
                doc.Add(new Paragraph($"{usuario.nombre} {usuario.primerapellido} {usuario.segundoapellido}", texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Fecha de Pago:", label));
                doc.Add(new Paragraph(DateTime.Now.ToString(), texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Código de Transacción:", label));
                doc.Add(new Paragraph(cita.codigoTransaccion, texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Nro de cita:", label));
                doc.Add(new Paragraph(cita.id.ToString(), texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("¡Contáctate si es necesario!", texto));

                doc.Add(new Paragraph("Empleado a cargo:", label));
                doc.Add(new Paragraph(empleado.nombre+" "+empleado.primerapellido+" "+empleado.segundoapellido, texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Correo del empleado:", label));
                doc.Add(new Paragraph(empleado.correoElectronico, texto));
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph("Teléfono del empleado:", label));
                doc.Add(new Paragraph(empleado.celular, texto));
                doc.Add(new Paragraph("\n\n"));

                // ===============================================
                //              TABLA DE SERVICIO
                // ===============================================

                PdfPTable tabla = new PdfPTable(5);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 35, 20, 15, 15, 15 }); //  Servicio - Fecha - Hora Inicio - Hora Fin - Precio

                // Encabezados
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);

                string[] headers = { "Servicio", "Fecha", "Hora Inicio", "Hora Fin", "Precio" };
                foreach (var h in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(h, headerFont))
                    {
                        BackgroundColor = verde,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 8
                    };
                    tabla.AddCell(headerCell);
                }

                // Nombre
                tabla.AddCell(new PdfPCell(new Phrase(servicio.nombre, texto)) { Padding = 5 });

                //Fecha
                tabla.AddCell(new PdfPCell(new Phrase(cita.fecha.ToString("d/MM/yyyy"), texto)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5 });

                // Hora inicio
                tabla.AddCell(new PdfPCell(new Phrase(cita.horaIni.ToString(), texto))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                // Hora Fin
                tabla.AddCell(new PdfPCell(new Phrase(cita.horaFin.ToString(), texto))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                // Precio
                tabla.AddCell(new PdfPCell(new Phrase("S/ " + servicio.precio, texto))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                doc.Add(tabla);

                // ===============================================
                //                     TOTAL
                // ===============================================

                doc.Add(new Paragraph("\n"));

                PdfPTable tablaTotal = new PdfPTable(1);
                tablaTotal.WidthPercentage = 30;
                tablaTotal.HorizontalAlignment = Element.ALIGN_RIGHT;

                PdfPCell totalCell = new PdfPCell(new Phrase("Total: S/ " + servicio.precio,
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.WHITE)))
                {
                    BackgroundColor = verde,
                    Padding = 10,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                tablaTotal.AddCell(totalCell);

                PdfPCell igvCell = new PdfPCell(new Phrase("IGV: S/ " + servicio.precio * TASA_IGV,
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, BaseColor.BLACK)))
                {
                    BackgroundColor = blancoFondo,
                    Padding = 10,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

                tablaTotal.AddCell(igvCell);

                doc.Add(tablaTotal);

                // ESPACIO FINAL
                doc.Add(new Paragraph("\n\n¡Gracias por tu reserva en MG Beauty SPA!", texto));

                doc.Close();
                return ms.ToArray();
            }
        }

        private void EnviarCorreoConPdf(string correoDestino, string asunto, string cuerpo, byte[] pdfBytes)
        {
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(correoEmpresa);
            mensaje.To.Add(correoDestino);
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.IsBodyHtml = false;

            // Adjuntar PDF
            mensaje.Attachments.Add(new Attachment(
                new MemoryStream(pdfBytes),
                "ComprobanteReserva.pdf",
                "application/pdf"
            ));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(correoEmpresa, contraseñaApp);
            smtp.EnableSsl = true;
            smtp.Send(mensaje);
        }

        protected void btnVolverInicio_Click(object sender, EventArgs e)
        {
            // Redirigir al perfil/reservas para ver la nueva cita
            Response.Redirect("~/Cliente/Perfil/Reservas.aspx");
        }
    }
}