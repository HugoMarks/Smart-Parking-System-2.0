$(document).ready(function () {
    $("#confirmDeleteButton").on('click', function () {
        deleteCollaborator();
    });

    function deleteCollaborator() {
        var userEmail = $("#Email").val();

        $.post("/Collaborator/Delete", { email: userEmail })
        .done(function (args) {
            window.location.href = "/LocalAdmin";
        })
        .fail(function (args) {
            alert(args);
        });
    }
});