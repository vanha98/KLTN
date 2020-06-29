(function () {
    $(function () {

        //bsCustomFileInput.init();
        //init datatable
        var url = "/SinhVien/DeTaiDaDangKy/LoadData"
        var table = $("#example1").DataTable({
            //"responsive": true,
            "autoWidth": false,
            "scrollX": true,
            "pageLength": 10,
            //"fixedColumns": {
            //    leftColumns: 0,
            //    rightColumns: 1,
            //},
            "language": {
                "lengthMenu": '',
                "zeroRecords": "Không có dữ liệu",
                "info": '',
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
                
                {
                    orderable: false,
                    addClass: "text-center",
                    "render": function (data, type, full, meta) {
                        return '<button class="btn btn-sm btn-primary btnXemNhom" data-toggle="modal" data-target="#modal-lg"  onclick="XemNhom(' + full.id + ')" data-id="' + full.id + '"><i class="fas fa-user-friends"></i> Xem nhóm </button>';
                    }
                },
                { "data": "tinhTrangPheDuyet", "name": "TinhTrangPheDuyet", "autoWidth": true },

                //thông tin GVHD
                { "data": "hoTenGVHD", "name": "HoTenGVHD", "autoWidth": true },
                { "data": "sdt", "name": "SDT", "autoWidth": true },
                { "data": "email", "name": "Email", "autoWidth": true },
                {
                    orderable: false,
                    data: "tinhTrangPheDuyet",
                    "render": function (data, type, full, meta) {
                        if (data != "Hoàn thành" && data != "Đã hủy") {
                            return '<button class="btn btn-sm btn-default btnXemBaoCao" title="Xem báo cáo" data-id="' + full.id + '" onclick="BaoCaoTienDo(' + full.id + ')"><i class="fas fa-clipboard-list"></i></button>' +
                                '<button class="btn btn-sm btn-default Delete" title="Hủy đề tài" data-id="' + full.id + '" data-toggle="modal" data-target="#ConfirmDelete"><i class="far fa-trash-alt"></i></button>';
                        }
                        else
                            return '<button class="btn btn-sm btn-default btnXemBaoCao" title="Xem báo cáo" data-id="' + full.id + '" onclick="BaoCaoTienDo(' + full.id + ')"><i class="fas fa-clipboard-list"></i></button>';
                    }
                },
            ],
            //"order": [[7, "asc"]],
            //"createdRow": function (row, data, dataIndex) {
            //    if (data.tinhTrangPheDuyet == "Đã gửi") {
            //        $(row).addClass('changeRowColor');
            //    }
            //}
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

        //Delete Object
        $(document).delegate('.Delete', 'click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            var url = '/SinhVien/DeTaiDaDangKy/HuyDeTai';
            $('#btnXoa').on('click', function () {
                HuyDeTai(url, id);
            });
        });
        //Change Status
        function HuyDeTai(url, data) {
            $.ajax({
                type: 'POST',
                url: url,
                data: { id: data },
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
    });
})();