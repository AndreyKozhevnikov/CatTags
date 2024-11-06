using Azure.AI.OpenAI;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.CodeParser;
using DevExpress.Data.Filtering;
using DevExpress.DataAccess.Wizard.Native;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DotNetEnv;
using dxTestSolution.Module.BusinessObjects;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace DXCatBase.Module.Controllers {
    public class GeneratePromptsController :ViewController {

        public GeneratePromptsController() {
            var generateFeaturesPrompts = new SimpleAction(this, "GenerateFeaturePrompts", PredefinedCategory.Edit);
            generateFeaturesPrompts.Execute += GenerateFeaturesPrompts_Execute;

        }

        private void GenerateFeaturesPrompts_Execute(object sender, SimpleActionExecuteEventArgs e) {
            var os = Application.CreateObjectSpace<Feature>();
            var features = os.GetObjects<Feature>();

            var client = GetClient();
            //  var result = client.CompleteChat("what is the meaning of the word Rio");
            var message1 = "You will receive several texts with subjects in JSON format. Most of them will fall under a single category. Your task is to create a prompt to evaluate whether future texts also belong to this category. Ensure that your prompt includes keywords to help identify texts within this category. Note that some texts may appear in this category by mistake, so if a text differs significantly from the others, skip it";
            foreach(var feature in features) {
                var data = GetTicketData(feature, os);
                if(data == null) {
                    continue;
                }
                var result = client.CompleteChat(message1, data);
                var promptText = result.Value.Content[0].Text;
                var featurePrompt = os.CreateObject<FeaturePrompt>();
                featurePrompt.Prompt= promptText;
                featurePrompt.Feature= feature;
                os.CommitChanges();
                Thread.Sleep(5000);
            }
        }
        public string GetTicketData(Feature feature, IObjectSpace os) {
            //ConnectionHelper.Connect(DevExpress.Xpo.DB.AutoCreateOption.None);

            //var uow = new UnitOfWork();

            var ticketData = os.GetObjects<TicketData>(CriteriaOperator.FromLambda<TicketData>(x => x.FeatureId == feature.Oid.ToString()));
            if(ticketData.Count == 0) {
                return null;
            }

            var lst = ticketData.Select(x => new { x.Subject, x.Question });

            var lstJson = Newtonsoft.Json.JsonConvert.SerializeObject(lst);

            return lstJson;

        }
        public ChatClient GetClient() {
            Env.Load();

            string keyFromEnvironment = Environment.GetEnvironmentVariable("API_KEY");

            AzureOpenAIClient azureClient = new(
                new Uri("https://dtestopenaiinstance1.openai.azure.com/"),
                new ApiKeyCredential(keyFromEnvironment));

            ChatClient chatClient = azureClient.GetChatClient("GPT4o");

            return chatClient;
        }
    }
}
