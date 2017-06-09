/*
-	расчёт метрик Холстеда;
-	расчёт метрик Джилба;
-	расчёт метрик Мак-Кейба;
-	расчёт метрик Мак-Клура;
-	расчёт метрик Кафура;
-	расчёт метрик Берлингера;
-	расчёт метрик Чепена;
*/
using System;///[e[e[e[e[s
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
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using RGZ.Properties;

namespace RGZ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btn_openmenu.Visibility = Visibility.Collapsed;
            //btn_closeright_Click(this, new RoutedEventArgs());
            btn_delete.Visibility = Visibility.Collapsed;
            btn_count.Visibility = Visibility.Collapsed;
            btn_deleteall.Visibility = Visibility.Collapsed;
            btn_settings_Click(this, new RoutedEventArgs());
            btn_openmenu_Click(this, new RoutedEventArgs());
            label_loading.Visibility = Visibility.Collapsed;
            showMetrics();
           // textBox_path.Text = Settings.Default["fwork"].ToString();
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
                btn_hideUI.IsEnabled = true;
            }

        }

        public void hideMetrics()
        {
            cb_Berlinger.Visibility = Visibility.Collapsed;
            cb_Chepen.Visibility = Visibility.Collapsed;
            cb_Holsted.Visibility = Visibility.Collapsed;
            cb_Jilb.Visibility = Visibility.Collapsed;
            cb_Kafur.Visibility = Visibility.Collapsed;
            cb_MakKeib.Visibility = Visibility.Collapsed;
            cb_MakKlur.Visibility = Visibility.Collapsed;
            cb_Svyaz.Visibility = Visibility.Collapsed;
            label_info.Visibility = Visibility.Collapsed;
           // textBox_path.Visibility = Visibility.Collapsed;
           // btn_framework.Visibility = Visibility.Collapsed;
        }

        public void showMetrics()
        {
            cb_Svyaz.Visibility = Visibility.Visible;
            cb_MakKlur.Visibility = Visibility.Visible;
            cb_MakKeib.Visibility = Visibility.Visible;
            cb_Kafur.Visibility = Visibility.Visible;
            cb_Jilb.Visibility = Visibility.Visible;
            cb_Holsted.Visibility = Visibility.Visible;
            cb_Chepen.Visibility = Visibility.Visible;
            cb_Berlinger.Visibility = Visibility.Visible;
            label_info.Visibility = Visibility.Visible;
           // textBox_path.Visibility = Visibility.Visible;
           // btn_framework.Visibility = Visibility.Visible;
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
            if(btn_delete.Visibility == Visibility.Visible)
            btn_hideUI_Click(this, new RoutedEventArgs());
            btn_hideUI.IsEnabled = false;
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Сохранить изменения?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                btn_save_opened_Click(this, new RoutedEventArgs());
            else if (result == MessageBoxResult.No)
                Close(); //зкрыть программу
        }

        private void Minimize_MouseDown(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //свернуть
        }

        private void Rect_navigation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); //перетаскивание окна
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            if (rect_menu.Height == 568 && rect_menu.Width == 250 || rect_menu.Width == 50)
            {
                textBox_settings.Text = "настройки";
                textBox_description.Visibility = Visibility.Collapsed;
                btn_apply.Visibility = Visibility.Visible;
                btn_training.Visibility = Visibility.Collapsed;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                if (rect_menu.Width == 250)
                {
                    btn_closemenu_Click(this, new RoutedEventArgs());
                }
                showMetrics();
            }
            else if (rect_settings.Height == 568 && rect_settings.Visibility == Visibility.Collapsed && rect_menu.Width == 50)
            {
                textBox_description.Visibility = Visibility.Collapsed;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                btn_training.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Collapsed;
                textBox_settings.Text = "настройки";
                showMetrics();

            }

        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            if (rect_menu.Height == 568 && rect_menu.Width == 250 || rect_menu.Width == 50)
            {
                textBox_settings.Text = "о программе";
                textBox_description.Visibility = Visibility.Visible;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                btn_training.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                if (rect_menu.Width == 250)
                {
                    btn_closemenu_Click(this, new RoutedEventArgs());
                }
                hideMetrics();
            }
            else if (rect_settings.Height == 568 && rect_settings.Visibility == Visibility.Collapsed && rect_menu.Width == 50)
            {
                textBox_description.Visibility = Visibility.Visible;
                btn_apply.Visibility = Visibility.Visible;
                btn_closeright.Visibility = Visibility.Visible;
                rect_settings.Visibility = Visibility.Visible;
                textBox_settings.Visibility = Visibility.Visible;
                btn_training.Visibility = Visibility.Visible;
                textBox_settings.Text = "о программе";
                hideMetrics();
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
                hideMetrics();
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
                hideMetrics();
            }
        }
        private void btn_hideUI_Click(object sender, RoutedEventArgs e)
        {
            if (btn_delete.Visibility == Visibility.Visible && btn_count.Visibility == Visibility.Visible)
            {
                btn_delete.Visibility = Visibility.Collapsed;
                btn_count.Visibility = Visibility.Collapsed;
                btn_deleteall.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_delete.Visibility = Visibility.Visible;
                btn_count.Visibility = Visibility.Visible;
                btn_deleteall.Visibility = Visibility.Visible;
            }

        }

        private void btn_save_opened_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Текстовый файл (*.txt)|*.txt";
            bool? res = sfd.ShowDialog();
            if (res == true)
            {
                using (StreamWriter sw = new StreamWriter(sfd.OpenFile(), System.Text.Encoding.Default))
                {
                    sw.Write(label_codes.Text);
                    sw.Close();
                }
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_namelist.Items.Count >= 1 )//&& listBox_namelist.SelectedIndex == listBox_namelist.Items.Count - 1)
            {
               // listBox_namelist.SelectedIndex++;
               // listBox_namelist.Items.RemoveAt(listBox_namelist.SelectedIndex - 1);
                listBox_namelist.Items.RemoveAt(listBox_namelist.SelectedIndex);
                label_codes.Text = "";
            }

        }
        private void btn_deleteall_Click(object sender, RoutedEventArgs e)
        {
         
            if (listBox_namelist.Items.Count != 0)
                for (int i = 0; i < listBox_namelist.Items
                      .Count; i++)
                    listBox_namelist.Items.RemoveAt(i);
            label_codes.Text = "";
            listBox_namelist.SelectedIndex = 0;
            btn_delete_Click(this, new RoutedEventArgs());
            
        }

        //private void btn_framework_Click(object sender, RoutedEventArgs e)
        //{
        //    FolderBrowserDialog dlg = new FolderBrowserDialog();
        //    DialogResult result = dlg.ShowDialog();
        //    dlg.ShowNewFolderButton = false;
        //    if (System.Windows.Forms.DialogResult.OK == result)//если папка выбрана
        //    {
        //        textBox_path.Text = dlg.SelectedPath;
        //    }
        //    Settings.Default["fwork"] = textBox_path.Text;
        //    Settings.Default.Save();
        //    textBox_path.Text = Settings.Default["fwork"].ToString();
        //}
        //РАБОТА С ФАЙЛОВОЙ СИСТЕМОЙ.........................................................................................................
        /// <summary>
        /// Доавление файлов 
        /// </summary>
        List<string> paths = new List<string>();
        private void btn_openfile_Click(object sender, RoutedEventArgs e)//кнопка открыть файлы (ПОКА ТОЛЬКО cs-ники)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "C# source (.cs)|*.cs";
            //лист путей к файлам
            dlg.Multiselect = true;//можно выбрать много файлов
            bool? res = dlg.ShowDialog();//вызываем окно открытия

            if (res == true)//если файлы выбраны
            {
                for (int i = 0; i < dlg.FileNames.Length; i++)
                {
                    if (dlg.FileNames[i].Length < 4 || (dlg.FileNames[i].Length > 3 && (dlg.FileNames[i][dlg.FileNames[i].Length - 1] != 's' || dlg.FileNames[i][dlg.FileNames[i].Length - 2] != 'c' || dlg.FileNames[i][dlg.FileNames[i].Length - 3] != '.')))
                    {
                        cs.Vars.Files.Clear();
                        paths.Clear();
                        //Бросить исключение "Недопустимое имя файла"     
                    }
                 
                    cs.Vars.Files.Add(dlg.FileNames[i]);
                    listBox_namelist.Items.Add(cs.Vars.GetFileName(dlg.FileNames[i])); //добавляем вкладки
                    paths.Add(dlg.FileNames[i]);//добавляем модули к листу
                }
                
                LoadingFiles(cs.Vars.Files);//загружаем каждый файл
                                     
                if (rect_settings.Height == 568 && rect_settings.Width == 250 && rect_menu.Width == 250)
                {
                    btn_closeright_Click(this, new RoutedEventArgs());
                    btn_closemenu_Click(this, new RoutedEventArgs());
                    btn_hideUI_Click(this, new RoutedEventArgs());
                    
                }
            }
            
        }
        private void btn_openfolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
            dlg.ShowNewFolderButton = false;
            if (System.Windows.Forms.DialogResult.OK == result)//если папка выбрана
            {
                foreach (string currentFile in System.IO.Directory.GetFiles(dlg.SelectedPath, "*.cs", SearchOption.AllDirectories))
                {
                    cs.Vars.Files.Add(currentFile);
                    listBox_namelist.Items.Add(cs.Vars.GetFileName(currentFile)); //добавляем вкладки
                    paths.Add(currentFile);//добавляем модули к листу        
                }
                LoadingFiles(paths);//загружаем каждый файл
                if (rect_settings.Height == 568 && rect_settings.Width == 250 && rect_menu.Width == 250)
                {
                    btn_closeright_Click(this, new RoutedEventArgs());
                    btn_closemenu_Click(this, new RoutedEventArgs());
                    btn_hideUI_Click(this, new RoutedEventArgs());
                }
            }
        }

        private void listBox_namelist_SelectionChanged(object sender, SelectionChangedEventArgs e)//выбор файла
        {
            label_codes.Text = "Код модуля:\n";
            try
            {
                if (listBox_namelist.Items.Count != 0)
                {
                    FileStream fs = new FileStream(cs.Vars.Files[listBox_namelist.SelectedIndex], FileMode.Open);


                    StreamReader sr = new StreamReader(fs);

                    while (!sr.EndOfStream)
                    {
                        label_codes.Text += sr.ReadLine() + "\n";
                    }
                    sr.Close();
                    fs.Close();
                }
            }
            catch
            {
                label_codes.Text = " ";
                listBox_namelist.SelectedIndex = -1;
            }
        }

        //ОБРАБОТКА ФАЙЛОВ...................................................................................................................
        /// <summary>
        /// Функция обработки файлов
        /// </summary>
        /// <param name="Files"></param>
        private void LoadingFiles(List<string> Files)
        {

            foreach (string x in cs.Vars.Files)//идём по файлам
            {
                List<string> ModuleText = new List<string>();//текст модуля
                FileStream fs = new FileStream(x, FileMode.Open, FileAccess.Read);//создаём файловый поток
                StreamReader sr = new StreamReader(fs);//создаём читателя
                //Здесь будет полная обработка файла, разбиение на лексемы, подсчет метрик и т.д.
                //посредством вызова методов для каждой из метрик
                while (!sr.EndOfStream)//читаем файл до конца
                {
                    ModuleText.Add(sr.ReadLine());
                }
                //результаты нужно куда-нибудь вывести или сохранить
                foreach (string s in ModuleText)
                    label_codes.Text += s;
                int CommentQuantity = 0;
                ModuleText = RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = ModuleMinimizing(ModuleText);//обработка модуля от лишних строк и разделителей
                ModuleText = ModuleProcessing(ModuleText);//обнуление строковых констант
                ModuleText = OneOperatorOneString(ModuleText);//"один оператор <=> одна строка"
                ModuleText = OneOperatorOneString(ModuleText);
                //восстановление имён, удаление шапок 
                //СalculationHolstedsMetrics(ModuleText);//вычисляем метрики Холстеда
                //System.Windows.MessageBox.Show(numberString.ToString());
                sr.Close();
                fs.Close();
            }
        }
        private List<string> ModuleMinimizing(List<string> Text)//обработка от лишних пробелов и пустых строк
        {
            //всё работает
            List<string> result = new List<string>();
            label_codes.Text = "Код модуля:\n";//после тестирования нужно удалить
            for (int i = 0; i < Text.Count; i++)//идём по листу
            {
                string Temp = "";
                for (int j = 0; j < Text[i].Length; j++)//идём по строке
                {
                    if (Char.IsSeparator(Text[i][j]))
                    {
                        Temp += Text[i][j];
                        while (j < Text[i].Length && Char.IsSeparator(Text[i][j]))
                            j++;
                        j--;
                    }
                    else
                        Temp += Text[i][j];
                }
                if (Temp != "" && !(Temp.Length == 1 && Char.IsSeparator(Temp[0])))
                {
                    result.Add(Temp);
                    label_codes.Text += Temp + "\n";//после тестирования нужно удалить
                }
            }
            return result;
        }
        private List<string> OneOperatorOneString(List<string> Text)
        {
            List<string> result = new List<string>();
            label_codes.Text = "Код модуля:\n";//после тестирования нужно удалить
            string Temp = "";
            foreach (string str in Text)
            {
                foreach (char x in str)
                {
                    if (x != '{')
                        Temp += x;
                    if (x == ';' || x == '{' || x == '}')
                    {
                        result.Add(Temp);
                        label_codes.Text += Temp + "\n";//после тестирования нужно удалить
                        Temp = "";
                        if (x == '{')//чтобы фигурную скобку отделить
                        {
                            Temp += x;
                            result.Add(Temp);
                            label_codes.Text += Temp + "\n";//после тестирования нужно удалить
                            Temp = "";
                        }
                    }
                }
            }
            return result;
        }
        private List<string> ModuleProcessing(List<string> Text)
        {
            label_codes.Text = "Код модуля:\n";//после тестирования нужно удалить
            List<String> result = new List<string>();
            string Temp = "";
            bool IsCarretIntoConstString = false, IsCarretIntoConstChar = false;
            for (int i = 0; i < Text.Count; i++)//идем по модулю
            {
                Temp = "";
                for (int j = 0; j < Text[i].Length; j++)
                {
                    if (!IsCarretIntoConstString && !IsCarretIntoConstChar)
                    {
                        if (Text[i][j] == '"' && (j == 0 || Text[i][j - 1] != '\\'))
                        {
                            Temp += Text[i][j];
                            j++;
                            IsCarretIntoConstString = true;
                            while (j < Text[i].Length && !(Text[i][j] == '"' && Text[i][j - 1] != '\\'))
                            {
                                j++;
                            }
                            if (j < Text[i].Length && j > 0 && Text[i][j] == '"' && Text[i][j - 1] != '\\')
                            {
                                Temp += Text[i][j];
                                IsCarretIntoConstString = false;
                                j++;
                            }
                        }
                        if (j < Text[i].Length && Text[i][j] == '\'')
                        {
                            Temp += Text[i][j];
                            j++;
                            IsCarretIntoConstChar = true;
                            while (j < Text[i].Length && !(Text[i][j] == '\'' && Text[i][j - 1] != '\\'))
                            {
                                j++;
                            }
                            if (j < Text[i].Length && Text[i][j] == '\'' && Text[i][j - 1] != '\\')
                            {
                                Temp += Text[i][j];
                                IsCarretIntoConstChar = false;
                                j++;
                            }
                        }
                    }
                    if (IsCarretIntoConstString)
                    {
                        while (j < Text[i].Length && !(Text[i][j] == '"' && Text[i][j - 1] != '\\'))
                        {
                            j++;
                        }
                        if (j < Text[i].Length && j > 0 && Text[i][j] == '"' && Text[i][j - 1] != '\\')
                        {
                            Temp += Text[i][j];
                            IsCarretIntoConstString = false;
                            j++;
                        }
                    }
                    if (IsCarretIntoConstChar)
                    {
                        while (j < Text[i].Length && !(Text[i][j] == '\'' && Text[i][j - 1] != '\\'))
                        {
                            j++;
                        }
                        if (j < Text[i].Length && Text[i][j] == '\'' && Text[i][j - 1] != '\\')
                        {
                            Temp += Text[i][j];
                            IsCarretIntoConstChar = false;
                            j++;
                        }
                    }
                    if (j < Text[i].Length)
                        Temp += Text[i][j];

                }
                result.Add(Temp);
                label_codes.Text += Temp + "\n";//после тестирования нужно удалить
            }
            return result;
        }

        private List<string> RemoveComments(List<string> Text, ref int CommentsQuantity)//считает и удаляет комментарии
        {
            //Считается, сколько строк содержит комментарий
            List<string> Result = new List<string>();
            int IsMultyStringComment = 0;
            string Temp = "";
            label_codes.Text = "Код модуля:\n";//после тестирования нужно удалить
            for (int i = 0; i < Text.Count; i++)//идём по листу
            {
                bool EndOfString = false;
                int IndexOfContinuous = 0;
                Temp = "";
                if (IsMultyStringComment > 0)//если многострочный коммент
                {
                    CommentsQuantity++;
                    for (int j = 0; j < Text[i].Length; j++)
                        if ((j != Text[i].Length - 1) && Text[i][j] == '*' && Text[i][j + 1] == '/')
                        {
                            if (j == Text[i].Length - 2)
                                EndOfString = true;
                            else IndexOfContinuous = j + 2;
                            IsMultyStringComment--;
                            break;
                        }
                }
                if (EndOfString) continue;
                if (IsMultyStringComment == 0) // тут не надо менять на елс, а то кусок строки может быть не проверен 
                    for (int j = IndexOfContinuous; j < Text[i].Length; j++)
                    {
                        //если однострочный или многострочный коммент
                        if (Text[i][j] == '/' && (j != Text[i].Length - 1))
                            if (Text[i][j + 1] == '/')
                            {
                                CommentsQuantity++;
                                break;
                            }
                            else if (Text[i][j + 1] == '*')
                            {
                                IsMultyStringComment++;
                                CommentsQuantity++;
                                while ((j < Text[i].Length - 1 && !(Text[i][j] != '*' && Text[i][j + 1] != '/')) || j == Text[i].Length - 1)
                                    j++;
                                if (j < Text[i].Length - 1 && Text[i][j] == '*' && Text[i][j + 1] == '/')
                                {
                                    j += 2;
                                    IsMultyStringComment--;
                                }
                            }
                        if ((j != Text[i].Length - 1) && Text[i][j] == '*' && Text[i][j + 1] == '/')
                        {
                            if (j == Text[i].Length - 2)
                            {
                                EndOfString = true;
                                break;
                            }
                            else j += 2;
                            IsMultyStringComment--;
                        }
                        if (IsMultyStringComment == 0 && j < Text[i].Length)
                            Temp += Text[i][j];
                    }
                Result.Add(Temp);
                label_codes.Text += Temp + "\n";//после тестирования нужно удалить
            }
            return Result;
        }

        private List<string> RestorationOfNameFromNickname(List<string> Text)//восстановление псевдонимов
        {
            List<string> Result = new List<string>();
            int i = 0;
            string Key = "", Value = "";
            string Temp = "";
            Dictionary<string, string> NicknameOldnameDictionary = new Dictionary<string, string>();//ключ псевдоним, значение стандартное имя
            while (!Text[i].Contains("class"))//после этого слова юзинги нельзя писать
            {
                if (Text[i].Contains("using") && Text[i].IndexOf('=') != -1)
                {
                    int j = Text[i].IndexOf('g') + 2;
                    while (!Char.IsSeparator(Text[i][j]))
                    {
                        Key += Text[i][j];
                        j++;
                    }
                    j = Text[i].IndexOf('=') + 2;
                    while (!Char.IsSeparator(Text[i][j]))
                    {
                        Value += Text[i][j];
                        j++;
                    }
                    NicknameOldnameDictionary.Add(Key, Value);//составлен словарь псевдонимов
                }
                Result.Add(Text[i]);
                i++;
            }
            for (int j = i; j < Text.Count; j++)
            {
                Temp = Text[i];
                foreach (string x in NicknameOldnameDictionary.Keys)
                {
                    for (int k = 0; k < x.Length; k++)
                    {
                        int index = Temp.IndexOf(x);
                        if (index != -1)
                        {
                            if ((i == 0 || (Temp[index - 1] == '.' || Char.IsSeparator(Temp[index - 1]))) && (Temp[index + x.Length + 1] == '.' || Char.IsSeparator(Temp[index + x.Length + 1])))
                            {
                                //допишу сегодня-завтра
                            }
                        }
                    }
                }
            }
            return Result;
        }
        private double[] СalculationHolstedsMetrics(List<string> Text)//возвращает значение метрик Холстеда
        {//теоретические величины дописать, елсы для ускорения кое-где повставлять
            //метрики в массиве: длина программы, объём программы, уровень качества программирования,
            //сложность понимания программы, трудоёмкость кодирования, уровень языка выражения
            bool[] operators = new bool[78];//массив "встречен ли оператор"
            List<string> Function = new List<string>(), TemplateClasses = new List<string>();//заполнить шаблонами

            double[] results = new double[6];//результаты
            double n1 = 0, n2 = 0, N1 = 0, N2 = 0; //мощность словаря операторов, операндов, суммарное количество операторов, операндов
            N1 = FindAllOperators(Text, ref operators, ref Function, TemplateClasses);
            //теперь всё надо посчитать
            return null;
        }
        /// <summary>
        /// Поиск всех операторов
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="operators"></param>
        /// <param name="Functions"></param>
        /// <param name="TemplateClasses"></param>
        /// <returns></returns>
        private int FindAllOperators(List<string> Text, ref bool[] operators, ref List<string> Functions, List<string> TemplateClasses)//поиск всех операторов
        {
            /*Операторы в следующем порядке: +(унарный), +(бинарный), -(унарный), -(бинарный), *, /, %, ++, --, ==, !=(10), >, <,
            >=, <=, is, &&, ||, !, >>, <<(20), &, |, ^, ~, +=, -=, *=, /=, =, %=(30), ^=, &=, |=, >>=, <<=, continue, break, return,
            goto, if(...)(40), for(...), while(...), foreach(...), switch(...), catch(...), throw, try, finally, (type), as(50), ., 
            [...], ->, ?., ?[, case, new, stacalloc, typeof, sizeof(60), nameof, {...}, ?:, yield, *(работа с указателями)
            fixed, lock, checked, unchecked, await, ::, =>, ??, true, false, using()*/

            int OperatorsQuantity = 0;//количество операторов
            foreach (string x in Text)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    //если if(...), for(...), while(...), foreach(...), switch(...) // внутри операторов не считают
                    if ((i == 0 || (i > 0 && !Char.IsDigit(x[i - 1]) && !Char.IsLetter(x[i - 1]) && x[i - 1] != '_')))
                    {
                        if ((i < x.Length - 4) && x[i] == 'i' && x[i + 1] == 'f' && (Char.IsSeparator(x[i + 2]) || x[i + 2] == '('))//встречено если
                        {
                            operators[40] = true;
                            while (i < x.Length && x[i] != ')')
                                i++;
                            i++;// пропускаем закрывающую скобку
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 5) && x[i] == 'f' && x[i + 1] == 'o' && x[i + 2] == 'r' && (Char.IsSeparator(x[i + 3]) || x[i + 3] == '('))//встречен фор
                        {
                            operators[41] = true;
                            while (i < x.Length && x[i] != ')')
                                i++;
                            i++;// пропускаем закрывающую скобку
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 7) && x[i] == 'w' && x[i + 1] == 'h' && x[i + 2] == 'i' && x[i + 3] == 'l' && x[i + 4] == 'e' && (Char.IsSeparator(x[i + 5]) || x[i + 5] == '('))//встречен вайл
                        {
                            operators[42] = true;
                            while (i < x.Length && x[i] != ')')
                                i++;
                            i++;// пропускаем закрывающую скобку
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 8) && x[i] == 's' && x[i + 1] == 'w' && x[i + 2] == 'i' && x[i + 3] == 't' && x[i + 4] == 'c' && x[i + 5] == 'h' && (Char.IsSeparator(x[i + 6]) || x[i + 6] == '('))//встречен свич
                        {
                            operators[44] = true;
                            while (i < x.Length && x[i] != ')')
                                i++;
                            i++;// пропускаем закрывающую скобку
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 9) && x[i] == 'f' && x[i + 1] == 'o' && x[i + 2] == 'r' && x[i + 3] == 'e' && x[i + 4] == 'a' && x[i + 5] == 'c' && x[i + 6] == 'h' && (Char.IsSeparator(x[i + 7]) || x[i + 7] == '('))//встречен форич
                        {
                            operators[43] = true;
                            while (i < x.Length && x[i] != ')')//
                                i++;
                            i++;// пропускаем закрывающую скобку
                            OperatorsQuantity++;
                        }
                    }
                    //если '(' от оператора приведения типов или функции
                    if (i > 1 && x[i] == '(' && ((Char.IsSeparator(x[i - 1]) && (Char.IsLetterOrDigit(x[i - 2]) || x[i - 2] == '_')) || Char.IsLetter(x[i - 1]) || x[i - 1] == '_') || (i > 0 && x[i] == '(' && ((Char.IsLetterOrDigit(x[i - 1]) || x[i - 1] == '_'))))
                    {
                        bool IsCoertionOperator = false;
                        int h = i;
                        while (x[h] != ')' && h < x.Length)
                        {
                            if (x[h] == ')' && h < x.Length - 2 && ((Char.IsSeparator(x[h + 1]) && (Char.IsLetter(x[h + 2]) || x[h + 2] == '_')) || Char.IsLetter(x[h + 1]) || x[h + 1] == '_'))
                            {
                                IsCoertionOperator = true;
                                operators[49] = true;
                                i = h;
                                OperatorsQuantity++;
                            }
                            h++;
                        }
                        if (!IsCoertionOperator)//если функция
                        {
                            int j = i - 1;
                            string InvertedNameOfFunction = "", NameOfFunction = "";
                            while (j >= 0 && (Char.IsLetter(x[j]) || Char.IsDigit(x[j]) || x[j] == '_' || x[j] == '.'))
                            {
                                InvertedNameOfFunction += x[j];
                                j--;
                            }
                            for (int k = InvertedNameOfFunction.Length - 1; k >= 0; k--)
                                NameOfFunction += InvertedNameOfFunction[k];
                            OperatorsQuantity++;
                            if (NameOfFunction == "nameof")
                                operators[61] = true;
                            else if (NameOfFunction == "sizeof")
                                operators[60] = true;
                            else if (NameOfFunction == "typeof")
                                operators[59] = true;
                            else
                            {
                                bool IsFunctionWasMet = false;
                                foreach (string q in Functions)
                                    if (q == NameOfFunction)
                                        IsFunctionWasMet = true;
                                if (!IsFunctionWasMet)
                                    Functions.Add(NameOfFunction);
                            }
                        }
                    }
                    else if (Char.IsLetter(x[i]) || x[i] == '_')//отрулил шаблоны
                    {
                        int h = i;
                        string Name = "";
                        while (h < x.Length && (Char.IsLetter(x[h]) || x[h] == '_'))
                        {
                            Name += x[h];
                            h++;
                        }
                        i = h;
                        if (TemplateClasses.Contains(Name))
                        {
                            while (x[h] != '>') h++;
                            i = h;
                        }
                        else if (Name == "using")
                        {
                            operators[77] = true;
                            OperatorsQuantity++;
                        }
                        //словесные операторы
                        else if (Name == "continue")
                        {
                            operators[36] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "break")
                        {
                            operators[37] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "return")
                        {
                            operators[38] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "goto")
                        {
                            operators[40] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "is")
                        {
                            operators[15] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "as")
                        {
                            operators[50] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "new")
                        {
                            operators[57] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "stackalloc")
                        {
                            operators[58] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "yield")
                        {
                            operators[64] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "fixed")
                        {
                            operators[66] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "lock")
                        {
                            operators[67] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "checked")
                        {
                            operators[68] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "unchecked")
                        {
                            operators[69] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "await")
                        {
                            operators[70] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "true")
                        {
                            operators[74] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "false")
                        {
                            operators[75] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "throw")
                        {
                            operators[46] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "try")
                        {
                            operators[47] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "catch")
                        {
                            operators[45] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "finally")
                        {
                            operators[48] = true;
                            OperatorsQuantity++;
                        }
                        else if (Name == "case")
                        {
                            operators[56] = true;
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '+')
                    {
                        if (x[i - 1] == '+' || x[i + 1] == '=') continue;//если конец ++ или это +=
                        else if (x[i + 1] == '+')
                        {
                            operators[7] = true;//использован инкремент
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '(' || x[i - 1] == '=' || (x[i - 1] == ' ' && x[i - 2] == '('))//если унарный минус
                        {
                            operators[0] = true;//использован унарный плюс
                            OperatorsQuantity++;
                        }
                        else
                        {
                            operators[1] = true;//использован бинарный плюс
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '-')
                    {
                        if (x[i - 1] == '-' || x[i + 1] == '=') continue;//если конец -- или это -=
                        else if (x[i + 1] == '-')
                        {
                            operators[8] = true;//использован инкремент
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '(' || x[i - 1] == '=' || (x[i - 1] == ' ' && x[i - 2] == '('))//если унарный минус
                        {
                            operators[2] = true;//использован унарный минус
                            OperatorsQuantity++;
                        }
                        else
                        {
                            operators[3] = true;//использован бинарный минус
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '*' && ((x[i - 1] == ' ' && (Char.IsDigit(x[i - 2]) || Char.IsLetter(x[i - 2]) || x[i - 2] == '_' || x[i - 2] == ')')) || (Char.IsDigit(x[i - 1]) || Char.IsLetter(x[i - 1]) || x[i - 1] == '_' || x[i - 1] == ' ' || x[i - 1] == ')')))
                    //если умножение, а не разыменование
                    {
                        operators[4] = true;//использовано умножение
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '*' && ((x[i - 1] == ' ' && (!Char.IsDigit(x[i - 2]) && !Char.IsLetter(x[i - 2]) && x[i - 2] != '_' && x[i - 2] != ')')) || (!Char.IsDigit(x[i - 1]) && !Char.IsLetter(x[i - 1]) && x[i - 1] != '_' && x[i - 1] != ' ' && x[i - 1] != ')')))
                    //если разыменование
                    {
                        operators[65] = true;//использовано разыменование
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '/')
                    //если деление
                    {
                        operators[5] = true;//использовано деление
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '%')
                    {
                        operators[6] = true;//использован остаток от деления
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '=')
                    {
                        if (x[i - 1] == '=')
                        {
                            operators[9] = true;//использовано ==
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '!')
                        {
                            operators[10] = true;//использовано !=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '>')
                        {
                            operators[13] = true;//использовано >=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '<')
                        {
                            operators[14] = true;//использовано <=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '+')
                        {
                            operators[25] = true;//использовано +=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '-')
                        {
                            operators[26] = true;//использовано -=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '*')
                        {
                            operators[27] = true;//использовано *=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '/')
                        {
                            operators[28] = true;//использовано /=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '%')
                        {
                            operators[30] = true;//использовано %=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '<' && x[i - 2] == '<')
                        {
                            operators[35] = true;//использовано <<=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '>' && x[i - 2] == '>')
                        {
                            operators[34] = true;//использовано >>=
                            OperatorsQuantity++;
                        }
                        else
                        {
                            operators[29] = true;//использовано =
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '>' && x[i + 1] != '=' && x[i - 1] != '-' && x[i + 1] != '>' && x[i - 1] != '>' && x[i - 1] != '=')//если знак больше
                    {
                        operators[11] = true;//использовано >
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '<' && x[i + 1] != '=' && x[i + 1] != '<' && x[i - 1] != '<' && x[i - 1] != '=')//если знак меньше
                    {
                        operators[12] = true;//использовано <
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '<' && x[i + 2] != '=' && x[i + 1] == '<')//если <<
                    {
                        operators[20] = true;//использовано <<
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '>' && x[i + 2] != '=' && x[i + 1] == '>')//если >>
                    {
                        operators[19] = true;//использовано >>
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '>' && x[i - 1] == '-')//если ->
                    {
                        operators[53] = true;//использовано ->
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '>' && x[i - 1] == '=')//если =>
                    {
                        operators[72] = true;//использовано =>
                        OperatorsQuantity++;
                    }
                    //if (i < 4 || (i == 4 && !(x[i - 4] == 'L' && x[i - 3] == 'i' && x[i - 2] == 's' && x[i - 1] == 't')) || (i > 4 && !(x[i - 5] == 'L' && x[i - 4] == 'i' && x[i - 3] == 's' && x[i - 2] == 't' && x[i - 1] == ' ')))
                    else if (x[i] == '&')//если оперсанд
                    {
                        if (i > 0 && x[i - 1] == '&') continue;//если конец лог И
                        else if (i < x.Length && x[i + 1] == '=')
                        {
                            operators[32] = true;//использовано &=
                            OperatorsQuantity++;
                        }
                        else if (i < x.Length && x[i + 1] == '&')
                        {
                            operators[16] = true;//использовано &&
                            OperatorsQuantity++;
                        }
                        else
                        {
                            operators[21] = true;//использовано &
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '|')//если |
                    {
                        if (i > 0 && x[i - 1] == '|') continue;//если конец лог ИЛИ
                        else if (i < x.Length && x[i + 1] == '=')
                        {
                            operators[33] = true;//использовано |=
                            OperatorsQuantity++;
                        }
                        else if (i < x.Length && x[i + 1] == '|')
                        {
                            operators[17] = true;//использовано ||
                            OperatorsQuantity++;
                        }
                        else
                        {
                            operators[22] = true;//использовано |
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '^')//если xor
                    {
                        if (i < x.Length && x[i + 1] == '=')
                        {
                            operators[31] = true;//использовано ^=
                            OperatorsQuantity++;
                        }
                        else
                        {
                            operators[23] = true;//использовано ^
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '!' && x[i + 1] != '=')
                    {
                        operators[18] = true;//использован !
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '~')
                    {
                        operators[24] = true;//использован ~
                        OperatorsQuantity++;
                    }
                    else if (x[i] == '.')
                    {
                        operators[51] = true;//использован .
                        OperatorsQuantity++;
                        if (x[i - 1] == '?')
                        {
                            operators[54] = true;//использован ?.
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '?')
                    {
                        if (x[i + 1] == '[')
                        {
                            operators[55] = true;//использован ?[
                            OperatorsQuantity++;
                        }
                        else if (x[i + 1] == '?')
                        {
                            operators[73] = true;//использован ??
                            OperatorsQuantity++;
                        }
                        else if (x[i + 1] != '.')
                        {
                            operators[63] = true;//использовано ?:
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '[')
                    {
                        operators[52] = true;//использован []
                        OperatorsQuantity++;
                    }
                    else if (x[i] == ':')
                    {
                        if (x[i + 1] == ':') continue;
                        else
                        {
                            operators[63] = true;//использовано ::
                            OperatorsQuantity++;
                        }
                    }
                    else if (x[i] == '{')
                    {
                        operators[62] = true;//использованы {}
                        OperatorsQuantity++;
                    }
                }
            }
            return OperatorsQuantity;
        }

       













        //private int FindAllOperands()
        //{

        //      return OperandsQuantity;
        //}
    }
}

