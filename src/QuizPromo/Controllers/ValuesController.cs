using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Cors;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Quiz.Resources;
using QuizPromo.Infrastructure.DDD;
using QuizPromo.ModelCore.BoundedContext;
using QuizPromo.ModelCore.QuestionContext;
using WebApi.Hal;

namespace Quiz.Controllers
{
    [Route("[controller]")]
    [EnableCors("any")]
    public class TestController : Controller
    {
        private readonly DbContext _context;
        private readonly IRepositoryWithTypedId<Question, int> _questionRepo;

        public TestController(DbContext context, IRepositoryWithTypedId<Question, int> questionRepo)
        {
            _context = context;
            _questionRepo = questionRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tests = new List<Test>()
            {
                new Test() {Id=1, Title = "1", Description = "dfsdfsd", Questions = new List<Question>() {new SingleChoiceQuestion() {Id=1} } },
                new Test() {Id=5, Title = "5", Description = "bcffsg"},
                new Test() {Id=2, Title = "2", Description = "herbd"},
            };

            var testReps = tests.Select(t => new TestRepresentation() { Id = t.Id, Title = t.Title, Description = t.Description }).ToList();

            var testList = new TestListRepresentation(testReps, tests.Count, 1, 1, new Link("test", "~/test{?page}"), null);

            return Ok(testList);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id)
        {
            var category = new Category() { Name = "etst" + new Random().Next() };
            _context.Set<Category>().Add(category);

            var multipleChoicesQuestion = _questionRepo.GetById(id) as MultipleChoicesQuestion;

            multipleChoicesQuestion.AnswerVariants = new List<AnswerVariant>() { new AnswerVariant() };

            _questionRepo.Save(multipleChoicesQuestion);

            await _context.SaveChangesAsync();

            return Ok(id);
        }
    }
}
