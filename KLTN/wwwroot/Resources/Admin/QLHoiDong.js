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
            rowId:"id",
            "columns": [
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "hoiDong", "name": "HoiDong", "autoWidth": true },
                {
                    orderable: false,
                    "data": "hoTenGV", "name": "HoTenGV", "autoWidth": true,
                    //render: function (data, type, row) {
                    //    $.each(data, function (i, item) {
                    //        return item.idGiangVien
                    //    })
                    //}
                },
                {
                    orderable: false,
                    "data": "vaiTro", "name": "VaiTro", "autoWidth": true,
                    //render: function (data, type, row) {
                    //    $.each(data, function (i, item) {
                    //        return item.idGiangVien
                    //    })
                    //}
                },
                
            ],
            rowGroup: {
                startRender: function (rows, group) {
                    var id = '';
                    rows.each(function () {
                        id = this.nodes().to$().attr('id');
                        return;
                    })
                    return group + '<a href="#" style="float:right" class="btnDelete" data-toggle="modal" data-target="#ConfirmDelete" data-id="' + id + '"><i class="fas fa-times"></i></a>' +
                        '<a href="#" class="mr-2" style="float:right" onclick = "EditAction(' + id + ')"><i class="fas fa-pencil-alt"></i></a>';
                },
                endRender: null,
                dataSrc: "hoiDong"
            },
            columnDefs: [{
                targets: [0,1],
                visible: false
            }]
        });

        $('#modal-lg').on('hidden.bs.modal', function () {
            ThanhViens = [];
            DelThanhViens = [];
        })
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
        var DelThanhViens = [];
        $(document).delegate('.deleteTV', 'click', function () {
            var idTV = $(this).data('id');
            var rowindex = $(this).parent().parent().index();
            $("#bodyTable tr").eq(rowindex).remove();
            DelThanhViens.push(idTV);
            console.log(DelThanhViens);
        })

        var ThanhViens = new Array();

        $(document).delegate('.deleteTemp', 'click', function () {
            var rowindex = $(this).parent().parent().index();
            var id = $(this).data('id');
            $("#bodyTable tr").eq(rowindex).remove();
            $.each(ThanhViens, function (i, item) {
                if (item.IdThanhVien == id) {
                    ThanhViens.splice(i, 1);
                    return;
                }
            })
            console.log(ThanhViens);
        })
        
        $(document).delegate('.btnAddTV', 'click', function (e) {
            e.preventDefault();
            var flag = true;
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

            var exist = $('#bodyTable tr > td:contains(' + textTen + ')').length;
            if (exist == 1) {
                $(".lblTable").html(textTen + " đã là thành viên hội đồng");
                $(".lblTable").prop("hidden", false);
                flag = false;
            }
            else
                $(".lblTable").prop("hidden", true);
            //$.each(ThanhViens, function (i, item) {
            //    if (item.IdThanhVien == tv.IdThanhVien) {
            //        $(".lblTable").html(textTen + " đã là thành viên hội đồng");
            //        $(".lblTable").prop("hidden", false);
            //        flag=false;
            //    }
            //    else
            //        $(".lblTable").prop("hidden", true);
            //})
            if (!flag)
                return;
            ThanhViens.push(tv);
            console.log(ThanhViens);
            var html = '<tr>' +
                '<td>' + textTen + '</td>' +
                '<td>' + textVaiTro + '</td>' +
                '<td><i class="fas fa-times deleteTemp" data-id="' + tv.IdThanhVien + '"></i></td>' +
                '</tr>';
            $("#bodyTable").append(html);
        })

        function LapHD(id, tenHD, listTV, listDelTV) {
            var obj = {
                Id: id,
                TenHoiDong: tenHD,
                ThanhViens: listTV,
                DelThanhViens: listDelTV,
            }
            $.ajax({
                url: 'QLHoiDong/LapHD',
                type: 'POST',
                data: obj,
                success: function (res) {
                    if (res.status == true) {
                        table.ajax.reload();
                        toastr.success(res.mess);
                        $('#modal-lg').modal('hide');
                        ThanhViens = [];
                        DelThanhViens = [];
                    }
                    else
                        toastr.error("Error");
                }
            });
        }

        $(document).delegate(".btnLapHD", 'click', function () {
            var idHD = $("#valueIdHD").val();
            var TenHD = $("#valueTenHD").val();
            var count = $("#bodyTable tr").length;
            if (TenHD == "") {
                $(".lblTenHD").prop("hidden", false);
                $(".lblTenHD").text("Chưa nhập tên hội đồng");
                return;
            }
            else {
                $(".lblTenHD").prop("hidden",true);
            }
            if (count < 3 || count > 5 || count == 4) {
                $(".lblTable").text("Hội đồng chưa đủ 3 hoặc 5 thành viên");
                $(".lblTable").prop("hidden", false);
                return;
            }
            else {
                $(".lblTable").prop("hidden",true);
            }
            if (DelThanhViens.length > 0) {
                LapHD(idHD, TenHD, ThanhViens, DelThanhViens);
            }
            else {
                LapHD(idHD, TenHD, ThanhViens);
            }
        })

        $(document).delegate(".btnDelete", 'click', function () {
            var id = $(this).data('id');
            $("#btnXoa").click(function () {
                $.ajax({
                    url: "/Admin/QLHoiDong/Delete/",
                    type: "POST",
                    data: { id: id },
                    success: function (res) {
                        if (res.status == true) {
                            table.ajax.reload();
                            toastr.success(res.mess);
                        }
                    }
                });
            })
        })

        
    });
})();