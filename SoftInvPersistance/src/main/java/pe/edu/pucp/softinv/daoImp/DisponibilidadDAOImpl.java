package pe.edu.pucp.softinv.daoImp;

import pe.edu.pucp.softinv.model.Disponibilidad.DisponibilidadDTO;

import java.sql.Time;

public class DisponibilidadDAOImpl implements {

    @Override
    public Integer insertar(DisponibilidadDTO disponibilidad){
        return this.ejecuta_DML(Tipo_Operacion.INSERTAR);
    }

    @Override
    public Integer modificar(DisponibilidadDTO disponibilidad){
        return this.ejecuta_DML(Tipo_Operacion.MODIFICAR);
    }

    @Override
    public Integer eliminar(DisponibilidadDTO disponibilidad){
        return this.ejecuta_DML(Tipo_Operacion.ELIMINAR);
    }

}
