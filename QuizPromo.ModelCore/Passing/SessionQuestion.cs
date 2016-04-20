using QuizPromo.ModelCore.BoundedContext;

namespace QuizPromo.ModelCore.Passing
{
    public class SessionQuestion
    {
        public int QuestionId { get; set; }
        public int SessionId { get; set; }
        public Question Question { get; set; }
        public Session Session { get; set; }
    }
}