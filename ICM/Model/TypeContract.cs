﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICM.Model
{
    public class TypeContract
    {
        public string Name
        {
            get;
            set;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}