
$(document).ready(function () {
    $("#login").autocomplete({

        source: function (request, response) {
            $.ajax({
                url: "/Home/Index",
                type: "POST",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return (item)

                    }))

                }

            })

        },
        minLength: 3,
        messages: {
            noResults: "", results: ""

        }

    });

})