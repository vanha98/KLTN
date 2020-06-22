(function () {
    $(function () {

        //bsCustomFileInput.init();
        //init datatable
        var btnCreate = "<button class='btn btn-success btn-sm btnCreate' onclick='CreateAction()'>Tạo đề tài</button>";
        var btnSave = "<button class='btn btn-primary btn-sm' id='btnGui'>Gửi đề tài</button>" +"<button class='btn btn-sm btn-danger ml-1' id='btnHuyGui'>Hủy gửi đề tài</button>";
        var url = "/GVHD/QLDeTai/LoadData"
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
                "info": btnSave,
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
                {
                    orderable:false,
                    data: "tinhTrangPheDuyet",
                    render: function (data, type, row) {
                        if(data == "Chưa gửi" || data == "Đã gửi")
                            return "<input class='form-check-inline' name='SelectRow' type='checkbox' data-id='" + row.id + "' />";
                        else
                            return "<input class='form-check-inline' name='SelectRow' type='checkbox' data-id='" + row.id + "' disabled/>";
                    },
                },
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "tenDeTai", "name": "TenDeTai", "autoWidth": true },
                { "data": "moTa", "name": "MoTa", "autoWidth": true },
                { "data": "ngayLap", "name": "NgayLap", "autoWidth": true },
                {
                    orderable: false,
                    addClass: "text-center",
                    "render": function (data, type, full, meta) {
                        return '<button class="btn btn-sm btn-primary btnXemNhom" onclick="XemNhom(' + full.id + ')" data-id="' + full.id + '"><i class="fas fa-user-friends"></i> Xem nhóm </button>';
                    }
                },
                {
                    data: "tepDinhKem",
                    render: function (data, type, row) {
                        if (data != null && data != "")
                            return "<a href='/../../FileUpload/DeTaiNghienCuu/" + data + "' download='" + data + "'>" + row.tenTep + "</a>";
                        else
                            return "";
                    }
                },
                //{ "data": "tenTep", "name": "TenTep", "autoWidth": true },
                { "data": "tinhTrangPheDuyet", "name": "TinhTrangPheDuyet", "autoWidth": true },
                {
                    orderable:false,
                    "render": function (data, type, full, meta) {
                        return '<button class="btn btn-sm btn-default" id="btnSua" data-id="'+full.id+'" onclick="EditAction('+full.id+')"><i class="far fa-edit"></i></button>'+
                        '<button class="btn btn-sm btn-default Delete" data-id="'+full.id+'" data-toggle="modal" data-target="#ConfirmDelete"><i class="far fa-trash-alt"></i></button>';
                    }
                },
            ],
            "order": [[7, "asc"]],
            "createdRow": function (row, data, dataIndex) {
                if (data.tinhTrangPheDuyet == "Đã gửi") {
                    $(row).addClass('changeRowColor');
                }
            }
        });

        ////
        //if (localStorage.getItem("Message")) {
        //    toastr.success(localStorage.getItem("Message"));
        //    localStorage.clear();
        //}

        //Refresh Value
        function Refresh() {
            $("#InputTenDeTai").val('');
            $("#InputMoTa").val('');
            $("#Files").val("");
            $(".custom-file-label").text("Chọn tệp");
            $('.lblFiles').text('');
            $('.lblTenDeTai').text('');
        };

        //Change Status
        function ChangeStatus(url, data, type) {
            $.ajax({
                type: 'POST',
                url: url,
                data: { data: data, type: type },
                success: function (response) {
                    if (response.status == true) {
                        //localStorage.setItem("Message",response.mess)
                        table.draw();
                        toastr.success(response.mess);
                        //window.location.reload();
                    }
                    else
                        toastr.error(response.toastr);
                }
            });
        };

        //CreateEdit Data
        function CreateEdit(data, CheckType) {
            var url = '/GVHD/QLDeTai/Edit';
            if (CheckType == "" || CheckType == null) {
                url = '/GVHD/QLDeTai/Create';
            }
            $.ajax({
                type: 'POST',
                url: url,
                processData: false,
                contentType: false,
                data: data,
                //async: false,
                success: function (response) {
                    if (response.status == true && response.create == true) {
                        table.ajax.reload();
                        toastr.success(response.mess);
                        $("#modal-lg").modal('hide');
                        Refresh();
                    }
                    else if (response.status == true) {
                        table.ajax.reload();
                        toastr.success(response.mess);
                        $("#modal-lg").modal('hide');
                        //localStorage.setItem("Message", response.mess)
                        //window.location.reload();
                    }
                    else {
                        $('.lblFiles').text(response.mess);
                        if (response.toastr != "")
                            toastr.error(response.toastr);
                    }
                }
            });
        };

        //Gui de tai
        $(document).delegate("#btnGui", 'click', function () {
            var data = $("table input:checkbox:checked").map(function () {
                return $(this).data('id');
            }).get(); // <----
            console.log(data);
            var url = '/GVHD/QLDeTai/ChangeStatus';
            ChangeStatus(url, data, 0);
        })
        //Huy gui de tai
        $(document).delegate("#btnHuyGui", 'click', function () {
            var data = $("table input:checkbox:checked").map(function () {
                return $(this).data('id');
            }).get(); // <----
            console.log(data);
            var url = '/GVHD/QLDeTai/ChangeStatus';
            ChangeStatus(url, data, 1);
        });

        //Delete Object
        $(document).delegate('.Delete', 'click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var url = '/GVHD/QLDeTai/ChangeStatus';
            $('#btnXoa').on('click', function () {
                ChangeStatus(url, id, 2);
            });
        });

        // Add the following code if you want the name of the file appear on select
        $(document).delegate('.custom-file-input', 'change', function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });

        //Open Model Create new object
        $(".btnCreate").on('click', function () {
            $(".titleHeader").text("Tạo mới");
            Refresh();

        });

        //Open Model Edit object
        $(document).delegate('#btnSua', 'click', function () {
            $(".titleHeader").text("Chỉnh sửa");
            //$("#CheckType").val(1);
        });

        //Save
        $(document).delegate('#btnLuu', 'click', function () {
            var CheckType = $("#CheckType").val();
            var Id = $("#InputId").val();
            var TenDeTai = $("#InputTenDeTai").val();
            if (TenDeTai == "") {
                $('.lblTenDeTai').text('Tên đề tài không được trống');
                return;
            } else { $('.lblTenDeTai').text(''); }
            var MoTa = $("#InputMoTa").val();
            var files = $("#Files").get(0).files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append('Files', files[i]);
            }
            data.append('Id', Id);
            data.append('TenDeTai', TenDeTai);
            data.append('MoTa', MoTa);
            CreateEdit(data, CheckType);
        });

    });
})();