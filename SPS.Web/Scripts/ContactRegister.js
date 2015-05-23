$(document).ready(function () {
    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
	spOptions = {
	    onKeyPress: function (val, e, field, options) {
	        field.mask(SPMaskBehavior.apply({}, arguments), options);
	    }
	};

    $("#phoneTextBox").mask(SPMaskBehavior, spOptions);
});

function addError(element) {
    $(element).closest(".form-group").addClass("has-error");
    $(element).closest(".form-group").removeClass("has-success");
}

function addSuccess(element) {
    $(element).closest(".form-group").addClass("has-success");
    $(element).closest(".form-group").removeClass("has-error");
}

