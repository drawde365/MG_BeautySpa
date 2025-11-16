<%@ Page Title="Añadir Producto" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="InsertarProducto.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.InsertarProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .h1-add-product {
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
            color: #5A5A5A;
        }

        .form-control[TextMode="MultiLine"] {
            height: 144px;
        }

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
            color: #148C76;
        }
         .input-group .form-control.pre-ml {
            border-right: 0;
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

        .tipo-producto-radios,
        .tipo-piel-checkboxes {
            display: flex;
            flex-wrap: wrap;
            gap: 16px 24px;
            width: 100%;
        }

        .tipo-producto-radios > span,
        .tipo-piel-checkboxes > span {
            display: inline-flex;
            align-items: center;
            white-space: nowrap;
        }

        .tipo-producto-radios label,
        .tipo-piel-checkboxes label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            color: #5A5A5A;
            cursor: pointer;
        }

        .tipo-producto-radios input,
        .tipo-piel-checkboxes input {
            margin-right: 8px;
        }

        .tipo-ingrediente-bloque {
            border: 1px solid #E3D4D9;
            border-radius: 12px;
            padding: 16px;
            margin-bottom: 24px;
            background-color: #FAFAFA;
            max-width: 480px;
        }

        .tipo-ingrediente-bloque > .form-label-tipo {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 1.1rem;
            font-weight: 700;
            color: #148C76;
            display: block;
            margin-bottom: 12px;
        }

        .stock-input-wrapper {
            max-width: 200px;
        }
        
        .validation-error {
            display: block;
            color: #C31E1E;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 14px;
            font-weight: 500;
            margin-top: 4px;
        }
        
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="h1-add-product" ID="h1Titulo" runat="server">Añadir Producto</h1>

    <div class="form-group-wrapper">
        <label for="<%= txtTitulo.ClientID %>" class="form-label">Título</label>
        <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Ingrese el título" MaxLength="45"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvTitulo" runat="server" 
            ControlToValidate="txtTitulo" 
            ErrorMessage="El título es obligatorio." 
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
    </div>

    <div class="form-group-wrapper">
        <label for="<%= txtDescripcion.ClientID %>" class="form-label">Descripción</label>
        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Ingrese la descripción" TextMode="MultiLine" Rows="5" MaxLength="500"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
            ControlToValidate="txtDescripcion" 
            ErrorMessage="La descripción es obligatoria." 
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
    </div>

    <div class="form-group-wrapper">
        <label class="form-label">Tipo de Producto</label>
        <asp:RadioButtonList ID="rblTipoProducto" runat="server" CssClass="tipo-producto-radios" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Value="Facial" Text="Facial" Selected="True" />
            <asp:ListItem Value="Corporal" Text="Corporal" />
        </asp:RadioButtonList>
    </div>

    <div id="pnlTiposPiel" class="form-group-wrapper" style="display: none;">
        <label class="form-label">Tipos de Piel (para Facial)</label>
        <asp:CheckBoxList ID="cblTiposPiel" runat="server" CssClass="tipo-piel-checkboxes" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Value="Grasa" Text="Grasa" />
            <asp:ListItem Value="Mixta" Text="Mixta" />
            <asp:ListItem Value="Sensible" Text="Sensible" />
            <asp:ListItem Value="Seca" Text="Seca" />
        </asp:CheckBoxList>
    </div>

    <div id="ingredientesContainer" class="mb-3">
    </div>

    <asp:HiddenField ID="hdnIngredientesPorTipo" runat="server" />

    <div class="form-group-wrapper">
         <label for="<%= txtPrecio.ClientID %>" class="form-label">Precio</label>
         <div class="input-group">
             <span class="input-group-text pre-precio">S/.</span>
             <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control post-precio" placeholder="Ingrese el precio"></asp:TextBox>
         </div>
         <asp:RequiredFieldValidator ID="rfvPrecio" runat="server" 
            ControlToValidate="txtPrecio" 
            ErrorMessage="El precio es obligatorio." 
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
         <asp:CompareValidator ID="cvPrecio" runat="server" 
            ControlToValidate="txtPrecio" 
            Operator="DataTypeCheck" 
            Type="Double"
            ErrorMessage="El precio debe ser un número (ej. 120.50)."
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
        
        <asp:RangeValidator ID="rvPrecio" runat="server"
            ControlToValidate="txtPrecio"
            MinimumValue="0"
            MaximumValue="1000000"
            Type="Double"
            ErrorMessage="El precio no puede ser negativo."
            CssClass="validation-error"
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
    </div>

    <div class="form-group-wrapper">
        <label for="<%= txtTamaño.ClientID %>" class="form-label">Tamaño</label>
        <div class="input-group">
            <asp:TextBox ID="txtTamaño" runat="server" CssClass="form-control pre-ml" placeholder="Ingrese el tamaño"></asp:TextBox>
            <span class="input-group-text post-ml">ml</span>
        </div>
        <asp:RequiredFieldValidator ID="rfvTamaño" runat="server" 
            ControlToValidate="txtTamaño" 
            ErrorMessage="El tamaño es obligatorio." 
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
        <asp:CompareValidator ID="cvTamaño" runat="server" 
            ControlToValidate="txtTamaño" 
            Operator="DataTypeCheck" 
            Type="Double"
            ErrorMessage="El tamaño debe ser un número (ej. 250)."
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
        
        <asp:RangeValidator ID="rvTamaño" runat="server"
            ControlToValidate="txtTamaño"
            MinimumValue="0"
            MaximumValue="100000"
            Type="Double"
            ErrorMessage="El tamaño no puede ser negativo."
            CssClass="validation-error"
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
    </div>

    <div class="form-group-wrapper">
        <label for="<%= txtComoUsar.ClientID %>" class="form-label">Cómo usar</label>
        <asp:TextBox ID="txtComoUsar" runat="server" CssClass="form-control" placeholder="Ingrese cómo se debería usar" TextMode="MultiLine" Rows="5" MaxLength="150"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvComoUsar" runat="server" 
            ControlToValidate="txtComoUsar" 
            ErrorMessage="El modo de uso es obligatorio." 
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto" />
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
        <asp:CustomValidator ID="cvImagen" runat="server" 
            ErrorMessage="Debe seleccionar una imagen para el producto nuevo."
            CssClass="validation-error" 
            Display="Dynamic"
            ValidationGroup="GuardarProducto"
            ClientValidationFunction="validateImageUpload" />
    </div>
    
    <asp:Label ID="litError" runat="server" CssClass="text-danger d-block mb-3"></asp:Label>
    
    <div class="d-flex justify-content-end gap-2 mt-4 mb-3">
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-admin btn-admin-cancel" OnClick="btnCancelar_Click" CausesValidation="false" />
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Producto" CssClass="btn btn-admin btn-admin-submit" OnClick="btnInsertarProducto_Click" ValidationGroup="GuardarProducto" />
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    
    <script type="text/javascript">
        
        function validateImageUpload(source, args) {
            const fileInput = document.getElementById('<%= fileUpload.ClientID %>');
            const hiddenImage = document.getElementById('<%= hdnImagenActual.ClientID %>');

            if (fileInput.files.length > 0) {
                args.IsValid = true;
                return;
            }

            if (hiddenImage.value !== '') {
                args.IsValid = true;
                return;
            }

            args.IsValid = false;
        }

        $(document).ready(function () {

            $(".file-upload-input").on("change", function () {
                const file = this.files[0];
                const $wrapper = $(this).closest(".file-upload-wrapper");
                const $label = $wrapper.find(".file-upload-label");
                const $labelText = $wrapper.find(".file-upload-text");
                const $labelStrong = $wrapper.find("strong");

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
                        $labelStrong.text("Archivo seleccionado:");
                        $labelText.text(file.name);
                        $wrapper.css("background-image", "none");
                        $wrapper.removeClass("has-preview");
                        $label.show();
                    }
                } else {
                    $labelStrong.text("Subir imagen");
                    $labelText.text("Arrastra y suelta o haz click para subir");
                    $wrapper.css("background-image", "none");
                    $wrapper.removeClass("has-preview");
                    $wrapper.removeClass("has-preview");
                    $label.show();
                }
            });

            const $rblTipoProducto = $("#<%= rblTipoProducto.ClientID %> input");
            const $cblTiposPiel = $("#<%= cblTiposPiel.ClientID %> input");
            const $pnlTiposPiel = $("#pnlTiposPiel");
            const $ingredientesContainer = $("#ingredientesContainer");
            const $hdnIngredientes = $("#<%= hdnIngredientesPorTipo.ClientID %>");

            function actualizarFormularioTipos() {
                const mainType = $rblTipoProducto.filter(":checked").val();
                let tiposParaIngredientes = [];

                if (mainType === "Facial") {
                    $pnlTiposPiel.slideDown();
                    $cblTiposPiel.filter(":checked").each(function () {
                        tiposParaIngredientes.push($(this).val());
                    });
                } else {
                    $pnlTiposPiel.slideUp();
                    if (mainType) {
                        tiposParaIngredientes.push(mainType);
                    }
                }

                $ingredientesContainer.empty();

                let oldData = {};
                try {
                    JSON.parse($hdnIngredientes.val() || '[]').forEach(item => {
                        oldData[item.tipo] = item;
                    });
                } catch (e) { }

                tiposParaIngredientes.forEach(function (tipo) {

                    const oldItem = oldData[tipo] || {};
                    const oldIngredientes = oldItem.ingredientes || "";
                    // ▼▼▼ CAMBIO AQUÍ: Asegura que el valor no sea negativo al cargar ▼▼▼
                    const oldStock = (oldItem.stock || 0) < 0 ? 0 : (oldItem.stock || 0);

                    const html = `
                        <div class="tipo-ingrediente-bloque" data-tipo="${tipo}">
                            <label class="form-label-tipo">Para: ${tipo}</label>
                            
                            <div class="form-group-wrapper">
                                <label class="form-label" for="ing-${tipo}">Ingredientes (Max 100 caracteres)</label>
                                <textarea id="ing-${tipo}" 
                                          class="form-control ingredientes-input" 
                                          placeholder="Ingrese ingredientes para el tipo de piel ${tipo}" 
                                          rows="3"
                                          maxlength="100" 
                                          data-tipo="${tipo}">${oldIngredientes}</textarea>
                            </div>
                            
                            <div class="form-group-wrapper stock-input-wrapper">
                                <label class="form-label" for="stock-${tipo}">Stock de este tipo</label>
                                <input type="number" id="stock-${tipo}" 
                                       class="form-control stock-input" 
                                       placeholder="0" 
                                       value="${oldStock}"
                                       min="0" <%-- <-- Validación HTML5 --%>
                                       data-tipo="${tipo}" />
                            </div>
                        </div>
                    `;
                    $ingredientesContainer.append(html.replace(/textarea/g, 'textarea'));
                });

                serializarIngredientes();
            }

            function serializarIngredientes() {
                let data = [];
                $ingredientesContainer.find(".tipo-ingrediente-bloque").each(function () {
                    const $bloque = $(this);
                    const tipo = $bloque.data("tipo");
                    const ingredientes = $bloque.find(".ingredientes-input").val();
                    const stock = $bloque.find(".stock-input").val();

                    // ▼▼▼ CAMBIO AQUÍ: Validación final antes de guardar en JSON ▼▼▼
                    let stockNum = stock ? parseInt(stock) : 0;
                    if (stockNum < 0) {
                        stockNum = 0; // Fuerza a 0 si es negativo
                    }

                    data.push({
                        tipo: tipo,
                        ingredientes: ingredientes,
                        stock: stockNum
                    });
                });

                $hdnIngredientes.val(JSON.stringify(data));
            }

            $rblTipoProducto.on("change", actualizarFormularioTipos);
            $cblTiposPiel.on("change", actualizarFormularioTipos);

            $ingredientesContainer.on("input", ".ingredientes-input, .stock-input", serializarIngredientes);

            actualizarFormularioTipos();
        });
    </script>
</asp:Content>