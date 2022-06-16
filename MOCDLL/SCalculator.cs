using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOC
{
    public class SCalculator
    {
        public SCalculator(double num)
        {
            Result = num;
            EquationForView = num.ToString();
            equation = num.ToString();
        }

        public SCalculator()
        {

        }

        #region Fields

        public List<double> MList { get; set; } = new();

        public string equation;

        public Calculate Cal { get; set; } = new();

        public string EquationForView { get; set; }

        public double Result { get; set; }

        #endregion

        #region Funcs

        public void Calculate()
        {
            Result = Cal.CalculateEquation(equation);
            equation = Result.ToString();
        }

        public void Add(string block)
        {
            equation += block;
            EquationForView += block;
            Calculate();
        }

        public List<double> M() => MList;

        public void MS() => MList.Add(Result);

        public void MC() => MList.Clear();

        public double MR() => MList.Last();

        public void MPlus(string equation) => MList[MList.Count - 1] += Cal.CalculateEquation(equation);

        public void MMinus(string equation) => MList[MList.Count - 1] -= Cal.CalculateEquation(equation);

        public void C()
        {
            Result = 0;
            equation = "";
            EquationForView = "";
        }
        #endregion
    }
}