$(document).ready(function () {

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
                var Data = '<tr id="' + item.idDeTai + '">' +
                    '<td><label class="text-truncate" style="max-width: 190px">' + item.tenDeTai + '</label ></td >' +
                    '<td>' + item.tinhTrang + '</td>' +
                    '</tr>';
                SetBaiPostCongKhai.append(Data);
            });
        }
        else {
            $.each(response.data, function (i, item) {
                var Data = '<tr id="' + item.idDeTai + '">' +
                    '<td><label class="text-truncate" style="max-width: 190px">' + item.tenDeTai + '</label ></td >' +
                    '<td>' + item.ngayPost + '</td>' +
                    '<td>' + item.tinhTrang + '</td>' +
                    '</tr>';
                SetBaiPostRiengTu.append(Data);
            });
        }
    };

    //Search BaiPost
    $("#btnSearch").click(function () {
        var SearchString = $("#txtSearch").val();
        var ActiveTabDot1 = $("#custom-tabs-one-home-tab").hasClass("active");
        var SetDeTaiDot1 = $("#tblDot1");
        var SetDeTaiDot2 = $("#tblDot2");
        CheckTab(ActiveTabDot1, SetDeTaiDot1, SetDeTaiDot2);

        $.ajax({
            url: "/GVHD/XetDuyetDeTai/SearchBaiPost",
            type: "post",
            data: {
                SearchString: SearchString,
                ActiveTabDot1: ActiveTabDot1
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowBaiPost(ActiveTabDot1, SetDeTaiDot1, SetDeTaiDot2, response);
                }
                else {
                    if (ActiveTabDot1) {
                        SetDeTaiDot1.append('<tr><td colspan="2">Không tìm thấy đề tài</td></tr>');
                    }
                    else {
                        SetDeTaiDot2.append('<tr><td colspan="2">Không tìm thấy đề tài</td></tr>');
                    }

                }
            }
        })
    });

    //Refresh List BaiPost
    $("#LkRefreshList").click(function () {
        var ActiveTabDot1 = $("#custom-tabs-one-home-tab").hasClass("active");
        var SetDeTaiDot1 = $("#tblDot1");
        var SetDeTaiDot2 = $("#tblDot2");
        CheckTab(ActiveTabDot1, SetDeTaiDot1, SetDeTaiDot2);

        $.ajax({
            url: "/GVHD/XetDuyetDeTai/RefreshList",
            type: "post",
            data: {
                ActiveTabDot1: ActiveTabDot1
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowBaiPost(ActiveTabDot1, SetDeTaiDot1, SetDeTaiDot2, response);
                }
                else {
                    if (ActiveTabDot1) {
                        SetDeTaiDot1.append('<tr><td>Không có dữ liệu</td></tr>');
                    }
                    else {
                        SetDeTaiDot2.append('<tr><td>Không có dữ liệu</td></tr>');
                    }
                }
            }
        })
    });

    ////reLoad
    function ReloadNoiDung(idDeTai) {
        $(".card-primary").html("");
        $.get('/GVHD/XetDuyetDeTai/LoadNoiDung/' + idDeTai, function (content) {
            $(".card-primary").html(content);
        });
    }

    //Click Row BaiPost hien thi noi dung
    $(document).on("click", "#tblDot1 tr", function () {
        $('#tblDot1 tr').removeClass("table-info");
        $(this).toggleClass("table-info");
        var iddeTai = $(this).attr("id");
        $('#valueIdDeTai').val(iddeTai);
        ReloadNoiDung(iddeTai);
        //LoadNoiDung(idbaipost);
    });

    $(document).on("click", "#tblDot2 tr", function () {
        $('#tblDot2 tr').removeClass("table-info");
        $(this).toggleClass("table-info");
        var iddeTai = $(this).attr("id");
        $('#valueIdDeTai').val(iddeTai);
        ReloadNoiDung(iddeTai);
        //LoadNoiDung(idbaipost);
    });

    $(document).delegate('.btnXemNhom', 'click', function () {
        $("#modal-NhomSV .modal-body").html("");
        var id = $('#valueIdDeTai').val();
        $.get('/GVHD/XetDuyetDeTai/XemNhom/' + id, function (content) {
            $("#modal-NhomSV .modal-body").html(content);
            $("#modal-NhomSV").modal();
        });
    })
    
    //function Refresh() {
    //    $('#InputTieuDe').val('');
    //    $('#InputNoiDung').val('');
    //    $('#SelectDeTai option:selected').val('');
    //    $('#divChonDeTai').prop('hidden', true);
    //    $('.publicCheck').prop('checked', false);
    //    $('.privateCheck').prop('checked', false);
    //    $("#Files").val("");
    //    $(".custom-file-label").text("Chọn tệp");
    //}

    // Add the following code if you want the name of the file appear on select
    $(document).delegate('.custom-file-input', 'change', function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    $(document).delegate('.btnDatCauHoi', 'click', function () {
        var value = $("#inputCauHoi").val();
        var files = $("#Files").get(0).files;
        if (value == "" && files.length == 0) {
            toastr.error("Chưa nhập câu hỏi hoặc chưa đính kèm tệp");
            return;
        }
        var idDeTai = $('#valueIdDeTai').val();
        var data = new FormData();
        data.append('File', files[0]);
        data.append('CauHoi', value);
        data.append('idDeTai', idDeTai);
        $.ajax({
            url: "XetDuyetDeTai/DatCauHoi",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                if (response.status == true) {
                    ReloadNoiDung(idDeTai);
                    toastr.success(response.mess);
                }
            }
        })
    })

    $(document).delegate('.btnDanhGia', 'click', function () {
        var DanhGia = $("#inputDanhGia").val();
        var Diem = $("#inputDiem").val();
        var idDeTai = $('#valueIdDeTai').val();
        if (DanhGia == "" || Diem == "") {
            toastr.error("Chưa nhập đánh giá hoặc điểm");
            return;
        }
        if (0 <= Diem && Diem <= 10) {
            var data = {
                NhanXet: DanhGia,
                Diem: Diem,
                idDeTai: idDeTai
            };
            $.ajax({
                url: "XetDuyetDeTai/DanhGia",
                type: "POST",
                data: data,
                success: function (response) {
                    if (response.status == true) {
                        ReloadNoiDung(idDeTai);
                        toastr.success(response.mess);
                    }
                    else {
                        toastr.error(response.mess);
                    }
                }
            })
        } else
            toastr.error("Điểm phải lớn hơn bằng 0 và nhỏ hơn bằng 10");
    })

    $(document).delegate('.xemTraLoi', 'click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        $("#modalXemTraLoi .modal-content").html();
        $.get('XetDuyetDeTai/LoadCauTraLoi', { id: id }, function (data) {
            $("#modalXemTraLoi .modal-content").html(data);
            $("#modalXemTraLoi").modal();
        })
    })
});