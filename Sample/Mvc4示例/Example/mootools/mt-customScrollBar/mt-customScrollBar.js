/*
 * 简易自定义滚动条
 * mt-customScrollBar.js by zhangxinxu
 * 2010-04-18 v1.0 创建
 */
 
 var $customScroll = function(container, content, options) {
	window.scrollParams = {
		barTop: 0,
		conTop: 0,
		currentY: 0,
		flag: false	
	};
	
	var defaults = {
		wheel: 4,
		backgroundColor: "cadetblue"
	};
	var params = $extend(defaults, options || {});
	
	if (container && content) {
		//界面初始化
		var hContainer = container.getSize().y, hContent = content.getSize().y, hScale = hContainer / hContent, hBar = hScale * hContainer;
		if (hScale < 1) {
			//如果高度超出
			var eleScrollBg = new Element("div", {
				styles: {
					width: 1,
					height: hContainer,
					backgroundColor: params.backgroundColor,
					position: "absolute",
					right: 2
				}
			}), eleScrollBar = new Element("span", {
				styles: {
					width: 5,
					height: hBar,
					position: "absolute",
					backgroundColor: params.backgroundColor,
					left: -2,
					cursor: "pointer"
				}
			}).store("data", {
				maxTop: hContainer - hBar,
				height: hBar,
				overTop: hContent - hContainer,
				target: content
			}).inject(eleScrollBg);
			
			//容器与滚动条相关联，一边一个页面有多个滚动时可以准确关联
			container.adopt(eleScrollBg).store("targetBar", eleScrollBar);
			
			//事件处理
			//拖动条事件绑定
			eleScrollBar.addEvent("mousedown", function(e) {
				scrollParams.flag = true;
				scrollParams.barTop = this.getStyle("top").toInt() || 0;
				scrollParams.conTop = this.retrieve("data").target.getStyle("top").toInt() || 0;
				scrollParams.currentY = e? e.page.y : 0;
				scrollParams.scrollTarget = this;
				return false;
			}).onselectstart = function() {
				return false;
			};
			
			//滚轮事件
			container.addEvent("mousewheel", function(event) {
				event = new Event(event);
				var eleBar = this.retrieve("targetBar");
				eleBar.fireEvent("mousedown");
				scrollParams.flag = false;
				if (event.wheel > 0) {
					//向上滚
					funScroll(params.wheel);
				} else if (event.wheel < 0) {
					funScroll(-1 * params.wheel);
				}
			});
			
			var funScroll = function(distance) {
				var shouldY = scrollParams.barTop + distance,
					bar = scrollParams.scrollTarget,
					top = bar.getStyle("top").toInt() || 0, 
					barData = bar.retrieve("data");
				//拖动条移动，主体内容滚动
				if (shouldY < 0) {
					shouldY = 0;	
				} else if (shouldY > barData.maxTop) {
					shouldY = barData.maxTop;
				}
				bar.setStyle("top", shouldY);
				barData.target.setStyle("top", barData.overTop * shouldY / barData.maxTop * -1);
			};
			
			var eleBody = $$("body")[0];
			if (!eleBody.retrieve("documentEvent")) {
				document.addEvents({
					mouseup: function() {
						scrollParams.flag = false;
					},
					mousemove: function(e) {
						if(scrollParams.flag){
							var nowY = e.page.y, disY = nowY - scrollParams.currentY;
							funScroll(disY);
						}
					}
				});
				eleBody.store("documentEvent", true);
			}
		}
	}	
};