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
using System.IO;

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
        //РАБОТА С ФАЙЛОВОЙ СИСТЕМОЙ.........................................................................................................
        private void btn_openfile_Click(object sender, RoutedEventArgs e)//кнопка открыть файлы (ПОКА ТОЛЬКО cs-ники)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "C# source (.cs)|*.cs";
            List<string> paths = new List<string>();//лист путей к файлам
            dlg.Multiselect = true;//можно выбрать много файлов
            bool? res = dlg.ShowDialog();//вызываем окно открытия
            if (res == true)//если файлы выбраны
            {
                for (int i = 0; i < dlg.FileNames.Length; i++)
                {
                    if (dlg.FileNames[i].Length < 4 || (dlg.FileNames[i].Length > 3 && (dlg.FileNames[i][dlg.FileNames[i].Length - 1] != 's' || dlg.FileNames[i][dlg.FileNames[i].Length - 2] != 'c' || dlg.FileNames[i][dlg.FileNames[i].Length - 3] != '.')))
                    {
                        paths.Clear();
                        //Бросить исключение "Недопустимое имя файла"     
                    }
                    paths.Add(dlg.FileNames[i]);//добавляем модули к листу
                }


                LoadingFiles(paths);//загружаем каждый файл
            }
        }
        //ОБРАБОТКА ФАЙЛОВ...................................................................................................................
        private void LoadingFiles(List<string> paths)
        {
            foreach (string x in paths)//идём по файлам
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
                int CommentQuantity = 0;
                ModuleText = RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = ModuleProcessing(ModuleText);//обработка модуля от лишних строк и разделителей
                СalculationHolstedsMetrics(ModuleText);//вычисляем метрики Холстеда
                //System.Windows.MessageBox.Show(numberString.ToString());
            }
        }
        private List<string> ModuleProcessing(List<string> Text)//обработка от лишних пробелов и пустых строк
        {
            List<string> result = new List<string>();
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
                    }
                    else
                        Temp += Text[i][j];
                }
                if (Temp != "" && !(Temp.Length == 1 && Char.IsSeparator(Temp[0])))
                    result.Add(Temp);
            }
            return result;
        }

        private List<string> RemoveComments(List<string> Text, ref int CommentsQuantity)//считает и удаляет комментарии
        {
            //Считается сколько строк содержат комментарий
            List<string> Result = new List<string>();
            bool IsMultyStringComment = false;
            for (int i = 0; i < Text.Count; i++)//идём по листу
            {
                if (IsMultyStringComment)//если многострочный коммент
                {
                    CommentsQuantity++;
                    for (int j = 0; j < Text[i].Length; j++)
                        if ((j != Text[i].Length - 1) && Text[i][j] == '*' && Text[i][j + 1] == '/')
                            IsMultyStringComment = false;
                }
                else
                    for (int j = 0; j < Text[i].Length; j++)
                    {
                        //если однострочный или многострочный коммент
                        if (Text[i][j] == '/' && (i != Text[i].Length - 1))
                        {
                            if (Text[i][j + 1] == '/')
                            {
                                CommentsQuantity++;
                                break;
                            }
                            else if (Text[i][j + 1] == '*')
                            {
                                IsMultyStringComment = true;
                                CommentsQuantity++;
                                break;
                            }
                        }
                        Result[i] += Text[i][j];//может вылетит, погляжу
                    }
            }
            return Result;
        }

        private double[] СalculationHolstedsMetrics(List<string> Text)//возвращает значение метрик Холстеда
        {//теоретические величины дописать, елсы для ускорения кое-где повставлять
            //метрики в массиве: длина программы, объём программы, уровень качества программирования,
            //сложность понимания программы, трудоёмкость кодирования, уровень языка выражения
            bool[] operators = new bool[77];//массив "встречен ли оператор"
            List<string> Function = new List<string>(), TemplateClasses = new List<string>();//заполнить шаблонами

            double[] results = new double[6];//результаты
            double n1 = 0, n2 = 0, N1 = 0, N2 = 0; //мощность словаря операторов, операндов, суммарное количество операторов, операндов
            N1 = FindAllOperators(Text, ref operators, ref Function, TemplateClasses);
            //теперь всё надо посчитать
            return null;
        }
        private int FindAllOperators(List<string> Text, ref bool[] operators, ref List<string> Functions, List<string> TemplateClasses)//поиск всех операторов
        {
            /*Операторы в следующем порядке: +(унарный), +(бинарный), -(унарный), -(бинарный), *, /, %, ++, --, ==, !=(10), >, <,
            >=, <=, is, &&, ||, !, >>, <<(20), &, |, ^, ~, +=, -=, *=, /=, =, %=(30), ^=, &=, |=, >>=, <<=, continue, break, return,
            goto, if(...)(40), for(...), while(...), foreach(...), switch(...), catch(...), throw, try, finally, (type), as(50), ., 
            [...], ->, ?., ?[, case, new, stacalloc, typeof, sizeof(60), nameof, {...}, ?:, yield, *(работа с указателями)
            fixed, lock, checked, unchecked, await, ::, =>, ??, true, false*/

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
                    if (x[i] == '(')
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
                        if (TemplateClasses.Contains(Name))
                        {
                            while (x[h] != '>') h++;
                            i = h;
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

        //}
    }
}
