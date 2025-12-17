# Survey - Arquitetura de Software Escalável para Pesquisa Online

![.NET 9](https://img.shields.io/badge/.NET-9.0-purple?style=flat-square&logo=dotnet)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

## 1. Visão Geral e Objetivo

Este projeto consiste em uma aplicação de uma API RESTful desenvolvida em **.NET 9**, projetada para gerenciar enquetes públicas de alta demanda (cenário de "milhões de votos").

O objetivo acadêmico principal é demonstrar a aplicação prática dos princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**, focando no desacoplamento entre regras de negócio, persistência de dados e interface de usuário. O sistema simula um backend para uma startup de pesquisas eleitorais, priorizando a integridade do domínio e a preparação para escalabilidade horizontal.

---

## 2. Tecnologias e Ferramentas

* **Plataforma:** .NET 9 (C# 12)
* **Framework Web:** ASP.NET Core Web API
* **ORM:** Entity Framework Core 9.0.0
* **Banco de Dados:** In-Memory Database
* **Documentação:** Swagger UI (OpenAPI)
* **IDE Recomendada:** Visual Studio Community ou VS Code

---

## 3. Arquitetura do Sistema

O projeto segue estritamente a arquitetura em camadas concêntricas (**Onion Architecture / Clean Architecture**), garantindo que o núcleo (Domínio) não dependa de detalhes externos.

### 3.1 Estrutura de Pastas e Responsabilidades

A solução é dividida nas seguintes camadas lógicas:

* **📂 Domain (Núcleo)**
    * Contém as Regras de Negócio Puras.
    * `Entities`: *Survey, Question, Vote* (Objetos com identidade e comportamento).
    * `Interfaces`: *ISurveyRepository, IVoteRepository* (Contratos de inversão de dependência).
    * *Nota:* Esta camada não possui referências a banco de dados ou frameworks HTTP.

* **📂 Application (Orquestração)**
    * Implementa os Casos de Uso.
    * `Services`: *SurveyService*. Coordena a lógica entre repositórios e entidades.
    * `DTOs`: Objetos de transferência de dados para evitar expor as entidades diretamente (Foco em Segurança e Performance).

* **📂 Infrastructure (Detalhes)**
    * Implementação técnica e acesso a dados.
    * `Persistence`: *ApplicationDbContext* (Configuração do EF Core).
    * `Repositories`: Implementação concreta das interfaces definidas no Domínio.

* **📂 API (Entrada)**
    * Camada de apresentação REST.
    * `Controllers`: Recebem requisições HTTP e devolvem Status Codes padronizados (200, 201, 404).

---

## 4. Decisões de Design (Design Decisions)

### 4.1 Domain-Driven Design (DDD)
* **Aggregate Root:** A entidade `Survey` atua como raiz de agregação. Perguntas (`Question`) e Opções (`Option`) só podem ser manipuladas através da `Survey`, garantindo consistência (ex: uma pergunta não pode existir órfã).
* **Encapsulamento:** As listas de perguntas são expostas como `IReadOnlyCollection`, impedindo que camadas externas manipulem a coleção diretamente sem passar pelas validações do método `AddQuestion`.

### 4.2 Estratégia de Escalabilidade (Performance)
Para atender ao requisito de "milhões de votos", a entidade `Vote` foi modelada de forma desacoplada da `Survey`.

* ❌ **Cenário Ingênuo:** Salvar o voto dentro da lista de votos da Pesquisa (`Survey.Votes.Add()`). Isso carregaria a pesquisa inteira na memória para cada voto, travando o banco e consumindo memória excessiva.
* ✅ **Cenário Implementado:** O `Vote` é uma entidade leve e independente. A gravação é uma operação de **INSERT pura (O(1))**. A leitura (Relatório) é feita via Projeção no Banco de Dados (`GroupBy`/`Select`), sem carregar os dados brutos para a memória da aplicação.

### 4.3 DTOs (Data Transfer Objects)
O uso de **Records** para DTOs garante imutabilidade e simplifica a transferência de dados. Isso desacopla o contrato da API (o que o frontend vê) do modelo do banco de dados (o que o backend armazena).

---

## 5. Como Executar o Projeto

### Pré-requisitos
* SDK do .NET 9.0 instalado.
* Visual Studio ou VS Code.

### Passo a Passo
1.  Clone o repositório ou baixe os arquivos.
2.  Abra a solução `SurveySystem.sln` no Visual Studio.
3.  Restaure os pacotes NuGet (o VS fará isso automaticamente ou via `dotnet restore`).
4.  Defina o projeto **API** como "StartUp Project".
5.  Execute a aplicação (F5 ou botão Play).
6.  Abra o navegador no endereço https://localhost:7254/swagger (depende de certificado SSL instalado) ou http://localhost:5156/swagger.

--- 

## 6. Documentação da API (Endpoints)

| Método   | Endpoint                   | Descrição                                               | Status Sucesso |
| :------- | :------------------------- | :------------------------------------------------------ | :------------- |
| **POST** | `/api/Surveys`             | Cria uma nova pesquisa com perguntas e opções.          | `201 Created`  |
| **GET**  | `/api/Surveys/{id}/report` | Obtém os dados da pesquisa e contagem parcial de votos. | `200 OK`       |
| **POST** | `/api/Surveys/{id}/vote`   | Registra um voto em uma opção específica.               | `200 OK`       |

### Exemplo de JSON para Criação (POST /api/Surveys)

```json
{
  "title": "Eleições Presidenciais",
  "description": "Pesquisa de intenção de voto 2026",
  "questions": [
    {
      "text": "Em quem você votaria?",
      "options": ["Candidato A", "Candidato B", "Nulo"]
    }
  ]
}
```