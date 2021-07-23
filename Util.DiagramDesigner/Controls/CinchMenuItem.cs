using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.DiagramDesigner
{
    /// <summary>
    /// Provides a mechanism for constructing MenuItems
    /// within a ViewModel
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    ///  AND IN VIEWMODEL C# DO THIS TO CREATE MENUS
    ///  private List<CinchMenuItem> CreateMenus()
    ///  {
    ///    var menu = new List<CinchMenuItem>();
    ///    //create the File Menu
    ///    var miFile = new CinchMenuItem("File");
    ///    var miExit = new CinchMenuItem("Exit");
    ///    miExit.Command = ExitApplicationCommand;
    ///    miFile.Children.Add(miExit);
    ///    menu.Add(miFile);
    ///    //create the Actions Menu
    ///    menu.Add(new CinchMenuItem("Actions"));
    ///    return menu;
    ///  }
    /// 
    /// 
    ///  public List<CinchMenuItem> MenuOptions
    ///  {
    ///     get
    ///     {
    ///         return CreateMenus();
    ///     }
    ///  }
    /// 
    ///   AND IN XAML DO THE FOLLOWING FOR THE STYLE
    ///   <Style x:Key="ContextMenuItemStyle">
    ///     <Setter Property="MenuItem.Header" Value="{Binding Text}"/>
    ///     <Setter Property="MenuItem.ItemsSource" Value="{Binding Children}"/>
    ///     <Setter Property="MenuItem.Command" Value="{Binding Command}" />
    ///     <Setter Property="MenuItem.Icon" Value="{Binding Icon}" />
    ///   </Style>
    /// 
    ///   AND YOU CAN CREATE A MENU LIKE THIS
    ///   <StackPanel Orientation="Horizontal">
    ///     <Image Source="{Binding Image}" Width="16" Height="16" />
    ///     <TextBlock Margin="5" HorizontalAlignment="Left" VerticalAlignment="Center" 
    ///         Text="{Binding Header}" />
    ///     <StackPanel.ContextMenu>
    ///         <ContextMenu ItemContainerStyle="{StaticResource ContextMenuItemStyle}" 
    ///             ItemsSource="{Binding MenuOptions}" />
    ///     </StackPanel.ContextMenu>
    ///   </StackPanel>
    /// ]]>
    /// </example>
    public class CinchMenuItem
    {
        #region Public Properties
        public String Text { get; set; }
        public String IconUrl { get; set; }
        public bool IsChecked { get; set; }
        public bool IsCheckable { get; set; }
        public List<CinchMenuItem> Children { get; private set; }
        public Object CommandParameter { get; set; }
        public SimpleCommand Command { get; set; }
        #endregion

        #region Ctor
        public CinchMenuItem()
        {
            Children = new List<CinchMenuItem>();
        }

        public CinchMenuItem(string item)
        {
            Text = item;
            Children = new List<CinchMenuItem>();
        }
        #endregion
    }
}
