namespace QuizPromo.ModelCore.Answers
{
    // for compatable to EF7 that doesn't support many-to-many relationship unlike EF6
    public class SingleAnswerInMultipleAnswer
    {
        public SingleAnswer SingleAnswer { get; set; }
        public MultipleAnswer MultipleAnswer { get; set; }
        public int SingleAnswerId { get; set; }
        public int MultipleAnswerId { get; set; }
    }
}