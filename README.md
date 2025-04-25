🛒 Mini Ecommerce - Web API em .NET 8

Este é um projeto de um Mini Ecommerce, com funcionalidades de gerenciamento de produtos, carrinho de compras, aplicação de cupons de desconto e finalização de pedidos. O sistema simula um pequeno comércio (ex: livraria com categoria “Material Escolar”) e é dividido em várias camadas para ter melhor organização e modularidade.

Funcionalidades:

Gerenciamento de Produtos: Criar, editar e excluir produtos.

Carrinho de Compras: Adicionar produtos ao carrinho com a quantidade desejada.

Descontos com Cupons: Aplicar cupons de desconto VSHOP_PROMO_10 ou VSHOP_PROMO_20 conforme registrados na tabela coupons.

Finalização do Pedido: Visualizar e revisar os produtos, inserir dados pessoais e concluir a compra com uma mensagem de sucesso.

Arquitetura: O projeto está dividido em múltiplos serviços com bancos de dados separados:

🔙 Backend

Mini_Ecommerce: APIs de categorias e produtos (vshopdb → tabelas: categories, products)

VShop.CartApi: Gerenciamento do carrinho (vshopcartdb → tabelas: cartheaders, cartitems, products)

Discount: Aplicação de cupons de desconto (vshopdiscountdb → tabela: coupons)

💻 Frontend

Web: Interface web usando Razor (.cshtml), conectando-se às APIs acima.

🛠️ Tecnologias Utilizadas e Extensões: 

ASP.NET Web API (.NET 8)

MySQL

Swagger UI

Postman

Bootstrap

AutoMapper

Entity Framework Core

Pomelo.EntityFrameworkCore.MySql

Swashbuckle.AspNetCore

🚀 Como Rodar o Projeto

Pré-requisitos:

.NET 8 

MySQL instalado e rodando

Visual Studio

Clonar o Repositório git clone https://github.com/Felipe-Martins-Nascimento/Mini_Ecommerce.git

Configurar o Banco de Dados Certifique se as connection strings de cada projeto estão corretos nos arquivos appsettings.json (Em todos os projetos Backend, igualmente como irá fazer com o dotnet).

Instalar Ferramentas EF Core Em cada projeto (abra o terminal dentro da pasta de cada projeto, exemplo: VShop.CartApi => Botão direito => Abrir no Terminal):

dotnet tool install --global dotnet-ef

dotnet tool update --global dotnet-ef

dotnet ef --version # para verificar se foi instalado corretamente

Atualizar o Banco de Dados Rode o comando abaixo dentro de cada projeto que tem DbContext:
dotnet ef database update

Caso precise passei o arquivo Backup.txt, apenas rode esse arquivo no MySql.

ENDPOINTS para rodar no Postman:

Catálogo de Produtos:

GET: /api/Products

http://localhost:5018/api/Products

POST: /api/Products

http://localhost:5018/api/Products

Colocar na aba headers:

Key: Content-Type

Value: application/json

Na aba Body:

{ "id": 78, "name": "apag", "description": "testar", "price": 20.22, "stock": 9, "imageURL": "balde.jpg", "categoryId": 1, "categoryName": null }

-> Ver no GET quais são os id que tem, e colocar um diferente no JSON

PUT: /api/Products

http://localhost:5018/api/Products

Colocar na aba headers:

Key: Content-Type

Value: application/json

Na aba Body:

{ "id": 41, "name": "apag", "description": "testar", "price": 20.22, "stock": 9, "imageURL": "balde.jpg", "categoryId": 1, "categoryName": null }

-> Ver no GET quais são os id que tem, e colocar um igual no JSON

GET: /api/Products/ {id}

http://localhost:5018/api/Products/{id}

Exemplo:

http://localhost:5018/api/Products/881

-> Ver no GET quais são os id que tem para caso tenha que substituir o 881, mesma coisa para o delete.

DELETE: /api/Products/{id} -> Verificar se tem algum produto com seu id

http://localhost:5018/api/Products/{id}

Exemplo:

http://localhost:5018/api/Products/881

Carrinho de Compras:

POST: /api/Cart/addcart

http://localhost:5039/api/Cart/addcart

Colocar na aba headers:

Key: Content-Type

Value: application/json

Na aba Body:

{ "cartItems": [ { "quantity": 2, "product": { "id": 1, "name": "Produto A", "stock": 10 } } ] }

GET: /api/Cart/getcart

http://localhost:5039/api/Cart/getcart

POST: /api/Cart/checkout

http://localhost:5039/api/Cart/checkout

Colocar na aba headers:

Key: Content-Type

Value: application/json

Na aba Body:

{ "cartItems": [ { "quantity": 2, "product": { "id": 1, "name": "Produto A", "stock": 10 } } ] }

DELETE: /api/Cart/clearcart -> Verificar se tem algum produto no carrinho

http://localhost:5039/api/Cart/clearcart

Além dessas APIs, implementei outras contribuindo para a parte WEB do projeto, para testar siga o mesmo padrão das APIs acima.
