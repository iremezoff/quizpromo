using QuizPromo.Infrastructure.DDD;

namespace QuizPromo.ModelCore.QuestionContext
{
    public class AnswerVariant : ValueEntity
    {
        public ChoiceQuestion Question { get; set; }
        public string Value { get; set; }
    }
}