# PopLume Web

## Escopo

Estas instrucoes se aplicam ao frontend em `src/App/poplume-web` e complementam
as regras do `AGENTS.md` da raiz.

## Tecnologias

- Angular 22 com componentes standalone
- TypeScript em modo estrito
- SCSS
- Angular Signals
- Angular `HttpClient`
- SSR e renderizacao hibrida
- Vitest
- API REST em ASP.NET Core

## Comandos

- Desenvolvimento: `npm start`
- Build de producao: `npm run build`
- Build continuo: `npm run watch`
- Testes: `npm test`
- Servir o build SSR: `npm run serve:ssr:poplume-web`

No PowerShell, use `npm.cmd` e `ng.cmd` quando a politica de execucao bloquear
os wrappers `.ps1`.

Nao execute esses comandos, nem comandos Docker, sem solicitacao explicita do
usuario.

## Arquitetura

Organize o codigo por funcionalidade:

- `core`: autenticacao, interceptors, guards, configuracao e servicos globais
- `shared`: componentes, diretivas, pipes e utilitarios reutilizaveis
- `features/catalog`: catalogo, categorias e busca
- `features/product`: detalhes do produto
- `features/cart`: carrinho
- `features/checkout`: finalizacao da compra
- `features/account`: conta do cliente

Nao coloque regras de negocio em componentes. Mantenha componentes focados em
apresentacao e coordenacao da interface.

## Convencoes Angular

- Use componentes standalone.
- Prefira Signals para estado local e derivado.
- Use `computed()` para valores derivados.
- Evite subscriptions manuais; prefira Signals, `async` pipe ou
  `takeUntilDestroyed()`.
- Use `ChangeDetectionStrategy.OnPush`.
- Use lazy loading nas rotas de funcionalidades.
- Mantenha componentes pequenos e com responsabilidade unica.
- Use Reactive Forms nos formularios.
- Nao use `any` sem justificativa.

## Integracao com a API

- No navegador, use apenas URLs relativas iniciadas por `/api`.
- Exemplo correto: `/api/Produto`.
- Nao use `localhost:8081` nem o hostname interno `poplume-api`.
- O Nginx encaminha `/api`, `/scalar`, `/openapi` e `/health` para a API.
- Mantenha os contratos da API tipados.
- Prefira gerar os clients a partir do contrato OpenAPI.
- Nao replique regras de negocio pertencentes ao backend.
- Trate erros HTTP de maneira centralizada.
- Nao habilite CORS para corrigir uma URL que deveria usar a mesma origem.
- Nunca armazene tokens, credenciais ou segredos no codigo-fonte.

## SSR e renderizacao hibrida

- O servidor SSR escuta na porta interna `4000`.
- Codigo executado durante SSR nao pode acessar diretamente `window`,
  `document`, `localStorage` ou `sessionStorage`.
- Quando necessario, verifique a plataforma antes de usar APIs exclusivas do
  navegador.
- Considere que uma requisicao HTTP pode ocorrer no servidor ou no navegador.
- Nao use o hostname Docker `poplume-api` em codigo compartilhado com o
  navegador.
- Paginas publicas de produtos e categorias devem favorecer SSR ou SSG.
- Carrinho, checkout e conta podem usar renderizacao no cliente quando isso for
  mais apropriado.
- Dados especificos do usuario nao devem entrar no transfer cache.

## Hosts e proxy

- Hosts autorizados sao fornecidos por `NG_ALLOWED_HOSTS`.
- Headers confiaveis do proxy sao definidos por `NG_TRUST_PROXY_HEADERS`.
- Nao use wildcard em `NG_ALLOWED_HOSTS`.
- O Nginx deve preservar host e porta com
  `proxy_set_header Host $http_host`.
- Ao adicionar um dominio, atualize a allowlist do ambiente de deploy.

## Estilo

- Use kebab-case em nomes de arquivos e seletores.
- Use PascalCase em classes e tipos.
- Use camelCase em variaveis e funcoes.
- Escreva os textos exibidos ao usuario em portugues do Brasil.
- Preserve acessibilidade, HTML semantico e navegacao por teclado.

## Validacao

- Nao execute build, testes ou Docker sem solicitacao explicita do usuario.
- Quando solicitado, verifique TypeScript, testes relacionados, build de
  producao e SSR conforme o escopo.
- Nao altere arquivos fora do escopo da tarefa.

## Git

- Preserve alteracoes existentes do usuario.
- Faca mudancas pequenas e focadas.
- Nao faca commit ou push sem solicitacao explicita.
