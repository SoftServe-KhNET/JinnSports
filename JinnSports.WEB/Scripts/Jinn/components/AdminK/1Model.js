'use strict';

function adminModel() {
    return new Model({
        data: {},

        updateData: function () {
            var proto = this;            
            var xhr = new XMLHttpRequest();
            xhr.open('GET', 'https://localhost:44300/api/Conformity', true);             
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var rez = JSON.parse(xhr.responseText);                    
                    proto.set('data', rez);                                      
                }
            }            
            xhr.send();
        },        
        
        send: function (inData) {            
            var self = this;
            var httpRequest = new XMLHttpRequest();

            var antiforgery = $('input[name="__RequestVerificationToken"]').val();

            httpRequest.open('POST', 'https://localhost:44300/api/Conformity', true);
            httpRequest.setRequestHeader('X-XSRF-Token', antiforgery);
            httpRequest.setRequestHeader('Content-Type', 'application/json');
            httpRequest.onreadystatechange = function () {
                if (httpRequest.readyState == 4 && httpRequest.status == 200) {                                       
                    self.updateData();
                }
            }
            
            httpRequest.send(JSON.stringify(inData));
        }
    })
};
