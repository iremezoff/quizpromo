using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Cors;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using Quiz.Resources;
using QuizPromo.Infrastructure.DDD;
using QuizPromo.Infrastructure.EF;
using QuizPromo.ModelCore;
using WebApi.Hal;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Quiz.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
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

            TestListRepresentation testList = new TestListRepresentation(testReps, tests.Count, 1, 1, new Link("test", "~/test{?page}"), null);

            return Ok(testList);
        }
    }

    [Route("[controller]")]
    [EnableCors("any")]
    public class DataController : Controller
    {
        private readonly DbContext _context;
        private readonly IRepositoryWithTypedId<Question, int> _questionRepo;

        public DataController(DbContext context, IRepositoryWithTypedId<Question, int> questionRepo)
        {
            _context = context;
            _questionRepo = questionRepo;

            //Question<MiltipleAnswer> q1;
            //var q1 = new MultipleChoicesQuestion() ;
            //q1.AnswerVariants = new List<AnswerVariant>() {new AnswerVariant() {Question = q1}};
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            dynamic d = new { test = "1", random = 7 };

            var qs = _questionRepo.GetAll();

            return Ok(qs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {


            return Ok(new BeerRepresentation()
            {
                Id = 7,
                Name = "fdsdfsd",
                BreweryId = 45,
                BreweryName = "5454",
                StyleId = 6456,
                StyleName = "sdfsgds",
                ReviewIds = new[] { 232, 434, 43 }.ToList()
            });
        }

        [HttpPost("{id}")]
        public IActionResult Post(int id)
        {
            var category = new Category() { Name = "etst" + new Random().Next() };
            _context.Set<Category>().Add(category);

            //var multipleChoicesQuestion = new MultipleChoicesQuestion()
            //{
            //    Category = category,
            //    Name = 111.ToString(),
            //    Statement = "gfgdf",

            //};

            var multipleChoicesQuestion = _questionRepo.GetById(id) as MultipleChoicesQuestion;


            multipleChoicesQuestion.AnswerVariants = new List<AnswerVariant>() { new AnswerVariant() };

            _questionRepo.Save(multipleChoicesQuestion);
            //_context.Set<Question>().Add(multipleChoicesQuestion);

            _context.SaveChanges();



            return Ok(id);
        }
    }


    public class BeerRepresentation : Representation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? BreweryId { get; set; }
        public string BreweryName { get; set; }

        public int? StyleId { get; set; }
        public string StyleName { get; set; }

        [JsonIgnore]
        public List<int> ReviewIds { get; set; }

        public override string Rel
        {
            get { return LinkTemplates.Beers.Beer.Rel; }
            set { }
        }

        public override string Href
        {
            get { return LinkTemplates.Beers.Beer.CreateLink(new { id = Id }).Href; }
            set { }
        }

        protected override void CreateHypermedia()
        {
            if (StyleId != null)
                Links.Add(LinkTemplates.BeerStyles.Style.CreateLink(new { id = StyleId }));
            if (BreweryId != null)
                Links.Add(LinkTemplates.Breweries.Brewery.CreateLink(new { id = BreweryId }));

            if (ReviewIds != null && ReviewIds.Count > 0)
                foreach (var rid in ReviewIds)
                    Links.Add(LinkTemplates.Reviews.GetBeerReview.CreateLink(new { id = Id, rid }));
        }
    }

    public static class LinkTemplates
    {
        public static class Breweries
        {
            /// <summary>
            /// /breweries
            /// </summary>
            public static Link GetBreweries { get { return new Link("breweries", "~/breweries"); } }

            /// <summary>
            /// /breweries/{id}
            /// </summary>
            public static Link Brewery { get { return new Link("brewery", "~/breweries/{id}", "title"); } }

            /// <summary>
            /// /breweries/{id}/beers
            /// </summary>
            public static Link AssociatedBeers { get { return new Link("beers", "~/breweries/{id}/beers{?page}"); } }
        }

        public static class BeerStyles
        {
            /// <summary>
            /// /styles
            /// </summary>
            public static Link GetStyles { get { return new Link("styles", "~/styles"); } }

            /// <summary>
            /// /styles/{id}/beers
            /// </summary>
            public static Link AssociatedBeers { get { return new Link("beers", "~/styles/{id}/beers{?page}"); } }

            /// <summary>
            /// /styles/{id}
            /// </summary>
            public static Link Style { get { return new Link("style", "~/styles/{id}"); } }
        }

        public static class Beers
        {
            /// <summary>
            /// /beers?page={page}
            /// </summary>
            public static Link GetBeers { get { return new Link("beers", "~/beers{?page}"); } }

            /// <summary>
            /// /beers?searchTerm={searchTerm}&amp;page={page}
            /// </summary>
            public static Link SearchBeers { get { return new Link("page", "~/beers{?searchTerm,page}"); } }

            /// <summary>
            /// /beers/{id}
            /// </summary>
            public static Link Beer { get { return new Link("beer", "~/beers/{id}"); } }
        }

        public static class BeerDetails
        {
            public static Link GetBeerDetail { get { return new Link("beerdetail", "~/beerdetail/{id}"); } }
        }

        public static class Reviews
        {
            /// <summary>
            /// /beers/{id}/reviews/{rid}
            /// </summary>
            public static Link GetBeerReview { get { return new Link("review", "~/beers/{id}/reviews/{rid}"); } }
        }
    }

}
