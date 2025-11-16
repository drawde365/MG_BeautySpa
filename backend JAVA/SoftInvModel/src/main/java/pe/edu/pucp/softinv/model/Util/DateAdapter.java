package pe.edu.pucp.softinv.model.util;

import java.util.Date;
import java.text.SimpleDateFormat;
import jakarta.xml.bind.annotation.adapters.XmlAdapter;

public class DateAdapter extends XmlAdapter<String, Date> {
    // Formato estándar que C# entiende
    private final SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");

    @Override
    public String marshal(Date v) throws Exception {
        if (v == null) return null;
        return dateFormat.format(v); // Convierte java.util.Date a String
    }
    @Override
    public Date unmarshal(String v) throws Exception {
        if (v == null || v.isEmpty()) return null;
        return dateFormat.parse(v); // Convierte String a java.util.Date
    }
}