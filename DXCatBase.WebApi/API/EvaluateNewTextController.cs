using DevExpress.ExpressApp.WebApi.Services;
using DXCatBase.Module.BusinessObjects;
using DXCatBase.Module.Controllers;
using dxTestSolution.Module.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        var tempRes = @"[{""tagName"":""Custom logic within View or Controller"",""tagId"":""7dcd9c5c-28af-42c3-9eb6-68d4f993d7f2"",""parentTagId"":""7888da8a-bc3f-4447-bc68-88732ae6f83a"",""percentage"":50},{""tagName"":""TabbedMDI"",""tagId"":""5ee330b7-d935-4105-86fe-87053fe20e1f"",""parentTagId"":""64e07510-1788-4206-b6dc-b5f543a35137"",""percentage"":0},{""tagName"":""Speed"",""tagId"":""e536ddf2-6f33-4db9-bbd7-f7a345d529f4"",""parentTagId"":""9a2987f2-4d67-441a-b932-0731ead5e6dc"",""percentage"":20},{""tagName"":""Grid List Editors"",""tagId"":""a3b9686a-ddd2-4ee4-9fa9-f3b40e4f00a9"",""parentTagId"":""886c0fd5-cf33-4615-bde3-e3ffa112bcbd"",""percentage"":50},{""tagName"":""Non-Persistent Objects"",""tagId"":""bc65e800-6313-48e2-90cf-107fe3cdf8af"",""parentTagId"":""da5d0e9f-4f67-4431-86c2-cfb1b25620b7"",""percentage"":0},{""tagName"":""Custom View Items & Editors"",""tagId"":""63ea8d21-269e-4850-a2a0-97c00f3a7a67"",""parentTagId"":""886c0fd5-cf33-4615-bde3-e3ffa112bcbd"",""percentage"":0},{""tagName"":""Lookups"",""tagId"":""a3df8ccc-a6f0-4c3d-94d1-3b4ec2bbe50d"",""parentTagId"":""886c0fd5-cf33-4615-bde3-e3ffa112bcbd"",""percentage"":0},{""tagName"":""Inline Edit (Batch, etc.)"",""tagId"":""35acbdb0-470b-4151-adca-0ebec09ea434"",""parentTagId"":""886c0fd5-cf33-4615-bde3-e3ffa112bcbd"",""percentage"":0},{""tagName"":""Built-in View Items & Editors"",""tagId"":""ae449694-f8b6-41a0-bfb5-0bf1733f653e"",""parentTagId"":""886c0fd5-cf33-4615-bde3-e3ffa112bcbd"",""percentage"":50},{""tagName"":""Built-in Permissions Setup/Troubleshooting"",""tagId"":""e8bb71e2-e5ac-417c-9cc6-3a69f2631eb6"",""parentTagId"":""cd782041-3d33-4b9f-93c8-290dc9a7980a"",""percentage"":0}]";
        return tempRes;



        ////
        var os = dataService.GetObjectSpace(typeof(TicketData));

        var lst = os.GetObjects<FeaturePrompt>();
        var intro = @"You will get several tags. For each tag there is a promt that describes whether a text fits to the tag. 
            Based on this info you need to determine in percentage how well each tag fits to the new text. 
            Prepare answer as a JSON string in the format <""tagName"":tagName,""tagId"":tagId,""percentage"":percentage>. 
            Don't wrap the result in '```json' - '```' strings";
        var tags = lst.Select(x => new { tagName = x.Feature.Name, prompt = x.Prompt, tagId = x.Feature.Oid });

        Dictionary<string,string> parents=new Dictionary<string, string>();
        lst.ToList().ForEach(x => parents[x.Feature.Oid.ToString()] = x.Feature.ParentCategory.Oid.ToString());

        var tagsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tags);
        var newTicketText = Newtonsoft.Json.JsonConvert.SerializeObject(ticketStub);

        var client = GeneratePromptsController.GetClient();
        var result = client.CompleteChat(intro, tagsJson, newTicketText);

        var resultText = result.Value.Content[0].Text;
        var helper = new EvaluateTextHelper();
        var res = helper.GetAIResults(resultText);
        res = helper.PopulateParents(res,parents);
        var jsonToSend = JsonConvert.SerializeObject(res);


        return jsonToSend;

    }

  
}

public class EvaluateTextHelper {
    public List<TagAIResult> PopulateParents(List<TagAIResult> tags, Dictionary<string, string> parents) {

        foreach(var tag in tags) {
            tag.parentTagId = parents[tag.tagId];
        }
        return tags;
    }

    public List<TagAIResult> GetAIResults(string input) {
        List<TagAIResult> tagAIList = new List<TagAIResult>();

        try {
            tagAIList = JsonConvert.DeserializeObject<List<TagAIResult>>(input);
        }
        catch(Exception e) {
            using(StreamWriter outputFile = new StreamWriter("myerrors.log")) {
                outputFile.WriteLine(e.Message);
                outputFile.WriteLine("=====");
                outputFile.WriteLine(input);

            }
        }
        return tagAIList;

    }

}

