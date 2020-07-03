$(document).ready(function () {

    if (localStorage.getItem("Message")) {
        toastr.success(localStorage.getItem("Message"));
        localStorage.clear();
    }
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

    // Add the following code if you want the name of the file appear on select
    $(document).delegate('.custom-file-input', 'change', function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    ////reLoad
    function ReloadCauHoi(id) {
        $("#modalTraLoiCauHoi .modal-body").html("");
        $.get('/SinhVien/XetDuyetDeTai/LoadCauHoi/' + id, function (content) {
            $("#modalTraLoiCauHoi .modal-body").html(content);
            $("#modalTraLoiCauHoi").modal();
        });
    }

    function ReloadBaoCao(id) {
        $("#modalNopBaoCao .modal-body").html("");
        $.get('/SinhVien/XetDuyetDeTai/LoadBaoCao/' + id, function (content) {
            $("#modalNopBaoCao .modal-body").html(content);
            $("#modalNopBaoCao").modal();
        });
    }

    $(document).on("click", ".btnCapNhatND", function () {
        var id = $(this).data('id');
        $('#valueIdXDDG').val(id);
        ReloadBaoCao(id);
        //LoadNoiDung(idbaipost);
    });

    $(document).on("click", "#btnLuuBaoCao", function () {
        var NoiDung = $("#InputMoTa").val();
        var files = $("#FilesBaoCao").get(0).files;
        if (NoiDung == "" && files.length == 0) {
            toastr.error("Chưa nhập mô tả báo cáo hoặc chưa đính kèm tệp");
            return;
        }
        var id = $('#valueIdXDDG').val();
        var data = new FormData();
        data.append('File', files[0]);
        data.append('NoiDung', NoiDung);
        data.append('IdXDDG', id);
        $.ajax({
            url: "XetDuyetDeTai/BaoCao",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                if (response.status == true) {
                    localStorage.setItem("Message", response.mess)
                    location.reload();
                }
                else {
                    toastr.error(response.mess);
                }
            }
        })
        //LoadNoiDung(idbaipost);
    });

    $(document).on("click", ".btnTraLoi", function () {
        var id = $(this).data('id');
        $('#valueIdCT').val(id);
        ReloadCauHoi(id);
        //LoadNoiDung(idbaipost);
    });

    $(document).on("click", "#btnLuu", function () {
        var traloi = $("#InputNoiDung").val();
        var files = $("#Files").get(0).files;
        if (traloi == "" && files.length == 0) {
            toastr.error("Chưa nhập câu trả lời hoặc chưa đính kèm tệp");
            return;
        }
        var id = $('#valueIdCT').val();
        var data = new FormData();
        data.append('File', files[0]);
        data.append('TraLoi', traloi);
        data.append('IdCT', id);
        $.ajax({
            url: "XetDuyetDeTai/TraLoi",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                if (response.status == true) {
                    localStorage.setItem("Message", response.mess)
                    location.reload();
                }
                else {
                    toastr.error(response.mess);
                }
            }
        })
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
});