using System;
using QuizPromo.ModelCore.BoundedContext;

namespace QuizPromo.ModelCore.QuestionContext
{
    public class SingleChoiceQuestion : ChoiceQuestion
    {
        public override bool Respond(Answer answer)
        {
            throw new NotImplementedException();
        }
    }
}