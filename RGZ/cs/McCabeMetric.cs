using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RGZ.cs
{
    class McCabeMetric
    {
        public static double CalculationMcCabeMetric(List<string> Files)
        {
            double[] JilbResults = cs.JilbMetrics.CalculationJilbMetrics(Files);
            double result = JilbResults[0] + JilbResults[1] + 1;
            return result;
        }


    }
}
