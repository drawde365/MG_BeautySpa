using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness; // Para los BOs
using SoftInvBusiness.SoftInvWSCita; // Para citaDTO
using SoftInvBusiness.SoftInvWSUsuario; // Para usuarioDTO
using System.Globalization;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class MisCitas : System.Web.UI.Page
    {
        private CitaBO citaBO;

        public MisCitas()
        {
            citaBO = new CitaBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

            // 1. Obtener todas las citas del empleado desde el BO
            var todasLasCitas = citaBO.ListarPorUsuario(user);
            if (todasLasCitas == null)
            {
                todasLasCitas = new citaDTO[0]; // Inicializar lista vacía
            }

            DateTime hoy = DateTime.Today;

            // 2. Filtrar las citas en tres categorías
            var proximas = todasLasCitas.Where(c =>
                c.activo == 1 &&
                c.fechaSpecified && c.fecha.Date >= hoy
            ).OrderBy(c => c.fecha).ToList();

            var pasadas = todasLasCitas.Where(c =>
                c.activo == 1 &&
                c.fechaSpecified && c.fecha.Date < hoy
            ).OrderByDescending(c => c.fecha).ToList();

            var canceladas = todasLasCitas.Where(c => c.activo == 0)
                                        .OrderByDescending(c => c.fecha)
                                        .ToList();

            // 3. Mapear y Enlazar "Próximas"
            if (proximas.Any())
            {
                rptProximas.DataSource = MapearCitas(proximas);
                rptProximas.DataBind();
                pnlNoProximas.Visible = false;
            }
            else
            {
                rptProximas.Visible = false;
                pnlNoProximas.Visible = true;
            }

            // 4. Mapear y Enlazar "Pasadas"
            if (pasadas.Any())
            {
                rptPasadas.DataSource = MapearCitas(pasadas);
                rptPasadas.DataBind();
                pnlNoPasadas.Visible = false;
            }
            else
            {
                rptPasadas.Visible = false;
                pnlNoPasadas.Visible = true;
            }

            // 5. Mapear y Enlazar "Canceladas"
            if (canceladas.Any())
            {
                rptCanceladas.DataSource = MapearCitas(canceladas);
                rptCanceladas.DataBind();
                pnlNoCanceladas.Visible = false;
            }
            else
            {
                rptCanceladas.Visible = false;
                pnlNoCanceladas.Visible = true;
            }
        }

        /// <summary>
        /// Convierte el DTO del WS a un objeto simple para el Repeater.
        /// </summary>
        private object MapearCitas(List<citaDTO> citas)
        {
            var culturaES = new CultureInfo("es-ES");

            return citas.Select(c => new {
                NumeroReserva = c.id.ToString("D3"),
                ClienteNombre = $"{c.cliente.nombre} {c.cliente.primerapellido}",
                ClienteCelular = c.cliente.celular ?? "N/A",
                ServicioNombre = c.servicio.nombre,
                Fecha = c.fecha.ToString("dd/MM/yyyy HH:mm:ss"),
                // Asumiendo que 'horaIni' es un string "HH:mm:ss" (por el TimeAdapter de Java)
                HoraInicio = TimeSpan.Parse(c.horaIni).ToString(@"hh\:mm"),
                Activo = c.activo,
                FechaCita = c.fecha
            }).ToList();
        }

        /// <summary>
        /// Da estilo al badge de estado (Completado, Pendiente, Cancelado)
        /// </summary>
        protected void rptCitas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cita = e.Item.DataItem;
                var litEstado = (Literal)e.Item.FindControl("litEstado");

                // Extraer los valores del objeto anónimo
                int activo = (int)DataBinder.Eval(cita, "Activo");
                DateTime fechaCita = (DateTime)DataBinder.Eval(cita, "FechaCita");

                if (activo == 0)
                {
                    litEstado.Text = "<span class=\"badge rounded-pill text-white estado-badge\" style=\"background-color: #C31E1E;\">Cancelado</span>";
                }
                else if (fechaCita < DateTime.Today)
                {
                    litEstado.Text = "<span class=\"badge rounded-pill text-white estado-badge\" style=\"background-color: #148C76;\">Completado</span>";
                }
                else
                {
                    litEstado.Text = "<span class=\"badge rounded-pill text-white estado-badge\" style=\"background-color: #A6A6A6;\">Pendiente</span>";
                }
            }
        }
    }
}