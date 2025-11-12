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


        // Propiedad para guardar la página actual en el ViewState
        private int PaginaActual
        {
            get
            {
                // Si ViewState["PaginaActual"] no existe, devuelve 1 (la primera página)
                object o = ViewState["PaginaActual"];
                if (o == null)
                {
                    return 1;
                }
                else
                {
                    return (int)o;
                }
            }
            set
            {
                ViewState["PaginaActual"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            productoBO = new ProductoBO();
            if (!IsPostBack)
            {
                // Limpiamos el ViewState y cargamos la primera página
                ViewState["ProductosCargados"] = null;
                this.PaginaActual = 1;
                CargarProductos();
            }
        }

        /// <summary>
        /// El evento principal que se dispara al hacer clic en "Ver más".
        /// </summary>
        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            // Incrementamos el contador de página y volvemos a cargar
            this.PaginaActual++;
            CargarProductos();
        }

        /// <summary>
        /// Método principal para obtener y enlazar datos al Repeater.
        /// </summary>
        private void CargarProductos()
        {

            // Trae solo los 10 productos de la página actual
            List<productoDTO> nuevosProductos = productoBO.obtenerProdsPagina(this.PaginaActual).ToList();
            // ----------------------------------------


            // --- 2. LÓGICA DE "VER MÁS" (APILAR DATOS) ---
            List<productoDTO> productosCargados = new List<productoDTO>();

            // Revisa si ya teníamos productos cargados en el ViewState
            if (ViewState["ProductosCargados"] != null)
            {
                productosCargados = (List<productoDTO>)ViewState["ProductosCargados"];
            }

            // Agrega los 10 *nuevos* productos a la lista *existente*
            productosCargados.AddRange(nuevosProductos);


            // --- 3. ENLAZAR (BIND) DATOS AL REPEATER ---
            rptProductos.DataSource = productosCargados;
            rptProductos.DataBind();

            // Guarda la lista *completa* (antiguos + nuevos) de vuelta en el ViewState
            ViewState["ProductosCargados"] = productosCargados;


            // --- 4. OCULTAR BOTÓN "VER MÁS" SI LLEGAMOS AL FINAL ---
            int cantPaginas = productoBO.GetCantPaginas();
            if (this.PaginaActual >= cantPaginas)
            {
                btnVerMas.Visible = false;
            }
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int idProducto = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                // Redirige a la página de edición con el ID del producto
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

                // 4. Ahora envía el objeto "completo" y "seguro"
                productoBO.eliminar(productoParaEliminar);

                ViewState["ProductosCargados"] = null;
                this.PaginaActual = 1;
                CargarProductos();

            }
        }
    }
}