new Vue({
    el: '.search',
    data: M_MailData,
    mounted:function(){
    },
    methods: {
        search: function () {
            vm_models.refresh();
        }
    }
});

new Vue({
    el: '.goBack',
    data: {},
    methods: {
        goback: function () {
            //if (window.location.pathname == "WEB/SHOPPINGMALL/INDEX") {
            //    location.href = "../../../WEB/";
            window.history.back();
        }
    }
});


