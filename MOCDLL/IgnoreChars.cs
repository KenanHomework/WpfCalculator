using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOC
{

    public class IgnoreChars
    {
        public IgnoreChars(string chars, int count = 1)
        {
            Chars = chars;
            Count = count;
        }

        public IgnoreChars()
        {

        }

        public string Chars { get; set; } = " ";
        public int Count { get; set; } = 0;

        public bool IsIgnore(char ch) => Chars.Contains(ch);
    }
}
