# OAuth 2.0 + OpenID Connect (OIDC)

O objetivo desse projeto é mostrar a implementação prática dos protocolos OAuth 2.0 e OpenID Connect.

Ele consiste de uma API, que vai simular uma aplicação qualquer.

No primeiro caso, vamos fazer nossa API obter acesso a uma pasta no Google Drive, através do fluxo de autorização fornecido pelo OAuth.

No segundo, vamos realizar o login na nossa API através de uma conta Google, através do OIDC.

## Sumário


## Draw

É uma API que vai simular o app draw.io

Essa API possui autenticação via JWT. É possível criar um usuário e logar com ele, recebendo um JWT gerado pela própria API.

Com o usuário logado, vamo fazer nossa API ter acesso ao Google Drive do usuário usando o OAuth.

Também vamos realizar o famoso Login com Google, usando o OIDC. Assim o usuário vai poder logar direto na API, sem a necessidade de realizar um cadastro antes.



## Setup Inicial

- Criar projeto no Google Cloud

- Habilitar o acesso do Draw a API do Google Drive

- Configurar projeto com nome Draw e domínio draw.host (aponta pro localhost)

- Adicionar meu email como usuário de teste

- Adicionar escopo de acesso ao Google Drive que o Draw vai pedir pro usuário
    - https://www.googleapis.com/auth/drive.file
    - Create new Drive files, or modify existing files, that you open with an app or that the user shares with an app while using the Google Picker API or the app's file picker.
    - See, edit, create, and delete only the specific Google Drive files you use with this app

- Adicionar escopo do OIDC para fazermos login depois
    - openid
    - Associate you with your personal info on Google

- Dentro do projeto, criar nossas credenciais:
    - URI de origem -> http://localhost:5001
    - URI de redirect -> http://localhost:5001/oauth/draw-callback
    - ClientId -> 11118065658-9s8e2aj77nguipq43lle8lcidu8vr5kd.apps.googleusercontent.com
    - ClientSecret -> Lalala@123








- OAuth2 serve para autorizar em outro sistema (acesso limitado)
    - Estou no draw.io e quero salvar meus diagramas no Google Drive

- OpenID Connect serve para se autenticar no sistema A, usando uma conta no sistema B
    - Extensão do OAuth2, traz dados (IdToken, /userinfo) sobre o usuário além do token de acesso
    - Quero logar no Syki usando minha conta do Google/LinkedIn/GitHub...

CRIAR UMA API EM .NET PARA SIMULAR O DRAW.IO
ASSIM CONSIGO DEBUGAR TODOS OS PASSOS E VER TODAS AS INFORMAÇÕES

- Trocar draw.io pelos contatos que tenho no gmail?




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


