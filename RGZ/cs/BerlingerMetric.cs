using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RGZ.cs
{
    class BerlingerMetric
    {
        public static double CalculationBerlingerMetric(List<string> Files)
        {
            List<string> ModuleText = new List<string>();
            double result = 0, frequency = 0, probability = 0;
            int[] Symbols = new int[129];//используются индексы с 1 по 128
            for (int i = 0; i < 129; i++)//зануляем массив
                Symbols[i] = 0;
            int SymbolsQuantity = 0, QuantityOfUniqueSymbols = 0;

            foreach (string x in Files)//идём по файлам
            {
                FileStream fs = new FileStream(x, FileMode.Open, FileAccess.Read);//создаём файловый поток
                StreamReader sr = new StreamReader(fs);//создаём читателя
                while (!sr.EndOfStream)//читаем файл до конца
                {
                    ModuleText.Add(sr.ReadLine());
                }
                int CommentQuantity = 0;
                ModuleText = cs.TransforamationFunctions.RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = cs.TransforamationFunctions.ModuleMinimizing(ModuleText);//обработка модуля от лишних строк и разделителей

                foreach (string str in ModuleText)//идём по строкам
                    foreach (Char Ch in str)//идём по столбцам
                        if ((int)Ch < 129)//что-бы не считать симвлы национальных алфавитов и т.д.
                        {
                            if (Symbols[(int)Ch] == 0)//считаем, какие символы встретились
                                QuantityOfUniqueSymbols++;
                            Symbols[(int)Ch]++;
                            SymbolsQuantity++;
                        }
                sr.Close();
                fs.Close();
            }
            for (int i = 1; i < 129; i++)
            {
                if (Symbols[i] != 0)
                {
                    frequency = (double)Symbols[i] / (double)SymbolsQuantity;
                    probability = 1.0 / (double)QuantityOfUniqueSymbols;
                    result += frequency * Math.Log(probability, 2.0);//мера Берлингера
                }
            }
            return result;
        }
    }
}
