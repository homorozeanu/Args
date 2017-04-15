# Args
A simple argument parser. Originally written by Robert C. Martin and ported to .NET by me. It is developed in C#.

# Other Versions
* Java Version by unclebob: http://github.com/unclebob/javaargs
* Ruby Version by unclebob: http://github.com/unclebob/rubyargs

# Args Sample
I provide here a simple example of how to use the Args class in order to parse some arguments using a predefined argument schema.

We start with the definition of the arguments schema and the arguments themselves. At the time of writing this sample, the schema supports boolean, integer, double, string and string array arguments.

Usually one uses the arguments provided by the Main method. In this sample we define these arguments locally, in order to simplify the execution of the sample.

After defining the schema and the arguments, we create an instance of the Args class, specifying in the constructor the schema and the arguments. With this instance we simply print each argument value on the console.

Here’s the code:

```cs
static void Main(string[] args)
{
	string schema = "bool,int#,double##,string*,stringArray[*]";
	string[] vargs = { "-bool", "-int", "10", "-double", System.Math.PI.ToString(), "-string", "hi", "-stringArray", "Hello", "World" };
	Args.Args parser = new Args.Args(schema, vargs);
 
	Console.WriteLine("bool: {0}", parser.GetBoolean("bool"));
	Console.WriteLine("int: {0}", parser.GetInt("int"));
	Console.WriteLine("double: {0}", parser.GetDouble("double"));
	Console.WriteLine("string: {0}", parser.GetString("string"));
 
	string[] stringArray = parser.GetStringArray("stringArray");
	for (int i = 0; i < stringArray.Length; i++)
	{
		Console.WriteLine("stringArray {0}: {1}", i, stringArray[i]);
	}
 
	Console.ReadLine();
}
```
