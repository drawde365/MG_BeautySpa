<%@ Page Title="Añadir Servicio" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="InsertarServicio.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.InsertarServicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .h1-add-service {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 32px;
            line-height: 40px;
            color: #1C0D12;
            margin-bottom: 16px; 
        }

        .form-group-wrapper {
            max-width: 480px; 
            margin-bottom: 24px; 
        }

        .form-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 16px;
            color: #1C0D12;
            margin-bottom: 8px; 
        }

        .form-control, .form-select {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            color: #5A5A5A; 
            background-color: #FFFFFF;
            border: 1px solid #E3D4D9; 
            border-radius: 12px; 
            padding: 15px; 
            height: 56px; 
            box-shadow: none;
        }

        .form-control::placeholder {
            color: #767676;
        }

        .form-control[TextMode="MultiLine"] {
            height: 144px; 
        }

        .input-group-text {
            background-color: #FFFFFF;
            border: 1px solid #E3D4D9;
            font-size: 16px;
            color: #767676;
            padding: 15px;
        }
        .input-group-text.pre-precio {
            border-right: 0;
            border-radius: 12px 0 0 12px;
        }
        .input-group .form-control.post-precio {
            border-left: 0;
        }

        .file-upload-wrapper {
            position: relative;
            width: 100%; 
            height: 168px; 
            border: 2px dashed #148C76; 
            border-radius: 12px; 
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
            cursor: pointer;
            overflow: hidden; 
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
            opacity: 0; 
            cursor: pointer;
        }
        .file-upload-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            color: #1C0D12;
            pointer-events: none; 
        }
        .file-upload-label strong {
            font-size: 18px; 
            font-weight: 700;
            display: block;
            margin-bottom: 8px; 
        }
        .file-upload-label .file-upload-text {
            font-size: 14px; 
            font-weight: 400;
        }

        .btn-admin {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 14px;
            border-radius: 20px; 
            height: 40px; 
            padding: 0 16px;
            border: none;
        }
        .btn-admin-cancel {
            background-color: #C31E1E; 
            color: #FCF7FA;
        }
        .btn-admin-cancel:hover {
            background-color: #a41919;
            color: #FCF7FA;
        }
        .btn-admin-submit {
            background-color: #1EC3B6; 
            color: #FCF7FA;
        }
        .btn-admin-submit:hover {
            background-color: #17a195;
            color: #FCF7FA;
        }

        /* --- ESTILOS AÑADIDOS PARA PREVIEW DE IMAGEN --- */
        .file-upload-wrapper.has-preview {
            border-style: solid;
            border-color: #148C76;
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
        }
        .file-upload-wrapper.has-preview .file-upload-label {
            display: none;
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="h1-add-service" ID="h1Titulo" runat="server">Añadir Servicio</h1>

    <div class="form-group-wrapper">
        <label for="txtTitulo" class="form-label">Título</label>
        <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Ingrese el título"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label for="txtDescripcion" class="form-label">Descripción</label>
        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Ingrese la descripción" TextMode="MultiLine" Rows="5"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label for="txtDuracion" class="form-label">Duración (horas)</label>
        <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" placeholder="Ej: 2" TextMode="Number"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
         <label for="txtPrecio" class="form-label">Precio</label>
         <div class="input-group">
             <span class="input-group-text pre-precio">S/.</span>
             <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control post-precio" placeholder="Ingrese el precio"></asp:TextBox>
         </div>
    </div>

    <div class="form-group-wrapper">
        <label for="ddlTipoServicio" class="form-label">Tipo de servicio</label>
        <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="form-select">
            <asp:ListItem Value="" Text="Seleccione"></asp:ListItem>
            <asp:ListItem Value="Facial" Text="Facial"></asp:ListItem>
            <asp:ListItem Value="Corporal" Text="Corporal"></asp:ListItem>
            <asp:ListItem Value="Terapia Complementaria" Text="Terapia Complementaria"></asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="mb-4">
        <label class="form-label">Subir imagen</label>
        <div class="file-upload-wrapper" ID="fileUploadWrapper" runat="server">
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="file-upload-input" />
            <div class="file-upload-label">
                <strong>Subir imagen</strong>
                <span class="file-upload-text">Arrastra y suelta o haz click para subir</span>
            </div>
            <asp:HiddenField ID="hdnImagenActual" runat="server" Value="" />
        </div>
    </div>

    <asp:Label ID="litError" runat="server" CssClass="text-danger d-block mb-3"></asp:Label>

    <div class="d-flex justify-content-end gap-2 mt-4 mb-3">
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-admin btn-admin-cancel" OnClick="btnCancelar_Click" CausesValidation="false" />
        <asp:Button ID="btnGuardar" runat="server" Text="Añadir Servicio" CssClass="btn btn-admin btn-admin-submit" OnClick="btnInsertarServicio_Click" />
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".file-upload-input").on("change", function () {
                const file = this.files[0];
                const $wrapper = $(this).closest(".file-upload-wrapper");
                const $label = $wrapper.find(".file-upload-label");

                if (file) {
                    if (file.type.startsWith("image/")) {
                        const reader = new FileReader();
                        reader.onload = function (e) {
                            $wrapper.css("background-image", "url(" + e.target.result + ")");
                            $wrapper.addClass("has-preview");
                            $label.hide();
                        };
                        reader.readAsDataURL(file);
                    } else {
                        const $labelStrong = $wrapper.find("strong");
                        const $labelText = $wrapper.find(".file-upload-text");
                        $labelStrong.text("Archivo seleccionado:");
                        $labelText.text(file.name);
                        $wrapper.css("background-image", "none");
                        $wrapper.removeClass("has-preview");
                        $label.show();
                    }
                } else {
                    const $labelStrong = $wrapper.find("strong");
                    const $labelText = $wrapper.find(".file-upload-text");
                    $labelStrong.text("Subir imagen");
                    $labelText.text("Arrastra y suelta o haz click para subir");
                    $wrapper.css("background-image", "none");
                    $wrapper.removeClass("has-preview");
                    $label.show();
                }
            });
        });
    </script>
</asp:Content>