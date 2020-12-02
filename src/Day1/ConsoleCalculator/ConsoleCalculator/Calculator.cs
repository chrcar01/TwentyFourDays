using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator
{
    public static class Calculator
    {
		public static double Evaluate(string expression)
		{
			var num = string.Empty;
			var nums = new Stack<double>();
			var lastOp = Char.MinValue;

			Func<string, string> Push = (num) =>
			{
				if (!string.IsNullOrWhiteSpace(num))
				{
					var negative = num.Count(c => c == '-') % 2 == 1;
					num = num.TrimStart('-');

					nums.Push(Convert.ToDouble(num) * (negative ? -1 : 1));
					//nums.Dump();
					//lastOp.Dump();
					return String.Empty;
				}
				return num;
			};

			Func<char, char> MultiplyOrDivide = (lastOp) =>
			{
				var result = lastOp;
				if ((lastOp == '*' || lastOp == '/') && nums.Count > 1)
				{
					var right = nums.Pop();
					var left = nums.Pop();
					if (lastOp == '*') nums.Push(left * right);
					if (lastOp == '/') nums.Push(left / right);
					result = Char.MinValue;
				}

				return result;
			};

			Func<string, int, int> FindMatchingParenIndex = (searchString, indexOfParenToMatch) =>
			{
				var count = 0;
				for (var i = indexOfParenToMatch; i < searchString.Length; i++)
				{
					if (searchString[i] == '(') count++;
					if (searchString[i] == ')') count--;
					if (count == 0)
					{
						return i;
					}
				}
				return -1;
			};

			for (var i = 0; i < expression.Length; i++)
			{
				var c = expression[i];
				if (c == '(')
				{
					// find the matching paren
					var closingParenIndex = FindMatchingParenIndex(expression, i);// i needs to pick up right after this guy
					if (closingParenIndex < 0)
					{
						closingParenIndex = expression.Length;
					}
					var start = i + 1;
					var length = closingParenIndex - start;
					var subExpression = expression.Substring(start, length);
					var result = Evaluate(subExpression);
					if (num == "-" && result < 0)
					{
						num = string.Empty;
						result = result * (-1);
					}
					num += result;
					num = Push(num);
					lastOp = MultiplyOrDivide(lastOp);
					i = closingParenIndex;
				}
				if (c == '+')
				{
					num = Push(num);
					lastOp = MultiplyOrDivide(lastOp);
					continue;
				}

				if (c == '-')
				{
					if (!string.IsNullOrWhiteSpace(num) && num != "-")
					{
						num = Push(num);
						lastOp = MultiplyOrDivide(lastOp);
					}

					num += c;


					continue;
				}

				if (Char.IsNumber(c) || c == '.')
				{
					num += c;
					continue;
				}

				if (c == '*' || c == '/')
				{
					if (!string.IsNullOrWhiteSpace(num))
					{
						num = Push(num);
						lastOp = MultiplyOrDivide(lastOp);
					}

					lastOp = c;
				}
			}

			if (!string.IsNullOrWhiteSpace(num))
			{
				num = Push(num);
				lastOp = MultiplyOrDivide(lastOp);
			}
			return nums.Sum();
		}
	}
}
