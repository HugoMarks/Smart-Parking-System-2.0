$(document).ready(function () {

    $("#addLocalAdminBtn").on('click', function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        postLocalAdminData();
    });

    $("#addLocalAdminModal").on('hidden.bs.modal', function () {
        $(this).find("input").val(null).closest(".form-group").removeClass("has-success").removeClass("has-error");
        $(".address").slideUp();
    });
});

function postLocalAdminData() {
    var data = $("#registerLocalAdminForm").serialize();

    $.post("/LocalAdmin/Register", data)
    .done(function (args) {
        $.get("/TokenGenerator/GenerateAntiForgeryToken", function (html) {
            var tokenValue = $('<div />').html(html).find('input[type="hidden"]').val();
            $('#registerLocalAdminForm input[type="hidden"]').val(tokenValue);
        });

        $("#addLocalAdminModal").modal('hide');
    })
    .fail(function (arg) {

    });
}