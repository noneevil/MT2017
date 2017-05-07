using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{
	/// <summary>
	/// 附件信息描述类
	/// </summary>
	public class AttachmentInfo
	{
		
		private int m_aid;	//附件aid
		private int m_uid;	//对应的帖子书posterid
		private int m_tid;	//对应的主题tid
		private int m_pid;	//对应的帖子pid
		private String m_postdatetime;	//发布时间
		private int m_readperm;	//所需阅读权限
		private String m_filename;	//存储文件名
		private String m_description;	//描述
		private String m_filetype;	//文件类型
		private long m_filesize;	//文件尺寸
		private String m_attachment;	//附件原始文件名
		private int m_downloads;	//下载次数
        private int m_attachprice;    //附件的售价

		private int m_sys_index;  //非数据库字段,用来代替上传文件所对应上传组件(file)的Index
		private String m_sys_noupload; //非数据库字段,用来存放未被上传的文件名

        private int m_getattachperm; //下载附件权限
        private int m_attachimgpost; //附件是否为图片
        private int m_allowread; //附件是否允许读取
        private String m_preview = String.Empty; //预览信息
        private int m_isbought = 0;//附件是否被买卖
        private int m_inserted = 0; //是否已插入到内容
        /// <summary>
        /// 下载附件权限
        /// </summary>
        [JsonProperty("download_perm")]
        [XmlElement("download_perm")]
        public int Getattachperm
        {
            get { return m_getattachperm; }
            set { m_getattachperm = value; }
        }

        /// <summary>
        /// 附件是否为图片
        /// </summary>
        [JsonProperty("is_image")]
        [XmlElement("is_image")]
        public int Attachimgpost
        {
            get { return m_attachimgpost; }
            set { m_attachimgpost = value; }
        }

        /// <summary>
        /// 附件是否允许读取
        /// </summary>
        [JsonProperty("allow_read")]
        [XmlElement("allow_read")]
        public int Allowread
        {
            get { return m_allowread; }
            set { m_allowread = value; }
        }

        /// <summary>
        /// 预览信息
        /// </summary>
        [JsonProperty("preview")]
        [XmlElement("preview")]
        public String Preview
        {
            get { return m_preview; }
            set { m_preview = value; }
        }

        /// <summary>
        /// 附件是否被买卖
        /// </summary>
        [JsonProperty("is_bought")]
        [XmlElement("is_bought")]
        public int Isbought
        {
            get { return m_isbought; }
            set { m_isbought = value; }
        }


        /// <summary>
        /// 是否已插入到内容
        /// </summary>
        [JsonProperty("inserted")]
        [XmlElement("inserted")]
        public int Inserted
        {
            get { return m_inserted; }
            set { m_inserted = value; }
        }

		///<summary>
		///附件aid
		///</summary>
        [JsonProperty("aid")]
        [XmlElement("aid")]
		public int Aid
		{
			get { return m_aid;}
			set { m_aid = value;}
		}
		///<summary>
		///对应的帖子posterid
		///</summary>
        [JsonProperty("uid")]
        [XmlElement("uid")]
        public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		///<summary>
		///对应的主题tid
		///</summary>
        [JsonProperty("tid")]
        [XmlElement("tid")]
        public int Tid
		{
			get { return m_tid;}
			set { m_tid = value;}
		}
		///<summary>
		///对应的帖子pid
		///</summary>
        [JsonProperty("pid")]
        [XmlElement("pid")]
        public int Pid
		{
			get { return m_pid;}
			set { m_pid = value;}
		}
		///<summary>
		///发布时间
		///</summary>
        [JsonProperty("post_date_time")]
        [XmlElement("post_date_time")]
        public String Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		///<summary>
		///所需阅读权限
		///</summary>
        [JsonProperty("read_perm")]
        [XmlElement("read_perm")]
        public int Readperm
		{
			get { return m_readperm;}
			set { m_readperm = value;}
		}
		///<summary>
		///存储文件名
		///</summary>
        [JsonProperty("file_name")]
        [XmlElement("file_name")]
        public String Filename
		{
			get { return m_filename;}
			set { m_filename = value;}
		}
		///<summary>
		///描述
		///</summary>
        [JsonProperty("description")]
        [XmlElement("description")]
        public String Description
		{
			get { return m_description;}
			set { m_description = value;}
		}
		///<summary>
		///文件类型
		///</summary>
        [JsonProperty("file_type")]
        [XmlElement("file_type")]
        public String Filetype
		{
			get { return m_filetype;}
			set { m_filetype = value;}
		}
		///<summary>
		///文件尺寸
		///</summary>
        [JsonProperty("file_size")]
        [XmlElement("file_size")]
        public long Filesize
		{
			get { return m_filesize;}
			set { m_filesize = value;}
		}
		///<summary>
		///附件原始文件名
		///</summary>
        [JsonProperty("original_file_name")]
        [XmlElement("original_file_name")]
        public String Attachment
		{
			get { return m_attachment;}
			set { m_attachment = value;}
		}
		///<summary>
		///下载次数
		///</summary>
        [JsonProperty("download_count")]
        [XmlElement("download_count")]
        public int Downloads
		{
			get { return m_downloads;}
			set { m_downloads = value;}
		}

        /// <summary>
        /// 附件的售价
        /// </summary>
        [JsonProperty("price")]
        [XmlElement("price")]
        public int Attachprice    
        {
			get { return m_attachprice;}
            set { m_attachprice = value; }
		}
        

		///<summary>
		///非数据库字段,用来代替上传文件所对应上传组件(file)的Index
		///</summary>
        [JsonIgnore]
        [XmlIgnore]
        public int Sys_index
		{
			get { return m_sys_index;}
			set { m_sys_index = value;}
		}

		///<summary>
		///非数据库字段,用来存放未被上传的文件名
		///</summary>
        [JsonIgnore]
        [XmlIgnore]
        public String Sys_noupload
		{
			get { return m_sys_noupload;}
			set { m_sys_noupload = value;}
		}

	}
}
