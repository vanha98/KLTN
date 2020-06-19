(function () {
    $(function () {
        
        //init datatable
        var btnCreate = "<button class='btn btn-success btn-sm btnCreate' onclick='CreateAction()'>Đề xuất đề tài/Chỉnh sửa</button>";
        var btnSave = "<button class='btn btn-primary btn-sm' id='btnGui'>Lưu đề tài</button>";
        var url = "/SinhVien/DangKyDeTai/LoadData"
        var table = $("#example1").DataTable({
            "autoWidth": false,
            "responsive":true,
            //"scrollX": true,
            //"fixedColumns": {
            //    leftColumns: 3,
            //},
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
                    orderable: false,
                    data: "tinhTrangDangKy",
                    render: function (data, type, row) {
                        if (data == "Còn")
                            return "<input class='form-check-inline SelectRow' type='checkbox' data-id='" + row.id + "' />";
                        else
                            return "<input class='form-check-inline' type='checkbox' disabled/>";
                    },
                },
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "tenDeTai", "name": "TenDeTai", "autoWidth": true },
                {
                    orderable: false,
                    data: "loai",
                    className: 'text-center',
                    render: function (data, type, row) {
                        if (data === true)
                            return "<input class='form-check-inline' type='checkbox' disabled/>";
                        else
                            return "<input class='form-check-inline' type='checkbox' checked disabled/>";
                    },
                },
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
                { "data": "tinhTrangDangKy", "name": "TinhTrangDangKy", "autoWidth": true, "className": 'text-center', },

                //thông tin GVHD
                { "data": "hoTenGVHD", "name": "HoTenGVHD", "autoWidth": true },
                { "data": "sdt", "name": "SDT", "autoWidth": true },
                { "data": "email", "name": "Email", "autoWidth": true },
                //
                //{
                //    orderable: false,
                //    "render": function (data, type, full, meta) {
                //        return '<button class="btn btn-sm btn-default" id="btnXemNhom" data-id="' + full.id + '"><i class="fas fa-user-friends"></i></button>';
                            
                //    }
                //},
            ],
            //"order": [[7, "asc"]],
            //"createdRow": function (row, data, dataIndex) {
            //    if (data.tinhTrangPheDuyet == "Đã gửi") {
            //        $(row).addClass('changeRowColor');
            //    }
            //}
        });


        if (localStorage.getItem("Message")) {
            toastr.success(localStorage.getItem("Message"));
            localStorage.clear();
        }

        //Load DeTaiDaDangKy
        $.get('/SinhVien/DangKyDeTai/LoadCurrentDeTai', function (response) {
            if (response == null || response == "") {
                return;
            }
            else {
                $("#example1 input.SelectRow:checkbox").attr("disabled", true);
            }
            var checkbox = '';
            if (response.data.loai === false)
                checkbox = "<input class='form-check-inline' type='checkbox' checked disabled/>";
            else
                checkbox = "<input class='form-check-inline' type='checkbox' disabled/>";
            var html = "<tr style='background: #C0C0C0'>" +
                "<th><input class='form-check-inline NotSelectRow' type='checkbox' data-id='" + response.data.id + "' checked/></th>" +
                "<th>" + response.data.id + "</th>" +
                "<th>" + response.data.tenDeTai + "</th>" +
                "<th class='text-center'>" + checkbox + "</th>" +
                "<th>" + response.data.moTa + "</th>" +
                "<th><a href='/../../FileUpload/DeTaiNghienCuu/" + response.data.tepDinhKem + "' download='" + response.data.tepDinhKem + "'>" + response.data.tenTep + "</a></th>" +
                "<th>Đã đăng ký</th>" +
                "<th>" + response.data.idgiangVienNavigation.ho + ' ' + response.data.idgiangVienNavigation.ten + "</th>" +
                "<th>" + response.data.idgiangVienNavigation.sdt + "</th>" +
                "<th>" + response.data.idgiangVienNavigation.email + "</th>" +
                "<th><button class='btn btn-sm btn-default btnXemNhom' data-id='" + response.data.id + "'><i class='fas fa-user-friends'></i></button></th>" +
                "</tr>";
            $("#Idfooter").html(html);
        });

        $(document).delegate(".inputMSSV",'keyup',function () {
            if ($(this).val() != null && $(this).val() != "")
                $("#btnDKNhom").attr("disabled", false);
            else
                $("#btnDKNhom").attr("disabled", true);
        })

        //CheckPopupNhom
        function CheckPopupNhom(id) {
            $.ajax({
                url: 'DangKyDeTai/CheckPopupNhom',
                type: 'POST',
                data: { idDeTai: id},
                success: function (res) {
                    if (res.status == false) {
                        $("#DKNhom .modal-body").html('');
                        $("#DKNhom .modal-footer").html('');
                        var body = '<div class="row">'+
                            '<input type="text" maxlength="12" class="form-control inputMSSV" placeholder ="Nhập MSSV" />'+
                '</div>'+
                            '<div class="mt-1"><label class="text-red text-bold" id="lblMSSV" hidden></label></div>';
                        var footer = '<button type="button" class="btn btn-primary" id="btnDKNhom" disabled>Đăng ký</button>';
                        $("#DKNhom .modal-body").html(body);
                        $("#DKNhom .modal-footer").html(footer);
                        $("#DKNhom").modal();
                    }
                    else {
                        $("#DKNhom .modal-body").html('');
                        $("#DKNhom .modal-footer").html('');
                        $("#DKNhom .modal-body").html(res);
                        $("#DKNhom").modal();
                    }
                }
            });
        }

        //Đăng ký nhóm
        $(document).delegate('.btnXemNhom', 'click', function () {
            var id = $(this).data('id');
            CheckPopupNhom(id);

            $(document).delegate("#btnDKNhom",'click',function () {
                var mssv = $(".inputMSSV").val();
                    $.ajax({
                    url: 'DangKyDeTai/DangKyNhom',
                    type: 'POST',
                    data: { idDeTai: id, mssv:mssv },
                    success: function (res) {
                        if (res.status == true) {
                            $("#lblMSSV").prop('hidden', true);
                            toastr.success(res.mess);
                            $("#DKNhom").modal('hide');
                        }
                        else {
                            $("#lblMSSV").prop('hidden', false);
                            $("#lblMSSV").html(res.mess);
                        }
                    }
                });
            });
            
        })

        //Hủy đăng ký nhóm
        $(document).delegate('.btnHuyNhom', 'click', function () {
            var id = $(".btnXemNhom").data('id');
            $.ajax({
                url: '/SinhVien/DangKyDeTai/HuyNhom',
                type: 'POST',
                data: {idDeTai:id},
                success: function (res) {
                    if (res.status == true) {
                        toastr.success(res.mess);
                        $("#DKNhom").modal('hide');
                    }
                    else
                        toastr.error(res.mess);
                }
            })
        });

        //Hủy đăng ký đề tài
        $(document).delegate('.NotSelectRow','click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $("#ConfirmDelete").modal();
            $("#btnXoa").click(function () {
                $.ajax({
                    url: '/SinhVien/DangKyDeTai/HuyDangKy',
                    type: 'POST',
                    data: { id: id },
                    success: function (res) {
                        if (res.status == true) {
                            $("#Idfooter").html('');
                            table.ajax.reload();
                            toastr.success(res.mess);
                        }
                        else
                            toastr.error(res.mess);
                    }
                })
            })
        })

        

        //Single Select
        $(document).delegate('input.SelectRow','change', function () {
            $('input.SelectRow').not(this).prop('checked', false);
        });

        // Add the following code if you want the name of the file appear on select
        $(document).delegate('.custom-file-input', 'change', function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });

        function Refresh() {
            $("#InputTenDeTai").val('');
            $("#InputMoTa").val('');
            $("#Files").val("");
            $(".custom-file-label").text("Chọn tệp");
            $('.lblFiles').text('');
            $('.lblTenDeTai').text('');
        };

        //Open Model Create new object
        $(".btnCreate").on('click', function () {
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

        function CreateEdit(data, CheckType) {
            $.ajax({
                url: '/SinhVien/DangKyDeTai/Create',
                type: 'POST',
                processData: false,
                contentType: false,
                data: data,
                success: function (res) {
                    if (res.status == true) {

                    }
                    else
                        toastr.error(res.mess);
                }
            });
        }

        //Luu de tai
        $(document).delegate("#btnGui", 'click', function () {
            var data = $("#example1 input:checkbox:checked").data('id')
            console.log(data);
            if (data != null) {
                var url = '/SinhVien/DangKyDeTai/LuuDeTai';
                LuuDeTai(url, data);
            }
            else
                toastr.error('Bạn chưa chọn đề tài');
        })

        function LuuDeTai(url, id) {
            $.ajax({
                url: url,
                type: "POST",
                data: { id: id },
                success: function (response) {
                    if (response.status == true) {
                        //var checkbox = '';
                        //if (response.data.loai === false)
                        //    checkbox = "<input class='form-check-inline' type='checkbox' checked disabled/>";
                        //else
                        //    checkbox = "<input class='form-check-inline' type='checkbox' disabled/>";
                        //var html = "<tr style='background: #C0C0C0'>" +
                        //    "<th><input class='form-check-inline NotSelectRow' type='checkbox' data-id='" + response.data.id + "' checked/></th>" +
                        //    "<th>" + response.data.id + "</th>" +
                        //    "<th>" + response.data.tenDeTai + "</th>" +
                        //    "<th class='text-center'>" + checkbox + "</th>" +
                        //    "<th>" + response.data.moTa + "</th>" +
                        //    "<th>" + response.data.tenTep + "</th>" +
                        //    "<th></th>" +
                        //    "<th>" + response.data.idgiangVienNavigation.ho + ' ' + response.data.idgiangVienNavigation.ten + "</th>" +
                        //    "<th>" + response.data.idgiangVienNavigation.sdt + "</th>" +
                        //    "<th>" + response.data.idgiangVienNavigation.email + "</th>" +
                        //    "<th><button class='btn btn-sm btn-default' id='btnXemNhom' data-id='" + response.data.id + "'><i class='fas fa-user-friends'></i></button></th>" +
                        //    "</tr>";
                        //$("#Idfooter").html(html);
//                        toastr.success(response.mess);
                        localStorage.setItem('Message', response.mess);
                        location.reload();
                    }
                },
            });
        }

    });
})();
        