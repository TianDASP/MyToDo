using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Design.Behavior;

namespace MyToDo.Extensions
{
    /// <summary>
    /// 为PasswordBox控件创建依赖属性
    /// 0: 在PasswordBehavior中定义PasswordBox的密码变化事件
    /// 1: 输入框输入 PasswordBox的Password属性(非依赖)变化,触发事件
    /// 2: 事件中将Password属性赋值给自定义的依赖属性
    /// 3: 自定义依赖属性将值同步到ViewModel里的Password属性
    /// 实现 密码框控件自带Password属性自动同步到ViewModel中,而不是在分部类中使用控件名称获取Password
    /// 这样可以将登录逻辑(Command)放在ViewModel中,而不是使用Event在分部类中登录
    /// </summary>
    public class PassWordExtension  
    {
        public static string GetPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(PwdProperty);
        }

        public static void SetPwd(DependencyObject obj, string value)
        {
            obj.SetValue(PwdProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PwdProperty =
            DependencyProperty.RegisterAttached("Pwd", typeof(string), typeof(PassWordExtension), new PropertyMetadata(string.Empty, OnPwdPropertyChanged));
         
        /// <summary>
        /// 自定义依赖属性改变后触发,一般不需要
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPwdPropertyChanged(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
             
        }
         
    }

    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            // 获取自定义依赖属性的值
            string password = PassWordExtension.GetPwd(passwordBox);
            if (passwordBox != null && passwordBox.Password != password)
            {
                PassWordExtension.SetPwd(passwordBox, passwordBox.Password);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}
