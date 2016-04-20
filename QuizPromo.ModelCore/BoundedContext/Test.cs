using System.Collections.Generic;
using QuizPromo.Infrastructure.DDD;

namespace QuizPromo.ModelCore.BoundedContext
{
    public class Test : AggregateRoot
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}