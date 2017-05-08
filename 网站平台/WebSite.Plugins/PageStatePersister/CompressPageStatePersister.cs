using System;
using System.Web.UI;
using CommonUtils;

namespace WebSite.Plugins
{
    /// <summary>
    /// 压缩方式传递 VIEWSTATE
    /// </summary>
    public class CompressPageStatePersister : PageStatePersister
    {
        //private static LosFormatter _formater = new LosFormatter();
        //private static ObjectStateFormatter _formater = new ObjectStateFormatter();

        private readonly String STATEKEY = "___VIEWSTATE";

        public CompressPageStatePersister(Page page) : base(page) { }

        /// <summary>
        /// 加载页面状态
        /// </summary>
        public override void Load()
        {
            //取得保存在客户端的状态内容
            String postbackstate = Page.Request.Form[STATEKEY];
            if (!String.IsNullOrEmpty(postbackstate))
            {
                //解压，反序列化
                //asp.net的viewstate包括控件状态和视图状态
                //存储两个相关对象
                postbackstate = SevenZipSharpHelper.Decompress(postbackstate);
                ObjectStateFormatter format = new ObjectStateFormatter();

                Pair data = (Pair)format.Deserialize(postbackstate);
                ViewState = data.First;
                ControlState = data.Second;
            }
        }
        /// <summary>
        /// 保存页面状态
        /// </summary>
        public override void Save()
        {
            if (ViewState != null || ControlState != null)
            {
                Pair data = new Pair(ViewState, ControlState);
                //序列化，压缩
                String strbase64 = new ObjectStateFormatter().Serialize(data);
                String strzip = SevenZipSharpHelper.Compress(strbase64);
                //把页面状态注册到页面上
                Page.ClientScript.RegisterHiddenField(STATEKEY, strzip);
            }
        }

    }
}
