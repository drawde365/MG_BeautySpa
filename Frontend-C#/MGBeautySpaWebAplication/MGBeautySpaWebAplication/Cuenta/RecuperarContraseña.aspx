<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarContraseña.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.RecuperarContraseña" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Recuperar Contraseña | MG Beauty Spa</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=ZCOOL+XiaoWei&family=Poppins:wght@300;400;500;600&family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">

    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css?v=3") %>" />
</head>

<body class="page-auth" style="background-color: #f0f2f5;">

    <form id="form1" runat="server" class="min-vh-100 d-flex align-items-center justify-content-center py-4" novalidate>
        
        <div class="card shadow-sm p-4 p-md-5 text-center" style="max-width: 450px; width: 100%; border-radius: 16px;">
            
            <div class="mb-4">
                <div style="width: 80px; height: 80px; background-color: #e0f7fa; border-radius: 50%; display: inline-flex; align-items: center; justify-content: center;">
                    <i class="bi bi-lock-fill" style="font-size: 40px; color: #1EC3B6;"></i>
                </div>
            </div>

            <h3 class="fw-bold mb-2" style="font-family: 'Plus Jakarta Sans', sans-serif; color: #1C0D12;">¿Olvidaste tu contraseña?</h3>
            <p class="text-muted mb-4 small">
                Ingresa el correo electrónico asociado a tu cuenta y te enviaremos un enlace para restablecerla.
            </p>

            <div class="text-start mb-3">
                <label for="txtEmail" class="form-label fw-medium">Correo electrónico</label>
                <div class="input-group">
                    <span class="input-group-text bg-white text-muted"><i class="bi bi-envelope"></i></span>
                    
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                        TextMode="Email" placeholder="ejemplo@correo.com" 
                        ValidationGroup="Recuperar"/>
                </div>
                
                <div class="mt-1">
                    <%-- QUITAMOS 'd-block' DE LAS CLASES CSS --%>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                        ControlToValidate="txtEmail"
                        ValidationGroup="Recuperar"
                        ErrorMessage="El correo es obligatorio." 
                        CssClass="text-danger small fw-bold" 
                        Display="Dynamic" />
                    
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                        ControlToValidate="txtEmail"
                        ValidationGroup="Recuperar"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" 
                        ErrorMessage="Formato de correo inválido." 
                        CssClass="text-danger small fw-bold" 
                        Display="Dynamic" />
                </div>
            </div>

            <asp:Label ID="lblMensaje" runat="server" EnableViewState="false" />

            <div class="mb-4 mt-3">
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar enlace" 
                    ValidationGroup="Recuperar"
                    CssClass="btn btn-primary w-100 py-2 rounded-pill fw-bold" 
                    Style="background: #1EC3B6; border: none;"
                    OnClick="btnEnviar_Click"/>
            </div>

            <div class="d-flex justify-content-between align-items-center border-top pt-3">
                <a href="../Login.aspx" class="text-decoration-none text-muted small fw-semibold">
                    <i class="bi bi-arrow-left me-1"></i> Volver al Login
                </a>
                <a href="Registro.aspx" class="text-decoration-none fw-semibold small" style="color: #1EC3B6;">
                    Crear cuenta
                </a>
            </div>

        </div>
    </form>

    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
</body>
</html>