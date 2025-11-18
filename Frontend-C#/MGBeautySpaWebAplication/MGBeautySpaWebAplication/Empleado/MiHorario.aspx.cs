using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using SoftInvBusiness.SoftInvWSCalendario; // Para CalendarioBO y DTOs
using SoftInvBusiness.SoftInvWSUsuario; // Para usuarioDTO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class MiHorario : System.Web.UI.Page
    {
        private HorarioTrabajoBO horarioTrabajoBO;
        private CalendarioBO calendarioBO; // Nuevo BO

        public MiHorario()
        {
            horarioTrabajoBO = new HorarioTrabajoBO();
            calendarioBO = new CalendarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MostrarMensajeFlash();
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

                if (usuario != null)
                {
                    CargarHorario(usuario.idUsuario);
                    CargarExcepciones(usuario.idUsuario); // Cargar lista inferior
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        // --- PARTE 1: HORARIO SEMANAL (GRILLA) ---
        private void CargarHorario(int empleadoId)
        {
            var horarioMap = new Dictionary<int, HorarioRow>();
            for (int i = 8; i <= 20; i++)
            {
                horarioMap[i] = new HorarioRow
                {
                    Hora = $"{i:00}:00",
                    Lunes = false,
                    Martes = false,
                    Miercoles = false,
                    Jueves = false,
                    Viernes = false,
                    Sabado = false
                };
            }

            var horarioDesdeSOAP = horarioTrabajoBO.ListarHorarioDeEmpleado(empleadoId);
            List<horarioTrabajoDTO> horarioArrayList = (horarioDesdeSOAP != null) ? horarioDesdeSOAP.ToList() : new List<horarioTrabajoDTO>();

            foreach (horarioTrabajoDTO bloque in horarioArrayList)
            {
                if (bloque.horaInicio == null || bloque.horaFin == null) continue;

                string horaInicioString = bloque.horaInicio.ToString();
                string horaFinString = bloque.horaFin.ToString();

                if (string.IsNullOrEmpty(horaInicioString) || string.IsNullOrEmpty(horaFinString)) continue;

                TimeSpan tsInicio, tsFin;
                if (!TimeSpan.TryParse(horaInicioString, CultureInfo.InvariantCulture, out tsInicio) ||
                    !TimeSpan.TryParse(horaFinString, CultureInfo.InvariantCulture, out tsFin))
                {
                    continue;
                }

                int horaInicio = tsInicio.Hours;
                int horaFin = tsFin.Hours;

                for (int horaActual = horaInicio; horaActual < horaFin; horaActual++)
                {
                    if (horarioMap.ContainsKey(horaActual))
                    {
                        HorarioRow filaDeLaHora = horarioMap[horaActual];
                        switch (bloque.diaSemana)
                        {
                            case 1: filaDeLaHora.Lunes = true; break;
                            case 2: filaDeLaHora.Martes = true; break;
                            case 3: filaDeLaHora.Miercoles = true; break;
                            case 4: filaDeLaHora.Jueves = true; break;
                            case 5: filaDeLaHora.Viernes = true; break;
                            case 6: filaDeLaHora.Sabado = true; break;
                        }
                    }
                }
            }

            List<HorarioRow> listaHorario = horarioMap.Values
                .OrderBy(h => int.Parse(h.Hora.Split(':')[0]))
                .ToList();

            rptHorario.DataSource = listaHorario;
            rptHorario.DataBind();
        }

        // --- PARTE 2: LISTA DE EXCEPCIONES ---
        private void CargarExcepciones(int empleadoId)
        {
            // Obtenemos todo el calendario generado
            var listaCalendario = calendarioBO.ListarCalendarioDeEmpleado(empleadoId);

            if (listaCalendario != null && listaCalendario.Count > 0)
            {
                // Filtramos: Días con (CantLibre <= 0) Y que tengan un motivo (para distinguir de días libres normales o llenos)
                // Asumimos que si tiene motivo, es una excepción manual.
                var listaExcepciones = listaCalendario
                    .Where(c => c.cantLibre <= 0 && !string.IsNullOrEmpty(c.motivo))
                    .OrderBy(c => c.fecha)
                    .ToList();

                if (listaExcepciones.Count > 0)
                {
                    rptExcepciones.DataSource = listaExcepciones;
                    rptExcepciones.DataBind();
                    pnlNoExcepciones.Visible = false;
                }
                else
                {
                    rptExcepciones.DataSource = null;
                    rptExcepciones.DataBind();
                    pnlNoExcepciones.Visible = true;
                }
            }
            else
            {
                pnlNoExcepciones.Visible = true;
            }
        }

        // --- PARTE 3: REHABILITAR (ELIMINAR EXCEPCIÓN) ---
        protected void rptExcepciones_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Rehabilitar")
            {
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario == null) return;

                string fechaStr = e.CommandArgument.ToString();
                DateTime fecha = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                // 1. Necesitamos saber cuántas horas libres le tocan ese día normalmente
                int horasNormales = ObtenerHorasNormalesDelDia(usuario.idUsuario, fecha);

                // 2. Actualizamos el calendario para borrar la excepción
                calendarioDTO calendarioRestaurar = new calendarioDTO();
                calendarioRestaurar.empleado = new SoftInvBusiness.SoftInvWSCalendario.empleadoDTO { idUsuario = usuario.idUsuario, idUsuarioSpecified = true };
                calendarioRestaurar.fecha = fecha;
                calendarioRestaurar.fechaSpecified = true;

                // Restauramos las horas y borramos el motivo
                calendarioRestaurar.cantLibre = horasNormales;
                calendarioRestaurar.cantLibreSpecified = true;
                calendarioRestaurar.motivo = null; // O cadena vacía

                // Usamos modificar para "pisar" la excepción con los datos limpios
                int resultado = calendarioBO.ModificarCalendario(calendarioRestaurar);

                if (resultado > 0)
                {
                    MostrarExito("Excepción eliminada. Tu horario para el " + fecha.ToShortDateString() + " ha sido rehabilitado.");
                    CargarExcepciones(usuario.idUsuario); // Recargar tabla
                }
                else
                {
                    // Manejo de error simple (puedes usar un label de error)
                }
            }
        }

        // Método auxiliar para saber cuántas horas debe tener el empleado un día X
        private int ObtenerHorasNormalesDelDia(int empleadoId, DateTime fecha)
        {
            // 1. Obtenemos el horario base
            var horarioBase = horarioTrabajoBO.ListarHorarioDeEmpleado(empleadoId); // Usando el método del BO de HorarioTrabajo

            // 2. Calculamos el día de la semana (1=Lunes, 7=Domingo)
            int diaSemana = (int)fecha.DayOfWeek;
            if (diaSemana == 0) diaSemana = 7;

            int totalHoras = 0;
            if (horarioBase != null)
            {
                foreach (var h in horarioBase)
                {
                    if (h.diaSemana == diaSemana)
                    {
                        // Sumamos los intervalos de ese día
                        totalHoras += h.numIntervalo;
                    }
                }
            }
            return totalHoras;
        }

        // --- UTILIDADES ---
        protected string GetCellClass(bool isOccupied)
        {
            return isOccupied ? "cell-occupied" : "cell-free";
        }

        protected void MostrarMensajeFlash()
        {
            if (Session["FlashMessage"] != null)
            {
                litAlertMessage.Text = Session["FlashMessage"].ToString();
                pnlAlert.CssClass = "alert alert-success alert-dismissible fade show mb-3";
                pnlAlert.Visible = true;
                Session["FlashMessage"] = null;
            }
        }

        private void MostrarExito(string mensaje)
        {
            litAlertMessage.Text = mensaje;
            pnlAlert.CssClass = "alert alert-success alert-dismissible fade show mb-3";
            pnlAlert.Visible = true;
        }

        public class HorarioRow
        {
            public string Hora { get; set; }
            public bool Lunes { get; set; }
            public bool Martes { get; set; }
            public bool Miercoles { get; set; }
            public bool Jueves { get; set; }
            public bool Viernes { get; set; }
            public bool Sabado { get; set; }
        }
    }
}