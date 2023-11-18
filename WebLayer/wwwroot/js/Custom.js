
function LoadCkEditor4() {

    if (!document.getElementById("ckeditor-field"))
        return;

    $("body").append("<script src='/ckeditor4/ckeditor/ckeditor.js'></script>");

    CKEDITOR.replace('ckeditor-field',
        {
            customConfig: '/ckeditor4/ckeditor/config.js'
        });
}
function changePage(page) {
    var url = new URL(window.location.href);
    var search_params = url.searchParams;

    // Change Page
    search_params.set('page', page);
    url.search = search_params.toString();

    // the new url string
    var new_url = url.toString();
    window.location.replace(new_url);
}
$(document).ready(function() {
    LoadCkEditor4();
})