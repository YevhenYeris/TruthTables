using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthTableApp.TruthTableBuilder.AST
{
    public abstract class Node
    {
        public abstract bool Evaluate();

        public override abstract string ToString();
    }

    public class TrueNode : Node
    {
        public override bool Evaluate()
        {
            return true;
        }

        public override string ToString()
        {
            return "T";
        }
    }

    public class FalseNode : Node
    {
        public override bool Evaluate()
        {
            return false;
        }

        public override string ToString()
        {
            return "F";
        }
    }

    public class NegateNode : Node
    {
        public NegateNode(Node underlying)
        {
            Underlying = underlying;
        }

        public Node Underlying { get; set; }

        public override bool Evaluate()
        {
            return !Underlying.Evaluate();
        }

        public override string ToString()
        {
            return $"not {Underlying.ToString()}";
        }
    }

    public class AndNode : Node
    {
        public AndNode(Node lhs, Node rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public Node Lhs { get; set; }

        public Node Rhs { get; set; }

        public override bool Evaluate()
        {
            return Lhs.Evaluate() && Rhs.Evaluate();
        }

        public override string ToString()
        {
            return $"({Lhs.ToString()} and {Rhs.ToString()})";
        }
    }

    public class OrNode : Node
    {
        public OrNode(Node lhs, Node rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public Node Lhs { get; set; }

        public Node Rhs { get; set; }

        public override bool Evaluate()
        {
            return Lhs.Evaluate() || Rhs.Evaluate();
        }

        public override string ToString()
        {
            return $"({Lhs.ToString()} or {Rhs.ToString()})";
        }
    }

    public class ImpliesNode : Node
    {
        public ImpliesNode(Node lhs, Node rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public Node Lhs { get; set; }

        public Node Rhs { get; set; }

        public override bool Evaluate()
        {
            return !Lhs.Evaluate() || Rhs.Evaluate();
        }

        public override string ToString()
        {
            return $"({Lhs.ToString()} -> {Rhs.ToString()})";
        }
    }

    public class IifNode : Node
    {
        public IifNode(Node lhs, Node rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public Node Lhs { get; set; }

        public Node Rhs { get; set; }

        public override bool Evaluate()
        {
            return Lhs.Evaluate() == Rhs.Evaluate();
        }

        public override string ToString()
        {
            return $"({Lhs.ToString()} <-> {Rhs.ToString()})";
        }
    }

    public class XorNode : Node
    {
        public XorNode(Node lhs, Node rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public Node Lhs { get; set; }

        public Node Rhs { get; set; }

        public override bool Evaluate()
        {
            return Lhs.Evaluate() != Rhs.Evaluate();
        }

        public override string ToString()
        {
            return $"({Lhs.ToString()} xor {Rhs.ToString()})";
        }
    }

    public class VariableNode : Node
    {
        public VariableNode(string name, Dictionary<string, bool> variables)
        {
            Name = name;
            Variables = variables;
        }

        public string Name { get; set; } = string.Empty;

        public Dictionary<string, bool> Variables { get; set; } = new Dictionary<string, bool>();

        public override bool Evaluate()
        {
            return Variables[Name];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

//function parse(input)
//{
//	var scanResult = scan(input);
//	var tokens = scanResult.tokens;

//	var operators = [];
//	var operands = [];

//	var needOperand = true;
	
//	for (var i in tokens)
//	{
//		var currToken = tokens[i];

//		if (needOperand)
//		{
//			if (isOperand(currToken))
//			{
//				addOperand(wrapOperand(currToken), operands, operators);
//				needOperand = false;
//			}
//			else if (currToken.type === "(" || currToken.type === "~")
//			{
//				operators.push(currToken);
//			}

//			else if (currToken.type === kScannerConstants.EOF)
//			{
//				if (operators.length === 0)
//				{
//					parseError("", 0, 0);
//				}

//				if (topOf(operators).type === "(")
//				{
//					parseError("This open parenthesis has no matching close parenthesis.",
//							   topOf(operators).start, topOf(operators).end);
//				}

//				parseError("This operator is missing an operand.",
//						   topOf(operators).start, topOf(operators).end);
//			}
//			else
//			{
//				parseError("We were expecting a variable, constant, or open parenthesis here.",
//						   currToken.start, currToken.end);
//			}
//		}
//		else
//		{
//			if (isBinaryOperator(currToken) || currToken.type === kScannerConstants.EOF)
//			{
//				while (true)
//				{
//					if (operators.length === 0) break;

//					if (topOf(operators).type === "(") break;

//					if (priorityOf(topOf(operators)) <= priorityOf(currToken)) break;

//					var operator = operators.pop();
//					var rhs = operands.pop();
//					var lhs = operands.pop();

//					addOperand(createOperatorNode(lhs, operator, rhs), operands, operators);
//				}

//				if (currToken.type === kScannerConstants.EOF) break;
//			}

//			else if (currToken.type === ")")
//			{
//				while (true)
//				{
//					if (operators.length === 0)
//					{
//						parseError("This close parenthesis doesn't match any open parenthesis.", currToken.start, currToken.end);
//					}
//					var currOp = operators.pop();

//					if (currOp.type === "(") break;

//					if (currOp.type === "~")
//					{
//						parseError("Nothing is negated by this operator.", currOp.start, currOp.end);
//					}

//					var rhs = operands.pop();
//					var lhs = operands.pop();

//					addOperand(createOperatorNode(lhs, currOp, rhs), operands, operators);
//				}
//				var expr = operands.pop();
//				addOperand(expr, operands, operators);
//			}
//			else
//			{
//				parseError("We were expecting a close parenthesis or a binary connective here.",
//						   currToken.start, currToken.end);
//			}
//		}
//	}

//	assert(operators.length !== 0, "No operators on the operator stack (logic error in parser?)");
//	assert(operators.pop().type === kScannerConstants.EOF, "Stack top is not EOF (logic error in parser?)");

//	if (operators.length !== 0)
//	{
//		var mismatchedOp = operators.pop();
//		assert(mismatchedOp.type === "(",
//				"Somehow missed an operator factoring in EOF (logic error in parser?)");

//		parseError("No matching close parenthesis for this open parenthesis.",
//				   mismatchedOp.start, mismatchedOp.end);
//	}

//	return {
//		ast: operands.pop(),
//	   variables: scanResult.variables
//	};
