# PopLume API

## Escopo

Estas instrucoes se aplicam ao backend localizado em `src/API` e complementam
as regras do `AGENTS.md` da raiz.

## Tecnologias e camadas

- ASP.NET Core 10
- Entity Framework Core 10
- PostgreSQL com Npgsql
- FluentValidation
- Serilog
- OpenAPI e Scalar

Respeite as responsabilidades das camadas:

- `PopLume.Domain`: entidades, contratos e regras de dominio
- `PopLume.Application`: casos de uso, servicos, validacoes e DTOs
- `PopLume.Infrastructure`: EF Core, contexto, repositorios e integracoes
- `PopLume.Api`: controllers, pipeline HTTP, composicao e configuracao
- `PopLume.Tests`: testes automatizados

O dominio nao deve depender de Infrastructure ou API. Nao coloque regras de
negocio em controllers ou no `DbContext`.

## Convencoes da API

- Mantenha controllers pequenos e focados em transporte HTTP.
- Use injecao de dependencias em vez de instanciacao direta de servicos.
- Preserve nullable reference types e implicit usings.
- Use `CancellationToken` em operacoes assincronas quando aplicavel.
- Nao exponha entidades persistidas diretamente se o contrato exigir DTOs.
- Mantenha codigos HTTP coerentes e erros no formato padronizado pelo projeto.
- Centralize o tratamento de excecoes no middleware existente.
- Registre novas dependencias nas extensions de configuracao apropriadas.

## Banco de dados

- O `PopLumeDbContext` pertence a Infrastructure.
- Crie migrations apenas quando houver alteracao intencional do modelo.
- Nao aplique, remova ou reverta migrations sem solicitacao explicita.
- Nao execute comandos contra bancos locais ou remotos sem solicitacao
  explicita.
- Preserve a connection string configuravel por
  `ConnectionStrings__PopLumeApi`.

## Health check

- `/health` e o readiness check da API.
- O endpoint deve retornar sucesso apenas quando a API conseguir conectar ao
  PostgreSQL.
- Mantenha o endpoint fora do documento OpenAPI, salvo decisao contraria.
- O Docker consulta internamente `http://localhost:8080/health`.
- O Nginx expoe externamente `/health`.
- Alteracoes nesse endpoint exigem revisar `dockercompose.yaml` e
  `docker/nginx.conf`.

## Proxy e HTTPS

- A API escuta HTTP na porta interna `8080`.
- O Nginx e responsavel pela entrada publica e pela futura terminacao TLS.
- Preserve `Host`, `X-Forwarded-For` e `X-Forwarded-Proto` enviados pelo Nginx.
- Ao habilitar HTTPS, configure forwarded headers na API e confie somente na
  rede ou no proxy conhecido.
- Nao habilite CORS para mascarar URLs incorretas do proxy. Frontend e API
  devem usar a mesma origem por meio de `/api`.

## Validacao

- Nao execute `dotnet restore`, `dotnet build`, `dotnet test`, migrations ou
  Docker sem solicitacao explicita do usuario.
- Quando solicitado, valide primeiro o projeto diretamente afetado.
- Informe separadamente falhas de codigo, infraestrutura e acesso externo.

## Git

- Nao altere migrations ou contratos publicos fora do escopo da tarefa.
- Nao faca commit ou push sem solicitacao explicita.

