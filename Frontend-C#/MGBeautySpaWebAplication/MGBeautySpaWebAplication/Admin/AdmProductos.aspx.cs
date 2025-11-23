using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using System;
using System.Collections.Generic;
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
                IList<productoDTO> todosLosProductos = productoBO.ListarTodosActivos();
                rptProductos.DataSource = todosLosProductos;
                rptProductos.DataBind();
            }
        }

        // ▼▼▼ NUEVO MÉTODO PARA GENERAR ESTRELLAS ▼▼▼
        public string GenerarHtmlEstrellas(object valoracionObj)
        {
            double valoracion = 0;

            // 1. Validamos y convertimos el objeto a double
            if (valoracionObj != null && double.TryParse(valoracionObj.ToString(), out double v))
            {
                valoracion = v;
            }

            // 2. Redondeamos al entero más cercano (ej: 3.7 -> 4, 3.2 -> 3)
            int estrellasLlenas = (int)Math.Round(valoracion);

            string html = "";

            // 3. Ciclo de 1 a 5 para dibujar las estrellas
            for (int i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas)
                {
                    // Estrella llena (Color principal)
                    html += "<i class='bi bi-star-fill'></i> ";
                }
                else
                {
                    // Estrella vacía (Clase star-empty que cambia el color a gris/claro)
                    html += "<i class='bi bi-star-fill star-empty'></i> ";
                }
            }

            return html;
        }
        // ▲▲▲ FIN NUEVO MÉTODO ▲▲▲

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

                // Lógica de eliminación...
                productoParaEliminar = productoBO.buscarPorId(idProducto);
                // ... (resto de tu lógica de eliminación) ...
                productoBO.eliminar(productoParaEliminar);

                Response.Redirect(Request.RawUrl, false);
            }
        }
    }
}