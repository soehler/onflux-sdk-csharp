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
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using OnflxFormSchemaV1;

namespace OnflxSendRequestExample
{
    public class OnflxRequest
    {

        /// Url do onflux recebe formulários externos e os envia para os dispositivos android
        public String URL = "http://www.onflux.com.br/android/ofxreceiver.aspx";

        // O valor dessa chave deve ser obtido no site do onflux, menu Auditoria / Gerador API Key
        //public String OnfxApiKey = "e777a75d61455d12b3eb7e9541caea12";
        public String OnfxApiKey = "e686a75d61544d12b3eb7e4128caea83"; //azimute apikey
        // Id do usuário que receberá o formulario (id que o usuário possui no cadastro do onflux)
        public String OnfxUser { get; set; }

        public String Verb = "POST";
        public HttpWebRequest HttpRequest { get; internal set; }
        public HttpWebResponse HttpResponse { get; internal set; }
        public CookieContainer CookieContainer = new CookieContainer();

        // Conteudo json
        public String Content
        {
            get { return "application/json"; }
        }

        // Construtor
        public OnflxRequest()
        {
        }

        // Executa o envio do formulario json
        // formulario e id do usuario que o receberá 
        public Object Execute(Form objForm, String user)
        {
            if (user == null || user.Equals(""))
            {
                throw new ArgumentException("User cannot be null.");
            }
            OnfxUser = user;

            HttpRequest = CreateRequest();

            WriteStream(objForm);

            try
            {
                HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
            }
            catch (WebException error)
            {
                HttpResponse = (HttpWebResponse)error.Response;
                return ReadResponseFromError(error);
            }
            return JsonConvert.DeserializeObject<OnflxResponse>(ReadResponse());
        }



        internal HttpWebRequest CreateRequest()
        {
            var basicRequest = (HttpWebRequest)WebRequest.Create(URL);
            basicRequest.ContentType = Content;
            basicRequest.Method = Verb;
            basicRequest.CookieContainer = CookieContainer;
            basicRequest.Headers.Add("x-oflx-api-key", OnfxApiKey);
            basicRequest.Headers.Add("x-oflx-user", OnfxUser);
            return basicRequest;
        }
        internal void WriteStream(object obj)
        {
            if (obj != null)
            {
                using (var streamWriter = new StreamWriter(HttpRequest.GetRequestStream()))
                {
                    if (obj is string)
                        streamWriter.Write(obj);
                    else
                        streamWriter.Write(JsonConvert.SerializeObject(obj));
                }
            }
        }
        internal String ReadResponse()
        {
            if (HttpResponse != null)
                using (var streamReader = new StreamReader(HttpResponse.GetResponseStream()))
                    return streamReader.ReadToEnd();

            return string.Empty;
        }
        internal String ReadResponseFromError(WebException error)
        {
            using (var streamReader = new StreamReader(error.Response.GetResponseStream()))
                return streamReader.ReadToEnd();
        }


    }
}