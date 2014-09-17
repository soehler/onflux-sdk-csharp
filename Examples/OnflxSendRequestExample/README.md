# Como Receber uma Dados do  Onflux Server

   O OnfluxServer pode se integrar com seus sistemas de retaguarda, executando uma requisição POST para uma URL do seu sistema.  
   As classes `HandleOnflxRequest.cs` e `OnflxResponse.cs`, implementam, em C# ,todos os mecanismos necessários para você "processar" os dados de respostas de formulários enviados para o sistema que você esta integrando com o Onflux.  
   O projeto  OnflxReceiveRequestExample, demonstra como utilizar as classes de integração andleOnflxRequest.cs e OnflxResponse.cs para receber e processar mensagens enviadas pelo OnfluxServer.  

## Para utilizar em seu projeto:

1. Acrescente as classes `HandleOnflxRequest.cs` e `OnflxResponse.cs` ao seu projeto.  
2. Acrescente referência `using OnflxAPICallHandling;` onde você for usar.  

```csharp
OnflxResponse onflxResp;
onflxResp = HandleOnflxRequest.Process(base.Request, base.Response);

//[processe aqui os dados retornados em no objeto OnflxResponse]
```

## Classe HandleOnflxRequest
O método **Process** retorna para você os seguinte resultados:

### SubscriptionConfirmation
SUCESSO, respondendo ao servidor do Onflux, que esta pronto e concorda em receber chamadas.  
onflxResp.payload contém OK.

### SubscriptionError
ERRO, processando chamada de confirmação do servidor Onflux.
onflxResp.payload contém mensagem de Erro.

### SubscriptionConfirmationError
ERRO, fazendo requisição ao servidor do Onflux para confirmar que esta  pronto e concorda em receber chamadas.  
onflxResp.payload contém mensagem de Erro.

### Notification
SUCESSO, resposta de formulário recebida (Este é o tipo de chamada que você vai receber no dia a dia).  
onflxResp.payload contém XML ou JSON da resposta do formulário.

### NotificationError
ERRO, processando resposta de formulário recebida (Entre em contato imediatamente com a True Systems !).  
onflxResp.payload contém mensagem de Erro.

### UnsubscribeConfirmation
Você foi descadastrado e não vai mais receber chamadas do Servidor do Onflux.
(Entre em contato imediatamente com a True Systems caso não tenha cancelado o Serviço Onflux).  
onflxResp.payload contém OK.

### NotOnflxCall
Você recebeu uma chamada que não veio do Onflux.
Alguém ou alguma coisa, que não é o Onflux, está tentando acessar seu webservice.  
onflxResp.payload contém OK.
