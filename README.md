# Отчет по практической работе №6

## Создание автоматизированных Unit-тестов

### 1. Скриншот таблицы пользователей в Microsoft SQL Server
![Скриншот таблицы Users](скринбазы.png)  


---

### 2. Скриншот окна «Обозреватель тестов» в Visual Studio
![Скриншот Test Explorer](автотест.png)  
*Все 11 тестов выполнены*

---

### 3. Результаты тестирования

#### Авторизация:  
- Вход с корректными данными администратора.  
- Вход с корректными данными врача.  
- Обработка неверного пароля.  
- Обработка несуществующего логина.  
- Проверка пустых полей (логин, пароль, оба поля).  

#### Регистрация:  
- Создание нового пользователя с уникальным логином.  
- Обработка регистрации с существующим логином.  
- Проверка несовпадающих паролей.  
- Проверка пустых полей при регистрации.  

##

 В ходе работы успешно протестированы модули авторизации и регистрации пользователей: все 11 тестов (позитивные и негативные сценарии) выполнены корректно, что говорит о корректности работы приложения для этих тестовых сценариев.
