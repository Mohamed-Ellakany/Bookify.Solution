
var updatedRow;
var table;
var datatable;

var exportedRows = [];

function ShowSuccessMessage(message = 'Saved Successfully') {

    Swal.fire({
        text: message,
        icon: "success",
        buttonsStyling: false,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
        }
    });

}

function ShowErrorMessage(message = 'An error occurred') {
    
    Swal.fire({
        text: message.responseText != undefined ? message.responseText : message,
        icon: "error",
        buttonsStyling: false,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
        }
    });

}
function onModalBegin() {
    $("body :submit").attr("disabled", "disabled").attr("data-kt-indicator", "on");
}
function onModalSuccess(row) {
    ShowSuccessMessage();
    $('#Modal').modal('hide');

    if (updatedRow !== undefined) {

        datatable.row(updatedRow).remove().draw();
        updatedRow = undefined;
    }


    var newRow = $(row);
    datatable.row.add(newRow).draw();

    KTMenu.init();
    KTMenu.initHandlers(); 
   // KTMenu.initGlobalHandlers();
}
function onModalComplete() {
    $("body :submit").removeAttr("disabled").removeAttr("data-kt-indicator");
}


//select2
function applySelect2() {
    $('.js-select2').select2();
    $('.js-select2').on('select2:select', function (e) {
        $('form').validate().element('#' + $(this).attr('id'));
    });
}


var headers = $('th');
$.each(headers, function (i) {
    var col = $(this);
    if (!col.hasClass('no-export')) {
        exportedRows.push(i);
    }

});

//datatables
var KTDatatablesExample = function () {
    // Shared variables


    // Private functions
    var initDatatable = function () {




        // Set date data order
        const tableRows = table.querySelectorAll('tbody tr');


        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = table.getAttribute('data-report-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedRows
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedRows
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedRows
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedRows
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-render-datatable');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();




$(document).ready(function () {
    // tinyMCE  
    if ($(".js-tinymce").length > 0) {
        var options = {
            selector: ".js-tinymce", height: "460", resize: false
        };

        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }

        tinymce.init(options);
    }


    // datepicker
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        drops: 'up',
        autoApply: true,
        maxDate : new Date()
    });

    //select2
    applySelect2();



    // Datatable
    KTUtil.onDOMContentLoaded(function () {
        KTDatatablesExample.init();
    });



    // alerts 
    var message = $('#message').text();
    if (message) {
        ShowSuccessMessage(message);
    }

    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');

        $('#ModalLabel').text(btn.data('title'));
        if (btn.data('update') !== undefined) {
            updatedRow = btn.closest('tr');
        }

        $.ajax({
            url: btn.data('url'),
            method: 'GET',
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);

                applySelect2();

            },
            error: function () {
                ShowErrorMessage();
            }
        })



        modal.modal('show');


    })

    // handle toggle status
    $('body').delegate('.js-toggle-status', 'click', function () {

        var btn = $(this);
        bootbox.confirm({
            title: "Are you sure you want to toggle the status of this item ?",
            message: "This action will change the availability of this item.",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {

                    $.post({
                        url: btn.data('url'),
                        data: {
                            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (lastUpdatedOn) {
                            var row = btn.parents('tr')
                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === "Available" ? "Deleted" : "Available";
                            status.text(newStatus).toggleClass('badge-light-danger badge-light-success');

                            row.find('.js-updated-on').html(lastUpdatedOn);
                            row.addClass('animate__animated animate__flash');

                            ShowSuccessMessage();

                            setTimeout(function () {
                                row.removeClass('animate__animated animate__flash');
                            }, 1000)

                        },
                        error: function () {
                            ShowErrorMessage();
                        }
                    });
                }
            }
        });

    });


    $('body').delegate('.js-confirm', 'click', function () {

        var btn = $(this);
        bootbox.confirm({
            message: btn.data('message'),
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {

                    $.post({
                        url: btn.data('url'),
                        data: {
                            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function () {
                            ShowSuccessMessage();

                        },
                        error: function () {
                            ShowErrorMessage();
                        }
                    });
                }
            }
        });

    });


    // handle sign out button

    $('.js-signout').on('click', function () {

        $('#SignOut').submit();

    });


})