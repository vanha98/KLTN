$(document).ready(function () {
    var currentImgSV = [];

    var arrayImgSV = new CustomArrayImg("imgFile", "imgAttach");
    //Delete imgTemp
    $(document).delegate('.DeleteImgTemp', 'click', function (e) {
        e.preventDefault();
        var file = $(this).data('id');
        var li = $(this).closest("li");
        li.remove();
        arrayImgSV.DeleteImgTemp(file);
    });

    //Save data Edit
    //add Img when Edit
    $(document).delegate('.btnaddImg', 'click', function () {
        $('#imgFile').trigger('click');

    });
    $(document).delegate('#imgFile', 'change', function () {
        arrayImgSV.pushImgToArray();
    });

    //Delete CurrentImg
    $(document).delegate('.DeleteImg', 'click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        currentImgSV.push(id);
        console.log(currentImgSV);
        var li = $(this).closest("li");
        li.remove();
        //DeleteImg(id);
    });

    //Huy Edit
    $(document).delegate('.btnHuyEdit', 'click', function () {
        var id = $('#valueIdBaiPost').val();
        ReloadNoiDung(id);
        //LoadNoiDung(id);
        //LoadComments(id);
    });

    //Luu chinh sửa
    $(document).delegate('.btnLuuEdit', 'click', function () {
        var id = $('#valueIdBaiPost').val();
        var NoiDung = $('#inputNoiDung').val();
        var TieuDe = $('#inputTieuDe').val();
        var data = new FormData();
        for (var i = 0; i < arrayImgSV.array.length; i++) {
            data.append('Files', arrayImgSV.array[i]);
        }
        data.append('NoiDung', NoiDung);
        data.append('Id', id);
        data.append('TieuDe', TieuDe);
        data.append('currentImg', currentImgSV);
        Edit(data, id);
    });

    
    //Edit BaiPost
    $(document).delegate('.btnEdit', 'click', function () {
        var txtNoiDung = $('#txtNoiDungBaiPost');
        txtNoiDung.prop('hidden', true);
        var txtTieuDe = $('#txtTieuDeBaiPost');
        txtTieuDe.prop('hidden', true);

        $('#inputTieuDe').prop('hidden', false);
        $('#inputNoiDung').prop('hidden', false);
        $('.btnLuuEdit').prop('hidden', false);
        $('.btnHuyEdit').prop('hidden', false);
        $('.DeleteImg').prop('hidden', false);
        $('.addImg').prop('hidden', false);

        $('#inputTieuDe').val(txtTieuDe.text());
        $('#inputNoiDung').val(txtNoiDung.text());
        $('#inputNoiDung').focus();

    });

    function Edit(data, id) {
        $.ajax({
            type: 'POST',
            url: '/SinhVien/ThaoLuan/Edit',
            processData: false,
            contentType: false,
            data: data,
            //async: false,
            success: function (response) {
                if (response.status == true) {
                    toastr.success(response.mess);
                    $("#tblSinhVien").load(window.location.href + " #tblSinhVien");
                    //LoadNoiDung(id);
                    arrayImgSV.array = [];
                    currentImgSV = [];
                    ReloadNoiDung(id);
                    //LoadComments(id);
                }
                else {
                    toastr.error(response.mess);
                }
            },
        });
    };

    //Delete BaiPost
    $(document).delegate('.btnDelete', 'click', function () {
        var id = $("#valueIdBaiPost").val();
        if (confirm('Xác nhận xóa bài đăng?')) {
            $.ajax({
                url: '/SinhVien/ThaoLuan/XoaBaiPost',
                type: 'POST',
                data: { id: id },
                success: function (response) {
                    if (response.status) {
                        toastr.success(response.mess);
                        $("#mydiv").load(window.location.href + " #mydiv");
                    }
                    else
                        toastr.error(response.mess);
                }
            });
        }
    })

    //Search BaiPost
    $("#btnSearch").click(function () {
        var SearchString = $("#txtSearch").val();
        var table = $("#tblSinhVien");
        var IdDeTaiNghienCuu = $('.select2 option:selected').val();
        $.get('/SinhVien/ThaoLuan/SearchBaiPost/', { searchString: SearchString, id: IdDeTaiNghienCuu }, function (data) {
            table.html(data);
        });
    });

    //Refresh List BaiPost
    $("#LkRefreshList").click(function () {
        var table = $("#tblSinhVien");
        var IdDeTaiNghienCuu = $('.select2 option:selected').val();
        table.html('');
        $.get('/SinhVien/ThaoLuan/RefreshList', { id: IdDeTaiNghienCuu }, function (data) {
            table.html(data);
        });
    });

    function ReloadNoiDung(idbaipost) {
        $(".card-widget").html("");
        currentImgSV = [];
        arrayImgSV = [];
        $.get('/SinhVien/ThaoLuan/NoiDungBaiPost/' + idbaipost, function (content) {
            $(".card-widget").html(content);
        });
    }
    //Click Row BaiPost hien thi noi dung
    $(document).on("click", "#tblSinhVien tr", function () {
        if ($(this).hasClass('table-info'))
            return;
        else {
            $('#tblSinhVien tr').removeClass("table-info");
            $(this).toggleClass("table-info");
            var idbaipost = $(this).attr("id");
            $('#valueIdBaiPost').val(idbaipost);
            ReloadNoiDung(idbaipost);
            //LoadNoiDung(idbaipost);
        }
    });

    // Add the following code if you want the name of the file appear on select
    $(document).delegate('.custom-file-input', 'change', function () {
        var files = $(this)[0].files;
        if (files.length > 1) {
            var fileName = files[0].name;
            for (i = 1; i < files.length; i++) {
                fileName = fileName + ', ' + files[i].name;
            }
        }
        else {
            var fileName = $(this).val().split("\\").pop();
        }
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    function Refresh() {
        $('#InputTieuDe').val('');
        $('#InputNoiDung').val('');
        $('#SelectDeTai option:selected').val('');
        $('#divChonDeTai').prop('hidden', true);
        $('.publicCheck').prop('checked', false);
        $('.privateCheck').prop('checked', false);
        $("#Files").val("");
        $(".custom-file-label").text("Chọn tệp");
    }

    //Create BaiPost
    function Create(data) {
        var table = $("#tblSinhVien");
        $.ajax({
            type: 'POST',
            url: '/SinhVien/ThaoLuan/Create',
            processData: false,
            contentType: false,
            data: data,
            //async: false,
            success: function (response) {
                if (response.status == true) {
                    //table.ajax.reload();
                    toastr.success(response.mess);
                    $("#modal-lg").modal('hide');
                    Refresh();
                    var Html = '<tr id="' + response.data.id + '">' +
                        '<td><label class="text-truncate" style="max-width: 190px">' + response.data.tieuDe + '</label ></td >' +
                        '<td>' + response.temp + '</td>' +
                        '</tr>';
                    table.append(Html);
                }
                else {
                    $('.lblFiles').text(response.mess);
                }
            }
        });
    };

    //Change DeTai
    $('.select2').change(function () {
        var id = $(this).val();
        $("#listBaiPost").html('');
        $.get('/SinhVien/ThaoLuan/ChangeDeTai', { id: id }, function (data) {
            $("#listBaiPost").html(data);
        })
    });

    //Button Luu Popup
    $('#btnLuu').on('click', function () {
        var TieuDe = $('#InputTieuDe').val();
        if (TieuDe == null || TieuDe == "") {
            $('.lblTieuDe').text("Tiêu đề không được để trống!");
            return;
        }
        else
            $('.lblTieuDe').text("");
        var NoiDung = $('#InputNoiDung').val();
        var IdDeTaiNghienCuu = $('.select2 option:selected').val();
        var files = $("#Files").get(0).files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append('Files', files[i]);
        }
        data.append('TieuDe', TieuDe);
        data.append('NoiDung', NoiDung);
        data.append('IdDeTaiNghienCuu', IdDeTaiNghienCuu);
        Create(data);
    })

    $(document).delegate('img', 'click', function () {
        src = $(this).data('src');
        $("#LargeImg").attr('src', src);
    });

});