$(document).ready(function () {
    $(".mouse-hand").click(function () {
        var id = $(this).attr('data-space-number');

        window.location.href = "/ParkingSpace/Edit/" + id;
    });
});