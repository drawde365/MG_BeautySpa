<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModificarContraseña.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.WebForm2" %>
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
</head> 
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Establecer Contraseña</title>
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet">

    <style>
        body, html {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        .page-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 0px;
            width: 100%;
            min-height: 800px;
            background: #ECFFFD;
            justify-content: center;
        }

        .content-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
        }

        .title-container {
            padding: 20px 16px 12px;
            width: 960px;
            box-sizing: border-box;
        }

        .title-text {
            width: 100%;
            margin: 0;
            font-weight: 700;
            font-size: 28px;
            line-height: 35px;
            text-align: center;
            color: #1C0D12;
        }

        .subtitle-container {
            padding: 4px 16px 12px;
            width: 960px;
            box-sizing: border-box;
        }

        .subtitle-text {
            width: 100%;
            margin: 0;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            text-align: center;
            color: #1C0D12;
        }

        .input-wrapper {
            padding: 12px 16px;
            width: 480px;
            max-width: 480px;
            box-sizing: border-box;
        }

        .form-input {
            box-sizing: border-box;
            display: flex;
            flex-direction: row;
            align-items: center;
            padding: 15px;
            width: 100%;
            height: 56px;
            background: #FFFFFF;
            border: 1px solid #E3D4D9;
            border-radius: 12px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            color: #1C0D12;
        }
        
        .form-input::placeholder {
            color: #757575;
        }

        .button-wrapper {
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: flex-start;
            padding: 12px 16px;
            width: 960px;
            box-sizing: border-box;
        }

        .submit-button {
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: center;
            padding: 0px 16px;
            width: 480px;
            max-width: 480px;
            height: 40px;
            background: #1EC3B6;
            border-radius: 20px;
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 15px;
            line-height: 21px;
            text-align: center;
            color: #FCF7FA;
            
            border: none;
            cursor: pointer;
        }
        .modal-superpuesto {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;

            background-color: rgba(0, 0, 0, 0.6); 

            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            z-index: 1000;
        }

        .success-box {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }

        .success-title {
            width: 100%;
            padding: 20px 16px 12px;
            box-sizing: border-box;
            margin: 0;
            font-weight: 700;
            font-size: 28px;
            line-height: 35px;
            text-align: center;
            color: #1A0F12;
        }

        .success-message {
            width: 100%;
            padding: 4px 16px 12px;
            box-sizing: border-box;
            margin: 0;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            text-align: center;
            color: #1A0F12;
        }

        .button-wrapper {
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 12px 16px;
        }

        .login-button {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 0px 16px;
            width: 122px;
            height: 40px;
            background: #1EC3B6;
            border-radius: 20px;
            font-family: 'Plus Jakarta Sans';
            font-weight: 700;
            font-size: 14px;
            line-height: 21px;
            text-align: center;
            color: #FFFFFF;
            text-decoration: none; 
            cursor: pointer;
            border: none;
        }

        .error-box {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }

        .error-title {
            width: 100%;
            padding: 20px 16px 12px;
            margin: 0;
            font-weight: 700;
            font-size: 28px;
            text-align: center;
            color: #B71C1C;
        }

        .error-message {
            width: 100%;
            padding: 4px 16px 12px;
            margin: 0;
            font-size: 16px;
            text-align: center;
            color: #1A0F12;
        }

        /* Botón rojo para errores */
        .login-button.error-btn {
            background: #E53935 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="formRegistro">
            <div class="page-container">
                <main class="content-wrapper">
            
                    <div class="title-container">
                        <h1 class="title-text">Establece tu nueva contraseña</h1>
                    </div>

                    <div class="subtitle-container">
                        <p class="subtitle-text">
                            Crea la nueva contraseña con la que inciarás sesión en tu cuenta.
                        </p>
                    </div>

                    <div class="input-wrapper">
                        <div class="password-wrapper">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input"
                                         TextMode="Password" placeholder="Ingrese tu nueva contraseña" />
                            <i class="bi bi-eye-slash toggle-password"
                               onclick="togglePassword('txtPassword', this)"></i>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="La contraseña es obligatoria." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator 
                            ID="revPasswordLength" runat="server"
                            ControlToValidate="txtPassword"
                            ValidationExpression="^.{1,30}$"
                            ErrorMessage="La contraseña no puede exceder los 30 caracteres."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                
                    <div class="input-wrapper" style="padding-top: 0;">
                        <div class="password-wrapper">
                            <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-input"
                                         TextMode="Password" placeholder="Confirme su contraseña" />
                            <i class="bi bi-eye-slash toggle-password"
                               onclick="togglePassword('txtConfirmar', this)"></i>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server" ControlToValidate="txtConfirmar"
                            ErrorMessage="Debe confirmar su contraseña." ForeColor="Red" Display="Dynamic" />
                        <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmar" ControlToCompare="txtPassword"
                            ErrorMessage="Las contraseñas no coinciden." ForeColor="Red" Display="Dynamic" />
                    </div>
                    <div class="button-container">
                        <asp:Button ID="btnModificaContraseña" 
                                    runat="server" 
                                    Text="Establecer nueva contraseña" 
                                    CssClass="submit-button"
                                    OnClick="btnModificaContraseña_Click" />
                    </div>

                </main>
            </div>
        </div>

        <div id="modalExito" class="modal-superpuesto" style="display: none;">
            <div class="success-box">
                <h1 class="success-title">Contraseña modificada</h1>
                <p class="success-message">
                    Su contraseña ha sido modificado correctamente. Ya puede iniciar sesión con su nueva contraseña.
                </p>
                <div class="button-wrapper">
                    <a href="../Login.aspx" class="login-button">
                        Iniciar Sesión
                    </a>
                </div>
            </div>
        </div>

        <div id="modalError" class="modal-superpuesto" style="display: none;">
            <div class="error-box">
                <h1 class="error-title">Enlace no válido</h1>
                <p class="error-message">
                    Este enlace ya fue utilizado, expiró o no es válido. Por favor solicita uno nuevo.
                </p>
                <div class="button-wrapper">
                    <a href="RecuperarContraseña.aspx" class="login-button" style="background:#E53935;">
                        Solicitar nuevo enlace
                    </a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>