using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace RGZ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//  Style="{StaticResource testBtn}"
        public MainWindow()
        {
            InitializeComponent();
            btn_openmenu.Visibility = Visibility.Collapsed;
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //закрыть
        }
      

        private void btn_closemenu_Click(object sender, RoutedEventArgs e)
        {
            if (rect_menu.Height == 568 && rect_menu.Width == 250)
            {
                btn_closemenu.Visibility = Visibility.Collapsed;
                btn_openmenu.Visibility = Visibility.Visible;
                btn_openfile_opened.Visibility = Visibility.Collapsed;
                btn_save_opened.Visibility = Visibility.Collapsed;
                btn_settings_open.Visibility = Visibility.Collapsed;
                btn_exit_opened.Visibility = Visibility.Collapsed;
                btn_openfolder_opened.Visibility = Visibility.Collapsed;
                btn_about_opened.Visibility = Visibility.Collapsed;
                textBox_name.Visibility = Visibility.Collapsed;
                rect_menu.Height = 568;
                rect_menu.Width = 50;
         
            }
           
        }

        private void btn_openmenu_Click(object sender, RoutedEventArgs e)
        {
            btn_openmenu.Visibility = Visibility.Collapsed;
            btn_closemenu.Visibility = Visibility.Visible;
            btn_openfile_opened.Visibility = Visibility.Visible;
            btn_save_opened.Visibility = Visibility.Visible;
            btn_settings_open.Visibility = Visibility.Visible;
            btn_exit_opened.Visibility = Visibility.Visible;
            btn_openfolder_opened.Visibility = Visibility.Visible;
            btn_about_opened.Visibility = Visibility.Visible;
            textBox_name.Visibility = Visibility.Visible;
            rect_menu.Height = 568;
            rect_menu.Width = 250;
           
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_MouseDown(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //свернуть
        }

      
    }
}
