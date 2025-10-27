<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.WebForm1" %>
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
    <title>Crear una Cuenta</title>
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet">
        
    <style>
        html, body {
        height: 100%;
        }

        body, html {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Plus Jakarta Sans', sans-serif;
            background-color: #f0f0f0; 
        }

        .page-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 0px;
            width: 100%; 
            height: 100%;
            padding-top: 20px;
        }

        .content-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 16px;
        }

        .title-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 16px 12px;
            width: 960px;
            box-sizing: border-box; 
        }

        .title-text {
            width: 100%;
            height: 35px;
            margin: 0; 
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 28px;
            line-height: 35px;
            text-align: center;
            color: #1C0D12;
        }
        
        .form-group {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 7px 16px;
            gap: 10px;
            width: 480px;
            box-sizing: border-box;
        }

        .form-label {
            width: 100%; 
            height: 24px;
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            color: #1C0D12;
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
            color: #757575;
        }

        .terms-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 4px 16px 12px;
            width: 960px;
            box-sizing: border-box;
            margin-top: 10px;
        }

        .terms-text {
            width: 100%;
            height: 21px;
            margin: 0;
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            text-align: center;
            color: #107369;
        }

        .button-container {
            display: flex;
            flex-direction: column;
            align-items: center;
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
            border: none;
            cursor: pointer; 

            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 15px;
            line-height: 21px;
            text-align: center;
            color: #FCF7FA;
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
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div id="formRegistro">

            <div class="page-container">
                <main class="content-wrapper">

                    <div class="title-container">
                        <h1 class="title-text">Crear una cuenta</h1>
                    </div>
                
                    <div class="form-group">
                        <label for="nombre" class="form-label">Nombre</label>
                        <input type="text" id="nombre" class="form-input" placeholder="Ingrese su nombre">
                    </div>
                
                    <div class="form-group">
                        <label for="apellido" class="form-label">Apellido</label>
                        <input type="text" id="apellido" class="form-input" placeholder="Ingrese su apellido">
                    </div>

                    <div class="form-group">
                        <label for="celular" class="form-label">Número de celular</label>
                        <input type="tel" id="celular" class="form-input" placeholder="Ingrese su número de celular">
                    </div>

                    <div class="form-group">
                        <label for="email" class="form-label">Correo electrónico</label>
                        <input type="email" id="email" class="form-input" placeholder="Ingrese su correo electrónico">
                    </div>

                    <div class="form-group">
                        <label for="password" class="form-label">Contraseña</label>
                        <input type="password" id="password" class="form-input" placeholder="Ingrese su contraseña">
                    </div>

                    <div class="form-group">
                        <label for="confirm-password" class="form-label">Confirmar contraseña</label>
                        <input type="password" id="confirm-password" class="form-input" placeholder="Confirme su contraseña">
                    </div>

                    <div class="terms-container">
                        <p class="terms-text">
                            Al asociar una cuenta a MG Beatuy Spa, usted está aceptando nuestros Términos de Servicios y Políticas de Privacidad.
                        </p>
                    </div>

                    <div class="button-container">
                        <asp:Button ID="btnCrearCuenta" 
                                    runat="server" 
                                    Text="Crear Cuenta" 
                                    CssClass="submit-button"
                                    OnClick="btnCrearCuenta_Click" />
                    </div>

                </main>
            </div>
        </div>

        <div id="modalExito" class="modal-superpuesto" style="display: none;">
            <div class="success-box">
                <h1 class="success-title">Cuenta creada</h1>
                <p class="success-message">
                    Su cuenta se ha creado correctamente. Ya puede empezar a explorar nuestros productos y servicios.
                </p>
                <div class="button-wrapper">
                    <a href="../Login.aspx" class="login-button">
                        Iniciar Sesión
                    </a>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
