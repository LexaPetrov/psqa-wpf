using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RGZ.cs
{
    class JilbMetrics
    {
        public static double[] CalculationJilbMetrics(List<string> Files)
        {
            //считается количество операторов условия, количество циклов, максимальная и средняя вложенность операторов цикла и условия
            List<string> ModuleText = new List<string>();
            double[] result = new double[4];
            int ConditionConstructionQuantity = 0;
            int LoopConstructionQuantity = 0;
            int MaxEnclosure = 0;//максимальная вложенность
            int SummaryOfConstructionEnclosure = 0;//сумма коэффициентов вложенности операторов условий и циклов 
            double AverageEnclosure = 0.0;//средняя вложенность
            int CurrentEnclosure = 0;

            foreach (string path in Files)//идём по файлам
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);//создаём файловый поток
                StreamReader sr = new StreamReader(fs);//создаём читателя
                ModuleText = new List<string>();
                while (!sr.EndOfStream)//читаем файл до конца
                {
                    ModuleText.Add(sr.ReadLine());
                }
                int CommentQuantity = 0;
                ModuleText = cs.TransformationFunctions.RemoveComments(ModuleText, ref CommentQuantity);//обработка модуля от комментариев
                ModuleText = cs.TransformationFunctions.ModuleMinimizing(ModuleText);//обработка модуля от лишних строк и разделителей
                ModuleText = cs.TransformationFunctions.ModuleProcessing(ModuleText);//обнуление строковых констант
                ModuleText = cs.TransformationFunctions.OneOperatorOneString(ModuleText);//"один оператор <=> одна строка"
                ModuleText = cs.TransformationFunctions.RestorationOfNameFromNickname(ModuleText);//восстановление псевдонимов
                ModuleText = cs.TransformationFunctions.ConditionParser(ModuleText);//разбиение простых условий
                ModuleText = cs.TransformationFunctions.ConditionParser2(ModuleText);

                for (int j = 0; j<ModuleText.Count; j++)
                {
                    if (ModuleText[j].Contains("{") && (ModuleText[j - 1].IndexOf(" if ") != -1 || ModuleText[j - 1].IndexOf("if ") == 0 || ModuleText[j - 1].IndexOf(" if(") != -1 ||
                        ModuleText[j - 1].IndexOf("if(") == 0 || ModuleText[j - 1].IndexOf(" for ") != -1 || ModuleText[j - 1].IndexOf("for ") == 0 || ModuleText[j - 1].IndexOf(" for(") != -1 ||
                        ModuleText[j - 1].IndexOf("for(") == 0 || ModuleText[j - 1].IndexOf(" while ") != -1 || ModuleText[j - 1].IndexOf("while ") == 0 || ModuleText[j - 1].IndexOf(" while(") != -1
                        || ModuleText[j - 1].IndexOf("while(") == 0 || ModuleText[j - 1].IndexOf(" foreach ") != -1 || ModuleText[j - 1].IndexOf("foreach ") == 0 ||
                        ModuleText[j - 1].IndexOf(" foreach(") != -1 || ModuleText[j - 1].IndexOf("foreach(") == 0 || ModuleText[j - 1].IndexOf(" else ") != -1 ||
                        ModuleText[j - 1].IndexOf("else ") == 0 || (ModuleText[j - 1].IndexOf(" else") == ModuleText[j - 1].Length - 5 && ModuleText[j - 1].Contains(" else")) ||
                        (ModuleText[j - 1].IndexOf("else") == 0 && ModuleText[j - 1].Length == 4) || ModuleText[j - 1].IndexOf(" switch ") != -1 || ModuleText[j - 1].IndexOf("switch ") == 0 || ModuleText[j - 1].IndexOf(" switch(") != -1
                        || ModuleText[j - 1].IndexOf("switch(") == 0))//если предыдущая строка -- конструкция, а в этой {
                        CurrentEnclosure++;
                    else if (ModuleText[j].Contains("}") && CurrentEnclosure>0)
                        CurrentEnclosure--;
                    else if (ModuleText[j].IndexOf(" if ") != -1 || ModuleText[j].IndexOf("if ") == 0 || ModuleText[j].IndexOf(" if(") != -1 || ModuleText[j].IndexOf("if(") == 0 ||
                        ModuleText[j].IndexOf(" switch ") != -1 || ModuleText[j].IndexOf("switch ") == 0 || ModuleText[j].IndexOf(" switch(") != -1 || ModuleText[j].IndexOf("switch(") == 0)
                    {
                        SummaryOfConstructionEnclosure += CurrentEnclosure;
                        ConditionConstructionQuantity++;
                        if (CurrentEnclosure > MaxEnclosure)
                            MaxEnclosure = CurrentEnclosure;
                    }
                    else if(ModuleText[j].IndexOf(" for ") != -1 || ModuleText[j].IndexOf("for ") == 0 || ModuleText[j].IndexOf(" for(") != -1 ||
                           ModuleText[j].IndexOf("for(") == 0 || ModuleText[j].IndexOf(" while ") != -1 || ModuleText[j].IndexOf("while ") == 0 || ModuleText[j].IndexOf(" while(") != -1
                           || ModuleText[j].IndexOf("while(") == 0 || ModuleText[j].IndexOf(" foreach ") != -1 || ModuleText[j].IndexOf("foreach ") == 0 ||
                           ModuleText[j].IndexOf(" foreach(") != -1 || ModuleText[j].IndexOf("foreach(") == 0)
                    {
                        SummaryOfConstructionEnclosure += CurrentEnclosure;
                        LoopConstructionQuantity++;
                        if (CurrentEnclosure > MaxEnclosure)
                            MaxEnclosure = CurrentEnclosure;
                    }
                }

                sr.Close();
                fs.Close();
            }
            if (ConditionConstructionQuantity + LoopConstructionQuantity!=0)
                AverageEnclosure = (double)SummaryOfConstructionEnclosure / (double)(ConditionConstructionQuantity + LoopConstructionQuantity);
            result[0] = ConditionConstructionQuantity;
            result[1] = LoopConstructionQuantity;
            result[2] = MaxEnclosure;
            result[3] = AverageEnclosure;
            return result;
        }
    }
}
