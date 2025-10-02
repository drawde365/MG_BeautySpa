using System;
using System.ComponentModel;

namespace softinv.model
{
    public class ServicioDTO
    {
        private int? idServicio;
        private string nombre;
        private string descripcion;
        private TipoServicio tipo;
        private double? precio;
        private double? promedioValoracion;
        private string urlImagen;
        private BindingList<ComentarioServicioDTO> comentarios;
        private int? duracionHora;
        private BindingList<EmpleadoDTO> empleados;
        private BindingList<CitaDTO> citas;

        public int? IdServicio { get => idServicio; set => idServicio = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public TipoServicio Tipo { get => tipo; set => tipo = value; }
        public double? Precio { get => precio; set => precio = value; }
        public double? PromedioValoracion { get => promedioValoracion; set => promedioValoracion = value; }
        public string UrlImagen { get => urlImagen; set => urlImagen = value; }
        public BindingList<ComentarioServicioDTO> Comentarios { get => comentarios; set => comentarios = value; }
        public int? DuracionHora { get => duracionHora; set => duracionHora = value; }
        public BindingList<EmpleadoDTO> Empleados { get => empleados; set => empleados = value; }
        public BindingList<CitaDTO> Citas { get => citas; set => citas = value; }

        public string getTipo()
        {
            if (Tipo == TipoServicio.FACIAL)
                return "FACIAL";
            else if (Tipo == TipoServicio.CORPORAL)
                return "CORPORAL";
            else if (Tipo == TipoServicio.TERAPIA_COMPLEMENTARIA)
                return "TERAPIA_COMPLEMENTARIA";
            return "";
        }

        public void setTipo(string tipo)
        {
            if (tipo == "FACIAL")
                this.Tipo = TipoServicio.FACIAL;
            else if (tipo == "CORPORAL")
                this.Tipo = TipoServicio.CORPORAL;
            else if (tipo == "TERAPIA_COMPLEMENTARIA")
                this.Tipo = TipoServicio.TERAPIA_COMPLEMENTARIA;
        }

        public ServicioDTO()
        {
            this.IdServicio = null;
            this.Nombre = null;
            this.Precio = null;
            this.PromedioValoracion = null;
            this.UrlImagen = null;
            this.Comentarios = null;
            this.DuracionHora = null;
            this.Empleados = null;
            this.Citas = null;
            this.Descripcion = null;
        }

        public ServicioDTO(int idServicio, string nombre, TipoServicio tipo, double precio, double promedioValoracion,
                           string urlImagen, BindingList<ComentarioServicioDTO> comentarios, int duracionHora,
                           BindingList<EmpleadoDTO> empleados, BindingList<CitaDTO> citas, string descripcion)
        {
            this.IdServicio = idServicio;
            this.Nombre = nombre;
            this.Tipo = tipo;
            this.Precio = precio;
            this.PromedioValoracion = promedioValoracion;
            this.UrlImagen = urlImagen;
            this.Comentarios = comentarios;
            this.DuracionHora = duracionHora;
            this.Empleados = empleados;
            this.Citas = citas;
            this.Descripcion = descripcion;
        }
    }

}