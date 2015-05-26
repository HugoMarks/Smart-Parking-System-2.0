$(document).ready(function () {
    var currentCPNJ = "";

    (function ($) {
        $('#filter').keyup(function () {
            var rex = new RegExp($(this).val(), 'i');

            $('.searchable tr').hide();
            $('.searchable tr').filter(function () {
                var text = $(this).text().trim();
                var query = text.split('\n')[0];

                return rex.test(query);
            }).show();

        });
    }(jQuery));

    $("#attachToParkingBtn").click(function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        $("#confirmAttachModal").modal('show');
    });

    $("#confirmAttachBtn").click(function () {
        $.post("/Parking/RequestAttach", { cnpj: currentCPNJ })
        .done(function (data) {
            $("#parkingDetailsModal").modal('hide');
        })
        .fail(function (data) {
            alert('Erro! Você já está vinculado a um estacionamento.');
            $("#parkingDetailsModal").modal('hide');
        });
    });

    $(".mouse-hand").click(function () {
        $("#parkingDetailsModal").modal('show');

        $("#parkingDetailsModalTitle").html("Carregando");
        $("#parkingDetailsModalContent").html('<p>Carregando...</p>');

        currentCPNJ = $(this).attr("data-parking");

        $.post("/Parking/GetDetails", { cnpj: currentCPNJ })
        .done(function (data) {
            if (!data.Success) {
                $("#parkingDetailsModalTitle").html("Erro");
                $("#parkingDetailsModalContent").html('<p>' + data.Error + '</p>');
                return;
            }

            var parking = data.Data;
            var content = '';

            $("#parkingDetailsModalTitle").html(parking.Name);

            content += '<p><b>Nome</b></p>' + '<p>' + parking.Name + '</p>';
            content += '<p><b>CNPJ</b></p>' + '<p>' + parking.CNPJ + '</p>';
            content += '<p><b>Endereço</b></p>';
            content += '<p>' + parking.Address.Street + ', ' + parking.StreetNumber + '</p>';
            content += '<p>' + parking.Address.Square + '</p>';
            content += '<p>' + parking.Address.City + ', ' + parking.Address.State + '</p>';
            content += '<p><b>Contato</b></p>' + '<p>' + parking.Number + '</p>';
            content += '<p><b>Vagas</b></p>' + '<p>' + parking.Spaces + '</p>';

            $("#parkingDetailsModalContent").html('<p>' + content + '</p>');
        });
    });
});