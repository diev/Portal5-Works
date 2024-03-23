# Portal5-Works
Works with API of Portal5.

## Settings / ���������

Appsettings:

- `CryptoBot.config.json` (located with App `exe`)
- `%ProgramData%\Diev\CryptoBot.config.json` (overwrites if exists)

Windows Credential Manager:

- `Portal5test *` (name: 'Portal5test https://{host}', user: '{username}', pass: '{password}')
- `Portal5 *` (name: 'Portal5 https://{host}', user: '{username}', pass: '{password}')
- `CryptoPro My` (name: 'CryptoPro My', user: '{cert}', pass: '{pin}')
- `SMTP *` (name: 'SMTP {host} {port} tls', user: '{sender}', pass: '{password}')

CLI:

- `-z XX` - 'Zadacha_XX'

## Requirements / ����������

- .NET 8 Desktop Runtime

## Build / ����������

Build an app with many dlls
`dotnet publish CryptoBot\CryptoBot.csproj -o Distr`

Build a single-file app when NET Desktop runtime required 
`dotnet publish CryptoBot\CryptoBot.csproj -o Distr -r win-x64 -p:PublishSingleFile=true --self-contained false`

Build a single-file app when no runtime required
`dotnet publish CryptoBot\CryptoBot.csproj -o Distr -r win-x64 -p:PublishSingleFile=true`

## License / ��������

Licensed under the [Apache License, Version 2.0].

[Apache License, Version 2.0]: LICENSE
