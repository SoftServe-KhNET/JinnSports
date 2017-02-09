'use strict';
var httpRequest = null;

function tableModel() {
    return new Model({

        data: {
            teams: [],
            current_page: 1,
            records_per_page: 10
        },

        updateData: function (draw, start, length) {
            var self = this;
            sendRequestToServer();

            function sendRequestToServer() {
                var params = ["draw=" + draw + "&start=" + start + "&length=" + length];
                self.sendRequest("/api/Team/LoadTeams", params, getResponseFromServer, "GET");
            }

            function getResponseFromServer() {
                if (httpRequest.readyState == 4) {
                    if (httpRequest.status == 200) {
                        self.set('teams', JSON.parse(httpRequest.responseText));
                        console.log(self.get('teams'));
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
            var httpParams = (params == null || params == '') ? null : params;
            var httpUrl = url;
            if (httpMethod == 'GET' && httpParams != null) {
                httpUrl = httpUrl + "?" + httpParams;
            }
            alert(httpRequest);
            httpRequest.open(httpMethod, httpUrl, true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onreadystatechange = callback;
            httpRequest.send(httpMethod == 'POST' ? httpParams : null);
        }
    })
};