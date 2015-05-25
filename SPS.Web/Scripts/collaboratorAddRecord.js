$(document).ready(function () {
    $('.date-time').mask('00/00/0000 00:00');
    $('.date-time').datetimepicker({
        locale: moment.locale('pt')
    });
    $('.decimal').mask('000.000.000.000.000,00', { reverse: true });
    $('.tag-id').mask('AA AA AA AA');

    $('#attachTagButton').click(function () {
        var form = $('#attachTagForm');

        form.validate();

        if (!form.valid()) {
            return;
        }

        $.post("/Collaborator/AttachTag", form.serialize())
        .done(function (data) {

        });
    });
});