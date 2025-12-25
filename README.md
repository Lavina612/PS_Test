# PS_Test
SPA-приложение для управления каталогом товаров. Позволяет создавать, просматривать, редактировать и удалять записи. Все товары сохраняются в PostgreSQL. Приложение написано на базе ASP.NET Core с использованием Vue3 и EntityFramework.


## Технологии
- **Backend:** ASP.NET Core 8 (.NET 8.0)
- **Database:** PostgreSQL 17.5 + Entity Framework Core 8.0.11 (Code First)
- **Frontend:** Vue 3 (Composition API) + Vite
- **API:** RESTful API (JSON)


## Установка и запуск
1. Склонируйте проект.
2. Настройте Базу данных:

Убедитесь, что у вас установлен PostgreSQL. В файле PS_test.Server/appsettings.json настройте строку подключения, прописав ваши данные вместо ```***```:
  ```
"ConnectionStrings": {
    "DefaultConnection": "Host=***;Port=***;Database=***;Username=***;Password=***"
  }
```
3. Запустите проект через Visual Studio. Приложение автоматически поднимет Backend на порту HTTPS и запустит Frontend через SPA-прокси.


## Планы по улучшению
- Добавить логику работы со связанными сущностями (Items, Orders, Customers) и их отображение.
- Добавить аутентификацию пользователей и соответствующие виды ролей
- Расширить обработку ошибок и логирование
