Vue.filter('amountAddDot', function (num) {
    var source = String(num).split(".");
    source[0] = source[0].replace(
        new RegExp("(\\d)(?=(\\d{3})+$)", "ig"),
        "$1,"
    );
    return source.join(".");
})
// Vue.filter('isNumber', function (evt) {
//     var evt = (evt) ? evt : window.event;
//     var charCode = (evt.which) ? evt.which : evt.keyCode;
//     debugger
//     console.log(evt.key);
//     if (charCode >= 48 && charCode <= 57) {
//         return true;
//     } else {
//         evt.preventDefault();
//     }
// })
Vue.filter('isNumber', function (evt) {
    var evt = (evt) ? evt : window.event;
    var numbers = /^[0-9]+$/;
    if (evt.key.match(numbers)) {
        return true;
    }else {
        evt.preventDefault();
    }
})

