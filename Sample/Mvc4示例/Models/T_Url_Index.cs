using MSSQLDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4Example.Models
{
    [Table("T_Url_Index")]
    [Serializable]
    public class T_Url_IndexEntity
    {
        /// <summary>
        /// Url
        /// </summary>
        [Field("Url")]
        public string Url { get; set; }
        /// <summary>
        /// XML
        /// </summary>
        [Field("XML")]
        public string XML { get; set; }
        /// <summary>
        /// del
        /// </summary>
        [Field("del")]
        public int del { get; set; }
    }
}