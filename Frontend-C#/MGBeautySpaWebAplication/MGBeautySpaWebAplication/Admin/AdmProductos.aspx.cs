using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSProductoTipo;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization; // Necesario para JSON
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmProductos : System.Web.UI.Page
    {
        private ProductoBO productoBO;
        private ProductoTipoBO productoTipoBO; // Necesitamos este BO

        protected void Page_Load(object sender, EventArgs e)
        {
            productoBO = new ProductoBO();
            productoTipoBO = new ProductoTipoBO(); // Inicializar

            if (!IsPostBack)
            {
                CargarTabla();
            }
        }

        private void CargarTabla()
        {
            IList<SoftInvBusiness.SoftInvWSProductos.productoDTO> todosLosProductos = productoBO.ListarTodosActivos();
            rptProductos.DataSource = todosLosProductos;
            rptProductos.DataBind();
        }

        // ▼▼▼ 1. MÉTODO PARA GENERAR JSON DE STOCKS EN EL BOTÓN ▼▼▼
        public string ObtenerDatosStockJSON(object idObj)
        {
            if (idObj == null) return "[]";
            int idProducto = Convert.ToInt32(idObj);

            // Obtenemos las variantes (Tipos) de este producto
            var listaTipos = productoTipoBO.ObtenerPorIdProductoActivo(idProducto);

            if (listaTipos == null) return "[]";

            // Creamos una lista simplificada solo con lo necesario para el Modal
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

            // Serializamos a JSON
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(listaSimple);
        }

        // Método de estrellas (Ya lo tenías)
        public string GenerarHtmlEstrellas(object valoracionObj)
        {
            /* ... (Tu código de estrellas se mantiene igual) ... */
            double valoracion = 0;
            if (valoracionObj != null && double.TryParse(valoracionObj.ToString(), out double v)) valoracion = v;
            int estrellasLlenas = (int)Math.Round(valoracion);
            string html = "";
            for (int i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas) html += "<i class='bi bi-star-fill'></i> ";
                else html += "<i class='bi bi-star-fill star-empty'></i> ";
            }
            return html;
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // ... (Tu lógica de Editar y Eliminar se mantiene igual) ...
            int idProducto = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"InsertarProducto.aspx?id={idProducto}", false);
            }
            if (e.CommandName == "Eliminar")
            {
                // ... tu logica de eliminar ...
                SoftInvBusiness.SoftInvWSProductos.productoDTO productoParaEliminar = new SoftInvBusiness.SoftInvWSProductos.productoDTO();
                productoParaEliminar.idProducto = idProducto;
                productoParaEliminar.idProductoSpecified = true;
                productoParaEliminar = productoBO.buscarPorId(idProducto);
                productoParaEliminar.promedioValoracion = 0;
                productoParaEliminar.promedioValoracionSpecified = true;
                productoBO.eliminar(productoParaEliminar);
                Response.Redirect(Request.RawUrl, false);
            }
        }

        // ▼▼▼ 2. LÓGICA PARA GUARDAR EL STOCK MODIFICADO ▼▼▼
        // ▼▼▼ LÓGICA SEGURA: LEER -> MODIFICAR -> GUARDAR ▼▼▼
        protected void btnGuardarStock_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonResult = hdnJsonStockGuardar.Value;
                // Validamos que el ID del producto sea válido
                if (string.IsNullOrEmpty(hdnIdProductoStock.Value) || string.IsNullOrEmpty(jsonResult)) return;

                int idProducto = int.Parse(hdnIdProductoStock.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var listaCambios = serializer.Deserialize<List<StockUpdateInput>>(jsonResult);

                if (listaCambios != null)
                {
                    foreach (var cambio in listaCambios)
                    {
                        // PASO 1: Obtener el objeto COMPLETO actual desde la BD
                        // Asumo que en tu BO tienes un método 'obtener' que recibe (idProducto, idTipo)
                        // Si no lo tienes en el BO, deberás agregarlo delegando al DAO.obtener(id, tipoId)
                        SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO objCompleto = productoTipoBO.obtener(idProducto, cambio.idTipo);

                        if (objCompleto != null)
                        {
                            // PASO 2: Modificar SOLO el campo que nos interesa
                            objCompleto.stock_fisico = cambio.nuevoStock;

                            // IMPORTANTE: Asegurar los flags 'Specified' para SOAP
                            objCompleto.stock_fisicoSpecified = true;

                            // (Opcional) Aseguramos que los IDs sigan correctos, aunque ya deberían venir cargados
                            if (objCompleto.producto == null)
                            {
                                objCompleto.producto = new SoftInvBusiness.SoftInvWSProductoTipo.productoDTO { idProducto = idProducto, idProductoSpecified = true };
                            }
                            if (objCompleto.tipo == null)
                            {
                                objCompleto.tipo = new SoftInvBusiness.SoftInvWSProductoTipo.tipoProdDTO { id = cambio.idTipo};
                            }

                            // PASO 3: Mandar a guardar el objeto completo (con ingredientes, despacho, etc. intactos)
                            productoTipoBO.Modificar(objCompleto);
                        }
                    }
                }

                // Recargar la tabla para ver los cambios reflejados
                CargarTabla();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al guardar stock: " + ex.Message);
            }
        }

        // En tu ProductoTipoBO
        public SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO obtener(int idProducto, int idTipo)
        {
            // Llama al DAO que ya programaste antes
            return productoTipoBO.obtener(idProducto, idTipo);
        }

        // Clase auxiliar interna para recibir el JSON del frontend
        public class StockUpdateInput
        {
            public int idTipo { get; set; }
            public int nuevoStock { get; set; }
        }
    }
}