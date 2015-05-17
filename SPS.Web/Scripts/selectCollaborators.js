$(document).ready(function () {
    $("#selectCollaboratorBtn").on('click', function () {
        $("#editCollaboratorForm").submit();
    });

    $.get("/Collaborator/GetCollaborators").done(function (html) {
        $("#selectCollaboratorContainer").html(html);

        $("#CollaboratorsDropDownList").on('change', function () {
            debugger;
            if (this.value == null || this.value == "") {
                $("#selectCollaboratorBtn").attr('disabled', 'disabled');
            } else {
                $("#selectCollaboratorBtn").removeAttr('disabled');
            }
        });
    });
});