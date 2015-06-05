$(document).ready(function () {
	var SPMaskBehavior = function (val) {
		return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
	},
	spOptions = {
		onKeyPress: function (val, e, field, options) {
			field.mask(SPMaskBehavior.apply({}, arguments), options);
		}
	};

	if ($("#cepTextBox").val() == null || $("#cepTextBox").val() == "") {
	    $(".address").hide();
	}

	$("#addParkingModal").on('hidden.bs.modal', function () {
	    $(this).find("input").val(null).closest(".form-group").removeClass("has-success").removeClass("has-error");
	    $(".address").slideUp();
	});

	$("#CNPJ").mask("99.999.999/9999-99");
	$("#cepTextBox").mask("00000-000");
	$("#phoneTextBox").mask(SPMaskBehavior, spOptions);

	$("#cepTextBox").blur(getParkingAddress);
	$("#CNPJ").blur(onCNPJTextBoxChanged);
	$("#CNPJ").on("keyup", onCNPJTextBoxChanged);

	function onCNPJTextBoxChanged(event) {
	    if (!validateCNPJ(event.target.value)) {
	        addError(event.target);
	        return;
	    }

	    addSuccess(event.target);
	}

	function addError(element) {
	    $(element).closest(".form-group").addClass("has-error");
	    $(element).closest(".form-group").removeClass("has-success");
	}

	function addSuccess(element) {
	    $(element).closest(".form-group").addClass("has-success");
	    $(element).closest(".form-group").removeClass("has-error");
	}
});

function getParkingAddress() {
	var cep = $("#cepTextBox").val();

	if (cep == null || cep == "") {
		return;
	}

	$.post("/Account/GetAddress", { postalCode: cep })
		.done(function (data) {
		    var address = JSON.parse(data);

		    $("#streetTextBox").val(address.Street).attr("readonly", "readonly");
		    $("#squareTextBox").val(address.Square).attr("readonly", "readonly");
		    $("#cityTextBox").val(address.City).attr("readonly", "readonly");
		    $("#stateTextBox").val(address.State).attr("readonly", "readonly");
		})
		.fail(function (jqXHR, textStatus, message) {
		    $("#streetTextBox").val(null).removeAttr("readonly");
		    $("#squareTextBox").val(null).removeAttr("readonly");
		    $("#cityTextBox").val(null).removeAttr("readonly");
		    $("#stateTextBox").val(null).removeAttr("readonly");
		})
        .always(function () {
            $(".address").slideDown();
        });
}

function validateCNPJ(cnpj) {
    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;
}