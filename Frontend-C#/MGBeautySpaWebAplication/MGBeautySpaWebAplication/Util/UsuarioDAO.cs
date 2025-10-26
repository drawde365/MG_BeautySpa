using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGBeautySpaWebAplication.Util
{
    public static class UsuarioDAO
    {
        public static UsuarioDTO ValidarLogin(string correo, string contrasena)
        {
            // ⚠️ Ejemplo de prueba — luego conectarás con tu backend o base de datos
            if (correo == "admin@gmail.com" && contrasena == "123")
                return new UsuarioDTO { Id = 1, Nombre = "Mirelly Garcia", Rol = "Administrador" };

            if (correo == "empleado@gmail.com" && contrasena == "123")
                return new UsuarioDTO { Id = 2, Nombre = "Miguel Guanira", Rol = "Empleado" };

            if (correo == "cliente@gmail.com" && contrasena == "123")
                return new UsuarioDTO { Id = 3, Nombre = "Ronny Cueva", Rol = "Cliente" };

            return null;
        }
    }
}