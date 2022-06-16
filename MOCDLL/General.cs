using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOC
{
    public enum Direction { Forwad, Back }

    public abstract class General
    {
        static public List<char> MathSymbols = new() { '*', '/', '+', '-', '^' };
        static public List<char> MathSymbolsOneSide = new() { '|', '!' };
        static public List<string> MathOperations = new() { "*", "/", "+", "-", "^", "|", "!", "hcf", "lcm" };
        static public List<string> MathOperationsNeedCheck = new() { "|", "!", "hcf", "lcm", "(" };
        static public List<string> MathTherms = new() { "hcf", "lcm" };
        static public double Pi = 3.1415926535897932384626433832795;
        static public double E = 2.7182818284590452353602874713527;
    }
}
