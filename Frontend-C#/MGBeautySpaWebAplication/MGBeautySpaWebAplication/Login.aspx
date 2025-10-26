<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MGBeautySpaWebAplication.Login" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Inicio de Sesión | MG Beauty Spa</title>

    <!-- Estilos locales -->
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/icomoon/icomoon.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/css/vendor.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css") %>" />
</head>

<body class="bg-light">
    <form id="form1" runat="server" class="vh-100 d-flex align-items-center justify-content-center">
        <div class="card shadow-sm p-4" style="max-width: 380px; width: 100%;">
            <div class="text-center mb-4">
                <img src="<%: ResolveUrl("~/Content/images/main-logo.png") %>" alt="MG Beauty Spa" style="height:60px;">
                <h5 class="mt-2 fw-semibold">Bienvenida a MG Beauty Spa</h5>
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
                <asp:TextBox ID="txtContrasena" runat="server" TextMode="Password" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
            </div>

            <asp:Label ID="lblError" runat="server" CssClass="text-danger small d-block mb-3"></asp:Label>

            <!-- Botones -->
            <asp:Button ID="btnIngresar" runat="server" Text="Iniciar sesión" CssClass="btn btn-primary w-100 mb-2" OnClick="btnIngresar_Click" />
            <asp:Button ID="btnInvitado" runat="server" Text="Continuar sin cuenta" CssClass="btn btn-outline-secondary w-100" OnClick="btnInvitado_Click" />

            <div class="text-center mt-3">
                <small><a href="#" class="text-decoration-none">¿Olvidaste tu contraseña?</a></small>
            </div>
        </div>
    </form>

    <!-- Scripts locales -->
    <script src="<%: ResolveUrl("~/Scripts/jquery.min.js") %>"></script>
    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>
