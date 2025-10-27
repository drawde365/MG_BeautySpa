<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MGBeautySpaWebAplication.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Inicio de Sesión | MG Beauty Spa</title>

    <!-- Fuentes (prioridad para ZCOOL XiaoWei y Poppins) -->
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=ZCOOL+XiaoWei&family=Poppins:wght@300;400;500;600&display=swap" rel="stylesheet" />

    <!-- Estilos del sistema -->
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/icomoon/icomoon.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/css/vendor.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css?v=3") %>" />

    <link rel="icon" type="image/svg+xml" href="<%: ResolveUrl("~/Content/images/MGFavicon.svg") %>" />
</head>

<body class="page-auth">
    <form id="form1" runat="server" class="min-vh-100 d-flex align-items-center justify-content-center">
        <div class="card shadow-sm p-4" style="max-width: 380px; width: 100%;">
            <div class="text-center mb-4">
                <img src="<%: ResolveUrl("~/Content/images/MGLogo2.svg") %>" alt="MG Beauty Spa" style="height:60px;">
                <h5 class="mt-3 fw-semibold">Bienvenido a MG Beauty Spa</h5>
                <small class="text-muted">Inicia sesión o continúa como invitado</small>
            </div>

            <!-- Email -->
            <div class="mb-3">
                <label for="txtCorreo" class="form-label">Correo electrónico</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="correo@ejemplo.com"></asp:TextBox>
            </div>

            <!-- Contraseña -->
            <div class="mb-3">
                <label for="txtContrasena" class="form-label">Contraseña</label>
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="Ingrese su contraseña"></asp:TextBox>
            </div>

            <!-- Error -->
            <asp:Label ID="lblError" runat="server" CssClass="text-danger small d-block mb-3"></asp:Label>

            <!-- Botones -->
            <asp:Button ID="btnIngresar" runat="server" Text="Iniciar sesión"
                CssClass="btn btn-primary w-100 mb-2" OnClick="btnIngresar_Click" />

            <asp:Button ID="btnInvitado" runat="server" Text="Continuar sin cuenta"
                CssClass="btn btn-outline-secondary w-100" OnClick="btnInvitado_Click" />

            <!-- Registro -->
           <div class="text-center mt-3">
                <small>
                    <a href="Cuenta/Registro.aspx" class="text-decoration-none">
                        ¿No tienes una cuenta asociada? ¡Regístrate!
                    </a>
                </small>
            </div>
            <!-- Recuperar Contraseña -->
            <div class="text-center mt-3">
                <small>
                    <a href="Cuenta/RecuperarContraseña.aspx" class="text-decoration-none">
                        ¿Olvidaste tu contraseña?</a>
                </small>
            </div>
        </div>
    </form>

    <!-- Scripts -->
    <script src="<%: ResolveUrl("~/Scripts/jquery.min.js") %>"></script>
    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
