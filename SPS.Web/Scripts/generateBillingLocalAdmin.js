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

        $.post('/LocalAdmin/GenerateBilling', data)
        .done(function (result) {
            if (result.Success) {
                $('#billingContainer').html(result.Content).fadeIn();
            }
            else {
                $('.fields').html(result.Content);
                $('input').addClass('has-error');
            }
        });
    });
});