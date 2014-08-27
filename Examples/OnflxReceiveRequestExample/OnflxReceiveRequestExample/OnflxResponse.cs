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

namespace OnflxAPICallHandling
{
    public class OnflxResponse
    {
        public String result{get;set;}
        public String payload{get;set;}
        public OnflxResponse() 
        {
            result = "";
            payload = "";
        }
    }
}