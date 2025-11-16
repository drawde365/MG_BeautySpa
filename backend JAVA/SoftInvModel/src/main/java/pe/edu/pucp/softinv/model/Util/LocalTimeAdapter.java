package pe.edu.pucp.softinv.model.util;

import java.time.LocalTime;
import jakarta.xml.bind.annotation.adapters.XmlAdapter;

public class LocalTimeAdapter extends XmlAdapter<String, LocalTime> {
    @Override
    public String marshal(LocalTime v) throws Exception {
        if (v == null) return null;
        return v.toString(); // Convierte LocalTime a "09:00:00"
    }
    @Override
    public LocalTime unmarshal(String v) throws Exception {
        if (v == null || v.isEmpty()) return null;
        return LocalTime.parse(v); // Convierte "09:00:00" a LocalTime
    }
}