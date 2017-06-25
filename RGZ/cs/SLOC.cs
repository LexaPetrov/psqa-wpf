using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RGZ.cs
{
    class SLOC
    {
        public static double[] CalculationSLOCMetric(List<string> Files)
        {
            List<string> ModuleText = new List<string>();
            double[] result = new double[2];
            double PhysicalLine = 0;
            double LogicalLine = 0;

            foreach (string x in Files)//идём по файлам
            {
                FileStream fs = new FileStream(x, FileMode.Open, FileAccess.Read);//создаём файловый поток
                StreamReader sr = new StreamReader(fs);//создаём читателя
                ModuleText = new List<string>();
                while (!sr.EndOfStream)//читаем файл до конца
                {
                    ModuleText.Add(sr.ReadLine());
                }
                PhysicalLine += ModuleText.Count;
                int CommentQuantity = 0;
                ModuleText = cs.TransforamationFunctions.RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = cs.TransforamationFunctions.ModuleMinimizing(ModuleText);//обработка модуля от лишних строк и разделителей
                ModuleText = cs.TransforamationFunctions.ModuleProcessing(ModuleText);//обнуление строковых констант
                //поиск условий и циклов
                LogicalLine += FindAllSemicolons(ModuleText);
                sr.Close();
                fs.Close();
            }

            result[0] = PhysicalLine;
            result[1] = LogicalLine;

            return result;
        }

        public static double FindAllSemicolons (List<string> ModuleText)
        {
            double LogicalLinesQuantity = 0;

            foreach (string x in ModuleText)
            {
                if (x.IndexOf(" if ") != -1 || x.IndexOf("if ") == 0 || x.IndexOf(" if(") != -1 || x.IndexOf("if(") == 0 || x.IndexOf(" for ") != -1 ||
                    x.IndexOf("for ") == 0 || x.IndexOf(" for(") != -1 || x.IndexOf("for(") == 0 || x.IndexOf(" while ") != -1 || x.IndexOf("while ") == 0 ||
                    x.IndexOf(" while(") != -1 || x.IndexOf("while(") == 0 || x.IndexOf(" foreach ") != -1 || x.IndexOf("foreach ") == 0 || x.IndexOf(" foreach(") != -1 ||
                    x.IndexOf("foreach(") == 0 || x.IndexOf(" switch ") != -1 || x.IndexOf("switch ") == 0 || x.IndexOf(" switch(") != -1 || x.IndexOf("switch(") == 0)
                    LogicalLinesQuantity++;
                if (x.Contains(';'))
                    LogicalLinesQuantity++;
            }

            return LogicalLinesQuantity;
        }
    }
}
