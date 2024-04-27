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

namespace dxTestSolution.Blazor.Server.Controllers;
//public class CustomBlazorController : ObjectViewController<DetailView,Contact> {


public class CustomBlazorController :ViewController {
    public CustomBlazorController() {
        var myAction1 = new SimpleAction(this, "MyBlazorAction1", PredefinedCategory.Edit);
        myAction1.Execute += MyAction1_Execute;
        // var mypopAction1 = new PopupWindowShowAction(this, "MyBlazorPopupAction1", null);
        // mypopAction1.CustomizePopupWindowParams += MyAction1_CustomizePopupWindowParams;

    }

    private void MyAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {
        var os = Application.CreateObjectSpace(typeof(Category));


        var st = "c:\\Dropbox\\Programming\\JS\\DXSCScripts\\allFeatures.txt";
        string content = File.ReadAllText(st);

        dynamic data = JsonConvert.DeserializeObject(content);

        

        //HashSet<string> test=new HashSet<string>();
        //test.Add("test1");
        //test.Add("test2");
        //test.Add("test1");

        Dictionary<string,HashSet<string>> allDict=new Dictionary<string,HashSet<string>>();


        foreach(var stub in data) {
            string id = data[0].id;
            string name = data[0].name;
            string parentId = data[0].parentId;
            string parentName = data[0].parentName;
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

