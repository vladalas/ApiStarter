﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseStarter.DAL
{
    public class ValidationMessage
    {
        public ValidationMessage(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
