using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.WebApi.Services;
using DXCatBase.Module.Controllers;
using dxTestSolution.Module.BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace DXCatBase.WebApi.API {
    [Route("api/[controller]")]
    [ApiController]
    
    public class CustomEnterTicketController :ControllerBase {
        private readonly IDataService dataService;

        public CustomEnterTicketController(IDataService dataService) {
            this.dataService = dataService;
        }


        [HttpPost]
        public void Post([FromBody] TicketDataStub ticketStub) {
            var objectSpace = dataService.GetObjectSpace(typeof(TicketData));

            var existingTicketData = objectSpace.FindObject<TicketData>(CriteriaOperator.FromLambda<TicketData>(x => x.TicketId == ticketStub.TicketId));
            if(existingTicketData != null) {
                return;
            }
            var ticket = objectSpace.CreateObject<TicketData>();
            ticket.Subject = ticketStub.Subject;
            ticket.TicketId = ticketStub.TicketId;
            ticket.FeatureId = ticketStub.FeatureId;
            ticket.Question = ticketStub.Question;

            objectSpace.CommitChanges();

        }

        [HttpGet]
        public string Get([FromBody] TicketDataStub ticketStub) {

            var os = dataService.GetObjectSpace(typeof(TicketData));

            var lst = os.GetObjects<FeaturePrompt>();
            var intro = "You will get several tags. For each tag there is a promt that describes whether a text fits to the tag. Based on this info you need to determine in percentage how well each tag fits to the new text. Prepare answer as a JSON string in the format <tagName,tagId,percentage>. Don't wrap the result in '```json' - '```' strings";
            var tags = lst.Select(x => new { tagName = x.Feature.Name, prompt = x.Prompt, tagId = x.Feature.Oid });
            var tagsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tags);
            var newTicketText = "{\"Subject\":\"Best approach for filtering lookup data based on another property selection in XAF DetailView\",\"Question\":\"Dear DevExpress Support Team,\\nI am currently developing an application using the DevExpress XAF Framework with EF, and I've encountered a challenge that I hope you can help me with. My goal is to dynamically filter the values of a lookup property based on the selection of another property within the same DetailView.\\n\\nTo provide a specific example, I have two domain classes, `Province` and `Municipality`. Each `Municipality` belongs to a `Province`, and in my DetailView for a third domain class, I need to ensure that when a user selects a `Province`, the lookup list for `Municipality` is filtered to only show `Municipality` that are associated with the selected `Province`.\\n\\nHere is a brief overview of my domain classes:\\n\\n```cs\\nusing DevExpress.Persistent.Base;\\nusing DevExpress.Persistent.BaseImpl.EF;\\n\\nnamespace StandaloneWebAPI.Module.BusinessObjects;:\\n\\n[DefaultClassOptions]\\npublic class Person : BaseObject\\n{\\n    public virtual string FirstName { get; set; }\\n    public virtual string LastName { get; set; }\\n    public virtual string Address { get; set; }\\n    public virtual Province  Province { get; set; }\\n    public virtual CAP CAP { get; set; }\\n    public virtual Municipality  Municipality  { get; set; }\\n}\\n```\\n\\nMy challenge is implementing a dynamic filter for the `Municipality` lookup list based on the selected `Province`.\\nI have attempted to follow the general advice found in your documentation and forums, such as using the `DataSourceProperty` attribute and trying to refresh the datasource when the selected `Province` changes, but I haven't achieved the desired behavior.\\n\\nCould you please advise on the best approach or practice for implementing this type of dynamic filtering in XAF? Are there specific methods, events, or patterns within XAF that I should utilize for this scenario?\\n\\nI appreciate any examples or guidance you can provide, including any relevant documentation links or forum threads that I might have missed.\\n\\nThank you for your assistance.\\nBest regards,\\nAlberto Piccioli\"";

            var client = GeneratePromptsController.GetClient();
            var result = client.CompleteChat(intro, tagsJson, newTicketText);

            var resultText = result.Value.Content[0].Text;



            return resultText;
        }

        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

    

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
