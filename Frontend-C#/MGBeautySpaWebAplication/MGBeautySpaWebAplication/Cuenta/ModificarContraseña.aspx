<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModificarContraseña.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.WebForm2" %>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Establecer Contraseña | MG Beauty Spa</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=ZCOOL+XiaoWei&family=Poppins:wght@300;400;500;600&family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">

    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css?v=3") %>" />
    
    <style>
        .toggle-password { cursor: pointer; color: #6c757d; }
        .toggle-password:hover { color: #1EC3B6; }
    </style>
</head>

<body class="page-auth" style="background-color: #f0f2f5;">

    <form id="form1" runat="server" class="min-vh-100 d-flex align-items-center justify-content-center py-4" novalidate>
        
        <div class="card shadow-sm p-4 p-md-5 text-center" style="max-width: 450px; width: 100%; border-radius: 16px;">
            
            <div class="mb-4">
                <div style="width: 80px; height: 80px; background-color: #e0f7fa; border-radius: 50%; display: inline-flex; align-items: center; justify-content: center;">
                    <i class="bi bi-shield-lock-fill" style="font-size: 40px; color: #1EC3B6;"></i>
                </div>
            </div>

            <h3 class="fw-bold mb-2" style="font-family: 'Plus Jakarta Sans', sans-serif; color: #1C0D12;">Nueva contraseña</h3>
            <p class="text-muted mb-4 small">
                Crea una nueva contraseña segura para acceder a tu cuenta.
            </p>

            <div class="text-start mb-3">
                <label for="txtPassword" class="form-label fw-medium">Contraseña</label>
                <div class="input-group">
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña" />
                    <span class="input-group-text bg-white">
                        <i class="bi bi-eye-slash toggle-password" onclick="togglePassword('<%= txtPassword.ClientID %>', this)"></i>
                    </span>
                </div>
                <div class="mt-1">
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria." CssClass="text-danger small fw-bold" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revPasswordLength" runat="server" ControlToValidate="txtPassword"
                        ValidationExpression="^.{1,30}$" ErrorMessage="Máximo 30 caracteres." 
                        CssClass="text-danger small fw-bold" Display="Dynamic" />
                </div>
            </div>

            <div class="text-start mb-4">
                <label for="txtConfirmar" class="form-label fw-medium">Confirmar contraseña</label>
                <div class="input-group">
                    <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirme su contraseña" />
                    <span class="input-group-text bg-white">
                        <i class="bi bi-eye-slash toggle-password" onclick="togglePassword('<%= txtConfirmar.ClientID %>', this)"></i>
                    </span>
                </div>
                <div class="mt-1">
                    <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server" ControlToValidate="txtConfirmar"
                        ErrorMessage="Debe confirmar su contraseña." CssClass="text-danger small fw-bold" Display="Dynamic" />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmar" ControlToCompare="txtPassword"
                        ErrorMessage="Las contraseñas no coinciden." CssClass="text-danger small fw-bold" Display="Dynamic" />
                </div>
            </div>

            <div class="mb-3">
                <asp:Button ID="btnModificaContraseña" runat="server" Text="Establecer contraseña" 
                    CssClass="btn btn-primary w-100 py-2 rounded-pill fw-bold" 
                    Style="background: #1EC3B6; border: none;"
                    OnClick="btnModificaContraseña_Click" />
            </div>

        </div>

        <div class="modal fade" id="modalExito" tabindex="-1" aria-hidden="true" data-bs-backdrop="static">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content text-center p-4" style="border-radius: 16px;">
                    <div class="modal-body">
                        <div class="mb-3">
                            <i class="bi bi-check-circle-fill text-success" style="font-size: 3rem;"></i>
                        </div>
                        <h4 class="fw-bold mb-3">¡Contraseña actualizada!</h4>
                        <p class="text-muted mb-4">Tu contraseña ha sido modificada correctamente. Ya puedes iniciar sesión.</p>
                        <a href="../Login.aspx" class="btn btn-primary w-100 rounded-pill" style="background: #1EC3B6; border: none;">Iniciar Sesión</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalError" tabindex="-1" aria-hidden="true" data-bs-backdrop="static">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content text-center p-4" style="border-radius: 16px;">
                    <div class="modal-body">
                        <div class="mb-3">
                            <i class="bi bi-exclamation-circle-fill text-danger" style="font-size: 3rem;"></i>
                        </div>
                        <h4 class="fw-bold mb-3 text-danger">Enlace no válido</h4>
                        <p class="text-muted mb-4">Este enlace ya fue utilizado, expiró o no es válido. Por favor solicita uno nuevo.</p>
                        <a href="RecuperarContraseña.aspx" class="btn btn-danger w-100 rounded-pill">Solicitar nuevo enlace</a>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
    
    <script>
        function togglePassword(textBoxId, icon) {
            var input = document.getElementById(textBoxId);
            if (input.type === "password") {
                input.type = "text";
                icon.classList.remove("bi-eye-slash");
                icon.classList.add("bi-eye");
            } else {
                input.type = "password";
                icon.classList.remove("bi-eye");
                icon.classList.add("bi-eye-slash");
            }
        }
    </script>
</body>
</html>