'use sctrict';

function NewsModel(){
	this.data = { 
		news: []
	};
	
	this.updateData = function(){
		
		var xhr = new XMLHttpRequest();
		
		var self = this;
		
		xhr.onload = function(evt){			
			var news = JSON.parse(evt.target.responseText);
			self.set('news', news.data);
		}
		
		xhr.onerror = function(evt){
			self.set('news', null);
		}
		
		xhr.open('GET', '/Api/News/LoadNews');
		xhr.setRequestHeader("Content-Type", "application/json");
		
		xhr.send();
	}
}	