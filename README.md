# AppleAcademy

## Executar com Docker Compose

Copie `.env.example` para `.env` na raiz e substitua os valores de `POSTGRES_PASSWORD`, `JWT_KEY` e `ADMIN_PASSWORD` por segredos próprios. A chave JWT precisa ter pelo menos 32 caracteres e a senha do administrador, pelo menos 10.

```powershell
Copy-Item .env.example .env
docker compose up --build
```

A API fica disponível em `http://localhost:5000` e a documentação em `http://localhost:5000/scalar/v1`. O PostgreSQL fica acessível apenas aos serviços do Compose; seus dados persistem no volume `postgres_data`.

Na inicialização, a API aplica as migrations, cria o perfil `Admin` (GUID `9ddf8627-87fa-4f38-86e1-9c5f5530ed98`) e, se ainda não existir um administrador ativo, cria o usuário com `ADMIN_EMAIL` e `ADMIN_PASSWORD`. A senha é armazenada como hash BCrypt. As credenciais do `.env` são usadas apenas na criação inicial; alterações posteriores no arquivo não redefinem a senha do usuário existente.

Para entrar, envie `POST /api/Auth/Login` com `{"email":"<ADMIN_EMAIL>","password":"<ADMIN_PASSWORD>"}`. Use o token retornado como Bearer nas rotas de `api/Users`. Apenas o perfil `Admin` pode acessar essas rotas, inclusive `api/Users/SignUp`.
