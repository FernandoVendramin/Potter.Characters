
# Potter.Characters

Este projeto consiste em uma API REST,  para inserir, atualizar, excluir e consultar (CRUD)  Personagens do site  [potterapi](https://www.potterapi.com) (Site que disponibiliza uma série de informações sobre a série Harry Potter).

Os personagens inseridos no banco do projeto, são validados de acordo com os dados disponíveis na API.

## Tecnologias Utilizadas

| Tecnologia                  | Versão      |
| --------------------------- | ----------- |
| .Net Core                   |         3.1 |
| Fluent Validation Core      |       9.0.1 |
| Serilog                     |       2.9.0 |
| MongoDB Driver              |      2.11.0 |
| Polly                       |       3.1.6 |
| Redis                       |       2.2.0 |
| Docker                      |    19.03.12 |


## Configurações

Antes de rodar o projeto, você deve realizar as configurações no `appsettings.json` marcadas entre `[]` , conforme exemplo abaixo.
```
{
  "PotterApiConfig": {
    "BaseUrl": "https://www.potterapi.com",
    "Key": "[CHAVE_DE_INTEGRACAO]"
  },
  "MongoConfig": {
    "Database": "[NOME_DO_BANCO]",
    "ConnectionString": "[STRING_DE_CONEXAO]
  },
  "Redis": {
    "ConnectionString": "[STRING_DE_CONEXAO]"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

###### Desenvolvido por Fernando Vendramin Ribeiro