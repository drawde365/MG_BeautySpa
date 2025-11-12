package pe.edu.pucp.softinv.daoImp.util;

import pe.edu.pucp.softinv.dao.ComentarioDAO;

public class ComentariosParametrosBuilder {
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


    public ComentariosParametrosBuilder(){
        this.producto_Id = null;
        this.servicio_Id = null;
    }


    public ComentariosParametrosBuilder conProducto_Id(Integer producto_Id) {
        this.producto_Id=producto_Id;
        return this;
    }

    public ComentariosParametrosBuilder conServicio_Id(Integer servicio_Id) {
        this.servicio_Id=servicio_Id;
        return this;
    }

    public ComentariosParametros BuildComentariosParametros(){
        ComentariosParametros comentariosParametros = new ComentariosParametros();
        comentariosParametros.setProducto_Id(producto_Id);
        comentariosParametros.setServicio_Id(servicio_Id);
        return comentariosParametros;
    }
}
