$(function() {
    $('.deleteNoun').click(function() {
        alert("adsf");
    });
    $('.deleteAdjective').click(function () {
        alert("afdgsdfgdfgdsf");
    });
    $('#newUserWrapper').hide();
    $('#nounsWrapper').hide();
    $('#adjectivesWrapper').hide();
    $('#addNewUserToggle').click(function() {
        $('#newUserWrapper').toggle();
    });
    $('#nounsToggle').click(function () {
        $('#nounsWrapper').toggle();
    });
    $('#adjectivesToggle').click(function () {
        $('#adjectivesWrapper').toggle();
    });
});