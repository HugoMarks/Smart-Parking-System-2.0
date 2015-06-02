$(document).ready(function () {
    function update() {
        var span = $(".numero-de-vagas");
        var cnpj = span.attr("data-parking-cnpj");
        var timeout;

        $.post("/Parking/GetFreeSpacesCount", { parkingCNPJ: cnpj })
        .done(function (data) {
            span.html(data.Count);
            timeout = setTimeout(update, 3000);
        })
        .fail(function (data) {
            clearTimeout(timeout);
        });
    }

    update();
});