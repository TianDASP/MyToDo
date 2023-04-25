using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Extensions;

namespace MyToDo.ViewModels
{
    class ToDoViewModel : NavigationViewModel
    {
        //public ToDoViewModel( )
        //{
        //    init();
        //    // 弹出添加待办窗口
        //    AddCommand = new DelegateCommand(() =>
        //    {
        //        IsRightDrawerOpen = !IsRightDrawerOpen;
        //    }); 
        //}
        public ToDoViewModel(SharedData sharedate, IToDoService toDoService, IContainerProvider containerProvider) : base(containerProvider)
        {
            this.sharedate = sharedate;
            this.service = toDoService;
            // 弹出添加待办窗口
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            //GetDataListAsync();
        }

        // 用于执行Action的command
        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增":
                    Add();
                    break;
                case "查询":
                    Query();
                    break;
                case "保存":
                    PostAndSave(CurrentSelectedDto);
                    break;
                case "刷新":
                    Refrash(); break;
            }
        }

        private async void Refrash()
        {
            RealToDoDtos.Clear();
            ToDoDtos.Clear();
            // 这里必须await,不然会先过滤
            await GetDataListAsync(); 
            Query();
        }

        private async void Delete(ToDoDto dtoToDelete)
        {
            var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项:{dtoToDelete.Title}");
            if(dialogResult?.Result != Prism.Services.Dialogs.ButtonResult.OK)
            {
                return;
            }
            var apiResponse = await service.DeleteAsync(dtoToDelete.Id);
            if (apiResponse?.Code == 200)
            {// 删除成功
                ToDoDtos.Remove(dtoToDelete);
            }
        }

        private async void PostAndSave(ToDoDto toDoDto)
        {
            if (string.IsNullOrEmpty(toDoDto.Title) || string.IsNullOrEmpty(toDoDto.Content)) { return; }

            ApiResponse<ToDoDto>? apiResponse;
            // 如果id为默认值,则 todo为新增的
            if (toDoDto.Id == 0)
            {
                apiResponse = await service.AddAsync(toDoDto);
                if (apiResponse?.Code == 200)
                {
                    RealToDoDtos.Add(apiResponse.Content);
                    var status = apiResponse.Content.Status ? 2 : 1;
                    if (!string.IsNullOrEmpty(Search))
                    { 
                        if ((apiResponse.Content.Title.Contains(Search.Trim()) || apiResponse.Content.Content.Contains(Search.Trim()))
                        && (status == SelectedStatus || SelectedStatus == 0))
                        {
                            ToDoDtos.Add(apiResponse.Content);
                        }
                    }else
                    {
                        ToDoDtos.Add(apiResponse.Content);
                    }
                }
            }
            else
            {
                apiResponse = await service.UpdateAsync(toDoDto);
                if (apiResponse?.Code == 200)
                {
                    toDoDto = apiResponse.Content;
                }
            }
            // 关闭右侧窗口
            IsRightDrawerOpen = false;
        }

        private void Query()
        {
            // 先过滤状态
            var filterFirst = SelectedStatus switch
            {
                0 => RealToDoDtos,
                1 => RealToDoDtos.Where(x => x.Status == false),
                2 => RealToDoDtos.Where(x => x.Status == true),
                _ => RealToDoDtos,
            };
            if (!string.IsNullOrEmpty(Search))
            {
                // 可以本地过滤显示
                var filterList = filterFirst.Where(x => x.Title.Contains(Search.Trim()) || x.Content.Contains(Search.Trim()));
                ToDoDtos = new ObservableCollection<ToDoDto>(filterList);
            }
            else
            {
                ToDoDtos = new ObservableCollection<ToDoDto>(filterFirst);
            }
        }

        private void Add()
        {
            CurrentSelectedDto = new ToDoDto();
            IsRightDrawerOpen = !IsRightDrawerOpen;
        }

        private readonly IDialogHostService dialogHost;
        /// <summary>
        /// 右侧窗口是否展开
        /// </summary>
        private bool isRightDrawerOpen = false;
        private readonly SharedData sharedate;
        private readonly IToDoService service;
        //  右侧编辑栏里面绑定的dto
        private ToDoDto currentSelectedDto;
        public DelegateCommand<string> ExecuteCommand { get; set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; set; }

        public DelegateCommand<ToDoDto> DeleteCommand { get; set; }
        private string search = "";
        private int selectedStatus = 0;

        public int SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                selectedStatus = value; RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                RaisePropertyChanged();
                if (string.IsNullOrEmpty(search))
                {
                    Query();
                }
            }
        }

        // 编辑选中对象/新增对象
        public ToDoDto CurrentSelectedDto
        {
            get { return currentSelectedDto; }
            set { SetProperty(ref currentSelectedDto, value); }
        }
        public bool IsRightDrawerOpen
        {
            get => isRightDrawerOpen;
            set => SetProperty(ref isRightDrawerOpen, value);
        }
        // 原始dtos
        public ObservableCollection<ToDoDto> RealToDoDtos { get; set; } = new ObservableCollection<ToDoDto>();
        // 过滤后的dto
        private ObservableCollection<ToDoDto> toDoDtos = new ObservableCollection<ToDoDto>();
        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
         

        private  void Selected(ToDoDto obj)
        {
            try
            {
                // 先开启加载动画
                UpdateLoading(true);
                // 可以再次查询详情 ,也可以直接给CurrentSelectedDto赋值
                //var apiResponse = await service.GetFirstOrDefaultAsync(obj.Id);
                //if (apiResponse?.Code == 200)
                //{
                //    CurrentSelectedDto = apiResponse.Content;
                //    IsRightDrawerOpen = true;
                //} 
                CurrentSelectedDto = obj;
                IsRightDrawerOpen = true;
            }
            catch (Exception ex)
            {

            }
            finally { UpdateLoading(false); }
        }
        //获取数据
        private async Task GetDataListAsync()
        {
            // 打开等待窗口
            UpdateLoading(true);
            var res = await service.GetAllAsync();
            if (res == null)
            {
                return;
            }
            RealToDoDtos.AddRange(res.Content); 
            // 关闭等待窗口
            UpdateLoading(false);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.RealToDoDtos = sharedate.RealToDoDtos;
            ToDoDtos = RealToDoDtos;
            if (!sharedate.IsToDoDtosInited)
            {
                await GetDataListAsync();
            }
            if (navigationContext.Parameters.ContainsKey("Value"))
            {
                SelectedStatus = navigationContext.Parameters.GetValue<int>("Value");
            } 
            Query();
            base.OnNavigatedTo(navigationContext);
        }
    }
}
