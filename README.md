# BioMatcher

BioMatcher is a .NET Framework fingerprint matching platform that includes:

- A multithreaded matching engine
- A WinForms SignalR server
- A WinForms client
- Benchmark tooling
- Optional ASMX/WCF service endpoints
- Pluggable fingerprint SDK adapters (Griaule, SourceAFIS, SecuGen)

## Architecture

![System Architecture](Docs/System%20Architecture%20Design.png)

## Solution Overview

`BioMatcher.sln` contains these main projects:

| Project | Type | Purpose |
| --- | --- | --- |
| `BioMatcher` | Class library | Core matching pipeline, cache, threading, and transport client wrapper |
| `BioMatcher.Server` | WinForms app | Hosts SignalR/Owin endpoint and server-side matching/cache |
| `BioMatcher.Client` | WinForms app | Sends identify requests via SignalR, ASMX, or WCF |
| `BioMatcher.Benchmark` | WinForms app | Performance testing for cache size/threading scenarios |
| `BioMatcher.WebService` | ASP.NET web app | ASMX/WCF service surface over `MatchManager` |
| `BioMatcher.ServiceAdapter` | Class library | SOAP adapter to fingerprint source service |
| `BioMatcher.Griaule` | Class library | Griaule SDK adapter (`IFingerprintSdk`) |
| `BioMatcher.OpenAFIS` | Class library | SourceAFIS adapter (partial implementation) |
| `BioMatcher.SecuGen` | Class library | SecuGen adapter (partial implementation) |

## Requirements

- Windows (WinForms + native fingerprint SDK DLLs)
- .NET Framework 4.5.2 Developer Pack
- Visual Studio 2017+ (solution format is VS15)
- NuGet package restore enabled

## Quick Start (Prebuilt Binaries)

Prebuilt outputs are available under `Build/`.

1. Configure server in `Build/Server/BioMatcher.Server.exe.config`.
2. Configure client in `Build/Client/BioMatcher.Client.exe.config`.
3. Start `Build/Server/BioMatcher.Server.exe`.
4. Start `Build/Client/BioMatcher.Client.exe`.

Recommended local SignalR settings in the client config:

```xml
<add key="BioMatcher.ServerURI" value="http://localhost:8080/signalr" />
<add key="BioMatcher.ClientMode" value="2" />
<add key="BioMatcher.ClietMode" value="2" />
```

Notes:

- `2` means SignalR mode (`0` = ASMX, `1` = WCF, `2` = SignalR).
- `BioMatcher.ClietMode` (legacy typo) is what the runtime currently reads.
- If your server runs on another host/port, update `BioMatcher.ServerURI` accordingly.

## Build From Source

1. Open `BioMatcher.sln` in Visual Studio.
2. Restore NuGet packages.
3. Build the solution (`Debug` or `Release`).
4. Set startup projects as needed:
   - `BioMatcher.Server` + `BioMatcher.Client` for SignalR flow
   - `BioMatcher.Benchmark` for performance tests
   - `BioMatcher.WebService` for ASMX/WCF hosting

MSBuild example:

```bash
msbuild BioMatcher.sln /t:Restore,Build /p:Configuration=Release
```

## Runtime Configuration

Common settings (in `App.config` / `.exe.config` / `Web.config`):

| Key | Description |
| --- | --- |
| `BioMatcher.ServerURI` | SignalR endpoint (`http://host:port/signalr`) for client, base URL for server host |
| `BioMatcher.ClientMode` / `BioMatcher.ClietMode` | Transport mode: `0` ASMX, `1` WCF, `2` SignalR |
| `BioMatcher.CacheMode` | `0` no client cache, `1` locale cache, `2` server mode |
| `LocaleId` | Locale partition for locale-cached requests |
| `BioMatcher.CacheSize` | Cache size target for benchmark scenarios |
| `BioMatcher.BenchmarkMode` | Enables benchmark behavior and benchmark output |
| `BioMatcher.SdkPath` | Assembly name for selected matcher SDK |
| `BioMatcher.SdkType` | Fully-qualified class implementing `IFingerprintSdk` |

## SDK Adapters

Switch matcher implementation via config:

```xml
<add key="BioMatcher.SdkPath" value="BioMatcher.Griaule" />
<add key="BioMatcher.SdkType" value="BioMatcher.Griaule.FingerprintMatcher" />
```

Available adapters:

- `BioMatcher.Griaule` (most complete)
- `BioMatcher.OpenAFIS` (partial / contains `NotImplementedException` paths)
- `BioMatcher.SecuGen` (partial / template extraction paths not implemented)

Native/vendor DLLs are stored under `Libraries/`.

## Data Sources and External Dependencies

By default, cache hydration is service-backed via SOAP endpoints configured in:

- `BioMatcher.ServiceAdapter/app.config`
- `BioMatcher.Server/App.config`
- `BioMatcher.Client/App.config`
- `BioMatcher.WebService/Web.config`

The repo ships placeholder endpoints under `*.externalservice.org`; replace them with your actual service URLs.

For benchmark-style local template data, the repository includes sample templates in `Build/Data/`.

## Documentation

Additional architecture and R&D docs are in `Docs/`:

- `Global Fingerprint Solution R&D.pdf`
- `System Architecture Design.png`

## License

MIT (see `LICENSE`).
