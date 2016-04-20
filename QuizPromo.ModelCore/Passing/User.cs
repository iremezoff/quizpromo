using QuizPromo.Infrastructure.DDD;

namespace QuizPromo.ModelCore.Passing
{
    public class User : AggregateRoot
    {
        public string FullName { get; set; }
    }
}