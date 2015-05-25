$(document).ready(function () {
    $(".date-time").datetimepicker({
        locale: moment.locale('pt'),
        format: 'DD/MM/YYYY',
        maxDate: moment()
    });

    $('#billingContainer').fadeOut();

    $('#submitButton').click(function () {
        var form = $('form');

        form.validate();

        if (!form.valid()) {
            return;
        }

        var data = form.serialize();

        $.get('/LocalAdmin/GenerateBilling', data)
        .done(function (partial) {
            $('#billingContainer').html(partial).fadeIn();
        });
    });
});