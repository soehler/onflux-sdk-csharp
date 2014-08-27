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
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using OnflxAPICallHandling;

namespace OnflxReceiveRequestExample
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * Para receber chamadas do Onflux Server:
             * 
             *   1) Acrescente as classes HandleOnflxRequest.cs e OnflxResponse.cs ao seu projeto 
             *   2) Acrescentar referência "using OnflxAPICallHandling;" onde você for usar.
             *   3) Utilize conforme exemplo abaixo
             */
            OnflxResponse onflxResp;
            onflxResp = HandleOnflxRequest.Process(base.Request, base.Response);

            switch (onflxResp.result)
            {
                case "SubscriptionConfirmation":
                    /*
                     *  SUCESSO respondendo ao servidor do Onflux de que esta pronto e concorda em receber chamadas.
                     *  onflxResp.payload contém OK. 
                     */
                    break;
                case "SubscriptionError":
                    /*
                     * ERRO, processando chamada de confirmação do servidor Onflux. 
                     * onflxResp.payload contém menssagem de erro. 
                     */
                    break;
                case "SubscriptionConfirmationError":
                    /*
                    * ERRO, fazendo requisição ao servidor do Onflux, para confirmar que esta pronto e concorda em receber chamadas.
                    * onflxResp.payload contém menssagem de erro. 
                    */
                    break;
                case "Notification":
                    /*
                     *  SUCESSO, resposta de formulário recebida (este é o tipo de chamado que você vai receber no dia a dia).
                     *  onflxResp.payload contém XML ou JSON da resposta do formulário. 
                     */
                    break;
                case "NotificationError":
                    /*
                     * ERRO, processando Resposta de formulário recebida (entre em contato imediatamente com a True Systems !).
                     * onflxResp.payload contém menssagem de erro. 
                     */
                    break;
                case "UnsubscribeConfirmation":
                    /*
                     * Você foi descadastrado e não vai mais receber chamadas do Onflux. 
                     * (Entre em contato imediatamente com a True Systems caso não tenha cancelado o Serviço Onflux)
                     * onflxResp.payload contém OK.
                     */
                    break;
                case "NotOnflxCall":
                    /*
                     * Você recebeu uma chamada que não veio do Onflux. 
                     * Alguém ou alguma coisa, que não é o Onflux, está tentando acessar seu webservice.
                     * onflxResp.payload contém OK.
                     */
                    break;
            }

            /* 
             * Grava resultados em um folder do servidor
             * verifique se o contexto de usuário da aplicação tem acesso de Read/Write/Create no subfolder log
             * Remova estas linhas quando for usar em produção
             * 
             */
            Guid g = Guid.NewGuid(); //log filename + .txt sufixo
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "log\\" + g + ".txt", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") + "\n"); 
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "log\\" + g + ".txt", onflxResp.result + "\n");
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "log\\" + g + ".txt", onflxResp.payload);
        }
    }
}