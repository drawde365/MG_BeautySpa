using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSEmpleado;
using SoftInvBusiness.SoftInvWSServicio;
using System.Web;

namespace MGBeautySpaWebAplication.Cliente
{
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
                    Response.Redirect("~/Cliente/Servicios.aspx");
                }
            }
        }

        private void CargarEmpleados(int servicioId)
        {
            var listaWSDTO = servicioBO.empleadosPorServicio(servicioId).ToList();

            if (listaWSDTO == null || !listaWSDTO.Any())
            {
                // Manejo si no hay empleados (opcional)
                // lblMensaje.Text = "No hay empleados disponibles para este servicio."
                // return;
            }

            var listaEmpleadosDisplay = listaWSDTO.Select(e => new EmpleadoDisplay
            {
                Id = e.idUsuario,
                Nombre = $"{e.nombre} {e.primerapellido}",
                AvatarUrl = ResolveUrl(e.urlFotoPerfil ?? "~/Content/Images/user_placeholder.png")
            }).ToList();

            rpEmpleados.DataSource = listaEmpleadosDisplay;
            rpEmpleados.DataBind();
        }

        protected void rpEmpleados_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string commandArgument = e.CommandArgument.ToString();
                string[] args = commandArgument.Split('|');

                string empleadoId = args[0]; // <-- CORRECCIÓN: Usar solo el ID

                string servicioId = Request.QueryString["servicioId"];

                Response.Redirect($"Calendario.aspx?empleadoId={empleadoId}&servicioId={servicioId}");
            }
        }
    }
}