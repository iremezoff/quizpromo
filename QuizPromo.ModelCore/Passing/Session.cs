using System;
using System.Collections.Generic;
using QuizPromo.Infrastructure.DDD;
using QuizPromo.ModelCore.BoundedContext;

namespace QuizPromo.ModelCore.Passing
{
    public class Session : AggregateRoot
    {
        public virtual User User { get; set; }
        public virtual Test Test { get; set; }

        public virtual Question CurrentQuestion { get; set; }

        public virtual ICollection<SessionQuestion> AssignedQuestions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }

        public DateTimeOffset BeginDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}