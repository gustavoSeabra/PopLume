# PopLume

## Visao geral

O PopLume e um e-commerce composto por:

- API REST em ASP.NET Core 10
- frontend em Angular 22 com SSR
- PostgreSQL
- Nginx como proxy reverso
- Docker Compose para o ambiente integrado

As instrucoes deste arquivo valem para todo o repositorio. Arquivos `AGENTS.md`
mais especificos complementam ou substituem estas regras dentro de seus
respectivos diretorios.

## Estrutura do repositorio

- `src/API`: backend .NET
- `src/App/poplume-web`: frontend Angular
- `docker`: Dockerfiles e configuracao do Nginx
- `dockercompose.yaml`: orquestracao local dos servicos
- `DB`: recursos relacionados ao banco de dados
- `LandingPage`: recursos da landing page

## Arquitetura de execucao

O fluxo HTTP local e:

```text
Navegador
   |
   v
Nginx :8080
   |-- /api, /scalar, /openapi e /health --> ASP.NET Core :8080
   `-- demais rotas ----------------------> Angular SSR :4000
                                                   |
PostgreSQL :5432 <---------- ASP.NET Core :8080 ----'
```

- Nginx e a unica entrada HTTP da aplicacao publicada para o navegador.
- A API e o Angular ficam acessiveis por seus nomes de servico apenas na rede
  interna do Docker.
- A porta `5432` do PostgreSQL esta publicada no host para desenvolvimento.
- O Nginx deve preservar o host original com `proxy_set_header Host $http_host`.
- O frontend deve chamar a API por URLs relativas iniciadas por `/api`.
- Nao adicione chamadas do navegador para hostnames internos como
  `poplume-api`.

## Servicos e health checks

- `db` usa `pg_isready` como health check.
- `poplume-api` expoe `/health`, que valida tambem a conexao com o PostgreSQL.
- `poplume-app` aguarda `poplume-api` ficar saudavel.
- `proxy` aguarda a API saudavel e o frontend iniciado.
- Preserve `condition: service_healthy` ao alterar dependencias do Compose.
- Uma rota de liveness futura nao deve substituir `/health` como readiness
  check sem uma decisao explicita.

## Seguranca e configuracao

- Nunca adicione novos segredos, tokens ou credenciais reais ao repositorio.
- Prefira variaveis de ambiente, secrets do ambiente de deploy ou arquivos
  locais ignorados pelo Git.
- Nao exponha diretamente API ou frontend no host sem necessidade.
- Preserve a validacao de hosts do Angular SSR.
- Nao use `NG_ALLOWED_HOSTS: "*"`; liste os hosts autorizados.
- So confie em headers `X-Forwarded-*` sobrescritos pelo proxy controlado pelo
  projeto.
- Em producao, o TLS deve terminar no proxy reverso.

## Operacao e validacao

- Nao execute builds, testes, restauracao de pacotes, containers ou comandos
  Docker sem solicitacao explicita do usuario.
- Quando a execucao nao for solicitada, limite a validacao a inspecoes
  estaticas e informe quais comandos o usuario pode executar.
- No PowerShell, prefira os executaveis `.cmd` para npm e Angular quando a
  politica de execucao bloquear wrappers `.ps1`.
- Nao altere portas, nomes de servicos, rotas do proxy ou contratos entre
  containers sem verificar todos os consumidores.

## Git

- Preserve alteracoes existentes do usuario.
- Mantenha as mudancas pequenas e focadas no pedido atual.
- Nao faca commit, push, merge ou abertura de pull request sem solicitacao
  explicita.
- Nao inclua artefatos de build, dependencias restauradas ou segredos no Git.

