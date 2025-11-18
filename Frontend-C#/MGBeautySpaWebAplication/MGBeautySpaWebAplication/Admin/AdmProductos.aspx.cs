using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmProductos : System.Web.UI.Page
    {
        private ProductoBO productoBO;

        protected void Page_Load(object sender, EventArgs e)
        {
            productoBO = new ProductoBO();
            if (!IsPostBack)
            {
                // 1. Carga TODOS los productos
                IList<productoDTO> todosLosProductos = productoBO.ListarTodosActivos();

                if (todosLosProductos == null)
                {
                    rptProductos.DataSource = todosLosProductos;
                    rptProductos.DataBind();
                }
                else
                {
                    rptProductos.DataSource = todosLosProductos;
                    rptProductos.DataBind();
                }
            }
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idProducto = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"InsertarProducto.aspx?id={idProducto}", false);
            }

            if (e.CommandName == "Eliminar")
            {
                productoDTO productoParaEliminar = new productoDTO();

                productoParaEliminar.idProducto = idProducto;
                productoParaEliminar.idProductoSpecified = true;

                productoParaEliminar = productoBO.buscarPorId(idProducto);
                productoParaEliminar.promedioValoracion = 0;
                productoParaEliminar.promedioValoracionSpecified = true;
                productoParaEliminar.comentarios = new comentarioDTO[0];
                productoParaEliminar.productosTipos = new productoTipoDTO[0];

                productoBO.eliminar(productoParaEliminar);

                // Importante: Redirige a la misma página para forzar la recarga
                // de la lista completa desde la BD.
                Response.Redirect(Request.RawUrl, false);
            }
        }
    }
}