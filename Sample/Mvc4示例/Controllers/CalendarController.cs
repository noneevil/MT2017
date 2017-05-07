using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Mvc4Example.Controllers
{
    public class CalendarController : Controller
    {
        /// <summary>
        /// 日历控件
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 日历控件
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Calendar(DateTime? dateTime)
        {
            dateTime = dateTime ?? DateTime.Now;//.Date.AddDays(-DateTime.Now.Day + 1);
            dateTime = dateTime.Value.Date.AddDays(-dateTime.Value.Day + 1);

            //var visibledate = dateTime;

            #region 日历控件

            var calendar = new Calendar()
            {
                CssClass = "calendar",
                Width = 350,
                Height = 200,
                ShowTitle = false,
                ShowGridLines = false,
                BorderWidth = 0,
                CellSpacing = 0,
                CellPadding = 0,
                ShowNextPrevMonth = false,
                FirstDayOfWeek = FirstDayOfWeek.Default,
                DayNameFormat = DayNameFormat.Shortest,
                VisibleDate = dateTime.Value
            };
            calendar.TodayDayStyle.CssClass = "today";

            calendar.DayRender += (sender, e) =>
            {
                e.Cell.Controls.Clear();
                e.Cell.HorizontalAlign = HorizontalAlign.Left;
                var span = new HtmlGenericControl("span");
                e.Cell.Controls.Add(span);
                span.InnerText = e.Day.Date.ToString("dd");

                if (e.Day.IsOtherMonth)
                {
                    //span.Visible = false;
                }
                else if (e.Day.Date.Date > DateTime.Now.Date)
                {
                    if (!e.Day.IsToday)
                    {
                        e.Cell.CssClass = "on";
                        e.Cell.Style.Add("cursor", "pointer");
                    }

                    var label = new HtmlGenericControl("label");
                    e.Cell.Controls.Add(label);
                    label.InnerText = "￥99.00";

                }
            };

            #endregion

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());
            calendar.RenderControl(writer);

            ViewData["Calendar"] = writer.InnerWriter.ToString();

            return PartialView("Calendar", dateTime);
        }
    }
}
