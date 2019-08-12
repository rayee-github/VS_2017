using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class MyButton
    {
        //定义一个delegate委托  
        public delegate void ClickHandler(object sender);
        //定义事件，类型为上面定义的ClickHandler委托  
        public event ClickHandler OnClick;

        public void Click()
        {
            //...触发之前可能做了n多操作  
            //.....  

            //这时触发Click事件  
            OnClick(this);
        }
    }

    public class Program
    {
        public static void Main()
        {
            MyButton btn = new MyButton();

            //注册事件，把btn_OnClick方法压入事件队列，  
            //可以+=多个，这里简单点就压入一个吧。  
            btn.OnClick += new MyButton.ClickHandler(btn_OnClick);
            btn.Click();
            Console.ReadLine();
        }

        //怎么看到这个函数很熟悉吧，就是你原来双击button自动产生的代码  
        public static void btn_OnClick(object sender)
        {
            Console.WriteLine("！");
        }
    }
}