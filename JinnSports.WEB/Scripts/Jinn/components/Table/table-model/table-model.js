'use strict';
var httpRequest = null;

function tableModel() {

    return new Model({
        data: {
            teams: [],
        },

        updateData: function (connectionString, records_per_page, currentPage) {
            var self = this;
            sendRequestToServer();

            function sendRequestToServer() {
                var draw = records_per_page;
                var start = records_per_page * (currentPage - 1);
                var length = records_per_page;
                if (connectionString.includes('?')) {
                    var params = connectionString.split('?')[1] + ["&draw=" + draw + "&start=" + start + "&length=" + length];
                    connectionString = connectionString.split('?')[0]
                }
                else {
                    var params = ["draw=" + draw + "&start=" + start + "&length=" + length];
                }
                self.sendRequest(connectionString, params, getResponseFromServer, "GET");
            }

            function getResponseFromServer() {
                if (httpRequest.readyState == 4) {
                    if (httpRequest.status == 200) {
                        self.set('teams', JSON.parse(httpRequest.responseText));
                    }
                }
            }
        },

        getXMLHttpRequest: function () {
            if (window.ActiveXObject) {
                try {
                    return new ActiveXObject("Msxml2.XMLHTTP");
                } catch (e) {
                    try {
                        return new ActiveXObject("Microsoft.XMLHTTP");
                    } catch (e1) {
                        return null;
                    }
                }
            } else if (window.XMLHttpRequest) {
                return new XMLHttpRequest();
            } else {
                return null;
            }
        },

        sendRequest: function (url, params, callback, method) {
            httpRequest = this.getXMLHttpRequest();
            var httpMethod = method ? method : 'GET';
            if (httpMethod != 'GET' && httpMethod != 'POST') {
                httpMethod = 'GET';
            }
            var httpParams = params;

            var httpUrl = url;
            if (httpMethod == 'GET' && httpParams != null) {
                httpUrl = httpUrl + "?" + httpParams;
            }

            httpRequest.open(httpMethod, httpUrl, true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onreadystatechange = callback;
            httpRequest.send(httpMethod != 'POST' ? httpParams : null);
        }
    })
};