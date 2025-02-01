# Cursor Rules - Clean Architecture .NET 9 Project

## Proje Genel Bakış
Bu döküman, Clean Architecture prensiplerine dayalı .NET 9 backend projesinin Cursor AI tarafından nasıl yorumlanması ve analiz edilmesi gerektiğini tanımlar.

## Mimari Yapı

### Katmanlar ve Sorumlulukları

#### 1. Core Katmanı
- **Domain Layer**
  - Entities: İş domain'inin temel varlıkları
  - Value Objects: Değer nesneleri
  - Events: Domain olayları
  - Interfaces: Soyutlamalar
  - Services: Domain servisleri
  - Specifications: İş kuralı spesifikasyonları

- **Application Layer**
  - Commands: İş komutları
  - Queries: Veri sorgulama işlemleri
  - DTOs: Veri transfer objeleri
  - Validators: Doğrulama kuralları
  - Mappings: Nesne dönüşümleri
  - Behaviors: MediatR pipeline davranışları

#### 2. Infrastructure Katmanı
- Persistence: Veritabanı işlemleri
- Identity: Kimlik yönetimi
- Cache: Önbellekleme
- Logging: Log yönetimi
- MessageBus: Mesajlaşma altyapısı
- Email: E-posta servisleri
- Storage: Dosya depolama

#### 3. Presentation Katmanı
- Controllers: API kontrolcüleri
- Middleware: Ara katman yazılımları
- Models: API modelleri
- Swagger: API dokümantasyonu
- HealthChecks: Sağlık kontrolleri

## Kod Standartları

### İsimlendirme Kuralları
```csharp
// Entities
public class OrderItem
public class Customer

// Interfaces
public interface IRepository<T>
public interface IUnitOfWork

// Commands
public class CreateOrderCommand
public class UpdateCustomerCommand

// Queries
public class GetOrderByIdQuery
public class ListCustomersQuery

// Handlers
public class CreateOrderCommandHandler
public class GetOrderByIdQueryHandler

// Controllers
public class OrdersController
public class CustomersController
```

### Dosya Organizasyonu
```
src/
├── Core/
│   ├── Domain/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   └── Events/
│   └── Application/
│       ├── Commands/
│       ├── Queries/
│       └── DTOs/
├── Infrastructure/
│   ├── Persistence/
│   ├── Identity/
│   └── Services/
└── Presentation/
    └── API/
```

## Geliştirme Kuralları

### Best Practices
1. Her entity için ayrı bir command/query handler
2. Validation logic'in command handler'dan önce çalışması
3. Domain event'lerin domain entity'ler içinde tetiklenmesi
4. Repository pattern ile veri erişimi
5. Unit of Work ile transaction yönetimi

### Yasaklanan Pratikler
- Domain entity'lerin direkt olarak API response'larında kullanılması
- Infrastructure concern'lerin Core katmanına sızması
- İş mantığının Application katmanı dışında uygulanması
- Entity Framework dependency'sinin Core katmanında olması

## Cursor AI Önerileri

### Code Analysis
- Clean Architecture prensiplerinin ihlali
- Dependency Injection container kayıtları
- CQRS pattern implementasyonu
- Domain-driven design prensipleri

### Snippets ve Templates
```csharp
// Entity Template
public class EntityName
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
}

// Command Template
public class CommandName : IRequest<CommandResult>
{
    public Guid Id { get; set; }
}

// Query Template
public class QueryName : IRequest<QueryResult>
{
    public Guid Id { get; set; }
}
```

## DevOps ve Deployment

### Docker Konfigürasyonu
- Multi-stage build yaklaşımı
- Alpine-based imajlar
- Development ve production ortamları için ayrı Dockerfile'lar

### Kubernetes Deployment
- Horizontal pod autoscaling
- Liveness ve readiness probe'ları
- ConfigMap ve Secret yönetimi

## Monitoring ve Logging

### Telemetri
- Request/response loglama
- Performance metrics
- Error tracking
- Distributed tracing

### Health Checks
- Database connectivity
- External service availability
- System resource usage
- Cache service status

## Güvenlik Kontrolleri

### Code Scanning
- SAST (Static Application Security Testing)
- Dependencies vulnerability scanning
- Code quality metrics

### Runtime Security
- JWT token validation
- API rate limiting
- SQL injection prevention
- XSS protection

## Test Stratejisi

### Test Tipleri
1. Unit Tests
   - Command/Query handlers
   - Domain logic
   - Validators
   
2. Integration Tests
   - API endpoints
   - Database operations
   - External service integrations

3. Performance Tests
   - Load testing
   - Stress testing
   - Endurance testing

### Test Conventions
```csharp
// Unit Test
public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrder_WithValidData_ShouldSucceed()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}
```

## CI/CD Pipeline

### Build Aşamaları
1. Code checkout
2. Dependency restore
3. Build
4. Test execution
5. Code analysis
6. Docker image build
7. Container registry push
8. Kubernetes deployment

## Katkıda Bulunma Kuralları

### Pull Request Süreci
1. Feature branch oluşturma
2. Kod yazımı ve testler
3. PR template doldurma
4. Code review
5. CI pipeline kontrolü
6. Merge işlemi

## Yardımcı Kaynaklar
- Architecture Decision Records (ADR)
- API Documentation
- Integration Guides
- Development Environment Setup
