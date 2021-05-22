# backend-clients
Сервис предназначен для: 
- регистрации клиентов (жителей и гостей Новороссийска) в системе; 
- получения данных о клиентах;
- получения статистики о зарегистрированных клиентах;
- аутентификации и авторизации клиентов в системе (мок, TODO: реализовать нормальный сервис авторизации с поддержкой OpenId Connect и OAuth 2.0).

## Требования для работы сервиса
- Linux (возможна работа и с другими ОС);
- Docker;
- docker-compose.

## Развёртывание

```bash
git clone git@github.com:era-ants/backend-clients.git
cd backend-clients
docker-compose build
docker-compose up -d
```
