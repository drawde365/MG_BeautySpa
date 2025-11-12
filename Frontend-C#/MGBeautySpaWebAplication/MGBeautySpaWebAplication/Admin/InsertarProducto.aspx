<%@ Page Title="Añadir Producto" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="InsertarProducto.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.InsertarProducto" %>

<%-- 
    1. BLOQUE DE ESTILOS
    Este CSS se insertará en el "HeadContent" de tu Master Page.
    Es necesario para que el formulario se vea como tu diseño.
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Título de la página (Añadir Producto) */
        .h1-add-product {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 32px;
            line-height: 40px;
            color: #1C0D12;
            margin-bottom: 16px; /* (gap: 12px + padding: 16px) -> ~28px */
        }

        /* Contenedor para cada campo del formulario */
        .form-group-wrapper {
            max-width: 480px; /* Ancho de tus campos (Depth 4, Frame 1) */
            margin-bottom: 24px; /* Espacio entre cada campo */
        }

        /* Etiquetas (Título, Descripción, etc.) */
        .form-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 16px;
            color: #1C0D12;
            margin-bottom: 8px; /* (padding: 0px 0px 8px) */
        }

        /* Todos los inputs (TextBox, DropDownList) */
        .form-control, .form-select {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            color: #5A5A5A;
            background-color: #FFFFFF;
            border: 1px solid #E3D4D9; /* 1px solid #E3D4D9 */
            border-radius: 12px; /* 12px */
            padding: 15px; /* 15px */
            height: 56px; /* 56px height */
            box-shadow: none; /* Quita el glow de Bootstrap */
        }

        .form-control::placeholder {
            color: #5A5A5A;
        }

        /* TextAreas (Descripción, Beneficios, etc.) */
        .form-control[TextMode="MultiLine"] {
            height: 144px; /* 144px */
        }

        /* Input Group (para Precio 'S/.' y Tamaño 'ml') */
        .input-group-text {
            background-color: #FFFFFF;
            border: 1px solid #E3D4D9;
            font-size: 16px;
            color: #5A5A5A;
            padding: 15px;
        }
        .input-group-text.pre-precio {
            border-right: 0;
            border-radius: 12px 0 0 12px;
        }
        .input-group .form-control.post-precio {
            border-left: 0;
        }
        .input-group-text.post-ml {
            border-left: 0;
            border-radius: 0 12px 12px 0;
            color: #148C76; /* Color 'ml' del diseño */
        }
         .input-group .form-control.pre-ml {
            border-right: 0;
        }

        /* Uploader de Archivos (Depth 4, Frame 9) */
        .file-upload-wrapper {
            position: relative;
            width: 100%; /* Ocupa todo el ancho del contenedor padre */
            height: 168px; /* 168px */
            border: 2px dashed #148C76; /* 2px dashed #148C76 */
            border-radius: 12px; /* 12px */
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
            cursor: pointer;
            overflow: hidden; /* Oculta el input feo */
            background-color: #fafcff;
        }
        .file-upload-wrapper:hover {
            background-color: #f7faff;
        }
        .file-upload-input {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            opacity: 0; /* Hace invisible el <input type="file"> */
            cursor: pointer;
        }
        .file-upload-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            color: #1C0D12;
            pointer-events: none; /* Permite que el click atraviese al input */
        }
        .file-upload-label strong {
            font-size: 18px; /* 18px */
            font-weight: 700;
            display: block;
            margin-bottom: 8px; /* 8px gap */
        }
        .file-upload-label .file-upload-text {
            font-size: 14px; /* 14px */
            font-weight: 400;
        }

        /* Botones (Depth 4, Frame 10) */
        .btn-admin {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 14px;
            border-radius: 20px; /* 20px */
            height: 40px; /* 40px */
            padding: 0 16px;
            border: none;
        }
        .btn-admin-cancel {
            background-color: #C31E1E; /* #C31E1E */
            color: #FCF7FA;
        }
        .btn-admin-cancel:hover {
            background-color: #a41919;
            color: #FCF7FA;
        }
        .btn-admin-submit {
            background-color: #1EC3B6; /* #1EC3B6 */
            color: #FCF7FA;
        }
        .btn-admin-submit:hover {
            background-color: #17a195;
            color: #FCF7FA;
        }

    </style>
</asp:Content>


<%-- 
    2. CONTENIDO PRINCIPAL
    Este es el formulario, que se insertará en el "MainContent"
    de tu Master Page (en el <main> blanco a la derecha).
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="h1-add-product">Añadir Producto</h1>

    <div class="form-group-wrapper">
        <label for="txtTitulo" class="form-label">Título</label>
        <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Ingrese el título"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label for="txtDescripcion" class="form-label">Descripción</label>
        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Ingrese la descripción" TextMode="MultiLine" Rows="5"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
         <label for="txtPrecio" class="form-label">Precio</label>
         <div class="input-group">
            <span class="input-group-text pre-precio">S/.</span>
            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control post-precio" placeholder="Ingrese el precio"></asp:TextBox>
        </div>
    </div>

    <div class="form-group-wrapper">
        <label for="txtTamaño" class="form-label">Tamaño</label>
        <div class="input-group">
            <asp:TextBox ID="txtTamaño" runat="server" CssClass="form-control pre-ml" placeholder="Ingrese el tamaño"></asp:TextBox>
            <span class="input-group-text post-ml">ml</span>
        </div>
    </div>

    <div class="form-group-wrapper">
        <label for="txtBeneficios" class="form-label">Beneficios</label>
        <asp:TextBox ID="txtBeneficios" runat="server" CssClass="form-control" placeholder="Ingrese los beneficios" TextMode="MultiLine" Rows="5"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label for="txtComoUsar" class="form-label">Cómo usar</label>
        <asp:TextBox ID="txtComoUsar" runat="server" CssClass="form-control" placeholder="Ingrese cómo se debería usar" TextMode="MultiLine" Rows="5"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label for="ddlTipoProducto" class="form-label">Tipo de producto</label>
        <asp:DropDownList ID="ddlTipoProducto" runat="server" CssClass="form-select">
            <asp:ListItem Value="" Text="Seleccione"></asp:ListItem>
            <asp:ListItem Value="Facial" Text="Facial"></asp:ListItem>
            <asp:ListItem Value="Corporal" Text="Corporal"></asp:ListItem>
            <asp:ListItem Value="Cabello" Text="Cabello"></asp:ListItem>
            <%-- Agrega más tipos de producto aquí --%>
        </asp:DropDownList>
    </div>

    <div class="mb-4">
        <label class="form-label">Subir imagen</label>
        <div class="file-upload-wrapper">
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="file-upload-input" />
            <div class="file-upload-label">
                <strong>Subir imagen</strong>
                <span class="file-upload-text">Arrastra y suelta o haz click para subir</span>
            </div>
        </div>
    </div>

    <div class="d-flex justify-content-end gap-2 mt-4 mb-3">
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-admin btn-admin-cancel" OnClick="btnCancelar_Click" CausesValidation="false" />
        <asp:Button ID="btnGuardar" runat="server" Text="Añadir Producto" CssClass="btn btn-admin btn-admin-submit" OnClick="btnInsertarProducto_Click" />
    </div>

</asp:Content>


<%-- 
    3. SCRIPTS (Opcional)
    Puedes agregar JS aquí si lo necesitas.
--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <%-- Por ejemplo, un script para mostrar el nombre del archivo cargado --%>
</asp:Content>