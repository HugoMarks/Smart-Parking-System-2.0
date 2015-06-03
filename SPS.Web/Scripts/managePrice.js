$(document).ready(function () {
    $(".input-time").mask("00:00");
    $('.input-price').mask('000.000.000.000.000,00', { reverse: true });
    $(".input-time").timepicker({
        minuteStep: 5,
        showInputs: false,
        disableFocus: true,
        showMeridian: false
    });

    $('#removePriceLink').on('click', function () {
        $('removeForm').submit();
    });

    $('#IsDefault').on('change', function () {
        if ($(this).is(":checked")) {
            $('.toggle-group').fadeOut();
            $("#StartTime").val('00:00');
            $("#EndTime").val('23:59');
        }
        else {
            $('.toggle-group').fadeIn();
            $("#StartTime").val('');
            $("#EndTime").val('');
        }
    });

    if ($('input:checked').length > 0) {
        $('.toggle-group').hide();
    }
});