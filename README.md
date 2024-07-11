# Portal5-Works
[![Build status](https://ci.appveyor.com/api/projects/status/25pytmgy12ey90ak?svg=true)](https://ci.appveyor.com/project/diev/portal5-works)
[![GitHub Release](https://img.shields.io/github/release/diev/Portal5-Works.svg)](https://github.com/diev/Portal5-Works/releases/latest)

Works with API of Portal5.
Работа с Portal5 по API.

## Settings / Параметры

Appsettings:

- `CryptoBot.config.json` (located with App `exe`)
- `%ProgramData%\Diev\CryptoBot.config.json` (overwrites if exists)

Windows Credential Manager:

- `Portal5test *` (name: `Portal5test https://{host}`, user: `{username}`, pass: `{password}`)
- `Portal5 *` (name: `Portal5 https://{host}`, user: `{username}`, pass: `{password}`)
- `CryptoPro My` (name: `CryptoPro My`, user: `{cert}`, pass: `{pin}`)
- `SMTP *` (name: `SMTP {host} {port} tls`, user: `{sender}`, pass: `{password}`)

CLI:

- Task:
  - `-z XX` - 'Zadacha_`XX`'
- Filter:
  - `-d` - today only
  - `-f yyyy-MM-dd` - from date
  - `-t yyyy-MM-dd` - to date

## API usage examples / Примеры использования API

Номера задач смотрите в документации на Portal5.
К каждой задаче может применяется фильтр дат.
По завершении операций подписчикам отправляются уведомления по e-mail.

### ЗСК (Светофор)

Отправка перечня клиентов (Zadacha_137):

    CryptoBot -z 137

Получение реестра (Zadacha_130): 

    CryptoBot -z 130

### Скачивание по фильтру

Скачивание Входящих писем из ЛК (Zadacha_3-1) по фильтру дат (за сегодня):

    CryptoBot -z 3-1 -d

Скачивание Исходящих писем из ЛК (Zadacha_2-1) по фильтру дат (за 2023):

    CryptoBot -z 2-1 -f 2023-01-01 -t 2024-01-01

Скачивание запросов ЦИК (Zadacha_54) по фильтру дат (с начала месяца):

    CryptoBot -z 54 -f 2024-07-01

Если стоит CryptoPro CSP, то все скачанные пакеты можно расшифровать для
помещения на хранение. После скачивания можно очистить место в ЛК.

### Очистка места в ЛК

Очистка ЛК от служебной информации ПП Дельта.
В коде зафиксировано оставить последние 30 дней, очищаются следующие задачи:

- inbox
  - Zadacha_97  - Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС1)
  - Zadacha_107 - Извещение о результатах контроля отчетности субъектов НПС (ИЭС1)
  - Zadacha_114 - Извещение о результатах контроля представления формы 0409310 (ИЭС1)
  - Zadacha_123 - Извещение о результатах контроля представления формы 0409310 (ИЭС2)
  - Zadacha_130 - Получение информации об уровне риска ЮЛ/ИП
  - Zadacha_133 - Извещение о результатах контроля отчетности субъектов НПС (ИЭС2)
  - Zadacha_140 - Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС2)
  - Zadacha_156 - Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС1) и др.
  - Zadacha_159 - Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС2) и др.
- outbox
  - Zadacha_155 - Представление отчетности КО в Банк России

    CryptoBot -z 0

## Requirements / Требования

- .NET 8 Desktop Runtime
- CryptoPro CSP (опционально - для криптоопераций)

## Build / Построение

Build an app with many dlls  
`dotnet publish CryptoBot\CryptoBot.csproj -o Distr`

Build a single-file app when NET Desktop runtime required  
`dotnet publish CryptoBot\CryptoBot.csproj -o Distr -r win-x64 -p:PublishSingleFile=true --no-self-contained`

Build a single-file app when no runtime required  
`dotnet publish CryptoBot\CryptoBot.csproj -o Distr -r win-x64 -p:PublishSingleFile=true`

Или просто используйте `build.cmd`.

## Versioning / Порядок версий

Номер версии программы указывается по нарастающему принципу:

* Требуемая версия .NET (8);
* Год текущей разработки (2024);
* Месяц без первого нуля и день редакции (624 - 24.06.2024);
* Номер билда - просто нарастающее число для внутренних отличий.
Если настроен сервис AppVeyor, то это его автоинкремент.

Продукт развивается для собственных нужд, и поэтому
Breaking Changes могут случаться чаще, чем это принято в SemVer.

## License / Лицензия

Licensed under the [Apache License, Version 2.0].

[Apache License, Version 2.0]: LICENSE
