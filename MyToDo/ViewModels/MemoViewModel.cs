using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    class MemoViewModel : NavigationViewModel
    {
        public MemoViewModel(SharedData sharedate, IMemoService service, IContainerProvider containerProvider) : base(containerProvider)
        {
            //init();
            // 弹出添加待办窗口
            AddCommand = new DelegateCommand(() =>
            {
                IsRightDrawerOpen = !IsRightDrawerOpen;
            });
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            ExecuteCommand = new DelegateCommand<string>(Execute);
            SelectedCommand = new DelegateCommand<MemoDto>(Selected);
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            this.sharedate = sharedate; 
            this.service = service;
            //GetDataListAsync();
        }

        private void Selected(MemoDto obj)
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

        private async void Delete(MemoDto dtoToDelete)
        {
            var dialogResult = await dialogHost.Question("温馨提示", $"确认删除备忘录:{dtoToDelete.Title}");
            if (dialogResult?.Result != Prism.Services.Dialogs.ButtonResult.OK)
            {
                var  x = dialogResult?.Result;
                return;
            }

            var apiResponse = await service.DeleteAsync(dtoToDelete.Id);
            if (apiResponse?.Code == 200)
            {// 删除成功
                MemoDtos.Remove(dtoToDelete);
            }
        }

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
                    Refrash();  break;
            }
        }

        private async void Refrash()
        {
            RealMemoDtos.Clear();
            MemoDtos.Clear();
            await GetDataListAsync();
            Query();
        }

        private async void PostAndSave(MemoDto memoDto)
        {
            if (string.IsNullOrEmpty(memoDto.Title) || string.IsNullOrEmpty(memoDto.Content)) { return; }

            ApiResponse<MemoDto>? apiResponse;
            // 如果id为默认值,则 todo为新增的
            if (memoDto.Id == 0)
            {
                apiResponse = await service.AddAsync(memoDto);
                if (apiResponse?.Code == 200)
                {
                    var x = MemoDtos;
                    var y = RealMemoDtos;
                    RealMemoDtos.Add(apiResponse.Content); 
                    if (!string.IsNullOrEmpty(Search))
                    { 
                        if (apiResponse.Content.Title.Contains(Search.Trim()) || apiResponse.Content.Content.Contains(Search.Trim()))
                        {
                            MemoDtos.Add(apiResponse.Content);
                        }
                    }
                }
            }
            else
            {
                apiResponse = await service.UpdateAsync(memoDto);
                if (apiResponse?.Code == 200)
                {
                    memoDto = apiResponse.Content;
                }
            }
            // 关闭右侧窗口
            IsRightDrawerOpen = false;
        }

        private void Query()
        {      
            if (!string.IsNullOrEmpty(Search))
            {
                // 可以本地过滤显示
                var filterList = RealMemoDtos.Where(x => x.Title.Contains(Search.Trim()) || x.Content.Contains(Search.Trim()));
                MemoDtos = new ObservableCollection<MemoDto>(filterList);
            }
            else
            {
                MemoDtos = RealMemoDtos;
            }
        }

        private void Add()
        {
            CurrentSelectedDto = new MemoDto();
            IsRightDrawerOpen = !IsRightDrawerOpen;
        }

        /// <summary>
        /// 右侧窗口是否展开
        /// </summary>
        private bool isRightDrawerOpen = false;
        private readonly SharedData sharedate;
        private readonly IMemoService service;

        //  右侧编辑栏里面绑定的dto
        private MemoDto currentSelectedDto;
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; set; }
        public DelegateCommand<string> ExecuteCommand { get; set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; set; }

        private readonly IDialogHostService dialogHost;

        // 原始dtos
        public ObservableCollection<MemoDto> RealMemoDtos { get; set; } = new ObservableCollection<MemoDto>();
        // 过滤后的dto
        private ObservableCollection<MemoDto> memoDtos = new ObservableCollection<MemoDto>();
        private string search = "";

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        // 编辑选中对象/新增对象
        public MemoDto CurrentSelectedDto
        {
            get { return currentSelectedDto; }
            set { SetProperty(ref currentSelectedDto, value); }
        }
        public bool IsRightDrawerOpen
        {
            get => isRightDrawerOpen;
            set => SetProperty(ref isRightDrawerOpen, value);
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
        private void init()
        {
            for (int i = 0; i < 30; i++)
            {

                MemoDtos.Add(new MemoDto() { Title = "待办" + i, Content = "ffgjhrt" + i});
            }
            MemoDtos.Add(new MemoDto() { Title = "待办1", Content = "sfasd"   });
            MemoDtos.Add(new MemoDto() { Title = "待办2", Content = "qwt25t" });
            MemoDtos.Add(new MemoDto() { Title = "待办3", Content = "rhrt234" });
            MemoDtos.Add(new MemoDto() { Title = "待办4", Content = "ffgjhrt" });
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
            RealMemoDtos.AddRange(res.Content);
            // 关闭等待窗口
            UpdateLoading(false);
        }
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        { 
            this.RealMemoDtos = sharedate.RealMemoDtos;
            if (!sharedate.IsMemoDtosInited)
            {
                await GetDataListAsync();
            }
            Query();
            base.OnNavigatedTo(navigationContext);
        }
    }

}
