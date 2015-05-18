$(document).ready(function () {
    $("#selectParkingBtn").on('click', function () {
        $("#editParkingForm").submit();
    });

    $("#ParkingSelectList").on('change', function () {
        debugger;
        if (this.value == null || this.value == "") {
            $("#selectParkingBtn").attr('disabled', 'disabled');
        } else {
            $("#selectParkingBtn").removeAttr('disabled');
        }
    });
});