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
                { "data": "tenGiangVien", "name": "TenGiangVien", "autoWidth": true },
                { "data": "loaiYeuCau", "name": "LoaiYeuCau", "autoWidth": true },
                { "data": "ngayTao", "name": "NgayTao", "autoWidth": true },
                { "data": "idNguoiDuyet", "name": "IdNguoiDuyet", "autoWidth": true },
                { "data": "status", "name": "Status", "autoWidth": true },
            ],
            
        });

      
    });
})();