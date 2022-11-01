using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthTableApp.TruthTableBuilder
{
    public class TruthTableCalculator
    {
		public TruthTable GenerateTruthTable(ParserResult parserResult)
        {
			var assignment = parserResult.Variables.Select(v => false).ToList();
			var truthTable = new TruthTable() { Formula = parserResult.Formula };

			do
			{
				for (int i = 0; i < parserResult.Variables.Count; ++i)
				{
					var varName = parserResult.Variables.ElementAt(i).Key;
					parserResult.Variables[varName] = assignment[i];
				}

				truthTable.Table[assignment] = parserResult.AST.Evaluate();
				assignment = NextAssignment(assignment);
			}
			while (assignment != null);

			if (truthTable.Table.Values.All(v => v))
			{
				truthTable.IsEquality = true;
				truthTable.IsExecutable = true;
			}
			else if (truthTable.Table.Values.Where(v => v).Any())
			{
				truthTable.IsExecutable = true;
			}

			truthTable.Variables = parserResult.Variables.Keys;

			return truthTable;
        }

		private List<bool> NextAssignment(List<bool> assignment)
		{
			var flipIndex = assignment.Count - 1;
			while (flipIndex >= 0 && assignment[flipIndex])
			{
				--flipIndex;
			}

			if (flipIndex == -1)
			{
				return null;
			}

			var newAssignment = assignment.Select(v => v).ToList();
			newAssignment[flipIndex] = true;

			for (int i = flipIndex + 1; i < assignment.Count; ++i)
            {
				newAssignment[i] = false;
            }

			return newAssignment;
		}
	}

	public class TruthTable
    {
		public Dictionary<List<bool>, bool> Table { get; set; } = new Dictionary<List<bool>, bool>();

		public IEnumerable<string> Variables { get; set; }

		public bool IsExecutable { get; set; } = false;

		public bool IsEquality { get; set; } = false;

		public string Formula { get; set; }
	}

	//function generateTruthTable(parseResult, callback)
	//{
	//	/* Create a new array of truth values that will stand for the truth values
	//	 * of all of the variables. Initially, these values will all be false. We'll
	//	 * treat this as a binary counter to enumerate all possible truth assignments.
	//	 */
	//	var assignment = [];
	//	for (var i = 0; i < parseResult.variables.length; i++)
	//	{
	//		assignment.push(false);
	//	}

	//	/* Evaluate the expression under all possible truth values. Remember - even
	//	 * if there are zero variables, there's still the vacuous truth assignment!
	//	 */
	//	do
	//	{
	//		callback(assignment, parseResult.ast.evaluate(assignment));
	//	} while (nextAssignment(assignment));
	//}

	///* Function: nextAssignment
	// *
	// * Given an array representing a truth assignment, generates the next truth
	// * assignment from it, returning true if one is found and false otherwise.
	// *
	// * This implementation works by simulating a binary counter to enumerate all
	// * truth values.
	// */
	//function nextAssignment(assignment)
	//{
	//	/* Walking from the right to the left, search for a false to make true. */
	//	var flipIndex = assignment.length - 1;
	//	while (flipIndex >= 0 && assignment[flipIndex]) flipIndex--;

	//	/* If we didn't find an index to flip, we've tried all assignments and are
	//	 * therefore done.
	//	 */
	//	if (flipIndex == -1) return false;

	//	/* Otherwise, flip this index to true and all following values to false. */
	//	assignment[flipIndex] = true;

	//	for (var i = flipIndex + 1; i < assignment.length; i++)
	//	{
	//		assignment[i] = false;
	//	}

	//	return true;
	//}

}