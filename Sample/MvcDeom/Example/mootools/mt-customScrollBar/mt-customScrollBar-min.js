var $customScroll=function(b,h,l){window.scrollParams={barTop:0,conTop:0,currentY:0,flag:false};var e={wheel:4,backgroundColor:"cadetblue"};var d=$extend(e,l||{});if(b&&h){var c=b.getSize().y,g=h.getSize().y,m=c/g,f=m*c;if(m<1){var i=new Element("div",{styles:{width:1,height:c,backgroundColor:d.backgroundColor,position:"absolute",right:2}}),k=new Element("span",{styles:{width:5,height:f,position:"absolute",backgroundColor:d.backgroundColor,left:-2,cursor:"pointer"}}).store("data",{maxTop:c-f,height:f,overTop:g-c,target:h}).inject(i);b.adopt(i).store("targetBar",k);k.addEvent("mousedown",function(n){scrollParams.flag=true;scrollParams.barTop=this.getStyle("top").toInt()||0;scrollParams.conTop=this.retrieve("data").target.getStyle("top").toInt()||0;scrollParams.currentY=n?n.page.y:0;scrollParams.scrollTarget=this;return false}).onselectstart=function(){return false};b.addEvent("mousewheel",function(o){o=new Event(o);var n=this.retrieve("targetBar");n.fireEvent("mousedown");scrollParams.flag=false;if(o.wheel>0){j(d.wheel)}else{if(o.wheel<0){j(-1*d.wheel)}}});var j=function(r){var p=scrollParams.barTop+r,o=scrollParams.scrollTarget,q=o.getStyle("top").toInt()||0,n=o.retrieve("data");if(p<0){p=0}else{if(p>n.maxTop){p=n.maxTop}}o.setStyle("top",p);n.target.setStyle("top",n.overTop*p/n.maxTop*-1)};var a=$$("body")[0];if(!a.retrieve("documentEvent")){document.addEvents({mouseup:function(){scrollParams.flag=false},mousemove:function(p){if(scrollParams.flag){var n=p.page.y,o=n-scrollParams.currentY;j(o)}}});a.store("documentEvent",true)}}}};