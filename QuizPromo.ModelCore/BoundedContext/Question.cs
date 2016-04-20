using System;
using QuizPromo.Infrastructure.DDD;

namespace QuizPromo.ModelCore.BoundedContext
{
    public abstract class Question : DictionaryEntity, ISafetyDeletable
    {
        public virtual Category Category { get; set; }
        public string Statement { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool IsDeleted { get; set; }


        public abstract bool Respond(Answer answer);
    }
}
