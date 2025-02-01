# Test Yapısı

## Klasör Yapısı

### Unit Tests
- Domain Entity testleri
- Application Service testleri
- Command/Query Handler testleri
- Validator testleri

### Integration Tests
- Repository testleri
- Database entegrasyon testleri
- API endpoint testleri
- External servis entegrasyon testleri

### Performance Tests
- Load testleri
- Stress testleri
- Endurance testleri
- Scalability testleri

### Architecture Tests
- Clean Architecture kuralları testleri
- Dependency flow testleri
- Layer isolation testleri
- Naming convention testleri

### Security Tests
- Authentication testleri
- Authorization testleri
- Input validation testleri
- Security header testleri

## Test Yazım Kuralları

### Naming Convention
```csharp
[TestClass]
public class OrderServiceTests 
{
    [TestMethod]
    public async Task MethodName_Scenario_ExpectedResult() 
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}
```

### Best Practices
1. Her test metodu tek bir şeyi test etmeli
2. Test metod isimleri açıklayıcı olmalı
3. AAA (Arrange-Act-Assert) pattern kullanılmalı
4. Mock framework'ü tutarlı kullanılmalı
5. Test data factory pattern kullanılmalı

### Code Coverage Hedefleri
- Domain Layer: %90+
- Application Layer: %85+
- Infrastructure Layer: %75+
- Presentation Layer: %70+ 