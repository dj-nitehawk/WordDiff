using System;
using WordDiff;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var oldText =
                @"the quick brown fox didn't really jump.";

            var newText =
                @"the slow brown fox did really jump.";

            var diffedText = DiffEngine.DoTheDiff(oldText, newText);

            Console.Write(diffedText);
            Console.Read();
        }
    }
}
