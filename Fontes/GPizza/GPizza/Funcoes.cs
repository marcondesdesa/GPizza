using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPizza
{
    public static class Funcoes
    {
        public static int getInt(string numero)
        {
            int num = 0;
            if (int.TryParse(numero, out num))
                return num;
            else
                return 0;
        }

        public static decimal getDecimal(string numero)
        {
            decimal num = 0;
            if (decimal.TryParse(numero, out num))
                return num;
            else
                return 0;
        }

        public static double getDouble(string numero)
        {
            double num = 0;
            if (double.TryParse(numero, out num))
                return num;
            else
                return 0;
        }

        public static DateTime getDateTime(string datatime)
        {
            DateTime num = DateTime.MinValue;
            if (DateTime.TryParse(datatime, out num))
                return num;
            else
                return DateTime.MinValue;
        }
    }
}
