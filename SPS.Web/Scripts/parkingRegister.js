$(document).ready(function () {
	var SPMaskBehavior = function (val) {
		return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
	},
	spOptions = {
		onKeyPress: function (val, e, field, options) {
			field.mask(SPMaskBehavior.apply({}, arguments), options);
		}
	};

	if ($("#parkingCepTextBox").val() == null || $("#parkingCepTextBox").val() == "") {
	    $(".address").hide();
	}

	$("#registerParkingSubmitBtn").click(function (event) {
	    event.preventDefault();
	    event.stopImmediatePropagation();
	    postParkingData();
	});

	$("#addParkingModal").on('hidden.bs.modal', function () {
	    $(this).find("input").val(null).closest(".form-group").removeClass("has-success").removeClass("has-error");
	    $(".address").slideUp();
	});

	$("#parkingCepTextBox").mask("00000-000");
	$("#parkingPhoneTextBox").mask(SPMaskBehavior, spOptions);

	$("#parkingCepTextBox").blur(getParkingAddress);
});

function getParkingAddress() {
	var cep = $("#parkingCepTextBox").val();

	if (cep == null || cep == "") {
		return;
	}

	$.post("/Account/GetAddress", { postalCode: cep })
		.done(function (data) {
		    var address = JSON.parse(data);

		    $("#parkingStreetTextBox").val(address.Street).attr("readonly", "readonly");
		    $("#parkingSquareTextBox").val(address.Square).attr("readonly", "readonly");
		    $("#parkingCityTextBox").val(address.City).attr("readonly", "readonly");
		    $("#parkingStateTextBox").val(address.State).attr("readonly", "readonly");
		})
		.fail(function (jqXHR, textStatus, message) {
		    $("#parkingStreetTextBox").val(null).removeAttr("readonly");
		    $("#parkingSquareTextBox").val(null).removeAttr("readonly");
		    $("#parkingCityTextBox").val(null).removeAttr("readonly");
		    $("#parkingStateTextBox").val(null).removeAttr("readonly");
		})
        .always(function () {
            $(".address").slideDown();
        });
}

function postParkingData() {
    $.post("/Parking/Register", $("#registerParkingForm").serialize())
    .done(function () {
        $("#addParkingModal").modal('hide');
    });
}