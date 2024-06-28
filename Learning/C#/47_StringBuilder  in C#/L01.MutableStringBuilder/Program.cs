using System;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Mah_Mattar.L01
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            string s = GenerateWithString();
            Console.WriteLine(s);
            
            string sb = GenerateWithStringBuilder();
            Console.WriteLine(sb);


            Console.ReadKey();
        }

        static string GenerateWithString()
        {
            string str = null;

            str += String.Concat(new char[] { 'A', 'H', 'M' }); // AHM

            str += String.Format("OUD {0}{1}{2}{3}{4}{5}", 'M', 'A', 'T', 'T', 'S', 'R'); // AHMOUD MATTSR

            str = "M" + str; // MAHMOUD MATTSR

            str = str.Replace('S', 'A'); // MAHMOUD MATTAR

            str = str.Remove(str.Length - 1); // MAHMOUD MATTA

            return str;
        }
        static string GenerateWithStringBuilder()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(new char[] { 'A', 'H', 'M' }); // AHM

            sb.AppendFormat("OUD {0}{1}{2}{3}{4}{5}", 'M', 'A', 'T', 'T', 'S', 'R'); // AHMOUD MATTSR

            sb.Insert(0, "M"); // MAHMOUD MATTSR

            sb.Replace('S', 'A'); // MAHMOUD MATTAR

            sb.Remove(sb.Length - 1, 1); // MAHMOUD MATTA

            return sb.ToString();
        }
    }
}


