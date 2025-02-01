# Geliştirme Ortamı Kurulum Rehberi

## Gereksinimler

### Temel Gereksinimler
- .NET 9 SDK
- Visual Studio 2022 veya üzeri (ya da Visual Studio Code)
- Git
- Docker Desktop

### Veritabanı
- PostgreSQL 15 veya üzeri
- pgAdmin 4 (opsiyonel)

### Araçlar
- Postman
- Draw.io Desktop (opsiyonel)

## Kurulum Adımları

### 1. Repository Klonlama
```powershell
git clone https://github.com/your-username/CleanArchitecture.git
cd CleanArchitecture
```

### 2. Veritabanı Kurulumu
```powershell
# Docker ile PostgreSQL kurulumu
docker run --name postgres-cleanarch -e POSTGRES_PASSWORD=your_password -p 5432:5432 -d postgres:15
```

### 3. User Secrets Yapılandırması
```powershell
cd src/Presentation/API/CleanArchitecture.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=CleanArchitectureDb;Username=postgres;Password=your_password"
```

### 4. Bağımlılıkların Yüklenmesi
```powershell
dotnet restore
```

### 5. Veritabanı Migration
```powershell
cd src/Infrastructure/Persistence/CleanArchitecture.Infrastructure.Persistence
dotnet ef database update
```

### 6. Uygulamayı Çalıştırma
```powershell
cd src/Presentation/API/CleanArchitecture.API
dotnet run
```

## Geliştirme Araçları Kurulumu

### Visual Studio Extensions
- CodeMaid
- SonarLint
- GitFlow for Visual Studio
- Web Essentials

### Visual Studio Code Extensions
- C# Dev Kit
- REST Client
- Docker
- GitLens
- Path Intellisense

## Docker Compose ile Tüm Servisleri Başlatma
```powershell
docker-compose up -d
```

## Troubleshooting

### Sık Karşılaşılan Hatalar

1. **Veritabanı Bağlantı Hatası**
   ```
   Solution: Connection string'i kontrol edin ve PostgreSQL servisinin çalıştığından emin olun
   ```

2. **Port Çakışması**
   ```
   Solution: docker ps komutu ile çalışan containerları kontrol edin ve gerekirse portu değiştirin
   ```

3. **Migration Hataları**
   ```
   Solution: Veritabanını drop edip yeniden migrate edin:
   dotnet ef database drop --force
   dotnet ef database update
   ```

## IDE Ayarları

### Visual Studio
```json
{
    "editor.formatOnSave": true,
    "editor.rulers": [120],
    "files.trimTrailingWhitespace": true
}
```

### Visual Studio Code
```json
{
    "editor.formatOnSave": true,
    "editor.rulers": [120],
    "files.trimTrailingWhitespace": true,
    "omnisharp.enableRoslynAnalyzers": true
}
```

## Git Hooks Kurulumu

### Pre-commit Hook
```bash
#!/bin/sh
dotnet format
dotnet test
```

## CI/CD Pipeline Lokalde Çalıştırma

### GitHub Actions
```powershell
# act tool'u ile GitHub Actions'ı lokalde çalıştırma
act -n # dry run
act # run all workflows
``` 