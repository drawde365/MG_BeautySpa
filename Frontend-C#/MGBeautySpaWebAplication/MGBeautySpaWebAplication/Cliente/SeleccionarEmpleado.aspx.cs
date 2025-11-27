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
            var resultadoWS = servicioBO.empleadosPorServicio(servicioId);

            if (resultadoWS == null)
            {
                rpEmpleados.Visible = false;
                pnlNoEmpleados.Visible = true;
            }
            else
            {
                rpEmpleados.Visible = true;
                pnlNoEmpleados.Visible = false;

                var listaEmpleadosDisplay = resultadoWS.Select(e => new EmpleadoDisplay
                {
                    Id = e.idUsuario,
                    Nombre = $"{e.nombre} {e.primerapellido}",
                    AvatarUrl = string.IsNullOrEmpty(e.urlFotoPerfil)
                                        ? "~/Content/images/blank-photo.jpg"
                                        : e.urlFotoPerfil
                }).ToList();

                rpEmpleados.DataSource = listaEmpleadosDisplay;
                rpEmpleados.DataBind();
            }
        }

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