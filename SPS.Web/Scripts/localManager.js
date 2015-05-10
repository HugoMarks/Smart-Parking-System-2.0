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

    $("#registerLocalAdminSubmitBtn").on('click', function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        postLocalManagerData();
    });

    $("#addLocalAdminModal").on('hidden.bs.modal', function () {
        $(this).find("input").val(null).closest(".form-group").removeClass("has-success").removeClass("has-error");
        $(".address").slideUp();
    });

    $("#cepTextBox").mask("00000-000");
    $("#cpfTextBox").mask('000.000.000-00', { reverse: true });
    $("#rgTextBox").mask("00.000.000-0");
    $("#phoneTextBox").mask(SPMaskBehavior, spOptions);

    $("#cepTextBox").blur(getLocalManagerAddress);
    $("#cpfTextBox").blur(onCPFTextBoxChanged);
    $("#cpfTextBox").on("keyup", onCPFTextBoxChanged);
});

function onCPFTextBoxChanged(event) {
    if (!validateCPF()) {
        addError(event.target);
        return;
    }

    addSuccess(event.target);
}

function getLocalManagerAddress() {
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

function validateCPF() {
    var cpfTextBox = $("#cpfTextBox");
    var cpf = cpfTextBox.val().replace(/[^\d]+/g, '');

    if (cpf == '') {
        return false;
    }

    if (cpf.length != 11 ||
        cpf == "00000000000" ||
        cpf == "11111111111" ||
        cpf == "22222222222" ||
        cpf == "33333333333" ||
        cpf == "44444444444" ||
        cpf == "55555555555" ||
        cpf == "66666666666" ||
        cpf == "77777777777" ||
        cpf == "88888888888" ||
        cpf == "99999999999") {
        return false;
    }

    // Valida 1o digito 
    add = 0;

    for (i = 0; i < 9; i++)
        add += parseInt(cpf.charAt(i)) * (10 - i);

    rev = 11 - (add % 11);

    if (rev == 10 || rev == 11)
        rev = 0;

    if (rev != parseInt(cpf.charAt(9))) {
        return false;
    }

    // Valida 2o digito 
    add = 0;
    for (i = 0; i < 10; i++)
        add += parseInt(cpf.charAt(i)) * (11 - i);

    rev = 11 - (add % 11);

    if (rev == 10 || rev == 11)
        rev = 0;

    if (rev != parseInt(cpf.charAt(10))) {
        return false;
    }

    return true;
}

function addError(element) {
    $(element).closest(".form-group").addClass("has-error");
    $(element).closest(".form-group").removeClass("has-success");
}

function addSuccess(element) {
    $(element).closest(".form-group").addClass("has-success");
    $(element).closest(".form-group").removeClass("has-error");
}

function postLocalManagerData() {
    $.post("/Account/RegisterLocalManager", $("#registerLocalAdminForm").serialize())
    .done(function () {
        $("#addLocalAdminModal").modal('hide');
    });
}