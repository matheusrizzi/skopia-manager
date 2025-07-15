# Skopia Manager API
Sistema de gerenciamento de tarefas e projetos, desenvolvido como parte de um desafio técnico.

## 🚀 Executando com Docker

### Pré-requisitos
- Docker instalado (https://www.docker.com) (Se instalar o docker desktop ele ja vem com o compose)
- Docker Compose instalado

### Como rodar a aplicação

1. Clone o repositório:

```bash
git clone https://github.com/matheusrizzi/skopia-manager.git
cd skopia-manager
docker-compose up --build
```
2. Se precisar refazer o processo:
```bash
docker-compose down -v
```
---
## 📩 Fase 2: Refinamento — Perguntas para o PO

- Um usuário pode alterar a task de outro usuario?
- Haverá controle de múltiplos usuários por projeto?
- Comentários nas tarefas podem ser editados ou excluídos?
- Precisamos suportar notificações por e-mail ou integração com ferramentas como Slack?
- Qual a expectativa de escalabilidade: é um sistema para poucos times ou muitos usuários simultâneos?
- Devemos pensar em deixar multitenant?

## 🚧 Fase 3: Melhorias e Visão Técnica

### 📦 Arquitetura e Organização

- Poderiamos melhorar usando microserviço como Azure Functions, assim ele é auto escalavel.
- Usar autenticação e sistema de roles/policies.

### ☁️ Cloud & DevOps

- Automatizar deploy com **Azure**.
- Utilizar user secrets e nao deixar exposto no settings as credenciais.

### 📈 Observabilidade

- Monitoramento de saúde com endpoints `/health` ex: healthCheck.
- Talvez adicionar um grafana para acompanhamento e informações detalhadas.

---

### 🛠️ **Tecnologias Utilizadas**

```md
## 🛠️ Tecnologias

- ASP.NET Core 9.0
- Clean Architecture
- CQRS
- Entity Framework Core
- SQL Server (Docker)
- MediatR
- Docker / Docker Compose
- Swagger
- Mapster
