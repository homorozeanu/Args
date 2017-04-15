using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgsTests
{
	[TestClass]
	public class ArgsTest
	{
		[TestMethod]
		public void TestWithNoSchemaOrArguments()
		{
			Args.Args args = new Args.Args("", new String[0]);
			Assert.AreEqual(0, args.Cardinality());
		}

		[TestMethod]
		//[ExpectedException(typeof(Args.ArgsException), "Should throw an exception.")]
		public void TestWithNoSchemaButWithOneArgument()
		{
			try
			{
				new Args.Args("", new String[] { "-x" });
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.UNEXPECTED_ARGUMENT, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestWithNoSchemaButWithMultipleArguments()
		{
			try
			{
				new Args.Args("", new string[]{"-x","-y"});
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.UNEXPECTED_ARGUMENT, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestNonLetterSchema()
		{
			try
			{
				new Args.Args("*", new String[] { });
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.INVALID_ARGUMENT_NAME, ex.ErrorCode);
				Assert.AreEqual("*", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestInvalidArgumentFormat()
		{
			try
			{
				new Args.Args("f~", new String[]{});
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.INVALID_ARGUMENT_FORMAT, ex.ErrorCode);
				Assert.AreEqual("f", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestSimpleBooleanPresent()
		{
			Args.Args args = new Args.Args("x", new String[]{"-x"});
			Assert.AreEqual(1, args.Cardinality());
			Assert.AreEqual(true, args.GetBoolean("x"));
		}

		[TestMethod]
		public void TestSimpleStringPresent()
		{
			Args.Args args = new Args.Args("x*", new String[]{"-x", "param"});
			Assert.AreEqual(1, args.Cardinality());
			Assert.IsTrue(args.Has("x"));
			Assert.AreEqual("param", args.GetString("x"));
		}

		[TestMethod]
		public void TestMissingStringArgument()
		{
			try
			{
				new Args.Args("x*", new String[] { "-x" });
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.MISSING_STRING, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestSingleStringArrayPresent()
		{
			Args.Args args = new Args.Args("x[*]", new String[] { "-x", "param1", "param2", "param3" });
			Assert.AreEqual(1, args.Cardinality());
			Assert.IsTrue(args.Has("x"));

			String[] expected = new String[] { "param1", "param2", "param3" };
			String[] actual = args.GetStringArray("x");
			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod]
		public void TestMissingStringArrayArgument()
		{
			try
			{
				new Args.Args("x[*]", new String[] { "-x" });
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.MISSING_STRING, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestSingleStringArrayWithMultipleArgsPresent()
		{
			Args.Args args = new Args.Args("x[*],l,d#", new String[] { "-x", "param1", "param2", "param3", "-l", "-d", "10" });
			Assert.AreEqual(3, args.Cardinality());
			Assert.IsTrue(args.Has("x"));
			Assert.IsTrue(args.Has("l"));
			Assert.IsTrue(args.Has("d"));

			String[] expected = new String[] { "param1", "param2", "param3" };
			String[] actual = args.GetStringArray("x");
			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
			Assert.IsTrue(args.GetBoolean("l"));
			Assert.AreEqual(10, args.GetInt("d"));
		}

		[TestMethod]
		public void TestMultipleStringArrayWithMultipleArgsSimilarWordsPresent()
		{
			Args.Args args = new Args.Args("x[*],y[*],l", new String[] { "-x", "param1", "param2", "param3", "-y", "param2", "param3", "-l" });
			Assert.AreEqual(3, args.Cardinality());
			Assert.IsTrue(args.Has("x"));
			Assert.IsTrue(args.Has("y"));
			Assert.IsTrue(args.Has("l"));

			String[] expected = new String[] { "param1", "param2", "param3" };
			String[] actual = args.GetStringArray("x");
			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}

			expected = new String[] {"param2", "param3" };
			actual = args.GetStringArray("y");
			for (int i = 0; i < 2; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
			Assert.IsTrue(args.GetBoolean("l"));
		}

    [TestMethod]
    public void TestMultipleStringArrayWithOneWordPresent()
    {
      String[] expected;
      Args.Args args = new Args.Args("f*,t[*],s[*],b[*]", 
        new String[] { "-f", "homogeo@agrardata.at", "-t", "homopau@agrardata.at", "-s", "Test Email Subject" });
      
      Assert.AreEqual(3, args.Cardinality());
      Assert.IsTrue(args.Has("f"));
      Assert.IsTrue(args.Has("t"));
      Assert.IsTrue(args.Has("s"));

      Assert.AreEqual("homogeo@agrardata.at", args.GetString("f"));
      expected = args.GetStringArray("t");
      Assert.AreEqual("homopau@agrardata.at", expected[0]);
      
      expected = args.GetStringArray("s");
      Assert.AreEqual("Test Email Subject", expected[0]);

      expected = args.GetStringArray("b");
      Assert.AreEqual(0, expected.Length);

    }

		[TestMethod]
		public void TestSpacesInFormat()
		{
			Args.Args args = new Args.Args("x, y", new String[]{"-x", "-y"});
			Assert.AreEqual(2, args.Cardinality());
			Assert.IsTrue(args.Has("x"));
			Assert.IsTrue(args.Has("y"));
			Assert.IsTrue(args.GetBoolean("x"));
			Assert.IsTrue(args.GetBoolean("y"));
		}

		[TestMethod]
		public void TestSimpleIntPresent()
		{
			Args.Args args = new Args.Args("x#", new string[]{"-x", "42"});
			Assert.AreEqual(1, args.Cardinality());
			Assert.IsTrue(args.Has("x"));
			Assert.AreEqual(42, args.GetInt("x"));
		}

		[TestMethod]
		public void TestInvalidInteger()
		{
			try
			{
				new Args.Args("x#", new string[] { "-x", "Forty two" });
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.INVALID_INTEGER, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
				Assert.AreEqual("Forty two", ex.ErrorParameter);
			}
		}

		[TestMethod]
		public void TestMissingInteger()
		{
			try
			{
				new Args.Args("x#", new string[]{"-x"});
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.MISSING_INTEGER, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
			}
		}

		[TestMethod]
		public void TestSimpleDoublePresent()
		{
			Args.Args args = new Args.Args("x##", new string[]{"-x", "42.3"});
			Assert.AreEqual(1, args.Cardinality());
			Assert.IsTrue(args.Has("x"));
			Assert.AreEqual(42.3, args.GetDouble("x"), .001);
		}

		[TestMethod]
		public void TestInvalidDouble()
		{
			try
			{
				new Args.Args("x##", new String[]{"-x", "Forty two"});
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.INVALID_DOUBLE, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
				Assert.AreEqual("Forty two", ex.ErrorParameter);
			}
		}

		[TestMethod]
		public void TestMissingDouble()
		{
			try
			{
				new Args.Args("x##", new String[]{"-x"});
			}
			catch (Args.ArgsException ex)
			{
				Assert.AreEqual(Args.ArgsException.ErrorCodes.MISSING_DOUBLE, ex.ErrorCode);
				Assert.AreEqual("x", ex.ErrorArgumentId);
			}
		}

	}
}
