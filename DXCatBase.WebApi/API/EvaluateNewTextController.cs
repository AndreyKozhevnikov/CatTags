using DevExpress.ExpressApp.WebApi.Services;
using DXCatBase.Module.BusinessObjects;
using DXCatBase.Module.Controllers;
using dxTestSolution.Module.BusinessObjects;
using Microsoft.AspNetCore.Mvc;

namespace DXCatBase.WebApi.API;
[Route("api/[controller]")]
[ApiController]

public class EvaluateNewTextController :ControllerBase {
    private readonly IDataService dataService;

    public EvaluateNewTextController(IDataService dataService) {
        this.dataService = dataService;
    }
    [HttpPost]
    public string Post([FromBody] TicketDataStub ticketStub) {
        var os = dataService.GetObjectSpace(typeof(TicketData));

        var lst = os.GetObjects<FeaturePrompt>();
        var intro = @"You will get several tags. For each tag there is a promt that describes whether a text fits to the tag. 
            Based on this info you need to determine in percentage how well each tag fits to the new text. 
            Prepare answer as a JSON string in the format <""tagName"":tagName,""tagId"":tagId,""percentage"":percentage>. 
            Don't wrap the result in '```json' - '```' strings";
        var tags = lst.Select(x => new { tagName = x.Feature.Name, prompt = x.Prompt, tagId = x.Feature.Oid });
        var tagsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tags);
        var newTicketText = Newtonsoft.Json.JsonConvert.SerializeObject(ticketStub);

        var client = GeneratePromptsController.GetClient();
        var result = client.CompleteChat(intro, tagsJson, newTicketText);

        var resultText = result.Value.Content[0].Text;




        return resultText;

    }


    public List<TagAIResult> GetAIResults(string input) {
        return null;
    }
}

