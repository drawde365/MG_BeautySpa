using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSHorarioTrabajo;
using System;
using System.Collections;
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
        public MiHorario()
        {
            horarioTrabajoBO = new HorarioTrabajoBO();
        }

        private HorarioTrabajoBO horarioTrabajoBO;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MostrarMensajeFlash();
                var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
                if (usuario != null)
                {
                    CargarHorario(usuario.idUsuario);
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }

        protected void MostrarMensajeFlash()
        {
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
            // const string TimeFormat = "HH:mm:ss"; <-- YA NO ES NECESARIO

            var horarioMap = new Dictionary<int, HorarioRow>();
            for (int i = 8; i <= 20; i++)
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

            var horarioDesdeSOAP = horarioTrabajoBO.ListarHorarioDeEmpleado(empleadoId);
            List<horarioTrabajoDTO> horarioArrayList = new List<horarioTrabajoDTO>();
            if (horarioDesdeSOAP != null)
            {
                horarioArrayList = horarioDesdeSOAP.ToList();
            }

            foreach (horarioTrabajoDTO bloque in horarioArrayList)
            {
                // 1. Validamos que los objetos existan (es la validación más fiable)
                if (bloque.horaInicio == null || bloque.horaFin == null)
                {
                    continue;
                }

                string horaInicioString = bloque.horaInicio.ToString();
                string horaFinString = bloque.horaFin.ToString();

                if (string.IsNullOrEmpty(horaInicioString) || string.IsNullOrEmpty(horaFinString))
                {
                    continue;
                }

                // --- CORRECCIÓN FINAL ---
                // Intentamos parsear usando TryParse con cultura Invariante,
                // ya que no podemos confiar en el formato exacto del ToString() del proxy.
                TimeSpan tsInicio, tsFin;

                if (!TimeSpan.TryParse(horaInicioString, CultureInfo.InvariantCulture, out tsInicio) ||
                    !TimeSpan.TryParse(horaFinString, CultureInfo.InvariantCulture, out tsFin))
                {
                    // Si el parseo falla, salta este bloque de horario
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

        protected string GetCellClass(bool isOccupied)
        {
            return isOccupied ? "cell-occupied" : "cell-free";
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