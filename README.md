# Portal5-Works

[![Build status](https://ci.appveyor.com/api/projects/status/25pytmgy12ey90ak?svg=true)](https://ci.appveyor.com/project/diev/portal5-works)
[![.NET8 Desktop](https://github.com/diev/Portal5-Works/actions/workflows/dotnet8-desktop.yml/badge.svg)](https://github.com/diev/Portal5-Works/actions/workflows/dotnet8-desktop.yml)
[![GitHub Release](https://img.shields.io/github/release/diev/Portal5-Works.svg)](https://github.com/diev/Portal5-Works/releases/latest)

Works with API of Portal5.  
Работа с Portal5 по API.

## Settings / Параметры

*Appsettings* с именем исполняемой программы - в отличие от принятого при
разработке в .NET единого имени, так можно все файлы настроек разных
программ размещать в одной папке для скриптов, но при этом какие-то общие
параметры (будут иметь приоритет) вынести из папки с программой, где они
могут быть нечаянно затерты при обновлении версии или конфиденциальная
информация может попасть в дистрибутивный архив:

- `CryptoBot.config.json` (located with App `exe`)
- `%ProgramData%\Diev\CryptoBot.config.json` (these settings overwrite if exist)

Однако, при запуске в Linux программа может не получать путь к папке
с программой - тогда она будет искать традиционный `appsettings.json`
в текущей папке.

*Windows Credential Manager* в Панели управления - *Диспетчер учетных данных*
(все пароли для всех программ меняются в одном месте и скрыты от
пользователей, как было бы при хранении в индивидуальных файлах настроек
к каждой программе):

- `Portal5test *` (name: `Portal5test https://{host}`, user: `{username}`, pass: `{password}`)
- `Portal5 *` (name: `Portal5 https://{host}`, user: `{username}`, pass: `{password}`)
- `CryptoPro My` (name: `CryptoPro My`, user: `{cert}`, pass: `{pin}`)
- `SMTP *` (name: `SMTP {host} {port} tls`, user: `{sender}`, pass: `{password}`)

Однако в Linux такой программы нет, и все параметры надо написать в JSON
открытым текстом (в Windows тоже так можно, игнорируя безопасность):

- `"TargetName": "https://{host} {username} {password}"`
- `"CryptoName": "CryptoPro My {cert} {pin}"`
- `"SmtpName": "{host} {port} tls {sender} {password}"`

При наличии (в Windows) КриптоПро можно указать, какая утилита
командной строки будет использоваться (важно для больших файлов):

- `"UtilName": "CspTest"` - бесплатная утилита
- `"UtilName": "CryptCP"` - утилита для больших файлов

## CLI - параметры командной строки:

Используйте `--help`, чтобы получить все параметры командной строки:

```txt
Description:
  Exchange point to upload/download with Portal5.

Usage:
  CryptoBot [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  clean        Очистить лишнее из отчетности старее 30 дней
  load <id>    Загрузить одно сообщение или все по фильтру
  z <130|137>  Запустить задачу XX
```

По команде `load --help`:

```txt
Description:
  Загрузить одно сообщение или все по фильтру

Usage:
  CryptoBot load [<id>] [options]

Arguments:
  <id>  Идентификатор одного сообщения (guid)

Options:
  -z, --zadacha <zadacha>    Номер задачи XX ('Zadacha_XX')
  -d, --today                Текущий день только
  -f, --min-date <min-date>  С какой даты (yyyy-mm-dd)
  -t, --max-date <max-date>  По какую дату (yyyy-mm-dd)
  --min-size <min-size>      От какого размера (байты)
  --max-size <max-size>      До какого размера (байты)
  --inbox                    Входящие сообщения только
  --outbox                   Исходящие сообщения только
  --status <status>          Статус сообщений
                             для inbox: new, read, replied
                             для outbox: draft, sent, delivered, error,
                             processing, registered, rejected, success
  --page <page>              Номер страницы по 100 сообщений
  -?, -h, --help             Show help and usage information
```

По команде `z --help`:

```txt
Description:
  Запустить задачу XX

Usage:
  CryptoBot z <xx> [options]

Arguments:
  <xx>  Номер задачи XX ('Zadacha_XX')
        130: Получение информации об уровне риска ЮЛ/ИП
        137: Ежедневное информирование Банка России о составе и объеме клиентской базы

Options:
  -?, -h, --help  Show help and usage information
```

## API usage examples / Примеры использования API

Документация на REST-API по Portal5 (ЕПВВ) находится по адресу
<https://www.cbr.ru/lk_uio/guide/rest_api/>.

В программе реализованы по документу "30.09.2023 Описание внешнего
взаимодействия. Технические условия внешнего обмена. Версия 2.4"
все актуальные разделы 3.1.3-3.1.6.

Программа получает с портала всю справочную информацию и решает следующие
конкретные задачи (названия приведены по справочнику задач `tasks`):

- `Zadacha_2-1` - Ответ на запрос, предписание. Запрос в Банк России. Квитанции из ВП ЕПВВ
- `Zadacha_3-1` - Запрос, предписание. Ответ на запрос НФО. Квитанции из САДД
- `Zadacha_54` - Запросы ЦИК
- `Zadacha_130` - Получение информации об уровне риска ЮЛ/ИП
- `Zadacha_137` - Ежедневное информирование Банка России о составе и объеме клиентской базы (ФПС "Отчетность")
- Очистка сообщений отчетности ПП Дельта в `inbox`:
  - `Zadacha_97`  - Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС1)
  - `Zadacha_107` - Извещение о результатах контроля отчетности субъектов НПС (ИЭС1)
  - `Zadacha_114` - Извещение о результатах контроля представления формы 0409310 (ИЭС1)
  - `Zadacha_123` - Извещение о результатах контроля представления формы 0409310 (ИЭС2)
  - `Zadacha_130` - Получение информации об уровне риска ЮЛ/ИП
  - `Zadacha_133` - Извещение о результатах контроля отчетности субъектов НПС (ИЭС2)
  - `Zadacha_140` - Извещение о результатах контроля информации о ВПОДК и их результатах (ИЭС2)
  - `Zadacha_156` - Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС1) и др.
  - `Zadacha_159` - Извещение о результатах контроля представления формы 0409601 отчет ко(нко) (ИЭС2) и др.
- Очистка сообщений отчетности ПП Дельта в `outbox`:
  - `Zadacha_155` - Представление отчетности КО в Банк России

Легко добавить и остальные по потребности.

К каждой задаче в API может применяться фильтр дат
(в программе некоторые зафиксированы на 14 или 30 дней - по смыслу).

По завершении каждой операции подписчикам отправляются уведомления по
e-mail.

### ЗСК (Светофор)

Получение реестра (`Zadacha_130`): 

    CryptoBot z 130

Отправка перечня клиентов (`Zadacha_137`):

    CryptoBot z 137

### Скачивание конкретного сообщения или по фильтру

Скачивание конкретного сообщения (указать его `Id`):

    CryptoBot load d55cdbbb-e41f-4a2a-8967-78e2a6e15701

Скачивание Входящих писем из ЛК (`Zadacha_3-1`) по фильтру дат (за сегодня):

    CryptoBot load -z 3-1 -d

Скачивание Исходящих писем из ЛК (`Zadacha_2-1`) по фильтру дат (за 2023):

    CryptoBot load -z 2-1 -f 2023-01-01 -t 2023-12-31

Скачивание запросов ЦИК (`Zadacha_54`) по фильтру дат (с начала июля):

    CryptoBot load -z 54 -f 2024-07-01

Если стоит CryptoPro CSP, то все скачанные пакеты можно расшифровать для
помещения на хранение. Если в скачиваемом периоде менялись ключи, то можно
указать предыдущие. После скачивания можно очистить место в ЛК.

### Очистка места в ЛК

Очистка ЛК от служебной информации ПП Дельта.
В коде зафиксировано оставить последние 30 дней (на самом деле и они не
нужны - все есть в самой ПП Дельта). Недокументированный код 0 для этой
задачи назначен условно - для целей единообразного запуска программы:

    CryptoBot clean

## Requirements / Требования

- .NET 8 Desktop Runtime
- CryptoPro CSP (опционально - для криптоопераций)

## Build / Построение

Build an app with many dlls (в папке дистрибутива будет `exe` и очень много
сопутствующих отдельных `dll` - вариант разработки в .NET по умолчанию):

    dotnet publish CryptoBot\CryptoBot.csproj -o Distr

Build a single-file app when NET Desktop runtime required (будет один
исполняемый файл `exe`, но требуется предварительная установка
общесистемной среды .NET - предпочитаемый мною вариант):

    dotnet publish CryptoBot\CryptoBot.csproj -o Distr -r win-x64 -p:PublishSingleFile=true --no-self-contained

Build a single-file app when no runtime required (вариант, когда
предварительная установка общесистемной среды .NET не нужна -
всё нужное из среды встроено в один большой файл - может быть полезно
для запуска в закрытой системе на AstraLinux, например, если применимо):

    dotnet publish CryptoBot\CryptoBot.csproj -o Distr -r win-x64 -p:PublishSingleFile=true

Или просто используйте `build.cmd` с предустановленным одним
из этих вариантов и созданием дистрибутивных архивов с исполняемой
программой и исходниками конкретной версии.

## Versioning / Порядок версий

Номер версии программы указывается по нарастающему принципу:

* Требуемая версия .NET (8);
* Год текущей разработки (2024);
* Месяц без первого нуля и день редакции (624 - 24.06.2024);
* Номер билда - просто нарастающее число для внутренних отличий.
Если настроен сервис AppVeyor, то это его автоинкремент.

Продукт развивается для собственных нужд, а не по коробочной
стратегии, и поэтому *Breaking Changes* могут случаться чаще,
чем это принято в *SemVer*.

## License / Лицензия

Licensed under the [Apache License, Version 2.0](LICENSE).  
Вы можете использовать эти материалы под свою ответственность.

[![Telegram](https://img.shields.io/badge/t.me-dievdo-blue?logo=telegram)](https://t.me/dievdo)
