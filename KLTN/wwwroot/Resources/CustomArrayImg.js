class CustomArrayImg {
    constructor(fileImgName, ulName, array) {
        this.fileImgName = fileImgName;
        this.ulName = ulName;
        this.array = [];

        var seft = this;
        // On button search click.
        //if (btnAction !== null && btnAction.length > 0) {
        //    $("#" + btnAction).click(function () {
        //    });
        //}
    }

    getInstance() {
        return $('#' + this.fileImgName);
    };

    makeTagli(cssName, data, img, name) {
        var li = '<li class="img_' + cssName + '">' +
            '<div class="img-fluid"><span class="close" title="Gỡ ảnh"><a class="DeleteImgTemp" data-id="' + data + '" href="#">&times;</a></span>' +
            '<span class="mailbox-attachment-icon has-img"><img data-toggle="modal" data-target="#modalImg" data-src = "'+img+'" src="' + img + '" alt="Attachment"></span></div>' +
            '<div class="mailbox-attachment-info">' +
            '<a href="#" class="mailbox-attachment-name"><i class="fas fa-camera"></i>' + name + '</a>' +
            '<span class="mailbox-attachment-size clearfix mt-1">' +
            '<span></span>' +
            '</span>' +
            '</div>' +
            '</li>';
        return li;
    };

    pushImgToArray() {
        var obj = this;
        var ul = $('#' + obj.ulName);
        var files = this.getInstance()[0].files;
        $.each(files, function (i, item) {
            obj.array.push(item);
        });
        console.log(obj.array);
        if (files.length > 1) {
            for (var i = 0; i < files.length; i++) {
                var reader = new FileReader();
                reader.fileName = files[i].name;
                reader.index = i;
                reader.onload = function (e) {
                    var li = obj.makeTagli(e.target.fileName.split('.')[0], e.target.fileName, e.target.result, e.target.fileName);
                    ul.prepend(li);
                };
                reader.readAsDataURL(files[i]);
            }
        }
        else {
            var reader = new FileReader();
            reader.fileName = files[0].name;
            reader.onload = function (e) {
                var li = obj.makeTagli(e.target.fileName.split('.')[0], e.target.fileName, e.target.result, files[0].name);
                ul.prepend(li);
            };
            reader.readAsDataURL(files[0]);
            
        }

    }

    DeleteImgTemp(filename) {
        var obj = this;
        $.each(obj.array, function (i, item) {
            if (item.name === filename) {
                obj.array.splice(i, 1);
                $(".img_" + filename.split('.')[0]).remove();
                return false;
            }
        });
        console.log(obj.array);
    }
}