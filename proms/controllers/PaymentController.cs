using Newtonsoft.Json;
using proms.models;
using proms.models.common;
using proms.models.payment;
using proms.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proms.controllers
{
    public class PaymentController : DatabaseHandler
    {

        public Response makePayment(PaymentModel paymentModel, Commoner commoner)
        {
            switch (paymentModel.paymentMode)
            {
                case "MPesa-STK":
                    return makeSTKPush(paymentModel, commoner);
                default:
                    return new Response(true, 0, "Failed.", new string[] { "Paymentmode not set." }, null);
            }
        }

        private static Response makeSTKPush(PaymentModel paymentModel, Commoner commoner)
        {

            STKPushResponse stkPushResponse = JsonConvert.DeserializeObject<STKPushResponse>((string)RequestService.post(null, "http://service.calista.co.ke/mpesa/api/stkpush/v1.php", JsonConvert.SerializeObject(new STKPush(paymentModel.mpesaPhone, paymentModel.amount, commoner.token, 5))));
            switch (stkPushResponse.ResponseCode)
            {
                case 0:
                    return new Response(true, 1, "Success", null, stkPushResponse);
                default:
                    return new Response(true, 0, "Failed", new string[] { stkPushResponse .ResponseDescription }, stkPushResponse);
            }
        }

        public Response mpesaCallback(STKCallbackResponse stkCallbackResponse, Commoner getModel)
        {
            Commoner[] commoner = new Commoner[] { getModel };
            commoner = JsonConvert.DeserializeObject<Commoner[]>(JsonConvert.SerializeObject(fetchRow("users", formatPairs(new KeyValue[] { new KeyValue("token", commoner[0].token) })).data));
            switch (stkCallbackResponse.body.stkCallback.ResultCode)
            {
                case 0:
                    /*Send email notifications*/
                    Email[] emailModels = new Email[] { new Email("Proms Datachip", commoner[0].email, null, "noreply@calista.co.ke", "New payment recieved", "Hi there, your payment has been recieved. From Proms by Datachip") };
                    RequestService.post(null, "http://service.calista.co.ke/email/api/express-s2s.php", JsonConvert.SerializeObject(emailModels));
                    /* Send message notifications*/
                    SmsMessage[] messageModels = new SmsMessage[] { new SmsMessage(commoner[0].mobile, "Hi there, your payment has been recieved. From Proms by Datachip") };
                    return JsonConvert.DeserializeObject<Response>((string)RequestService.post(null, "http://service.calista.co.ke/sms/api/express-s2s.php", JsonConvert.SerializeObject(messageModels)));
                default:
                    return new Response(true, 0, "Failed", new string[] { stkCallbackResponse.body.stkCallback.ResultCode.ToString() }, stkCallbackResponse);
            }
        }
    }
}