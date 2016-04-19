$(document).ready(function () {
    //tel het aantal aangevinkte vakjes
    var telAantal = function () {
        return $('#voorlichtingkeuzemenu input:checked').length;
    }
    $(".voorlichting").click(function () {
        var aangevinkt = telAantal();
        $("#teller").html(aangevinkt);
    });
});