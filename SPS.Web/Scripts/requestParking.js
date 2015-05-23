$(document).ready(function () {
    $("#requestParkingLink").on('click', function () {
        $.post("/Client/RequestParking", { parkingCNPJ: '88.985.513/0001-40' })
        .done(function (data) {
            alert("Done!");
        })
        .fail(function (data) {
            alert("Failed!")
        });
    });
});