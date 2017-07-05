using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGZ.cs
{
    class TransforamationFunctions
    {
        public static List<string> ModuleMinimizing(List<string> Text)//обработка от лишних пробелов и пустых строк
        {
            //всё работает
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
                        j--;
                    }
                    else
                        Temp += Text[i][j];
                }
                if (Temp != "" && !(Temp.Length == 1 && Char.IsSeparator(Temp[0])))
                {
                    result.Add(Temp);
                }
            }
            return result;
        }
        public static List<string> OneOperatorOneString(List<string> Text)
        {
            List<string> result = new List<string>();
            int CycleSearcher = 0;//если равна 5, то мы в цикле for
            string Temp = "";
            foreach (string str in Text)
            {
                for (int i = 0; i<str.Length; i++)
                {
                    if (CycleSearcher == 0)//Ищем фор
                    {
                        if (Char.IsSeparator(str[i]))
                            CycleSearcher++;
                        else if (i == 0 && str[i] == 'f')
                            CycleSearcher += 2;
                    }
                    else if (CycleSearcher == 1)
                    {
                        if (str[i] == 'f')
                            CycleSearcher++;
                        else
                            CycleSearcher--;
                    }
                    else if (CycleSearcher == 2)
                        if (str[i] == 'o')
                            CycleSearcher++;
                        else
                            CycleSearcher-=2;
                    else if (CycleSearcher == 3)
                        if (str[i] == 'r')
                            CycleSearcher++;
                        else
                            CycleSearcher = 0;
                    else if (CycleSearcher==4)
                    {
                        if (!Char.IsLetterOrDigit(str[i]) && str[i] != '_')
                            CycleSearcher++;
                        else
                            CycleSearcher = 0;
                    }

                    if (str[i] == ')')
                        CycleSearcher = 0;
                    if (str[i] != '{')
                        Temp += str[i];
                    if ((str[i] == ';' || str[i] == '{' || str[i] == '}') && CycleSearcher != 5)
                    {
                        result.Add(Temp);
                        Temp = "";
                        if (str[i] == '{')//чтобы фигурную скобку отделить
                        {
                            Temp += str[i];
                            result.Add(Temp);
                            Temp = "";
                        }
                    }
                }
            }
            return result;
        }
        public static List<string> ModuleProcessing(List<string> Text)
        {
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
                        if (Text[i][j] == '"' && (j == 0 || (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\'))))
                        {
                            Temp += Text[i][j];
                            j++;
                            IsCarretIntoConstString = true;
                            while (j < Text[i].Length && !(Text[i][j] == '"' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\'))))
                            {
                                j++;
                            }
                            if (j < Text[i].Length && j > 0 && Text[i][j] == '"' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\')))
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
                            while (j < Text[i].Length && !(Text[i][j] == '\'' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\'))))
                            {
                                j++;
                            }
                            if (j < Text[i].Length && Text[i][j] == '\'' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\')))
                            {
                                Temp += Text[i][j];
                                IsCarretIntoConstChar = false;
                                j++;
                            }
                        }
                    }
                    if (IsCarretIntoConstString)
                    {
                        while (j < Text[i].Length && !(Text[i][j] == '"' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\'))))
                        {
                            j++;
                        }
                        if (j < Text[i].Length && j > 0 && Text[i][j] == '"' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\')))
                        {
                            Temp += Text[i][j];
                            IsCarretIntoConstString = false;
                            j++;
                        }
                    }
                    if (IsCarretIntoConstChar)
                    {
                        while (j < Text[i].Length && !(Text[i][j] == '\'' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\'))))
                        {
                            j++;
                        }
                        if (j < Text[i].Length && Text[i][j] == '\'' && (Text[i][j - 1] != '\\' || (Text[i][j - 1] == '\\' && Text[i][j - 2] == '\\')))
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
            }
            return result;
        }

        public static List<string> RemoveComments(List<string> Text, ref int CommentsQuantity)//считает и удаляет комментарии
        {
            //Считается, сколько строк содержит комментарий
            List<string> Result = new List<string>();
            int IsMultyStringComment = 0;
            string Temp = "";
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
            }
            return Result;
        }

        public static List<string> RestorationOfNameFromNickname(List<string> Text)//восстановление псевдонимов
        {
            List<string> Result = new List<string>();
            int i = 0;
            string Key = "", Value = "";
            string Temp = "";
            Dictionary<string, string> NicknameOldnameDictionary = new Dictionary<string, string>();//ключ псевдоним, значение стандартное имя
            while (i != Text.Count)
            {
                Key = "";
                Value = "";
                if (Text[i].Contains("using") && Text[i].IndexOf('=') != -1 && Text[i].IndexOf('(') == -1)//если юзинг оператор, а не директива.
                {
                    int j = Text[i].IndexOf('g') + 2;
                    while (!Char.IsSeparator(Text[i][j]))
                    {
                        Key += Text[i][j];
                        j++;
                    }
                    j = Text[i].IndexOf('=') + 2;
                    while (j < Text[i].Length && (Char.IsLetterOrDigit(Text[i][j]) || Text[i][j] == '_'))
                    {
                        Value += Text[i][j];
                        j++;
                    }
                    if (!NicknameOldnameDictionary.ContainsKey(Key))
                        NicknameOldnameDictionary.Add(Key, Value);//составлен словарь псевдонимов
                }
                i++;
            }
            for (int j = 0; j < Text.Count; j++)
            {
                Temp = Text[j];
                foreach (string x in NicknameOldnameDictionary.Keys)
                {
                    for (int k = 0; k < Temp.Length; k++)
                    {
                        int index = Temp.IndexOf(x, k);
                        if (index == -1) break;
                        if (index != -1)//если в строке есть ключ
                        {
                            if ((index == 0 || (Temp[index - 1] == '.' || Char.IsSeparator(Temp[index - 1]))) && (Temp[index + x.Length] == '.' || Char.IsSeparator(Temp[index + x.Length])))
                            {
                                Temp = Temp.Remove(index, x.Length);
                                Temp = Temp.Insert(index, NicknameOldnameDictionary[x]);
                            }
                        }
                    }
                }
                Result.Add(Temp);
            }
            return Result;
        }

        public static List<string> DeleteAllFunctionHeader(List<string> Text)//обработка от лишних пробелов и пустых строк
        { return null; }

        public static List<string> ConditionParser(List<string> Text)
        {
            List<string> Result = new List<string>();
            string Temp = "";
            for (int j=0; j<Text.Count;j++)
            {
                int i = 0;
                while (i < Text[j].Length)
                {
                    if (Text[j].IndexOf(" if ", i)!=-1 || Text[j].IndexOf("if ", i) == i || Text[j].IndexOf(" if(", i) != -1 || Text[j].IndexOf("if(", i) == i || 
                        Text[j].IndexOf(" for ", i) != -1 || Text[j].IndexOf("for ", i) == i || Text[j].IndexOf(" for(", i) != -1 || Text[j].IndexOf("for(", i) == i || 
                        Text[j].IndexOf(" while ", i) != -1 || Text[j].IndexOf("while ", i) == i || Text[j].IndexOf(" while(", i) != -1 || Text[j].IndexOf("while(", i) == i 
                        || Text[j].IndexOf(" foreach ", i) != -1 || Text[j].IndexOf("foreach ", i) == i || Text[j].IndexOf(" foreach(", i) != -1 ||
                        Text[j].IndexOf("foreach(", i) == i)//встречена конструкция
                    {
                        int bracketCounter = 1;
                        while (Text[j][i] != '(')
                        {
                            Temp += Text[j][i];
                            i++;
                        }
                        Temp += Text[j][i];
                        i++;
                        while (i < Text[j].Length && bracketCounter != 0)
                        {
                            if (Text[j][i] == '(')
                                bracketCounter++;
                            if (Text[j][i] == ')')
                                bracketCounter--;
                            Temp += Text[j][i];
                            i++;
                        }
                        Result.Add(Temp);
                        Temp = "";
                        while (i < Text[j].Length && Char.IsSeparator(Text[j][i]))
                            i++;
                    }
                    else if (Text[j].IndexOf(" else ", i) != -1 || Text[j].IndexOf("else ", i) == i ||(Text[j].IndexOf(" else", i) == Text[j].Length - 5 && 
                        Text[j].Contains(" else")) || (Text[j].IndexOf("else", i) == i && Text[j].Length == 4)) //встречен элс
                    {
                        while (Char.IsSeparator(Text[j][i]))
                        {
                            Temp += Text[j][i];
                            i++;
                        }
                        while (i<Text[j].Length && !Char.IsSeparator(Text[j][i]))
                        {
                            Temp += Text[j][i];
                            i++;
                        }
                        Result.Add(Temp);
                        Temp = "";
                        while (i < Text[j].Length && Char.IsSeparator(Text[j][i]))
                            i++;
                    }
                    else//дописали остаток
                    {
                        while (i < Text[j].Length)
                        {
                            Temp += Text[j][i];
                            i++;
                        }
                        Result.Add(Temp);
                        Temp = "";
                    }
                }
            }
            return Result;
        }
        public static List<string> ConditionParser2(List<string> Text)//добавление в конструкции передачи управления {}, где их нет
        {
            List<string> Result = new List<string>();
            Stack<Stack<bool>> ConstructionStacks = new Stack<Stack<bool>>();
            ConstructionStacks.Push(new Stack<bool>());
            for (int j = 0; j < Text.Count; j++)
            {
                if (Text[j].IndexOf(" if ") != -1 || Text[j].IndexOf("if ") == 0 || Text[j].IndexOf(" if(") != -1 || Text[j].IndexOf("if(") == 0)//встречен иф
                {
                    Result.Add(Text[j]);
                    if (!Text[j + 1].Contains("{") && (Text[j + 1].IndexOf(" if ") != -1 || Text[j + 1].IndexOf("if ") == 0 || Text[j + 1].IndexOf(" if(") != -1 || 
                        Text[j + 1].IndexOf("if(") == 0 || Text[j + 1].IndexOf(" for ") != -1 || Text[j + 1].IndexOf("for ") == 0 || Text[j + 1].IndexOf(" for(") != -1 || 
                        Text[j + 1].IndexOf("for(") == 0 || Text[j + 1].IndexOf(" while ") != -1 || Text[j + 1].IndexOf("while ") == 0 || Text[j + 1].IndexOf(" while(") != -1
                        || Text[j + 1].IndexOf("while(") == 0 || Text[j + 1].IndexOf(" foreach ") != -1 || Text[j + 1].IndexOf("foreach ") == 0 || 
                        Text[j + 1].IndexOf(" foreach(") != -1 || Text[j + 1].IndexOf("foreach(") == 0 || Text[j + 1].IndexOf(" else ") != -1 || 
                        Text[j + 1].IndexOf("else ") == 0 || (Text[j + 1].IndexOf(" else") == Text[j + 1].Length - 5 && Text[j + 1].Contains(" else")) ||
                        (Text[j + 1].IndexOf("else") == 0 && Text[j + 1].Length == 4)))
                    {
                        Result.Add("{");
                        ConstructionStacks.Peek().Push(true);
                    }
                }
                else if (Text[j].IndexOf(" for ") != -1 || Text[j].IndexOf("for ") == 0 || Text[j].IndexOf(" for(") != -1 || Text[j].IndexOf("for(") == 0 ||
                        Text[j].IndexOf(" while ") != -1 || Text[j].IndexOf("while ") == 0 || Text[j].IndexOf(" while(") != -1 || Text[j].IndexOf("while(") == 0
                        || Text[j].IndexOf(" foreach ") != -1 || Text[j].IndexOf("foreach ") == 0 || Text[j].IndexOf(" foreach(") != -1 ||
                        Text[j].IndexOf("foreach(") == 0 || Text[j].IndexOf(" else ") != -1 || Text[j].IndexOf("else ") == 0 || (Text[j].IndexOf(" else") == Text[j].Length - 5
                        && Text[j].Contains(" else")) || (Text[j].IndexOf("else") == 0 && Text[j].Length == 4))//встречена другая конструкция
                {
                    Result.Add(Text[j]);
                    if (!Text[j + 1].Contains("{") && (Text[j + 1].IndexOf(" if ") != -1 || Text[j + 1].IndexOf("if ") == 0 || Text[j + 1].IndexOf(" if(") != -1 ||
                        Text[j + 1].IndexOf("if(") == 0 || Text[j + 1].IndexOf(" for ") != -1 || Text[j + 1].IndexOf("for ") == 0 || Text[j + 1].IndexOf(" for(") != -1 ||
                        Text[j + 1].IndexOf("for(") == 0 || Text[j + 1].IndexOf(" while ") != -1 || Text[j + 1].IndexOf("while ") == 0 ||
                        Text[j + 1].IndexOf(" while(") != -1 || Text[j + 1].IndexOf("while(") == 0 || Text[j + 1].IndexOf(" foreach ") != -1 ||
                        Text[j + 1].IndexOf("foreach ") == 0 || Text[j + 1].IndexOf(" foreach(") != -1 || Text[j + 1].IndexOf("foreach(") == 0 ||
                        Text[j + 1].IndexOf(" else ") != -1 || Text[j + 1].IndexOf("else ") == 0 || (Text[j + 1].IndexOf(" else") == Text[j + 1].Length - 5 && 
                        Text[j + 1].Contains(" else")) || (Text[j + 1].IndexOf("else") == 0 && Text[j + 1].Length == 4)))
                    {
                        Result.Add("{");
                        ConstructionStacks.Peek().Push(false);
                    }
                }
                else if (Text[j].Contains("{"))//если встречена фигурная скобка
                {
                    Result.Add("{");
                    ConstructionStacks.Push(new Stack<bool>());//новый уровень вложенности, новый стэк
                }
                else if (Text[j].Contains("}"))//если встречена фигурная скобка
                {
                    Result.Add("}");
<<<<<<< HEAD
                    while (ConstructionStacks.Count>0 && ConstructionStacks.Peek().Count != 0)//выгружаем весь стэк
=======
                    while (ConstructionStacks.Peek().Count != 0)//выгружаем весь стэк
>>>>>>> 41e307b6b8ba7726a0302919b40a6593bdb90acd
                    {
                        Result.Add("}");
                        ConstructionStacks.Peek().Pop();
                    }
<<<<<<< HEAD
                    if (ConstructionStacks.Count > 0)
                        ConstructionStacks.Pop();//и удаляем
=======
                    ConstructionStacks.Pop();//и удаляем
>>>>>>> 41e307b6b8ba7726a0302919b40a6593bdb90acd
                }
                else//если простая инструкция 
                {
                    if (j>0 && (Text[j - 1].IndexOf(" if ") != -1 || Text[j - 1].IndexOf("if ") == 0 || Text[j - 1].IndexOf(" if(") != -1 ||
                        Text[j - 1].IndexOf("if(") == 0 || Text[j - 1].IndexOf(" for ") != -1 || Text[j - 1].IndexOf("for ") == 0 || Text[j - 1].IndexOf(" for(") != -1 ||
                        Text[j - 1].IndexOf("for(") == 0 || Text[j - 1].IndexOf(" while ") != -1 || Text[j - 1].IndexOf("while ") == 0 ||
                        Text[j - 1].IndexOf(" while(") != -1 || Text[j - 1].IndexOf("while(") == 0 || Text[j - 1].IndexOf(" foreach ") != -1 ||
                        Text[j - 1].IndexOf("foreach ") == 0 || Text[j - 1].IndexOf(" foreach(") != -1 || Text[j - 1].IndexOf("foreach(") == 0 ||
                        Text[j - 1].IndexOf(" else ") != -1 || Text[j - 1].IndexOf("else ") == 0 || (Text[j - 1].IndexOf(" else") == Text[j - 1].Length - 5 && Text[j - 1].Contains(" else")) ||
                        (Text[j - 1].IndexOf("else") == 0 && Text[j - 1].Length == 4)))//Одна инструкция и перед ней что-то из конструкций
                    {
                        Result.Add("{");
                        Result.Add(Text[j]);
                        Result.Add("}");
                        if (j<Text.Count-1 && (Text[j + 1].IndexOf(" else ") != -1 || Text[j + 1].IndexOf("else ") == 0 || (Text[j + 1].IndexOf(" else") == Text[j + 1].Length - 5 && Text[j + 1].Contains(" else")) ||
                        (Text[j + 1].IndexOf("else") == 0 && Text[j + 1].Length == 4)) && !(Text[j - 1].IndexOf(" if ") != -1) && !(Text[j - 1].IndexOf("if ") == 0) &&
                        !(Text[j - 1].IndexOf(" if(") != -1) &&
                        !(Text[j - 1].IndexOf("if(") == 0))
                        {//в предыдущей строке не условие, а в следующей элс
                            while (!ConstructionStacks.Peek().Peek())
                            {
                                Result.Add("}");
                                ConstructionStacks.Peek().Pop();
                            }
                            Result.Add("}");
                            ConstructionStacks.Peek().Pop();
                        }
                        else if (j < Text.Count - 1 && (Text[j + 1].IndexOf(" else ") != -1 || Text[j + 1].IndexOf("else ") == 0 || (Text[j + 1].IndexOf(" else") == Text[j + 1].Length - 5 && Text[j + 1].Contains(" else")) ||
                        (Text[j + 1].IndexOf("else") == 0 && Text[j + 1].Length == 4)) && ((Text[j - 1].IndexOf(" if ") != -1) || (Text[j - 1].IndexOf("if ") == 0) ||
                        (Text[j - 1].IndexOf(" if(") != -1) || (Text[j - 1].IndexOf("if(") == 0))) {/*ничего не нужно делать*/}
                        else//если следующее не элс
                        {
                            while (ConstructionStacks.Peek().Count != 0)
                            {
                                Result.Add("}");
                                ConstructionStacks.Peek().Pop();
                            }
                        }
                    }
                    else//если инструкция после инструкции
                    {
                        Result.Add(Text[j]);
                        if (j<Text.Count-1 && (Text[j + 1].IndexOf(" else ") != -1 || Text[j + 1].IndexOf("else ") == 0 || (Text[j + 1].IndexOf(" else") == Text[j + 1].Length - 5 && Text[j + 1].Contains(" else")) ||
                        (Text[j + 1].IndexOf("else") == 0 && Text[j + 1].Length == 4)))//если следующая строка элс
                        {
                            while (!ConstructionStacks.Peek().Peek())
                            {
                                Result.Add("}");
                                ConstructionStacks.Peek().Pop();
                            }
                            Result.Add("}");
                            ConstructionStacks.Peek().Pop();
                        }
                    }
                }
            }
            return Result;
        }
    }
}
