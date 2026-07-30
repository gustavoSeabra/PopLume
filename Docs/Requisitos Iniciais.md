# Definições do E-comerce PopLume

Este projeto visa a construção de um e-comerce para a loja Poplume, expecializada em produtos de impressão 3D.
Seu layout deve seguir as regras:
- Abordagem responsiva para celular, tablet e desktop. Contendo acessibilidade e navegação por teclado
- Feedback ao adicionar itens ao carrinho
- Paginação e / ou carregamento progressivo
- Consentimento e tratamento de dados conforme a LGPD
- Uma identidade mais sofisticada e divertida

## Sobre a referência visual
#### As cores principais:
Verde-menta: #7FE7C4
Verde-petróleo: #0F5F63
Lilás: #C9B6FF
Roxo: #7A4DFF
Preto: #111111

#### Textos:
**Títulos** em Poppins Bold
**Subtítulos** em Poppins Medium
**Corpo** em Inter Regular

A combinação transmite bem produtos criativos, personalizados e impressão 3D. O preto e o verde-petróleo dão estrutura, enquanto o roxo e o menta funcionam bem como cores de destaque.

---

## Perfis e permissões

Abaixo uma tabela contendo os perfis e o que cada um pode fazer nesta etapa do projeto.

| Recurso | Cliente | Funcionário | Administrador |
|---|---:|---:|---:|
| Editar a própria conta | Sim | Sim | Sim |
| Consultar pedidos próprios | Sim | Não | Sim |
| Gerenciar produtos | Não | Conforme permissão | Sim |
| Gerenciar clientes | Não | Conforme permissão | Sim |
| Gerenciar funcionários | Não | Não | Sim |
| Acessar relatórios | Não | Conforme permissão | Sim |
| Cadastro de banner (carrossel home) | Não | Sim | Sim |


O perfil funcionário não pode ser um cliente simultaneamente. Para isto, ele precisa ter uma conta com perfil de cliente e uma conta com perfil de funcionário.

## Fluxo público
Além da home, o e-commerce também terá:
 - Catálogo e categorias
 - Busca e filtros
 - Página de detalhes do produto
 - Carrinho
 - Login, cadastro e recuperação de senha
 - Checkout
 - Endereço, entrega e pagamento
 - Confirmação e acompanhamento do pedido
 - Políticas de privacidade, troca, devolução e termos de uso

Essas páginas não vão entrar no primeiro layout, mas devem existir no mapa do produto.

## Home

Detalhes da Home pontos precisam ser esclarecidos:

- Cabeçalho com logomarca, menu, botão de busca de produtos, botão para acessar o menu de usuário logado e botão do carrinho que irá conter a quantidade de produtos colocados no carrinho pelo usuário.
- Um banner grande com texto e alguns exemplos de produtos que a loja faz
- Vitrine dos novos produtos em destaque
- Rodapé  com 5 colunas sendo:
  - Logomarca
  - Menu sobre a empresa
  - Menu sobre ajuda
  - Dados de contato como telefone, links para rede social
  - Banner para o usuário cadastrar seu e-mail para newsletter


Páginas filhas que terão seus links a partir da home (ainda não definido)

Regras da home page:

- O botão de usuário na área deslogada deve abrir login/cadastro.
- O botão de usuário na área deslogada, após login, deve abrir:
  - Formulário de edição de dados de usuário
  - Formulário para troca de senha
  - Formulário de histórico de compras
  - Botão Sair (efetuar logof)
- A quantidade de ítens no carrinho ficará disponível mesmo sem autenticação, mas quando o usuário avançar para finalizar a compra, ele deve estar logado. Caso contrário, direciona-lo para efetuar login e após login, volta para o carrinho.
- O carrinho anônimo será preservado ao fazer login
- Inicialmente as categorias que aparecerão no menu são:
  - Decoração
  - Utilitários
  - Presentes
  - Area Pet
  - Personalizados
- Os produtos em destaques serão administráveis. Existe uma flag no cadastro do produto onde informará se ele é destaque ou não.
- O banner principal será carrossel e gerenciado pelo sistema. Vamos ter um cadastro de banner para ser usado no carrossel.
- Redes sociais também ficarão dentro da coluna de contato, chegando então às cinco pretendidas.


## Área administrativa

Os primeiros módulos do sistema serão produtos, equipamentos, filamentos, marketplaces, clientes e relatórios contendo:
- Listagem
- Busca e filtros
- Cadastro com máscaras e validações dos formulários
- Edição
- Visualização
- Ativação/inativação
- Paginação
- Confirmação de operações
- Estados de loading, erro, vazio, indisponibilidade e erro

O único módulo vendável é produto. Os demais, são cadastros para controle interno e montagem de preço de venda.


# Particularidades da loja

O cliente não poderá enviar arquivos. Caso ele queira algum produto personalizado, deverá entrar em contato conosco a partir dos dados de contato informados na home page.
Haverá uma módulo de orçamento, onde o funcionário ou adiminstrador irá selecionar o produto, equipamento, filamento e canal de venda (marketplaces) e o preço será calculado automaticamente. Ao final, o mesmo será adicionado no registro do produto.
Produtos não terão variações
No MVP não haverá controle de estoque. Isto é uma feature futura.
Filamentos serão usados apenas na produção e composição de preço do produto.
Marketplaces servirão apenas para registrar canais de venda. No futuro, podemos pensar em uma integração. Mas para o MVP não.