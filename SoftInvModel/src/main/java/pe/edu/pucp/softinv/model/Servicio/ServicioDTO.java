package pe.edu.pucp.softinv.model.Servicio;

import pe.edu.pucp.softinv.model.Comentario.ComentarioServicioDTO;
import pe.edu.pucp.softinv.model.Personas.EmpleadoDTO;

import java.util.ArrayList;

public class ServicioDTO {
    private Integer idServicio;
    private String nombre;
    private String descripcion;
    private TipoServicio tipo;
    private Double precio;
    private Double promedioValoracion;
    private String urlImagen;
    private ArrayList<ComentarioServicioDTO> comentarios;
    private Integer duracionHora;
    private ArrayList<EmpleadoDTO> empleados;
    private ArrayList<CitaDTO> citas;

    public Integer getIdServicio() {
        return idServicio;
    }

    public void setIdServicio(Integer idServicio) {
        this.idServicio = idServicio;
    }

    public ArrayList<CitaDTO> getCitas() {
        return citas;
    }

    public void setCitas(ArrayList<CitaDTO> citas) {
        this.citas = citas;
    }

    public ArrayList<EmpleadoDTO> getEmpleados() {
        return empleados;
    }

    public void setEmpleados(ArrayList<EmpleadoDTO> empleados) {
        this.empleados = empleados;
    }

    public Integer getDuracionHora() {
        return duracionHora;
    }

    public void setDuracionHora(Integer duracionMin) {
        this.duracionHora = duracionMin;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getTipo() {
        if(tipo==TipoServicio.FACIAL)
            return "FACIAL";
        else if(tipo==TipoServicio.CORPORAL)
            return "CORPORAL";
        else if(tipo==TipoServicio.TERAPIA_COMPLEMENTARIA)
            return "TERAPIA_COMPLEMENTARIA";
        return "";
    }

    public void setTipo(TipoServicio tipo) {
        this.tipo = tipo;
    }

    public void setTipo(String tipo) {
        if(tipo == "FACIAL")
            this.tipo = TipoServicio.FACIAL;
        else if (tipo == "CORPORAL")
            this.tipo = TipoServicio.CORPORAL;
        else if (tipo == "TERAPIA_COMPLEMENTARIA")
            this.tipo = TipoServicio.TERAPIA_COMPLEMENTARIA;
    }

    public Double getPrecio() {
        return precio;
    }

    public void setPrecio(Double precio) {
        this.precio = precio;
    }

    public Double getPromedioValoracion() {
        return promedioValoracion;
    }

    public void setPromedioValoracion(Double promedioValoracion) {
        this.promedioValoracion = promedioValoracion;
    }

    public String getUrlImagen() {
        return urlImagen;
    }

    public void setUrlImagen(String urlImagen) {
        this.urlImagen = urlImagen;
    }

    public ArrayList<ComentarioServicioDTO> getComentarios() {
        return comentarios;
    }

    public void setComentarios(ArrayList<ComentarioServicioDTO> comentarios) {
        this.comentarios = comentarios;
    }

    public ServicioDTO() {
        idServicio=null;
        nombre = null;
        tipo = null;
        precio = null;
        promedioValoracion = null;
        urlImagen = null;
        comentarios = null;
        duracionHora=null;
        empleados=null;
        citas=null;
        descripcion = null;
    }

    public ServicioDTO(Integer idServicio,String nombre,TipoServicio tipo,Double precio,Double promedioValoracion,
                       String urlImagen,ArrayList<ComentarioServicioDTO> comentarios, Integer duracionHora,
                       ArrayList<EmpleadoDTO> empleados,ArrayList<CitaDTO> citas, String descripcion) {
        this.idServicio=idServicio;
        this.nombre=nombre;
        this.tipo=tipo;
        this.precio=precio;
        this.promedioValoracion=promedioValoracion;
        this.urlImagen=urlImagen;
        this.comentarios=comentarios;
        this.duracionHora=duracionHora;
        this.empleados=empleados;
        this.citas=citas;
        this.descripcion = descripcion;
    }

    public String getDescripcion() {
        return descripcion;
    }

    public void setDescripcion(String descripcion) {
        this.descripcion = descripcion;
    }
}
