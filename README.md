# 📌 API Pedidos

API para gerenciamento de produtos e pedidos, construída em **ASP.NET Core 8**.  
Permite cadastrar produtos, criar pedidos, adicionar/remover itens e fechar ou cancelar pedidos.

---

## ✅ Funcionalidades Principais

### 🛍️ Produtos
- Criar produto  
- Listar produtos ativos e inativos  
- Atualizar dados do produto  
- Inativar produto (**soft delete**)  
- Validações de negócio

### 📦 Pedidos
- Criar pedido  
- Adicionar itens  
- Remover itens  
- Listar e consultar por ID  
- Calcular total automaticamente  
- Fechar e cancelar pedido  
- Paginação e filtro por status ✅  

---

## 🧱 Tecnologias Utilizadas
- .NET 8 — ASP.NET Web API
- Entity Framework InMemory
- Swagger — Documentação interativa
- xUnit + Moq — Testes unitários

---

## 🔗 Endpoints

### 📦 **Pedidos**

| Ação | Método | Rota |
|------|--------|------|
| Criar pedido | POST | `/api/Pedidos` |
| Listar pedidos | GET | `/api/Pedidos` |
| Buscar por ID | GET | `/api/Pedidos/{id}` |
| Adicionar item ao pedido | POST | `/api/Pedidos/{pedidoId}/produtos/{produtoId}` |
| Remover item do pedido | DELETE | `/api/Pedidos/{pedidoId}/produtos/{produtoId}` |
| Fechar pedido | POST | `/api/Pedidos/{pedidoId}/fechar` |
| Cancelar pedido | POST | `/api/Pedidos/{pedidoId}/cancelar` |
| Listar pedidos paginados | GET | `/api/Pedidos/paginado` |
| Filtrar pedidos por status | GET | `/api/Pedidos/status/{status}` |

---

### 🛍️ **Produtos**

| Ação | Método | Rota |
|------|--------|------|
| Listar produtos ativos | GET | `/api/Produtos/Ativos` |
| Buscar produto por ID | GET | `/api/Produtos/{id}` |
| Atualizar produto | PUT | `/api/Produtos/{id}` |
| Inativar produto (soft delete) | DELETE | `/api/Produtos/{id}` |
| Criar produto | POST | `/api/Produtos` |
| Listar produtos inativos | GET | `/api/Produtos/Inativos` |


