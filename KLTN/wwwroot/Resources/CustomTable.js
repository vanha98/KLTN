class CustomTable {
    constructor(TableName, btnCreate, btnSave) {
        this.TableName = TableName;
        this.btnCreate = btnCreate;
        this.btnSave = btnSave;
    }
    Init() {
        $('#' + this.TableName).DataTable({
            "responsive": true,
            "autoWidth": false,
            pageLength: 10,
            language: {
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
        });
    };
}