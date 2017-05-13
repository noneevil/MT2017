using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Selenium
{
    public partial class Form1 : Form
    {
        private ChromeDriver driver;
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            driver = new ChromeDriver();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                driver.Navigate().GoToUrl("https://www.taobao.com/");

                var link1 = driver.FindElement(By.LinkText("我的淘宝"));
                link1.Click();

                var link2 = driver.FindElement(By.LinkText("密码登录"));
                link2.Click();

                var user = driver.FindElement(By.Name("TPL_username"));
                var pass = driver.FindElement(By.Name("TPL_password"));

                user.SendKeys("杭得住");
                pass.SendKeys("aou2013ff");

                //var scale = driver.FindElement(By.ClassName("nc_scale"));
                //var nc1 = driver.FindElement(By.Id("nc_1_n1z"));
                //new Actions(driver).Click(nc1).MoveByOffset(0, scale.Location.X).Perform();

                //var input = driver.FindElement(By.Name("q"));
                //input.SendKeys("AOU精油");

                //var submit = driver.FindElement(By.CssSelector("button.btn-search"));
                //submit.Click();

                //Console.WriteLine(driver.WindowHandles.Count());
                //var link = driver.FindElement(By.XPath("//a[contains(@href,'21123252400')]"));
                //link.Click();
                //Console.WriteLine(driver.WindowHandles.Count());

                //var screen = driver.GetScreenshot();
                //screen.SaveAsFile(@"z:\a.jpg", ScreenshotImageFormat.Jpeg);

                //Console.WriteLine(driver.Title);
                //var handle = driver.CurrentWindowHandle;
                //var win = driver.WindowHandles.FirstOrDefault(x => x != handle);
                //driver.SwitchTo().Window(win);
                //var add = driver.FindElement(By.XPath("//a[@title='加入购物车']"));
                //add.Click();
                //Console.WriteLine(driver.Title);

                //Thread.Sleep(1000);
                ////var frame = driver.FindElement(By.Id("frameId or iframeId"));
                //var _frame = driver.FindElement(By.XPath("//iframe[contains(@src,'login.taobao.com')]"));
                //driver.SwitchTo().Frame(_frame);
                //var a0 = driver.FindElement(By.Id("J_Quick2Static"));
                //a0.Click();

                //Thread.Sleep(1000);
                //driver.SwitchTo().ParentFrame();
                //var a1 = driver.FindElement(By.ClassName("mnl-close"));
                //a1.Click();

                //Console.WriteLine(driver.Title);
                //var a2 = driver.FindElement(By.LinkText("我的淘宝"));
                //var action = new Actions(driver);
                //action.MoveToElement(a2).Perform();

                //Thread.Sleep(1000);
                //var a3 = driver.FindElement(By.ClassName("ks-imagezoom-wrap"));
                //new Actions(driver).MoveToElement(a3).Perform();
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            driver.Close();
            driver.Quit();
        }
    }
}
