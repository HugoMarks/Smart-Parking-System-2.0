$(document).ready(function () {
    $("#requestTagLink").on('click', function () {
        $('#confirmRequestModal').modal('show');
    });

    $('#confirmRequestBtn').click(function () {
        $.post("/Collaborator/RequestNewTag").done(function () {
            alert("Ok");
        });
    });
});