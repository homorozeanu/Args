﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Args
{
    public interface IArgumentMarshaler
    {
			void Set(IEnumerator<string> currentArgument);
		}
}
