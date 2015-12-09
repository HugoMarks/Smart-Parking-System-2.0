$(document).ready(function () {
    $('.date-time').mask('00/00/0000 00:00');
    //$('.date-time').datetimepicker({
    //    locale: moment.locale('pt')
    //});
    $('.decimal').mask('000.000.000.000.000,00', { reverse: true });
    $('.tag-id').mask('AA AA AA AA');
    $('.plate-id').mask('AAA-0000');

    $('.add-record-toggle').hide();

    $('#searchButton').on('click', function () {
        $.post('/Collaborator/GetRecordFromTag', { tagId: $('#Tag').val() })
        .done(function (data) {
            if (data.Success) {
                $('#EnterDateTime').val(data.Record.EnterTime);
                $('#ExitDateTime').val(data.Record.ExitTime);
                $('#TotalHours').val(data.Record.TotalHours);
                $('#TotalCash').val(data.Record.TotalValue);
                $('.add-record-toggle').slideDown();
            }
        });
    });

    $('#attachTagButton').click(function () {
        var form = $('#attachTagForm');

        form.validate();

        if (!form.valid()) {
            return;
        }

        $.post("/Collaborator/AttachTag", form.serialize())
        .done(function (data) {
            $('#attachResultModalContainer').html('<p>' + data.Message + '<p>');
            $('#attachResultModal').modal('show');
            
            if (data.Success) {
                $('#UserEmail').val('');
                $('#TagId').val('');
            }
        });
    });

    $('#attachPlateButton').click(function () {
        var form = $('#attachPlateForm');

        form.validate();

        if (!form.valid()) {
            return;
        }

        $.post("/Collaborator/AttachPlate", form.serialize())
        .done(function (data) {
            $('#attachPlateResultModalContainer').html('<p>' + data.Message + '<p>');
            $('#attachPlateResultModal').modal('show');

            if (data.Success) {
                $('#UserEmail').val('');
                $('#PlateId').val('');
            }
        });
    });
});