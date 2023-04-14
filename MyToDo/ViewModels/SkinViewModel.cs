using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyToDo.ViewModels
{
    public class SkinViewModel : BindableBase 
    {
        public SkinViewModel()
        {

            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
        }

        private readonly PaletteHelper paletteHelper = new PaletteHelper();
        public IEnumerable<ISwatch> Swatches { get; set; } = SwatchHelper.Swatches; 
        public DelegateCommand<object> ChangeHueCommand { get; }
        private bool isDarkTheme = false;
        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                if (SetProperty(ref isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                }
            }
        } 

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
        private void ChangeHue(object? obj)
        {
            var hue = (Color)obj!;

            ITheme theme = paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(hue.Lighten());
            theme.PrimaryMid = new ColorPair(hue);
            theme.PrimaryDark = new ColorPair(hue.Darken());

            paletteHelper.SetTheme(theme);
        }
         
    }
}
