﻿using SimpleMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleMvc.Services
{
    public class EmailService : IEmailService
    {
        public void Send(string emailAddress, string sendMessage)
        {
        }
    }
}