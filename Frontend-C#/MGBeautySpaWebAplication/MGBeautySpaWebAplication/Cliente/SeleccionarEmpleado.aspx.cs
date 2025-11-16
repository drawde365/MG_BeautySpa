using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSEmpleado; // Para EmpleadoBO y empleadoDTO
using SoftInvBusiness.SoftInvWSServicio;  // Para obtener el nombre del servicio
using System.Web;

namespace MGBeautySpaWebAplication.Cliente
{
    // --- 1. MODELO DE DATOS ---
    public class EmpleadoDisplay
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string AvatarUrl { get; set; }
    }

    public partial class SeleccionarEmpleado : System.Web.UI.Page
    {
        private EmpleadoBO empleadoBO;
        private ServicioBO servicioBO;

        public SeleccionarEmpleado()
        {
            empleadoBO = new EmpleadoBO();
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["servicioId"], out int servicioId))
                {
                    CargarEmpleados(servicioId);
                }
                else
                {
                    // Manejo de error si no hay ID de servicio
                    Response.Redirect("~/Cliente/Servicios.aspx");
                }
            }
        }

        // --- 2. LÓGICA DINÁMICA ---
        private void CargarEmpleados(int servicioId)
        {
            // 1. Obtiene la lista de empleados que ofrecen este servicio desde el BO
            // (Asumo que tu BO tiene el método ListarEmpleadosDeServicio, que creamos antes)
            var listaWSDTO = servicioBO.empleadosPorServicio(servicioId).ToList();

            // 2. Mapeo a la clase local de display (EmpleadoDisplay)
            var listaEmpleadosDisplay = listaWSDTO.Select(e => new EmpleadoDisplay
            {
                Id = e.idUsuario,
                Nombre = $"{e.nombre} {e.primerapellido}", // Combina Nombre y Apellido
                AvatarUrl = e.urlFotoPerfil ?? "/Content/Images/user_placeholder.png"
            }).ToList();

            // 3. Enlaza la lista al control Repeater
            rpEmpleados.DataSource = listaEmpleadosDisplay;
            rpEmpleados.DataBind();
        }

        // --- 3. LÓGICA DE EVENTOS ---
        protected void rpEmpleados_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // El CommandArgument debe tener el ID del empleado.
                string commandArgument = e.CommandArgument.ToString();
                string[] args = commandArgument.Split('|');

                string empleadoId = args[0];

                // Obtenemos el ID del servicio de la URL (lo necesitamos para la cita)
                string servicioId = Request.QueryString["servicioId"];

                // Redirige al calendario
                Response.Redirect($"Calendario.aspx?empleadoId={empleadoId}&servicioId={servicioId}");
            }
        }
    }
}