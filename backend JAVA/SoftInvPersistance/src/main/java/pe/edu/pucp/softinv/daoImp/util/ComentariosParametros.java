package pe.edu.pucp.softinv.daoImp.util;

public class ComentariosParametros {
    private Integer contadorPagina;
    private Integer producto_Id;
    private Integer servicio_Id;

    public Integer getServicio_Id() {
        return servicio_Id;
    }

    public void setServicio_Id(Integer servicio_Id) {
        this.servicio_Id = servicio_Id;
    }

    public Integer getProducto_Id() {
        return producto_Id;
    }

    public void setProducto_Id(Integer producto_Id) {
        this.producto_Id = producto_Id;
    }

    public Integer getContadorPagina() {
        return contadorPagina;
    }

    public void setContadorPagina(Integer contadorPagina) {
        this.contadorPagina = contadorPagina;
    }

    public ComentariosParametros() {
        contadorPagina=null;
        producto_Id=null;
        servicio_Id=null;
    }
}
