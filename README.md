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
### 1️⃣ Simulando o Draw.io
### 2️⃣ Setup Inicial



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
    - ClientId                 -> 11118065658-9s8e2aj77nguipq43lle8lcidu8vr5kd.apps.googleusercontent.com
    - ClientSecret             -> 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

* Sim, essas credenciais são reais, mas eu já apaguei elas lá do Google Cloud antes subir o código aqui pro GitHub.






## OAuth2

- Protocolo de Autorização

- Problema:
    - Estou no draw.io e quero salvar meus diagramas no Google Drive
    - Obviamente não quero informar pro draw.io meu email e senha do Google
    - Como resolver isso? Como dar acesso limitado pro draw.io para que ele possa APENAS salvar e buscar dados no meu Google Drive
    - Como posso revogar o acesso do draw.io?
    - O draw.io pode acessar pastas ou documentos privados?
    - Ele pode deletar todos os meus dados?
    - Permissão GRANULAR!

- Setup inicial
    - Cadastrar minha aplicacao no Google
        - ClientId
        - ClientSecret
        - Callback URI
        - Scopes

- Terminologia
    - Resource Owner
        - Eu, o dono das pastas/arquivos que estão no Drive
    - Client
        - Draw.io, a aplicação que está pedindo acesso ao meus recursos
    - Authorization Server
        - Servidor do Google que pergunta se quero deixar o draw.io acessar meus dados
        - Ele conhece meu email e minha senha (hash)
        - Ele conhece o draw.io (id + secret)
        - Ele conhece o Google Drive
        - Ele pode emitir tokens de acesso, usados para ler/salvar dados no Drive
    - Resource Server
        - Google Drive, onde estão meus recursos (pastas e arquivos)
    - Authorization Grant
        - Flows -> Code | Implicit | Password Credentials | Client Credentials
        - Response Type
        - Response Mode
        - Code que prova que eu permiti o acesso do draw.io ao meu Drive
        - O que o draw.io espera receber no callback quando o usuário permitir seu acesso ao Drive
        - Usado no backend pelo draw.io para conseguir um JWT, que dá acesso ao Drive do usuário
    - Redirect URI
        - Callback URI (fica no draw.io)
        - URI que o draw.io usa para pegar o token gerado no Authorization Server quando o usuário permite
    - Access Token
        - JWT usado pelo draw.io para realizar a comunicação backend com o Drive (ler e salvar arquivos)
    - Scope
        - Permissões que o token possui
        - Ex: criar arquivos numa pasta / ler arquivos de uma pasta / deletar pasta
        - Elas sao mostradas na tela de consentimento, quando o auth server perguntar se quero dar permissao
    - State
        - Prova que eu iniciei e terminei o fluxo?
    - Nonce

    - Front Channel
        - Comunicacao via frontend (acontece no navegador) MENOS SEGURO
        - Onde ocorre todo o fluxo ate a obtencao do Authorization Grant (code)
    - Back Channel
        - Comunicacao via backend (server -> server) HTTPS / ALTAMENTE SEGURO





- PKCE (Proof Key for Code Exchange)

- Scopes
    - id, name, email...

- Flows
    - Authorization Code
    - 



## OpenID Connect

- Protocolo de Autenticação

- Extensão do OAuth2
    - ID Token (JWT, assim como o Access Token)
    - Userinfo Endpoint
    - Standard set of scopes (openid)





## Cookies

- Set-Cookie Header


## Autenticacao

- Via ApiKey, JWT, Cookie...
- Logar com Google, GitHub, Facebook...
- Apresento credenciais e recebo um token/cookie/key de acesso

- Possui varios Schemas (Cookie, Bearer, OAuth)

## Referências

- OAuth 2.0 and OpenID Connect (in plain English) (https://youtu.be/996OiexHze0)
- O que você deveria saber sobre Oauth 2.0 e OpenID! (https://youtu.be/68azMcqPpyo)
- An Illustrated Guide to OAuth and OpenID Connect (https://youtu.be/t18YB3xDfXI)

- An introduction to OpenID Connect in ASP.NET Core (https://andrewlock.net/an-introduction-to-openid-connect-in-asp-net-core)

- ASP.NET Core OAuth Authorization (.NET 7 Minimal Apis C#) (https://youtu.be/0uSwPdYOm9k)
- How to secure ASP.NET Core with OAuth and JSON Web Tokens (https://blog.elmah.io/how-to-secure-asp-net-core-with-oauth-and-json-web-tokens/)


