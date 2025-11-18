using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness; 
using SoftInvBusiness.SoftInvWSCita; 
using SoftInvBusiness.SoftInvWSUsuario; 
using System.Globalization;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class MisCitas : System.Web.UI.Page
    {
        private CitaBO citaBO;

        // Propiedad para almacenar la lista completa en Sesión
        private IList<citaDTO> ListaCompletaReservas
        {
            get { return (IList<citaDTO>)Session["ListaReservasEmpleado"]; }
            set { Session["ListaReservasEmpleado"] = value; }
        }

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
            ListaCompletaReservas = citaBO.ListarPorUsuario(user);

            // 1. Obtener todas las citas (solo si la caché está vacía)
            if (ListaCompletaReservas == null)
            {
                var reservas = citaBO.ListarPorUsuario(user);
                ListaCompletaReservas = (reservas != null) ? reservas.ToList() : new List<citaDTO>();
            }

            var todasLasCitas = ListaCompletaReservas;
            DateTime hoy = DateTime.Today;

            // 2. Filtrar las citas en tres categorías
            var proximas = todasLasCitas.Where(c => 
                c.activo == 1 && 
                c.fechaSpecified && c.fecha.Date >= hoy
            ).OrderBy(c => c.fecha).ThenBy(c => c.horaIni).ToList();
            
            var pasadas = todasLasCitas.Where(c => 
                c.activo == 1 && 
                c.fechaSpecified && c.fecha.Date < hoy
            ).OrderByDescending(c => c.fecha).ToList();
            
            var canceladas = todasLasCitas.Where(c => c.activo == 0)
                                        .OrderByDescending(c => c.fecha)
                                        .ToList();

            // 3. Enlazar "Próximas"
            if (proximas.Any())
            {
                rptProximas.DataSource = MapearCitas(proximas);
                rptProximas.DataBind();
                pnlNoProximas.Visible = false;
            }
            else
            {
                rptProximas.DataSource = null;
                rptProximas.DataBind();
                pnlNoProximas.Visible = true;
            }

            // 4. Enlazar "Pasadas"
            if (pasadas.Any())
            {
                rptPasadas.DataSource = MapearCitas(pasadas);
                rptPasadas.DataBind();
                pnlNoPasadas.Visible = false;
            }
            else
            {
                rptPasadas.DataSource = null;
                rptPasadas.DataBind();
                pnlNoPasadas.Visible = true;
            }

            // 5. Enlazar "Canceladas"
            if (canceladas.Any())
            {
                rptCanceladas.DataSource = MapearCitas(canceladas);
                rptCanceladas.DataBind();
                pnlNoCanceladas.Visible = false;
            }
            else
            {
                rptCanceladas.DataSource = null;
                rptCanceladas.DataBind();
                pnlNoCanceladas.Visible = true;
            }
        }

        private object MapearCitas(List<citaDTO> citas)
        {
            var culturaES = new CultureInfo("es-ES");
            
            return citas.Select(c => new {
                CitaId = c.id,
                ClienteNombre = $"{c.cliente.nombre} {c.cliente.primerapellido}",
                ClienteCelular = c.cliente.celular ?? "N/A",
                ServicioNombre = c.servicio.nombre,
                Fecha = c.fechaSpecified ? c.fecha.ToString("dd/MM/yyyy") : "N/A",
                HoraInicio = !string.IsNullOrEmpty(c.horaIni) 
                             ? DateTime.Parse(c.horaIni, culturaES).ToString("hh:mm tt", culturaES) 
                             : "N/A",
                // Guardamos la duración para recalcular la horaFin al modificar
                DuracionServicio = c.servicio.duracionHoraSpecified ? c.servicio.duracionHora * 60 : 60,
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

        // --- ▼▼▼ NUEVA FUNCIÓN PARA ABRIR EL MODAL ▼▼▼ ---
        protected void rptProximas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                if (!int.TryParse(e.CommandArgument.ToString(), out int citaId))
                {
                    return; 
                }

                // 1. Obtener la cita de la caché
                var cita = ListaCompletaReservas.FirstOrDefault(c => c.id == citaId);
                if (cita == null) return;

                // 2. Poblar el modal
                hdnCitaIdModal.Value = cita.id.ToString();
                txtNuevaFecha.Text = cita.fecha.ToString("yyyy-MM-dd");
                txtNuevaHora.Text = TimeSpan.Parse(cita.horaIni).ToString(@"hh\:mm"); // Formato HH:mm para el control <input type="time">

                // 3. Abrir el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModificarModal",
                    "var myModal = new bootstrap.Modal(document.getElementById('modificarCitaModal')); myModal.show();", 
                    true);
            }
        }

        // --- ▼▼▼ NUEVA FUNCIÓN PARA GUARDAR DESDE EL MODAL ▼▼▼ ---
        protected void btnGuardarCambiosCita_Click(object sender, EventArgs e)
        {
            try
            {
                int citaId = int.Parse(hdnCitaIdModal.Value);
                
                // 1. Validar y parsear las nuevas entradas
                if (!DateTime.TryParse(txtNuevaFecha.Text, out DateTime nuevaFecha))
                {
                    // Mostrar error (opcional)
                    return;
                }
                if (!TimeSpan.TryParse(txtNuevaHora.Text, out TimeSpan nuevaHora))
                {
                    // Mostrar error (opcional)
                    return;
                }

                // 2. Obtener la cita de la caché
                var citaParaModificar = ListaCompletaReservas.FirstOrDefault(c => c.id == citaId);
                if (citaParaModificar == null) return;

                // 3. (IMPORTANTE) Lógica de validación de disponibilidad
                // Aquí deberías llamar a tu CalendarioBO.CalcularBloquesDisponibles
                // para asegurarte de que la nueva 'nuevaHora' es válida para la 'nuevaFecha'.
                // Por ahora, asumimos que es válida y procedemos a guardar.

                // 4. Actualizar el objeto DTO
                citaParaModificar.fecha = nuevaFecha;
                citaParaModificar.fechaSpecified = true;

                citaParaModificar.horaIni = nuevaHora.ToString();
                
                // Recalcular horaFin
                int duracion = citaParaModificar.servicio.duracionHoraSpecified ? citaParaModificar.servicio.duracionHora * 60 : 60;
                citaParaModificar.horaFin = nuevaHora.Add(TimeSpan.FromMinutes(duracion)).ToString();

                // 5. Guardar en la BD
                citaBO.ModificarCita(citaParaModificar);

                // 6. Limpiar caché de sesión y recargar todo
                ListaCompletaReservas = null;
                CargarCitas();

                // 7. Cerrar el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideModificarModal",
                    "var myModal = bootstrap.Modal.getInstance(document.getElementById('modificarCitaModal')); myModal.hide();", 
                    true);
            }
            catch (Exception ex)
            {
                // Manejar error (ej. mostrar un literal en el modal)
            }
        }
    }
}