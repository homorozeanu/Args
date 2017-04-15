using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgsTests
{
	[TestClass]
	public class ArgsExceptionTest
	{
		[TestMethod]
		public void TestUnexpectedMessage()
		{
			Args.ArgsException e = new Args.ArgsException(Args.ArgsException.ErrorCodes.UNEXPECTED_ARGUMENT, "x", null);
			Assert.AreEqual("Argument -x unexpected.", e.ErrorMessage());
		}

		[TestMethod]
		public void TestMissingStringMessage()
		{
			Args.ArgsException e = new Args.ArgsException(Args.ArgsException.ErrorCodes.MISSING_STRING, "x", null);
			Assert.AreEqual("Could not find string parameter for -x.", e.ErrorMessage());
		}

		[TestMethod]
		public void TestInvalidIntegerMessage()
		{
			Args.ArgsException e = new Args.ArgsException(Args.ArgsException.ErrorCodes.INVALID_INTEGER, "x", "Forty two");
			Assert.AreEqual("Argument -x expects an integer but was 'Forty two'.", e.ErrorMessage());
		}

		[TestMethod]
		public void TestMissingIntegerMessage()
		{
			Args.ArgsException e = new Args.ArgsException(Args.ArgsException.ErrorCodes.MISSING_INTEGER, "x", null);
			Assert.AreEqual("Could not find integer parameter for -x.", e.ErrorMessage());
		}

		[TestMethod]
		public void TestInvalidDoubleMessage()
		{
			Args.ArgsException e = new Args.ArgsException(Args.ArgsException.ErrorCodes.INVALID_DOUBLE, "x", "Forty two");
			Assert.AreEqual("Argument -x expects a double but was 'Forty two'.", e.ErrorMessage());
		}

		[TestMethod]
		public void TestMissingDoubleMessage()
		{
			Args.ArgsException e = new Args.ArgsException(Args.ArgsException.ErrorCodes.MISSING_DOUBLE, "x", null);
			Assert.AreEqual("Could not find double parameter for -x.", e.ErrorMessage());
		}

	}

}
