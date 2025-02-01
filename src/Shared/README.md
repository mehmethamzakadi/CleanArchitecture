# Shared Kernel

Bu klasör, tüm katmanlar tarafından kullanılabilecek ortak kod ve yapıları içerir.

## Klasör Yapısı

### Constants
- Sistem sabitleri
- Enum değerleri
- Hata kodları
- Validation mesajları

### Extensions
- String extensions
- DateTime extensions
- IQueryable extensions
- Collection extensions

### Utilities
- Helper metodları
- Utility sınıfları
- Common functions
- Type converters

### Configuration
- Ortak konfigürasyon modelleri
- Settings sınıfları
- Options pattern implementasyonları

## Kullanım Kuralları

### DRY (Don't Repeat Yourself)
- Tekrar eden kodlar bu katmana taşınmalı
- Utility metodları generic olmalı
- Extension metodları açıklayıcı olmalı

### Dependency Rules
- Bu katman sadece .NET Core ve temel NuGet paketlerine bağımlı olabilir
- Diğer katmanlara bağımlılık OLAMAZ
- Third-party dependencies minimize edilmeli

### Naming Conventions
```csharp
// Constants
public static class ApplicationConstants
{
    public const string DefaultCulture = "tr-TR";
}

// Extensions
public static class StringExtensions
{
    public static string ToSlug(this string value)
    {
        // Implementation
    }
}

// Utilities
public static class DateTimeHelper
{
    public static DateTime GetQuarterStart(DateTime date)
    {
        // Implementation
    }
}
```

### Documentation
- Tüm public API'ler XML comments içermeli
- Kompleks metodlar için örnek kullanımlar eklenmeli
- Breaking changes dokümante edilmeli 