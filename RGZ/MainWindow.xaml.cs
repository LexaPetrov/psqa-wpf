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
            this.Close(); //закрыть программу на крестик (Х)
        }
      

        private void btn_closemenu_Click(object sender, RoutedEventArgs e)
        {
            if (rect_menu.Height == 568 && rect_menu.Width == 250)
            {
                System.Windows.Media.Animation.DoubleAnimation da = new System.Windows.Media.Animation.DoubleAnimation();
                da.From = 250;
                da.To = rect_menu.Width - 200;
                da.Duration = TimeSpan.FromSeconds(0.22222);
              /*--------------------------------------------------------*/
                rect_menu.BeginAnimation(Rectangle.WidthProperty, da);
                btn_closemenu.Visibility = Visibility.Collapsed;
                btn_openmenu.Visibility = Visibility.Visible;
                btn_openfile_opened.Visibility = Visibility.Collapsed;
                btn_save_opened.Visibility = Visibility.Collapsed;
                btn_settings_open.Visibility = Visibility.Collapsed;
                btn_exit_opened.Visibility = Visibility.Collapsed;
                btn_openfolder_opened.Visibility = Visibility.Collapsed;
                btn_about_opened.Visibility = Visibility.Collapsed;
                textBox_name.Visibility = Visibility.Collapsed;
                /*--------------------------------------------------------*/




            }

        }

        private void btn_openmenu_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Animation.DoubleAnimation da = new System.Windows.Media.Animation.DoubleAnimation();
            da.From = rect_menu.Width - 200;
            da.To = 250;
            da.Duration = TimeSpan.FromSeconds(0.07);
            rect_menu.BeginAnimation(Rectangle.WidthProperty, da);
         /*--------------------------------------------------------*/
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
          /*--------------------------------------------------------*/
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); //зкрыть программу
        }

        private void Minimize_MouseDown(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //свернуть
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            if (rect_menu.Height == 568 && rect_menu.Width == 250 || rect_menu.Width==50)
            {
                textBox_settings.Text = "настройки";
                textBox_description.Visibility = Visibility.Collapsed;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                if (rect_menu.Width == 250)
                {
                    
                    btn_closemenu_Click(this, new RoutedEventArgs());
                }
            }  
           else if(rect_settings.Height == 568 && rect_settings.Visibility == Visibility.Collapsed && rect_menu.Width == 50 )
            {
                textBox_description.Visibility = Visibility.Collapsed;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                textBox_settings.Text = "настройки";
            }
           
        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            if (rect_menu.Height == 568 && rect_menu.Width == 250 || rect_menu.Width==50)
            {
                textBox_settings.Text = "о программе";
                textBox_description.Visibility = Visibility.Visible;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                if (rect_menu.Width == 250)
                {
                   
                    btn_closemenu_Click(this, new RoutedEventArgs());
                }
            }
           else if (rect_settings.Height == 568 && rect_settings.Visibility == Visibility.Collapsed && rect_menu.Width == 50)
            {
                textBox_description.Visibility = Visibility.Visible;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                textBox_settings.Text = "о программе";
            }
            
        }

        private void btn_apply_Click(object sender, RoutedEventArgs e)
        {
            if (rect_settings.Height == 568 && rect_settings.Width == 250 || rect_settings.Width == 50)
            {
              
                textBox_description.Visibility = Visibility.Collapsed;
                rect_settings.Visibility = Visibility.Collapsed;
                textBox_settings.Visibility = Visibility.Collapsed;
                btn_apply.Visibility = Visibility.Collapsed;
                btn_closeright.Visibility = Visibility.Collapsed;
            }
        }

        private void btn_closeright_Click(object sender, RoutedEventArgs e)
        {
            if (rect_settings.Height == 568 && rect_settings.Width == 250 || rect_settings.Width == 50)
            {
               
                textBox_description.Visibility = Visibility.Collapsed;
                rect_settings.Visibility = Visibility.Collapsed;
                textBox_settings.Visibility = Visibility.Collapsed;
                btn_apply.Visibility = Visibility.Collapsed;
                btn_closeright.Visibility = Visibility.Collapsed;
            }
        }
    }
}
