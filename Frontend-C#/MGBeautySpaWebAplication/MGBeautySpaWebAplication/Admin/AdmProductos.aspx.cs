using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSProductoTipo;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmProductos : System.Web.UI.Page
    {
        private ProductoBO productoBO;
        private ProductoTipoBO productoTipoBO;

        protected void Page_Load(object sender, EventArgs e)
        {
            productoBO = new ProductoBO();
            productoTipoBO = new ProductoTipoBO();

            if (!IsPostBack)
            {
                CargarTabla();
            }
        }

        private void CargarTabla()
        {
            IList<SoftInvBusiness.SoftInvWSProductos.productoDTO> todosLosProductos =
                productoBO.ListarTodosActivos();

            rptProductos.DataSource = todosLosProductos;
            rptProductos.DataBind();
        }

        // -------------------------------------------------------------
        //  JSON PARA EL MODAL DE STOCK
        // -------------------------------------------------------------
        public string ObtenerDatosStockJSON(object idObj)
        {
            if (idObj == null) return "[]";

            int idProducto = Convert.ToInt32(idObj);
            var listaTipos = productoTipoBO.ObtenerPorIdProductoActivo(idProducto);

            if (listaTipos == null) return "[]";

            var listaSimple = new List<object>();

            foreach (var item in listaTipos)
            {
                listaSimple.Add(new
                {
                    idTipo = item.tipo.id,
                    nombreTipo = item.tipo.nombre,
                    stockFisico = item.stock_fisico,
                    stockDespacho = item.stock_despacho
                });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(listaSimple);
        }

        // -------------------------------------------------------------
        //  ESTRELLAS
        // -------------------------------------------------------------
        public string GenerarHtmlEstrellas(object valoracionObj)
        {
            double valoracion = 0;

            if (valoracionObj != null &&
                double.TryParse(valoracionObj.ToString(), out double v))
            {
                valoracion = v;
            }

            int estrellasLlenas = (int)Math.Round(valoracion);
            string html = "";

            for (int i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas)
                    html += "<i class='bi bi-star-fill'></i> ";
                else
                    html += "<i class='bi bi-star-fill star-empty'></i> ";
            }

            return html;
        }

        // -------------------------------------------------------------
        //  EVENTOS DEL REPEATER
        // -------------------------------------------------------------
        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idProducto = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"InsertarProducto.aspx?id={idProducto}", false);
            }

            // IMPORTANTE:
            // Ya NO eliminamos aquí. Eliminar ahora ocurre en el modal,
            // en btnConfirmarEliminarProducto_Click.
        }

        // -------------------------------------------------------------
        //  GUARDAR STOCK MODIFICADO DESDE EL MODAL
        // -------------------------------------------------------------
        protected void btnGuardarStock_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonResult = hdnJsonStockGuardar.Value;

                if (string.IsNullOrEmpty(hdnIdProductoStock.Value) ||
                    string.IsNullOrEmpty(jsonResult))
                    return;

                int idProducto = int.Parse(hdnIdProductoStock.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var listaCambios = serializer.Deserialize<List<StockUpdateInput>>(jsonResult);

                if (listaCambios != null)
                {
                    foreach (var cambio in listaCambios)
                    {
                        var objCompleto = productoTipoBO.obtener(idProducto, cambio.idTipo);

                        if (objCompleto != null)
                        {
                            objCompleto.stock_fisico = cambio.nuevoStock;
                            objCompleto.stock_fisicoSpecified = true;

                            productoTipoBO.Modificar(objCompleto);
                        }
                    }
                }

                CargarTabla();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error guardando stock: " + ex.Message);
            }
        }

        public SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO obtener(int idProducto, int idTipo)
        {
            return productoTipoBO.obtener(idProducto, idTipo);
        }

        public class StockUpdateInput
        {
            public int idTipo { get; set; }
            public int nuevoStock { get; set; }
        }

        // -------------------------------------------------------------
        //  ELIMINAR PRODUCTO DESDE EL MODAL
        // -------------------------------------------------------------
        protected void btnConfirmarEliminarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(hdnIdProductoEliminar.Value, out int idProducto))
                    return;

                var producto = productoBO.buscarPorId(idProducto);

                if (producto != null)
                {
                    producto.promedioValoracion = 0;
                    producto.promedioValoracionSpecified = true;

                    productoBO.eliminar(producto);
                }

                CargarTabla();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error eliminando: " + ex.Message);
            }
        }
    }
}
