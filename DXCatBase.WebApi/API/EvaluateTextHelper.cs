﻿using DevExpress.XtraPrinting.Native;
using DXCatBase.Module.BusinessObjects;
using Newtonsoft.Json;

namespace DXCatBase.WebApi.API;

public class EvaluateTextHelper {
    public void PopulateParents(List<TagAIResult> tags, Dictionary<string, string> parents) {

        foreach(var tag in tags) {
            tag.parentTagId = parents[tag.tagId];
        }

    }

    public string GetPreparedJSON(string input, Dictionary<string, string> parents) {
        var res = GetAIResults(input);
        PopulateParents(res, parents);
        res = res.OrderByDescending(x => x.percentage).ToList();
        var jsonToSend = JsonConvert.SerializeObject(res);
        return jsonToSend;
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

