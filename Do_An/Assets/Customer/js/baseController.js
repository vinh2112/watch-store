var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#txtSearch").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Customer/Product/ListItem",
                    type: "POST", 
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (data) {
                        response(data.data);
                    },
                });
            },
            focus: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;

            },

            select: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;

            },
        })
            .autocomplete("instance")._renderItem = function (ul, item) {
                return $("<li>")
                    .append("<a>" + item.label + "</a>")
                    .append(ul);
            };
    }

}
common.init();

//$(document).ready(function () {

//    $("#txtSearch").autocomplete({

//        source: '/Customer/Product/ListItem',

//        minLength: 1

//    });
//});