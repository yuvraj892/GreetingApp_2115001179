﻿using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class GreetingBL: IGreetingBL
    { 
         public string GetGreeting()
        {
            return "Hello world";
        }
    }
}
