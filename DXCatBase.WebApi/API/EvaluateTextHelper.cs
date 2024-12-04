using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.XtraPrinting.Native;
using DXCatBase.Module.BusinessObjects;
using dxTestSolution.Module.BusinessObjects;
using Newtonsoft.Json;

namespace DXCatBase.WebApi.API;

public class EvaluateTextHelper {
    public void PopulateParents(List<TagAIResult> tags, Dictionary<string, string> parents) {

        foreach(var tag in tags) {
            tag.parentTagId = parents[tag.tagId];
        }

    }

    public TicketData CreateTickedDataFromStub(TicketDataStub ticketStub, IObjectSpace objectSpace) {
        var ticket = objectSpace.FindObject<TicketData>(CriteriaOperator.FromLambda<TicketData>(x => x.TicketId == ticketStub.TicketId));
        if(ticket == null) {
            ticket = objectSpace.CreateObject<TicketData>();
            ticket.Subject = ticketStub.Subject;
            ticket.TicketId = ticketStub.TicketId;
            ticket.Question = ticketStub.Question;
            ticket.EnteredDate = DateTime.Now;
        }
        ticket.FeatureId = ticketStub.FeatureId;
        return ticket;
    }

    //public string GetPreparedJSON(string input, Dictionary<string, string> parents) {
    //    var res = GetAIResults(input);
    //    PopulateParents(res, parents);
    //    res = res.OrderByDescending(x => x.percentage).Take(3).ToList();
    //    var jsonToSend = JsonConvert.SerializeObject(res);
    //    return jsonToSend;
    //}
    public List<TagAIResult> GetTagsToSend(string input, Dictionary<string, string> parents) {
        var res = GetAIResults(input);
        PopulateParents(res, parents);
        res = res.OrderByDescending(x => x.percentage).Take(3).ToList();
        return res;
    }
    public List<TagAIResult> GetAIResults(string input) {
        List<TagAIResult> tagAIList = new List<TagAIResult>();

        try {
            tagAIList = JsonConvert.DeserializeObject<List<TagAIResult>>(input);
        }
        catch(Exception e) {
            var st = string.Format("myError{0}.log", DateTime.Now.ToString("dd-MM-yy_hh:mm"));
            using(StreamWriter outputFile = new StreamWriter(st)) {
                outputFile.WriteLine(e.Message);
                outputFile.WriteLine("=====");
                outputFile.WriteLine(input);

            }
        }
        return tagAIList;

    }



}

