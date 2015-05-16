$(document).ready(function () {
    $("#salaryTextBox").mask('000.000.000.000.000,00', { reverse: true });

    $("#addCollaboratorBtn").on('click', function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        postCollaboratorData();
    });

    $("#addCollaboratorModal").on('hidden.bs.modal', function () {
        $(this).find("input").val(null).closest(".form-group").removeClass("has-success").removeClass("has-error");
        $(".address").slideUp();
    });
});

function postCollaboratorData() {
    var data = $("#registerCollaboratorForm").serialize();

    $.post("/Collaborator/Register", data)
    .done(function (args) {
        $.get("/TokenGenerator/GenerateAntiForgeryToken", function (html) {
            var tokenValue = $('<div />').html(html).find('input[type="hidden"]').val();
            $('#registerLocalAdminForm input[type="hidden"]').val(tokenValue);
        });

        $("#addCollaboratorModal").modal('hide');
    })
    .fail(function (arg) {

    });
}