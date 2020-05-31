class CustomTable {
    constructor(TableName, btnCreate, btnSave, ajaxUrl, mycolumns) {
        this.TableName = TableName;
        this.btnCreate = btnCreate;
        this.btnSave = btnSave;
        this.ajaxUrl = ajaxUrl;
        this.mycolumns = mycolumns;
    }
    Init() {
        $('#' + this.TableName).DataTable({
            "responsive": true,
            "autoWidth": false,
            "pageLength": 10,
            "language": {
                "lengthMenu": this.btnCreate,
                "zeroRecords": "Không có dữ liệu",
                "info": this.btnSave,
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
                "url": this.ajaxUrl,
                "type": "POST",
                "datatype": "json"
            },
            //Custom Datetime
            //"columnDefs": [{
            //    "targets": [4],
            //    render: $.fn.dataTable.render.moment('YYYY/MM/DD', 'Do MMM YY', 'fr')
            //}],
            "columns": this.mycolumns

            //"columns": [
            //    {
            //        data: null,
            //        render: function (data, type, row) {
            //            return "<input class='form-check-inline' name='SelectRow' type='checkbox' data-id='"+row.id+"' />";
            //        }
            //    },
            //    { "data": "id", "name": "Mã đề tài", "autoWidth": true },
            //    { "data": "tenDeTai", "name": "TenDeTai", "autoWidth": true },
            //    { "data": "moTa", "name": "MoTa", "autoWidth": true },
            //    { "data": "ngayLap", "name": "NgayLap", "autoWidth": true },
            //    { "data": "idnhom", "name": "Idnhom", "autoWidth": true },
            //    { "data": "tenTep", "name": "TenTep", "autoWidth": true },
            //    { "data": "tinhTrangPheDuyet", "name": "TinhTrangPheDuyet", "autoWidth": true },
            //    {
            //        "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/GVHD/QLDeTai/Edit/' + full.id + '">Edit</a>'; }
            //    },
            //    {
            //        data: null,
            //        render: function (data, type, row) {
            //            return "<a href='#' class='btn btn-danger' onclick=DeleteData('" + row.id + "'); >Delete</a>";
            //        }
            //    },
            //]  
        });
    };
    DeleteData(CustomerID) {
    if (confirm("Are you sure you want to delete ...?")) {
        Delete(CustomerID);
    } else {
        return false;
    }
    };


    Delete(CustomerID) {
    var url = '@Url.Content("~/")' + "DemoGrid/Delete";

    $.post(url, { ID: CustomerID }, function (data) {
        if (data) {
            oTable = $('#example').DataTable();
            oTable.draw();
        } else {
            alert("Something Went Wrong!");
        }
    });
    };
}