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

        .file-upload-wrapper.has-preview {
            border-style: solid;
            border-color: #148C76;
            background-size: cover; /* Hace que la imagen llene el contenedor */
            background-position: center;
            background-repeat: no-repeat;
        }

/* Cuando tenemos un preview, ocultamos el texto ("Subir imagen", etc.)
        */
        .file-upload-wrapper.has-preview .file-upload-label {
            display: none;
        }

        .tipo-producto-radios,
        .tipo-piel-checkboxes {
        display: flex;       /* ¡La clave! */
        flex-wrap: wrap;     /* ¡La responsividad! */
        gap: 16px 24px;      /* Espacio vertical y horizontal entre items */
        width: 100%;         /* Asegura que ocupe el ancho disponible */
        }

        /* * Hijos (los <span> que envuelven cada checkbox/radio)
        * Les decimos que no se partan por dentro (ej. no separar el check del texto)
        */
        .tipo-producto-radios > span,
        .tipo-piel-checkboxes > span {
        display: inline-flex;  /* Mantiene el input y el label juntos */
        align-items: center;   /* Alinea verticalmente el check/radio con el texto */
        white-space: nowrap; /* Evita que "Tipo de Piel" se parta */
        }

        /* * Estilo del texto (Label)
        * (Quitamos 'margin-right' porque ahora 'gap' lo maneja mejor)
        */
        .tipo-producto-radios label,
        .tipo-piel-checkboxes label {
        font-family: 'Plus Jakarta Sans', sans-serif;
        font-size: 16px;
        color: #5A5A5A;
        cursor: pointer;
        /* margin-right: 20px; <-- ELIMINADO */
        }

        /* * Estilo del Input (Checkbox/Radio)
        * (Se mantiene igual)
        */
        .tipo-producto-radios input,
        .tipo-piel-checkboxes input {
            margin-right: 8px;
        }

        .tipo-ingrediente-bloque {
            border: 1px solid #E3D4D9; /* Borde sutil */
            border-radius: 12px;
            padding: 16px;
            margin-bottom: 24px;
            background-color: #FAFAFA; /* Fondo ligeramente gris */
            max-width: 480px;
        }

        /* Título del bloque (ej. "Para: Piel Grasa") */
        .tipo-ingrediente-bloque > .form-label-tipo {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 1.1rem;
            font-weight: 700;
            color: #148C76; /* Color de tu tema */
            display: block;
            margin-bottom: 12px;
        }

        /* Ajuste para que los campos de stock no sean tan anchos */
        .stock-input-wrapper {
            max-width: 200px; /* Ancho máximo para el campo de stock */
        }
    </style>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h1 class="h1-add-product" ID="h1Titulo" runat="server">Añadir Producto</h1>

    <div class="form-group-wrapper">
        <label for="txtTitulo" class="form-label">Título</label>
        <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" placeholder="Ingrese el título"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label for="txtDescripcion" class="form-label">Descripción</label>
        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Ingrese la descripción" TextMode="MultiLine" Rows="5"></asp:TextBox>
    </div>

    <div class="form-group-wrapper">
        <label class="form-label">Tipo de Producto</label>
        <asp:RadioButtonList ID="rblTipoProducto" runat="server" CssClass="tipo-producto-radios" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Value="Facial" Text="Facial" Selected="True" />
            <asp:ListItem Value="Corporal" Text="Corporal" />
        </asp:RadioButtonList>
    </div>

    <%-- Este panel solo se mostrará si se elige "Facial" --%>
    <div id="pnlTiposPiel" class="form-group-wrapper" style="display: none;">
        <label class="form-label">Tipos de Piel (para Facial)</label>
        <asp:CheckBoxList ID="cblTiposPiel" runat="server" CssClass="tipo-piel-checkboxes" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Value="Grasa" Text="Grasa" />
            <asp:ListItem Value="Mixta" Text="Mixta" />
            <asp:ListItem Value="Sensible" Text="Sensible" />
            <asp:ListItem Value="Seca" Text="Seca" />
        </asp:CheckBoxList>
    </div>

    <%-- Aquí se generarán dinámicamente los campos de ingredientes --%>
    <div id="ingredientesContainer" class="mb-3">
        </div>

    <%-- Este campo oculto guardará los ingredientes como un JSON --%>
    <asp:HiddenField ID="hdnIngredientesPorTipo" runat="server" />

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
        <label for="txtComoUsar" class="form-label">Cómo usar</label>
        <asp:TextBox ID="txtComoUsar" runat="server" CssClass="form-control" placeholder="Ingrese cómo se debería usar" TextMode="MultiLine" Rows="5"></asp:TextBox>
    </div>
    <!--
    <div class="form-group-wrapper">
        <label for="ddlTipoProducto" class="form-label">Tipo de producto</label>
        <asp:DropDownList ID="ddlTipoProducto" runat="server" CssClass="form-select">
            <asp:ListItem Value="" Text="Seleccione"></asp:ListItem>
            <asp:ListItem Value="Facial" Text="Facial"></asp:ListItem>
            <asp:ListItem Value="Corporal" Text="Corporal"></asp:ListItem>
            <asp:ListItem Value="Cabello" Text="Cabello"></asp:ListItem>
        </asp:DropDownList>
    </div>
    -->

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
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Producto" CssClass="btn btn-admin btn-admin-submit" OnClick="btnInsertarProducto_Click" />
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    
    <%-- 1. AÑADIMOS JQUERY (Necesario para que '$' funcione) --%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    
    <%-- 2. Corregí el 'type' de tu etiqueta script --%>
    <script type="text/javascript">
        $(document).ready(function () {

            // --- INICIO: LÓGICA DE UPLOAD DE IMAGEN (Tu script original) ---
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
                    $label.show();
                }
            });
            // --- FIN: LÓGICA DE UPLOAD DE IMAGEN ---


            // 1. Selectores (igual que antes)
            const $rblTipoProducto = $("#<%= rblTipoProducto.ClientID %> input");
            const $cblTiposPiel = $("#<%= cblTiposPiel.ClientID %> input");
            const $pnlTiposPiel = $("#pnlTiposPiel");
            const $ingredientesContainer = $("#ingredientesContainer");
            const $hdnIngredientes = $("#<%= hdnIngredientesPorTipo.ClientID %>");

            // 2. Función que actualiza la UI (MODIFICADA)
            function actualizarFormularioTipos() {
                const mainType = $rblTipoProducto.filter(":checked").val();
                let tiposParaIngredientes = [];

                // A. Mostrar/Ocultar panel (igual que antes)
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

                // B. (Re)Generar los campos (MODIFICADO para incluir STOCK)
                $ingredientesContainer.empty(); // Limpia campos antiguos

                // Lee los datos antiguos del JSON (si existen) para repoblar
                let oldData = {};
                try {
                    JSON.parse($hdnIngredientes.val() || '[]').forEach(item => {
                        oldData[item.tipo] = item;
                    });
                } catch (e) { }

                tiposParaIngredientes.forEach(function (tipo) {

                    // Obtiene valores antiguos si existen
                    const oldItem = oldData[tipo] || {};
                    const oldIngredientes = oldItem.ingredientes || "";
                    const oldStock = oldItem.stock || "";

                    // Crea el HTML para el bloque completo
                    const html = `
                        <div class="tipo-ingrediente-bloque" data-tipo="${tipo}">
                            <label class="form-label-tipo">Para: ${tipo}</label>
                            
                            <div class="form-group-wrapper">
                                <label class="form-label" for="ing-${tipo}">Ingredientes</label>
                                <textarea id="ing-${tipo}" 
                                          class="form-control ingredientes-input" 
                                          placeholder="Ingrese ingredientes para el tipo de piel ${tipo}" 
                                          rows="3"
                                          data-tipo="${tipo}">${oldIngredientes}</textarea>
                            </div>
                            
                            <div class="form-group-wrapper stock-input-wrapper">
                                <label class="form-label" for="stock-${tipo}">Stock de este tipo</label>
                                <input type="number" id="stock-${tipo}" 
                                       class="form-control stock-input" 
                                       placeholder="0" 
                                       value="${oldStock}"
                                       min="0"
                                       data-tipo="${tipo}" />
                            </div>
                        </div>
                    `;
                    // Usamos .replace() para que 'textarea' funcione
                    $ingredientesContainer.append(html.replace(/textarea/g, 'textarea'));
                });

                // C. Actualizar el campo oculto
                serializarIngredientes();
            }

            // 3. Función que serializa los datos (MODIFICADA)
            function serializarIngredientes() {
                let data = [];
                // Itera sobre cada BLOQUE (no sobre cada input)
                $ingredientesContainer.find(".tipo-ingrediente-bloque").each(function () {
                    const $bloque = $(this);
                    const tipo = $bloque.data("tipo");
                    const ingredientes = $bloque.find(".ingredientes-input").val();
                    const stock = $bloque.find(".stock-input").val(); // <-- OBTIENE EL STOCK

                    data.push({
                        tipo: tipo,
                        ingredientes: ingredientes,
                        // Convierte el stock a número, o 0 si está vacío
                        stock: stock ? parseInt(stock) : 0
                    });
                });

                // Convertimos a JSON y guardamos (igual que antes)
                $hdnIngredientes.val(JSON.stringify(data));
            }

            // 4. Asignar los Eventos (igual que antes)
            $rblTipoProducto.on("change", actualizarFormularioTipos);
            $cblTiposPiel.on("change", actualizarFormularioTipos);

            // 5. Evento para actualizar el JSON (MODIFICADO)
            //    Ahora escucha AMBOS campos (ingredientes y stock)
            //    Usamos 'input' en vez de 'keyup' para capturar cambios en el campo numérico
            $ingredientesContainer.on("input", ".ingredientes-input, .stock-input", serializarIngredientes);

            // 6. Carga inicial (igual que antes)
            actualizarFormularioTipos();

            // --- FIN: NUEVA LÓGICA DE TIPOS E INGREDIENTES ---
        });
    </script>
</asp:Content>