// document.addEventListener('DOMContentLoaded', function() {
//   var elems = document.querySelectorAll('.sidenav');
//   // var elems = document.querySelectorAll('.parallax');
//   var instances = M.Sidenav.init(elems, Option);
//   // var instances = M.Parallax.init(elems, Option);

// });
$(document).ready(function () {

    showPreLoader();

    sleep(500).then(() => { hidePreLoader(); });
});


M.AutoInit();
document.addEventListener('DOMContentLoaded', function() {
    var elems = document.querySelectorAll('.slider');
    var instances = M.Slider.init(elems, Option);
  });
  var instance = M.Carousel.init({
    fullWidth: true
  });
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function showPreLoader() {
    $("#docconter").css("opacity", "0.01");
    $('#pre').removeClass('hide');
}

function hidePreLoader() {
    $('#pre').addClass('hide');
    $("#docconter").css("opacity", "1");
}
function getin(name) {
    $(name).fadeIn("slow");
}
function getout(name) {
    $(name).fadeOut();
}
function gettoggle(name) {
    $(name).fadeToggle();
}
