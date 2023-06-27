using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    class IndexViewModel : NavigationViewModel
    {
        #region single field
        private readonly IRegionManager regionManager;
        private readonly SharedData sharedate;
        private readonly IDialogHostService dialog;
        private readonly IDialogService prismDialogService;
        private readonly IToDoService todoService;
        private readonly IMemoService memoService; 
        private IRegionNavigationJournal journal;
        #endregion
        public IndexViewModel(SharedData sharedate, IContainerProvider containerProvider, IDialogHostService dialog, IDialogService prismDialogService) : base(containerProvider)
        {
            CreateTaskBars();
            //CreateTestData();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.sharedate = sharedate;
            this.journal = sharedate.journal;
            //sharedate.ToDoListUpdated += (sender, args) =>
            //{

            //};
            this.dialog = dialog;

            this.prismDialogService = prismDialogService; 
            regionManager = containerProvider.Resolve<IRegionManager>();
            todoService = containerProvider.Resolve<IToDoService>();
            memoService = containerProvider.Resolve<IMemoService>();
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            ToDoCompletedCommand = new DelegateCommand<ToDoDto>(Completed);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            //InitData();
        }
         
        private void Navigate(TaskBar obj)
        {
            if (string.IsNullOrEmpty(obj.Target)) return;
            NavigationParameters param = new NavigationParameters();
            if (obj.Title == "已完成")
            {
                param.Add("Value", 2);
            }
            if (obj.Title == "汇总")
            {
                param.Add("Value", 0);
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target,  callback =>
            {
                journal = callback.Context.NavigationService.Journal;
            },param);


        }

        private async void Completed(ToDoDto obj)
        {
            if (obj.Status == true)
            {
                var updateRes = await todoService.UpdateAsync(obj);
                if (updateRes?.Code == 200)
                {
                    ToDoDtos.Remove(obj);
                    CalTaskBarsData();
                    base.eventAggregator.SendMessage("已完成!");
                }
                else
                {
                    obj.Status = false;
                } 
            }
        }
        #region prop与propfull
        public DelegateCommand<string> ExecuteCommand { get; set; }
        public DelegateCommand<ToDoDto> EditToDoCommand { get; set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; set; }
        public DelegateCommand<ToDoDto> ToDoCompletedCommand { get; set; }
        public DelegateCommand<TaskBar> NavigateCommand { get; set; }
        public ObservableCollection<TaskBar> TaskBars { get; set; } = new ObservableCollection<TaskBar>();
        // 原始dtos
        public ObservableCollection<ToDoDto> RealToDoDtos { get; set; } = new ObservableCollection<ToDoDto>();
        // 过滤后的dto
        private ObservableCollection<ToDoDto> toDoDtos = new ObservableCollection<ToDoDto>();
        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
        // 过滤后的dto
        private ObservableCollection<MemoDto> memoDtos { get; set; } = new ObservableCollection<MemoDto>();
        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        private int test = 0;

        public int Test
        {
            get { return test; }
            set { test = value; RaisePropertyChanged(); }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }


        #endregion
        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办":
                    AddToDo(null);
                    break;
                case "新增备忘录":
                    AddMemo(null);
                    break;
            }
        }

        /// <summary>
        /// 添加待办
        /// </summary>
        private async void AddToDo(ToDoDto? toDoDto)
        {
            //使用自定义dialogHostService弹出 dialog
            var param = new DialogParameters();
            if (toDoDto != null)
            {
                param.Add("Value", toDoDto);
            }
            var dialogResult = await dialog.ShowDialog("AddToDoView", param);
            if (dialogResult?.Result == ButtonResult.OK)
            {
                var editedTodo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                if (editedTodo.Id > 0)
                {
                    var updateRes = await todoService.UpdateAsync(editedTodo);
                    if (updateRes?.Code == 200)
                    {
                        // 可以直接更新model
                        toDoDto = updateRes.Content;
                        if (toDoDto.Status == true)
                            ToDoDtos.Remove(ToDoDtos.FirstOrDefault(x => x.Id == toDoDto.Id)!);
                        base.eventAggregator.SendMessage("已完成!");
                    }
                }
                else
                {// 新增
                    var addResult = await todoService.AddAsync(editedTodo);
                    if (addResult?.Code == 200)
                    {
                        RealToDoDtos.Add(addResult.Content);
                        if (addResult.Content.Status == false)
                        {
                            ToDoDtos.Add(addResult.Content);
                        }
                    }
                }
                CalTaskBarsData();
            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        private async void AddMemo(MemoDto? memoDto)
        {
            //使用自定义dialogHostService弹出 dialog
            var param = new DialogParameters();
            if (memoDto != null)
            {
                param.Add("Value", memoDto);
            }
            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
            if (dialogResult?.Result == ButtonResult.OK)
            {
                var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");
                if (memo.Id > 0)
                {
                    var updateRes = await memoService.UpdateAsync(memo);
                    if (updateRes?.Code == 200)
                    {
                        // 可以直接更新model
                        memo = updateRes.Content;
                    }
                }
                else
                {// 新增
                    var addResult = await memoService.AddAsync(memo);
                    if (addResult?.Code == 200)
                    {
                        MemoDtos.Add(addResult.Content);
                    }
                }
                CalTaskBarsData();
            }
            //prism自带dialog
            //prismDialogService.ShowDialog("AddMemoView");
        }

        void CreateTaskBars()
        {
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Content = "0", Color = "#FF0CA0FF", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Content = "0", Color = "#FF1ECA3A", Target = "ToDoView" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Content = "100%", Color = "#FF02C6DC", Target = "" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Content = "0", Color = "#FFFFA000", Target = "MemoView" });
        }
        void CreateTestData()
        {
            ToDoDtos.Add(new ToDoDto() { Title = "待办1", Content = "sfasd", Status = true });
            ToDoDtos.Add(new ToDoDto() { Title = "待办2", Content = "qwt25t", Status = false });
            ToDoDtos.Add(new ToDoDto() { Title = "待办3", Content = "rhrt234", Status = false });
            ToDoDtos.Add(new ToDoDto() { Title = "待办4", Content = "ffgjhrt", Status = true });

            MemoDtos.Add(new MemoDto() { Title = "备忘录1", Content = "sfasd" });
            MemoDtos.Add(new MemoDto() { Title = "备忘录2", Content = "sfasd" });
            MemoDtos.Add(new MemoDto() { Title = "备忘录3", Content = "sfasd" });
            MemoDtos.Add(new MemoDto() { Title = "备忘录4", Content = "sfasd" });
        }

        async void InitData()
        {
            if (sharedate.IsToDoDtosInited == false)
            {
                var res1 = await todoService.GetAllAsync();
                if (res1?.Code == 200)
                {
                    RealToDoDtos.AddRange(res1.Content);
                    ToDoDtos.AddRange(RealToDoDtos.Where(x => x.Status == false));
                    sharedate.IsToDoDtosInited = true;
                    //CalTaskBarsData();
                }
            }
            if (sharedate.IsMemoDtosInited == false)
            {
                var res2 = await memoService.GetAllAsync();
                if (res2?.Code == 200)
                {
                    MemoDtos.AddRange(res2.Content);
                    sharedate.IsMemoDtosInited = true;
                    //CalTaskBarsData();
                }
            }
            CalTaskBarsData();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 每次导航需要初始化的vm
            Title = $"你好,{AppSession.UserName} {DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
            // 将页面的数据绑定到共享数据
            RealToDoDtos = sharedate.RealToDoDtos;
            ToDoDtos.Clear();
            ToDoDtos.AddRange(RealToDoDtos.Where(x => x.Status == false));
            MemoDtos = sharedate.RealMemoDtos;
            InitData();
            //base.eventAggregator.SendMenubarUpdateMessage("首页", "IndexView");
            base.OnNavigatedTo(navigationContext);

        }
        private void CalTaskBarsData()
        {
            var TodoNum = RealToDoDtos.Count;
            var CompletedTodoNum = RealToDoDtos.Count(x => x.Status == true);
            // 全部待办
            TaskBars[0].Content = TodoNum.ToString();
            TaskBars[1].Content = CompletedTodoNum.ToString();
            TaskBars[2].Content = (CompletedTodoNum / (double)TodoNum).ToString("0%");
            TaskBars[3].Content = MemoDtos.Count.ToString();
        }
    }
}
