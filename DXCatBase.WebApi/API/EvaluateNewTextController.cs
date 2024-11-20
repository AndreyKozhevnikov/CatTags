using DevExpress.ExpressApp.WebApi.Services;
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

        var tempRes = "[{\"tagName\":\"Grid List Editors\",\"tagId\":\"a3b9686a-ddd2-4ee4-9fa9-f3b40e4f00a9\",\"parentTagId\":\"886c0fd5-cf33-4615-bde3-e3ffa112bcbd\",\"percentage\":70},{\"tagName\":\"Custom logic within View or Controller\",\"tagId\":\"7dcd9c5c-28af-42c3-9eb6-68d4f993d7f2\",\"parentTagId\":\"7888da8a-bc3f-4447-bc68-88732ae6f83a\",\"percentage\":40},{\"tagName\":\"Built-in View Items & Editors\",\"tagId\":\"ae449694-f8b6-41a0-bfb5-0bf1733f653e\",\"parentTagId\":\"886c0fd5-cf33-4615-bde3-e3ffa112bcbd\",\"percentage\":25}]";
        return tempRes;



        ////
        var os = dataService.GetObjectSpace(typeof(TicketData));

        var lst = os.GetObjects<FeaturePrompt>();
        var intro = @"You will get several tags. For each tag there is a promt that describes whether a text fits to the tag. 
            Based on this info you need to determine in percentage how well each tag fits to the new text. 
            Prepare answer as a JSON string in the format <""tagName"":tagName,""tagId"":tagId,""percentage"":percentage>. 
            Don't wrap the result in '```json' - '```' strings";
        var tags = lst.Select(x => new { tagName = x.Feature.Name, prompt = x.Prompt, tagId = x.Feature.Oid });

        Dictionary<string, string> parents = new Dictionary<string, string>();
        lst.ToList().ForEach(x => parents[x.Feature.Oid.ToString()] = x.Feature.ParentCategory.Oid.ToString());

        var tagsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tags);
        var newTicketText = Newtonsoft.Json.JsonConvert.SerializeObject(ticketStub);

        var client = GeneratePromptsController.GetClient();
        var result = client.CompleteChat(intro, tagsJson, newTicketText);

        var resultText = result.Value.Content[0].Text;
        var helper = new EvaluateTextHelper();
        var jsonToSend = helper.GetPreparedJSON(resultText,parents);

        return jsonToSend;

    }


}

