using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quiz.Controllers;
using QuizPromo.ModelCore;
using WebApi.Hal;

namespace Quiz.Resources
{
    public class TestRepresentation : Representation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string Rel
        {
            get { return new Link("test", "~/test/{id}").Rel; }
            set { }
        }

        public override string Href
        {
            get { return new Link("test", "~/test/{id}").CreateLink(new { id = Id }).Href; }
            set { }
        }

        protected override void CreateHypermedia()
        {
            Links.Add(new Link("session", "~/session", "Take the test"));
        }
    }

    public abstract class PagedRepresentationList<TRepresentation> : SimpleListRepresentation<TRepresentation> where TRepresentation : Representation
    {
        readonly Link uriTemplate;

        protected PagedRepresentationList(IList<TRepresentation> res, int totalResults, int totalPages, int page, Link uriTemplate, object uriTemplateSubstitutionParams)
            : base(res)
        {
            this.uriTemplate = uriTemplate;
            TotalResults = totalResults;
            TotalPages = totalPages;
            Page = page;
            UriTemplateSubstitutionParams = uriTemplateSubstitutionParams;
        }

        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }

        protected object UriTemplateSubstitutionParams;

        protected override void CreateHypermedia()
        {
            var prms = new List<object> { new { page = Page } };
            if (UriTemplateSubstitutionParams != null)
                prms.Add(UriTemplateSubstitutionParams);

            Href = Href ?? uriTemplate.CreateLink(prms.ToArray()).Href;

            Links.Add(new Link { Href = Href, Rel = "self" });

            if (Page > 1)
            {
                var item = UriTemplateSubstitutionParams == null
                                ? uriTemplate.CreateLink("prev", new { page = Page - 1 })
                                : uriTemplate.CreateLink("prev", UriTemplateSubstitutionParams, new { page = Page - 1 }); // page overrides UriTemplateSubstitutionParams
                Links.Add(item);
            }
            if (Page < TotalPages)
            {
                var link = UriTemplateSubstitutionParams == null // kbr
                               ? uriTemplate.CreateLink("next", new { page = Page + 1 })
                               : uriTemplate.CreateLink("next", UriTemplateSubstitutionParams, new { page = Page + 1 }); // page overrides UriTemplateSubstitutionParams
                Links.Add(link);
            }
            Links.Add(new Link("page", uriTemplate.Href));
        }
    }

    public class TestListRepresentation : PagedRepresentationList<TestRepresentation>
    {
        public TestListRepresentation(IList<TestRepresentation> res, int totalResults, int totalPages, int page,
            Link uriTemplate, object uriTemplateSubstitutionParams)
            : base(res, totalResults, totalPages, page, uriTemplate, uriTemplateSubstitutionParams)
        {
        }
    }

    public class SessionRepresentation : Representation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestId { get; set; }

        public bool IsCompleted { get; set; }

        public List<int> NextQuestions { get; set; }

        public override string Rel
        {
            get { return new Link("session", "~/test/{id}").Rel; }
            set { }
        }

        public override string Href
        {
            get { return new Link("session", "~/test/{id}").CreateLink(new { id = Id }).Href; }
            set { }
        }

        protected override void CreateHypermedia()
        {
            if (NextQuestions != null && NextQuestions.Any())
            {
                foreach (var questionId in NextQuestions)
                {
                    Links.Add(new Link("questions", "~/session/{sid}/questions/{qid}").CreateLink(new { sid = Id, qid = questionId }));
                }

                Links.Add(new Link("answer", "~/session/{sid}/answers/{aid}"));
            }
        }
    }
}
