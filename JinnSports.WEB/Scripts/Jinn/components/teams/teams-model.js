'use strict';
var httpRequest = null;

var TeamsModel = function () {
    this.teams = [];
};

TeamsModel.prototype = {

    getTeams: function () {
        alert('get teams');
        return this.teams;
    },

    setTeams: function (teams) {
        this.teams = this.parseData(teams);
        alert(this.teams);
    },

    //updateData: function() {
    //    var updatedTeams = '{ \
    //     "teams": [ \
    //     {"id":"1","name":"Bayern"}, \
    //     {"id":"2","name":"Manchester City"}, \
    //     {"id":"3","name":"Manchester United"}, \
    //     {"id":"4","name":"Juventus"}, \
    //     {"id":"5","name":"Metallist"}, \
    //     {"id":"6","name":"Dynamo Kiev"}, \
    //     {"id":"7","name":"Shakhter Donetsk"}, \
    //     {"id":"8","name":"Spartak Moskva"}, \
    //     {"id":"9","name":"Barcelona"} \
    //     ]\
    //}';

    //this.setTeams(updatedTeams);
    //    //parse
    //},

    updateData: function (draw, start, length) {
        alert('updateData');
        var proto = this;
        sendRequestToServer();
        function sendRequestToServer() {
            var params = ["draw=" + draw + "&start=" + start + "&length=" + length];
            proto.sendRequest("/api/Team/LoadTeams", params, getResponseFromServer, "GET");
        }

        function getResponseFromServer() {
            if (httpRequest.readyState == 4) {
                if (httpRequest.status == 200) {
                    proto.setTeams(httpRequest.responseText);
                    alert('good response');
                }
            }
        }
    },

    init: function() {

    },

    parseData: function (jsonString) {
        console.log(JSON.parse(jsonString).data);
        return JSON.parse(jsonString).data;
    },

    getXMLHttpRequest: function() {
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

    sendRequest: function(url, params, callback, method) {
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
}
