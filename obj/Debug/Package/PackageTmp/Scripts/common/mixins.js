var myMixin = {
    created: function () {
        // console.log('mixin created');

    },
    methods: {
        getUrlParameter: function (name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
            var results = regex.exec(location.search);
            return results === null
                ? ""
                : decodeURIComponent(results[1].replace(/\+/g, " "));
        },
        pageBack: function () {
            if (location.href == document.referrer) {
                window.history.go(-1);
                return false;
            }

            location.href = document.referrer;
        },
        scrollToTop: function () {
            setTimeout(function () {
                window.scrollTo(0, 0);
            }, 200)
            return true;
        },
        toast: function (msg) {
            this.counter++
            this.$bvToast.toast(msg, {
                toaster: 'b-toaster-top-center',
                solid: true,
                appendToast: false,
                noCloseButton: true,
                toastClass: 'toastContainer'
            })
        }
    }
}

// Vue.mixin({
//     created: function() {

//     },
//     methods: {

//     }
// })