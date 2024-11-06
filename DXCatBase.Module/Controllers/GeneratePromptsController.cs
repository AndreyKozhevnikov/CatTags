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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DXCatBase.Module.Controllers {
    public class GeneratePromptsController :ViewController {

        public GeneratePromptsController() {
            var generateFeaturesPrompts = new SimpleAction(this, "GenerateFeaturePrompts", PredefinedCategory.Edit);
            generateFeaturesPrompts.Execute += GenerateFeaturesPrompts_Execute;

            var evaluateTicketText= new SimpleAction(this, "GetTicketPercentage", PredefinedCategory.Edit);
            evaluateTicketText.Execute += CreateMainPrompt_Execute;

        }

        private void CreateMainPrompt_Execute(object sender, SimpleActionExecuteEventArgs e) {
            var os = Application.CreateObjectSpace<Feature>();

            var lst= os.GetObjects<FeaturePrompt>();
            var intro = "You will get several tags. For each tag there is a promt that describes whether a text fits to the tag. Based on this info you need to determine in percentage how well each tag fits to the new text. Prepare answer as a JSON string in the format <tagName,tagId,percentage>. Don't wrap the result in '```json' - '```' strings";
            var tags = lst.Select(x => new { tagName = x.Feature.Name, prompt = x.Prompt, tagId=x.Feature.Oid });
            var tagsJson = Newtonsoft.Json.JsonConvert.SerializeObject(tags);
            var newTicketText = "{\"Subject\":\"Best approach for filtering lookup data based on another property selection in XAF DetailView\",\"Question\":\"Dear DevExpress Support Team,\\nI am currently developing an application using the DevExpress XAF Framework with EF, and I've encountered a challenge that I hope you can help me with. My goal is to dynamically filter the values of a lookup property based on the selection of another property within the same DetailView.\\n\\nTo provide a specific example, I have two domain classes, `Province` and `Municipality`. Each `Municipality` belongs to a `Province`, and in my DetailView for a third domain class, I need to ensure that when a user selects a `Province`, the lookup list for `Municipality` is filtered to only show `Municipality` that are associated with the selected `Province`.\\n\\nHere is a brief overview of my domain classes:\\n\\n```cs\\nusing DevExpress.Persistent.Base;\\nusing DevExpress.Persistent.BaseImpl.EF;\\n\\nnamespace StandaloneWebAPI.Module.BusinessObjects;:\\n\\n[DefaultClassOptions]\\npublic class Person : BaseObject\\n{\\n    public virtual string FirstName { get; set; }\\n    public virtual string LastName { get; set; }\\n    public virtual string Address { get; set; }\\n    public virtual Province  Province { get; set; }\\n    public virtual CAP CAP { get; set; }\\n    public virtual Municipality  Municipality  { get; set; }\\n}\\n```\\n\\nMy challenge is implementing a dynamic filter for the `Municipality` lookup list based on the selected `Province`.\\nI have attempted to follow the general advice found in your documentation and forums, such as using the `DataSourceProperty` attribute and trying to refresh the datasource when the selected `Province` changes, but I haven't achieved the desired behavior.\\n\\nCould you please advise on the best approach or practice for implementing this type of dynamic filtering in XAF? Are there specific methods, events, or patterns within XAF that I should utilize for this scenario?\\n\\nI appreciate any examples or guidance you can provide, including any relevant documentation links or forum threads that I might have missed.\\n\\nThank you for your assistance.\\nBest regards,\\nAlberto Piccioli\"";

            var client = GetClient();
            var result = client.CompleteChat(intro, tagsJson, newTicketText);

            var resultText = result.Value.Content[0].Text;




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
        public static ChatClient GetClient() {
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
