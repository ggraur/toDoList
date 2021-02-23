function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}


//function GabContabSelectChanged1() {
//    //$('#GabContab').change(function () {
//    var selectedGab = $("#GabContab").val();
//    var empresaSelect = $('#EmprGab');
//    var anoEmprSelect = $('#AnoEmprGab');
//    empresaSelect.empty();
//    anoEmprSelect.empty();
//    if (selectedGab != null && selectedGab != '') {
//        alert('empty');
//        $.getJSON('@Url.Action("GetEmprContab")', { EmpresaID: selectedGab }, function (empresas) {
//            if (empresas != null && !jQuery.isEmptyObject(empresas)) {

//                $.each(empresas, function (index, empresa) {
//                    empresaSelect.append($('<option/>', {
//                        value: empresa.Value,
//                        text: empresa.Text
//                    }));
//                });
//            };
//        }, 'json');
//        $.getJSON('@Url.Action("GetAnoEmprContab")', { EmpresaID: "0" }, function (empresas) {
//            if (empresas != null && !jQuery.isEmptyObject(empresas)) {
//                $.each(empresas, function (index, empresa) {
//                    anoEmprSelect.append($('<option/>', {
//                        value: empresa.Value,
//                        text: empresa.Text
//                    }));
//                });
//            };
//        }, 'json');
//    };
//    //});
//}
//function EmpresaContabSelected1() {
//    $('#EmprGab').change(function () {
//        var selectedGab = $("#GabContab").val();
//        var empresaSelect = $('#EmprGab').val();
//        var anoEmprSelect = $('#AnoEmprGab');
//        anoEmprSelect.empty();
//        if (selectedGab != null && selectedGab != '' && empresaSelect != null && empresaSelect != '') {
//            $.getJSON('@Url.Action("GetAnoEmprContab")', { EmpresaID: empresaSelect }, function (empresas) {
//                if (empresas != null && !jQuery.isEmptyObject(empresas)) {
//                    $.each(empresas, function (index, empresa) {
//                        anoEmprSelect.append($('<option/>', {
//                            value: empresa.Value,
//                            text: empresa.Text
//                        }));
//                    });
//                };
//            }, 'json');
//        }
//    });
//}

function VoltarParaListaEmpresas() {
    var b = RenderWithNoRefresh("/Empresa/Index")
}

function GabContabSelectChanged(_selectedGab) {

   // alert(_selectedGab + 'sdfsfdfg');

    var selectedGab = _selectedGab;// $("#GabContab").val();
    var empresaSelect = $('#EmprGab');
    var anoEmprSelect = $('#AnoEmprGab');
    empresaSelect.empty();

    if (selectedGab !== null && selectedGab !== '') {
        $.ajax({
            url: '/GabContab/GetEmprContab',
            dataType: "json",
            type: "GET",
            cache: false,
            data: { EmpresaID: selectedGab },
            success: function (jsondata) {
                $.each(jsondata, function (index, data) {
                    empresaSelect.append($('<option/>', {
                        value: data.Value,
                        text: data.Text
                    }));
                });
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    };

    anoEmprSelect.empty();

    if (empresaSelect != null && empresaSelect != '') {
        $.ajax({
            url: '/GabContab/GetAnoEmprContab',
            dataType: "json",
            type: "GET",
            cache: false,
            data: { EmpresaID: empresaSelect },
            success: function (jsondata) {
                $.each(jsondata, function (index, data) {
                    anoEmprSelect.append($('<option/>', {
                        value: data.Value,
                        text: data.Text
                    }));
                });
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    };
}

function EmpresaContabSelectChanged()  {
    var selectedGab = $("#GabContab").val();
    var empresaSelect = $("#EmprGab").val();
    var anoEmprSelect = $("#AnoEmprGab");
    anoEmprSelect.empty();
    $.ajax({
        url: '/GabContab/GetAnoEmprContab',
        dataType: "json",
        type: "GET",
        cache: false,
        data: { EmpresaID: empresaSelect },
        success: function (jsondata) {
            $.each(jsondata, function (index, data) {
                anoEmprSelect.append($('<option/>', {
                    value: data.Value,
                    text: data.Text
                }));
            });
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function AnoSelectChanged() {
     var anoEmprSelect = $("#AnoEmprGab :selected").text();
    $.ajax({
        url: '/GabContab/SaveSessionAnoEmprContab',
        dataType: "json",
        type: "GET",
        cache: false,
        data: { AnoSelectionado: anoEmprSelect },
        success: function (jsondata) {
              
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}

function RenderWithNoRefreshWithData(_url, _data, _method) {
    //alert("RenderWithNoRefreshWithData");
    $.ajax({
        url: _url,
        type: _method,
        cache: false,
        data: _data,
        //dataType:"json",
        success: function (data) {
            $("#renderbody").html(data);
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}

function RenderWithNoRefresh(_url) {
    $.ajax({
        url: _url,
        type: "GET",
        cache: false,
        success: function (data) {
            $("#renderbody").html(data);
        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    })
}

function SideBarStiky() {
    var $sidebar = $("#mySideBar"),
        $window = $(window),
        offset = $sidebar.offset(),
        topPadding = 15;

    //$window.scroll(function () {
        if ($window.scrollTop() > offset.top) {
            $sidebar.stop().animate({
                marginTop: $window.scrollTop() - offset.top + topPadding
            });
        } else {
            $sidebar.stop().animate({
                marginTop: 0
            });
        }
    //});
}
 
/* upload files functions */
//var Upload = function (file) {
//    this.file = file;
//};

//Upload.prototype.getType = function () {
//    return this.file.type;
//};
//Upload.prototype.getSize = function () {
//    return this.file.size;
//};
//Upload.prototype.getName = function () {
//    return this.file.name;
//};
//Upload.prototype.doUpload = function (_url, method) {
//    var that = this;
//    var formData = new FormData();

//    // add assoc key values, this will be posts values
//    formData.append("file", this.file, this.getName());
//    formData.append("upload_file", true);

//    $.ajax({
//        type: method, //"POST",
//        url: _url, //"CLab/UploadFile",
//        data: formData,
//        xhr: function () {
//            var myXhr = $.ajaxSettings.xhr();
//            if (myXhr.upload) {
//                myXhr.upload.addEventListener('progress', that.progressHandling, false);
//            }
//            return myXhr;
//        },
//        success: function (data) {
//            $('#idUpload').html(data.message);
//            $('#encPath').attr('value', data.filePath)
//            $('#idWell').show().css("display", "inline").fadeOut(3000);
//            //filePath
//        },
//        error: function (error) {
//            // handle error
//        },
//        async: true,
//        data: formData,
//        cache: false,
//        contentType: false,
//        processData: false,
//        timeout: 60000
//    });
//};
//Upload.prototype.progressHandling = function (event) {
//    var percent = 0;
//    var position = event.loaded || event.position;
//    var total = event.total;
//    var progress_bar_id = "#progress-wrp";
//    if (event.lengthComputable) {
//        percent = Math.ceil(position / total * 100);
//    }
//    // update progressbars classes so it fits your code
//    $(progress_bar_id + " .progress-bar").css("width", +percent + "%");
//    $(progress_bar_id + " .status").text(percent + "%");
//};

/* upload files functions */