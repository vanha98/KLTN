(function () {
    $(function () {

        //init datatable
        var btnCreate = "<button class='btn btn-success btn-sm btnCreate' onclick='CreateAction()'>Thành lập hội đồng</button>";
        //var btnSave = "<button class='btn btn-primary btn-sm' id='btnGui'>Gửi đề tài</button>" + "<button class='btn btn-sm btn-danger ml-1' id='btnHuyGui'>Hủy gửi đề tài</button>";
        var url = "/Admin/QLHoiDong/LoadData"
        var table = $("#example1").DataTable({
            //"responsive": true,
            "autoWidth": false,
            "scrollX": true,
            //"fixedColumns": {
            //    leftColumns: 3,
            //},
            //"fixedHeader": true,
            "pageLength": 10,
            "language": {
                "lengthMenu": btnCreate,
                "zeroRecords": "Không có dữ liệu",
                "info": "",
                "infoEmpty": "",
                "infoFiltered": "",
                "search": "",
                "searchPlaceholder": "Tìm kiếm",
                "paginate": {
                    "first": "<<",
                    "last": ">>",
                    "next": ">",
                    "previous": "<"
                },
            },
            "processing": true, // for show progress bar    
            "serverSide": true, // for process server side    
            "filter": true, // this is for disable filter (search box)    
            "orderMulti": false, // for disable multiple column at once    
            "ajax": {
                "url": url,
                "type": "POST",
                "datatype": "json"
            },
            "columns": [
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "tenHoiDong", "name": "TenHoiDong", "autoWidth": true },
                { "data": "ngayLap", "name": "NgayLap", "autoWidth": true },
                { "data": "idNguoiTao", "name": "IdNguoiTao", "autoWidth": true },
                {
                    "data": "loaiYeuCau", "name": "LoaiYeuCau", "autoWidth": true,
                    render: function (data, type, row) {
                        if (data == 0 && row.status == 0)
                            return '<a href="#" class="text-primary btnInfo" data-id="' + row.id + '" title="Xem thông tin chỉnh sửa">Chỉnh sửa <i class="far fa-eye" ></i></a>';
                        else if (data == 0)
                            return '<lable class="text-dark">Chỉnh sửa</label>';
                        else if (data == 2)
                            return '<lable class="text-dark">Duyệt đăng ký</label>';
                    }
                },
                { "data": "ngayTao", "name": "NgayTao", "autoWidth": true },
                { "data": "idNguoiDuyet", "name": "IdNguoiDuyet", "autoWidth": true },
                {
                    "data": "status", "name": "Status", "autoWidth": true,
                    render: function (data, type, row) {
                        if (data == 0)
                            return '<button class="btn btn-sm btn-success btnApprove " data-id="' + row.iddeTai + '"><i class="fas fa-check"> Đồng ý</i></button>' +
                                '<button class="btn btn-sm btn-danger ml-1 btnReject" data-id="' + row.iddeTai + '" data-toggle="modal" data-target="#ConfirmDelete"><i class="far fa-times-circle"> Từ chối</i></button>';
                        else if (data == 1)
                            return '<lable class="text-success">Đã duyệt</label>';
                        else
                            return '<lable class="text-danger">Từ chối</label>';
                    }
                },
            ],

        });

        //function ChangeStatus(id, type) {
        //    $.ajax({
        //        url: 'PheDuyetYeuCau/ChangeStatus',
        //        type: 'POST',
        //        data: { idDeTai: id, type: type },
        //        success: function (res) {
        //            if (res.status == true) {
        //                toastr.success(res.mess);
        //                table.ajax.reload();
        //                table2.ajax.reload();
        //            }
        //            else
        //                toastr.error(res.mess);
        //        }
        //    });
        //}

        //$(document).delegate('.btnApprove', 'click', function () {
        //    var idDeTai = $(this).data('id');
        //    var type = 1; // approve
        //    ChangeStatus(idDeTai, type);
        //})
        var ThanhViens = new Array();

        $(document).delegate('.deleteTemp', 'click', function () {
            var rowindex = $(this).parent().parent().index();
            $("#bodyTable tr").eq(rowindex).remove();
            ThanhViens.splice(rowindex, 1);
        })
        
        $(document).delegate('.btnAddTV', 'click', function (e) {
            e.preventDefault();
            var flag = true;
            var demPhanBien = 0;
            var IdTV = $("#SelectGVHD option:selected").val();
            var textTen = $("#SelectGVHD option:selected").text();
            var VaiTro = $('.selectVaiTro option:selected').val();
            var textVaiTro = $(".selectVaiTro option:selected").text();
            if (IdTV == 0) {
                $(".lblGVHD").prop("hidden", false);
                $(".lblGVHD").text("Chưa chọn GVHD");
                return;
            }
            else
                $(".lblGVHD").prop("hidden",true);

            if (VaiTro == 0) {
                $(".lblVaiTro").prop("hidden", false);
                $(".lblVaiTro").text("Chưa chọn vai trò");
                return;
            }
            else
                $(".lblVaiTro").prop("hidden",true);
            var tv = {
                IdThanhVien: IdTV,
                VaiTro: VaiTro
            }
            $.each(ThanhViens, function (i, item) {
                if (item.IdThanhVien == tv.IdThanhVien) {
                    toastr.error(textTen + " đã là thành viên hội đồng");
                    flag=false;
                }
            })
            if (!flag)
                return;
            ThanhViens.push(tv);
            console.log(ThanhViens);
            var html = '<tr>' +
                '<td>' + textTen + '</td>' +
                '<td>' + textVaiTro + '</td>' +
                '<td><i class="fas fa-times deleteTemp"></i></td>' +
                '</tr>';
            $("#bodyTable").append(html);
        })

        function LapHD(id, tenHD, listTV) {
            var obj = {
                Id: id,
                TenHoiDong: tenHD,
                ThanhViens: listTV
            }
            $.ajax({
                url: 'QLHoiDong/LapHD',
                type: 'POST',
                data: obj,
                success: function (res) {
                    if (res.status == true) {
                        alert(1);
                    }
                }
            });
        }

        $(document).delegate(".btnLapHD", 'click', function () {
            var idHD = $("#valueIdHD").val();
            var TenHD = $("#valueTenHD").val();
            if (TenHD == "") {
                $(".lblTenHD").prop("hidden", false);
                $(".lblTenHD").text("Chưa nhập tên hội đồng");
                return;
            }
            else {
                $(".lblTenHD").prop("hidden",true);
            }
            if (ThanhViens.length < 2) {
                $(".lblTable").text("Thành viên hội đồng chưa đạt đủ tối thiểu (3 thành viên)");
                $(".lblTable").prop("hidden", false);
            }
            else if (ThanhViens.length > 4) {
                $(".lblTable").text("Vượt quá số lượng thành viên (5 thành viên)");
                $(".lblTable").prop("hidden", false);
            }
            else {
                $(".lblTable").prop("hidden",true);
            }
            LapHD(idHD,TenHD, ThanhViens);
        })
    });
})();