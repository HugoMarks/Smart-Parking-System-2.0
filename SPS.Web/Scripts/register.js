$(document).ready(function () {
	var SPMaskBehavior = function (val) {
		return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
	},
	spOptions = {
		onKeyPress: function (val, e, field, options) {
			field.mask(SPMaskBehavior.apply({}, arguments), options);
		}
	};

	$("#cepTextBox").mask("00000-000");
	$("#cpfTextBox").mask('000.000.000-00', { reverse: true });
	$("#rgTextBox").mask("00.000.000-0");
	$("#phoneTextBox").mask(SPMaskBehavior, spOptions);

	$("#cepTextBox").blur(getAddress);

	function getAddress() {
		var cep = $("#cepTextBox").val();

		if (cep == null || cep == "")
			return;

		$.ajax({
			type: "POST",
			contentType: "application/json",
			url: "/Account/GetAddress",
			data: JSON.stringify({ postalCode: cep }),
			dataType: "json",
			success: function (data) {
				$("#streetTextBox").val(data.Street);
				$("#cityTextBox").val(data.City);
				$("#stateTextBox").val(data.State);
			}
		});
	}
});