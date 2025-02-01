# 2. Domain-Driven Design (DDD) Yaklaşımı

Tarih: 2024-02-01

## Durum

Kabul edildi

## Bağlam

Projemiz karmaşık iş kuralları ve domain mantığı içeriyor. Bu karmaşıklığı yönetmek ve domain uzmanları ile geliştiriciler arasında ortak bir dil oluşturmak için bir yaklaşım gerekiyor.

## Karar

Domain-Driven Design prensiplerini uygulamaya karar verdik. Bu yaklaşım ile:

- Ubiquitous Language kullanımı
- Bounded Context'lerin belirlenmesi
- Aggregate Root'ların tanımlanması
- Domain Event'lerin kullanımı
- Value Object'lerin oluşturulması

### Örnek Yapı

```csharp
// Aggregate Root
public class Order : BaseEntity, IAggregateRoot
{
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    
    public CustomerId CustomerId { get; private set; }
    public Money TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    
    public void AddItem(Product product, int quantity)
    {
        var orderItem = new OrderItem(product.Id, product.Price, quantity);
        _orderItems.Add(orderItem);
        
        RecalculateTotal();
        AddDomainEvent(new OrderItemAddedEvent(this, orderItem));
    }
    
    private void RecalculateTotal()
    {
        TotalAmount = new Money(_orderItems.Sum(i => i.Price.Amount * i.Quantity));
    }
}

// Value Object
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}

// Domain Event
public class OrderItemAddedEvent : DomainEvent
{
    public Order Order { get; }
    public OrderItem AddedItem { get; }
}
```

## Sonuçlar

### Olumlu

- Domain mantığının daha iyi anlaşılması
- İş kurallarının domain modelinde kapsüllenmesi
- Domain uzmanları ile daha iyi iletişim
- Kodun daha maintainable olması

### Olumsuz

- Öğrenme eğrisi
- Daha fazla başlangıç maliyeti
- Bazı basit CRUD operasyonları için overengineering olabilir

### Riskler

- Yanlış bounded context tanımlamaları
- Aggregate sınırlarının yanlış belirlenmesi
- Domain event'lerin yanlış kullanımı

## Alternatifler

1. Anemic Domain Model
2. Transaction Script Pattern
3. Table Module Pattern

## Referanslar

- [Domain-Driven Design Reference](https://domainlanguage.com/ddd/reference/)
- [DDD Quickly](https://www.infoq.com/minibooks/domain-driven-design-quickly/)
- [Vaughn Vernon - Implementing Domain-Driven Design](https://www.amazon.com/Implementing-Domain-Driven-Design-Vaughn-Vernon/dp/0321834577) 