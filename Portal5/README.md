# CryptoBot

[![Build status](https://ci.appveyor.com/api/projects/status/25pytmgy12ey90ak?svg=true)](https://ci.appveyor.com/project/diev/portal5-works)
[![.NET10 Desktop](https://github.com/diev/Portal5-Works/actions/workflows/dotnet10-desktop.yml/badge.svg)](https://github.com/diev/Portal5-Works/actions/workflows/dotnet10-desktop.yml)
[![GitHub Release](https://img.shields.io/github/release/diev/Portal5-Works.svg)](https://github.com/diev/Portal5-Works/releases/latest)

GUI viewer for Portal5.  
Графический интерфейс к Portal5 по API.

## Параметры

*Windows Credential Manager* (*Диспетчер учетных данных*) в Панели управления
Windows - все пароли для всех программ меняются в одном месте и скрыты от
пользователей - вот какие настройки ищет там программа:

- `Portal5test https://{host}` - программа ищет эту настройку по маске
`Portal5test *` - как указано в параметре `Api:TargetName` - это пример
тестового сервера
  - `{username}` - логин
  - `{password}` (пароль
- `Portal5 https://{host}` - пример указания рабочего сервера - и указать
программе маску `Portal5 *` для использования
  - `{username}` - логин
  - `{password}` - пароль

## Порядок версий

Номер версии программы указывается по нарастающему принципу:

* Протестированная максимальная версия NET (10);
* Год текущей разработки (2026);
* Месяц без первого нуля и день редакции (121 - 21.01.2026);
* Номер билда - просто нарастающее число для внутренних отличий.
Если настроен сервис автосборки (например, AppVeyor), то это его
автоинкремент.

Продукт развивается для собственных нужд, а не по коробочной
стратегии, и поэтому *Breaking Changes* могут случаться чаще,
чем это принято в *SemVer*.

## License / Лицензия

Licensed under the [Apache License, Version 2.0](LICENSE).  
Вы можете использовать эти материалы под свою ответственность.

[![Telegram](https://img.shields.io/badge/t.me-dievdo-blue?logo=telegram)](https://t.me/dievdo)
