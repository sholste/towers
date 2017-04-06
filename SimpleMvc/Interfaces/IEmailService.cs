using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleMvc.Interfaces
{
    public interface IEmailService
    {
        void Send(string emailAddress, string sendMessage);
    }
}