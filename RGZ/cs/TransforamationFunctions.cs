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
            int CycleSearcher = 0;//если равна 3, то мы в цикле for
            string Temp = "";
            foreach (string str in Text)
            {
                foreach (char x in str)
                {
                    if (CycleSearcher == 0)//Ищем фор
                    {
                        if (x == 'f')
                            CycleSearcher++;
                    }
                    else if (CycleSearcher == 1)
                        if (x == 'o')
                            CycleSearcher++;
                        else
                            CycleSearcher--;
                    else if (CycleSearcher == 2)
                        if (x == 'r')
                            CycleSearcher++;
                        else
                            CycleSearcher = 0;
                    if (x == ')')
                        CycleSearcher = 0;
                    if (x != '{')
                        Temp += x;
                    if ((x == ';' || x == '{' || x == '}') && CycleSearcher != 3)
                    {
                        result.Add(Temp);
                        Temp = "";
                        if (x == '{')//чтобы фигурную скобку отделить
                        {
                            Temp += x;
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
    }
}
