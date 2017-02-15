'use sctrict';

function NewsView(){
	var newsComponent;
	var newsBody;
	var newsBox;
	
	this.render = function(){
		this.destroyNewsBox();
		this.buildNewsBox();		
	}
	 
	
	this.init = function(){
		
		this.newsComponent = document.getElementById('component-news');	
		this.newsComponent.className = 'col-md-3';
		this.newsBody = document.createElement('div');
		this.newsBody.className = 'box box-warning';
		this.newsComponent.appendChild(this.newsBody);
		
		var newsTitle = document.createElement('div');
		newsTitle.className = 'box-header with-border';
		var titleElement = document.createElement('h3');
		titleElement.className = 'box-title';
		titleElement.innerHTML = 'Новости';
		newsTitle.appendChild(titleElement);
		
		this.newsBody.appendChild(newsTitle);
		
		this.newsBox = document.createElement('div');
		this.newsBox.className = 'box-body';
		this.newsBody.appendChild(this.newsBox);
		
		return this;
	}
	
	this.show = function(){		
		this.newsComponent.style.display = 'block';
		
		this.models[0].updateData();
	}
	
	this.hide = function(){		
		this.newsComponent.style.display = 'none';
	}
	
	this.buildNewsBox = function(){
		var newsData = this.models[0].get('news');
		if (newsData == null || newsData.length == 0) {
			this.newsBox.appendChild(document.createTextNode('Нет новостей'));
			this.newsComponent.appendChild(this.newsBox);
		}
		else {
			for(var i = 0; i < newsData.length; i++){
				
				var aLink = document.createElement('a');
				aLink.setAttribute('href', newsData[i].Link);			
				
				var pTitle = document.createElement('p');
				pTitle.innerHTML = newsData[i].Title;
				
				aLink.appendChild(pTitle);
				
				var pDescription = document.createElement('p');
				pDescription.innerHTML = newsData[i].Description;
				
				var spanTime = document.createElement('span');
				spanTime.className = 'time pull-right';
				
				var iElement = document.createElement('i');
				iElement.className = 'fa fa-clock-o';
				
				spanTime.appendChild(iElement);
				
				spanTime.appendChild(document.createTextNode(newsData[i].Time));
				
				pDescription.appendChild(spanTime);
				
				this.newsBox.appendChild(aLink);
				this.newsBox.appendChild(pDescription);
				this.newsBox.appendChild(document.createElement('hr'));			
			}
		}
	}
	
	this.destroyNewsBox = function(){
		console.log(this.newsBox);
		while(this.newsBox.firstChild != null){
			this.newsBox.removeChild(this.newsBox.firstChild);
		}			
	}
}	