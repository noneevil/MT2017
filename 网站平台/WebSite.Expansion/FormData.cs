
using System.Collections;
using System.Web.UI;
namespace WebSite.Expansion
{
    public class FormData
    {
        public FormData(Control control)
        {
            this.Data = new Hashtable();
            this.Control = control;
        }

        private Control Control { get; set; }

        public Hashtable Data { get; set; }

        public void GetData()
        {

        }
    }
}