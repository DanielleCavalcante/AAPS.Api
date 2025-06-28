
<h1 align="center">🐾 Sistema de Gerenciamento de Adoções - ONG AAPS 🐾</h1>

<p align="center">
  <strong>Trabalho de Graduação</strong><br/>
  Apresentado à Faculdade de Tecnologia de Sorocaba - Fatec Sorocaba,<br/>
  como parte dos pré-requisitos para obtenção do título de Tecnólogo em Análise e Desenvolvimento de Sistemas.
</p>

---

### 🎓 Informações Acadêmicas

- **Instituição:** Fatec Sorocaba  
- **Curso:** Análise e Desenvolvimento de Sistemas  
- **Semestre:** 7º

### 👨‍💻 Integrantes do Projeto

- Adriana Akagui  
- Alisson de Lima  
- Danielle da Silva  
- Franciele Rodrigues  
- Nicollas Schlemm  

---

## 📋 Sobre o Projeto

Este repositório contém a **API RESTful** desenvolvida para a ONG **AAPS (Associação Anjos e Protetores de Sorocaba)**.

O sistema tem como finalidade:
- Centralizar e facilitar a gestão de **cadastros de animais, adotantes, doadores e voluntários**
- Emissão e envio de **termos de adoção por e-mail**
- **Acompanhamento do histórico** de saúde dos animais (vacinas, castração, cirurgias, etc.)
- **Geração de relatórios** por período
- Documentação interativa via **Swagger**

> 🔗 O repositório do front-end que consome esta API está disponível em:  
> [https://github.com/DanielleCavalcante/AAPS-Frontend](https://github.com/DanielleCavalcante/AAPS-Frontend)

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Descrição |
|------------|-----------|
| **.NET 8** | Backend API REST com ASP.NET Core |
| **Entity Framework Core** | ORM para comunicação com SQL Server |
| **SQL Server** | Banco de dados relacional |
| **JWT + Identity** | Autenticação e controle de acesso |
| **Swagger** | Interface para testes de endpoints |
| **QuestPDF / iText7** | Geração de PDFs |
| **ClosedXML** | Exportação de dados para Excel |

### 📦 Principais Pacotes NuGet

- `Microsoft.EntityFrameworkCore` - 8.0.8  
- `Microsoft.EntityFrameworkCore.SqlServer` - 8.0.8  
- `Microsoft.AspNetCore.Identity` - 2.3.1  
- `Microsoft.AspNetCore.Authentication.JwtBearer` - 8.0.10  
- `Swashbuckle.AspNetCore` (Swagger) - 6.4.0  
- `QuestPDF` - 2025.5.0  
- `ClosedXML` - 0.104.2  
- `iText7` - 9.1.0  

---

## ▶️ Como Executar Localmente

### ✅ Pré-requisitos

- Visual Studio 2022+  
- SQL Server + SSMS (SQL Server Management Studio)  
- .NET 8 SDK  

---

### 📁 1. Clone o Repositório

```bash
git clone https://github.com/DanielleCavalcante/AAPS.Api.git
```

Abra a solução no Visual Studio.

---

### ⚙️ 2. Configure o `appsettings.Development.json`

1. **Duplique o arquivo de exemplo**:

```bash
cp appsettings.Development.example.json appsettings.Development.json
```

2. **Edite o `appsettings.Development.json`** com as suas configurações reais, como:

#### 🔗 Conexão com o banco de dados

```json
"ConnectionStrings": {
  "DbAapsLocal": "Server=SEU_SERVIDOR_SQL\\SQLEXPRESS;Database=DbAaps;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> 💡 Dica: Você encontra o nome do servidor ao abrir o SSMS (exemplo: `DESKTOP-NOME\SQLEXPRESS`).

#### 🔐 Chave JWT

Necessária para a autenticação de usuários da API:

```json
"Jwt": {
  "Key": "sua-chave-jwt-aqui",
  "Issuer": "https://localhost",
  "Audience": "https://localhost"
}
```

#### ✉️ Credenciais de e-mail

Utilizadas para envio de termos de adoção por e-mail através da API:

```json
"EmailSettings": {
  "Email": "seu@email.com",
  "Senha": "sua-senha-de-email"
}
```

> 📬 **Solicite as credenciais de e-mail com a equipe do projeto**, caso ainda não tenha acesso.

---

#### 📌 Sobre o `appsettings.Development.json`

* 🔐 Esse arquivo está ignorado no `.gitignore`, garantindo que suas credenciais fiquem protegidas.
* 💡 A aplicação detecta automaticamente o ambiente de desenvolvimento e carrega esse arquivo ao rodar localmente.

---

### 🛠️ 3. Gerar o Banco de Dados

1. No Visual Studio, vá em:  
   `Ferramentas > Gerenciador de Pacotes NuGet > Console do Gerenciador de Pacotes`
2. Execute:

```powershell
Update-Database
```

Isso criará o banco com base nas Migrations existentes.

---

### 🚀 4. Execute a API

- Pressione `F5` ou clique em **Iniciar** no Visual Studio.
- O Swagger UI será aberto automaticamente no navegador com todos os endpoints documentados.

---

## 🔍 Swagger - Documentação Interativa

A API conta com a interface Swagger, onde é possível visualizar e testar os endpoints de forma prática.

---

## 💡 Considerações Finais

- Esta é uma **API RESTful** dedicada à ONG AAPS.
- O front-end está disponível separadamente em:  
  👉 [https://github.com/DanielleCavalcante/AAPS-Frontend](https://github.com/DanielleCavalcante/AAPS-Frontend)
- Sistema desenvolvido para fins **educacionais e sociais**.

---

<p align="center"><strong>📌 Projeto acadêmico desenvolvido para fins educacionais e sociais.</strong></p>
