$(document).ready(function () {
    $("#selectLocalAdminBtn").on('click', function () {
        $("#editLocalAdminForm").submit();
    });

    $.get("/LocalAdmin/GetLocalAdmins").done(function (html) {
        $("#selectLocalAdminContainer").html(html);

        $("#LocalAdminsDropDownList").on('change', function () {
            if (this.value == null || this.value == "") {
                $("#selectLocalAdminBtn").attr('disabled', 'disabled');
            } else {
                $("#selectLocalAdminBtn").removeAttr('disabled');
            }
        });
    });
});