$(document).ready(function () {

    var SetBaiPostCongKhai = $("#tblCongKhai");
    var SetBaiPostRiengTu = $("#tblRiengTu");

    function CheckTab(CongKhaiTab) {
        if (CongKhaiTab) {
            SetBaiPostCongKhai.html("");
        }
        else {
            SetBaiPostRiengTu.html("");
        }
    };

    function ShowBaiPost(CongKhaiTab, response) {
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

    function LoadNoiDung(idbaipost, SetTieuDe, SetNoiDung) {
        $.ajax({
            url: "/GVHD/ThaoLuan/NoiDungBaiPost",
            type: "post",
            data: {
                idbaipost: idbaipost
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    var datatieude = '<h3 class="col text-center">' + response.data.tieuDe + '</h3>';
                    SetTieuDe.append(datatieude);
                    SetNoiDung.append(response.data.noiDung);
                }
            }
        })
    }

    //Search BaiPost
    $("#btnSearch").click(function () {
        var SearchString = $("#txtSearch").val();

        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");
        CheckTab(CongKhaiTab);

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
                    ShowBaiPost(CongKhaiTab, response);
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

        CheckTab(CongKhaiTab);

        $.ajax({
            url: "/GVHD/ThaoLuan/RefreshList",
            type: "post",
            data: {
                CongKhaiTab: CongKhaiTab
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowBaiPost(CongKhaiTab, response);
                }
            }
        })
    });

    //Click Row BaiPost hien thi noi dung
    $(document).on("click", "#tblCongKhai tr", function () {
        $('#tblCongKhai tr').removeClass("table-info");
        $(this).toggleClass("table-info");

        var idbaipost = $(this).attr("id");
        var SetNoiDung = $("#txtNoiDungBaiPost");
        var SetTieuDe = $("#txtTieuDeBaiPost");
        SetTieuDe.html("");
        SetNoiDung.html("");
        LoadNoiDung(idbaipost, SetTieuDe, SetNoiDung);
    });

    $(document).on("click", "#tblRiengTu tr", function () {
        $('#tblRiengTu tr').removeClass("table-info");
        $(this).toggleClass("table-info");

        var idbaipost = $(this).attr("id");
        var SetNoiDung = $("#txtNoiDungBaiPost");
        var SetTieuDe = $("#txtTieuDeBaiPost");
        SetTieuDe.html("");
        SetNoiDung.html("");
        LoadNoiDung(idbaipost, SetTieuDe, SetNoiDung);
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
        $('.publicCheck').prop('checked', false);
        $('.privateCheck').prop('checked', false);
        $("#Files").val("");
        $(".custom-file-label").text("Chọn tệp");
    }

    //Create BaiPost
    function Create(data) {
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
                    if (response.toastr != "")
                        toastr.error(response.toastr);
                }
            }
        });
    };

    //Button Luu Popup
    $('#btnLuu').on('click', function () {
        var TieuDe = $('#InputTieuDe').val();
        var NoiDung = $('#InputNoiDung').val();
        var IdDeTaiNghienCuu = $('#SelectDeTai option:selected').val();
        var PublicCheck = $('.publicCheck').is(':checked');
        var PrivateCheck = $('.privateCheck').is(':checked');
        //if (PublicCheck == false && PrivateCheck == false)
        //    toastr.error("")
        var Type = '';
        if (PublicCheck)
            Type = 0;
        else
            Type = 1;
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
});