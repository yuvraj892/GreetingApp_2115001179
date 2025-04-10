﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreeting();
        string PersonalizedGreeting(RequestModel requestModel);

        GreetingEntity SaveGreeting(string message);

        GreetingEntity GetGreetingsById(int id);

        List<GreetingEntity> GetAllGreetings();

        GreetingEntity EditGreetings(int id, string message);

        bool DeleteGreetingMessage(int id);
    }
}
