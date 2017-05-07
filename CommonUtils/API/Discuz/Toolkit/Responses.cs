using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{
    [XmlRoot("auth_createToken_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TokenInfo
    {
        [XmlElement("session_key")]
        public String Token;
    }

    /// <summary>
    /// Session信息类
    /// </summary>
    [XmlRoot("auth_getSession_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SessionInfo
    {
        [XmlElement("session_key")]
        public String SessionKey;

        [XmlElement("uid")]
        public long UId;

        [XmlIgnore]
        public String Secret;

        [XmlElement("user_name")]
        public String UserName;

        [XmlElement("expires")]
        public long Expires;

        public SessionInfo()
        { }

        // use this if you want to create a session based on infinite session
        // credentials
        public SessionInfo(String session_key, long uid, String secret, String rest_url)
        {
            this.SessionKey = session_key;
            this.UId = uid;
            this.Secret = secret;
            this.Expires = 0;
        }
    }

    [XmlRoot("auth_register_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class RegisterResponse
    {
        [XmlText]
        public int Uid;

    }

    [XmlRoot("auth_encodePassword_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class EncodePasswordResponse
    {
        [XmlText]
        public String Password;
    }

    [XmlRoot("users_changePassword_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class ChangePasswordResponse
    {
        [XmlText]
        public int Result;
    }


    [XmlRoot("users_getInfo_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class UserInfoResponse
    {
        [XmlElement("user")]
        public User[] user_array;

        [XmlIgnore]
        public User[] Users
        {
            get { return user_array ?? new User[0]; }
        }

        [XmlAttribute("list")]
        public Boolean List;
    }

    [XmlRoot("users_getLoggedInUser_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class LoggedInUserResponse
    {
        [XmlText]
        public int Uid;

        [XmlAttribute("list")]
        public Boolean List;
    }

    [XmlRoot("users_getID_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class GetIDResponse
    {
        [XmlText]
        public int UId;
    }

    public class ArgResponse
    {
        [XmlElement("arg")]
        public Arg[] Args;

        [XmlAttribute("list")]
        public Boolean List;
    }


    [XmlRoot("topics_create_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicCreateResponse
    {
        [JsonPropertyAttribute("topic_id")]
        [XmlElement("topic_id")]
        public int Topicid;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public String Url;

        [JsonPropertyAttribute("need_audit")]
        [XmlElement("need_audit")]
        public Boolean NeedAudit;
    }

    [XmlRoot("users_setInfo_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SetInfoResponse
    {
        [XmlText]
        public int Successfull;
    }

    [XmlRoot("users_setExtCredits_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SetExtCreditsResponse
    {
        [XmlText]
        public int Successfull;
    }

    [XmlRoot("notification_send_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SendNotificationResponse
    {
        [XmlText]
        public String Result;
    }

    [XmlRoot("notification_sendemail_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class SendNotificationEmailResponse
    {
        [XmlText]
        public String Recipients;
    }

    [XmlRoot("notification_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class GetNotiificationResponse
    {
        [JsonPropertyAttribute("message")]
        [XmlElement("message", IsNullable = true)]
        public Notification Message;

        [JsonPropertyAttribute("notification")]
        [XmlElement("notification", IsNullable = true)]
        public Notification Notification;
    }

    [XmlRoot("forums_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class GetForumResponse
    {
        [JsonPropertyAttribute("fid")]
        [XmlElement("fid")]
        public int Fid;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public String Url;

        [JsonPropertyAttribute("topics")]
        [XmlElement("topics")]
        public int Topics;	//主题数

        [JsonPropertyAttribute("current_topics")]
        [XmlElement("current_topics")]
        public int CurTopics;	//主题数

        [JsonPropertyAttribute("posts")]
        [XmlElement("posts")]
        public int Posts;	//帖子数

        [JsonPropertyAttribute("today_posts")]
        [XmlElement("today_posts")]
        public int TodayPosts;	//今日发帖

        [JsonPropertyAttribute("last_post")]
        [XmlElement("last_post")]
        public String LastPost;	//最后发表日期

        [JsonPropertyAttribute("last_poster")]
        [XmlElement("last_poster")]
        public String LastPoster; //最后发表的用户名

        [JsonPropertyAttribute("last_poster_id")]
        [XmlElement("last_poster_id")]
        public int LastPosterId; //最后发表的用户id

        [JsonPropertyAttribute("last_tid")]
        [XmlElement("last_tid")]
        public int LastTid; //最后发表帖子的主题id

        [JsonPropertyAttribute("last_title")]
        [XmlElement("last_title")]
        public String LastTitle; //最后发表的帖子标题

        [JsonPropertyAttribute("description")]
        [XmlElement("description")]
        public String Description;	//论坛描述

        [JsonPropertyAttribute("icon")]
        [XmlElement("icon")]
        public String Icon;	//论坛图标,显示于首页论坛列表等

        [JsonPropertyAttribute("moderators")]
        [XmlElement("moderators")]
        public String Moderators;	//版主列表(仅供显示使用,不记录实际权限)

        [JsonPropertyAttribute("rules")]
        [XmlElement("rules")]
        public String Rules;	//本版规则

        [JsonPropertyAttribute("parent_id")]
        [XmlElement("parent_id")]
        public int ParentId;	//本论坛的上级论坛或分本论坛的上级论坛或分类的fid

        [JsonPropertyAttribute("path_list")]
        [XmlElement("path_list")]
        public String PathList; //论坛级别所处路径的html链接代码

        [JsonPropertyAttribute("parent_id_list")]
        [XmlElement("parent_id_list")]
        public String ParentIdList; //论坛级别所处路径id列表

        [JsonPropertyAttribute("sub_forum_count")]
        [XmlElement("sub_forum_count")]
        public int SubForumCount; //论坛包括的子论坛个数

        [JsonPropertyAttribute("name")]
        [XmlElement("name")]
        public String Name;	//论坛名称

        [JsonPropertyAttribute("status")]
        [XmlElement("status")]
        public int Status;	//是否显示
    }


    [XmlRoot("forums_create_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class CreateForumResponse
    {
        [JsonPropertyAttribute("fid")]
        [XmlElement("fid")]
        public int Fid;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public String Url;
    }

    [XmlRoot("topics_reply_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicReplyResponse
    {
        [JsonPropertyAttribute("post_id")]
        [XmlElement("post_id")]
        public int PostId;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public String Url;

        [JsonPropertyAttribute("need_audit")]
        [XmlElement("need_audit")]
        public Boolean NeedAudit;

    }

    [XmlRoot("topics_getRecentReplies_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicGetRencentRepliesResponse
    {
        [XmlElement("count")]
        public int Count;

        [XmlElement("post")]
        public Post[] post_array;

        [JsonIgnore]
        public Post[] Posts
        {
            get { return post_array ?? new Post[0]; }
        }
        [JsonIgnore]
        [XmlAttribute("list")]
        public Boolean List;
    }

    [XmlRoot("topics_getList_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicGetListResponse
    {
        [XmlElement("count")]
        public int Count;

        [XmlElement("topic")]
        public ForumTopic[] Topics;

        [JsonIgnore]
        [XmlAttribute("list")]
        public Boolean List;
    }

    [XmlRoot("topics_edit_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicEditResponse
    {
        [XmlText]
        public int Result;
    }

    [XmlRoot("topics_delete_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicDeleteResponse
    {
        [XmlText]
        public int Result;
    }

    [XmlRoot("topics_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicGetResponse
    {
        [JsonPropertyAttribute("topic_id")]
        [XmlElement("topic_id")]
        public int TopicId;

        [JsonPropertyAttribute("url")]
        [XmlElement("url")]
        public String Url;

        [JsonPropertyAttribute("title")]
        [XmlElement("title", IsNullable = false)]
        public String Title;

        [JsonPropertyAttribute("fid")]
        [XmlElement("fid", IsNullable = false)]
        public int Fid;

        //[JsonPropertyAttribute("message")]
        //[XmlElement("message", IsNullable = false)]
        //public String Message;

        [JsonPropertyAttribute("icon_id")]
        [XmlElement("icon_id", IsNullable = false)]
        public int Iconid;

        [JsonPropertyAttribute("tags")]
        [XmlElement("tags", IsNullable = true)]
        public String Tags;

        [JsonPropertyAttribute("author")]
        [XmlElement("author")]
        public String Author = String.Empty;

        [JsonPropertyAttribute("author_id")]
        [XmlElement("author_id")]
        public int AuthorId;

        [JsonPropertyAttribute("reply_count")]
        [XmlElement("reply_count")]
        public int ReplyCount;

        [JsonPropertyAttribute("view_count")]
        [XmlElement("view_count")]
        public int ViewCount;

        [JsonPropertyAttribute("last_post_time")]
        [XmlElement("last_post_time")]
        public String LastPostTime = String.Empty;

        [JsonPropertyAttribute("last_poster_id")]
        [XmlElement("last_poster_id")]
        public int LastPosterId;

        [XmlElement("post")]
        [JsonProperty("posts")]
        public Post[] Posts;

        [XmlElement("attachment")]
        [JsonProperty("attachments")]
        public AttachmentInfo[] Attachments;

        [JsonIgnore]
        [XmlAttribute("list")]
        public Boolean List;

        [JsonPropertyAttribute("type_id")]
        [XmlElement("type_id")]
        public int TypeId;

        [JsonPropertyAttribute("type_name")]
        [XmlElement("type_name")]
        public String TypeName;
    }

    [XmlRoot("messages_send_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class MessagesSendResponse
    {
        [XmlText]
        public String Result;
    }

    [XmlRoot("messages_get_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class MessagesGetResponse
    {
        [JsonPropertyAttribute("count")]
        [XmlElement("count")]
        public int count;

        [XmlElement("pm")]
        [JsonProperty("pms")]
        public PrivateMessage[] PM;
    }

    [XmlRoot("topics_deleteReplies_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class TopicDeleteRepliesResponse
    {
        [XmlText]
        public String Result;
    }

    [XmlRoot("forums_getIndexList_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class GetIndexListResponse
    {
        [XmlElement("forum")]
        [JsonProperty("forums")]
        public IndexForum[] Forum;
    }
}