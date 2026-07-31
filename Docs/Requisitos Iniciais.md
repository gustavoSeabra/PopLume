# Definições do E-commerce PopLume

Este projeto visa a construção de um e-commerce para a loja Poplume, especializada em produtos de impressão 3D.
Seu layout deve seguir as regras:

- Abordagem responsiva para celular, tablet e desktop. Contendo acessibilidade e navegação por teclado
- Feedback ao adicionar itens ao carrinho
- Paginação e / ou carregamento progressivo
- Consentimento e tratamento de dados conforme a LGPD
- Uma identidade mais sofisticada e divertida

## Sobre a referência visual
#### As cores principais:
| Nome | Hexadecimal |
|---|---:|
| Verde-menta | #7FE7C4 |
| Verde-petróleo | #0F5F63 |
| Lilás | #C9B6FF |
| Roxo | #7A4DFF |
| Preto | #111111 |

#### Textos:
**Títulos** em Poppins Bold
**Subtítulos** em Poppins Medium
**Corpo** em Inter Regular

A combinação transmite bem produtos criativos, personalizados e impressão 3D. O preto e o verde-petróleo dão estrutura, enquanto o roxo e o menta funcionam bem como cores de destaque.

# Particularidades da loja

No MVP, a única variação comercial do produto será a cor. Tamanho, material e demais características não poderão ser escolhidos pelo cliente. Abaixo a regra do produto:

- Cada produto terá uma ou mais cores disponíveis.
- Cada cor estará associada a um filamento e a um preço.
- A escolha da cor será obrigatória antes de adicionar o produto ao carrinho.
- O produto poderá possuir imagens associadas a cada cor.
- O mesmo produto em cores diferentes será tratado como itens distintos no carrinho.

O cliente não poderá enviar arquivos. Caso ele queira algum produto personalizado, deverá entrar em contato conosco a partir dos dados de contato informados na home page.

Haverá um módulo de orçamento, onde o funcionário ou administrador irá selecionar o produto, equipamento, filamento e canal de venda (marketplaces) e o preço será calculado automaticamente. Ao final, o preço calculado será adicionado no registro do produto.

Filamentos serão usados apenas na produção e composição de preço do produto.

Marketplaces servirão apenas para registrar canais de venda. No futuro, poderá ser considerada uma integração, mas ela não fará parte do MVP.

## Escopo do MVP

O MVP será um catálogo digital de produtos. O cliente poderá navegar pelo
catálogo, pesquisar produtos, aplicar filtros, consultar os detalhes e
adicionar produtos ao carrinho.

O carrinho funcionará como uma lista de interesse. A conclusão da compra não
será realizada diretamente pela aplicação. Ao finalizar, o cliente será
direcionado para um canal de atendimento da loja, inicialmente o WhatsApp,
com a relação dos produtos e quantidades selecionados.

O MVP não terá pagamento online, cálculo de frete, criação de pedidos pelo
cliente, acompanhamento de pedidos ou integração com marketplaces.

Essas funcionalidades poderão ser adicionadas em evoluções futuras.

## Perfis e permissões

Abaixo uma tabela contendo os perfis e o que cada um pode fazer nesta etapa do projeto.

| Recurso | Cliente | Funcionário | Administrador | MVP |
|---|---:|---:|---:|---:|
| Editar a própria conta | Sim | Sim | Sim | Sim |
| Consultar pedidos próprios | Sim | Não | Sim | Não |
| Gerenciar produtos | Não | Sim | Sim | Sim |
| Gerenciar clientes | Não | Não, apenas envio de link para trocar senha | Sim | Sim |
| Gerenciar funcionários | Não | Não | Sim | Sim |
| Acessar relatórios | Não | Sim | Sim | Não |
| Gerenciar Equipamentos | Não | Não | Sim |  Sim |
| Gerenciar Filamentos | Não | Sim | Sim | Sim |
| Gerenciar Marketplaces | Não | Não | Sim | Sim |
| Gerenciar Orçamentos | Não | Sim | Sim | Sim |
| Gerenciar Banners (carrossel home) | Não | Sim | Sim | Sim |
| Gerenciar Pedidos | Não | Sim | Sim | Não |
| Gerenciar Newsletter | Não | Sim | Sim| Sim |
| Gerenciar Categorias | Não | Sim | Sim | Sim |


O perfil funcionário não pode ser um cliente simultaneamente. Para isto, ele precisa ter uma conta com perfil de cliente e uma conta com perfil de funcionário.

O perfil funcionário, ao gerenciar clientes, ele só conseguirá listar os clientes e clicar em um botão na própria listagem de clientes que enviará um e-mail com o link para o usuário trocar a sua senha.

O perfil Cliente pode solicitar a troca de senha, com isto, ele recebera um e-mail com um link que redicionará para um formulário onde conseguirá alterar a sua senha.

## Fluxo público do MVP

O MVP terá:

- Home
- Catálogo e categorias
- Busca e filtros
- Página de detalhes do produto
- Carrinho como lista de interesse
- Redirecionamento para o canal de atendimento
- Login, cadastro e recuperação de senha
- Edição dos dados da conta
- Política de privacidade e termos de uso
- Políticas de troca e devolução aplicáveis à compra online

## Evoluções futuras do fluxo público

Não fazem parte do MVP:

- Checkout dentro da aplicação
- Cadastro de endereços de entrega
- Cálculo de frete
- Pagamento online
- Confirmação de pedido
- Histórico e acompanhamento de pedidos

## Regras do Carrinho:

- O carrinho estará disponível sem autenticação.
- A quantidade exibida no cabeçalho representará a soma das unidades adicionadas.
- O carrinho anônimo será preservado durante a navegação.
- O cliente poderá navegar, adicionar produtos e editar o carrinho sem autenticação.
- Para enviar a lista de interesse, será necessário estar autenticado.
- Caso não esteja autenticado, o cliente será direcionado ao login.
- Após o login, retornará ao carrinho com os itens preservados.
- Nome, telefone e e-mail serão obtidos da conta autenticada.
- Ao concluir, o cliente será direcionado para o WhatsApp da loja com uma
  mensagem contendo seu nome, telefone, e-mail, os produtos, quantidades e links correspondentes.
  - Deverá ser enviado também uma cópia da lista de interesse para o e-mail da loja (vendas@poplume.com.br) 
- O carrinho não criará um pedido na aplicação.
- O carrinho não terá um campo para observação
- Caso um produto fique inativo e ele existe em algum carrinho, não acontece nada. Pois o mesmo pode ser re-impresso para atender aquela solicitação.
  - Produto inativo não aparece no catálogo, mas seleções anteriores ainda podem ser enviadas
- O carrinho será mantido enquanto a aba do navegador permanecer aberta e será
apagado quando ela for encerrada.

### Importante!

Antes de enviar o e-mail e abrir o WhatsApp, a API deverá validar os produtos, cores, quantidades e preços atuais. Os preços enviados serão sempre os preços vigentes no momento da solicitação.

### Fluxo do Carrinho:

```
Cliente autenticado confirma a lista
        ↓
Frontend envia a solicitação para a API
        ↓
API valida produtos, cores, quantidades e preços
        ↓
API tenta enviar a cópia por e-mail
        ↓
Frontend abre o WhatsApp com a mensagem preenchida
```
A API não armazenará a lista nem os dados enviados, apenas tentará enviar o e-mail e registrará erros técnicos. Em caso de falha, a API registra via Log o problema que aconteceu. O cliente não deve ser afetado por esta falha. Ou seja, o fluxo continua.

Os dados de Nome, e-mail e telefone serão obtido através dos dados do cliente logado no sistema. Estes dados não vão ficar fixos no código.

## Home

A home terá os seguintes elementos:

- Cabeçalho com logomarca, menu, botão de busca de produtos, botão para acessar o menu de usuário logado e botão do carrinho que irá conter a quantidade de produtos colocados no carrinho pelo usuário.
- Um banner grande com texto e alguns exemplos de produtos que a loja faz
- Vitrine dos novos produtos em destaque
- Rodapé  com 5 colunas sendo:
  - Logomarca
  - Menu sobre a empresa
  - Menu sobre ajuda
  - Dados de contato como telefone, links para rede social
  - Banner para o usuário cadastrar seu e-mail para newsletter


A home fornecerá acesso ao catálogo, às categorias, aos detalhes dos produtos, ao carrinho, à autenticação, à conta do cliente e às páginas institucionais.

Regras da home page:

- O botão de usuário na área deslogada deve abrir login/cadastro.
- O botão de usuário na área deslogada, após login, deve abrir:
  - Formulário de edição de dados de usuário
  - Formulário para troca de senha
  - Botão Sair (efetuar logoff)
- A quantidade de itens no carrinho ficará disponível mesmo sem autenticação, quando o usuário avançar para finalizar a compra, ele deve estar logado. Caso contrário, direcioná-lo para efetuar login e após login, volta para o carrinho.
  - Pois desta forma, os dados do cliente como Nome, Telefone e e-mail serão enviados junto à lista de interesse por Whatsapp e e-mail. (vide [Regras do Carrinho](#regras-do-carrinho))
- O carrinho anônimo será preservado ao fazer login
- Inicialmente as categorias que aparecerão no menu são:
  - Decoração
  - Utilitários
  - Presentes
  - Área Pet
  - Personalizados
- O carrinho não cria um pedido
- Os produtos em destaque serão administráveis. Existe uma flag no cadastro do produto que informará se ele é destaque ou não.
- O banner principal será carrossel e gerenciado pelo sistema. Vamos ter um cadastro de banner para ser usado no carrossel.
- Redes sociais também ficarão dentro da coluna de contato, chegando então às cinco pretendidas.


## Área administrativa

Os primeiros módulos do sistema serão produtos, categorias de produtos, banners, equipamentos, filamentos, marketplaces, clientes e Newsletter contendo:

- Listagem
- Busca e filtros
- Cadastro com máscaras e validações dos formulários
- Edição
- Visualização
- Ativação/inativação
- Paginação
- Confirmação de operações
- Estados de carregamento, erro, vazio e indisponibilidade

O único módulo vendável é o de produtos. Os demais são cadastros para controle interno e montagem de preço de venda.

## Módulo Cadastro de cliente

O MVP será preparado para evolução mesmo o cliente não efetuando compras direto no sistema. Com isto, vamos manter o cadastro, login e edição de perfil, mesmo com utilidade inicial limitada.

## Módulo Cadastro de produto

Validar se o domínio de `Produto` contem todos os campos abaixo, caso não tenha, precisamos alterar a API para receber estas informações.

- Nome
- Descrição curta e completa
- Categoria
- Preço visível
- Produto ativo
- Produto em destaque
- Slug da página
- Prazo estimado de produção
- Código interno
- Cores disponíveis
  - Filamento associado
  - Preço informado manualmente
  - Uma ou mais imagens associadas
  - A primeira imagem adicionada, será a destaque

## Módulo Formação de Preço

O preço do produto se deve a várias informações previamente cadastradas. Sua fórmula ainda não foi finalizada. Mas algumas informações precisão estar previamente cadastradas.

- O produto deve estar cadastrado
- O equipamento deve estar cadastrado
- O filamento deve estar cadastrado
- O Marketplace deve estar cadastrado

O cálculo considera tempo de impressão, consumo de filamento, energia, margem, comissão do marketplace e preço dos produtos filhos vinculados ao produto principal.

Ao final do orçamento, deve exibir uma mensagem de confirmação perguntando se quer atualizar o preço do produto.

### Importante!

O módulo de formação de preço não faz parte da primeira entrega do MVP. Enquanto sua fórmula não estiver definida, os preços serão cadastrados manualmente para cada cor do produto.

## Módulo Newsletter e LGPD

Criar uma página para consentimento explicíto, política de privacidade.
Sobre a Newsletter, criar uma rota na API para poder armazenar os e-mails dos clientes, data e origem do consentimento. E uma outra rota para remover o e-mail do cliente, cancelando sua inscrição na newsletter.

# Evoluções Futuras

## Módulo de pedidos

Este módulo é responsável por todo o ciclo de vida do pedido. Quando implementado, o módulo terá inicialmente os seguintes recursos:

- Listagem e detalhes dos pedidos
- Alteração de situação
- Cancelamento
- Registro de pagamento

Após este módulo for implementado, na home page será necessário adicionar a seguinte regra:
- O botão de usuário na área deslogada, após login, deve abrir:
  - Página de histórico de pedidos
- A quantidade de itens no carrinho ficará disponível mesmo sem autenticação, mas quando o usuário avançar para finalizar a compra, ele deve estar logado. Caso contrário, direcioná-lo para efetuar login e após login, volta para o carrinho.
- O carrinho cria um pedido

Os passos seguintes, ficarão para evolução do módulo. São eles:

- Acompanhamento de produção
- Código de rastreamento
- Histórico das mudanças

## Módulo de Pagamento

As regras deste módulo ficarão para evolução do sistema. Mas a previsão será:

- Formas de pagamento via Pix ou cartão
- Integração com Gateway de pagamento
- Integração com sistema de correios para cálculo de frete
- Correios ou transportadora
- Prazo de produção separado do prazo de entrega

## Relatórios

Os relatórios não fazem parte do MVP de catálogo. Quando o sistema passar a
registrar vendas e pedidos, estão previstos:

- Vendas por período
- Pedidos por situação
- Clientes
- Vendas por marketplace

# Fora de Escopo do MVP

Características do sistema:

- No MVP não haverá controle de estoque. Essa é uma funcionalidade futura