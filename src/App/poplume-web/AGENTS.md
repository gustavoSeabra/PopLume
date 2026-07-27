# PopLume Web

## Visão geral

Frontend do e-commerce PopLume, construído com Angular e integrado à API REST
em ASP.NET Core.

## Tecnologias

- Angular 22 com componentes standalone
- TypeScript em modo estrito
- SCSS
- Angular Signals
- Angular `HttpClient`
- SSR e renderização híbrida
- Vitest
- API REST em ASP.NET Core

## Comandos

- Desenvolvimento: `npm start`
- Build de produção: `npm run build`
- Build contínuo: `npm run watch`
- Testes: `npm test`
- Servir o build SSR: `npm run serve:ssr:poplume-web`

No PowerShell, use `npm.cmd` e `ng.cmd` ao executar comandos diretamente caso a
política de execução bloqueie os wrappers `.ps1`.

## Arquitetura

Organize o código por funcionalidade:

- `core`: autenticação, interceptors, guards, configuração e serviços globais
- `shared`: componentes, diretivas, pipes e utilitários reutilizáveis
- `features/catalog`: catálogo, categorias e busca
- `features/product`: detalhes do produto
- `features/cart`: carrinho
- `features/checkout`: finalização da compra
- `features/account`: conta do cliente

Não coloque regras de negócio em componentes. Mantenha componentes focados em
apresentação e coordenação da interface.

## Convenções Angular

- Use componentes standalone.
- Prefira Signals para estado local e derivado.
- Use `computed()` para valores derivados.
- Evite subscriptions manuais; prefira Signals, `async` pipe ou
  `takeUntilDestroyed()`.
- Use `ChangeDetectionStrategy.OnPush`.
- Use lazy loading nas rotas de funcionalidades.
- Mantenha componentes pequenos e com responsabilidade única.
- Use Reactive Forms nos formulários.
- Não use `any` sem justificativa.

## Integração com a API

- Centralize a URL da API na configuração de ambiente.
- Mantenha os contratos da API tipados.
- Prefira gerar os clientes a partir do contrato OpenAPI/Swagger.
- Não replique regras de negócio pertencentes ao backend.
- Trate erros HTTP de maneira centralizada.
- Nunca armazene tokens, credenciais ou segredos no código-fonte.

## SSR e renderização híbrida

- Código executado durante SSR não pode acessar diretamente `window`,
  `document`, `localStorage` ou `sessionStorage`.
- Quando necessário, verifique a plataforma antes de usar APIs exclusivas do
  navegador.
- Páginas públicas de produtos e categorias devem favorecer SSR ou SSG.
- Carrinho, checkout e conta podem usar renderização no cliente quando isso for
  mais apropriado.
- Dados específicos do usuário não devem entrar no transfer cache.
- Considere que uma requisição HTTP pode ocorrer tanto no servidor quanto no
  navegador.

## Estilo

- Use kebab-case em nomes de arquivos e seletores.
- Use PascalCase em classes e tipos.
- Use camelCase em variáveis e funções.
- Escreva os textos exibidos ao usuário em português do Brasil.
- Preserve acessibilidade, HTML semântico e navegação por teclado.

## Validação das alterações

Antes de concluir uma mudança:

1. Execute os testes relacionados.
2. Execute o build de produção.
3. Verifique erros de TypeScript.
4. Verifique o funcionamento com SSR quando aplicável.
5. Não altere arquivos fora do escopo da tarefa.

## Git

- Preserve alterações existentes do usuário.
- Faça mudanças pequenas e focadas.
- Não faça commit ou push sem solicitação explícita.
