# Proje Dokümantasyonu

## Klasör Yapısı

### ADR (Architecture Decision Records)
Bu klasör, projede alınan önemli mimari kararların kayıtlarını içerir.
- Teknoloji seçimleri
- Mimari pattern'ler
- Framework tercihleri
- Database seçimleri

### Guides (Teknik Rehberler)
- Geliştirici kurulum rehberi
- Coding standards
- Best practices
- Troubleshooting guides

### Postman
- API koleksiyonları
- Environment değişkenleri
- Test senaryoları
- Example requests

### Diagrams
- System architecture
- Database schema
- Component diagrams
- Sequence diagrams

## Dokümantasyon Yazım Kuralları

### ADR Format
```markdown
# Title: [Karar Başlığı]

## Status
[Accepted/Rejected/Deprecated/Superseded]

## Context
[Kararın alınmasına neden olan bağlam]

## Decision
[Alınan karar ve nedenleri]

## Consequences
[Kararın olumlu ve olumsuz sonuçları]
```

### Diagram Standards
- PlantUML veya Draw.io kullanılmalı
- Her diagram açıklama içermeli
- Versiyon kontrolü yapılmalı
- Export formatı: PNG ve kaynak dosya

### API Documentation
- OpenAPI/Swagger standardı
- Example requests ve responses
- Error scenarios
- Authentication details 