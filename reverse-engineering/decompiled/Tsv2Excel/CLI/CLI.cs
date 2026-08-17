using System;
using System.Collections;

namespace CLI
{
	internal class CLI
	{
		private const int TOption = 1;

		private const int TOptionWithArg = 2;

		private const int TArgument = 3;

		public ArrayList commandList;

		private int numberOfArguments;

		private int numberOfOptions;

		private string[] valOptions;

		private int minNumberOfArgs;

		private int maxNumberOfArgs;

		private bool anySyntaxError;

		public int numberOfArgs => numberOfArguments;

		public CLI(string[] args, string[] validOptions, int minNbrArgs, int maxNbrArgs)
		{
			commandList = new ArrayList();
			int num = 1;
			numberOfOptions = 0;
			numberOfArguments = 0;
			minNumberOfArgs = minNbrArgs;
			maxNumberOfArgs = maxNbrArgs;
			valOptions = validOptions;
			anySyntaxError = false;
			foreach (string text in args)
			{
				if (text.StartsWith("/"))
				{
					bool flag = true;
					string[] array = text.Split('/');
					string[] array2 = array;
					foreach (string text2 in array2)
					{
						if (!text2.Equals(""))
						{
							if (text2.Contains("="))
							{
								string[] array3 = text2.Split('=');
								if (array3.Length == 2)
								{
									cliElement value = new cliElement
									{
										option = array3[0],
										argument = array3[1],
										type = 2,
										argumentPos = 0
									};
									commandList.Add(value);
									numberOfOptions++;
								}
								else
								{
									anySyntaxError = true;
								}
							}
							else
							{
								cliElement value2 = new cliElement
								{
									option = text2,
									argument = "",
									type = 1,
									argumentPos = 0
								};
								commandList.Add(value2);
								numberOfOptions++;
							}
						}
						else if (flag)
						{
							flag = false;
						}
						else
						{
							anySyntaxError = true;
						}
					}
				}
				else
				{
					cliElement value3 = new cliElement
					{
						option = "",
						argument = text,
						type = 3,
						argumentPos = num++
					};
					commandList.Add(value3);
				}
			}
			numberOfArguments = num - 1;
		}

		public string[] CheckCommmandLine()
		{
			int num = numberOfOptions;
			if (anySyntaxError)
			{
				num++;
			}
			if (numberOfArguments < minNumberOfArgs || numberOfArguments > maxNumberOfArgs)
			{
				num++;
			}
			string[] array = new string[num];
			int num2 = 0;
			bool flag = false;
			foreach (cliElement command in commandList)
			{
				flag = false;
				if (command.type != 1 && command.type != 2)
				{
					continue;
				}
				string[] array2 = valOptions;
				foreach (string value in array2)
				{
					flag = command.option.Equals(value);
					if (flag)
					{
						if (command.type == 2 && command.argument.Equals(""))
						{
							array[num2++] = "ERROR: Missing argument in /" + command.option + "=";
						}
						break;
					}
				}
				if (!flag)
				{
					array[num2++] = "ERROR: Unknown option /" + command.option;
				}
			}
			if (anySyntaxError)
			{
				array[num2++] = "ERROR: Any syntax error";
			}
			if (numberOfArguments < minNumberOfArgs)
			{
				array[num2++] = "ERROR: Too less arguments";
			}
			else if (numberOfArguments > maxNumberOfArgs)
			{
				array[num2++] = "ERROR: Too much arguments";
			}
			if (num2 == 0)
			{
				return null;
			}
			return array;
		}

		public bool IsOption(string option)
		{
			bool flag = false;
			foreach (cliElement command in commandList)
			{
				if (command.type == 1)
				{
					flag = command.option.Equals(option);
				}
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		public bool GetOptionArgument(string option, out string attr)
		{
			bool flag = false;
			attr = "";
			foreach (cliElement command in commandList)
			{
				if (command.type == 2)
				{
					flag = command.option.Equals(option);
				}
				if (flag)
				{
					attr = command.argument;
					break;
				}
			}
			return flag;
		}

		public bool GetArgument(int argPosition, out string attr)
		{
			bool flag = false;
			attr = "";
			foreach (cliElement command in commandList)
			{
				if (command.type == 3)
				{
					flag = command.argumentPos == argPosition;
				}
				if (flag)
				{
					attr = command.argument;
					break;
				}
			}
			return flag;
		}

		public bool IsHelpOption()
		{
			if (!IsOption("?") && !IsOption("H"))
			{
				return IsOption("h");
			}
			return true;
		}

		private void PrintCommandLine()
		{
			foreach (cliElement command in commandList)
			{
				Console.WriteLine("Type = " + command.type.ToString() + "; Option = " + command.option + "; Attr = " + command.argument + "; Pos = " + command.argumentPos);
			}
			IsOption("U");
			GetOptionArgument("F", out var _);
			GetArgument(1, out var _);
			GetArgument(2, out var _);
			Console.ReadKey();
		}
	}
}
