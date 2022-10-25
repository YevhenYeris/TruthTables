using System;
using TruthTablesLib;

namespace TruthTables
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             * Scans for tokens and variables - no
             */
            var scanner = new Scanner();
            var input = "aBc and (bdc or Fsd) or not (M xor bdc)";
            var result = scanner.Scan(input);

            var parser = new Parser(scanner);
            var pResult = parser.Parse(input);

            var res = pResult.AST.Evaluate();

            var truthTableCalculator = new TruthTableCalculator();
            var truthTable = truthTableCalculator.GenerateTruthTable(pResult);

            var a = 0;
        }
    }

}
