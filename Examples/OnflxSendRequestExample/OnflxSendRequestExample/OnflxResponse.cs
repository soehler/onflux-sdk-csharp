/********************************************************************************
 * 
 * This code and any derivations are property of True Systems Informática Ltda,
 * and shall no be distributed or shared without authorization
 * 
 * Author : soehler
 * 
 * Date : May 22, 2014 6:03:05 PM
 * 
 * (c) True Systems Informática 1995-2014
 * 
 * 
 *********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnflxSendRequestExample
{
    public class OnflxResponse
    {
        public String status{get;set;}
        public String message{get;set;}
        public OnflxResponse() 
        {
            status = "";
            message = "";
        }
    }
}