using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User : Friend
    {
        public static readonly String[] FIELDS = { "uid", "user_name", "nick_name", "password", "space_id", "secques", "gender", 
                                                     "admin_id", "group_id", "group_expiry", "reg_ip", "join_date", "last_ip", 
                                                     "last_visit", "last_activity", "last_post", "last_post_id", "last_post_title", 
                                                     "post_count", "digest_post_count", "online_time", "page_view_count", "credits", 
                                                     "ext_credits_1", "ext_credits_2", "ext_credits_3", "ext_credits_4", 
                                                     "ext_credits_5", "ext_credits_6", "ext_credits_7", "ext_credits_8", "email", 
                                                     "birthday", "tpp", "ppp", "template_id", "pm_sound", "show_email", "invisible", 
                                                     "has_new_pm", "new_pm_count", "access_masks", "online_state", "web_site", "icq", 
                                                     "qq", "yahoo", "msn", "skype", "location", "custom_status", "avatar", 
                                                     "avatar_width", "avatar_height", "medals", "about_me", "sign_html", "real_name", 
                                                     "id_card", "mobile", "telephone" };
        //[XmlElement("user_id", IsNullable=false)]
        //public int Uid;	//用户uid

        [XmlElement("user_name", IsNullable = true)]
        [JsonPropertyAttribute("user_name")]
        public String UserName;	//用户名

        [XmlElement("nick_name", IsNullable = true)]
        [JsonPropertyAttribute("nick_name")]
        public String NickName;	//昵称

        [XmlElement("password", IsNullable = true)]
        [JsonPropertyAttribute("password")]
        public String Password;	//用户密码

        [XmlElement("space_id", IsNullable = true)]
        [JsonPropertyAttribute("space_id")]
        public int? SpaceId; //个人空间ID,0为用户还未申请空间;负数是用户已经申请,等待管理员开通,绝对值为开通以后的真实Spaceid;正数是用户已经开通的Spaceid

        [XmlElement("secques", IsNullable = true)]
        [JsonPropertyAttribute("secques")]
        public String Secques;	//用户安全提问码

        [XmlElement("gender", IsNullable = true)]
        [JsonPropertyAttribute("gender")]
        public int? Gender;	//性别

        [XmlElement("admin_id", IsNullable = true)]
        [JsonPropertyAttribute("admin_id")]
        public int? Adminid;	//管理组ID

        [XmlElement("group_id", IsNullable = true)]
        [JsonPropertyAttribute("group_id")]
        public int? GroupId;	//用户组ID

        [XmlElement("group_expiry", IsNullable = true)]
        [JsonPropertyAttribute("group_expiry")]
        public int? GroupExpiry;	//组过期时间

        [XmlElement("ext_groupids", IsNullable = true)]
        [JsonPropertyAttribute("ext_groupids")]
        public String ExtGroupids;	//扩展用户组

        [XmlElement("reg_ip", IsNullable = true)]
        [JsonPropertyAttribute("reg_ip")]
        public String RegIp;	//注册IP

        [XmlElement("join_date", IsNullable = true)]
        [JsonPropertyAttribute("join_date")]
        public String JoinDate;	//注册时间

        [XmlElement("last_ip", IsNullable = true)]
        [JsonPropertyAttribute("last_ip")]
        public String LastIp;	//上次登录IP

        [XmlElement("last_visit", IsNullable = true)]
        [JsonPropertyAttribute("last_visit")]
        public String LastVisit;	//上次访问时间

        [XmlElement("last_activity", IsNullable = true)]
        [JsonPropertyAttribute("last_activity")]
        public String LastActivity;	//最后活动时间

        [XmlElement("last_post", IsNullable = true)]
        [JsonPropertyAttribute("last_post")]
        public String LastPost;	//最后发贴时间

        [XmlElement("last_post_id", IsNullable = true)]
        [JsonPropertyAttribute("last_post_id")]
        public int? LastPostid;	//最后发贴id

        [XmlElement("last_post_title", IsNullable = true)]
        [JsonPropertyAttribute("last_post_title")]
        public String LastPostTitle;	//最后发贴标题

        [XmlElement("post_count", IsNullable = true)]
        [JsonPropertyAttribute("post_count")]
        public int? Posts;	//发贴数

        [XmlElement("digest_post_count", IsNullable = true)]
        [JsonPropertyAttribute("digest_post_count")]
        public int? DigestPosts;	//精华贴数

        [XmlElement("online_time", IsNullable = true)]
        [JsonPropertyAttribute("online_time")]
        public int? OnlineTime;	//在线时间

        [XmlElement("page_view_count", IsNullable = true)]
        [JsonPropertyAttribute("page_view_count")]
        public int? PageViews;	//页面浏览量

        [XmlElement("credits", IsNullable = true)]
        [JsonPropertyAttribute("credits")]
        public int? Credits;	//积分数

        [XmlElement("ext_credits_1", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_1")]
        public float? ExtCredits1;	//扩展积分1

        [XmlElement("ext_credits_2", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_2")]
        public float? ExtCredits2;	//扩展积分2

        [XmlElement("ext_credits_3", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_3")]
        public float? ExtCredits3;	//扩展积分3

        [XmlElement("ext_credits_4", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_4")]
        public float? ExtCredits4;	//扩展积分4

        [XmlElement("ext_credits_5", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_5")]
        public float? ExtCredits5;	//扩展积分5

        [XmlElement("ext_credits_6", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_6")]
        public float? ExtCredits6;	//扩展积分6

        [XmlElement("ext_credits_7", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_7")]
        public float? ExtCredits7;	//扩展积分7

        [XmlElement("ext_credits_8", IsNullable = true)]
        [JsonPropertyAttribute("ext_credits_8")]
        public float? ExtCredits8;	//扩展积分8

        [XmlIgnore]
        [JsonIgnore]
        public int? AvatarShowId;	//头像ID

        [XmlElement("email", IsNullable = true)]
        [JsonPropertyAttribute("email")]
        public String Email;	//邮件地址

        [XmlElement("birthday", IsNullable = true)]
        [JsonPropertyAttribute("birthday")]
        public String Birthday;	//生日

        [XmlIgnore]
        [JsonIgnore]
        public int? SigStatus;	//签名

        [XmlElement("tpp", IsNullable = true)]
        [JsonPropertyAttribute("tpp")]
        public int? Tpp;	//每页主题数

        [XmlElement("ppp", IsNullable = true)]
        [JsonPropertyAttribute("ppp")]
        public int? Ppp;	//每页贴数

        [XmlElement("template_id", IsNullable = true)]
        [JsonPropertyAttribute("template_id")]
        public int? Templateid;	//风格ID

        [XmlElement("pm_sound", IsNullable = true)]
        [JsonPropertyAttribute("pm_sound")]
        public int? PmSound;	//短消息铃声

        [XmlElement("show_email", IsNullable = true)]
        [JsonPropertyAttribute("show_email")]
        public int? ShowEmail;	//是否显示邮箱

        //[XmlElement("tv")]
        //public ReceivePMSettingType m_newsletter;	//是否接收论坛通知

        [XmlElement("invisible", IsNullable = true)]
        [JsonPropertyAttribute("invisible")]
        public int? Invisible;	//是否隐身
        //private String m_timeoffset;	//时差

        [XmlElement("has_new_pm", IsNullable = true)]
        [JsonPropertyAttribute("has_new_pm")]
        public int? NewPm;	//是否有新消息

        [XmlElement("new_pm_count", IsNullable = true)]
        [JsonPropertyAttribute("new_pm_count")]
        public int? NewPmCount;	//新短消息数量

        [XmlElement("access_masks", IsNullable = true)]
        [JsonPropertyAttribute("access_masks")]
        public int? AccessMasks;	//是否使用特殊权限

        [XmlElement("online_state", IsNullable = true)]
        [JsonPropertyAttribute("online_state")]
        public int? OnlineState;	//在线状态, 1为在线, 0为不在线





        [XmlElement("web_site", IsNullable = true)]
        [JsonPropertyAttribute("web_site")]
        public String WebSite;	//网站

        [XmlElement("icq", IsNullable = true)]
        [JsonPropertyAttribute("icq")]
        public String Icq;	//icq号码

        [XmlElement("qq", IsNullable = true)]
        [JsonPropertyAttribute("qq")]
        public String Qq;	//qq号码

        [XmlElement("yahoo", IsNullable = true)]
        [JsonPropertyAttribute("yahoo")]
        public String Yahoo;	//yahoo messenger帐号

        [XmlElement("msn", IsNullable = true)]
        [JsonPropertyAttribute("msn")]
        public String Msn;	//msn messenger帐号

        [XmlElement("skype", IsNullable = true)]
        [JsonPropertyAttribute("skype")]
        public String Skype;	//skype帐号

        [XmlElement("location", IsNullable = true)]
        [JsonPropertyAttribute("location")]
        public String Location;	//来自

        [XmlElement("custom_status", IsNullable = true)]
        [JsonPropertyAttribute("custom_status")]
        public String CustomStatus;	//自定义头衔

        [XmlElement("avatar", IsNullable = true)]
        [JsonPropertyAttribute("avatar")]
        public String Avatar;	//头像宽度

        [XmlElement("avatar_width", IsNullable = true)]
        [JsonPropertyAttribute("avatar_width")]
        public int? AvatarWidth;	//头像宽度

        [XmlElement("avatar_height", IsNullable = true)]
        [JsonPropertyAttribute("avatar_height")]
        public int? AvatarHeight;	//头像高度

        [XmlElement("medals", IsNullable = true)]
        [JsonPropertyAttribute("medals")]
        public String Medals; //勋章列表

        [XmlElement("about_me", IsNullable = true)]
        [JsonPropertyAttribute("about_me")]
        public String Bio;	//自我介绍

        [XmlIgnore]
        [JsonIgnore]
        public String Signature;	//签名

        [XmlElement("sign_html", IsNullable = true)]
        [JsonPropertyAttribute("sign_html")]
        public String Sightml;	//签名Html(自动转换得到)

        [XmlIgnore]
        [JsonIgnore]
        public String AuthStr;	//验证码

        [XmlIgnore]
        [JsonIgnore]
        public String AuthTime;	//验证码生成日期

        [XmlIgnore]
        [JsonIgnore]
        public byte AuthFlag;	//验证码使用标志(0 未使用,1 用户邮箱验证及用户信息激活, 2 用户密码找回)

        [XmlElement("real_name", IsNullable = true)]
        [JsonPropertyAttribute("real_name")]
        public String RealName;  //用户实名

        [XmlElement("id_card", IsNullable = true)]
        [JsonPropertyAttribute("id_card")]
        public String IdCard;    //用户身份证件号

        [XmlElement("mobile", IsNullable = true)]
        [JsonPropertyAttribute("mobile")]
        public String Mobile;    //用户移动电话

        [XmlElement("telephone", IsNullable = true)]
        [JsonPropertyAttribute("telephone")]
        public String Phone;     //用户固定电话
    }

    public class UserForEditing //: Friend
    {

        [JsonPropertyAttribute("nick_name")]
        public String NickName;	//昵称


        [JsonPropertyAttribute("password")]
        public String Password;	//用户密码


        [JsonPropertyAttribute("space_id")]
        public String SpaceId; //个人空间ID,0为用户还未申请空间;负数是用户已经申请,等待管理员开通,绝对值为开通以后的真实Spaceid;正数是用户已经开通的Spaceid


        //[JsonPropertyAttribute("secues")]
        //public String Secques;	//用户安全提问码


        [JsonPropertyAttribute("gender")]
        public String Gender;	//性别


        [JsonPropertyAttribute("ext_credits_1")]
        public String ExtCredits1;	//扩展积分1


        [JsonPropertyAttribute("ext_credits_2")]
        public String ExtCredits2;	//扩展积分2


        [JsonPropertyAttribute("ext_credits_3")]
        public String ExtCredits3;	//扩展积分3


        [JsonPropertyAttribute("ext_credits_4")]
        public String ExtCredits4;	//扩展积分4


        [JsonPropertyAttribute("ext_credits_5")]
        public String ExtCredits5;	//扩展积分5


        [JsonPropertyAttribute("ext_credits_6")]
        public String ExtCredits6;	//扩展积分6


        [JsonPropertyAttribute("ext_credits_7")]
        public String ExtCredits7;	//扩展积分7


        [JsonPropertyAttribute("ext_credits_8")]
        public String ExtCredits8;	//扩展积分8


        [JsonPropertyAttribute("email")]
        public String Email;	//邮件地址


        [JsonPropertyAttribute("birthday")]
        public String Birthday;	//生日

        //[XmlIgnore]
        //[JsonIgnore]
        //public int SigStatus;	//签名


        //[JsonPropertyAttribute("invisible")]
        //public int Invisible;	//是否隐身
        //private String m_timeoffset;	//时差



        [JsonPropertyAttribute("web_site")]
        public String WebSite;	//网站


        [JsonPropertyAttribute("icq")]
        public String Icq;	//icq号码


        [JsonPropertyAttribute("qq")]
        public String Qq;	//qq号码


        [JsonPropertyAttribute("yahoo")]
        public String Yahoo;	//yahoo messenger帐号


        [JsonPropertyAttribute("msn")]
        public String Msn;	//msn messenger帐号


        [JsonPropertyAttribute("skype")]
        public String Skype;	//skype帐号


        [JsonPropertyAttribute("location")]
        public String Location;	//来自


        [JsonPropertyAttribute("about_me")]
        public String Bio;	//自我介绍


        [JsonPropertyAttribute("real_name")]
        public String RealName;  //用户实名


        [JsonPropertyAttribute("id_card")]
        public String IdCard;    //用户身份证件号


        [JsonPropertyAttribute("mobile")]
        public String Mobile;    //用户移动电话


        [JsonPropertyAttribute("telephone")]
        public String Phone;     //用户固定电话

    }

}
