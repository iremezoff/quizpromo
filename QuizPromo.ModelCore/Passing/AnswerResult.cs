using QuizPromo.Infrastructure.DDD;
using QuizPromo.ModelCore.BoundedContext;

namespace QuizPromo.ModelCore.Passing
{
    public class AnswerResult : ValueEntity
    {
        public Session Session { get; set; }
        public Answer Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}