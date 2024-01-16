var options = [
    { selector: '#fadding', offset: 400, callback: 'Materialize.showStaggeredList("#fadding")' },
    { selector: '#hading', offset: 400, callback: 'Materialize.showStaggeredList("#hading")' },
    { selector: '#mading', offset: 400, callback: 'Materialize.showStaggeredList("#mading")' },
    { selector: '#img-ga', offset: 800, callback: 'Materialize.showStaggeredList("#img-ga")' },
    { selector: '#scrool-u', offset: 400, callback: 'Materialize.showStaggeredList("#scrool-u")' },
    { selector: '#builder', offset: 400, callback: 'Materialize.showStaggeredList("#builder")' },
    { selector: '#information', offset: 400, callback: 'Materialize.showStaggeredList("#information")' },

];
Materialize.scrollFire(options);
var mb = document.querySelectorAll('.materialboxed');
M.Materialbox.init(mb, {

})
$(document).ready(function () {
    $('.parallax').parallax();
});
