using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.WebApi.Services;
using dxTestSolution.Module.BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace DXCatBase.WebApi.API;

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





        return null;
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

