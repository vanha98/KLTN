$(document).ready(function () {

    if (localStorage.getItem("Message")) {
        toastr.success(localStorage.getItem("Message"));
        localStorage.clear();
    }

    function ShowHoiDong(bodyTable,response) {
        $.each(response.data, function (i, item) {
            var phanCong = "Chưa phân công";
            var css = "text-danger";
            if (item.statusPhanCong == 1) {
                phanCong = "Đã phân công";
                css = "text-success";
            }
            var Data = '<tr id="' + item.id + '">' +
                '<td><label class="text-truncate" style="max-width: 190px">' + item.tenHoiDong + '</label ></td >' +
                '<td class="'+css+'">' + phanCong + '</td>' +
                '</tr>';
            bodyTable.append(Data);
        });
    };

    //Search BaiPost
    $("#btnSearch").click(function () {
        var SearchString = $("#txtSearch").val();
        var bodyTable = $("#tblHoiDong");
        bodyTable.html("");
        $.ajax({
            url: "/Admin/PhanCongHoiDong/SearchHoiDong",
            type: "post",
            data: {
                SearchString: SearchString,
            },
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowHoiDong(bodyTable, response);
                }
                else {
                   bodyTable.append('<tr><td colspan="2">Không tìm thấy hội đồng</td></tr>');
                }
            }
        })
    });

    //Refresh List BaiPost
    $(document).on('click',"#LkRefreshList",function () {
        var bodyTable = $("#tblHoiDong");
        bodyTable.html("");
        $.ajax({
            url: "/Admin/PhanCongHoiDong/RefreshList",
            type: "post",
            dataType: "json",
            success: function (response) {
                if (response.status) {
                    ShowHoiDong(bodyTable, response);
                }
            }
        })
    });

    //reLoad
    function ReloadThanhVien(idHoiDong) {
        $(".card-primary .card-body").html("");
        $.get('/Admin/PhanCongHoiDong/LoadThanhVien/' + idHoiDong, function (content) {
            $(".card-primary .card-body").html(content);
        });
    }

    function DeTaiDaPhanCong(idHoiDong) {
        $.ajax({
            url: "/Admin/PhanCongHoiDong/DeTaiDaPhanCong",
            type: "POST",
            data: { idHoiDong: idHoiDong },
            success: function (data) {
                $(".card-warning .card-body").html("");
                $(".card-warning .card-body").html(data);
                $('.duallistbox').bootstrapDualListbox({
                    infoText: '',
                    filterPlaceHolder: 'Tìm kiếm',
                    infoTextEmpty: 'Trống',
                    selectedListLabel: "Đề tài đã phân công",
                    nonSelectedListLabel: "Đề tài chưa phân công"
                })
                    //var option = '';
                    //$.each(res.data, function (i, item) {
                    //    option = '<option value=' + item.id + '>' + item.id + ' - '+ item.tenDeTai + item.idgiangVienNavigation.ho+ '</option>';
                    //    list.append(option);
                    //})
            }
        });
    }

    $(document).delegate('.duallistbox', 'change', function () {
        $(".btnPhanCong").prop("disabled", false);
    })

    //Click Row BaiPost hien thi noi dung
    $(document).on("click", "#tblHoiDong tr", function (e) {
        e.preventDefault();
        $('#tblHoiDong tr').removeClass("table-info");
        $(this).toggleClass("table-info");
        var idHoiDong = $(this).attr("id");
        $('#valueIdHoiDong').val(idHoiDong);
        ReloadThanhVien(idHoiDong);
        DeTaiDaPhanCong(idHoiDong);
        //LoadNoiDung(idbaipost);
    });

    function PhanCong(idHoiDong, idMoDot,idsDeTai) {
        $.ajax({
            url: "PhanCongHoiDong/PhanCong",
            type: "POST",
            data: { idHoiDong: idHoiDong, idMoDot: idMoDot,idsDeTai: idsDeTai },
            success: function (res) {
                if (res.status == true) {
                    localStorage.setItem('Message', res.mess);
                    location.reload();
                }
                else
                    toastr.error(res.mess);
            }
        })
    }

    $(document).delegate(".btnPhanCong",'click',function () {
        var list = $("#bootstrap-duallistbox-selected-list_ option");
        var idsDeTai = [];
        var idHoiDong = $('#valueIdHoiDong').val();
        var idMoDot = $('#valueIdMoDot').val();
        if (idMoDot == null || idMoDot == "") {
            toastr.error("Vui lòng mở đợt để thực hiện phân công");
            return;
        }
        if (idHoiDong == null || idHoiDong == "") {
            toastr.error("Bạn chưa chọn hội đồng!!");
            return;
        }
        $.each(list, function (index, elem) {
            //this will log the value for each item inside the boxview2 list
            //if (elem.hasAttribute("data-sortindex"))
                idsDeTai.push(elem.getAttribute("value"));
        });
        if (idsDeTai.length > 0) {
            PhanCong(idHoiDong, idMoDot, idsDeTai);
        }
        else
            toastr.error("Chưa chọn đề tài cần phân công!!");
    });
});