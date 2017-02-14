'use strict';

function $j_table(additionalParameters, model) {
    var props = {
        isActive: false,

        data: {
                currentModelIndex: 0
            },

        render: function () {
            console.log(this);
            alert(this.isActive);
            if (this.isActive) {
                var table = document.getElementById(this.$j_tableId);
                var pagingNavBar = document.getElementById(this.$j_tableId + "_pagingNavBar");

                if (table || pagingNavBar) {
                    table.remove();
                    this.buildTable();
                    pagingNavBar.remove();
                    this.buildPagingNavBar();
                }
                else {
                    this.build();
                }
            }
        },

            update: function()
            {
                this.models[this.data.currentModelIndex].updateData(this.$j_ajax.url);
            },

            init: function () {
                this.setupHandlers()
                    .enable();

                this.update();
            },

            build: function () {
                if (this.$j_settings.selector.options.length > 0) {
                    this.buildSelector();
                }

                this.buildTable();

                this.buildPagingNavBar();
            },

            show: function () {
                this.isActive = true;
                this.update();
            },

            hide: function () {
                alert('hide' + this);
                this.isActive = false;
            },

            buildSelector: function () {
                var self = this;
                    var selector = new $j_selector(
                        {
                            $j_selectorId: self.$j_tableId + "Selector",
                            
                            $j_container: self.$j_container,

                            $j_data: {
                                info: this.$j_settings.selector.info,
                                options: this.$j_settings.selector.options
                            },
                        });

                    selector = document.getElementById(this.$j_tableId + "Selector");

                    selector.onchange = function () {
                        self.changeDataCount();
                    }
                },

            buildTable: function () {
                var tableContainer = document.getElementsByClassName(this.$j_container)[0];
                if (!tableContainer)
                {
                    if (this.$j_container) {
                        var tableContainer = document.createElement('div');
                        tableContainer.setAttribute('class', this.$j_container);
                    } else
                    {
                        var tableContainer = document.createElement('div');
                        tableContainer.setAttribute('class', '$j_' + this.$j_tableId);
                        this.$j_container = '$j_' + this.$j_tableId;
                    }
                }

                var tbl = document.createElement('table');
                var tbdy = document.createElement('tbody');
                var teams = this.models[this.data.currentModelIndex].get('teams');

                tbl.style.width = '100%';
                tbl.setAttribute('border', '1');
                tbl.setAttribute('id', this.$j_tableId);

                var row = document.createElement('tr');           
                if (this.$j_data.headers.length > 0) {
                    for (var i = 0; i < this.$j_data.headers.length; i++) {
                        var header = document.createElement('th');
                        header.appendChild(document.createTextNode(this.$j_data.headers[i]));
                        row.appendChild(header);
                    }
                }
                tbdy.appendChild(row);

                for (var i = 0; i < teams.data.length; i++) {
                    var team = teams.data[i];

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
                document.body.appendChild(tableContainer);
            },

            buildPagingNavBar: function () {
                var self = this;
               
                var pagingContainer = document.getElementsByClassName(this.$j_container)[0];

                var pagination_ul = document.createElement('ul');
                pagination_ul.setAttribute('class', 'pagination');
                pagination_ul.setAttribute('id', this.$j_tableId + "_pagingNavBar");
                var pagination_li = document.createElement('li');
                var pagination_li_a = document.createElement('a');
                pagination_li_a.setAttribute('href', '#');

                pagination_li_a.onclick = function () {
                    self.prevPage();
                }

                if (this.$j_settings.pagination.prev == '') {
                    pagination_li_a.innerHTML = 'Previous';
                }
                else {
                    pagination_li_a.innerHTML = this.$j_settings.pagination.prev;
                }
                pagination_li.appendChild(pagination_li_a);
                pagination_ul.appendChild(pagination_li);

                pagination_li = document.createElement('li');
                pagination_li_a = document.createElement('a');

                if (this.$j_settings.pagination.first == "") {
                    pagination_li_a.innerHTML = 1;
                }
                else {
                    pagination_li_a.innerHTML = this.$j_settings.pagination.first;
                }

                pagination_li_a.id = 'page ' + 1;
                pagination_li_a.onclick = function () {
                    self.choosePage(this.id.split(' ')[1]);
                }
                pagination_li.appendChild(pagination_li_a);
                pagination_ul.appendChild(pagination_li);

                if (this.models[this.data.currentModelIndex].data.current_page <= 4)
                {
                    for (var iter = 1; iter < 5 ; iter++) {
                        pagination_li = document.createElement('li');
                        pagination_li_a = document.createElement('a');
                        pagination_li_a.innerHTML = iter + 1;
                        pagination_li_a.id = 'page ' + (iter + 1);
                        pagination_li_a.onclick = function () {
                            self.choosePage(this.id.split(' ')[1]);
                        }
                        pagination_li.appendChild(pagination_li_a);
                        pagination_ul.appendChild(pagination_li);
                    }

                    pagination_li = document.createElement('li');
                    pagination_li_a = document.createElement('a');
                    pagination_li_a.innerHTML = "...";
                    pagination_li_a.id = 'page r_divider';
                    pagination_li.appendChild(pagination_li_a);
                    pagination_ul.appendChild(pagination_li);

                }
                else
                {
                    if ((this.numPages() - this.models[this.data.currentModelIndex].data.current_page) < 4)
                    {
                        pagination_li = document.createElement('li');
                        pagination_li_a = document.createElement('a');
                        pagination_li_a.innerHTML = "...";
                        pagination_li_a.id = 'page l_divider';
                        pagination_li.appendChild(pagination_li_a);
                        pagination_ul.appendChild(pagination_li);

                        for (var iter = this.numPages() - 5; iter < this.numPages() - 1; iter++) {
                            pagination_li = document.createElement('li');
                            pagination_li_a = document.createElement('a');
                            pagination_li_a.innerHTML = iter + 1;
                            pagination_li_a.id = 'page ' + (iter + 1);
                            pagination_li_a.onclick = function () {
                                self.choosePage(this.id.split(' ')[1]);
                            }
                            pagination_li.appendChild(pagination_li_a);
                            pagination_ul.appendChild(pagination_li);
                        }
                    }
                    else
                    {
                        pagination_li = document.createElement('li');
                        pagination_li_a = document.createElement('a');
                        pagination_li_a.innerHTML = "...";
                        pagination_li_a.id = 'page l_divider';
                        pagination_li.appendChild(pagination_li_a);
                        pagination_ul.appendChild(pagination_li);

                        for (var iter = this.models[this.data.currentModelIndex].data.current_page - 2; iter < (+this.models[this.data.currentModelIndex].data.current_page + 1); iter++)
                        {
                            pagination_li = document.createElement('li');
                            pagination_li_a = document.createElement('a');
                            pagination_li_a.innerHTML = iter + 1;
                            pagination_li_a.id = 'page ' + (iter + 1);
                            pagination_li_a.onclick = function () {
                                self.choosePage(this.id.split(' ')[1]);
                            }
                            pagination_li.appendChild(pagination_li_a);
                            pagination_ul.appendChild(pagination_li);
                        }

                        pagination_li = document.createElement('li');
                        pagination_li_a = document.createElement('a');
                        pagination_li_a.innerHTML = "...";
                        pagination_li_a.id = 'page r_divider';
                        pagination_li.appendChild(pagination_li_a);
                        pagination_ul.appendChild(pagination_li);
                    }

                    pagination_li = document.createElement('li');
                    pagination_li_a = document.createElement('a');
                }

                pagination_li = document.createElement('li');
                pagination_li_a = document.createElement('a');

                if (this.$j_settings.pagination.last == "") {
                    pagination_li_a.innerHTML = this.numPages();
                }
                else {
                    pagination_li_a.innerHTML = this.$j_settings.pagination.last;
                }
                pagination_li_a.id = 'page ' + this.numPages();
                pagination_li_a.onclick = function () {
                    self.choosePage(this.id.split(' ')[1]);
                }
                pagination_li.appendChild(pagination_li_a);
                pagination_ul.appendChild(pagination_li);
            
                pagination_li = document.createElement('li');
                pagination_li_a = document.createElement('a');
                pagination_li_a.setAttribute('href', '#');
                pagination_li_a.onclick = function () {
                    self.nextPage();
                }

                if (this.$j_settings.pagination.next == '')
                {
                    pagination_li_a.innerHTML = 'Next';
                }
                else
                {
                    pagination_li_a.innerHTML = this.$j_settings.pagination.next;
                }

                pagination_li.appendChild(pagination_li_a);
                pagination_ul.appendChild(pagination_li);

                pagingContainer.appendChild(pagination_ul);

                pagination_li_a = document.getElementById('page ' + this.models[this.data.currentModelIndex].data.current_page);
                pagination_li_a.setAttribute('class', 'active');
            },

            choosePage: function (page) {
                if (this.models[this.data.currentModelIndex].data.current_page != page) {
                    this.models[this.data.currentModelIndex].data.current_page = page;
                    this.changePage(this.models[this.data.currentModelIndex].data.current_page);
                }
            },

            prevPage: function () {
                if (this.models[this.data.currentModelIndex].data.current_page > 1) {
                    this.models[this.data.currentModelIndex].data.current_page--;
                    this.changePage(this.models[this.data.currentModelIndex].data.current_page);
                }
            },

            nextPage: function () {
                if (this.models[this.data.currentModelIndex].data.current_page < this.numPages()) {
                    this.models[this.data.currentModelIndex].data.current_page++;
                    this.changePage(this.models[this.data.currentModelIndex].data.current_page);
                }
            },

            numPages: function () {
                return Math.ceil(this.models[this.data.currentModelIndex].get('teams').recordsTotal / this.models[this.data.currentModelIndex].data.records_per_page);
            },

            changePage: function (page) {
                // Validate page

                if (page < 1) page = 1;
                if (page > this.numPages()) page = this.numPages();

                //отрисовка данных по номеру страницы, запрос на сервер отдельная функция
                var pagingNavBar = document.getElementById(this.$j_tableId + '_pagingNavBar');
                var pages = pagingNavBar.getElementsByTagName('a');

                for (var i = 0; i < pages.length; i++) {
                    if (pages[i].getAttribute('class') == 'active') {
                        pages[i].removeAttribute('class');
                    }
                    if (pages[i].id == page) {
                        pages[i].setAttribute('class', 'active');
                    }
                }
                this.update();
            },

            changeDataCount: function () {
                var selected_index = document.getElementById(this.$j_tableId + 'Selector').selectedIndex;
                if (selected_index => 0) {
                    var selected_option_value = document.getElementById(this.$j_tableId + 'Selector').options[selected_index].value;
                    this.models[0].data.records_per_page = selected_option_value;
                    this.models[0].data.current_page = 1;
                    this.changePage(1);
                }
                else {
                    alert('non selected');
                }
            }
        }

    _.extend(props, additionalParameters);

    return new View(props, model)
};