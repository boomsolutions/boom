using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MvcApplication4.Controllers
{
    /*
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            var soapSms = new MvcApplication4.ASPSMSX2.ASPSMSX2SoapClient("ASPSMSX2Soap");
            soapSms.SendSimpleTextSMS(
                System.Configuration.ConfigurationManager.AppSettings["ASPSMSUSERKEY"],
                System.Configuration.ConfigurationManager.AppSettings["ASPSMSPASSWORD"],
                message.Destination,
                System.Configuration.ConfigurationManager.AppSettings["ASPSMSORIGINATOR"],
                message.Body);
            soapSms.Close();
            return Task.FromResult(0);
        }
    }*/
}