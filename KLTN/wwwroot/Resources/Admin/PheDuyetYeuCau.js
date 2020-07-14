(function () {
    $(function () {

        //bsCustomFileInput.init();
        //init datatable
        //var btnCreate = "<button class='btn btn-success btn-sm btnCreate' onclick='CreateAction()'>Tạo đề tài</button>";
        //var btnSave = "<button class='btn btn-primary btn-sm' id='btnGui'>Gửi đề tài</button>" + "<button class='btn btn-sm btn-danger ml-1' id='btnHuyGui'>Hủy gửi đề tài</button>";
        var url = "/Admin/PheDuyetYeuCau/LoadData"
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
                "lengthMenu": "",
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
                { "data": "iddeTai", "name": "IddeTai", "autoWidth": true },
                { "data": "tenDeTai", "name": "TenDeTai", "autoWidth": true, "className": "text-truncate" },
                { "data": "moTa", "name": "MoTa", "autoWidth": true },
                {
                    data: "tepDinhKem",
                    render: function (data, type, row) {
                        if (data != null && data != "")
                            return "<a href='/../../FileUpload/DeTaiNghienCuu/" + data + "' download='" + data + "'>" + row.tenTep + "</a>";
                        else
                            return "";
                    }
                },
                { "data": "tenGiangVien", "name": "TenGiangVien", "autoWidth": true },
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
                        else if(data == 1)
                            return '<lable class="text-success">Đã duyệt</label>';
                        else
                            return '<lable class="text-danger">Từ chối</label>';
                    }
                },
            ],
            
        });

        var table2 = $("#example2").DataTable({
            //"responsive": true,
            "autoWidth": false,
            "scrollX": true,
            //"fixedColumns": {
            //    leftColumns: 3,
            //},
            //"fixedHeader": true,
            "pageLength": 10,
            "language": {
                "lengthMenu": "",
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
                "url": "/Admin/PheDuyetYeuCau/LoadData2",
                "type": "POST",
                "datatype": "json"
            },
            "columns": [
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "tenDeTai", "name": "TenDeTai", "autoWidth": true, "className": "text-truncate" },
                { "data": "moTa", "name": "MoTa", "autoWidth": true },
                {
                    data: "tepDinhKem",
                    render: function (data, type, row) {
                        if (data != null && data != "")
                            return "<a href='/../../FileUpload/DeTaiNghienCuu/" + data + "' download='" + data + "'>" + row.tenTep + "</a>";
                        else
                            return "";
                    }
                },
                {
                    "data": "idgiangVienNavigation", "name": "idgiangVienNavigation", "autoWidth": true,
                    orderable: false,
                    render: function (data, type, row) {
                        return '<a>'+data.ho+' '+data.ten+'</a>';
                    }
                },
                
            ],

        });

        function ChangeStatus(id,type) {
            $.ajax({
                url: 'PheDuyetYeuCau/ChangeStatus',
                type: 'POST',
                data: { idDeTai: id, type: type },
                success: function (res) {
                    if (res.status == true)
                    {
                        toastr.success(res.mess);
                        table.ajax.reload();
                        table2.ajax.reload();
                    }
                    else
                        toastr.error(res.mess);
                }
            });
        }

        $(document).delegate('.btnApprove', 'click', function () {
            var idDeTai = $(this).data('id');
            var type = 1; // approve
            ChangeStatus(idDeTai, type);
        })  

        $(document).delegate('.btnReject', 'click', function () {
            var idDeTai = $(this).data('id');
            var type = 0; // reject
            $('#btnXoa').click(function(){
                ChangeStatus(idDeTai, type);
            })
        }) 

        $(document).delegate('.btnInfo', 'click', function (e) {
            e.preventDefault();
            var idDeTai = $(this).data('id');
            $.get('PheDuyetYeuCau/ThongTinEdit', { id: idDeTai }, function (data) {
                $("#ThongTinEdit .modal-body").html(data);
                $("#ThongTinEdit").modal();
            });
        }) 

    });
})();