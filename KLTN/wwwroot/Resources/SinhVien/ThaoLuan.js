$(document).ready(function ()
{

    var ulImg = $('#imgAttach');

    function CheckTab(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu)
    {
        if (CongKhaiTab)
        {
            SetBaiPostCongKhai.html("");
        }
        else
        {
            SetBaiPostRiengTu.html("");
        }
    };

    function ShowBaiPost(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu, response)
    {
        if (CongKhaiTab)
        {
            $.each(response.data, function (i, item)
            {
                var Data = '<tr id="' + item.id + '">' +
                    '<td><label class="text-truncate" style="max-width: 190px">' + item.tieuDe + '</label ></td >' +
                    '<td>' + item.ngayPost + '</td>' +
                    '</tr>';
                SetBaiPostCongKhai.append(Data);
            });
        }
        else
        {
            $.each(response.data, function (i, item)
            {
                var Data = '<tr id="' + item.id + '">' +
                    '<td><label class="text-truncate" style="max-width: 190px">' + item.tieuDe + '</label ></td >' +
                    '<td>' + item.ngayPost + '</td>' +
                    '<td>' + item.idDeTai + '</td>' +
                    '</tr>';
                SetBaiPostRiengTu.append(Data);
            });
        }
    };

    function LoadNoiDung(idbaipost)
    {
        var SetNoiDung = $("#txtNoiDungBaiPost");
        var SetTieuDe = $("#txtTieuDeBaiPost");
        SetTieuDe.html("");
        SetNoiDung.html("");
        ulImg.html("");
        $.ajax({
            url: "/SinhVien/ThaoLuan/NoiDungBaiPost",
            type: "post",
            data: {
                idbaipost: idbaipost
            },
            dataType: "json",
            success: function (response)
            {
                if (response.status)
                {
                    var datatieude = '<h3 class="col text-center">' + response.data.tieuDe + '</h3>';
                    SetTieuDe.append(datatieude);
                    if (response.data.noiDung != null)
                        SetNoiDung.append("<p style='white-space: pre-line'>" + response.data.noiDung + "</p>");
                    $(".btnEdit").attr("data-id", idbaipost);
                    $(".btnDelete").attr("data-id", idbaipost);
                    LoadImg(response.listImg);
                    DisableEdit();
                }
            }
        })
    }

    function LoadImg(listImg)
    {
        if (listImg != null || listImg != '')
        {
            $.each(listImg, function (i, item)
            {
                var li = '<li class="img_' + item.id + '">' +
                    '<div class="img-wrap"><span class="close" title="Gỡ ảnh"><a class="DeleteImg" hidden href="#" data-id="' + item.id + '">&times;</a></span>' +
                    '<span class="mailbox-attachment-icon has-img"><img data-toggle="modal" data-target="#modalImg" src="/../../img/ThaoLuan/' + item.anhDinhKem + '" alt="Attachment"></span></div>' +
                    '<div class="mailbox-attachment-info">' +
                    '<a href="#" class="mailbox-attachment-name"><i class="fas fa-camera"></i>' + item.tenAnh + '</a>' +
                    '<span class="mailbox-attachment-size clearfix mt-1">' +
                    '<span>' + item.kichThuoc + '</span>' +
                    '<a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>' +
                    '</span>' +
                    '</div>' +
                    '</li>';
                ulImg.append(li);
            });
            var last = '<li class="addImg" hidden><button class="btn-block h-100 btn btnaddImg"><i style="font-size:86px">+</i></button></li>';
            ulImg.append(last);
        }
    };

    function DisableEdit()
    {
        $('#txtNoiDungBaiPost').prop('hidden', false);
        $('#txtTieuDeBaiPost').prop('hidden', false);

        $('#inputTieuDe').prop('hidden', true);
        $('#inputNoiDung').prop('hidden', true);
        $('.btnLuuEdit').prop('hidden', true);
        $('.btnHuyEdit').prop('hidden', true);
        $('.DeleteImg').prop('hidden', true);
        $('.addImg').prop('hidden', true);
    }

    var arrayImg = Array();
    //Delete imgTemp
    $(document).delegate('.DeleteImgTemp', 'click', function ()
    {
        var filename = $(this).data('id');
        $.each(arrayImg, function (i, item)
        {
            if (item.name === filename)
            {
                arrayImg.splice(i, 1);
                $(".img_" + filename.split('.')[0]).remove();
                return false;
            }
        });
        console.log(arrayImg);
    });

    //Save data Edit


    //add Img when Edit
    $(document).delegate('.btnaddImg', 'click', function ()
    {
        $('#imgFile').trigger('click');

    });
    $('#imgFile').change(function ()
    {
        var files = $(this)[0].files;
        $.each(files, function (i, item)
        {
            arrayImg.push(item);
        });
        console.log(arrayImg);
        if (files.length > 1)
        {
            for (i = 0; i < files.length; i++)
            {
                var reader = new FileReader();
                reader.fileName = files[i].name;
                reader.index = i;
                reader.onload = function (e)
                {
                    var li = '<li class="img_' + e.target.fileName.split('.')[0] + '">' +
                        '<div class="img-wrap"><span class="close" title="Gỡ ảnh"><a class="DeleteImgTemp" data-id="' + e.target.fileName + '" href="#">&times;</a></span>' +
                        '<span class="mailbox-attachment-icon has-img"><img data-toggle="modal" data-target="#modalImg" src="' + e.target.result + '" alt="Attachment"></span></div>' +
                        '<div class="mailbox-attachment-info">' +
                        '<a href="#" class="mailbox-attachment-name"><i class="fas fa-camera"></i>' + e.target.fileName + '</a>' +
                        '<span class="mailbox-attachment-size clearfix mt-1">' +
                        '<span></span>' +
                        '<a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>' +
                        '</span>' +
                        '</div>' +
                        '</li>';
                    ulImg.prepend(li);
                };
                reader.readAsDataURL(files[i]);
            }
        }
        else
        {
            var reader = new FileReader();
            reader.fileName = files[0].name;
            reader.onload = function (e)
            {
                var li = '<li class="img_' + e.target.fileName.split('.')[0] + '">' +
                    '<div class="img-wrap"><span class="close" title="Gỡ ảnh"><a class="DeleteImgTemp" data-id="' + e.target.fileName + '" href="#">&times;</a></span>' +
                    '<span class="mailbox-attachment-icon has-img"><img data-toggle="modal" data-target="#modalImg" src="' + e.target.result + '" alt="Attachment"></span></div>' +
                    '<div class="mailbox-attachment-info">' +
                    '<a href="#" class="mailbox-attachment-name"><i class="fas fa-camera"></i>' + files[0].name + '</a>' +
                    '<span class="mailbox-attachment-size clearfix mt-1">' +
                    '<span></span>' +
                    '<a href="#" class="btn btn-default btn-sm float-right"><i class="fas fa-cloud-download-alt"></i></a>' +
                    '</span>' +
                    '</div>' +
                    '</li>';
                ulImg.prepend(li);
            };
            reader.readAsDataURL(files[0]);
        }
        //$('#imgFile').val('');
    });

    function DeleteImg(id)
    {
        $.ajax({
            url: "/GVHD/ThaoLuan/GoAnh",
            type: "post",
            data: {
                id: id
            },
            dataType: "json",
            success: function (response)
            {
                if (response.status)
                {
                    $(".img_" + id).remove();
                }
            }
        });
    }

    //Delete Img
    $(this).delegate('.DeleteImg', 'click', function (e)
    {
        e.preventDefault();
        var id = $(this).data('id');
        DeleteImg(id);
    });

    //Huy Edit
    $('.btnHuyEdit').on('click', function ()
    {
        var id = $('#valueIdBaiPost').val();
        LoadNoiDung(id);
    });

    //Luu
    $('.btnLuuEdit').on('click', function ()
    {
        var id = $('#valueIdBaiPost').val();
        var NoiDung = $('#inputNoiDung').val();
        var TieuDe = $('#inputTieuDe').val();
        var data = new FormData();
        for (var i = 0; i < arrayImg.length; i++)
        {
            data.append('Files', arrayImg[i]);
        }
        data.append('NoiDung', NoiDung);
        data.append('Id', id);
        data.append('TieuDe', TieuDe);
        Edit(data, id);
    });

    //Edit BaiPost
    $('.btnEdit').on('click', function ()
    {
        var id = $(this).attr('data-id');
        $('#valueIdBaiPost').val(id);

        var txtNoiDung = $('#txtNoiDungBaiPost');
        txtNoiDung.prop('hidden', true);
        var txtTieuDe = $('#txtTieuDeBaiPost');
        txtTieuDe.prop('hidden', true);

        $('#inputTieuDe').prop('hidden', false);
        $('#inputNoiDung').prop('hidden', false);
        $('.btnLuuEdit').prop('hidden', false);
        $('.btnHuyEdit').prop('hidden', false);
        $('.DeleteImg').prop('hidden', false);
        $('.addImg').prop('hidden', false);

        $('#inputTieuDe').val(txtTieuDe.text());
        $('#inputNoiDung').val(txtNoiDung.text());
        $('#inputNoiDung').focus();

    });

    function Edit(data, id)
    {
        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");
        $.ajax({
            type: 'POST',
            url: '/SinhVien/ThaoLuan/Edit',
            processData: false,
            contentType: false,
            data: data,
            //async: false,
            success: function (response)
            {
                if (response.status == true)
                {
                    //table.ajax.reload();
                    toastr.success(response.mess);
                    if (!CongKhaiTab)
                        $("#tblRiengTu").load(window.location.href + " #tblRiengTu");
                    else
                        $("#tblCongKhai").load(window.location.href + " #tblCongKhai");
                    LoadNoiDung(id);
                    arrayImg = [];
                }
                else
                {
                    toastr.error(response.mess);
                }
            },
        });
    };

    //Delete BaiPost
    $(document).delegate('.btnDelete', 'click', function ()
    {
        var id = $(this).data('id');
        $.ajax({
            url: '/GVHD/ThaoLuan/XoaBaiPost',
            type: 'POST',
            data: { id: id },
            success: function (response)
            {
                if (response.status)
                {
                    toastr.success(response.mess);
                    $("#mydiv").load(window.location.href + " #mydiv");
                }
            }
        })
    })

    //Search BaiPost
    $("#btnSearch").click(function ()
    {
        var SearchString = $("#txtSearch").val();
        var SetBaiPostCongKhai = $("#tblCongKhai");
        var SetBaiPostRiengTu = $("#tblRiengTu");
        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");

        CheckTab(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu);

        $.ajax({
            url: "/GVHD/ThaoLuan/SearchBaiPost",
            type: "post",
            data: {
                SearchString: SearchString,
                CongKhaiTab: CongKhaiTab
            },
            dataType: "json",
            success: function (response)
            {
                if (response.status)
                {
                    ShowBaiPost(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu, response);
                }
                else
                {
                    if (CongKhaiTab)
                    {
                        SetBaiPostCongKhai.append('<tr><td colspan="2">Không tìm thấy tiêu đề</td></tr>');
                    }
                    else
                    {
                        SetBaiPostRiengTu.append('<tr><td colspan="2">Không tìm thấy tiêu đề</td></tr>');
                    }

                }
            }
        })
    });

    //Refresh List BaiPost
    $("#LkRefreshList").click(function ()
    {
        var CongKhaiTab = $("#custom-tabs-one-home-tab").hasClass("active");
        var SetBaiPostCongKhai = $("#tblCongKhai");
        var SetBaiPostRiengTu = $("#tblRiengTu");
        CheckTab(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu);

        $.ajax({
            url: "/GVHD/ThaoLuan/RefreshList",
            type: "post",
            data: {
                CongKhaiTab: CongKhaiTab
            },
            dataType: "json",
            success: function (response)
            {
                if (response.status)
                {
                    ShowBaiPost(CongKhaiTab, SetBaiPostCongKhai, SetBaiPostRiengTu, response);
                }
            }
        })
    });

    //Click Row BaiPost hien thi noi dung
    $(document).on("click", "#tblSinhVien tr", function ()
    {
        $('#tblSinhVien tr').removeClass("table-info");
        $(this).toggleClass("table-info");
        var idbaipost = $(this).attr("id");
        LoadNoiDung(idbaipost);
    });

    //Click checkbox Public or Private
    $('input.form-check-input').on('change', function ()
    {
        $('input.form-check-input').not(this).prop('checked', false);
        if ($(this).hasClass('privateCheck') && $(this).is(':checked'))
            $('#divChonDeTai').prop('hidden', false);
        else
            $('#divChonDeTai').prop('hidden', true);
    });

    // Add the following code if you want the name of the file appear on select
    $(document).delegate('.custom-file-input', 'change', function ()
    {
        var files = $(this)[0].files;
        if (files.length > 1)
        {
            var fileName = files[0].name;
            for (i = 1; i < files.length; i++)
            {
                fileName = fileName + ', ' + files[i].name;
            }
        }
        else
        {
            var fileName = $(this).val().split("\\").pop();
        }
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    function Refresh()
    {
        $('#InputTieuDe').val('');
        $('#InputNoiDung').val('');
        $('#SelectDeTai option:selected').val('');
        $('#SelectDeTai').prop('hidden', true);
        $('.publicCheck').prop('checked', false);
        $('.privateCheck').prop('checked', false);
        $("#Files").val("");
        $(".custom-file-label").text("Chọn tệp");
    }

    //Create BaiPost
    function Create(data)
    {
        var SetBaiPostCongKhai = $("#tblCongKhai");
        var SetBaiPostRiengTu = $("#tblRiengTu");
        $.ajax({
            type: 'POST',
            url: '/GVHD/ThaoLuan/Create',
            processData: false,
            contentType: false,
            data: data,
            //async: false,
            success: function (response)
            {
                if (response.status == true)
                {
                    //table.ajax.reload();
                    toastr.success(response.mess);
                    $("#modal-lg").modal('hide');
                    Refresh();
                    if (response.data.loai == 0)
                    {
                        var Html = '<tr id="' + response.data.id + '">' +
                            '<td><label class="text-truncate" style="max-width: 190px">' + response.data.tieuDe + '</label ></td >' +
                            '<td>' + response.temp + '</td>' +
                            '</tr>';
                        SetBaiPostCongKhai.append(Html);
                    }
                    else
                    {
                        var Html = '<tr id="' + response.data.id + '">' +
                            '<td><label class="text-truncate" style="max-width: 190px">' + response.data.tieuDe + '</label ></td >' +
                            '<td>' + response.temp + '</td>' +
                            '<td>' + response.data.idkenhThaoLuanNavigation.iddeTai + '</td>' +
                            '</tr>';
                        SetBaiPostRiengTu.append(Html);
                    }
                }
                else
                {
                    $('.lblFiles').text(response.mess);
                }
            }
        });
    };

    //Button Luu Popup
    $('#btnLuu').on('click', function ()
    {
        var TieuDe = $('#InputTieuDe').val();
        if (TieuDe == null || TieuDe == "")
        {
            $('.lblTieuDe').text("Tiêu đề không được để trống!");
            return;
        }
        else
            $('.lblTieuDe').text("");
        var NoiDung = $('#InputNoiDung').val();
        var IdDeTaiNghienCuu = $('#SelectDeTai option:selected').val();
        var PublicCheck = $('.publicCheck').is(':checked');
        var PrivateCheck = $('.privateCheck').is(':checked');
        var Type = '';
        if (PublicCheck)
            Type = 0;
        else if (PrivateCheck)
        {
            Type = 1;
            if (IdDeTaiNghienCuu == null || IdDeTaiNghienCuu == "")
            {
                $('.lblChonDeTai').text("Vui lòng chọn đề tài");
                return;
            }
            else
            {
                $('.lblChonDeTai').text("");
            }
        }
        else
        {
            $('.textCheckbox').text("Vui lòng chọn chế độ của bài đăng");
            return;
        }
        var files = $("#Files").get(0).files;
        var data = new FormData();
        for (var i = 0; i < files.length; i++)
        {
            data.append('Files', files[i]);
        }
        data.append('TieuDe', TieuDe);
        data.append('NoiDung', NoiDung);
        data.append('IdDeTaiNghienCuu', IdDeTaiNghienCuu);
        data.append('Loai', Type);
        Create(data);
    })
});