using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RGZ.cs
{
    class HolstedMetrics
    {

        public static double[] СalculationHolstedsMetrics(List<string> Files)//возвращает значение метрик Холстеда
        {
            //метрики в массиве: длина программы, объём программы, уровень качества программирования,
            //сложность понимания программы, трудоёмкость кодирования, уровень языка выражения

            List<string> ModuleText = new List<string>();//текст модуля для подсчёта операторов
            bool[] operators = new bool[78];//массив "встречен ли оператор"
            List<string> Function = new List<string>();//массив вызванных функций
            List<string> operands = new List<string>();//массив операндов
            double[] results = new double[6];//результаты
            double n1 = 0, n2 = 0, N1 = 0, N2 = 0; //мощность словаря операторов, операндов, суммарное количество операторов, операндов

            foreach (string x in Files)//идём по файлам
            {
                FileStream fs = new FileStream(x, FileMode.Open, FileAccess.Read);//создаём файловый поток
                StreamReader sr = new StreamReader(fs);//создаём читателя
                ModuleText = new List<string>();
                while (!sr.EndOfStream)//читаем файл до конца
                {
                    ModuleText.Add(sr.ReadLine());
                }

                int CommentQuantity = 0;
                ModuleText = cs.TransforamationFunctions.RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = cs.TransforamationFunctions.ModuleMinimizing(ModuleText);//обработка модуля от лишних строк и разделителей
                ModuleText = cs.TransforamationFunctions.ModuleProcessing(ModuleText);//обнуление строковых констант
                ModuleText = cs.TransforamationFunctions.OneOperatorOneString(ModuleText);//"один оператор <=> одна строка"
                ModuleText = cs.TransforamationFunctions.RestorationOfNameFromNickname(ModuleText);//восстановление псевдонимов

                N1 += FindAllOperators(ModuleText, ref operators, ref Function) + Function.Count;
                N2 += FindAllOperands(ModuleText, ref operands);

                sr.Close();
                fs.Close();
            }

            n2 += operands.Count;
            foreach (bool i in operators)//подсчёт словаря операторов
                if (i) n1++;

            results[0] = N1 + N2;//Длина программы
            results[1] = (N1 + N2) * Math.Log((n1 + n2), 2.0);//Объём программы
            results[2] = (2 * n2) / (n1 * N2);//Уровень качества программирования
            results[3] = results[1] / results[2];//сложность понимания программы
            results[4] = 1 / results[2];//Трудоёмкость кодирования
            results[5] = results[1] / (results[4] * results[4]);//уровень языка выражения
            return results;
        }
        /// <summary>
        /// Поиск всех операторов
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="operators"></param>
        /// <param name="Functions"></param>
        /// <param name="TemplateClasses"></param>
        /// <returns></returns>
        public static int FindAllOperators(List<string> Text, ref bool[] operators, ref List<string> Functions)//поиск всех операторов
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
                            int bracketCounter = 1;
                            while (x[i] != '(') i++;
                            i++;
                            while (i < x.Length && bracketCounter != 0)
                            {
                                if (x[i] == '(')
                                    bracketCounter++;
                                if (x[i] == ')')
                                    bracketCounter--;
                                i++;
                            }
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 5) && x[i] == 'f' && x[i + 1] == 'o' && x[i + 2] == 'r' && (Char.IsSeparator(x[i + 3]) || x[i + 3] == '('))//встречен фор
                        {
                            operators[41] = true;
                            int bracketCounter = 1;
                            while (x[i] != '(') i++;
                            i++;
                            while (i < x.Length && bracketCounter != 0)
                            {
                                if (x[i] == '(')
                                    bracketCounter++;
                                if (x[i] == ')')
                                    bracketCounter--;
                                i++;
                            }
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 7) && x[i] == 'w' && x[i + 1] == 'h' && x[i + 2] == 'i' && x[i + 3] == 'l' && x[i + 4] == 'e' && (Char.IsSeparator(x[i + 5]) || x[i + 5] == '('))//встречен вайл
                        {
                            operators[42] = true;
                            int bracketCounter = 1;
                            while (x[i] != '(') i++;
                            i++;
                            while (i < x.Length && bracketCounter != 0)
                            {
                                if (x[i] == '(')
                                    bracketCounter++;
                                if (x[i] == ')')
                                    bracketCounter--;
                                i++;
                            }
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 8) && x[i] == 's' && x[i + 1] == 'w' && x[i + 2] == 'i' && x[i + 3] == 't' && x[i + 4] == 'c' && x[i + 5] == 'h' && (Char.IsSeparator(x[i + 6]) || x[i + 6] == '('))//встречен свич
                        {
                            operators[44] = true;
                            int bracketCounter = 1;
                            while (x[i] != '(') i++;
                            i++;
                            while (i < x.Length && bracketCounter != 0)
                            {
                                if (x[i] == '(')
                                    bracketCounter++;
                                if (x[i] == ')')
                                    bracketCounter--;
                                i++;
                            }
                            OperatorsQuantity++;
                        }
                        else if ((i < x.Length - 9) && x[i] == 'f' && x[i + 1] == 'o' && x[i + 2] == 'r' && x[i + 3] == 'e' && x[i + 4] == 'a' && x[i + 5] == 'c' && x[i + 6] == 'h' && (Char.IsSeparator(x[i + 7]) || x[i + 7] == '('))//встречен форич
                        {
                            operators[43] = true;
                            int bracketCounter = 1;
                            while (x[i] != '(') i++;
                            i++;
                            while (i < x.Length && bracketCounter != 0)
                            {
                                if (x[i] == '(')
                                    bracketCounter++;
                                if (x[i] == ')')
                                    bracketCounter--;
                                i++;
                            }
                            OperatorsQuantity++;
                        }
                    }
                    //если '(' от оператора приведения типов или функции (т.е. перед ней идентификатор, а не операция)
                    if (i<x.Length && (i > 1 && x[i] == '(' && ((Char.IsSeparator(x[i - 1]) && (Char.IsLetterOrDigit(x[i - 2]) || x[i - 2] == '_')) || Char.IsLetter(x[i - 1]) || x[i - 1] == '_') || (i > 0 && x[i] == '(' && ((Char.IsLetterOrDigit(x[i - 1]) || x[i - 1] == '_')))))
                    {
                        bool IsCoertionOperator = false;//флаг "оператор приведения типов ли?"
                        int h = i;
                        while (h < x.Length)
                        {
                            //если после ) идентификатор, то это оператор приведения типа
                            if (x[h] == ')' && h < x.Length - 2 && ((Char.IsSeparator(x[h + 1]) && (Char.IsLetter(x[h + 2]) || x[h + 2] == '_')) || Char.IsLetter(x[h + 1]) || x[h + 1] == '_'))
                            {
                                IsCoertionOperator = true;
                                operators[49] = true;
                                i = h + 1;
                                OperatorsQuantity++;
                                break;
                            }
                            h++;
                        }
                        if (!IsCoertionOperator)//если функция
                        {
                            int j = i - 1;
                            string InvertedNameOfFunction = "", NameOfFunction = "";
                            while (j >= 0 && (Char.IsLetterOrDigit(x[j]) || x[j] == '_' || x[j] == '.'))
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
                                    if (q == NameOfFunction || (NameOfFunction.Length <= q.Length && q.Substring(q.Length - NameOfFunction.Length) == NameOfFunction && (q.Length - NameOfFunction.Length - 1 == 0 || q[q.Length - NameOfFunction.Length - 1] == '.')))
                                        IsFunctionWasMet = true;
                                if (!IsFunctionWasMet)//если функция не встречалась ранее, добавляем
                                    Functions.Add(NameOfFunction);
                            }
                        }
                    }
                    else if (i<x.Length && (Char.IsLetterOrDigit(x[i]) || x[i] == '_'))//отрулил шаблоны и ключевые слова
                    {
                        int h = i;
                        string Name = "";
                        while (h < x.Length && (Char.IsLetterOrDigit(x[h]) || x[h] == '_'))
                        {
                            Name += x[h];
                            h++;
                        }
                        i = h - 1;
                        bool IsTemplate = true;
                        int f = h;
                        if (f < x.Length && Char.IsSeparator(x[f]))
                            f++;
                        if (f < x.Length && x[f] == '<')
                        {
                            while (f < x.Length && x[f] != '>')
                            {
                                if (x[f] == '&' || x[f] == '|' || x[f] == '=' || x[f] == '!' || x[f] == '^')//если не шаблон
                                    IsTemplate = false;
                                f++;
                            }
                            if (f == x.Length)//если так и не встретили >
                                IsTemplate = false;
                        }
                        else
                            IsTemplate = false;
                        if (IsTemplate)//если это шаблон
                        {
                            while ( x[h] != '>') h++;
                            i = h;
                        }
                        //ключевые слова
                        else if (Name == "using" && (x[i] == '(' || x[i + 1] == '('))
                        {
                            operators[77] = true;
                            OperatorsQuantity++;
                        }
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
                    else if (i<x.Length && x[i] == '+')
                    {
                        if (x[i - 1] == '+' || x[i + 1] == '=') continue;//если конец ++ или это +=
                        else if (i+1 < x.Length && x[i + 1] == '+')
                        {
                            operators[7] = true;//использован инкремент
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '(' || x[i - 1] == '=' || (Char.IsSeparator(x[i - 1]) && (x[i - 2] == '(' || x[i - 2] == '=')))//если унарный плюс
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
                    else if (i < x.Length && x[i] == '-')
                    {
                        if (x[i - 1] == '-' || x[i + 1] == '=' || x[i + 1] == '>') continue;//если конец -- или это -=
                        else if (i+1 < x.Length && x[i + 1] == '-')
                        {
                            operators[8] = true;//использован декремент
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '(' || x[i - 1] == '=' || (Char.IsSeparator(x[i - 1]) && (x[i - 2] == '(' || x[i - 2] == '=')))//если унарный минус
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
                    else if (i < x.Length && x[i] == '*' && ((Char.IsSeparator(x[i - 1]) && (Char.IsLetterOrDigit(x[i - 2]) || x[i - 2] == '_' || x[i - 2] == ')')) || (Char.IsLetterOrDigit(x[i - 1]) || x[i - 1] == '_' || x[i - 1] == ')')))
                    //если умножение, а не разыменование
                    {
                        operators[4] = true;//использовано умножение
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '*' && x[i + 1] != '=')
                    //если разыменование
                    {
                        operators[65] = true;//использовано разыменование
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '/' && x[i + 1] != '=')
                    //если деление
                    {
                        operators[5] = true;//использовано деление
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '%' && x[i + 1] != '=')
                    {
                        operators[6] = true;//использован остаток от деления
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '=')
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
                        else if (x[i - 1] == '>' && x[i - 2] != '>')
                        {
                            operators[13] = true;//использовано >=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '<' && x[i - 2] != '<')
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
                        else if (x[i - 1] == '|')
                        {
                            operators[33] = true;//использовано |=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '&')
                        {
                            operators[32] = true;//использовано &=
                            OperatorsQuantity++;
                        }
                        else if (x[i - 1] == '^')
                        {
                            operators[31] = true;//использовано ^=
                            OperatorsQuantity++;
                        }
                        else if (i+1 < x.Length && x[i + 1] != '>' && x[i + 1] != '=')
                        {
                            operators[29] = true;//использовано =
                            OperatorsQuantity++;
                        }
                    }
                    else if (i < x.Length && x[i] == '>' && x[i + 1] != '=' && x[i - 1] != '-' && x[i + 1] != '>' && x[i - 1] != '>' && x[i - 1] != '=')//если знак больше
                    {
                        operators[11] = true;//использовано >
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '<' && x[i + 1] != '=' && x[i + 1] != '<' && x[i - 1] != '<')//если знак меньше
                    {
                        operators[12] = true;//использовано <
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '<' && x[i + 2] != '=' && x[i + 1] == '<')//если <<
                    {
                        operators[20] = true;//использовано <<
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '>' && x[i + 2] != '=' && x[i + 1] == '>')//если >>
                    {
                        operators[19] = true;//использовано >>
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '>' && x[i - 1] == '-')//если ->
                    {
                        operators[53] = true;//использовано ->
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '>' && x[i - 1] == '=')//если =>
                    {
                        operators[72] = true;//использовано =>
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '&')//если оперсанд
                    {
                        if (i > 0 && (x[i - 1] == '&' || x[i + 1] == '=')) continue;//если конец лог И или &=
                        else if (i+1 < x.Length && x[i + 1] == '&')
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
                    else if (i < x.Length && x[i] == '|')//если |
                    {
                        if (i > 0 && (x[i - 1] == '|' || x[i + 1] == '=')) continue;//если конец лог ИЛИ или |=
                        else if (i+1 < x.Length && x[i + 1] == '|')
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
                    else if (i < x.Length && x[i] == '^')//если xor
                    {
                        if (i > 0 && x[i + 1] == '=') continue;//если ^=
                        else
                        {
                            operators[23] = true;//использовано ^
                            OperatorsQuantity++;
                        }
                    }
                    else if (i < x.Length && x[i] == '!' && x[i + 1] != '=')
                    {
                        operators[18] = true;//использован !
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '~')
                    {
                        operators[24] = true;//использован ~
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == '.')
                    {
                        operators[51] = true;//использован .
                        OperatorsQuantity++;
                        if (x[i - 1] == '?')
                        {
                            operators[54] = true;//использован ?.
                            OperatorsQuantity++;
                        }
                    }
                    else if (i < x.Length && x[i] == '?')
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
                    else if (i < x.Length && x[i] == '[' && (Char.IsLetterOrDigit(x[i + 1]) || Char.IsLetterOrDigit(x[i + 2]) || x[i + 2] == '_' || x[i + 1] == '_'))
                    {
                        operators[52] = true;//использован []
                        OperatorsQuantity++;
                    }
                    else if (i < x.Length && x[i] == ':')
                    {
                        if (x[i + 1] == ':') continue;
                        else if (x[i - 1] == ':')
                        {
                            operators[63] = true;//использовано ::
                            OperatorsQuantity++;
                        }
                    }
                    else if (i < x.Length && x[i] == '{')
                    {
                        operators[62] = true;//использованы {}
                        OperatorsQuantity++;
                    }
                }
            }
            return OperatorsQuantity;
        }

        public static int FindAllOperands(List<string> Text, ref List<string> operands)
        {
            int OperandsQuantity = 0, i=0;
            int ind;
            string InvertedCurrentOperandName = "", CurrentOperandName = "";
            List<int> used;
            foreach (string x in Text)
            {
                used = new List<int>();
                i = 0;
                while (x.IndexOf("++", i)!=-1)//если есть инкремент
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("++", i) + 2;
                    while (Char.IsSeparator(x[ind]))
                        ind++;
                    if (Char.IsLetter(x[ind]) || x[ind]=='_')
                    {//префиксный
                        used.Add(ind);//добавление операнда как встреченного
                        OperandsQuantity++;
                        while (x[ind]=='_' || Char.IsLetterOrDigit(x[ind]))
                        {//пока символы операнда
                            CurrentOperandName += x[ind];
                            ind++;
                        }
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                    }
                    else
                    {//постфиксный
                        ind = x.IndexOf("++", i) - 1;
                        OperandsQuantity++;
                        if (Char.IsSeparator(x[ind]))
                            ind--;
                        while(ind>=0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        used.Add(ind+1);//добавление операнда как встреченного
                        for (int j = InvertedCurrentOperandName.Length-1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                    }
                    i = x.IndexOf("++", i) + 1;
                }
                i = 0;
                while (x.IndexOf("--", i) != -1)//если есть декремент
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("--", i) + 2;
                    while (Char.IsSeparator(x[ind]))
                        ind++;
                    if (Char.IsLetter(x[ind]) || x[ind] == '_')
                    {//префиксный
                        used.Add(ind);//добавление операнда как встреченного
                        OperandsQuantity++;
                        while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                        {//пока символы операнда
                            CurrentOperandName += x[ind];
                            ind++;
                        }
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                    }
                    else
                    {//постфиксный
                        ind = x.IndexOf("--", i) - 1;
                        OperandsQuantity++;
                        if (Char.IsSeparator(x[ind]))
                            ind--;
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        used.Add(ind + 1);//добавление операнда как встреченного
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                    }
                    i = x.IndexOf("--", i) + 1;
                }
                i = 0;
                while (x.IndexOf("+=", i) != -1)//если есть плюс-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("+=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind]=='(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while(ind<x.Length && BracketCounter!=0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind]=='"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind]=='"' && (x[ind-1]!='\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("+=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind>=0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName!="" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("+=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("-=", i) != -1)//если есть минус-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("-=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("-=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("-=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("*=", i) != -1)//если есть умножить-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("*=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("*=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("*=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("/=", i) != -1)//если есть делить-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("/=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("/=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("/=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("%=", i) != -1)//если есть процент-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("%=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("%=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("%=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("&=", i) != -1)//если встречено и-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("&=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("&=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("&=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("|=", i) != -1)//если есть или-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("|=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("|=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("|=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("^=", i) != -1)//если есть ксор-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("^=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("^=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("^=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("<<=", i) != -1)//если есть сдвиг влево-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<<=", i) + 3;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<<=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("<<=", i) + 1;
                }
                i = 0;
                while (x.IndexOf(">>=", i) != -1)//если есть сдвиг вправо-равно 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">>=", i) + 3;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">>=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf(">>=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("&&", i) != -1)//если есть логическое И 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("&&", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    if (x[ind] == ')')
                                        BracketCounter--;
                                    if (x[ind] == '(')
                                        BracketCounter++;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("&&", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("&&", i) + 1;
                }
                i = 0;
                while (x.IndexOf("||", i) != -1)//если есть логическое ИЛИ 
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("||", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("||", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("||", i) + 1;
                }
                i = 0;
                while (x.IndexOf(">=", i) != -1)//если есть больше-равно
                {
                    if ((x.IndexOf(">=", i)-1) == '>')//если сдвиг-равно
                    {
                        i = x.IndexOf(">=", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf(">=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("<=", i) != -1)//если есть меньше-равно
                {
                    if ((x.IndexOf("<=", i) - 1) == '<')//если сдвиг-равно
                    {
                        i = x.IndexOf("<=", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("<=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("!=", i) != -1)//если есть не равно
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("!=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"")|| CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("!=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '"')//если строковая константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("!=", i) + 1;
                }
                i = 0;
                while (x.IndexOf("==", i) != -1)//если есть равно
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("==", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("==", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '"')//если строковая константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("==", i) + 1;
                }
                i = 0;
                while (x.IndexOf("[", i) != -1)//если есть индексация
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("[", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    i = x.IndexOf("[", i) + 1;
                }
                i = 0;
                while (x.IndexOf("<<", i) != -1)//если есть сдвиг влево
                {
                    if ((x.IndexOf("<<", i) + 2) == '=')//если сдвиг-равно
                    {
                        i = x.IndexOf("<<", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<<", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<<", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("<<", i) + 1;
                }
                i = 0;
                while (x.IndexOf(">>", i) != -1)//если есть сдвиг влево
                {
                    if ((x.IndexOf(">>", i) + 2) == '=')//если сдвиг-равно
                    {
                        i = x.IndexOf(">>", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">>", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">>", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf(">>", i) + 1;
                }
                i = 0;
                while (x.IndexOf("~", i) != -1 && x.IndexOf("~", i) > 2 && ((x[x.IndexOf("~", i)-1] == '(') || (x[x.IndexOf("~", i) - 2] == '(') || (x[x.IndexOf("~", i) - 1] == '=') || (x[x.IndexOf("~", i) - 2] == '=')))//если есть побитовая инверсия, а не начало финализатора
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("~", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    i = x.IndexOf("~", i) + 1;
                }
                i = 0;
                while (x.IndexOf("!", i) != -1 && x[x.IndexOf("!", i)+1] != '=')//если есть отрицание
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("!", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    i = x.IndexOf("!", i) + 1;
                }
                i = 0;
                while (x.IndexOf("&", i) != -1)//если есть побитовое И
                {
                    if (x.IndexOf("&", i) + 1 == '=' || x.IndexOf("&", i) + 1 == '&' || x.IndexOf("&", i) - 1 == '&')//если И-равно или &&
                    {
                        i = x.IndexOf("&", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("&", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("&", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("&", i) + 1;
                }
                i = 0;
                while (x.IndexOf("|", i) != -1)//если есть побитовое ИЛИ
                {
                    if (x.IndexOf("|", i) + 1 == '=' || x.IndexOf("|", i) + 1 == '|' || x.IndexOf("|", i) - 1 == '|')//если ИЛИ-равно или ||
                    {
                        i = x.IndexOf("|", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("|", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("|", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("|", i) + 1;
                }
                i = 0;
                while (x.IndexOf("^", i) != -1)//если есть побитовый ксор
                {
                    if (x.IndexOf("^", i) + 1 == '=')//если ксор-равно
                    {
                        i = x.IndexOf("^", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("^", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("^", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("^", i) + 1;
                }
                i = 0;
                while (x.IndexOf("%", i) != -1)//если есть остаток от деления
                {
                    if (x.IndexOf("%", i) + 1 == '=')//если процент-равно
                    {
                        i = x.IndexOf("%", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("%", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("%", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("%", i) + 1;
                }
                i = 0;
                while (x.IndexOf("/", i) != -1)//если есть деление
                {
                    if (x.IndexOf("/", i) + 1 == '=')//если делить-равно
                    {
                        i = x.IndexOf("/", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("/", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("/", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("/", i) + 1;
                }
                i = 0;
                while (x.IndexOf("*", i) != -1)//если есть умножение
                {
                    if (x.IndexOf("*", i) + 1 == '=' || x.IndexOf("*", i) - 1 == '(' || (x.IndexOf("*", i)>1 && x.IndexOf("*", i) - 2 == '('))//если умножить-равно или разыменование
                    {
                        i = x.IndexOf("*", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("*", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("*", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("*", i) + 1;
                }
                i = 0;
                while (x.IndexOf("-", i) != -1)//если есть минус
                {
                    if (x.IndexOf("-", i) + 1 == '=' || x.IndexOf("-", i) - 1 == '-' || x.IndexOf("-", i) + 1 == '-')//если минус-равно или декремент
                    {
                        i = x.IndexOf("-", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("-", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("-", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("-", i) + 1;
                }
                i = 0;
                while (x.IndexOf("+", i) != -1)//если есть плюс
                {
                    if (x.IndexOf("+", i) + 1 == '=' || x.IndexOf("+", i) - 1 == '+' || x.IndexOf("+", i) + 1 == '+')//если плюс-равно или инкремент
                    {
                        i = x.IndexOf("+", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("+", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\'") || CurrentOperandName.Contains("\"")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("+", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '"')//если строковая константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\'") || CurrentOperandName.Contains("\"")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("+", i) + 1;
                }
                i = 0;
                while (x.IndexOf(">", i) != -1)//если есть больше
                {
                    if ((x.IndexOf(">", i) - 1) == '>' || (x.IndexOf(">", i) - 1) == '=' || (x.IndexOf(">", i) - 1) == '-' || (x.IndexOf(">", i) + 1) == '>' || (x.IndexOf(">", i) + 1) == '=')//если сдвиг-равно или >=, =>,->
                    {
                        i = x.IndexOf(">", i) + 1;
                        continue;
                    }
                    bool IsTemplate = true;
                    int f = x.IndexOf(">", i);
                    int FranchBracketCounter = 1;
                    f--;
                    while (f >=0 && FranchBracketCounter != 0)
                    {
                        if (x[f] == '<')
                            FranchBracketCounter--;
                        if (x[f] == '>')
                            FranchBracketCounter++;
                        if (x[f] == '&' || x[f] == '|' || x[f] == '=' || x[f] == '!' || x[f] == '^')//если не шаблон
                        {
                            IsTemplate = false;
                            break;
                        }
                        f--;
                    }
                    if (IsTemplate)//если это шаблон
                    {
                        i = x.IndexOf(">", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf(">", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf(">", i) + 1;
                }
                i = 0;
                while (x.IndexOf("<", i) != -1)//если есть меньше
                {
                    if ((x.IndexOf("<", i) - 1) == '<' || (x.IndexOf("<", i) + 1) == '<' || (x.IndexOf("<", i) + 1) == '=')//если сдвиг-равно или >=
                    {
                        i = x.IndexOf("<", i) + 1;
                        continue;
                    }
                    bool IsTemplate = true;
                    int f = x.IndexOf("<", i);
                    int FranchBracketCounter = 1;
                    f++;
                    while (f < x.Length && FranchBracketCounter != 0)
                    {
                        if (x[f] == '<')
                            FranchBracketCounter++;
                        if (x[f] == '>')
                            FranchBracketCounter--;
                        if (x[f] == '&' || x[f] == '|' || x[f] == '=' || x[f] == '!' || x[f] == '^')//если не шаблон
                        {
                            IsTemplate = false;
                            break;
                        }
                        f++;
                    }
                    if (IsTemplate)//если это шаблон
                    {
                        i = x.IndexOf("<", i) + 1;
                        continue;
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("<", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\'"))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("<", i) + 1;
                }
                i = 0;
                while (x.IndexOf("=", i) != -1)//если есть присвоение
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("=", i) + 1;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (CurrentOperandName == "new")//операнд -- новый объект
                            {
                                while (Char.IsSeparator(x[ind]))
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                                while (ind < x.Length && x[ind] != ';')
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else
                            {
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//функция
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && (CurrentOperandName.Contains("\"") || CurrentOperandName.Contains("\'")))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("=", i) + 1;
                }
                /*while (x.IndexOf("+=", i) != -1)//шаблон, есть всё
                {
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("+=", i) + 2;
                    while (Char.IsSeparator(x[ind]))//идём к правому операнду
                        ind++;
                    if (!used.Contains(ind))//если операнд ещё не посчитан
                    {
                        if (Char.IsLetter(x[ind]) || x[ind] == '_')//если это идентификатор или функция
                        {//правый операнд
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//функция
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        else if (Char.IsDigit(x[ind]))//если числовая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            while (Char.IsDigit(x[ind]))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '"')//если строковая константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        else if (x[ind] == '\'')//если символьная константа
                        {
                            used.Add(ind);//добавление операнда как встреченного
                            OperandsQuantity++;
                            CurrentOperandName += x[ind];//первая кавычка
                            ind++;
                            while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                            {//пока символы операнда
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                            CurrentOperandName += x[ind];//вторая кавычка
                            ind++;
                        }
                        if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\""))))
                            operands.Add(CurrentOperandName);
                    }
                    CurrentOperandName = "";
                    InvertedCurrentOperandName = "";
                    ind = x.IndexOf("+=", i) - 1;
                    if (Char.IsSeparator(x[ind]))
                        ind--;
                    if (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')//если это идентификатор или числовая константа
                    {//левый операнд
                        while (ind >= 0 && (x[ind] == '_' || Char.IsLetterOrDigit(x[ind])))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == ')')//если функция или скобка приоритета
                    {
                        int BracketCounter = 1;
                        InvertedCurrentOperandName += x[ind];//добавляем скобку
                        ind--;
                        while (ind >= 0 && BracketCounter != 0)//читаем всё, что в скобках
                        {
                            if (x[ind] == '(')
                                BracketCounter--;
                            if (x[ind] == ')')
                                BracketCounter++;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        if ((ind > 0 && (x[ind - 1] == '_' || Char.IsLetterOrDigit(x[ind - 1]))) || (ind > 0 && (x[ind - 2] == '_' || Char.IsLetterOrDigit(x[ind - 2]))))
                        {//скобка функции
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            while (x[ind] == '_' || Char.IsLetterOrDigit(x[ind]) || x[ind] == '.')//читаем имя функции
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                                CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!used.Contains(ind + 1))//операнд встречен впервые
                            {
                                used.Add(ind + 1);//добавление операнда как встреченного
                                OperandsQuantity++;
                            }
                            else
                                CurrentOperandName = "";
                        }
                    }
                    else if (x[ind] == '"')//если строковая константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '"' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    else if (x[ind] == '\'')//если символьная константа
                    {
                        InvertedCurrentOperandName += x[ind];//первая кавычка
                        ind--;
                        while (!(x[ind] == '\'' && (x[ind - 1] != '\\' || (x[ind - 1] == '\\' && x[ind - 2] == '\\'))))
                        {//пока символы операнда
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                        }
                        InvertedCurrentOperandName += x[ind];//вторая кавычка
                        ind--;
                        for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)
                            CurrentOperandName += InvertedCurrentOperandName[j];
                        if (!used.Contains(ind + 1))//операнд встречен впервые
                        {
                            used.Add(ind + 1);//добавление операнда как встреченного
                            OperandsQuantity++;
                        }
                        else
                            CurrentOperandName = "";
                    }
                    if (CurrentOperandName != "" && (!operands.Contains(CurrentOperandName) || (operands.Contains(CurrentOperandName) && CurrentOperandName.Contains("\""))))
                        operands.Add(CurrentOperandName);
                    i = x.IndexOf("+=", i) + 1;
                }*/
            }
            return OperandsQuantity;
        }
    }
}

/*
 for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == '+')
                    {
                        if (x[i - 1] == '+' || x[i + 1] == '=') continue;//если конец ++ или это +=
                        else if (x[i + 1] == '+')//если ++
                        {
                            if (x[i + 2] == ';' || (Char.IsSeparator(x[i + 2]) && x[i + 3] == ';'))//если постфиксная форма
                            {
                                int ind = i;
                                while (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//идём назад, пока не дошли до операнда
                                    ind--;
                                while ((ind >= 0) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.'))//Собираем инвертированное имя операнда
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";

                            }
                            else //префиксная форма
                            {
                                int ind = i;
                                while (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//идём назад, пока не дошли до операнда
                                    ind++;
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.'))//Собираем инвертированное имя операнда
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind++;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else if (x[i - 1] == '(' || x[i - 1] == '=' || (Char.IsSeparator(x[i - 1]) && (x[i - 2] == '(' || x[i - 2] == '=')))//если унарный плюс
                        {//унарный плюс
                            int ind = i;
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                        else//бинарный плюс
                        {
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '"')//встретили строку
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '"'))//чтоб концы строк отрулить
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '"')//встретили строку
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '"'))//чтоб концы строк отрулить
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '\'')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                    }
                    else if (x[i] == '-')
                    {
                        if (x[i - 1] == '-' || x[i + 1] == '=') continue;//если конец -- или это -=
                        else if (x[i + 1] == '-')//если --
                        {
                            if (x[i + 2] == ';' || (Char.IsSeparator(x[i + 2]) && x[i + 3] == ';'))//если постфиксная форма
                            {
                                int ind = i;
                                while (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//идём назад, пока не дошли до операнда
                                    ind--;
                                while ((ind >= 0) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.'))//Собираем инвертированное имя операнда
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                            else //префиксная форма
                            {
                                int ind = i;
                                while (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//идём назад, пока не дошли до операнда
                                    ind++;
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.'))//Собираем инвертированное имя операнда
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind++;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else if (x[i - 1] == '(' || x[i - 1] == '=' || (Char.IsSeparator(x[i - 1]) && (x[i - 2] == '(' || x[i - 2] == '=')))//если унарный минус
                        {//унарный минус
                            int ind = i;
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                        else//бинарный минус
                        {
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                    }
                    else if ((x[i] == '*' && ((Char.IsSeparator(x[i - 1]) && (Char.IsLetterOrDigit(x[i - 2]) || x[i - 2] == '_' || x[i - 2] == ')'))
                        || (Char.IsLetterOrDigit(x[i - 1]) || x[i - 1] == '_' || x[i - 1] == ')'))) || (x[i] == '/' && x[i + 1] != '=') ||
                        (x[i] == '%' && x[i + 1] != '=') || (x[i] == '>' && x[i + 1] != '=' && x[i - 1] != '-' && x[i + 1] != '>' &&
                        x[i - 1] != '>' && x[i - 1] != '=') || (x[i] == '<' && x[i + 1] != '=' && x[i + 1] != '<' && x[i - 1] != '<'))
                    //если *, /, %, > или <
                    {
                        int ind = i;//правый операнд
                        while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                            ind++;
                        if (x[ind] == '(')//сложный операнд
                        {
                            int BracketCounter1 = 1;
                            ind++;
                            while (BracketCounter1 != 0)
                            {
                                if (x[ind] == '(') BracketCounter1++;
                                if (x[ind] == ')') BracketCounter1--;
                                if (!Char.IsSeparator(x[ind]))
                                    CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//встретили символ
                        {
                            CurrentOperandName += x[ind];
                            ind++;
                            while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else//простой операнд
                        {
                            while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                            {
                                if (!Char.IsSeparator(x[ind]))
                                    CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//если операнд -- вызов функции
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter++;
                                    if (x[ind] == ')')
                                        BracketCounter--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                        if (PreviousOperandName != CurrentOperandName)
                            OperandsQuantity++;
                        PreviousOperandName = CurrentOperandName;
                        CurrentOperandName = "";
                        InvertedCurrentOperandName = "";

                        ind = i - 1;//левый операнд
                        while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                            ind--;
                        if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                        {
                            InvertedCurrentOperandName = "";
                            int BracketCounter = 1;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                            while (BracketCounter != 0)
                            {
                                if (x[ind] == '(')
                                    BracketCounter--;
                                if (x[ind] == ')')
                                    BracketCounter++;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                            {
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                            else//если функция
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else if (x[ind] == '\'')//встретили символ
                        {
                            CurrentOperandName += x[ind];
                            ind--;
                            while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                            }
                        }
                        else//простой операнд
                        {
                            while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                    CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                    }
                    else if (x[i] == '=')
                    {
                        if (x[i - 1] == '+' || x[i - 1] == '-' || x[i - 1] == '*' || x[i - 1] == '/' || x[i - 1] == '%' ||
                           (x[i - 1] == '<' && x[i - 2] == '<') || (x[i - 1] == '>' && x[i - 2] == '>') || x[i - 1] == '|' || x[i - 1] == '&'
                           || x[i - 1] == '^')
                        {//если встречен двухсимвольный оператор присваивания
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '"')//встретили строку
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '"'))//чтоб концы строк отрулить
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 2;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                    CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                        else if (x[i - 1] == '=' || x[i - 1] == '!' || x[i - 1] == '>' || x[i - 1] == '<')
                        {//если встречено двухсимвольное сравнение
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '"')//встретили строку
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '"'))//чтоб концы строк отрулить
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 2;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else if (x[i + 1] != '>' && x[i + 1] != '=')
                        {//встречено обычное присваивание
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '"')//встретили строку
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '"'))//чтоб концы строк отрулить
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                    CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                    }
                    else if ((x[i] == '<' && x[i + 2] != '=' && x[i + 1] == '<') || (x[i] == '>' && x[i + 2] != '=' && x[i + 1] == '>'))//если <<, >>
                    {
                        int ind = i + 1;//правый операнд
                        while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                            ind++;
                        if (x[ind] == '(')//сложный операнд
                        {
                            int BracketCounter1 = 1;
                            ind++;
                            while (BracketCounter1 != 0)
                            {
                                if (x[ind] == '(') BracketCounter1++;
                                if (x[ind] == ')') BracketCounter1--;
                                if (!Char.IsSeparator(x[ind]))
                                    CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else if (x[ind] == '\'')//встретили символ
                        {
                            CurrentOperandName += x[ind];
                            ind++;
                            while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else//простой операнд
                        {
                            while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                            {
                                if (!Char.IsSeparator(x[ind]))
                                    CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//если операнд -- вызов функции
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter++;
                                    if (x[ind] == ')')
                                        BracketCounter--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                        if (PreviousOperandName != CurrentOperandName)
                            OperandsQuantity++;
                        PreviousOperandName = CurrentOperandName;
                        CurrentOperandName = "";
                        InvertedCurrentOperandName = "";

                        ind = i - 1;//левый операнд
                        while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                            ind--;
                        if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                        {
                            InvertedCurrentOperandName = "";
                            int BracketCounter = 1;
                            InvertedCurrentOperandName += x[ind];
                            ind--;
                            while (BracketCounter != 0)
                            {
                                if (x[ind] == '(')
                                    BracketCounter--;
                                if (x[ind] == ')')
                                    BracketCounter++;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind--;
                            if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                            {
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                            else//если функция
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else if (x[ind] == '\'')//встретили символ
                        {
                            CurrentOperandName += x[ind];
                            ind--;
                            while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                            }
                        }
                        else//простой операнд
                        {
                            while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                            {
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                            }
                            for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                    CurrentOperandName += InvertedCurrentOperandName[j];
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                    }
                    else if (x[i] == '&')//если оперсанд
                    {
                        if (i > 0 && (x[i - 1] == '&' || x[i + 1] == '=')) continue;//если конец лог И или &=
                        else if (i < x.Length && x[i + 1] == '&')
                        {//если встречено &&
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else
                        {//побитовое И
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                    }
                    else if (x[i] == '|')//если |
                    {
                        if (i > 0 && (x[i - 1] == '|' || x[i + 1] == '=')) continue;//если конец лог ИЛИ или |=
                        else if (i < x.Length && x[i + 1] == '|')
                        {//если ||
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                        else
                        {//если |
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                    }
                    else if (x[i] == '^')//если xor
                    {
                        if (i > 0 && x[i + 1] == '=') continue;//если ^=
                        else
                        {//если^
                            int ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";

                            ind = i - 1;//левый операнд
                            while (ind >= 0 && Char.IsSeparator(x[ind]))//идём, пока не дошли до операнда
                                ind--;
                            if (x[ind] == ')')//проверка, от функции скобка или скобка приоритета
                            {
                                InvertedCurrentOperandName = "";
                                int BracketCounter = 1;
                                InvertedCurrentOperandName += x[ind];
                                ind--;
                                while (BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter--;
                                    if (x[ind] == ')')
                                        BracketCounter++;
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind--;
                                if (!Char.IsLetterOrDigit(x[ind]) && x[ind] != '_')//если это скобка приоритета
                                {
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                                else//если функция
                                {
                                    while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '.')
                                    {
                                        InvertedCurrentOperandName += x[ind];
                                        ind--;
                                    }
                                    for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                        if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                            CurrentOperandName += InvertedCurrentOperandName[j];
                                    if (!operands.Contains(CurrentOperandName))
                                        operands.Add(CurrentOperandName);
                                    if (PreviousOperandName != CurrentOperandName)
                                        OperandsQuantity++;
                                    PreviousOperandName = CurrentOperandName;
                                    CurrentOperandName = "";
                                    InvertedCurrentOperandName = "";
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind--;
                                while (!(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind--;
                                }
                            }
                            else//простой операнд
                            {
                                while (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_')
                                {
                                    InvertedCurrentOperandName += x[ind];
                                    ind--;
                                }
                                for (int j = InvertedCurrentOperandName.Length - 1; j >= 0; j--)//инвертируем имя
                                    if (!Char.IsSeparator(InvertedCurrentOperandName[j]))
                                        CurrentOperandName += InvertedCurrentOperandName[j];
                                if (!operands.Contains(CurrentOperandName))
                                    operands.Add(CurrentOperandName);
                                if (PreviousOperandName != CurrentOperandName)
                                    OperandsQuantity++;
                                PreviousOperandName = CurrentOperandName;
                                CurrentOperandName = "";
                                InvertedCurrentOperandName = "";
                            }
                        }
                    }
                    else if (x[i] == '!' && x[i + 1] != '=')
                    {//если логическое отрицание
                        int ind = i;//правый операнд
                        while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                            ind++;
                        if (x[ind] == '(')//сложный операнд
                        {
                            int BracketCounter1 = 1;
                            ind++;
                            while (BracketCounter1 != 0)
                            {
                                if (x[ind] == '(') BracketCounter1++;
                                if (x[ind] == ')') BracketCounter1--;
                                if (!Char.IsSeparator(x[ind]))
                                    CurrentOperandName += x[ind];
                                ind++;
                            }
                        }
                        else//простой операнд
                        {
                            while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                            {
                                if (!Char.IsSeparator(x[ind]))
                                    CurrentOperandName += x[ind];
                                ind++;
                            }
                            if (Char.IsSeparator(x[ind]))
                                ind++;
                            if (x[ind] == '(')//если операнд -- вызов функции
                            {
                                int BracketCounter = 1;
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && BracketCounter != 0)
                                {
                                    if (x[ind] == '(')
                                        BracketCounter++;
                                    if (x[ind] == ')')
                                        BracketCounter--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                        }
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                        if (PreviousOperandName != CurrentOperandName)
                            OperandsQuantity++;
                        PreviousOperandName = CurrentOperandName;
                        CurrentOperandName = "";
                        InvertedCurrentOperandName = "";
                    }
                    else if (x[i] == '~')//отрулить деструктор
                    {
                        int ind = i - 1;
                        bool IsDestructor = true;
                        while (ind >= 0)
                        {
                            if (!Char.IsSeparator(x[ind]))
                                IsDestructor = false;
                            ind--;
                        }
                        if (!IsDestructor)
                        {
                            ind = i;//правый операнд
                            while (ind < x.Length && !Char.IsLetterOrDigit(x[ind]) && x[ind] != '_' && x[ind] != '(' && x[ind] != '"' && x[ind] != '\'')//идём, пока не дошли до операнда
                                ind++;
                            if (x[ind] == '(')//сложный операнд
                            {
                                int BracketCounter1 = 1;
                                ind++;
                                while (BracketCounter1 != 0)
                                {
                                    if (x[ind] == '(') BracketCounter1++;
                                    if (x[ind] == ')') BracketCounter1--;
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else if (x[ind] == '\'')//встретили символ
                            {
                                CurrentOperandName += x[ind];
                                ind++;
                                while (ind < x.Length && !(x[ind - 2] != '\\' && x[ind - 1] == '\''))//собираем символ
                                {
                                    CurrentOperandName += x[ind];
                                    ind++;
                                }
                            }
                            else//простой операнд
                            {
                                while ((ind < x.Length) && (Char.IsLetterOrDigit(x[ind]) || x[ind] == '_' || x[ind] == '"' || x[ind] == '.'))//Собираем имя операнда
                                {
                                    if (!Char.IsSeparator(x[ind]))
                                        CurrentOperandName += x[ind];
                                    ind++;
                                }
                                if (Char.IsSeparator(x[ind]))
                                    ind++;
                                if (x[ind] == '(')//если операнд -- вызов функции
                                {
                                    int BracketCounter = 1;
                                    CurrentOperandName += x[ind];
                                    ind++;
                                    while (ind < x.Length && BracketCounter != 0)
                                    {
                                        if (x[ind] == '(')
                                            BracketCounter++;
                                        if (x[ind] == ')')
                                            BracketCounter--;
                                        if (!Char.IsSeparator(x[ind]))
                                            CurrentOperandName += x[ind];
                                        ind++;
                                    }
                                }
                            }
                            if (!operands.Contains(CurrentOperandName))
                                operands.Add(CurrentOperandName);
                            if (PreviousOperandName != CurrentOperandName)
                                OperandsQuantity++;
                            PreviousOperandName = CurrentOperandName;
                            CurrentOperandName = "";
                            InvertedCurrentOperandName = "";
                        }
                    }
                    else if (x[i] == '[' && (Char.IsLetterOrDigit(x[i + 1]) || Char.IsLetterOrDigit(x[i + 2]) || x[i + 2] == '_' || x[i + 1] == '_'))
                    {
                        int ind = i + 1;
                        while (x[ind] != ']')//Собираем имя операнда
                        {
                            CurrentOperandName += x[ind];
                            ind++;
                        }
                        if (!operands.Contains(CurrentOperandName))
                            operands.Add(CurrentOperandName);
                        if (PreviousOperandName != CurrentOperandName)
                            OperandsQuantity++;
                        PreviousOperandName = CurrentOperandName;
                        CurrentOperandName = "";
                        InvertedCurrentOperandName = "";
                    }
                }
     */
