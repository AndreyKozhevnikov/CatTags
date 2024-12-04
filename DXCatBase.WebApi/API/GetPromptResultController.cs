using DevExpress.ExpressApp.WebApi.Services;
using DXCatBase.Module.BusinessObjects;
using dxTestSolution.Module.BusinessObjects;
using Microsoft.AspNetCore.Mvc;

namespace DXCatBase.WebApi.API {
    //[Route("api/[controller]")]
    //[ApiController]
    //public class GetPromptResultController :ControllerBase {
    //    private readonly IDataService dataService;

    //    public GetPromptResultController(IDataService dataService) {
    //        this.dataService = dataService;
    //    }
    //    [HttpPost]
    //    public void Post([FromBody] PromptResultDataStub promptResultStub) {
    //        var os = dataService.GetObjectSpace(typeof(TicketData));

    //        var result = os.CreateObject<PromptResult>();
    //        result.DateRespond = DateTime.Now;
    //        result.Result = promptResultStub.Result;
    //        FeaturePrompt featurePrompt = (FeaturePrompt)os.GetObjectByKey(typeof(FeaturePrompt), promptResultStub.PromptId);
    //        result.Prompt = featurePrompt;

    //        os.CommitChanges();
    //    }
    //}
}
