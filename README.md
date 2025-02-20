# OAuth 2.0 + OpenID Connect (OIDC)

O objetivo desse projeto é mostrar na prática o uso dos protocolos OAuth 2.0 e OpenID Connect.

Para facilitar o entendimento, vamos supor que você quer usar uma aplicação web para desenhar diagramas de arquitetura de software, como o Draw.io.

No primeiro cenário, você vai se cadastrar na aplicação, informar nome e email, confirmar seu email, definir sua senha e enfim logar no sistema.
O Draw.io possui integração com o Google Drive, o que permite que você salve seus diagramas na nuvem e os acesse de qualquer lugar.
Mas como você pode habilitar essa funcionalidade de maneira simples e segura?
Como garantir que o Draw.io vai poder acessar apenas os arquivos que você **autorizar**?
Resposta: usando o OAuth 2.0!

No segundo cenário, suponha que você não quer passar por todo o fluxo de se cadastrar no sistema, confirmar seu email, definir senha e só então logar no app.
Afinal, você já está logado na sua conta Google, poderia muito bem usar ela para se **autenticar** no Draw.io.
Mas novamente, como isso pode ser feito, de maneira simples e segura?
Resposta: usando o OpenID Connect!

## Sumário
- 1️⃣ Simulando o Draw.io
- 2️⃣ Setup Inicial



## 1️⃣ Simulando o Draw.io

Para ver na prática como esses protocolos funcionam, criei uma API em .NET bem simples, que vai simular a aplicação do Draw.io nos dois cenários descritos anteriormente.
Vou me referir a essa API como DrawApp de agora em diante.

Ela possui os seguintes endpoints:
- POST /users
    - Cria um usuário, informando nome, email e senha
- POST /login
    - Realiza o login no sistema, informando email e senha
    - A autenticação é feita via Cookie

- GET /users/data
    - Retorna alguns dados do usuário logado
    - Sendo eles nome, email e se a integração com o Google Drive está ativada

- GET /oauth/google-drive
    - Redireciona o usuário logado para a tela de consenso do Google
    - Nela o usuário pode autorizar que o DrawApp salve dados no seu Google Drive
- POST /google-drive/files
    - Permite a criação de arquivos no Google Drive do usuário, caso ele tenha autorizado o acesso usando o endpoint anterior

- GET /login/google
    - Redireciona o usuário deslogado para a tela de consenso do Google
    - Nela o usuário pode autorizar que o DrawApp tenha acesso ao seu nome, email e foto de perfil
    - Se o usuário permitir, a API automaticamente realiza seu cadastro e o loga no sistema

<p align="center">
  <img src="./DrawApp//Docs/00_draw_app_api.gif" width="800" style="display: block; margin: 0 auto" />
</p>

Estou usando o Postgres para salvar todos os dados do DrawApp.

## 2️⃣ Setup Inicial

Antes de mais nada, é razoável pensar que o DrawApp precise estar previamente configurado no Google para que tudo isso funcione.
Afinal, quando o usuário é redirecionado pra tela de consenso, o Google já conhece o DrawApp e sabe quais permissões ele deseja obter do usuário.

Seguem os principais passos para realizar esse setup inicial:
- Criar projeto no Google Cloud e configurar seu nome como DrawApp
- Habilitar o acesso do DrawApp à API do Google Drive
- Adicionar meu email como usuário de teste

- Adicionar escopo de acesso ao Google Drive que o DrawApp vai pedir pro usuário no fluxo de autorização (OAuth 2.0)
    - Vamos usar o escopo "drive.file", que permite **apenas** a criação/edição de arquivos que o usuário utilizou no DrawApp

- Adicionar escopos para que o DrawApp tenha acesso aos dados necessários para realizar o login via conta Google
    - Aqui vamos usar os escopos "openid" e "userinfo.email"
    - Eles juntos retornam dados pessoais do usuário, como nome, email e foto de perfil

- Dentro do projeto, criar nossas credenciais:
    - URI de origem            -> http://localhost:5001
    - URI de callback do OAuth -> http://localhost:5001/oauth/drawapp-callback
    - URI de callback do OIDC  -> http://localhost:5001/oidc/drawapp-callback
    - ClientId                 -> Identifica o DrawApp dentro do Authorization Server
    - ClientSecret             -> Autentica o DrawApp dentro do Authorization Server

## 3️⃣ Autorização com OAuth 2.0

Vamos voltar pro primeiro cenário apresentado lá no começo:

Você vai se cadastrar no DrawApp, informar nome e email, confirmar seu email, definir sua senha e enfim logar no sistema.
O DrawApp possui integração com o Google Drive, o que permite que você salve seus diagramas na nuvem e os acesse de qualquer lugar.
Mas como você pode habilitar essa funcionalidade de maneira simples e segura?
Como garantir que o DrawApp vai poder acessar apenas os arquivos que você **autorizar**?
E indo além, como posso revogar o acesso do DrawApp ao meu Google Drive?

Como você já sabe, podemos atingir esses objetivos usando o OAuth, pois ele é um protocolo de autorização criado justamente para problemas desse tipo (Delegated Authorization).

Vamos definir alguns termos antes:

- Resource Owner
    - O usuário, que usa o DrawApp e é dono da conta no Google Drive
- Client
    - DrawApp, a aplicação que está pedindo permissão ao usuário para acessar seus arquivos no Google Drive
- Resource Server
    - Google Drive, onde estão os recursos (arquivos) do usuário
- Authorization Server
    - Servidor do Google que intermedia todos os fluxos
    - Ele é o responsável pela emissão de códigos e tokens de acesso

- Scopes
    - São as permissões que o DrawApp quer receber do usuário (mostradas na tela de consentimento do Authorization Server)
- Redirect URI
    - Callback URI (http://localhost:5001/oauth/drawapp-callback)
    - O Authorization Server redireciona o usuário pra essa URI quando ele permite que o DrawApp tenha acesso ao Google Drive
- Authorization Code
    - Esse código é a prova que o usuário clicou em "Permitir acesso" na tela de consentimento
- Access Token
    - O DrawApp utiliza o Authorization Code, juntamente com seu ClientId e ClientSecret, para obter esse token no Authorization Server
    - O token obtido permite que o DrawApp tenha acesso ao Google Drive do usuário

-------------------------------------------------
- Authorization Grant
    - Response Mode
- State
    - Prova que eu iniciei e terminei o fluxo?
- Nonce
    - String aleatória
- PKCE (Proof Key for Code Exchange)
-------------------------------------------------

Agora sim podemos descrever os fluxo completo:
- 0️⃣ Usuário informa nome e email para se cadastrar no DrawApp, define sua senha e realiza o login
- 1️⃣ Uma vez logado, ele acessa o endpoint GET /oauth/google-drive para permitir que o DrawApp possa salvar arquivos na sua conta do Google Drive
- 2️⃣ Ao acessar esse endpoint, o DrawApp monta a seguinte url e redireciona o usuário pro Authorization Server através dela

google.com./lalala

DESCREVER O QUE CADA PARAMETRO SIGNIFICA!

- 3️⃣ Agora na página de consentimento do Authorization Server, o usuário pode ver quais escopos o DrawApp quer acessar. Ao clicar em "Permitir acesso", o Authorization Server irá gerar um Authorization Code e enviá-lo pro DrawApp ao redirecionar o usuário pra Callback URI definida no setup inicial:

http://localhost:5001/oauth/drawapp-callback?lalala=fewf

DESCREVER O QUE CADA PARAMETRO SIGNIFICA!

- 4️⃣ Ao receber os dados, o DrawApp utiliza o Authorization Code (juntamente com ClientId e ClientSecret) para realizar uma chamada pra API do Authorization Server (https://oauth2.googleapis.com/token), que valida todas as informações e retorna um Access Token pro DrawApp. Esse token é então salvo no banco de dados e toda vez que o usuário quiser salvar um diagrama no seu Google Drive, basta que o DrawApp utilize-o nas chamadas de API.

- 5️⃣ Se o usuário não quiser mais permitir o acesso do DrawApp ao seu Google Drive, basta acessar sua conta Google e revogar as permissões dadas anteriormente.

FAZER ISSO E VALIDAR QUE O ACCESS TOKEN REALMENTE N FUNCIONA MAIS...

## 4️⃣ Autenticação com OpenID Connect

Como acabamos de ver, o OAuth é desenhado apenas para resolver problemas de autorização. No entanto, com algumas modificações, daria pra extendê-lo e assim usá-lo como um protocolo de autenticação. Assim nasce o protocolo OpenID Connect!

Perceba que o OAuth não entrega pro DrawApp nenhuma informação pessoal do usuário, como nome ou email, que poderiam ser usadas para autenticálo no DrawApp. O OIDC surge como uma camada acima do OAuth, definindo um padrão para que esses dados sobre o usuário cheguem até o DrawApp.

Dessa forma, o OIDC possibilita que o usuário utilize sua conta Google para se cadastrar e logar no DrawApp de maneira automática, apenas com alguns cliques.




- Protocolo de Autenticação

- Extensão do OAuth2
    - ID Token (JWT, assim como o Access Token)
    - Userinfo Endpoint
    - Standard set of scopes (openid)





## Referências

- OAuth 2.0 and OpenID Connect (in plain English) (https://youtu.be/996OiexHze0)
- An Illustrated Guide to OAuth and OpenID Connect (https://youtu.be/t18YB3xDfXI)
- ASP.NET Core OAuth Authorization (.NET 7 Minimal Apis C#) (https://youtu.be/0uSwPdYOm9k)
- An introduction to OpenID Connect in ASP.NET Core (https://andrewlock.net/an-introduction-to-openid-connect-in-asp-net-core)
- How to secure ASP.NET Core with OAuth and JSON Web Tokens (https://blog.elmah.io/how-to-secure-asp-net-core-with-oauth-and-json-web-tokens/)
