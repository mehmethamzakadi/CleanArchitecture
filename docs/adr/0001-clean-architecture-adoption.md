# Clean Architecture Adoption

## Status
Accepted

## Context
Modern bir .NET backend projesinin nasıl yapılandırılması gerektiği konusunda bir karar vermemiz gerekiyordu. Önemli kriterlerimiz:

- Sürdürülebilirlik
- Test edilebilirlik
- Ölçeklenebilirlik
- Domain odaklı geliştirme
- Bağımlılıkların yönetimi

## Decision
Clean Architecture pattern'ini benimsemeye karar verdik. Bu mimari yaklaşım:

- Domain-centric bir yaklaşım sunar
- Bağımlılıkları içten dışa doğru yönetir
- SOLID prensiplerini destekler
- Test edilebilirliği artırır
- Teknoloji bağımsızlığı sağlar

Katmanlar:
1. Core (Domain + Application)
2. Infrastructure
3. Presentation
4. Shared Kernel

## Consequences

### Olumlu
- Domain logic'in izole edilmesi
- Yüksek test coverage imkanı
- Kolay teknoloji değişimi
- Temiz dependency graph
- Daha iyi separation of concerns

### Olumsuz
- Initial setup complexity
- Daha fazla boilerplate kod
- Steep learning curve
- Küçük projeler için overengineering riski

## Notes
- CQRS pattern ile birlikte kullanılacak
- MediatR için pipeline behavior'lar eklenecek
- Validation için FluentValidation kullanılacak
- ORM olarak Entity Framework Core tercih edilecek 