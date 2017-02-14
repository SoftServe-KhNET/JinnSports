'use strict';
function View(model) {
    this.model = model    
}

View.prototype = {
    init: function (context) {        
        //console.log(this);
        var self = this;
        this.model.updateData('https://localhost:44300/api/Conformity', self.render(context.model.getData()));
        
        //self.render(context.model.getData());
        
       // console.log("need");
        //this.render(context.model.getData());
    },    

    render: function (data) {
        console.log(data);
        
        var cont = $(".start");

        var fragment = $(document.createDocumentFragment());  

        for (var i = 0; i < data.length; i++) {
            var form = $('<form></form>');
            var hd = $('<h4></h4>').text(data[i].GroupName);
            form.append(hd);
            var label = $('<label></label>').text('Put your variant...');
            var input = $('<input>').prop('type', 'text');
            label.append(input);
            form.append(label);

            var select = $('<select></select>');
            for (var j = 0; j < data[i].Dtos.length; j++) {                
                var option = $('<option></option>').prop('value', data[i].Dtos[j].ExistedName);
                select.append(option);
            }
            form.append(select);
            fragment.append(form);
        }
        cont.append(fragment);
    }
}