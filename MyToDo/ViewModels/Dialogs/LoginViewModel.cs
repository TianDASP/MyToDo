using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public LoginViewModel(ILoginService loginService,IEventAggregator aggregator)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.loginService = loginService;
            this.aggregator = aggregator;
        }

        private void Execute(string arg)
        {
            switch (arg)
            {
                case "Login": Login(); break;

                case "Logout": Logout(); break;
                //跳转到注册页面
                case "GoToRegister": SelectedIndex = 1; break;
                //注册账号
                case "Register": Register(); break;
                //返回登录页面
                case "Return": SelectedIndex = 0; break;
            }
        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(RegisterRequest.Account) || string.IsNullOrEmpty(RegisterRequest.UserName) ||
                string.IsNullOrEmpty(Password1) || string.IsNullOrEmpty(Password2))
            {
                return;
            }
            if (Password1 != Password2)
            {
                aggregator.SendMessage("密码不一致","login");
                return;
            }
            registerRequest.Password = Password1;
            var res = await loginService.RegisterAsync(RegisterRequest);
            if (res?.Code == 200)
            {// 注册成功 跳转登录
                aggregator.SendMessage("注册成功", "login");
                SelectedIndex = 0; 
            }
            else
            {
                aggregator.SendMessage("注册失败", "login");
            }
            //注册失败提示...
            // 这里将注册信息清空
            RegisterRequest.Account = "";
            RegisterRequest.Password = "";
            Password1 = "";
            Password2 = "";
        }

        private void Logout()
        {

        }
            
        private async void Login()
        {
            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
                return;
            var res = await loginService.LoginAsync(new Common.Models.UserDto() { Account = account, Password = password });
            if (res?.Code == 200)
            {
                aggregator.SendMessage("登录成功", "login");
                // 获取res里的 jwt和 User相关信息
                var x = res.Content;
                AppSession.Id = res.Content.Id;
                AppSession.Account = res.Content.Account;
                AppSession.UserName = res.Content.UserName;
                AppSession.Token = res.Msg;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                aggregator.SendMessage("登录失败", "login");
            }
            // 登录失败可以提示
            // 目前将密码清除
            Account = "";
            Password = "";
        }

        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand<string> ExecuteCommand { get; set; }

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }

        private string password;
        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }
        private int selectedIndex = 0;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        private RegisterRequest registerRequest = new RegisterRequest();

        public RegisterRequest RegisterRequest
        {
            get { return registerRequest; }
            set { registerRequest = value; RaisePropertyChanged(); }
        }

        private string password1;

        public string Password1
        {
            get { return password1; }
            set { password1 = value; RaisePropertyChanged(); }
        }

        private string password2;

        public string Password2
        {
            get { return password2; }
            set { password2 = value; RaisePropertyChanged(); }
        }


        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            // 手动关闭登录框,退出app
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
