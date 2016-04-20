using System.Collections.Generic;
using QuizPromo.ModelCore.BoundedContext;

namespace QuizPromo.ModelCore.Answers
{
    public class MultipleAnswer : Answer
    {
        public ICollection<SingleAnswerInMultipleAnswer> Choices { get; set; }
    }
}