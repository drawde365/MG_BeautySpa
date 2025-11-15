package pe.edu.pucp.softinv.model.Producto;

// Este es el DTO para la tabla TIPOS_PRODS
public class TipoProdDTO {
    private int id;
    private String nombre;

    // --- Constructores ---
    public TipoProdDTO() {}

    public TipoProdDTO(int id, String nombre) {
        this.id = id;
        this.nombre = nombre;
    }

    // --- Getters y Setters ---
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }
}