using System;
using System.Collections.Generic;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DescripcionCorta { get; set; }
        public string DescripcionLarga { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; }
        public List<string> Beneficios { get; set; }
        public ReviewSummary ResumenReviews { get; set; }
        public List<Resena> Resenas { get; set; }
    }

    public class Resena
    {
        public string NombreAutor { get; set; }
        public string Fecha { get; set; }
        public int Rating { get; set; }
        public string Comentario { get; set; }
        public string AvatarUrl { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }

    public class ReviewSummary
    {
        public double ScorePromedio { get; set; }
        public int TotalCount { get; set; }
        public List<RatingBar> Bars { get; set; }
    }

    public class RatingBar
    {
        public int Stars { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }        
    public partial class DetalleServicio : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Simulas la carga de un servicio.
                // En un caso real, obtendrías el ID de la URL, por ej:
                // int servicioId = Convert.ToInt32(Request.QueryString["id"]);
                
                Servicio servicio = CargarDatosDelServicio(1); // Carga el servicio de ejemplo
                PoblarDatos(servicio);
            }
        }

        private void PoblarDatos(Servicio servicio)
        {
            // --- Encabezado y Breadcrumb ---
            litNombreBreadcrumb.Text = servicio.Nombre;
            litNombreServicio.Text = servicio.Nombre;
            litDescripcionCorta.Text = servicio.DescripcionCorta;

            // --- Sección Principal ---
            imgServicio.ImageUrl = servicio.ImagenUrl;
            imgServicio.AlternateText = servicio.Nombre;
            litDescripcionLarga.Text = servicio.DescripcionLarga;
            litPrecio.Text = servicio.Precio.ToString("F2"); // Formato con 2 decimales

            // --- Beneficios (Usando un Repeater) ---
            rpBeneficios.DataSource = servicio.Beneficios;
            rpBeneficios.DataBind();

            // --- Resumen de Reseñas ---
            litReviewScore.Text = servicio.ResumenReviews.ScorePromedio.ToString("F1");
            litReviewCount.Text = $"{servicio.ResumenReviews.TotalCount} reseñas";

            // Barras de puntaje (Usando un Repeater)
            rpRatingBars.DataSource = servicio.ResumenReviews.Bars;
            rpRatingBars.DataBind();

            // --- Lista de Reseñas (Usando un Repeater) ---
            rpResenas.DataSource = servicio.Resenas;
            rpResenas.DataBind();
        }

        /// <summary>
        /// MÉTODO DE EJEMPLO: Simula una llamada a la base de datos
        /// </summary>
        private Servicio CargarDatosDelServicio(int id)
        {
            // Datos de ejemplo para las barras
            var bars = new List<RatingBar>
            {
                new RatingBar { Stars = 5, Count = 50, Percentage = 42 },
                new RatingBar { Stars = 4, Count = 29, Percentage = 24 },
                new RatingBar { Stars = 3, Count = 20, Percentage = 17 },
                new RatingBar { Stars = 2, Count = 15, Percentage = 12 },
                new RatingBar { Stars = 1, Count = 6, Percentage = 5 } // Ajustado para que sume 120
            };

            // Datos de ejemplo para las reseñas
            var resenas = new List<Resena>
            {
                new Resena
                {
                    NombreAutor = "Sofía Castro",
                    Fecha = "hace 2 meses",
                    Rating = 5,
                    Comentario = "Salí con la piel súper limpia, suave y luminosa. Fue una experiencia relajante y sentí la diferencia al instante. ¡Lo recomiendo!",
                    AvatarUrl = "/avatar-placeholder.png",
                    Likes = 12,
                    Dislikes = 2
                },
                new Resena
                {
                    NombreAutor = "Carlos Mendoza",
                    Fecha = "hace 3 meses",
                    Rating = 4,
                    Comentario = "Muy buen servicio, aunque la música estaba un poco alta para mi gusto. Pero el resultado en la piel es innegable.",
                    AvatarUrl = "/avatar-placeholder-2.png",
                    Likes = 8,
                    Dislikes = 0
                }
            };
            
            // Creas el objeto principal
            return new Servicio
            {
                Id = id,
                Nombre = "Limpieza Facial Profunda",
                DescripcionCorta = "Un tratamiento facial personalizado para rejuvenecer y revitalizar tu piel, dejándola radiante y saludable.",
                DescripcionLarga = "Tratamiento que elimina impurezas, células muertas y exceso de grasa acumulada en la piel. Ayuda a desobstruir los poros, previene brotes de acné y mejora la absorción de productos faciales. Ideal para renovar y revitalizar la piel del rostro, dejándola fresca, suave y luminosa. Recomendado para todo tipo de piel.",
                Precio = 89.90m,
                ImagenUrl = "/placeholder-image.png",
                Beneficios = new List<string>
                {
                    "Piel más limpia y clara",
                    "Reducción de líneas finas y arrugas",
                    "Hidratación profunda",
                    "Mejora de la textura y el tono de la piel"
                },
                ResumenReviews = new ReviewSummary
                {
                    ScorePromedio = 4.2,
                    TotalCount = 120,
                    Bars = bars
                },
                Resenas = resenas
            };
        }

        // --- Eventos de Botones ---
        
        protected void btnReservarCita_Click(object sender, EventArgs e)
        {
            //Response.Redirect("SeleccionarEmpleado.aspx?servicioId=" + Request.QueryString["id"]);
            Response.Redirect("~/Cliente/SeleccionarEmpleado.aspx");
        }

        protected void btnEnviarResena_Click(object sender, EventArgs e)
        {
            // Aquí iría tu lógica para guardar la reseña
            string comentario = txtNuevaResena.Text;
            // int rating = ... (Necesitarías un control para esto)
            
            // Guardar en BD...
            
            // Limpiar y recargar
            txtNuevaResena.Text = "";
            
            // Vuelves a cargar los datos para mostrar la nueva reseña
            Servicio servicio = CargarDatosDelServicio(1); 
            PoblarDatos(servicio);
        }
    }
}