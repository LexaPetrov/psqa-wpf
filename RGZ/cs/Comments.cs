using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RGZ.cs
{
    class Comments
    {
        public static double[] CalculationCommentsMetric(List<string> Files)
        {
            List<string> ModuleText = new List<string>();
            double[] result = new double[2];
            int CommentsQuantity = 0, LineQuantity=0;

            foreach (string x in Files)//идём по файлам
            {
                FileStream fs = new FileStream(x, FileMode.Open, FileAccess.Read);//создаём файловый поток
                StreamReader sr = new StreamReader(fs);//создаём читателя
                ModuleText = new List<string>();
                while (!sr.EndOfStream)//читаем файл до конца
                {
                    ModuleText.Add(sr.ReadLine());
                }
                LineQuantity += ModuleText.Count;
                int CommentQuantity = 0;
                ModuleText = cs.TransforamationFunctions.RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = cs.TransforamationFunctions.ModuleMinimizing(ModuleText);//обработка модуля от лишних строк и разделителей
                CommentsQuantity += CommentQuantity;
                

                sr.Close();
                fs.Close();
            }

            result[0] = CommentsQuantity;
            result[1] = (double)CommentsQuantity / (double)LineQuantity;

            return result;
        }
    }
}
