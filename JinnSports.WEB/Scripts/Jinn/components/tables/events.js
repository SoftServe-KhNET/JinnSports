'use strict';

function eventsView(model) {

    this.table = new $j_table({
        $j_tableId: "teamsTable",

        //$j_container: "$j_table-component",

        $j_ajax: {
            url: "/api/Team/LoadTeams"
        },
        $j_settings: {

            pagination:
                {
                    next: "Следующая",
                    prev: "Предыдущая",
                    first: "",
                    last: "",
                    infoFormat: "Show __$j_START__ to __$j_FINISH__ from __$j_COUNT__",
                },
            selector:
                {
                    info: "Show __$j_Selector__ records",
                    options: ["10", "20", "30", "40"]
                }
        },
        $j_data: {
            headers: ["Number", "Team Name"]
        }
    }, model);

    this.selector = new $j_selector(
        {
            $j_selectorId: "eventsSelector",

            //$j_container: "newContainer",

            $j_data: {
                info: "Please input time",
                options: ["10", "25", "50", "100"]
            },
        });
};
_.extend(eventsView.prototype, {
    init: function () {
        console.log(this);
        this.selector.init();
        this.table.init();
        var selector = document.getElementById(self.selector.$j_selectorId);
        selector.onchange = function () {
            self.changeTime();
        }
    }(),

    changeTime: function() {
        var selected_index = document.getElementById(this.selector.$j_selectorId).selectedIndex;
        console.log(selected_index);
        if (selected_index => 0) {
            var selected_option_value = document.getElementById(this.selector.$j_selectorId).options[selected_index].value;
            console.log(selected_option_value);
            alert('event!!!');
        }
        else {
            alert('non selected');
        }
    }
});