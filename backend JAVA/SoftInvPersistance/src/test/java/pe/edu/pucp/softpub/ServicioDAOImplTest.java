package pe.edu.pucp.softpub;

import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.Test;
import pe.edu.pucp.softinv.dao.ServicioDAO;
import pe.edu.pucp.softinv.daoImp.ServicioDAOImpl;
import pe.edu.pucp.softinv.model.Servicio.ServicioDTO;
import pe.edu.pucp.softinv.model.Servicio.TipoServicio;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Tag;

@Tag("integration")
public class ServicioDAOImplTest {
    private static ServicioDAO servicioDAO;
    private ArrayList<ServicioDTO> servicios;

    @BeforeAll
    static void setUp() {
        servicioDAO = new ServicioDAOImpl();
    }

    void llenarDatos(ServicioDTO servicio){
        servicio.setNombre("ServiceSample");
        servicio.setTipo(TipoServicio.CORPORAL);
        servicio.setPrecio(99.90);
        servicio.setDescripcion("Este es una descripción de prueba. El servicio no es real, mas sí adquirible");
        servicio.setUrlImagen("ejemploURL.com");
        servicio.setDuracionHora(2);
        servicio.setPromedioValoracion(3.4);
        servicio.setActivo(1);
    }

    @Test
    void testInsertarServicio(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int id_servicio_insertado = servicioDAO.insertar(servicio);
        servicio.setIdServicio(id_servicio_insertado);
        System.out.println(id_servicio_insertado);
        assertTrue(id_servicio_insertado>0, "El servicio debería insertarse correctamente");
        servicioDAO.eliminar(servicio);
    }

    @Test
    void testObtenerServicioPorId(){
        ServicioDTO servicio = new ServicioDTO();
        ServicioDTO servicioID = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        servicioID = servicioDAO.obtenerPorId(servicio.getIdServicio());
        assertNotNull(servicioID, "El servicio debe existir en la BD");
        assertEquals("ServiceSample", servicio.getNombre());
        servicioDAO.eliminar(servicio);
    }
    
    @Test
    void testModificarServicio(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        servicio.setNombre("ServiceModificado");
        servicio.setPrecio(130.99);
        int result = servicioDAO.modificar(servicio);
        assertTrue(result!=0);
        ServicioDTO actualizado = servicioDAO.obtenerPorId(servicio.getIdServicio());
        assertEquals("ServiceModificado", actualizado.getNombre(), "El nombre debe haberse actualizado");
        assertEquals(130.99, actualizado.getPrecio(), "El precio debe haberse actualizado");
        servicioDAO.eliminar(servicio);
    }

    @Test
    void testEliminarServicio(){
        ServicioDTO servicio = new ServicioDTO();
        llenarDatos(servicio);
        int resul = servicioDAO.insertar(servicio);
        servicio.setIdServicio(resul);
        servicioDAO.eliminar(servicio);
        servicio = servicioDAO.obtenerPorId(resul);
        assertNull(servicio, "El servicio ya no debería existir");
    }
    

}
