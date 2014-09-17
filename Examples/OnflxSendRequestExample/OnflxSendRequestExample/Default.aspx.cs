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
using Newtonsoft.Json;
using OnflxFormSchemaV1;

namespace OnflxSendRequestExample
{

    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            OnflxRequest objRequest;
            OnflxResponse objResponse;
            String jsonFormulario;


            // Le o arquivo que contem o formulario de teste no formato json
            jsonFormulario = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "FormularioPacienteTeste.txt");
            //Deserializa JSon para classe .Net
            Form objFormulario = JsonConvert.DeserializeObject<Form>(jsonFormulario, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto, MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead });



            // Envia 5 formularios de paciente (de teste) para o usuário administrador (id 633)
            for (int i = 0; i < 5; i++)
            {
                String g = Guid.NewGuid().ToString(); //log filename + .txt sufixo

                objRequest = new OnflxRequest();
                // Carrega o json do formulario e troca o id (nova guid sem os hifens)
                objFormulario.responseId = g.Replace("-", "");


                // envia o formulario
                objResponse = (OnflxResponse)objRequest.Execute(objFormulario, "633");

                switch (objResponse.status)
                {
                    case "200":
                        logResponse(objResponse);
                        break;

                    case "201": // Form already processed.
                        logResponse(objResponse);
                        break;

                    case "900": // Empty post.
                        logResponse(objResponse);
                        break;

                    case "901": // Empty x-oflx-api-key and/or x-oflx-user information.
                        logResponse(objResponse);
                        break;

                    case "902": // Invalid x-oflx-api-key and/or x-oflx-user information.
                        logResponse(objResponse);
                        break;

                    case "903": //Invalid form. Details: [schema validation fail details].
                        logResponse(objResponse);
                        break;

                    case "909": //Error processing form: [error message].
                        logResponse(objResponse);
                        break;
                }
            }
        }

        internal void logResponse(OnflxResponse objResponse)
        {
            /* 
             * Grava resultados em um folder do servidor
             * verifique se o contexto de usuário da aplicação tem acesso de Read/Write/Create no subfolder log
             * Remova estas linhas quando for usar em produção
             * 
             */
            Guid x = Guid.NewGuid(); //log filename + .txt sufixo
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "log\\" + x + ".txt", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss") + "\n");
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "log\\" + x + ".txt", objResponse.status + "\n");
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "log\\" + x + ".txt", objResponse.message);
        }

    }


}