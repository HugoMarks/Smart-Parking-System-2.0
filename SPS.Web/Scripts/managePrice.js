$(document).ready(function () {
    $(".input-time").mask("00:00");
    $('.input-price').mask('000.000.000.000.000,00', { reverse: true });
    $(".input-time").timepicker({
        minuteStep: 5,
        showInputs: false,
        disableFocus: true,
        showMeridian: false
    });
});