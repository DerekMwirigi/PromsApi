using proms.models;
using proms.models.auth;
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
            Response response = fetchRow("users", formatPairs(new KeyValue[] { new KeyValue("email", signIn.uId), new KeyValue("password", signIn.uSecret) }));
            if (response.status_code == 1) { return new Response(true, 1, "Sign in success", null, response.data); }
            return new Response(true, 0, "Sign in failed", response.errors, null);
        }
    }
}