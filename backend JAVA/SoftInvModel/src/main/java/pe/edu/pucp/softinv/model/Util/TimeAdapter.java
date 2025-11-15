package pe.edu.pucp.softinv.model.util; // O el paquete que prefieras

import java.sql.Time;
import jakarta.xml.bind.annotation.adapters.XmlAdapter;

public class TimeAdapter extends XmlAdapter<String, Time> {

    // 1. De Objeto (Time) a String (para enviar en SOAP)
    @Override
    public String marshal(Time v) throws Exception {
        if (v == null) {
            return null;
        }
        return v.toString(); // Convierte Time a "09:00:00"
    }

    // 2. De String (recibido de SOAP) a Objeto (Time)
    @Override
    public Time unmarshal(String v) throws Exception {
        if (v == null || v.isEmpty()) {
            return null;
        }
        return Time.valueOf(v); // Convierte "09:00:00" a Time
    }
}