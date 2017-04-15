using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgsExample
{
	class Program
	{
		static void Main(string[] args)
		{
			string schema = "bool,int#,double##,string*,stringArray[*]";
			string[] vargs = { "-bool", "-int", "10", "-double", System.Math.PI.ToString(), "-string", "hi", "-stringArray", "Hello", "World" };
			Args.Args parser = new Args.Args(schema, vargs);

			Console.WriteLine("bool: {0}", parser.GetBoolean("bool"));
			Console.WriteLine("int: {0}", parser.GetInt("int"));
			Console.WriteLine("double: {0}", parser.GetDouble("double"));
			Console.WriteLine("string: {0}", parser.GetString("string"));

			string[] stringArray = parser.GetStringArray("stringArray");
			for (int i = 0; i < stringArray.Length; i++)
			{
				Console.WriteLine("stringArray {0}: {1}", i, stringArray[i]);
			}

			Console.ReadLine();
		}
	}
}
