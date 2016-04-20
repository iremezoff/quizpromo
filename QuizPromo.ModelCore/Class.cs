using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using QuizPromo.Infrastructure.DDD;
using QuizPromo.ModelCore;

namespace QuizPromo.ModelCore
{
    public abstract class Question/*<T>*/ : DictionaryEntity, ISafetyDeletable //where T : Answer
    {
        public virtual Category Category { get; set; }
        public string Statement { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool IsDeleted { get; set; }


        public abstract bool Respond(Answer answer);
    }

    public abstract class ChoiceQuestion/*<T>*/ : Question//<T> where T : Answer
    {
        public virtual ICollection<AnswerVariant> AnswerVariants { get; set; }
    }

    public class SingleChoiceQuestion : ChoiceQuestion//<SingleAnswer>
    {
        public override bool Respond(Answer answer)
        {
            throw new NotImplementedException();
        }
    }

    public class Category : DictionaryEntity
    {

    }

    public class MultipleChoicesQuestion : ChoiceQuestion//<MiltipleAnswer>
    {
        public override bool Respond(Answer answer)
        {


            throw new NotImplementedException();
        }
    }

    //public class StrictOrderQuestion : MultipleChoicesQuestion
    //{
    //}


    public class Test : AggregateRoot
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }

    public class User : AggregateRoot
    {
        public string FullName { get; set; }
    }

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

    //public class SessionFactory : ISessionFactory
    //{
    //    public Session MakeSession(Test test)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public interface ISessionFactory
    //{
    //    Session MakeSession(Test test);
    //}

    // for compatable to EF7 that doesn't support many-to-many relationship unlike EF6
    public class SessionQuestion
    {
        public int QuestionId { get; set; }
        public int SessionId { get; set; }
        public Question Question { get; set; }
        public Session Session { get; set; }
    }

    public class AnswerResult : ValueEntity
    {
        public Session Session { get; set; }
        public Answer Answer { get; set; }
        public bool IsCorrect { get; set; }
    }

    public abstract class Answer : ValueEntity
    {
        public Session Session { get; set; }

        public Question Question { get; set; }

        public bool IsCorrect { get; set; }
    }

    public class SingleAnswer : Answer
    {
        public string Choice { get; set; }
    }

    public class MultipleAnswer : Answer
    {
        public ICollection<SingleAnswerInMultipleAnswer> Choices { get; set; }
    }

    public class SingleAnswerInMultipleAnswer
    {
        public SingleAnswer SingleAnswer { get; set; }
        public MultipleAnswer MultipleAnswer { get; set; }
        public int SingleAnswerId { get; set; }
        public int MultipleAnswerId { get; set; }
    }

    public class AnswerVariant : ValueEntity
    {
        public ChoiceQuestion Question { get; set; }
        public string Value { get; set; }
    }
}
