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

	$("#cepTextBox").mask("00000-000");
	$("#phoneTextBox").mask(SPMaskBehavior, spOptions);

	$("#cepTextBox").blur(getParkingAddress);
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