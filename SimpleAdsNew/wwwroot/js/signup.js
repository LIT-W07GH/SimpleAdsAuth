$(() => {
    $("#email").on('blur', function() {
        const email = $("#email").val();
        $.get(`/account/emailavailable?email=${email}`, function(obj) {
            if (obj.isAvailable) {
                $("#unavailable").hide();
            } else {
                $("#unavailable").show();
            }
        });
    });
});