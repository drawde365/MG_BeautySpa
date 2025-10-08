package pe.edu.pucp.softinv.business.BO;

import java.util.ArrayList;

//De esta interfaz base heredan las interfaces BO, que serán implementadas por un BOImpl propio
public interface BOBase <T> {
    int insertar(T objeto) throws Exception;
    int modificar(T objeto) throws Exception;
    int eliminar(int idObjeto) throws Exception;
    T obtenerPorId(int idObjeto) throws Exception;
    ArrayList<T> listarTodos() throws Exception;
    void validar(T objeto) throws Exception;
}
