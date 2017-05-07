/**
 * 轮显
 * 
 * @version 2009-8-26 16:02:54
 * @author Zerolone
 */
window.addEvent('domready',function(){
	//初始化小图
	var cycle=new Array();
	for(var i=0;i<4;i++){
		cycle[i]=$('cycle_img'+i);
		cycle[i].src=Switcher[i]['pic'];
	}

	//大图
	var bigpic=$('bigpic');
	//初始化大图
	bigpic.src=Switcher[0]['pic'];

	//循环li
	var li=new Array();
	for(var i=0;i<4;i++){
		li[i]=$('li'+i);		
	}

	//增加鼠标移入事件
	li[0].addEvent('mouseenter', function(e){lienter(0);});
	li[1].addEvent('mouseenter', function(e){lienter(1);});
	li[2].addEvent('mouseenter', function(e){lienter(2);});
	li[3].addEvent('mouseenter', function(e){lienter(3);});

	//增加鼠标移出事件
	li[0].addEvent('mouseleave', function(e){cycleplay();});
	li[1].addEvent('mouseleave', function(e){cycleplay();});
	li[2].addEvent('mouseleave', function(e){cycleplay();});
	li[3].addEvent('mouseleave', function(e){cycleplay();});
	
	//添加鼠标点击事件
	li[0].addEvent('click', function(e){window.open(Switcher[0]['url']);});
	
	//整体
	$('cycle').addEvent('mouseenter', function(e){cyclestop();});	
	$('cycle').addEvent('mouseleave', function(e){cycleplay();});

	//大图
	$('bigpic').addEvent('mouseenter', function(e){cyclestop();});	
	$('bigpic').addEvent('mouseleave', function(e){cycleplay();});
	
	//背景
	$('bg').addEvent('mouseenter', function(e){cyclestop();});	
	$('bg').addEvent('mouseleave', function(e){cycleplay();});	

	//文字
	$('SwitchTitle').addEvent('mouseenter', function(e){cyclestop();});	
	$('SwitchTitle').addEvent('mouseleave', function(e){cycleplay();});	

	//默认
	var slidint;
	Switchid=0;

	cycleplay();
	lienter(0);
	cycleplay();
});

/**
 * 移入换背景
 */
function lienter(id){
	//隐藏所有
	liclear();
	
	//停止滚动
	cyclestop();	
	
	//选定背景
	$('li'+id).setStyle('background-image', 'url(images/bg.gif)');
	
	//文字
	var str_title = '' ;
	str_title = '<h3><a href="'+Switcher[id]['url']+'" target="_blank">'+Switcher[id]['title']+'</a></h3><p>'+Switcher[id]['stitle']+'</p>' ;
	$('SwitchTitle').set('html',str_title);
	
	//大图链接
	$('bighref').href=Switcher[id]['url'];
//	$('bigpic').addEvent('click', function(e){window.open(Switcher[id]['url']);});
//	$('bigpic').removeEvent('click');
	
	//大图
	//$('bigpic').src=Switcher[id]['pic'];
	
	//大图效果
	var bigpic = $('bigpic');
	var fx = new Fx.Tween(bigpic,{
		duration: 50,
		onComplete: function(){
			bigpic.src=Switcher[id]['pic'];
			bigpic.fade('in');
		}
	});
	fx.start('opacity',1,0);
}

/**
 * 清除背景
 */
function liclear(){
	for(var i=0;i<4;i++){
		$('li'+i).setStyle('background-image', '');
	}	
}

/**
 * 循环，三秒
 */
function cycleplay(){
	slidint = setTimeout(playnext,1000);
}

/**
 * 显示下一个
 */
function playnext(){
	if(Switchid==3){
		Switchid = 0;
	}
	else{
		Switchid++;
	};
	
	lienter(Switchid);
	cycleplay();
}

/**
 * 停止
 */
function cyclestop(){
	clearTimeout(slidint);
}