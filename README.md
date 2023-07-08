# Product_Management_Web_API
Projeto Web-API desenvolvido em .NET 6.0, utilizando DDD, EF (Entity Framework) e banco SQL Server (mssql).

## Description
Trata-se de um CRUD de Gerenciamento de Produto, onde é possível adicionar produtos, listar os produtos da base de dados, retornar um produto, atualizar um produto e remover um produto.

## Installation
- Visual Studio
    - Baixar e instalar (recomendo o 2022).
    - Clonar o projeto.
    - Em "appsettings.json" atualizar os valores da config com os dados do seu banco de dados SQL Server (Server, User Id, Password).
    - Abrir Tools > NuGet Package Manager > Package Manager Console
        - Remover a pasta "Migrations" do projeto. Está em ProductManagement.Infra.
	    - Entrar com o comando "add-migration ProductManagement -context ProductManagementDbContext"
	        - Espere realizar a criação do Migrations.
	    - Entrar com o comando "update-database"
	        - Espere realizar a atualização do Migrations e mapeamento com o seu SQL Server Management Studio.
    - Se sua pasta "Migrations" foi criada em ProductManagement.API, basta recorta-la e cola-la em ProductManagemet.Infra (seguir os princípios do DDD).
    - Recompilar o projeto e ajeitar qualquer namespace que tenha ficado errado decorrente do último copy-paste.

- SQL Server
    - Baixar o Microsoft SQL Server Management Studio (caso não tenha).

## Usage
- Compilar o projeto no Visual Studio
    - Clean e Rebuild.
- Rodar o projeto.
- Recomendo Insomnia ou Postman para realizar as chamadas.
