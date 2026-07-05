function onAddModelSuccess(row) {

    ShowSuccessMessage();
    $('#Modal').modal('hide');

    $('tbody').prepend(row);
    KTMenu.createInstances();

    var count = $('#CopiesCount')
    var newCount = parseInt(count.text()) + 1;
    count.text(newCount);

    $('table').removeClass('d-none')
    $('.js-alert').addClass('d-none')

}


function onEditModelSuccess(row) {
    ShowSuccessMessage();
    $('#Modal').modal('hide');
    
    $(updatedRow).replaceWith(row);
    KTMenu.createInstances();
}
