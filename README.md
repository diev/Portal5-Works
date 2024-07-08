# Portal5-Works
[![Build status](https://ci.appveyor.com/api/projects/status/25pytmgy12ey90ak?svg=true)](https://ci.appveyor.com/project/diev/portal5-works)
[![GitHub Release](https://img.shields.io/github/release/diev/Portal5-Works.svg)](https://github.com/diev/Portal5-Works/releases/latest)

Works with API of Portal5.

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

## Requirements / Требования

- .NET 8 Desktop Runtime

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
