using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class DetalleProducto : Page
    {
        // Guardamos el “producto demo” en Session para que sobreviva a postbacks
        private const string SESSION_PRODUCTO = "ProductoDemo";

        private ProductoDTODetalle ProductoActual
        {
            get { return (ProductoDTODetalle)Session[SESSION_PRODUCTO]; }
            set { Session[SESSION_PRODUCTO] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProductoActual == null)
                    ProductoActual = CrearProductoDemo();

                PintarProducto(ProductoActual);
                PintarComentarios(ProductoActual.Comentarios);
            }

            // IMPORTANTE: registrar SIEMPRE (también en postbacks asíncronos)
            RegisterIngredientesAsyncTriggers();
        }
        private void RegisterIngredientesAsyncTriggers()
        {
            var sm = ScriptManager.GetCurrent(Page);
            foreach (RepeaterItem it in rpPresentaciones.Items)
            {
                var btn = it.FindControl("btnIngredientes") as LinkButton;
                if (btn != null)
                    sm.RegisterAsyncPostBackControl(btn);
            }
        }


        /* =====================  UI  ===================== */

        private void PintarProducto(ProductoDTODetalle p)
        {
            if (p == null)
            {
                Response.Redirect("~/Cliente/Productos.aspx");
                return;
            }

            litTitulo.Text = p.Nombre;
            litNombreProd.Text = p.Nombre;
            litNombre.Text = p.Nombre;
            litDescripcion.Text = p.Descripcion;
            litDescripcionLarga.Text = p.Descripcion;
            litModoUso.Text = p.ModoUso;
            litPrecio.Text = p.Precio.ToString("0.00");
            imgProducto.ImageUrl = ResolveUrl(p.UrlImagen);

            rpPresentaciones.DataSource = p.ProductosTipos;
            rpPresentaciones.DataBind();
        }

        private void PintarComentarios(List<ComentarioDTODetalle> lista)
        {
            rpComentarios.DataSource = lista;
            rpComentarios.DataBind();
            pnlNoComments.Visible = lista == null || lista.Count == 0;
        }

        /* ============  Interacciones de la página  ============ */

        protected void rpPresentaciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var btn = (LinkButton)e.Item.FindControl("btnIngredientes");
                btn.CommandArgument = e.Item.ItemIndex.ToString(); // índice para recuperar la presentación
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(btn); // <-- disparo AJAX
            }
        }
        protected void btnIngredientes_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            if (!int.TryParse(btn.CommandArgument, out var idx)) return;

            var pres = ProductoActual?.ProductosTipos?[idx];
            if (pres == null) return;

            litIngredientes.Text = Server.HtmlEncode(pres.Ingredientes).Replace("\n", "<br/>");

            upModalIng.Update();                            // refresca solo el modal
            ScriptManager.RegisterStartupScript(
                this, GetType(), "openIng", "openIng();", true);   // lo abre
        }

        // Suma al contador de carrito (demo)
        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            int n = 0;
            int.TryParse(Convert.ToString(Session["CartCount"]), out n);
            Session["CartCount"] = n + 1;
        }

        // Crea un comentario y refresca la pestaña “Reseñas”
        protected void btnEnviarComent_Click(object sender, EventArgs e)
        {
            string autor = string.IsNullOrWhiteSpace(txtNombreComent.Text) ? "Anónimo" : txtNombreComent.Text.Trim();
            string texto = txtComentario.Text?.Trim();
            if (string.IsNullOrEmpty(texto)) return;

            ProductoActual.Comentarios.Add(new ComentarioDTODetalle
            {
                Autor = autor,
                Texto = texto,
                Fecha = DateTime.Now
            });

            // Limpio y repinto
            txtNombreComent.Text = "";
            txtComentario.Text = "";
            PintarComentarios(ProductoActual.Comentarios);

            // Vuelvo a la pestaña de comentarios
            ScriptManager.RegisterStartupScript(this, GetType(), "tabCom", "setTab('com');", true);
        }

        /* ============  “Datos” cargados directo al DTO  ============ */

        private ProductoDTODetalle CrearProductoDemo()
        {
            return new ProductoDTODetalle
            {
                IdProducto = 1,
                Nombre = "Fotoprotector",
                Descripcion = "Protege eficazmente contra rayos UVA y UVB, previene manchas y envejecimiento prematuro. Textura ligera de rápida absorción.",
                Precio = 49.90,
                ModoUso = "Aplicar uniformemente sobre la piel limpia 15 minutos antes de la exposición solar. Reaplicar cada 2 horas.",
                UrlImagen = "~/Content/images/CremaHidratante.jpg",
                PromedioValoracion = 4.6,
                Activo = 1,
                Tamaño = 200,
                ProductosTipos = new List<ProductoTipoDTODetalle>
                {
                    new ProductoTipoDTODetalle
                    {
                        Tipo = "Sensible",
                        Ingredientes = "Óxido de zinc, Dióxido de titanio, Glicerina, Pantenol, Alantoína",
                        Stock_fisico = 25,
                        Stock_despacho = 10,
                        Activo = 1
                    },
                    new ProductoTipoDTODetalle
                    {
                        Tipo = "Mixta",
                        Ingredientes = "Avobenzona, Octisalato, Niacinamida, Ácido hialurónico ligero, Vitamina E",
                        Stock_fisico = 18,
                        Stock_despacho = 10,
                        Activo = 1
                    },
                    new ProductoTipoDTODetalle
                    {
                        Tipo = "Seca",
                        Ingredientes = "Ceramidas, Ácido hialurónico, Escualano, Manteca de karité",
                        Stock_fisico = 12,
                        Stock_despacho = 10,
                        Activo = 1
                    },
                    new ProductoTipoDTODetalle
                    {
                        Tipo = "Grasa",
                        Ingredientes = "Octinoxato, Té verde, Zinc PCA, Extracto de hamamelis",
                        Stock_fisico = 20,
                        Stock_despacho = 10,
                        Activo = 1
                    }
                },
                Comentarios = new List<ComentarioDTODetalle>()
            };
        }

        
    }

    /* =====================  DTOs  ===================== */

    public class ComentarioDTODetalle
    {
        public string Autor { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ProductoDTODetalle
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public string ModoUso { get; set; }
        public string UrlImagen { get; set; }
        public List<ComentarioDTODetalle> Comentarios { get; set; }
        public double PromedioValoracion { get; set; }
        public List<ProductoTipoDTODetalle> ProductosTipos { get; set; }
        public int Activo { get; set; }
        public double Tamaño { get; set; }
    }

    public class ProductoTipoDTODetalle
    {
        public ProductoDTODetalle Producto { get; set; }
        public int Stock_fisico { get; set; }
        public int Stock_despacho { get; set; }
        public string Ingredientes { get; set; }
        public string Tipo { get; set; }
        public int Activo { get; set; }
    }
}
