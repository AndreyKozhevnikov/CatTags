using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using dxTestSolution.Module.BusinessObjects;

using System.IO;
using System.ComponentModel;
using Newtonsoft.Json;
using DevExpress.ExpressApp.Xpo;
using DXCatBase.Module.BusinessObjects;
using DevExpress.XtraRichEdit.Import.Html;
using System.Xml.Linq;

namespace dxTestSolution.Blazor.Server.Controllers;
//public class CustomBlazorController : ObjectViewController<DetailView,Contact> {


public class CustomController :ViewController {
    public CustomController() {

        //var myActionCSV = new SimpleAction(this, "ImportFeatureViewCSV", PredefinedCategory.Edit);
        //myActionCSV.Execute += MyActionCSV_Execute; ;

        var myAction1 = new SimpleAction(this, "FeatureImportFromTXT", PredefinedCategory.Edit);
        myAction1.Execute += MyImport_Execute;

        var myAction2 = new SimpleAction(this, "MyExport", PredefinedCategory.Edit);
        myAction2.Execute += MyAction2_Execute;


        var removeAction = new SimpleAction(this, "remove features", PredefinedCategory.Edit);
        removeAction.Execute += RemoveAction_Execute;


        var importWithAPIAction = new PopupWindowShowAction(this, "import with API", PredefinedCategory.Edit);
        importWithAPIAction.TargetViewType = ViewType.DetailView;
        importWithAPIAction.TargetObjectType = typeof(Category);
        importWithAPIAction.CustomizePopupWindowParams += ImportWithAPIAction_CustomizePopupWindowParams;
        importWithAPIAction.Execute += ImportWithAPIAction_Execute;
        // var mypopAction1 = new PopupWindowShowAction(this, "MyBlazorPopupAction1", null);
        // mypopAction1.CustomizePopupWindowParams += MyAction1_CustomizePopupWindowParams;

    }

    private void MyActionCSV_Execute(object sender, SimpleActionExecuteEventArgs e) {
        var st = "c:\\temp\\CatDataCSV.csv";

        var os = Application.CreateObjectSpace<Feature>();
        var allFeatures = os.GetObjects<Feature>().ToList();
        using(var reader = new StreamReader(st)) {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            while(!reader.EndOfStream) {
                var line = reader.ReadLine();
                var values = line.Split(';');

                var fName=values[0];
                var fValue=values[1];

                var rFeature=allFeatures.Where(x=>x.Name==fName).FirstOrDefault();
                if(rFeature != null) {
                    rFeature.ForSort=int.Parse(fValue); 
                }

                //listA.Add(values[0]);
                //listB.Add(values[1]);
            }
        }
        os.CommitChanges();
    }

    private void ImportWithAPIAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e) {


        var rawData = ((DataFromApiHolder)e.PopupWindowView.CurrentObject).Data;

        var response = JsonConvert.DeserializeObject<List<FeatureFromAPI>>(rawData);

        var os = Application.CreateObjectSpace<Feature>();
        var parentCategory = os.GetObject((Category)e.CurrentObject);
        foreach(var apiFeature in response) {
            if(apiFeature.text == "General")
                continue;


            Feature feature = os.CreateObject<Feature>();
            feature.Oid = Guid.Parse(apiFeature.id);
            feature.Name = apiFeature.text;
            parentCategory.Features.Add(feature);
        }
        os.CommitChanges();
        //var adResponse = JsonSerializer.de<AnswerDeskResponse>(rawData);
    }

    private void ImportWithAPIAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
        var os = Application.CreateObjectSpace<DataFromApiHolder>();
        var holder = os.CreateObject<DataFromApiHolder>();

        var category = (Category)View.CurrentObject;
        var id = category.Oid;
        var targetURL = "https://int.devexpress.com/supportstat/TicketCategories/CaTGetLinkedFeatures?supportTeam=9cf2ac98-e331-446e-a0c4-c8e22a5cd32a&control=" + id;

        holder.URL = targetURL;
        var detailView = Application.CreateDetailView(os, holder);
        e.View = detailView;
    }



    private void RemoveAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
        var os = Application.CreateObjectSpace(typeof(Feature));
        var category = View.SelectedObjects[0] as Category;
        var ftrs = category.Features.ToList();
        for(int i = ftrs.Count - 1; i >= 0; i--) {
            var f = os.GetObject(ftrs[i]);
            os.Delete(f);
        }
        os.CommitChanges();
        var res = ((XPObjectSpace)os).Session.PurgeDeletedObjects();
        ((XPObjectSpace)os).Session.CommitTransaction();

    }

    private void MyAction2_Execute(object sender, SimpleActionExecuteEventArgs e) {
        var os = Application.CreateObjectSpace(typeof(Feature));
        var lst = os.GetObjects<Feature>().ToList().OrderBy(x => x.ParentCategory.Oid);

        var resSt = new List<string>();
        foreach(var feat in lst) {
            if(feat.Name.ToLower().Contains("obsolete"))
                continue;
            var st = string.Format("['{0}','{1}','{2}']", feat.Oid.ToString(), feat.Name, feat.ParentCategory.Oid);
            resSt.Add(st);
        }
        var resultString = string.Format("[{0}]", string.Join(",", resSt));
    }

    private void MyImport_Execute(object sender, SimpleActionExecuteEventArgs e) {
        var os = Application.CreateObjectSpace(typeof(Category));


        //var st = "c:\\Dropbox\\Programming\\JS\\DXSCScripts\\allFeatures.txt";
        var st = "c:\\Dropbox\\Programming\\C#\\DXCatBase\\ClientScript\\newFeatures.txt";
        string content = File.ReadAllText(st);

        dynamic data = JsonConvert.DeserializeObject(content);



        //HashSet<string> test=new HashSet<string>();
        //test.Add("test1");
        //test.Add("test2");
        //test.Add("test1");

        Dictionary<string, Category> allDict = new Dictionary<string, Category>();

        var existingParents = os.GetObjects<Category>().ToList();
        foreach(var parent in existingParents) {
            allDict.Add(parent.Oid.ToString(), parent);
        }


        foreach(var stub in data) {
            string id = stub.id;
            if(id.StartsWith("00000000")) {
                continue;
            }
            string name = stub.name;
            string parentId = stub.parentId;
            string parentName = stub.parentName;

            Category parentCategory;
            allDict.TryGetValue(parentId, out parentCategory);
            if(parentCategory == null) {
                parentCategory = os.CreateObject<Category>();
                parentCategory.Oid = Guid.Parse(parentId);
                parentCategory.Name = parentName;
                allDict[parentId] = parentCategory;
            } else {
                // var ftrs = parentCategory.Features.ToList();
                // for(int i=ftrs.Count-1; i>=0; i--) { 
                //     var f = ftrs[i];
                //     os.Delete(f);
                // }
                //((XPObjectSpace)os).Session.PurgeDeletedObjects();
                //((XPObjectSpace)os).Session.CommitTransaction();
            }
            bool isFeatureExists = parentCategory.Features.Where(x => x.Oid == Guid.Parse(id)).Any();
            if(isFeatureExists) {
                continue;
            }
            Feature feature = os.CreateObject<Feature>();
            feature.Oid = Guid.Parse(id);
            feature.Name = name;
            parentCategory.Features.Add(feature);

        }


        //var obj = os.CreateObject<Category>();
        //obj.Name = "test";
        //obj.Oid = Guid.Parse("5f35ec54-9dcf-4f9b-b704-df9e21e8c0c7");
        //var obj2=os.CreateObject<Feature>();
        //obj2.Name = "feature1";
        //obj.Features.Add(obj2);
        //   obj.Oid = Guid.Parse("78f98b68-3d61-4516-9549-75d0b40af103");
        os.CommitChanges();
        //var detailView = Application.CreateDetailView(os, obj);
        //var
    }


    // private void MyAction1_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e) {
    // var os = Application.CreateObjectSpace(typeof(Contact));
    // var obj = os.CreateObject<Contact>();
    // var view = Application.CreateDetailView(os, obj);
    // var listView = Application.CreateListView(typeof(Contact), true);
    // e.View = view;
    // }
    protected override void OnActivated() {
        base.OnActivated();
        var cnt = Frame.GetController<NewObjectViewController>();
        if(cnt != null) {

        }
        //View.CustomizeViewItemControl<StringPropertyEditor>(this, SetCalendarView, nameof(Contact.LastName));
    }
    //private void SetCalendarView(StringPropertyEditor propertyEditor) {

    //}
    protected override void OnViewControlsCreated() {
        base.OnViewControlsCreated();

    }
    protected override void OnDeactivated() {
        base.OnDeactivated();
    }
}

