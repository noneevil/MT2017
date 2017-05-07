using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 分类表
    /// </summary>
    [Serializable]
    [Table("T_Group")]
    public class T_GroupEntities
    {
        public T_GroupEntities()
        {
            this.ID = -1;
            this.Childs = new List<T_GroupEntities>();
        }
        /// <summary>
        /// 自动编号
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增主键
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]//非自增主键
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]//计算列
        [JsonProperty("ID")]
        public long ID { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        [JsonProperty("ParentID")]
        public long ParentID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }
        /// <summary>
        /// 子分类
        /// </summary>
        [ForeignKey("ParentID")]
        public List<T_GroupEntities> Childs { get; set; }
    }
}