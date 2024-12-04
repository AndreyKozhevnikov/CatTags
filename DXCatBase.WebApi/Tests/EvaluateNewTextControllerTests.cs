using DXCatBase.Module.BusinessObjects;
using DXCatBase.WebApi.API;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DXCatBase.WebApi.Tests;
[TestFixture]
public class EvaluateNewTextControllerTests {
    [Test]
    public void EvaluateNewText() {
        //arrange
        var input = @"[
    {""tagName"":""Custom logic within View or Controller"",""tagId"":""7dcd9c5c-28af-42c3-9eb6-68d4f993d7f2"",""percentage"":60},
    {""tagName"":""TabbedMDI"",""tagId"":""5ee330b7-d935-4105-86fe-87053fe20e1f"",""percentage"":0},
    {""tagName"":""Speed"",""tagId"":""e536ddf2-6f33-4db9-bbd7-f7a345d529f4"",""percentage"":0},
    {""tagName"":""Grid List Editors"",""tagId"":""a3b9686a-ddd2-4ee4-9fa9-f3b40e4f00a9"",""percentage"":80},
    {""tagName"":""Non-Persistent Objects"",""tagId"":""bc65e800-6313-48e2-90cf-107fe3cdf8af"",""percentage"":0},
    {""tagName"":""Custom View Items & Editors"",""tagId"":""63ea8d21-269e-4850-a2a0-97c00f3a7a67"",""percentage"":20},
    {""tagName"":""Lookups"",""tagId"":""a3df8ccc-a6f0-4c3d-94d1-3b4ec2bbe50d"",""percentage"":0},
    {""tagName"":""Inline Edit (Batch, etc.)"",""tagId"":""35acbdb0-470b-4151-adca-0ebec09ea434"",""percentage"":0},
    {""tagName"":""Built-in View Items & Editors"",""tagId"":""ae449694-f8b6-41a0-bfb5-0bf1733f653e"",""percentage"":30},
    {""tagName"":""Built-in Permissions Setup/Troubleshooting"",""tagId"":""e8bb71e2-e5ac-417c-9cc6-3a69f2631eb6"",""percentage"":0}
]";
        //act
        var cnt = new EvaluateTextHelper();
        var res = cnt.GetAIResults(input);
        //assert
        Assert.AreEqual(10, res.Count);
        Assert.AreEqual(80, res[3].percentage);
    }
    [Test]
    public void GetPreparedJSONTest() {
        //arrange
        var input = @"[{""tagName"":""Custom logic within View or Controller"",""tagId"":""222"",""percentage"":60},    {""tagName"":""TabbedMDI"",""tagId"":""333"",""percentage"":70},    {""tagName"":""Speed"",""tagId"":""111"",""percentage"":80}
        ]";

        var parents = new Dictionary<string, string>();
        parents["222"] = "222p";
        parents["111"] = "111p";
        parents["333"] = "333p";
        //act
        var cnt = new EvaluateTextHelper();
        var tagsToSend = cnt.GetTagsToSend(input, parents);

        var res = JsonConvert.SerializeObject(tagsToSend);
        //assert
        var expected = @"[
{""tagName"":""Speed"",""tagId"":""111"",""parentTagId"":""111p"",""percentage"":80},
{""tagName"":""TabbedMDI"",""tagId"":""333"",""parentTagId"":""333p"",""percentage"":70},
{""tagName"":""Custom logic within View or Controller"",""tagId"":""222"",""parentTagId"":""222p"",""percentage"":60}
]";

        Assert.AreEqual(expected.Replace("\r\n",null), res);

    }


    [Test]
    public void GetPreparedJSONTest2() {
        //arrange
        var input = @"[{""tagName"":""Custom logic within View or Controller"",""tagId"":""222"",""percentage"":60},
{""tagName"":""TabbedMDI"",""tagId"":""333"",""percentage"":70},
{""tagName"":""Speed"",""tagId"":""111"",""percentage"":80},
{""tagName"":""SomeSuperTag"",""tagId"":""444"",""percentage"":90}
        ]";

        var parents = new Dictionary<string, string>();
        parents["222"] = "222p";
        parents["111"] = "111p";
        parents["333"] = "333p";
        parents["444"] = "444p";
        //act
        var cnt = new EvaluateTextHelper();
        var tagsToSend = cnt.GetTagsToSend(input, parents);

        var res = JsonConvert.SerializeObject(tagsToSend);
        //assert
        var expected = @"[
{""tagName"":""SomeSuperTag"",""tagId"":""444"",""parentTagId"":""444p"",""percentage"":90},
{""tagName"":""Speed"",""tagId"":""111"",""parentTagId"":""111p"",""percentage"":80},
{""tagName"":""TabbedMDI"",""tagId"":""333"",""parentTagId"":""333p"",""percentage"":70}
]";

        Assert.AreEqual(expected.Replace("\r\n", null), res);

    }

    [Test]
    public void PopulateParents() {
        //arrange
        var tags = new List<TagAIResult>();
        tags.Add(new TagAIResult() { tagName = "testTag", tagId = "123" });
        var parents = new Dictionary<string, string>();
        parents["222"] = "333";
        parents["123"] = "222";
        //act
        var cnt = new EvaluateTextHelper();
        cnt.PopulateParents(tags, parents);
        //assert

        Assert.AreEqual("222", tags[0].parentTagId);


    }
}
