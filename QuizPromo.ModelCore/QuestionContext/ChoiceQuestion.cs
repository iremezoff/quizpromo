using System.Collections.Generic;
using QuizPromo.ModelCore.BoundedContext;

namespace QuizPromo.ModelCore.QuestionContext
{
    public abstract class ChoiceQuestion: Question
    {
        public virtual ICollection<AnswerVariant> AnswerVariants { get; set; }
    }
}