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
using System.Collections.Specialized;
using System.Net;
using System.IO;


namespace OnflxAPICallHandling
{
    public static class HandleOnflxRequest
    {

        public static OnflxResponse Process(HttpRequest request, HttpResponse response)
        {
            NameValueCollection headers = request.Headers;
            String messageType = "";        // valor retornado na propriedade result do objeto OnflxResponse
            String messagePayload = "";     // valor retornado na propriedade payload do objeto OnflxResponse
            String subscribeUrl = "";
            String subscription = ""; 

            OnflxResponse responseFromOnflx = new OnflxResponse();

            //Procura por header no request que indica que esta é uma chamada do Onflux
            for (int i = 0; i < headers.Count; i++)
            {
                string key = headers.GetKey(i);
                string value = headers.Get(i);
                if (key == "x-amz-sns-message-type")
                {
                    messageType = value;
                    break;
                }
            }

            /*
             * Dependendo do message Type(Json name/value pair) processa Message(Json name/value pair):
             * 
             * Se Type for Notification, retorna a resposta do formulário em XML ou JSon  na propriedade 
             * payload do objeto OnflxResponse.
             * Para qualquer outro Type é retornado em "plain text" uma menssagem de erro, caso ocorra
             * algum problema ou "OK", indicando sucesso na operação.
             * Na ocorrência de erro nas operaçãoes de SubscriptionConfirmation ou Notification 
             * o campo result retorna respectivamente SubscriptionError, NotificationError
             * No caso da chamada não ser uma menssagem válida, retorna NotSNSCall no campo result
            */
            switch (messageType)
            {
                // Ocorre somente na primeira vez que a URL é chamada, para confirmar que o sistema esta apto
                // a receber menssagens contendo dados.
                case "SubscriptionConfirmation":
                    subscribeUrl = getSubscribeUrl(request);
                    if (subscribeUrl.StartsWith("#ERROR#"))
                    {
                        //Erro no SubscriptionConfirmation, muda result para SubscriptionError e retorna messagem de erro em payload.
                        responseFromOnflx.result = "SubscriptionError";
                        messagePayload = messagePayload.Replace("#ERROR#", "");
                    }
                    else
                    {
                        // Executa chamada(GET) para confirmar a Subscription.
                        subscription = subscribe(subscribeUrl);
                        
                        if (subscription.StartsWith("#ERROR#"))
                        {
                            //Erro no SubscriptionConfirmationError, muda result para SubscriptionConfirmatioError e retorna messagem 
                            // de erro em payload.
                            responseFromOnflx.result = "SubscriptionConfirmationError";
                            messagePayload = messagePayload.Replace("#ERROR#", "");
                        } else{
                            //Sucesso em  SubscriptionConfirmation, "OK" em payload.
                            responseFromOnflx.result = "SubscriptionConfirmation";
                            messagePayload = "OK"; 
                        }
                    }
                    break;

                case "Notification":
                    // recupera resposta do formulário.
                    messagePayload = getPayload(request);
                    
                    if (messagePayload.StartsWith("#ERROR#"))
                    {
                        //Erro na Notification, muda result para NotificationError e retorna messagem de erro em payload.
                        responseFromOnflx.result = "NotificationError";
                        messagePayload = messagePayload.Replace("#ERROR#", "");
                    }
                    else
                    {
                        //Notification recebida com sucesso, "OK" em payloadpayload vai conter respostado formulário.
                        responseFromOnflx.result = "Notification";
                    }
                    break;
                case "UnsubscribeConfirmation":
                    // Indica que este endereço não vai mair receber notificações do Onflux Server, pois as mesmas
                    // foram suspensas.
                    responseFromOnflx.result = "UnsubscribeConfirmation";
                    messagePayload = "OK";
                    break;
                default:
                    // Chamada recebida não contém uma menssagem do Onflux.
                    responseFromOnflx.result = "NotOnflxCall";
                    messagePayload = "OK";
                    break;
            }

            // Retorna objeto OnflxResponse.  
            responseFromOnflx.payload = messagePayload;
            return responseFromOnflx;

        }

        /*
         * Função para extrair resposta do formuário do Request HTTP recebido.
         */
        private static String getPayload(HttpRequest request)
        {

            String message = "";

            try
            {
                System.Collections.Generic.Dictionary<String, Object> myObjectDictionary;
                myObjectDictionary = parseJsonFromStream(request);
                message = myObjectDictionary["Message"].ToString();

            }
            catch (Exception e)
            {
                message = "#ERROR#" + e.Message;
            }

            return message;

        }

        /*
         * Função para extrair a URL que deve ser chamada para confirmar que o serviço 
         * está apto para receber chamadas do Onflux.
         */
        private static String getSubscribeUrl(HttpRequest request)
        {

            String subscribeUrl = "";

            try
            {
                
                System.Collections.Generic.Dictionary<String, Object> myObjectDictionary;
                myObjectDictionary = parseJsonFromStream(request);
                subscribeUrl = myObjectDictionary["SubscribeURL"].ToString();

            }
            catch (Exception e)
            {
                subscribeUrl = "#ERROR#" + e.Message;
            }

            return subscribeUrl;
        }

        /*
         * Helper fuction utilizada para fazer parsing de JSon e para a extração de name/value pairs.
         */
        private static System.Collections.Generic.Dictionary<String, Object> parseJsonFromStream(HttpRequest request)
        {
            //Converte o Request.InputStream para string.
            byte[] myByteArray = new byte[request.InputStream.Length];
            request.InputStream.Read(myByteArray, 0, Convert.ToInt32(request.InputStream.Length));
            String inputStreamContents;
            inputStreamContents = System.Text.Encoding.UTF8.GetString(myByteArray);

            // Converte o JSON para um Dictionary.  Para usar o System.Web.Script namespace é necessária uma referência para
            // System.Web.Extensions.dll.
            System.Web.Script.Serialization.JavaScriptSerializer myJavaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            System.Collections.Generic.Dictionary<String, Object> myObjectDictionary;
            myObjectDictionary = myJavaScriptSerializer.DeserializeObject(inputStreamContents) as System.Collections.Generic.Dictionary<String, Object>;

            return myObjectDictionary;
        }

        /*
         * Executa chamada ao Onflux (usando url extraida por getSubscribeUrl) para confirmar 
         * que o sistema está apto para receber chamadas do Onflux.
         */
        private static String subscribe(String subscribeUrl)
        {
            HttpWebResponse response;

            try
            {

                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(subscribeUrl);
                response = (HttpWebResponse)wrGETURL.GetResponse();
                if (response.StatusDescription.StartsWith("OK"))
                {
                    return "OK";
                }
                else
                {
                    return "#ERROR#" + response.StatusDescription;
                }

            }
            catch (Exception e)
            {
                return "#ERROR#" + e.Message;
            }           
           
        }

    }
}