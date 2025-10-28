using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

// Asegúrate de que este "namespace" coincida con el de tu proyecto
namespace MGBeautySpaWebAplication.Cliente
{
    // --- 1. MODELO DE DATOS ---
    // Esta clase representa cada día en el calendario
    public class DiaCalendario
    {
        public int Day { get; set; }        // El número del día (1-31)
        public int Status { get; set; }     // -1 = Blanco, 0 = No disponible, 1 = Disponible
        public List<string> Horarios { get; set; }
    }

    public partial class Calendario : System.Web.UI.Page
    {
        // --- 2. ESTADO DE LA PÁGINA ---
        // Usamos ViewState para recordar en qué mes estamos navegando
        private DateTime CurrentDisplayMonth
        {
            get
            {
                if (ViewState["CurrentDisplayMonth"] == null)
                {
                    // CAMBIO: Ahora el calendario EMPIEZA en el mes actual real,
                    // no en "Julio 2025".
                    ViewState["CurrentDisplayMonth"] = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                }
                return (DateTime)ViewState["CurrentDisplayMonth"];
            }
            set
            {
                ViewState["CurrentDisplayMonth"] = value;
            }
        }
        
        // Simulación de "hoy" para la lógica de días pasados
        private DateTime SimuladoHoy = new DateTime(2025, 7, 4);


        // --- 3. EVENTOS DE PÁGINA ---
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCalendar();
                ClearHoursDropDown(true); 
            }
        }

        // --- 4. SIMULACIÓN DE DATOS ---
        /// <summary>
        /// Genera la lista de días para el mes actual, incluyendo días no disponibles.
        /// </summary>
        private List<DiaCalendario> SimularDatosCalendario()
        {
            List<DiaCalendario> dias = new List<DiaCalendario>();
            
            DateTime primerDiaDelMes = CurrentDisplayMonth;
            int diasEnMes = DateTime.DaysInMonth(primerDiaDelMes.Year, primerDiaDelMes.Month);
            int primerDiaSemana = (int)primerDiaDelMes.DayOfWeek; // 0=Domingo
            
            // CAMBIO: Usamos el "hoy" real.
            DateTime hoy = DateTime.Today;

            // 1. Agregar días en blanco al inicio
            for (int i = 0; i < primerDiaSemana; i++)
            {
                dias.Add(new DiaCalendario { Day = 0, Status = -1 }); // -1 = Blanco
            }

            // 2. Generar datos para cada día del mes
            for (int i = 1; i <= diasEnMes; i++)
            {
                var dia = new DiaCalendario { Day = i };
                DateTime fechaActual = new DateTime(primerDiaDelMes.Year, primerDiaDelMes.Month, i);

                // LÓGICA (Req 2): Días anteriores a "hoy" no se pueden seleccionar
                if (fechaActual < hoy)
                {
                    dia.Status = 0; // 0 = No disponible
                }
                // LÓGICA: Simular otros días no disponibles (ej. cada 5to día)
                else if (i % 5 == 0) 
                {
                    dia.Status = 0;
                }
                // LÓGICA: Día disponible
                else
                {
                    dia.Status = 1;
                    // ... (la lógica de generar horarios sigue igual) ...
                    dia.Horarios = new List<string>
                    {
                        "09:00 AM", "10:00 AM", "11:00 AM", "02:00 PM", "03:00 PM", "04:00 PM"
                    };
                    if (i % 3 == 0)
                    {
                        dia.Horarios = dia.Horarios.Take(2).ToList();
                    }
                }
                dias.Add(dia);
            }
            return dias;
        }

        // --- 5. RENDERIZADO DEL CALENDARIO ---
        
        /// <summary>
        /// Dibuja el calendario en la página.
        /// </summary>
        private void BindCalendar()
        {
            // Pone el título (ej. "octubre 2025")
            litMonthYear.Text = CurrentDisplayMonth.ToString("MMMM yyyy").Replace("de", "").Replace(".", "");

            // --- INICIO DE NUEVA LÓGICA (Req 1) ---
            // Deshabilitar el botón "anterior" si estamos en el mes actual
            DateTime primerDiaMesActual = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            
            if (CurrentDisplayMonth <= primerDiaMesActual)
            {
                btnPrevMonth.Enabled = false;
                // Añadimos una clase CSS para que se vea gris
                btnPrevMonth.CssClass = "nav-arrow nav-arrow--disabled"; 
            }
            else
            {
                btnPrevMonth.Enabled = true;
                btnPrevMonth.CssClass = "nav-arrow";
            }
            var dias = SimularDatosCalendario();
            rpCalendarDays.DataSource = dias;
            rpCalendarDays.DataBind();
        }

        /// <summary>
        /// Este evento se dispara para CADA día que se dibuja en el Repeater.
        /// Aquí es donde aplicamos los estilos (colores).
        /// </summary>
        protected void rpCalendarDays_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var dia = (DiaCalendario)e.Item.DataItem;
                var btnDay = (LinkButton)e.Item.FindControl("btnDay");

                if (dia.Status == -1) // Es un día en blanco
                {
                    btnDay.CssClass = "calendar-day--blank";
                    btnDay.Enabled = false;
                }
                else
                {
                    btnDay.Text = dia.Day.ToString();
                    btnDay.CommandName = "SelectDay";
                    btnDay.CommandArgument = dia.Day.ToString();
                    btnDay.CssClass = "calendar-day";

                    // LÓGICA: Aplicar color gris (no disponible)
                    if (dia.Status == 0)
                    {
                        btnDay.CssClass += " calendar-day--unavailable";
                        btnDay.Enabled = false;
                    }

                    // LÓGICA: Aplicar color verde (seleccionado)
                    if (dia.Day.ToString() == hdnSelectedDay.Value)
                    {
                        btnDay.CssClass += " calendar-day--selected";
                    }
                }
            }
        }
        
        /// <summary>
        /// Se dispara Clic en un día del calendario.
        /// </summary>
        protected void rpCalendarDays_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectDay")
            {
                // 1. Guardar el día seleccionado
                hdnSelectedDay.Value = e.CommandArgument.ToString();
                
                // 2. Cargar las horas para ese día
                int diaNum = Convert.ToInt32(hdnSelectedDay.Value);
                var dias = SimularDatosCalendario(); // Re-obtener datos para buscar las horas
                var diaSeleccionado = dias.FirstOrDefault(d => d.Day == diaNum);

                if (diaSeleccionado != null && diaSeleccionado.Horarios != null && diaSeleccionado.Horarios.Any())
                {
                    ddlHorarios.DataSource = diaSeleccionado.Horarios;
                    ddlHorarios.DataBind();
                    ddlHorarios.Items.Insert(0, new ListItem("Seleccione un horario", ""));
                    ddlHorarios.Enabled = true;
                }
                else
                {
                    ClearHoursDropDown(false);
                }

                // 3. Volver a dibujar el calendario para que se muestre el color verde
                BindCalendar();
            }
        }


        // --- 6. EVENTOS DE BOTONES ---
        
        // Navegación de Mes
        protected void btnPrevMonth_Click(object sender, EventArgs e)
        {
            CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(-1);
            hdnSelectedDay.Value = ""; // Limpiar selección
            ClearHoursDropDown(true);
            BindCalendar();
        }

        protected void btnNextMonth_Click(object sender, EventArgs e)
        {
            CurrentDisplayMonth = CurrentDisplayMonth.AddMonths(1);
            hdnSelectedDay.Value = ""; // Limpiar selección
            ClearHoursDropDown(true);
            BindCalendar();
        }
        
        // Acciones
        protected void btnReservar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hdnSelectedDay.Value))
            {
                MostrarAlerta("Por favor, seleccione una fecha.");
                return;
            }
            if (string.IsNullOrEmpty(ddlHorarios.SelectedValue))
            {
                MostrarAlerta("Por favor, seleccione una hora.");
                return;
            }

            string fecha = $"{hdnSelectedDay.Value} de {litMonthYear.Text}";
            string hora = ddlHorarios.SelectedValue;

            MostrarAlerta($"¡Cita reservada!\\nFecha: {fecha}\\nHora: {hora}");
            
            // Aquí iría tu lógica para guardar en la BD
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Vuelve a la página anterior (Seleccionar Empleado)
            Response.Redirect("SeleccionarEmpleado.aspx"); 
        }

        // --- 7. MÉTODOS AYUDANTES ---
        
        /// <summary>
        /// Limpia el DropDownList de horarios
        /// </summary>
        private void ClearHoursDropDown(bool estadoInicial)
        {
            ddlHorarios.Items.Clear();
            if (estadoInicial)
            {
                ddlHorarios.Items.Add(new ListItem("Seleccione una fecha primero", ""));
            }
            else
            {
                ddlHorarios.Items.Add(new ListItem("No hay horas disponibles", ""));
            }
            ddlHorarios.Enabled = false;
        }

        /// <summary>
        /// Muestra una alerta (popup) de JavaScript desde C#.
        /// </summary>
        private void MostrarAlerta(string mensaje)
        {
            // El UpdatePanel requiere usar ScriptManager.RegisterStartupScript
            ScriptManager.RegisterStartupScript(this, GetType(), "ReservaAlerta", $"alert('{mensaje}');", true);
        }
    }
}