$(document).ready(function () {
    var arrayImgComment = new CustomArrayImg("imgCommentFile", "imgComment");
    $(document).delegate('.btnOpenImgComment','click',function () {
        $('#imgCommentFile').trigger('click');
    });

    $(document).delegate('#imgCommentFile','change',function () {
        arrayImgComment.replaceImg();
        //$(this).val('');
    })

    ////Delete imgComments
    //$(document).delegate('.DeleteImgComments', 'click', function () {
    //    var li = $(this).closest("li");
    //    li.remove();
    //});

    function SendComment(data) {
        $.ajax({
            type: 'POST',
            url: '/SinhVien/ThaoLuan/SendComment',
            processData: false,
            contentType: false,
            data: data,
            //async: false,
            success: function (response) {
                if (response.status == true) {
                    $('#inputComments').val('');
                    $("#imgCommentFile").val('');
                    $("#imgComment").empty();
                    var cardbody = $('#bodyComments');
                    
                    var img = '';
                    if (response.data.anhDinhKem != null)
                        img = '<img style="width:30%; height:30%" data-toggle="modal" data-target="#modalImg" data-src="/../../img/GVHD/Comments/' + response.data.anhDinhKem + '" src="/../../img/GVHD/Comments/' + response.data.anhDinhKem + '" alt="Attachment">';
                    var html = '<div class="card-comment">' +
                        '<img class="img-circle img-sm" src="../dist/img/user3-128x128.jpg" alt="User Image">' +
                        '<div class="comment-text">' +
                        '<span class="username">' + response.data.nguoiComment + '<span class="text-muted float-right">' + response.data.ngayPost + '</span>' +
                        '</span><!-- /.username -->' +
                        '<p style="white-space: pre-line">' + response.data.noiDungComment+'</p>' +
                        '</div>' +
                        '<span class="mailbox-attachment-icon has-img">' +
                        img +
                        '</span>' +
                        '</div>';
                    cardbody.prepend(html);
                }
                else {
                    toastr.error(response.mess);
                }
            },
        });
    }

    $(document).delegate('.btnSendComment','click',function () {
        var comment = $('#inputComments').val();
        var id = $("#valueIdBaiPost").val();
        var data = new FormData();
        var files = $("#imgCommentFile").get(0).files;
        data.append('Files', files[0]);
        data.append('NoiDungComment', comment);
        data.append('IdbaiPost', id);
        SendComment(data);
    });
   
    //$(document).delegate('#showMore','click',function (e) {
    //    e.preventDefault();
    //    var length = $('#bodyComments .card-comment').length;
        
    //})
});