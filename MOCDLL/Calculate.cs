using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOC
{
    public class Calculate
    {
        public double CalculateNums(double num1, double num2, string operation)
        {
            switch (operation)
            {
                case "+":
                    return Add(num1, num2);
                case "-":
                    return Sub(num1, num2);
                case "/":
                    return Div(num1, num2);
                case "*":
                    return Mul(num1, num2);
                case "^":
                    return Power(num1, num2);
                case "hcf":
                    return HCF(num1, num2);
                case "lcm":
                    return LCM(num1, num2);
                default:
                    break;
            }
            return 1;
        }

        public double CalculateNum(double num, string operation)
        {
            switch (operation)
            {
                case "!":
                    return Fact(num);
                case "|":
                    return Pip(num);
                default:
                    break;
            }
            return 1;
        }

        public double CalculateEquation(string equation)
        {
            try
            {
                return Convert.ToDouble(equation);
            }
            catch (Exception) { }

            CalculateBrackets(ref equation);
            double res = 0;

        /* 1~ Calculate Math Funcs*/
        CalculateFN:
            if (equation.HasMember(General.MathTherms))
            {
                foreach (string func in General.MathTherms)
                {
                    while (true)
                    {
                        if (!equation.Contains(func))
                            break;

                        int index = equation.IndexOf(func);
                        int open = equation.IndexOf(func);
                        int close = equation.IndexOf(')', open);
                        index = equation.GetRange(new(index, equation.Length)).IndexOf('(') + index;
                        List<string> list = equation.GetRangeToTarget(index, ')', Direction.Forwad).Split(',').ToList();

                        if (list.Count == 2)
                            res = CalculateNums(double.Parse(list[0]), double.Parse(list[1]), func);
                        else if (list.Count == 1)
                            res = CalculateNum(double.Parse(list[0]), func);

                        equation = equation.Change(new(open, close), res.ToString());
                    }
                }
                goto CalculateFN;
            }

        /* 2~ Calculate Math Symbols */
        CalculateSY:
            if (equation.HasMember(General.MathSymbols))
            {
                double num1 = 0;
                double num2 = 0;
                int index = 0;
                int indexOfNum1 = 0;
                int indexOfNum2 = 0;
                string temp = "";

                foreach (char operation in General.MathSymbols)
                {
                    while (true)
                    {
                        index = equation.IndexOf(operation);
                        if (index < 0)
                            break;

                        temp = equation.GetRangeToTarget(index, General.MathSymbols, Direction.Back);
                        indexOfNum1 = index - temp.Length;
                        num1 = CalculateEquation(temp);

                        temp = equation.GetRangeToTarget(index, General.MathSymbols, Direction.Forwad);
                        indexOfNum2 = index + temp.Length;
                        num2 = CalculateEquation(temp);


                        res = CalculateNums(num1, num2, operation.ToString());
                        equation = equation.Replace(equation.GetRange(indexOfNum1, indexOfNum2 + 1), res.ToString());
                    }
                }
                goto CalculateSY;
            }

        CalculateSYOS:
            if (equation.HasMember(General.MathSymbolsOneSide))
            {
                double num = 0;
                int index = 0;
                int indexOfNum = 0;
                string temp = "";

                foreach (char operation in General.MathSymbolsOneSide)
                {
                    while (true)
                    {
                        index = equation.IndexOf(operation);
                        if (index < 0)
                            break;

                        temp = equation.GetRangeToTarget(index, General.MathSymbols, Direction.Forwad);
                        indexOfNum = index + temp.Length;
                        num = Convert.ToDouble(temp);

                        try
                        {
                            if (equation[indexOfNum - 1].IsNumber())
                                equation = equation.AppendAt(indexOfNum - 1, '*');
                        }
                        catch (ArgumentOutOfRangeException) { }


                        res = CalculateNum(num, operation.ToString());
                        equation = equation.Replace(equation.GetRange(index, indexOfNum + 1), res.ToString());
                    }
                }
                goto CalculateSYOS;
            }

            return res;
        }

        public void CalculateBrackets(ref string equation)
        {
            if (!equation.Contains('('))
                return;

            SolveDoubleOperation(ref equation);

            StringBuilder builder = new();
            int count = equation.Count(ch => ch == '(');

            for (int i = 0; i < count; i++)
            {
                int open = equation.LastIndexOf('(');
                int close = equation.GetRange(new(open, equation.Length)).IndexOf(')') + open;

                if (open > 0 && !equation[open - 1].IsMathSymbol())
                    equation = equation.AppendAt(open - 1, '*');

                if (equation[open - 1].IsMathSymbol())
                    continue;

                open = equation.LastIndexOf('(');
                close = equation.GetRange(new(open, equation.Length)).IndexOf(')') + open;
                string s = equation.GetRange(new(open + 1, close));
                double d = CalculateEquation(s);
                equation = equation.Change(new(open, close), d.ToString());
            }

            SolveDoubleOperation(ref equation);
        }

        public void SolveDoubleOperation(ref string equation)
        {
            equation = equation.Replace("+-", "-");
            equation = equation.Replace("-+", "-");
            equation = equation.Replace("--", "+");
            equation = equation.Replace("++", "+");
        }

        public double Add(double num1, double num2) => num1 + num2;

        public double Sub(double num1, double num2) => num1 - num2;

        public double Mul(double num1, double num2) => num1 * num2;

        public double Div(double num1, double num2)
        {
            if (num1 == 0 || num2 == 0)
                throw new Exception("Cannot divide by zero");
            return num1 / num2;
        }

        public double Power(double num1, double power)
        {
            double res = num1;
            for (int i = 1; i < power; i++)
                res *= num1;
            return res;
        }

        public double Pip(double num) => num >= 0 ? num : num * -1;

        public double Fact(double num)
        {
            double res = num;
            for (int i = (int)num - 1; i >= 1; i--)
                res *= (num - i);
            return res;
        }

        public double Cicik(double num1, double num2) => num1 < num2 ? num1 : num2;

        public double Boyuk(double num1, double num2) => num1 > num2 ? num1 : num2;

        public bool IsBolen(double bolunen, double num) => !Convert.ToBoolean(bolunen % num);

        public bool IsSimple(double num)
        {
            if (num == 4)
                return false;
            for (int i = 2; i < (num / 2); i++)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
        }

        public List<double> SadeEdedler(double num)
        {
            List<double> list = new List<double>();
            for (int i = 2; i <= num; i++)
                if (IsSimple(i))
                    list.Add(i);
            return list;
        }

        public List<double> SadeVuruq(double num)
        {
            List<double> sadeEdedler = SadeEdedler(num);
            List<double> vuruqlar = new List<double>();


            for (int i = 0; i < sadeEdedler.Count(); i++)
            {
                if (!Convert.ToBoolean(num % sadeEdedler[i]))
                {
                    num /= sadeEdedler[i];
                    vuruqlar.Add(sadeEdedler[i]);
                    i--;
                }
            }
            return vuruqlar;

        }

        public List<double> OrtaqVuruqlar(List<double> small, List<double> big)
        {
            List<double> ortaq = new List<double>();
            foreach (int i in big)
            {
                if (small.Contains(i))
                    ortaq.Add(i);
            }
            return ortaq;
        }

        public bool IsQarshiliqliSadeEded(List<double> ortaqVuruqlar) => ortaqVuruqlar.Count == 0;

        public double LCM(double num1, double num2)
        {
            double smallNum = Cicik(num1, num2);
            double bigNum = Boyuk(num2, num1);
            double ekob = bigNum;
            /* EKOB(a,b) = a (bolunen) */
            if (IsBolen(bigNum, smallNum))
                return bigNum;

            /* EKOB(a,b) =  c */
            var smallList = SadeVuruq(smallNum);
            var bigList = SadeVuruq(bigNum);
            List<double> ortaq = OrtaqVuruqlar(smallList, bigList);

            /* Qarsiliqli sade ededlerin EKOB'u onlarin hasiline beraberdir */
            if (IsQarshiliqliSadeEded(ortaq))
                return num1 * num2;

            /* Find Different  ~ C */
            foreach (int i in ortaq)
                smallList.Remove(i);


            foreach (int i in smallList)
                ekob *= i;

            return ekob;

        }

        public double HCF(double num1, double num2)
        {
            double smallNum = Cicik(num1, num2);
            double bigNum = Boyuk(num2, num1);
            double ebob = 1;

            /* EBOB(a,b) = b (bolen) */
            if (IsBolen(bigNum, smallNum))
                return smallNum;

            /* Ortaq Vuruqlar */
            var smallList = SadeVuruq(smallNum);
            var bigList = SadeVuruq(bigNum);
            List<double> ortaq = OrtaqVuruqlar(smallList, bigList);

            /* Qarsiliqli sade ededlerin EBOB'u 1-dir */
            if (IsQarshiliqliSadeEded(ortaq))
                return 1;

            /* EBOB */
            ortaq.ForEach(vuruq => ebob *= vuruq);

            return ebob;
        }
    }
}