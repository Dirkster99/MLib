namespace BindToMLib.BindDropDownButtonLib
{
    using System.Windows;

    public class BindAccentColor
    {
        public void Bind()
        {
            Application.Current.Resources[DropDownButtonLib.Themes.ResourceKeys.ControlAccentColorKey] =
            Application.Current.Resources[MLib.Themes.ResourceKeys.ControlAccentColorKey];
        }
    }
}
