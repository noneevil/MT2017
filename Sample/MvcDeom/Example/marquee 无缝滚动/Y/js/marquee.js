/**
 * 无缝滚动
 * 
 * @version 2011-3-21 15:55:01
 * @author Zerolone
 */
//运行速度， 唯一需要在JS中调整的数据
marquees_speed=20;
marquees_stop=false;
window.addEvent('domready',function(){
	//获取滚动
	var marquees      = $('marquees');
	
	//获取滚动宽度
	marqueesHeight = marquees.clientHeight;
	
	//声明鼠标移入停止， 移出运行
	marquees.addEvent('mouseenter', function(e){marquees_stop=true});
	marquees.addEvent('mouseleave', function(e){marquees_stop=false});
	
	//复制2遍
	marquees.set('html', marquees.get('html'), marquees.get('html'));
	
	//定时器
	var marquees_timerCount = new Hash({counter: 0});
	var marquees_timer = marquees_timerFunc.periodical(20, marquees_timerCount);
});

var marquees_timerFunc = function(){
	var marquees     = $('marquees');
	var halfmarquees = marquees.getScrollSize().x / 2;
	
	//移动到
	marquees.scrollTo(this.counter * 1, 0);
	
	//如果移入
	if(!marquees_stop) this.counter++;
	
	//如果循环到头
	if(halfmarquees.round() == marquees.getScroll().x) this.counter=0;
	
	//调试信息
	$('aaa').value= halfmarquees;$('bbb').value= marquees.getScroll().x;
}