$(document).ready(function () {
    $("#requestTagLink").on('click', function () {
        var parkings = parseInt($("#requestTagLink").attr('data-parkings'));

        if (parkings > 0) {
            $('#confirmRequestModal').modal('show');
        } else {
            $('#errorRequestModal').modal('show');
        }
    });

    $('#confirmRequestBtn').click(function () {
        $.post("/Collaborator/RequestNewTag").done(function () {
            alert("Ok");
        });
    });
});