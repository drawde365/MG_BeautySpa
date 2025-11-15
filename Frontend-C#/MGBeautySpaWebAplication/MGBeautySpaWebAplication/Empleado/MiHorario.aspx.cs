using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class MiHorario : System.Web.UI.Page
    {

        public MiHorario()
        {
            horarioTrabajoBO = new HorarioTrabajoBO();
        }

        private HorarioTrabajoBO horarioTrabajoBO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Revisa si hay un mensaje "flash" ANTES de cargar el horario
                MostrarMensajeFlash();
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                CargarHorario(usuario.idUsuario);
            }
        }

        protected void MostrarMensajeFlash()
        {
            // 1. Revisa si hay un mensaje en la Sesión
            if (Session["FlashMessage"] != null)
            {
                litAlertMessage.Text = Session["FlashMessage"].ToString();
                pnlAlert.CssClass = "alert alert-success alert-dismissible fade show";
                pnlAlert.Visible = true;
                Session["FlashMessage"] = null;
            }
        }

        private void CargarHorario(int empleadoId)
        {
            // Usamos un Diccionario para acceso rápido (ej. buscar "9:00")
            var horarioMap = new Dictionary<int, HorarioRow>();
            for (int i = 8; i <= 20; i++) // De 8:00 a 20:00
            {
                horarioMap[i] = new HorarioRow
                {
                    Hora = $"{i}:00",
                    Lunes = false,
                    Martes = false,
                    Miercoles = false,
                    Jueves = false,
                    Viernes = false,
                    Sabado = false
                };
            }

            // --- 2. OBTENER LOS BLOQUES OCUPADOS DE LA BD ---
            List<horarioTrabajoDTO> horarioArrayList = horarioTrabajoBO.ListarHorarioDeEmpleado(empleadoId).ToList();

            // --- 3. "PINTAR" LA GRILLA CON LOS BLOQUES OCUPADOS ---
            foreach (horarioTrabajoDTO bloque in horarioArrayList)
            {
                string horaInicioString = bloque.horaInicio;
                string horaFinString = bloque.horaFin;

                TimeSpan tsInicio = TimeSpan.Parse(horaInicioString);
                TimeSpan tsFin = TimeSpan.Parse(horaFinString);

                if (string.IsNullOrEmpty(horaInicioString) || string.IsNullOrEmpty(horaFinString))
                {
                    continue;
                }

                int horaInicio = tsInicio.Hours;
                int horaFin = tsFin.Hours;

                // Recorremos cada hora dentro del bloque (ej. 9, 10, 11, 12)
                for (int horaActual = horaInicio; horaActual < horaFin; horaActual++)
                {
                    // Verificamos que la hora exista en nuestra grilla (8-20)
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

            // --- 4. ENLAZAR LA GRILLA YA "PINTADA" AL REPEATER ---

            // Convertimos el Diccionario a una Lista y la ordenamos por hora
            List<HorarioRow> listaHorario = horarioMap.Values
                                                .OrderBy(h => int.Parse(h.Hora.Split(':')[0]))
                                                .ToList();

            rptHorario.DataSource = listaHorario;
            rptHorario.DataBind();
        }

        protected string GetCellClass(bool isOccupied)
        {
            return isOccupied ? "cell-occupied" : "cell-free";
        }
    }
}