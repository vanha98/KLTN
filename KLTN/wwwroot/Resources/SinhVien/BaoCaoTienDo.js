(function () {
    $(function () {
        //init datatable
        var btnCreate = "<button type='button' class='btn btn-success btn-sm' onclick='CreateAction()'>Tạo báo cáo</button>";
        var btnSave = "";
        var url = "/SinhVien/BaoCaoTienDo/LoadData"
        var table = $("#example1").DataTable({
            "autoWidth": false,
            "responsive": true,
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
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "ngayNop", "name": "NgayNop", "autoWidth": true },
                { "data": "tuanDaNop", "name": "TuanDaNop", "autoWidth": true, render: function (data, type, row) { return "Tuần " + data; } },
                { "data": "noiDung", "name": "NoiDung", "autoWidth": true },
                {
                    data: "tepDinhKem",
                    render: function (data, type, row) {
                        if (data != null && data != "")
                            return "<a href='/../../FileUpload/BaoCaoTienDo/" + data + "' download='" + data + "'>" + row.tenTep + "</a>";
                        else
                            return "";
                    }
                },
                { "data": "danhGia", "name": "DanhGia", "autoWidth": true }, /*"className": 'text-center',*/ 
                { "data": "tienDo", "name": "TienDo", "autoWidth": true },
                { "data": "status", "name": "Status", "autoWidth": true },
                //
                {
                    orderable: false,
                    "render": function (data, type, full, meta) {
                        return '<button class="btn btn-sm btn-default btnEdit" onclick="EditAction('+full.id+')"><i class="fas fa-pencil-alt"></i></button>';

                    }
                },
            ],
            //"initComplete": function (settings, json) {
            //    LoadDeTaiDaDangKy();
            //}
        });

        //Save
        $(document).delegate('#btnLuu', 'click', function () {
            var Id = $("#InputId").val();
            var IdDeTai = $("#valueIdDeTai").val();
            var NoiDung = $("#InputNoiDung").val();
            if (NoiDung == null || NoiDung == "")
                return;
            var TienDo = $("#InputTienDo").val();
            var files = $("#Files").get(0).files;
            var data = new FormData();
            data.append('File', files[0]);
            data.append('Id', Id);
            data.append('IddeTai', IdDeTai);
            data.append('NoiDung', NoiDung);
            data.append('TienDo', TienDo);
            CreateEdit(data);
        });

        // Add the following code if you want the name of the file appear on select
        $(document).delegate('.custom-file-input', 'change', function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });

        //CreateEdit Data
        function CreateEdit(data) {
            $.ajax({
                type: 'POST',
                url: '/SinhVien/BaoCaoTienDo/CreateEdit',
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

    });
})();