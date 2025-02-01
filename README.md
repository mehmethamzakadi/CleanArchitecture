# Clean Architecture .NET 9 Projesi

Modern, ölçeklenebilir Clean Architecture prensiplerine dayalı .NET 9 backend projesi.

## 🏗️ Mimari Yapı

Proje, Clean Architecture prensiplerini takip eden katmanlı bir mimariye sahiptir:

### Core Katmanı
- **Domain**: İş mantığının kalbi, entities, value objects ve domain servisleri
- **Application**: Use case'ler, CQRS pattern ile command ve query'ler

### Infrastructure Katmanı
- **Persistence**: Veritabanı işlemleri ve repository implementasyonları
- **Identity**: Kimlik doğrulama ve yetkilendirme
- **Cache**: Redis tabanlı önbellekleme
- **Logging**: Merkezi loglama sistemi
- **MessageBus**: Mesajlaşma altyapısı
- **Email**: E-posta servisleri
- **Storage**: Dosya depolama servisleri

### Presentation Katmanı
- **API**: RESTful API endpoints
- **Middleware**: Cross-cutting concerns
- **Swagger**: API dokümantasyonu
- **HealthChecks**: Sistem sağlık kontrolleri

## 🚀 Başlangıç

### Gereksinimler
- .NET 9 SDK
- PostgreSQL
- Redis
- Docker (opsiyonel)

### Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/yourusername/cleanarchitecture.net9.git
```

2. Veritabanını oluşturun:
```bash
dotnet ef database update
```

3. Projeyi çalıştırın:
```bash
cd src/Presentation/API/CleanArchitecture.API
dotnet run
```

## 🔧 Konfigürasyon

`appsettings.json` dosyasında aşağıdaki ayarları yapılandırın:

- Database connection string
- Redis connection string
- JWT settings
- Email settings
- External service URLs

## 📚 API Dokümantasyonu

Swagger UI'a http://localhost:5000 adresinden erişebilirsiniz.

## 🏗️ Mimari Kararlar

Projede alınan önemli mimari kararlar:

1. CQRS pattern kullanımı
2. Domain-Driven Design prensipleri
3. Event-driven mimari
4. Microservice'lere geçişe hazır yapı

## 🧪 Test Stratejisi

- Unit Tests: Domain ve Application katmanı
- Integration Tests: Infrastructure ve API
- Performance Tests: Yük testleri
- Architecture Tests: Mimari kuralların kontrolü

## 📈 Monitoring

- Health Checks: `/health` endpoint'i
- Metrics: Prometheus & Grafana
- Logging: Serilog & Elasticsearch
- Tracing: OpenTelemetry

## 🔒 Güvenlik

- JWT authentication
- Role-based authorization
- API rate limiting
- Input validation
- SQL injection prevention
- XSS protection

## 🤝 Katkıda Bulunma

1. Fork the Project
2. Create your Feature Branch
3. Commit your Changes
4. Push to the Branch
5. Open a Pull Request

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. 