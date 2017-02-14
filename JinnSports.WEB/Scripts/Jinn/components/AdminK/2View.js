'use strict';
function adminView(model) {
    return new View({

        render: function () {
            var cont = $("#adminForm");            
            
            if (cont.length > 0) {                
                cont.remove();
                this.renderConfs();
            }
            else {                
                this.renderConfs();
            }
        },

        init: function () {
            console.log("init");
            this.setupHandlers()
                .enable();
            this.models[0].updateData();
        },
          
        doubleInputPreventer: function(event){
            if (event.target.tagName == 'INPUT') {                
                if (event.target.value != "") {
                    $('#' + event.target.parentElement.parentElement.id + " select")["0"].value = "Existed Name";
                }   
            }
            if (event.target.tagName == 'SELECT') {                
                if (event.target.value != 0) {
                    $('#' + event.target.parentElement.parentElement.id + " input")["0"].value = '';
                }
            }
        },
          
        renderConfs: function () {
            var self = this;
            var data = this.models[0].data.data;
            var cont = $("#adminka");
            
            var fragment = $(document.createDocumentFragment());
            var mainhd = $('<h2></h2>').text("Naming matching panel");
            fragment.append(mainhd);
            var form = $('<form></form>').attr('id', 'adminForm');

            for (var i = 0; i < data.Conformities.length; i++) {
                var div = $('<div></div>')
                    .addClass("form-group")
                    .attr('id', 'form' + i);

                var hd = $('<h4></h4>').text(data.Conformities[i].GroupName);
                div.append(hd);

                var br = $('<br>')
                var label1 = $('<label></label>').text('Custom name:');
                var input = $('<input>')
                    .addClass("form-control")
                    .prop('type', 'text')
                    .prop('value', '')
                    .focusout(this.doubleInputPreventer)
                    .autocomplete({
                        source: function (request, response) {
                            var myExp = new RegExp(request.term, 'i');
                            var output=[];
                            $.each(self.models[0].data.data.Names, function (key, val) {                                
                                if (val.search(myExp) != -1) {                                    
                                    var result = { label: val, value:val };
                                    output.push(result);
                                }
                            })                            
                            response(output);
                        },
                        minLength: 1
                    });

                label1.append(input);
                div.append(label1);
                div.append(br);

                var label2 = $('<label></label>').text('Existed name:');
                var select = $('<select></select>')
                    .addClass("form-control")
                    .focusout(this.doubleInputPreventer);

                var defaultOption = $('<option></option>')
                    .prop('value', "Existed Name")
                    .text("Existed Name")
                    .prop('selected', true);

                select.append(defaultOption);

                for (var j = 0; j < data.Conformities[i].Dtos.length; j++) {
                    var option = $('<option></option>')
                        .prop('value', data.Conformities[i].Dtos[j].ExistedName)
                        .text(data.Conformities[i].Dtos[j].ExistedName);

                    select.append(option);
                }
                label2.append(select.val("Existed Name"));
                div.append(label2);
                form.append(div);                
            }
           
            var btn = $('<button>Save</button>').click(this.save.bind(this));
            form.append(btn);
            fragment.append(form);
            cont.append(fragment);            
        },

        collectData: function () {
            console.log("collect");
            var divs = $('form div');            
            var dataToSend = [];

            for (var i = 0; i < divs.length; i++) {
                var input = $('#' + divs[i].id + ' h4')[0].innerText;
                var existed = $('#' + divs[i].id + ' input')[0].value;
                var select = $('#' + divs[i].id + ' select')[0].value;
                
                if (existed != "") {
                    dataToSend.push({ Id : 0, InputName: input, ExistedName: existed });
                }
                if (select != "Existed Name") {
                    dataToSend.push({ Id: 0, InputName: input, ExistedName: select });
                }  
            }
                        
            return dataToSend;
        },

        save: function (e) {
            e.preventDefault();            
            var data = this.collectData();            
            this.models[0].send(data);
        }
    }, model)
};
