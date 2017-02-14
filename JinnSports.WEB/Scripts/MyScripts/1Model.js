'use strict';
function Model() {    
    //this.data = {www:'www'}    
}

Model.prototype = {

    getData: function () {       
        return this.data;
    },

    setData: function (inpData) {        
        this.data = inpData;       
    },    

    updateData:         
        function (url, callback) {            
            var proto = this;
            var xhr = new XMLHttpRequest();
            xhr.open('GET', url, true);
            xhr.onreadystatechange = function() {
                if (xhr.readyState == 4 && xhr.status == 200) {                    
                    var rez = JSON.parse(xhr.responseText);                    
                    proto.setData(rez);                    
                }
            }
            xhr.send();        
    },

    init: function () {

    },    

    //getXMLHttpRequest: function () {
    //    if (window.ActiveXObject) {
    //        try {
    //            return new ActiveXObject("Msxml2.XMLHTTP");
    //        } catch (e) {
    //            try {
    //                return new ActiveXObject("Microsoft.XMLHTTP");
    //            } catch (e1) {
    //                return null;
    //            }
    //        }
    //    } else if (window.XMLHttpRequest) {
    //        return new XMLHttpRequest();
    //    } else {
    //        return null;
    //    }
    //},

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
}