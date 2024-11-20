using DXCatBase.WebApi.API;
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
        var cnt = new EvaluateNewTextController(null);
        var res = cnt.GetAIResults(input);
        //assert
        Assert.AreEqual(10, res.Count);
        Assert.AreEqual(80, res[3].percentage);
    }
}
