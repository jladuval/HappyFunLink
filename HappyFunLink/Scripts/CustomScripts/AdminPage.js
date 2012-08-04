$(function () {
    var pattern = /[0-9]+/g;
    $('.deleteNoun').click(function() {
        var id = $(this).attr('id').match(pattern)[0];
        deleteNoun(id);
        $(this).closest('.inputDiv').remove();
        return false;
    });
    $('.deleteAdjective').click(function () {
        var id = $(this).attr('id').match(pattern)[0];
        deleteAdjective(id);
        $(this).closest('.inputDiv').remove();
        return false;
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

var deleteNoun = function(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/DeleteNoun",
        dataType: "json",
        data: {nounId: id}
    });
};

var deleteAdjective = function (id) {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/Admin/DeleteAdjective",
        data: { adjId: id }
    });
};