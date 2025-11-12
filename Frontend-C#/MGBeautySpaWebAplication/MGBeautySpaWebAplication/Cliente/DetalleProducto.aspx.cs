using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSComentario;
using SoftInvBusiness.SoftInvWSProductos;
using SoftInvBusiness.SoftInvWSProductoTipo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    // --- DTOs (Definidos fuera de la clase principal) ---
    //public class ComentarioDTODetalle
    //{
    //    public string Autor { get; set; }
    //    public string Texto { get; set; }
    //    public DateTime Fecha { get; set; }
    //    public string AvatarUrl { get; set; } // Añadido para el diseño
    //}
    /*
    public class ProductoTipoDTODetalle
    {
        public ProductoDTODetalle Producto { get; set; }
        public int Stock_fisico { get; set; }
        public int Stock_despacho { get; set; }
        public string Ingredientes { get; set; }
        public string Tipo { get; set; }
        public int Activo { get; set; }
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
        public string Beneficios { get; set; } // Añadido para el diseño
    }
    */
    // --- CLASE DE LA PÁGINA ---
    public partial class DetalleProducto : Page
    {
        //private const string SESSION_PRODUCTO = "ProductoDemo";

        /*
        private ProductoDTODetalle ProductoActual
        {
            get { return (ProductoDTODetalle)Session[SESSION_PRODUCTO]; }
            set { Session[SESSION_PRODUCTO] = value; }
        }
        */
        private ProductoBO productoBO;
        private ComentarioBO comentarioBO;
        private ProductoTipoBO productoTipoBO;
        private SoftInvBusiness.SoftInvWSProductos.productoDTO producto;
        private IList<SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO> tipos;

        public DetalleProducto()
        {
            productoBO = new ProductoBO();
            productoTipoBO = new ProductoTipoBO();
            comentarioBO = new ComentarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDatos();
            if (!IsPostBack)
            {
                PintarProducto();
            }
        }

        private void CargarDatos()
        {
            if (!IsPostBack)
            {
                // --- PRIMERA CARGA (viene de la URL) ---
                string idProductoStr = Request.QueryString["id"];
                if (int.TryParse(idProductoStr, out int idProducto))
                {
                    // Llama a tus servicios SOAP/BO
                    producto = productoBO.buscarPorId(idProducto);
                    tipos = productoTipoBO.ObtenerPorIdProducto(idProducto);

                    // ¡GUÁRDALOS EN SESSION para el próximo PostBack!
                    Session["detalle_producto"] = producto;
                    Session["detalle_tipos"] = tipos;
                }
                else
                {
                    Response.Redirect("~/Cliente/Productos.aspx");
                }
            }
            else
            {
                producto = Session["detalle_producto"] as SoftInvBusiness.SoftInvWSProductos.productoDTO;
                tipos = Session["detalle_tipos"] as IList<SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO>;
            }
        }
        /// <summary>
        /// Pinta los datos en los controles del NUEVO ASPX (sin pestañas).
        /// </summary>
        private void PintarProducto()
        {
            if (producto == null)
            {
                Response.Redirect("~/Cliente/Productos.aspx");
                return;
            }

            // Mapeo a los IDs del ASPX unificado
            Page.Title = producto.nombre;
            litNombreProd.Text = producto.nombre; // Breadcrumb
            litNombre.Text = producto.nombre;     // Título H1
            litDescripcion.Text = producto.descripcion; // Descripción corta
            litPrecio.Text = producto.precio.ToString("C", new CultureInfo("es-PE")); // "S/. 1,234.56"
            imgProducto.ImageUrl = ResolveUrl(producto.urlImagen);

            // --- SECCIÓN "Detalles del Producto" ---
            litTamano.Text = producto.tamanho.ToString() + "ml";
            litComoUsar.Text = producto.modoUso;
            litBeneficios.Text = "Parece que no existe"; // Rellenamos el nuevo literal

            // Repeater de Tipos
            rpPresentaciones.DataSource = tipos;
            rpPresentaciones.DataBind();

            PintarResenas();
        }

        /// <summary>
        /// Pinta toda la sección de reseñas (resumen, barras y lista).
        /// </summary>
        /// 
        private void PintarResenas()
        {
            
            var listaComentarios = comentarioBO.ObtenerComentariosPorProducto(producto.idProducto);
            //ACAAAAAAA
            // 1. Pintar lista de reseñas (ID 'rpComentarios' del C#)
            rpComentarios.DataSource = listaComentarios;
            rpComentarios.DataBind();
            if (listaComentarios == null)
            {
                pnlNoComments.Visible = false;
            } else
            {
                pnlNoComments.Visible = true;
                litReviewScore.Text = producto.promedioValoracion.ToString("0.0");
                litReviewCount.Text = $"{listaComentarios.Count} reseñas";
            }

            // Panel de no comentarios (ID 'pnlNoComments' del C#)
            

            // 2. Pintar Resumen de Calificación
            

            // 3. Pintar Barras de Calificación (Datos de demo)
            //var ratingBars = new[]
            //{
            //    new { Stars = "5", Percentage = 70, Count = listaComentarios.Count(c => c. == "Sofía Castro") },
            //    new { Stars = "4", Percentage = 20, Count = listaComentarios.Count(c => c.Autor != "Sofía Castro") },
            //    new { Stars = "3", Percentage = 5, Count = 0 },
            //    new { Stars = "2", Percentage = 2, Count = 0 },
            //    new { Stars = "1", Percentage = 3, Count = 0 }
            //};
            //rpRatingBars.DataSource = ratingBars;
            //rpRatingBars.DataBind();
        }
        protected void rpPresentaciones_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var btn = (LinkButton)e.Item.FindControl("btnIngredientes");
                btn.CommandArgument = e.Item.ItemIndex.ToString();
                ScriptManager.GetCurrent(Page).RegisterAsyncPostBackControl(btn);
            }
        }

        protected void btnIngredientes_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)sender;
            if (!int.TryParse(btn.CommandArgument, out var idx)) return;

            var pres = tipos[idx];
            if (pres == null) return;

            litIngredientes.Text = Server.HtmlEncode(pres.ingredientes).Replace("\n", "<br/>");

            upModalIng.Update();

            ScriptManager.RegisterStartupScript(
                this, GetType(), "openIng", "var modal = new bootstrap.Modal(document.getElementById('modalIng')); modal.show();", true);
        }

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            List<CartItemDTO> cartItems = Session["Carrito"] as List<CartItemDTO>;
            if (cartItems == null) cartItems = new List<CartItemDTO>();

            int totalItemsAdded = 0;

            foreach (RepeaterItem item in rpPresentaciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtCantidad = (TextBox)item.FindControl("txtCantidad");
                    int itemIndex = item.ItemIndex;

                    if (txtCantidad != null &&
                        int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
                    {
                        SoftInvBusiness.SoftInvWSProductoTipo.productoTipoDTO presentacion = tipos[itemIndex];

                        CartItemDTO newItem = new CartItemDTO
                        {
                            ProductId = producto.idProducto,
                            Nombre = producto.nombre,
                            PrecioUnitario = (decimal)producto.precio,
                            ImagenUrl = producto.urlImagen,
                            TipoPiel = presentacion.tipo,
                            Tamano = producto.tamanho.ToString(),
                            Cantidad = cantidad
                        };

                        CartItemDTO existingItem = cartItems.FirstOrDefault(i =>
                            i.ProductId == newItem.ProductId && i.TipoPiel == newItem.TipoPiel);

                        if (existingItem != null)
                        {
                            existingItem.Cantidad += cantidad;
                        }
                        else
                        {
                            cartItems.Add(newItem);
                        }

                        totalItemsAdded += cantidad;
                        txtCantidad.Text = "0";
                    }
                }
            }

            if (totalItemsAdded > 0)
            {
                Session["Carrito"] = cartItems;

                int currentCartCount = (Session["CartCount"] as int?) ?? 0;
                Session["CartCount"] = currentCartCount + totalItemsAdded;

                Cliente masterPage = this.Master as Cliente;
                if (masterPage != null)
                {
                    masterPage.UpdateCartDisplay();
                }

                lblCartMessage.Text = "¡Producto(s) añadido(s) al carrito!";
                lblCartMessage.Visible = true;
            }
            else
            {
                lblCartMessage.Text = "Por favor, selecciona una cantidad.";
                lblCartMessage.Visible = true;
            }
        }

        /// <summary>
        /// Publica un nuevo comentario (adaptado a los nuevos IDs).
        /// </summary>
        protected void btnEnviarComent_Click(object sender, EventArgs e)
        {
            // IDs del C# ('txtNombreComent', 'txtComentario')
            string autor = string.IsNullOrWhiteSpace(txtNombreComent.Text) ? "Anónimo" : txtNombreComent.Text.Trim();
            string texto = txtComentario.Text?.Trim();
            if (string.IsNullOrEmpty(texto)) return;
            /*
            ProductoActual.Comentarios.Add(new ComentarioDTODetalle
            {
                Autor = autor,
                Texto = texto,
                Fecha = DateTime.Now,
                AvatarUrl = "/avatar-placeholder-user.png" // Asignamos un avatar demo
            });

            // Limpio y repinto
            txtNombreComent.Text = "";
            txtComentario.Text = "";
            PintarResenas(ProductoActual); // Llamamos a la nueva función de pintar
            */
        }

        // --- DATOS DE DEMO (Adaptados al nuevo DTO) 
    }
}

/*using System;
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
            List<CartItemDTO> cartItems = Session["Carrito"] as List<CartItemDTO>;
            if (cartItems == null)
            {
                cartItems = new List<CartItemDTO>();
            }

            int totalItemsAdded = 0;
            ProductoDTODetalle productoBase = ProductoActual;
            if (productoBase == null) return;

            foreach (RepeaterItem item in rpPresentaciones.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtCantidad = (TextBox)item.FindControl("txtCantidad");
                    LinkButton btnIngredientes = (LinkButton)item.FindControl("btnIngredientes");

                    if (txtCantidad != null && btnIngredientes != null &&
                        int.TryParse(btnIngredientes.CommandArgument, out int itemIndex) &&
                        int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
                    {
                        ProductoTipoDTODetalle presentacion = productoBase.ProductosTipos[itemIndex];

                        CartItemDTO newItem = new CartItemDTO
                        {
                            ProductId = productoBase.IdProducto,
                            Nombre = productoBase.Nombre,
                            PrecioUnitario = (decimal)productoBase.Precio,
                            ImagenUrl = productoBase.UrlImagen,
                            TipoPiel = presentacion.Tipo,
                            Tamano = productoBase.Tamaño.ToString(),
                            Cantidad = cantidad
                        };

                        CartItemDTO existingItem = cartItems.FirstOrDefault(i =>
                            i.ProductId == newItem.ProductId && i.TipoPiel == newItem.TipoPiel);

                        if (existingItem != null)
                        {
                            existingItem.Cantidad += cantidad;
                        }
                        else
                        {
                            cartItems.Add(newItem);
                        }

                        totalItemsAdded += cantidad;
                        txtCantidad.Text = "0";
                    }
                }
            }

            if (totalItemsAdded > 0)
            {
                Session["Carrito"] = cartItems;

                int currentCartCount = (Session["CartCount"] as int?) ?? 0;
                Session["CartCount"] = currentCartCount + totalItemsAdded;

                Cliente masterPage = this.Master as Cliente;
                if (masterPage != null)
                {
                    masterPage.UpdateCartDisplay();
                }
            }
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
}
*/