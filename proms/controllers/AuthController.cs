using Newtonsoft.Json;
using proms.models;
using proms.models.auth;
using proms.models.common;
using proms.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proms.controllers
{
    public class AuthController : DatabaseHandler
    {
        public Response signIn(SignIn signIn)
        {
            foreach (string account in new string[] { "users" })
            {
                Response response = fetchRow(account, formatPairs(new KeyValue[] {
                    new KeyValue("email", signIn.uId),
                    new KeyValue("password", Utils.encryptPassword(signIn.uSecret))
                }));
                if (response.status_code == 1) { return new Response(true, 1, "Sign in success", null, JsonConvert.DeserializeObject<Commoner[]>(JsonConvert.SerializeObject(response.data))[0]); }
                return new Response(true, 0, "Sign in failed", response.errors, null);
            }
            return new Response(true, 0, "Sign in failed.", new string[] { "Invalid details." }, null);
        }

        public Response signUp(string account, Commoner commoner)
        {
            Response response = insertRow(account, new KeyValue[] {
                new KeyValue("code", Utils.randomNumber(11111, 99999).ToString()),
                new KeyValue("token", Utils.encryptPassword(Utils.randomString(90, true))),
                new KeyValue("firstName", commoner.firstName),
                new KeyValue("lastName", commoner.lastName),
                new KeyValue("mobile", commoner.mobile),
                new KeyValue("email", commoner.email),
                new KeyValue("password", Utils.encryptPassword(commoner.password)),
                new KeyValue("avatar", commoner.avatar)
            });
            if (response.status_code == 1)
            {
                return signIn(new SignIn(commoner.email, commoner.password));
            }
            return new Response(true, 0, "Sign up failed.", response.errors, null);
        }
    }
}