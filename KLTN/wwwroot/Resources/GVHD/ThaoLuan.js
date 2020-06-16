$(document).ready(function () {
    var currentImg = [];
    function CheckTab(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu) {
        if (CongKhaiTab) {
            SetBaiPostCongKhai.html("");
        }
        else {
            SetBaiPostRiengTu.html("");
        }
    };

    function ShowBaiPost(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu, response) {
        if (CongKhaiTab) {
            $.each(response.data, function (i, item) {
                var Data = '<tr id="' + item.id + '">' +
                    '<td><label class="text-truncate" style="max-width: 190px">' + item.tieuDe + '</label ></td >' +
                    '<td>' + item.ngayPost + '</td>' +
                    '</tr>';
                SetBaiPostCongKhai.append(Data);
            });
        }
        else {
            $.each(response.data, function (i, item) {
                var Data = '<tr id="' + item.id + '">' +
                    '<td><label class="text-truncate" style="max-width: 190px">' + item.tieuDe + '</label ></td >' +
                    '<td>' + item.ngayPost + '</td>' +
                    '<td>' + item.idDeTai + '</td>' +
                    '</tr>';
                SetBaiPostRiengTu.append(Data);
            });
        }
    };

    var arrayImg = new CustomArrayImg("imgFile", "imgAttach");
    //Delete imgTemp
    $(document).delegate('.DeleteImgTemp', 'click', function (e) {
        e.preventDefault();
        var file = $(this).data('id');
        var li = $(this).closest("li");
        li.remove();
        arrayImg.DeleteImgTemp(file);
    });

    //Save data Edit
    //add Img when Edit
    $(document).delegate('.btnaddImg', 'click', function () {
        $('#imgFile').trigger('click');

    });
    $(document).delegate('#imgFile','change',function () {
        arrayImg.pushImgToArray();
    });

    //Delete Img
    $(document).delegate('.DeleteImg','click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        currentImg.push(id);
        console.log(currentImg);
        var li = $(this).closest("li");
        li.remove();
    });

    //Huy Edit
    $(document).delegate('.btnHuyEdit','click', function () {
        var id = $('#valueIdBaiPost').val();
        arrayImg.array = [];
        currentImg = [];
        ReloadNoiDung(id);
        //LoadNoiDung(id);
        //LoadComments(id);
    });

    //Luu chinh sửa
    $(document).delegate('.btnLuuEdit','click', function () {
        var id = $('#valueIdBaiPost').val();
        var NoiDung = $('#inputNoiDung').val();
        var TieuDe = $('#inputTieuDe').val();
        var data = new FormData();
        for (var i = 0; i < arrayImg.array.length; i++) {
            data.append('Files', arrayImg.array[i]);
        }
        data.append('NoiDung', NoiDung);
        data.append('Id', id);
        data.append('TieuDe', TieuDe);
        data.append('currentImg', currentImg);
        Edit(data,id);
    });

    //Edit BaiPost
    $(document).delegate('.btnEdit','click', function () {
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
        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");
        $.ajax({
            type: 'POST',
            url: '/GVHD/ThaoLuan/Edit',
            processData: false,
            contentType: false,
            data: data,
            //async: false,
            success: function (response) {
                if (response.status == true) {
                    //table.ajax.reload();
                    toastr.success(response.mess);
                    if (!CongKhaiTab)
                        $("#tblRiengTu").load(window.location.href + " #tblRiengTu");
                    else
                        $("#tblCongKhai").load(window.location.href + " #tblCongKhai");
                    //LoadNoiDung(id);
                    arrayImg.array = [];
                    currentImg = [];
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
                url: '/GVHD/ThaoLuan/XoaBaiPost',
                type: 'POST',
                data: { id: id },
                success: function (response) {
                    if (response.status) {
                        toastr.success(response.mess);
                        $("#mydiv").load(window.location.href + " #mydiv");
                    }
                }
            });
        }
    })

    //Search BaiPost
    $("#btnSearch").click(function () {
        var SearchString = $("#txtSearch").val();
        var SetBaiPostCongKhai = $("#tblCongKhai");
        var SetBaiPostRiengTu = $("#tblRiengTu");
        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");
        
        CheckTab(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu);

        $.ajax({
            url: "/GVHD/ThaoLuan/SearchBaiPost",
            type: "post",
            data: {
                SearchString: SearchString,
                CongKhaiTab: CongKhaiTab
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowBaiPost(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu, response);
                }
                else {
                    if (CongKhaiTab) {
                        SetBaiPostCongKhai.append('<tr><td colspan="2">Không tìm thấy tiêu đề</td></tr>');
                    }
                    else {
                        SetBaiPostRiengTu.append('<tr><td colspan="2">Không tìm thấy tiêu đề</td></tr>');
                    }

                }
            }
        })
    });

    //Refresh List BaiPost
    $("#LkRefreshList").click(function () {
        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");
        var SetBaiPostCongKhai = $("#tblCongKhai");
        var SetBaiPostRiengTu = $("#tblRiengTu");
        CheckTab(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu);

        $.ajax({
            url: "/GVHD/ThaoLuan/RefreshList",
            type: "post",
            data: {
                CongKhaiTab: CongKhaiTab
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowBaiPost(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu, response);
                }
            }
        })
    });

    //reLoad
    function ReloadNoiDung(idbaipost) {
        $(".card-widget").html("");
        $.get('/GVHD/ThaoLuan/NoiDungBaiPost/' + idbaipost, function (content) {
            $(".card-widget").html(content);
        });
    }

    //Click Row BaiPost hien thi noi dung
    $(document).on("click", "#tblCongKhai tr", function () {
        $('#tblCongKhai tr').removeClass("table-info");
        $(this).toggleClass("table-info");
        var idbaipost = $(this).attr("id");
        $('#valueIdBaiPost').val(idbaipost);
        ReloadNoiDung(idbaipost);
        //LoadNoiDung(idbaipost);
    });

    $(document).on("click", "#tblRiengTu tr", function () {
        $('#tblRiengTu tr').removeClass("table-info");
        $(this).toggleClass("table-info");
        var idbaipost = $(this).attr("id");
        $('#valueIdBaiPost').val(idbaipost);
        ReloadNoiDung(idbaipost);
        //LoadNoiDung(idbaipost);
    });

    //Click checkbox Public or Private
    $('input.form-check-input').on('change', function () {
        $('input.form-check-input').not(this).prop('checked', false);
        if ($(this).hasClass('privateCheck') && $(this).is(':checked'))
            $('#divChonDeTai').prop('hidden', false);
        else
            $('#divChonDeTai').prop('hidden', true);
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
        $('#divChonDeTai').prop('hidden',true);
        $('.publicCheck').prop('checked', false);
        $('.privateCheck').prop('checked', false);
        $("#Files").val("");
        $(".custom-file-label").text("Chọn tệp");
    }

    //Create BaiPost
    function Create(data) {
        var SetBaiPostCongKhai = $("#tblCongKhai");
        var SetBaiPostRiengTu = $("#tblRiengTu");
        $.ajax({
            type: 'POST',
            url: '/GVHD/ThaoLuan/Create',
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
                    if (response.data.loai == 0) {
                        var Html = '<tr id="' + response.data.id + '">' +
                            '<td><label class="text-truncate" style="max-width: 190px">' + response.data.tieuDe + '</label ></td >' +
                            '<td>' + response.temp + '</td>' +
                            '</tr>';
                        SetBaiPostCongKhai.append(Html);
                    }
                    else {
                        var Html = '<tr id="' + response.data.id + '">' +
                            '<td><label class="text-truncate" style="max-width: 190px">' + response.data.tieuDe + '</label ></td >' +
                            '<td>' + response.temp + '</td>' +
                            '<td>' + response.data.idkenhThaoLuanNavigation.iddeTai + '</td>' +
                            '</tr>';
                        SetBaiPostRiengTu.append(Html);
                    }
                }
                else {
                    $('.lblFiles').text(response.mess);
                }
            }
        });
    };

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
        var IdDeTaiNghienCuu = $('#SelectDeTai option:selected').val();
        var PublicCheck = $('.publicCheck').is(':checked');
        var PrivateCheck = $('.privateCheck').is(':checked');
        var Type = '';
        if (PublicCheck)
            Type = 0;
        else if (PrivateCheck) {
            Type = 1;
            if (IdDeTaiNghienCuu == null || IdDeTaiNghienCuu == "") {
                $('.lblChonDeTai').text("Vui lòng chọn đề tài");
                return;
            }
            else {
                $('.lblChonDeTai').text("");
            }
        }
        else {
            $('.textCheckbox').text("Vui lòng chọn chế độ của bài đăng");
            return;
        }
        var files = $("#Files").get(0).files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append('Files', files[i]);
        }
        data.append('TieuDe', TieuDe);
        data.append('NoiDung', NoiDung);
        data.append('IdDeTaiNghienCuu', IdDeTaiNghienCuu);
        data.append('Loai', Type);
        Create(data);
    })

    $(document).delegate('img','click', function () {
        src = $(this).data('src');
        $("#LargeImg").attr('src', src);
    });

});