namespace BusinessDirectory.Domain.Common
{
    public interface IDomainEvent
    {
        // زمان وقوع رویداد
        DateTime OccurredOn { get; }
    }
}
