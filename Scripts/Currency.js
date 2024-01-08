var appkey = '';

$(function () {
    appkey = '6a9f75e56be4f62d1af715f2';
    //Bind Currency code dropdown
    $.get("https://v6.exchangerate-api.com/v6/" + appkey + "/codes", {}, function (data) {
        var row = "";
        $.each(data.supported_codes, function (i, v) {
            row += "<option value='" + v[0] + "'>" + v[0] + '-' + v[1] + "</option>";
        });
        $("#ddlform").append(row);
        $("#ddlTo").append(row);
        $("#ddlform").val('USD');
        $("#ddlTo").val('EUR');
        calculate();
    }).fail(function (d) {
        alert("Error while calling API");
        console.log(d.responseText);
    });
});

//calculate currency conversion
function calculate() {
    $.get("https://v6.exchangerate-api.com/v6/" + appkey + "/pair/" + $("#ddlform").val() + "/" + $("#ddlTo").val() + "/" + Number($("#txtamount").val()) + "", {}, function (data) {
        $.get("#txtoutput").val(data.conversion_result);
    }).fail(function (d) {
        alert("Error while calling API");
        console.log(d.responseText);
    });

}