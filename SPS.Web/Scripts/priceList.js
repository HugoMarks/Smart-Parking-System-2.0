$(document).ready(function () {
    $(".mouse-hand").click(function () {
        var id = $(this).attr('data-price');

        window.location.href = "/Price/Edit/" + id;
    });
});