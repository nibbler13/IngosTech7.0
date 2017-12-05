using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IngosTech7._0 {
	class Parser {
		public static string ParseString(string enteredText) {
			Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
			enteredText = regex.Replace(enteredText, " ");


			string[] specialSymbols = new string[] { "+", "-", "*", "/", "(", ")" };
			foreach (string symbol in specialSymbols) {
				enteredText = enteredText.Replace(" " + symbol, symbol);
				enteredText = enteredText.Replace(symbol + " ", symbol);
			}

			if (enteredText.Contains(" "))
				return "Строка имеет неверный формат (лишние пробелы)";

			if (string.IsNullOrEmpty(enteredText) ||
				string.IsNullOrWhiteSpace(enteredText)) {
				return "Введенная строка пуста";
			}

			while (enteredText.Contains("(")) {
				for (int i = enteredText.Length - 1; i >= 0; i--) {
					string symbol = enteredText.Substring(i, 1);
					if (!symbol.Equals("("))
						continue;

					bool isReplaced = false;
					for (int x = i; x < enteredText.Length; x++) {
						string symbolNext = enteredText.Substring(x, 1);
						if (!symbolNext.Equals(")"))
							continue;

						string bracket = enteredText.Substring(i + 1, x - i - 1);
						string result = CalcExpression(bracket);
						if (result.StartsWith("-1"))
							return result;

						enteredText = enteredText.Replace("(" + bracket + ")", result);
						isReplaced = true;
						break;
					}

					if (!isReplaced)
						return "Строка содержит неверное количество скобок";

					break;
				}
			}

			if (enteredText.Contains(")"))
				return "Строка содержит неверное количество скобок";

			enteredText = CalcExpression(enteredText);

			return enteredText;
		}

		private static string CalcExpression(string text, bool isMultiplyAndDivide = true) {
			if (string.IsNullOrEmpty(text) ||
				string.IsNullOrWhiteSpace(text))
				return text;

			if (text.StartsWith("-1"))
				return text;

			Console.WriteLine("CalcExpression: " + text);

			string specialSymbols = "+-*/ ";

			if (!text.Contains("+") &&
				!text.Contains("-") &&
				!text.Contains("*") &&
				!text.Contains("/") &&
				!text.Contains(" "))
				return text;

			while (true) {
				if (isMultiplyAndDivide) {
					if (!text.Contains("*") && !text.Contains("/"))
						break;
				} else {
					if (!text.Contains("+") && !text.Contains("-") && !text.Contains(" "))
						break;
				}

				string operand1 = string.Empty;
				string operand2 = string.Empty;
				string operationType = string.Empty;

				string temp = string.Empty;

				for (int i = 0; i < text.Length; i++) {
					string symbol = text.Substring(i, 1);

					if (specialSymbols.Contains(symbol)) {
						if (i == text.Length - 1)
							return "-1 Неправильный формат строки (оператор без операнда в правой части)";

						if (!string.IsNullOrEmpty(temp)) {
							if (string.IsNullOrEmpty(operand1))
								operand1 = temp;
							else if (string.IsNullOrEmpty(operand2)) {
								operand2 = temp;
								temp = string.Empty;
								break;
							} else
								break;

							temp = string.Empty;
						} else
							return "-1 Неправильный формат строки (оператор без операнда / повторяющийся оператор)";

						if (isMultiplyAndDivide)
							if (symbol.Equals("+") || symbol.Equals("-")) {
								if (!string.IsNullOrEmpty(operand1) && 
									string.IsNullOrEmpty(operationType) && 
									string.IsNullOrEmpty(operand2)) {
									operand1 = string.Empty;
									temp = string.Empty;
									continue;
								} else
									break;
							}

						if (symbol.Equals(" "))
							continue;

						operationType = symbol;
						continue;
					}

					temp += symbol;

					if (i == text.Length - 1) {
						if (string.IsNullOrEmpty(operand1))
							operand1 = temp;
						else if (string.IsNullOrEmpty(operand2))
							operand2 = temp;
						else
							return "-1 Повторяющиеся операнды";
					}
				}

				if (string.IsNullOrEmpty(operand1) ||
					string.IsNullOrEmpty(operationType) ||
					string.IsNullOrEmpty(operand2))
					return "-1 Неправильный формат выражения для расчета";

				int begin = text.IndexOf(operand1);
				string result = Calc(operand1, operand2, operationType);
				if (result.StartsWith("-1"))
					return result;

				text = text.Replace(operand1 + operationType + operand2, result);
			}

			text = CalcExpression(text, false);

			Console.WriteLine("return: " + text);
			return text;
		}

		private static string Calc(string operand1, string operand2, string operationType) {
			if (string.IsNullOrEmpty(operand1) || string.IsNullOrEmpty(operand2))
				return "-1 Пустой операнд для операции: '" + operand1 + "' " + operationType + " '" + operand2 + "'";

			switch (operationType) {
				case "+":
					return operand1 + operand2;
				case "-":
					if (operand1.EndsWith(operand2))
						return operand1.Substring(0, operand1.Length - operand2.Length);
					else
						return operand1;
				case "*":
					string resultMultiply = string.Empty;

					for (int i = 0; i < (operand1.Length >= operand2.Length ? operand1.Length : operand2.Length) ; i++) {
						if (i < operand1.Length)
							resultMultiply += operand1.Substring(i, 1);
						if (i < operand2.Length)
							resultMultiply += operand2.Substring(i, 1);
					}

					return resultMultiply;
				case "/":
					if (operand2.Length * 2 > operand1.Length)
						return operand1;

					string resultDivide = string.Empty;
					
					for (int i = 0; i < operand2.Length; i++) {
						if (!operand1.Substring(i * 2 + 1, 1).Equals(operand2.Substring(i, 1)))
							return operand1;

						resultDivide += operand1.Substring(i * 2, 1);
					}

					if (operand1.Length > operand2.Length * 2)
						resultDivide += operand1.Substring(operand2.Length * 2, operand1.Length - operand2.Length * 2);

					return resultDivide;
				default:
					return "-1 Неподдерживаемый тип операции: " + operationType;
			}
		}
	}
}
