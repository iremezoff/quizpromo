using System;

namespace QuizPromo.Infrastructure.DDD
{
    public interface ISafetyDeletable
    {
        DateTimeOffset Created { get; set; }
        DateTimeOffset Updated { get; set; }
        bool IsDeleted { get; set; }
    }
}
