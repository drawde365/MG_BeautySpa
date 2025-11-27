using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmServicios : System.Web.UI.Page
    {
        ServicioBO servicioBO;

        private const string SESSION_LISTA_SERVICIOS = "AdmServicios_Lista";

        public AdmServicios()
        {
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarServicios();
            }
        }

        private void CargarServicios()
        {
            try
            {
                // Listar todos (activos + inactivos)
                IList<servicioDTO> todosLosServicios = servicioBO.ListarTodo();

                Session[SESSION_LISTA_SERVICIOS] = todosLosServicios;

                var dataParaRepeater = todosLosServicios.Select(s => new
                {
                    IDServicio = s.idServicio,
                    RutaImagen = s.urlImagen ?? "/Content/images/placeholder.png",
                    NombreServicio = s.nombre,
                    Codigo = s.idServicio,
                    Tipo = s.tipo,
                    Precio = s.precio,
                    Valoracion = s.promedioValoracion,
                    UrlEditar = $"InsertarServicio.aspx?id={s.idServicio}",
                    Activo = s.activo   // ajusta si el campo se llama distinto
                }).ToList();

                rptServicios.DataSource = dataParaRepeater;
                rptServicios.DataBind();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar servicios: " + ex.Message);
            }
        }

        protected void rptServicios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idServicio = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Restaurar")
            {
                RestaurarServicio(idServicio);
            }
            // Eliminar ya no se hace por ItemCommand, sino por el botón del modal
        }

        private void EliminarServicio(int idServicio)
        {
            try
            {
                var servicio = servicioBO.obtenerPorId(idServicio);
                if (servicio == null) return;

                // baja lógica
                servicioBO.eliminar(servicio);

                CargarServicios();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al eliminar servicio: " + ex.Message);
            }
        }

        private void RestaurarServicio(int idServicio)
        {
            try
            {
                var servicio = servicioBO.obtenerPorId(idServicio);
                if (servicio == null) return;

                servicio.activo = 1;
                servicio.activoSpecified = true;

                servicioBO.modificar(servicio);

                CargarServicios();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar servicio: " + ex.Message);
            }
        }

        // Handler llamado por el botón "Sí, dar de baja" del modal
        protected void btnConfirmarEliminarServicio_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hdnIdServicioEliminar.Value, out int idServicio))
                return;

            EliminarServicio(idServicio);
        }

        protected string RenderStars(object valoracionObj)
        {
            int valoracion = 0;
            if (valoracionObj != null && valoracionObj != DBNull.Value)
            {
                valoracion = Convert.ToInt32(valoracionObj);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                if (i <= valoracion)
                    sb.Append("<i class=\"bi bi-star-fill\"></i> ");
                else
                    sb.Append("<i class=\"bi bi-star-fill star-empty\"></i> ");
            }
            return sb.ToString();
        }
    }
}
