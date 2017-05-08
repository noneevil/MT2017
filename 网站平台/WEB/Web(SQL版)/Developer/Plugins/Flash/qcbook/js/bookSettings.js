// 电子书图片
flippingBook.pages = [
	"pages/qc001.jpg",
	"pages/qc002.jpg",
	"pages/qc003.jpg",
	"pages/qc004.jpg",
	"pages/qc005.jpg",
	"pages/qc006.jpg",
	"pages/qc007.jpg",
	"pages/qc008.jpg",
	"pages/qc009.jpg",
	"pages/qc010.jpg",
	"pages/qc011.jpg",
	"pages/qc012.jpg",
	"pages/qc013.jpg",
	"pages/qc014.jpg",
	"pages/qc015.jpg",
	"pages/qc016.jpg",
	"pages/qc017.jpg",
	"pages/qc018.jpg",
	"pages/qc019.jpg",
	"pages/qc020.jpg",
	"pages/qc021.jpg",
	"pages/qc022.jpg",
	"pages/qc023.jpg",
	"pages/qc024.jpg",
	"pages/qc025.jpg",
	"pages/qc026.jpg"
];

// 电子书目录页面
flippingBook.contents = [
    [ "封面", 1 ],
	[ "第1页", 1 ],	
	[ "第2页", 5 ],
	[ "封底", 30 ]
];

// 电子书设置
flippingBook.settings.bookWidth = 1096;  //宽度
flippingBook.settings.bookHeight = 588;  //高度
flippingBook.settings.zoomImageWidth = 1306;  //放大后的宽度
flippingBook.settings.zoomImageHeight = 1400; //放大高的度
flippingBook.settings.downloadURL = "pdf/qc.pdf";
flippingBook.settings.zoomPath = "large/";
flippingBook.settings.downloadSize = "Size: 400.7 Mb",
flippingBook.settings.zoomHint = "双击放大图片";

flippingBook.settings.pageBackgroundColor = 0xffffff;
flippingBook.settings.backgroundColor = 0xe4e4e4;
flippingBook.settings.zoomUIColor = 0xc50500;
flippingBook.settings.useCustomCursors = false;
flippingBook.settings.dropShadowEnabled = false,
flippingBook.settings.flipSound = "sounds/02.mp3";
flippingBook.settings.flipCornerStyle = "first page only";
flippingBook.settings.zoomHintEnabled = true;
// default settings can be found in the flippingbook.js file
flippingBook.create();
