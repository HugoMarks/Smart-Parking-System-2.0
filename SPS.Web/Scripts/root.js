$(document).ready(function () {
    $("#cpfTextBox").mask("000.000.000-00");

    $("#modalBtn").click(function () {
        $.post("Root/GenerateKey").done(function (data) {
            $(".modal-body").append("<p class='text-center'> " + data.key + " </p>");
        });
    });
});