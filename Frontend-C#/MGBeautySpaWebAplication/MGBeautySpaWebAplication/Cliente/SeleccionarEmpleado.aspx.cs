using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSEmpleado; // Para EmpleadoBO y empleadoDTO
using SoftInvBusiness.SoftInvWSServicio;  // Para ServicioBO
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
                    // Opcional: Cargar nombre del servicio para el título
                    // CargarInfoServicio(servicioId); 

                    CargarEmpleados(servicioId);
                }
                else
                {
                    Response.Redirect("~/Cliente/Servicios.aspx");
                }
            }
        }

        // --- 2. LÓGICA DINÁMICA ---
        private void CargarEmpleados(int servicioId)
        {
            // 1. Obtiene la lista cruda del WS
            var resultadoWS = servicioBO.empleadosPorServicio(servicioId);

            // 2. Verificamos si es NULL o si está VACÍA
            if (resultadoWS == null)
            {
                rpEmpleados.Visible = false;
                pnlNoEmpleados.Visible = true; 
            }
            else
            {
                // CASO CON DATOS:
                rpEmpleados.Visible = true;
                pnlNoEmpleados.Visible = false; 

                // 3. Mapeo a la clase local
                var listaEmpleadosDisplay = resultadoWS.Select(e => new EmpleadoDisplay
                {
                    Id = e.idUsuario,
                    Nombre = $"{e.nombre} {e.primerapellido}",
                    AvatarUrl = string.IsNullOrEmpty(e.urlFotoPerfil)
                                ? "~/Content/images/blank-photo.jpg" // Ruta segura por defecto
                                : e.urlFotoPerfil
                }).ToList();

                // 4. Enlazar datos
                rpEmpleados.DataSource = listaEmpleadosDisplay;
                rpEmpleados.DataBind();
            }
        }

        // --- 3. LÓGICA DE EVENTOS ---
        protected void rpEmpleados_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string commandArgument = e.CommandArgument.ToString();
                string[] args = commandArgument.Split('|');

                string empleadoId = args[0];
                string servicioId = Request.QueryString["servicioId"];

                Response.Redirect($"Calendario.aspx?empleadoId={empleadoId}&servicioId={servicioId}");
            }
        }
    }
}