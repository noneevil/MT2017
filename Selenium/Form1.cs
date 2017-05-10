using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            driver.Navigate().GoToUrl("https://world.taobao.com/");
            var input = driver.FindElement(By.Name("q"));
            input.SendKeys("AOU精油");

            var submit = driver.FindElement(By.CssSelector("button.btn-search"));
            submit.Click();

            Console.WriteLine(driver.WindowHandles.Count());
            var link = driver.FindElement(By.XPath("//a[contains(@href,'21123252400.htm')]"));
            link.Click();
            Console.WriteLine(driver.WindowHandles.Count());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            driver.Close();
            driver.Quit();
        }
    }
}
