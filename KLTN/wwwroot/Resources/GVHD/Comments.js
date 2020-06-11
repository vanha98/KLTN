$(document).ready(function () {

    $('.btnOpenImgComment').click(function () {
        $('#imgCommentFile').trigger('click');
    });

    $('.btnSendComment').click(function () {
        var comment = $('#inputComments').val();
    });
});