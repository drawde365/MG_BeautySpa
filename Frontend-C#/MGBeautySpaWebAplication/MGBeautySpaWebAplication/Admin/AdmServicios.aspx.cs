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
                var todosLosServicios = servicioBO.ListarTodoActivo();

                var dataParaRepeater = todosLosServicios.Select(s => new {
                    IDServicio = s.idServicio,
                    RutaImagen = s.urlImagen ?? "/Content/images/placeholder.png",
                    NombreServicio = s.nombre,
                    Codigo = s.idServicio,
                    Tipo = s.tipo,
                    Precio = s.precio,
                    Valoracion = s.promedioValoracion,
                    // ▼▼▼ ¡AÑADE ESTA LÍNEA! ▼▼▼
                    UrlEditar = $"InsertarServicio.aspx?id={s.idServicio}"
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

            if (e.CommandName == "Eliminar")
            {
                try
                {
                    servicioDTO servicio = new servicioDTO();
                    servicio = servicioBO.obtenerPorId(idServicio);
                    servicioBO.eliminar(servicio);

                    CargarServicios();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error al eliminar servicio: " + ex.Message);
                }
            }
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
                {
                    sb.Append("<i class=\"bi bi-star-fill\"></i> ");
                }
                else
                {
                    sb.Append("<i class=\"bi bi-star-fill star-empty\"></i> ");
                }
            }
            return sb.ToString();
        }
    }
}