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
        public string Get(string ticketStub) {

            var os = dataService.GetObjectSpace(typeof(TicketData));

            var lst = os.GetObjects<FeaturePrompt>();
            var intro = "You will get several tags. For each tag there is a promt that describes whether a text fits to the tag. Based on this info you need to determine in percentage how well each tag fits to the new text. Prepare answer as a JSON string in the format <tagName,tagId,percentage>. Don't wrap the result in '```json' - '```' strings";
            var tags = lst.Select(x => new { tagName = x.Feature.Name, prompt = x.Prompt, tagId = x.Feature.Oid });
            var tagsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tags);
            var newTicketText = Newtonsoft.Json.JsonConvert.SerializeObject(ticketStub);

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
