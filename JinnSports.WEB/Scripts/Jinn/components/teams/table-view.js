'use strict';

function tableView(model) {
    return new View({

        render: function () {
            var table = document.getElementById("teamsTable");

            if (table)
            {
                table.remove();
                this.buildTable();
            }
            else
            {
                this.show();
            }
        },

        init: function () {
            this.setupHandlers()
                .enable();
            this.models[0].updateData(this.models[0].data.records_per_page, this.models[0].data.records_per_page * (this.models[0].data.current_page - 1), this.models[0].data.records_per_page);
        },

        show: function () {
            this.buildTable();
            this.buildPagingNavBar();
            this.buildSelector();
        },

        buildSelector: function () {
            alert('buildSelector');
            var selectContainer = document.getElementsByClassName('js-select-container')[0];
            var selector = document.createElement('select');
            selector.setAttribute('class', 'select-style');
            selector.setAttribute('id', 'tableSelector');
            var proto = this;

            selector.onchange = function () {
                proto.changeDataCount();
            }

            function addOption(oList, optionName, optionValue) {
                var oOption = document.createElement("option");
                oOption.appendChild(document.createTextNode(optionName));
                oOption.setAttribute("value", optionValue);
                oList.appendChild(oOption);
            }

            addOption(selector, '10', '10');
            addOption(selector, '25', '25');
            addOption(selector, '50', '50');
            addOption(selector, '100', '100');

            selectContainer.appendChild(selector);
        },

        buildTable: function () {
            alert('build table');
            var tableContainer = document.getElementsByClassName('js-table-container')[0];
            var tbl = document.createElement('table');

            //tbl.setAttribute('class', 'table');
            var tbdy = document.createElement('tbody');
            var teams = this.models[0].get('teams');

            console.log(teams);

            tbl.style.width = '100%';
            tbl.setAttribute('border', '1');
            tbl.setAttribute('id', 'teamsTable');

            for (var i = 0; i < teams.data.length; i++) {
                var team = teams.data[i];

                console.log(team);
                var row = document.createElement('tr');

                for (var team_prop in team) {
                    var cell = document.createElement('td');
                    if (team.hasOwnProperty(team_prop)) {
                        var cellText = document.createTextNode(team[team_prop]);
                    }

                    cell.appendChild(cellText);
                    row.appendChild(cell);
                }
                tbdy.appendChild(row);
            }
            tbl.appendChild(tbdy);
            tableContainer.appendChild(tbl);
        },

        buildPagingNavBar: function () {
            alert('buildSNavBar');
            var proto = this;
            var pagingContainer = document.getElementsByClassName('js-paging-container')[0];

            var pagination_ul = document.createElement('ul');
            pagination_ul.setAttribute('class', 'pagination');
            pagination_ul.setAttribute('id', 'pagingNavBar');
            var pagination_li = document.createElement('li');
            var pagination_li_a = document.createElement('a');
            pagination_li_a.setAttribute('href', '#');

            pagination_li_a.onclick = function () {
                proto.prevPage();
            }

            pagination_li_a.innerHTML = '&laquo';
            pagination_li.appendChild(pagination_li_a);
            pagination_ul.appendChild(pagination_li);

            pagination_li = document.createElement('li');
            pagination_li_a = document.createElement('a');
            pagination_li_a.innerHTML = 1;
            pagination_li_a.id = 1;
            pagination_li_a.onclick = function () {
                proto.choosePage(this.id);
            }
            pagination_li_a.setAttribute('class', 'active');
            pagination_li.appendChild(pagination_li_a);
            pagination_ul.appendChild(pagination_li);

            for (var iter = 1; iter < this.numPages() ; iter++) {
                pagination_li = document.createElement('li');
                pagination_li_a = document.createElement('a');
                pagination_li_a.innerHTML = iter + 1;
                pagination_li_a.id = iter + 1;
                pagination_li_a.onclick = function () {
                    proto.choosePage(this.id);
                }
                pagination_li.appendChild(pagination_li_a);
                pagination_ul.appendChild(pagination_li);
            }

            pagination_li = document.createElement('li');
            pagination_li_a = document.createElement('a');
            pagination_li_a.setAttribute('href', '#');
            pagination_li_a.onclick = function () {
                proto.nextPage();
            }
            pagination_li_a.innerHTML = '&raquo';
            pagination_li.appendChild(pagination_li_a);
            pagination_ul.appendChild(pagination_li);

            pagingContainer.appendChild(pagination_ul);
        },

        choosePage: function (page) {
            if (this.models[0].data.current_page != page) {
                this.models[0].data.current_page = page;
                this.changePage(this.models[0].data.current_page);
            }
        },

        prevPage: function () {
            if (this.models[0].data.current_page > 1) {
                this.models[0].data.current_page--;
                this.changePage(this.models[0].data.current_page);
            }
        },

        nextPage: function () {
            if (this.models[0].data.current_page < this.numPages()) {
                this.models[0].data.current_page++;
                this.changePage(this.models[0].data.current_page);
            }
        },

        numPages: function () {
            return Math.ceil(this.models[0].get('teams').recordsTotal / this.models[0].data.records_per_page);
        },

        changePage: function (page) {
            alert('changePage');
            // Validate page
            if (page < 1) page = 1;
            if (page > this.numPages()) page = this.numPages();

            //отрисовка данных по номеру страницы, запрос на сервер отдельная функция
            var pagingNavBar = document.getElementById('pagingNavBar');
            var pages = pagingNavBar.getElementsByTagName('a');

            for (var i = 0; i < pages.length; i++) {
                if (pages[i].getAttribute('class') == 'active') {
                    pages[i].removeAttribute('class');
                }
                if (pages[i].id == page) {
                    pages[i].setAttribute('class', 'active');
                }
            }
            this.models[0].updateData(this.models[0].data.records_per_page, this.models[0].data.records_per_page * (this.models[0].data.current_page - 1) + 1, this.models[0].data.records_per_page);
        },

        changeDataCount: function () {
            var selected_index = document.getElementById('tableSelector').selectedIndex;

            if (selected_index > 0) {
                var selected_option_value = document.getElementById('tableSelector').options[selected_index].value;
                this.models[0].data.records_per_page = selected_option_value;
            }
            else {
                alert('non selected');
            }

        }

    }, model)
};