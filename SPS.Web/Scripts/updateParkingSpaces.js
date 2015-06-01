$(document).ready(function () {
    function update() {
        var span = $(".numero-de-vagas");
        var cnpj = span.attr("data-parking-cnpj");

        $.post("/Parking/GetFreeSpacesCount", { parkingCNPJ: cnpj })
        .done(function (data) {
            span.html(data.Count);
            setInterval(update, 3000);
        });
    }

    update();
});