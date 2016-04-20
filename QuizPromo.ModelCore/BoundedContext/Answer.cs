using QuizPromo.Infrastructure.DDD;
using QuizPromo.ModelCore.Passing;

namespace QuizPromo.ModelCore.BoundedContext
{
    public abstract class Answer : ValueEntity
    {
        public Session Session { get; set; }

        public Question Question { get; set; }

        public bool IsCorrect { get; set; }
    }
}