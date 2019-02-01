using System.Diagnostics;
using System.IO;
using WordDiff;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var oldText = @"the quick brown fox didn't really jump.";

            var newText = @"this text was replaced. really jump.";

            var diffedText = DiffEngine.DoTheDiff(oldText, newText);

            var html =
                    $@"
                    <html>
                     <head>
                      <style>
                        ins{{background-color: #d5ffba;}}
                        del{{background-color: #ffc6d5;}}
                      </style>
                     </head>
                      <body>
                        {diffedText}
                      </body>
                    </html>
                    ";

            File.WriteAllText(@"test.html", html);
            Process.Start("test.html");
        }
    }
}
