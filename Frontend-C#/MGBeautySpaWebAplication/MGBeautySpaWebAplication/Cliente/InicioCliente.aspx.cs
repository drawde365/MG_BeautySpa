using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSServicio;

namespace MGBeautySpaWebAplication.Cliente
{
    public class ItemDestacadoDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImageUrl { get; set; }
        public string UrlDetalle { get; set; }
    }

    public partial class InicioCliente : System.Web.UI.Page
    {
        private ProductoBO productoBO;
        private ServicioBO servicioBO;

        public InicioCliente()
        {
            productoBO = new ProductoBO();
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductosDestacados();
                CargarServiciosDestacados();
            }
        }

        private void CargarProductosDestacados()
        {
            try
            {
                var productos = productoBO.ListarTodosActivos();
                if (productos == null) return;

                var productosDestacados = productos
                    .OrderByDescending(p => p.promedioValoracion)
                    .Select(p => new ItemDestacadoDTO
                    {
                        Nombre = p.nombre,
                        Descripcion = p.descripcion.Length > 50 ? p.descripcion.Substring(0, 50) + "..." : p.descripcion,
                        ImageUrl = ResolveUrl(p.urlImagen ?? "~/Content/images/placeholder.png"),
                        UrlDetalle = ResolveUrl($"~/Cliente/DetalleProducto.aspx?id={p.idProducto}")
                    })
                    .ToList();

                rptProductosDestacados.DataSource = productosDestacados;
                rptProductosDestacados.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        private void CargarServiciosDestacados()
        {
            try
            {
                var servicios = servicioBO.ListarTodoActivo();
                if (servicios == null) return;

                var serviciosDestacados = servicios
                    .OrderByDescending(s => s.promedioValoracion)
                    .Select(s => new ItemDestacadoDTO
                    {
                        Nombre = s.nombre,
                        Descripcion = s.descripcion.Length > 50 ? s.descripcion.Substring(0, 50) + "..." : s.descripcion,
                        ImageUrl = ResolveUrl(s.urlImagen ?? "~/Content/images/placeholder.png"),
                        UrlDetalle = ResolveUrl($"~/Cliente/DetalleServicio.aspx?id={s.idServicio}")
                    })
                    .ToList();

                rptServiciosDestacados.DataSource = serviciosDestacados;
                rptServiciosDestacados.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
    }
}