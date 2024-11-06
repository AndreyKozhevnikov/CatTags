
// ==UserScript==
// @name         Set tags
// @namespace    http://tampermonkey.net/
// @version      0.1.7
// @description  Set and search CAT Tags
// @author       AndreyK
// @match        https://isc.devexpress.com/internal/ticket/details/*
// @grant        none
// @downloadURL  https://gitserver/Kozhevnikov.Andrey/DXCatTags/-/raw/master/ClientScript/setCatTags.user.js
// @updateURL  https://gitserver/Kozhevnikov.Andrey/DXCatTags/-/raw/master/ClientScript/setCatTags.user.js
// ==/UserScript==



function createButtons() {

    let lstPopular = [
        ['de23fdf2-0009-4057-92dc-30d5eb9bee19', 'Splash, Loading Panel', '64e07510-1788-4206-b6dc-b5f543a35137'],
        ['8602ce70-4198-4ecc-a808-b3e6c30d3e3e', 'Popups', '886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],
        ['a3df8ccc-a6f0-4c3d-94d1-3b4ec2bbe50d', 'Lookups', '886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],
        ['a3b9686a-ddd2-4ee4-9fa9-f3b40e4f00a9', 'Grid List Editors', '886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],
        ['ae48895d-6e37-43ac-8d0f-17aaa8c4d2d4','EF Core Specifics','da5d0e9f-4f67-4431-86c2-cfb1b25620b7']
    ];
    let tabElement = document.getElementsByClassName('editable-properties')[1];
    lstPopular.forEach(x => {
        let newLi = document.createElement('li');
        let newLink = document.createElement('a');
        newLink.innerHTML = x[1];

        newLink.addEventListener('click', () => {
            SetFeature([x[0], null, x[2]]);
        });

        newLi.appendChild(newLink);
        tabElement.appendChild(newLi);
    });




}



//https://www.w3schools.com/howto/howto_js_filter_lists.asp

function createSearchBox() {
    //console.log('create search');
    let tabElement = document.getElementsByClassName('editable-properties')[1];
    let inpt = document.createElement('input');


    tabElement.appendChild(inpt);
    inpt.id = 'myInput'
    inpt.placeholder = 'search CAT tags';
    inpt.onkeyup = myKeyUp;
    //console.log('search created');


    let divResult = document.createElement('div');
    divResult.id = 'divresult';
    tabElement.appendChild(divResult);


    let newLi=document.createElement('li');
    let newLink=document.createElement('a');
    newLink.innerHTML ='create txt';

    newLink.addEventListener('click', () => {
        createTextFromTicket();
    });

    newLi.appendChild(newLink);
    tabElement.appendChild(newLi);
}

function createTextFromTicket(){
    let platformElement = document.getElementById('property-PlatformedProductId')
    let viewModel = ko.contextFor(platformElement)
    let subject= viewModel.$root.subject.value()

    let question = viewModel.$root.question().description()

    let l2 = document.getElementById('FeatureId')
    let viewModel2 = ko.contextFor(l2)
    let featureId=viewModel2.$data.ticketField.selectedValues()[0]


    let ticketId=viewModel.$root.friendlyId()
    let myRes={Subject:subject,Question:question,FeatureId:featureId,TicketId:ticketId}

    console.log('ready to send');
    fetch("https://localhost:44319/api/odata/TicketData", {
        method: "POST",
        body: JSON.stringify(myRes),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    });

    console.log('sent');
    // console.log(myRes);


}

function getData() {
    // viewModel.$data.ticketField.options.peek()
    let dvCtrls = document.getElementById('controls');
    let selectedComponent = dvCtrls.querySelectorAll('.mdl-color-text--primary')[2];
    let parentName = selectedComponent.innerText;
    let parentId = selectedComponent.parentElement.parentElement.parentElement.attributes['data-item-id'].value;
    let dv = document.getElementById('features');
    let lis = dv.querySelectorAll('.dx-treeview-node-is-leaf');
    let result = [];

    lis.forEach(li => {
        let id = li.attributes['data-item-id'].value;
        let name = li.attributes['aria-label'].value;

        //let obj=[id,name,parentId,parentName];
        let obj = { id: id, name: name, parentId: parentId, parentName: parentName }
        result.push(obj);
        //console.log(id+';'+name+';'+parentId+';'+parentName)
    }
               );
    console.log(result);
}


function myKeyUp() {
    // Declare variables
    let input, filter, divResult;
    input = document.getElementById('myInput');
    filter = input.value.toUpperCase();
    let lst;
    if (filter.length > 1) {
        if (lst == null) {
            lst = getAllFeatures();
        }
        if (divResult == null) {
            divResult = document.getElementById('divresult');
        }
        divResult.innerHTML = "";
        let result = lst.filter(x => x[1].toUpperCase().includes(filter));
        result.forEach(x => {
            console.log(x[1]);
            let newLi = document.createElement('li');
            let newLink = document.createElement('a');
            newLink.innerHTML = x[1];

            newLink.addEventListener('click', () => {
                SetFeature(x);
            });
            newLi.appendChild(newLink);
            divResult.appendChild(newLi);
        }
                      );
    }

    console.log(filter);

    return;
}
function SetFeature(featureData) {
    let l = document.getElementById('ControlId')
    let viewModel = ko.contextFor(l);
    viewModel.$data.ticketField.selectedValues(featureData[2]);


    let l2 = document.getElementById('FeatureId')
    let viewModel2 = ko.contextFor(l2);
    viewModel2.$data.ticketField.selectedValues(featureData[0])

    var bts = document.getElementById('update-actions')
    var vm = ko.contextFor(bts)
    vm.$rawData.saveTicketChangesClick()
}

$(document).ready(function () {

    createButtons();
    createSearchBox();
   // createTextFromTicket();
});
//console.log('test123');


function getAllFeatures() {
    let lst =
        [['a0b6264a-754c-438f-b052-03b2e7e9ca18','File System or custom storages','0169c105-8557-4f94-92fe-fb8501e9db1d'],['57302913-e39d-411b-b574-5788b3bfea32','FileData in Non-XAF apps (CompressionUtils)','0169c105-8557-4f94-92fe-fb8501e9db1d'],['e676badf-fbf6-49b8-8184-5860f301ab02','Controls customization','0169c105-8557-4f94-92fe-fb8501e9db1d'],['5a0e0146-521f-4b27-872c-5a8b6e164780','Collapsible Layout Groups','08fe9cc6-5964-42a0-b574-b0ca64221c15'],['b07d40da-613c-4edb-acad-a3696cad016e','Light Style','08fe9cc6-5964-42a0-b574-b0ca64221c15'],['7218697f-1333-4fac-80c9-e431ece69790',' Async operations','08fe9cc6-5964-42a0-b574-b0ca64221c15'],['76293470-bbb6-4622-aa5b-0d5f9f2351e5','Form Templates','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['b26ec17f-2310-43e7-bbf7-288bed9da819','Inplace Actions in Grid Cells','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['a36c74e8-4fb7-4c2f-8c77-49bf592232bc','Active/Enabled State','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['89f48958-3f24-4f40-ab1e-546694d94dd4','Keyboard shortcuts','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['ab985493-786d-4dd4-9a9e-609b3d57c19f','Action and Container Controls','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['94c9047d-3cd8-4aed-a566-7c890f4590f6','Built-in Action Types','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['6cee7192-985a-441b-9529-a0dc87d8208a','Custom Controllers Design','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['d9f7eaaf-3319-4de0-8366-eab64da364b2','Built-in Controllers (System)','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['edec39b5-1572-4044-a8c1-f98a7ddf0edc','LinkNewObjectToParentImmediately','108d5ce5-79c5-4865-81ee-7d9c77aae1a5'],['96fc4ca5-34ec-4ee4-9dcf-685b0f39bfe0','Audit in Non-XAF Apps','11bee438-6a4c-41c0-acec-6eeb2f17d955'],['3a15c378-4071-4d26-bdfd-6d7d0d765f35','Performance, Purge History, Lightweight mode','11bee438-6a4c-41c0-acec-6eeb2f17d955'],['ea019113-5775-46bc-9498-9d827e994b70','Custom Operations & Other Storage Classes','11bee438-6a4c-41c0-acec-6eeb2f17d955'],['f59491b0-9ccd-4c8a-b7f1-e64800a940c0','Audit History in the UI','11bee438-6a4c-41c0-acec-6eeb2f17d955'],['f7735174-5ad7-4695-bba4-14523082fffa','Custom Filtering','12999f26-366c-45ad-9a61-9e9c2eb6ecec'],['65227ac4-f85a-4f17-bba9-50c50a985e53','Timeout','12999f26-366c-45ad-9a61-9e9c2eb6ecec'],['167e64fe-dde5-4f0b-a821-608e3bc7bc9a','IEvent and IResource implementation','12999f26-366c-45ad-9a61-9e9c2eb6ecec'],['03c6c479-8421-4853-9056-7f3abd0fc2a1','Recurring Events','12999f26-366c-45ad-9a61-9e9c2eb6ecec'],['43057e6e-68e6-41b7-9db5-84b82e29f215','Scheduler control customizations','12999f26-366c-45ad-9a61-9e9c2eb6ecec'],['3f23bf75-0ebd-4abe-869b-62b0cd082fea','PerMonitor Support','13963930-5ab7-4601-8173-680ca43071de'],['370f77b8-bafc-4dab-bb5a-e777b52f91ee','DirectX / Hardware Accelleration Rendering','13963930-5ab7-4601-8173-680ca43071de'],['6e8d7e42-e3ff-41d7-bca6-266fce471282','Missing assemblies, resources','13e97488-2a30-4d9c-8b93-8d6a7fccf5c3'],['8fad2139-f9c0-4c1d-ba5d-77bcad1b5d02','Linux (nginx, Apache, Electron, Docker)','13e97488-2a30-4d9c-8b93-8d6a7fccf5c3'],['8f2de3dd-8605-4069-8302-79869e10c474','ClickOnce','13e97488-2a30-4d9c-8b93-8d6a7fccf5c3'],['93682df5-eb8e-43e6-b374-960c41ef46ec','XAF Application Updater','13e97488-2a30-4d9c-8b93-8d6a7fccf5c3'],['0473741b-54c3-4f94-8f88-96d2ef6833c8','Cloud Deployment (Azure, AWS, etc.)','13e97488-2a30-4d9c-8b93-8d6a7fccf5c3'],['04b8b68c-e1ae-452a-b7b3-a647f7d4871b','ModuleUpdater & DBUpdater','13e97488-2a30-4d9c-8b93-8d6a7fccf5c3'],['f5a662ad-9c94-4410-9296-cb5d80cb765b','Expression Editor','13fd9def-1e2f-4973-b115-a9c03b0c876d'],['663168bf-10db-4eca-a364-1ba54bf9ad35','Non-XAF Data Sources','1581df05-a939-4d29-b98b-64267476d845'],['3038757d-4bc7-4498-be1e-5c06985daad0','FillServices','1581df05-a939-4d29-b98b-64267476d845'],['ab20dc45-b96d-4195-a53e-740c6c37eca6','Control Customization (Widgets, Extensions)','1581df05-a939-4d29-b98b-64267476d845'],['c4dea69b-6cd8-4c24-ba2c-a0d3aa910577','Filtering','1581df05-a939-4d29-b98b-64267476d845'],['8938846d-eaba-45b4-86a8-3476ee0ce8c1','ISupportNotifications implementation','235ad28b-63bd-4528-8554-78b13acdd083'],['5d81aee5-a1fd-46a2-8849-8d30b6b3a8af','Custom Filtering','235ad28b-63bd-4528-8554-78b13acdd083'],['853b2e20-d436-4690-884e-0972e4631c3a','XpoDataSource for ASP.NET WebForms','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['c921d7a7-5ebb-4866-89a3-64bf5156f588','XPObjectSource for Report & Dashboard','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['51b4d58c-aea1-42c3-9d33-66cb384304ad','DataAccessMode=Queryable','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['a1cf830d-01a5-460a-9b3f-7fb09176021d','XPBindingSource','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['68c66ea6-b9df-4064-9481-af4a054be973','DataAccessMode = Server/IF (regular) & Non-XAF','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['6b27c38e-9ed6-4663-b8f3-e6779644d435','DataAccessMode = ServerView/IFView & Non-XAF','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['0ca06186-d15b-416e-aac3-f7fc6c518ecd','DataAccessMode = DataView & Non-XAF XPView','29143705-7c6c-4fae-b8f8-7919df70d0ea'],['62311dff-fb42-4187-b55a-11ce11d0cfd7','Code Customizations','380cc8f4-2b15-478b-acac-181cf9f3a9da'],['29464b32-b43b-44dc-9555-94cf4d39c0c2','Wizard Customizations','380cc8f4-2b15-478b-acac-181cf9f3a9da'],['7477145e-9db6-44d5-acd9-0ff30a7e8383','False-Positive Diagnostic','39c66d91-7d44-4896-95d8-648e58f117e6'],['6da3ff85-0cc6-414e-8793-8013f0908969','Default merging','64c43741-95ad-4d3a-af1a-8848283370f4'],['6fcfc1e8-0c2b-482f-bcea-d26a33af44ec','Track Changes','64c43741-95ad-4d3a-af1a-8848283370f4'],['de23fdf2-0009-4057-92dc-30d5eb9bee19','Splash, Loading Panel','64e07510-1788-4206-b6dc-b5f543a35137'],['adf64e43-19eb-40cc-bd4e-44ecffcba66b','Access Hardware (camera, GPS, barcode scan, etc.)','64e07510-1788-4206-b6dc-b5f543a35137'],['f50d8434-9ec3-459e-bab5-52b7741d1b4e','User-Friendly URL, NavigationManager','64e07510-1788-4206-b6dc-b5f543a35137'],['dbeb9194-252c-40f3-985f-5c3f422b4da3','Context Menu','64e07510-1788-4206-b6dc-b5f543a35137'],['e4ea49e6-c17b-48b6-8721-61968a46d3bf','Themes & Customization (Compact, Bootstrap)','64e07510-1788-4206-b6dc-b5f543a35137'],['0af481a1-f4d4-4342-9164-77fe38917e33','WebAssembly','64e07510-1788-4206-b6dc-b5f543a35137'],['5ee330b7-d935-4105-86fe-87053fe20e1f','TabbedMDI','64e07510-1788-4206-b6dc-b5f543a35137'],['e0e330a7-443e-44b7-88fe-8e2ea7e8c8f9','PWA','64e07510-1788-4206-b6dc-b5f543a35137'],['eb07bc3c-3240-41be-8500-e5261d7af57b','Offline','64e07510-1788-4206-b6dc-b5f543a35137'],['4b1946c9-99d7-4720-aaba-eeba8c829637','Migrate Existing Projects','64e07510-1788-4206-b6dc-b5f543a35137'],['daca1a83-6ae9-44f7-82ff-f3c8964a3473','Horizontal/Vertical Scalling to 100, 1000+ users','64e07510-1788-4206-b6dc-b5f543a35137'],['cd0d6bfb-66f4-47c8-9f1d-83870fb528b9','RuleSet customizations','68800ccb-d256-4ae9-ac69-868cb93d05e9'],['5f2ab9dc-48aa-42b7-9b5f-9f6f3159f9c5','Custom Rule Type Implementation','68800ccb-d256-4ae9-ac69-868cb93d05e9'],['38307e2c-6887-4a19-8a51-c81f714ce95c','Custom error highligthing','68800ccb-d256-4ae9-ac69-868cb93d05e9'],['958b3243-bfa9-49fb-a5df-dbfb46e20d07','Validate persistent objects in Non-XAF Apps','68800ccb-d256-4ae9-ac69-868cb93d05e9'],['581f4a9f-7833-428e-a65f-dc9d830dd595','Built-in Rules Setup/Troubleshooting','68800ccb-d256-4ae9-ac69-868cb93d05e9'],['d73ee94c-e15e-47a1-877d-078da13fc68b','Nuget','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['f723aa09-9fcc-43ea-89b4-0f1f58a4a44b','Dynamically loaded modules & types','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['420c352b-06f8-47c7-8ecd-32151c66732b','Xafari (Galaktika)','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['254c3014-0ae8-4144-bfac-341b77da5970','ValueManager','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['27d42f28-b725-478e-943d-469c5c45adcf','Solution Wizard','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['6ec8df79-5609-4294-b9d8-80ee9697f39c','Application Designer','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['c09929de-5740-4191-8d24-829b70e652bc','Module Designer','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['f402a696-642c-451a-b26b-96f50d103135','LlamachantFramework (Dave)','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['a1d94148-bfaf-4391-979a-aa3700b71e1e','Application Builder','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['a2a33527-7f44-4490-94de-defb7c490977','eXpand (Tolis)','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['add19512-7c5d-4275-a9e6-e263e616b558','Version Control System','69134b3b-3c12-46d1-8c28-d1bc2e06453b'],['655dad44-f3ef-4d78-aa8c-0415955624eb','Error: using CreateObjectSpace()','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['71173067-e9fe-41b7-9964-301de9646288','Error: Thread & Reentrancy (T419520 & K18167)','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['264a3be7-b12a-449b-b623-45d59b4be3d0','Custom logic within a data Model','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['fb1cc054-ff59-4c9f-bbb6-591ab8e162ba','Transactions/Unit of Works','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['7dcd9c5c-28af-42c3-9eb6-68d4f993d7f2','Custom logic within View or Controller','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['15c0a745-83e3-4469-8921-8a03917ed6c3','XAF data and logic in Non-XAF apps','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['8b3d2026-a90a-4180-8f23-9d0c80fb0b4b','Error: Object belongs to a different session/space','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['bbd1888b-aa92-471e-831a-9d3295efeaa8','Async/Await Method Support','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['3b32657c-d8aa-483c-81d7-ef277ecdefdf','Dependency Injection (DI) in Controllers, BO, etc','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['98a82ae3-315f-4528-b244-ff3b9388969b','Error: Object was changed by another user','7888da8a-bc3f-4447-bc68-88732ae6f83a'],['1f70347c-7883-4693-b67b-9761c2395246','Cloner customization','7ed2db87-9dba-4537-8748-e32a268a02bb'],['470d9f60-f41c-4449-b593-f06a2922bd0f','Custom storages','8102dffd-28c0-4791-8195-90a46213145b'],['ef01afe8-302c-436f-a9c8-09395d3b39fb','ImmediatePostData','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['ae449694-f8b6-41a0-bfb5-0bf1733f653e','Built-in View Items & Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['35acbdb0-470b-4151-adca-0ebec09ea434','Inline Edit (Batch, etc.)','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['e72485bb-7aa7-46e7-a2e7-20c2cc62c0c2','DashboardViews','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['a8065283-7474-4176-8007-2bd4688792d5','Grid Print, Export','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['057a89eb-59b8-47ee-b000-3a5521698887','Lazy Loading (DelayedViewItemsInitialization)','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['a3df8ccc-a6f0-4c3d-94d1-3b4ec2bbe50d','Lookups','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['930f8143-794b-4436-93b1-40467d3bf323','Collapsible Groups (IsGroupCollapsed)','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['9d620208-6e17-43a8-b4ac-41050b388efc','Criteria Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['b4c2ef13-482b-4daa-b249-440a7f0802e1','Property Editor Value Formatting','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['3043aed2-55b5-40a4-8372-45281e52fc0e','Creating & Showing Views','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['02ef00bd-f7e9-4a77-ba3c-518fba6dcfe2','Text Nofitications (ShowMessage, Toast)','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['72ed8805-57aa-4876-b613-53c8abbcb43f','Filter Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['78f98b68-3d61-4516-9549-75d0b40af103','Layout Generation & Customization API','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['ad5d5471-063b-4e7e-85e5-7db12480c416','Image Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['63ea8d21-269e-4850-a2a0-97c00f3a7a67','Custom View Items & Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['29434ccb-511a-4bb9-a81d-a17cf950a9d4','Enum Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['8602ce70-4198-4ecc-a808-b3e6c30d3e3e','Popups','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['5f35ec54-9dcf-4f9b-b704-df9e21e8c0c7','Layout End-User Customization','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['a3b9686a-ddd2-4ee4-9fa9-f3b40e4f00a9','Grid List Editors','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['ee24f322-be7b-4f94-abf2-f65800e15769','ListViewAndDetailView (master detail)','886c0fd5-cf33-4615-bde3-e3ffa112bcbd'],['1d6f23c1-b71a-475b-962d-0ace180f967b','IsPostBackRequired','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['26bcccf7-f5d9-49d7-8470-1b90473fb1c1','User-Friendly URLs for Views','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['58a11278-aa4b-40a6-bd96-2a31bf4191b6','Callbacks','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['10b61a3e-4070-4c39-9975-4583779a983b','ASPxGridLookupPropertyEditor','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['cfc61265-e8aa-4c16-9219-6b3bbd8202ff','Horizontal/Vertical Scalling to 100, 1000+ users','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['6da91fed-d139-4090-b11b-78e035509f20','Themes','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['da98afdb-bdd1-4d6f-837f-91f76b875c85','Batch Edit ASPxGridListEditor','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['88a4b0f3-da53-4961-b76e-ed3cbc5daacd','URL/Routing Customization','8bca4929-0c44-4f41-94f3-3ab9b0e7d1bb'],['84e9d3ea-47bf-4807-8f45-2ee5a77475c1','Connection Troubleshooting','990d2015-17da-4d62-9114-9346df8efd13'],['7fd6d6ca-4651-4b00-b2bc-441dba8f0959','Always Encrypted (SQL Server)','990d2015-17da-4d62-9114-9346df8efd13'],['b2e9baf8-cf61-4114-9fbc-5377d17d4d42','Microsoft.Data.SqlClient (T885153)','990d2015-17da-4d62-9114-9346df8efd13'],['d7a71d85-f279-4f36-8244-a0bcd1c00084','Value Converters','990d2015-17da-4d62-9114-9346df8efd13'],['120383b0-e870-4550-bd26-c1d6723085ae','Custom Provider Implementation','990d2015-17da-4d62-9114-9346df8efd13'],['72d5f6c2-4abe-431c-bcd2-e99e604fec17','Table & Column Mapping Customizations','990d2015-17da-4d62-9114-9346df8efd13'],['bbb585dc-26c5-4bf3-9b00-ebfe23defbdb','Typed Parameters BC (T889138 & T870008)','990d2015-17da-4d62-9114-9346df8efd13'],['d45f9784-4fc0-4156-825b-7af54bd4c89f','Memory Consumption','9a2987f2-4d67-441a-b932-0731ead5e6dc'],['e536ddf2-6f33-4db9-bbd7-f7a345d529f4','Speed','9a2987f2-4d67-441a-b932-0731ead5e6dc'],['34d69dd3-0a5f-44b8-a91f-5586b03cbc39','Custom Image Source','9c48de6f-ceca-47b9-a11d-9aaa220568db'],['c21c69e3-9f29-4c81-8989-e17749d269a1','Missing images (ShowXXX options & issues)','9c48de6f-ceca-47b9-a11d-9aaa220568db'],['fd1114ac-f25c-486e-a104-eb9907a30820','ImageLoader customizations','9c48de6f-ceca-47b9-a11d-9aaa220568db'],['40acf0dd-c018-4d63-a79b-fa7d50dd3ea3','SVG','9c48de6f-ceca-47b9-a11d-9aaa220568db'],['9ccd49d4-9533-43d4-9bb9-5cb741cb0463','Custom Enable/Hide states','b0130ff9-0603-4532-a09e-256a849df919'],['e70a7bda-4be2-4355-9113-93b8abb39dab','Roadmap & Future','b8cce36a-daef-468d-9447-5bcf8c09fa73'],['7d41d8d4-2984-4d6f-890f-9e1e943dda70','GitHub Code Examples','b8cce36a-daef-468d-9447-5bcf8c09fa73'],['5bfc44ad-ddf7-40fd-8caf-b68f95a6bbf0','Training & Consulting','b8cce36a-daef-468d-9447-5bcf8c09fa73'],['46e755db-9b9c-4592-a6fa-bc7078d0adf6','Demos','b8cce36a-daef-468d-9447-5bcf8c09fa73'],['9f624966-f356-451e-bb0a-cee77b76e496','Videos','b8cce36a-daef-468d-9447-5bcf8c09fa73'],['44e78bb7-33d7-4946-a58f-6da3e07986b4','.NET Standard 2.0','c18f51a3-b472-458f-a241-d6d0e3fa42b8'],['9df4e63b-d914-4d32-9f6c-c91d264f35d9','.NET Core 3.0','c18f51a3-b472-458f-a241-d6d0e3fa42b8'],['797a339f-5d2e-4dae-a787-e4a8aefbb0f5','Upgrades, breaking changes','c18f51a3-b472-458f-a241-d6d0e3fa42b8'],['2c5bed2b-1c7f-4abc-9b81-f467a0261b4c','Xamarin/Mono','c18f51a3-b472-458f-a241-d6d0e3fa42b8'],['c7ccbbe7-08e8-4b93-a67d-12815d0bf522','Obtain captions in Non-XAF apps','c8213d32-ad5d-417b-afac-4b2662ab2252'],['16cfbb72-69ab-4b54-b8ad-8a2f08fcf85d','Custom Localization (e.g. in code)','c8213d32-ad5d-417b-afac-4b2662ab2252'],['9e64dfb5-76e7-4b0a-b021-a7ff270bdee7','Template Localization','c8213d32-ad5d-417b-afac-4b2662ab2252'],['8976a5e2-0013-480e-b2a9-c8a246a35fe2','Right To Left (RTL)','c8213d32-ad5d-417b-afac-4b2662ab2252'],['c5024e7d-eb78-4eb9-ae61-ce638112d2bd','Accessibility (WPAT, Section 508)','c8213d32-ad5d-417b-afac-4b2662ab2252'],['14458e32-a8e5-4fa6-8446-cfd191103208','Model Editor','c8213d32-ad5d-417b-afac-4b2662ab2252'],['1fc7d93b-c09b-47a7-8810-fcede8d73177','Localization Service','c8213d32-ad5d-417b-afac-4b2662ab2252'],['1b2e1ce6-4822-471e-8c08-4014d5312fcc','Connect several DB at once','c8b74c8b-464d-48d2-9c00-c9a2c9c8b900'],['7f5dfdd5-cf2b-4969-9702-5ed96e122a08','XPO WCF Data Store (regular and cache)','c8b74c8b-464d-48d2-9c00-c9a2c9c8b900'],['87a5728f-1fb1-4671-adc7-d3ade967b04b','ThreadSafeDataLayer','c8b74c8b-464d-48d2-9c00-c9a2c9c8b900'],['3570184d-2d1a-4b9c-93a4-d56f1a586dbd','XPO OData, Web API Services, JSON Serialization','c8b74c8b-464d-48d2-9c00-c9a2c9c8b900'],['48a28633-9df8-4e90-a6a7-dde7a596e7f1','Obtaining data from external services, other DB','c8b74c8b-464d-48d2-9c00-c9a2c9c8b900'],['1d125455-ab3c-400f-86c8-e913e83c9732','XPO Web API Data Store (+cache) for .NET 5','c8b74c8b-464d-48d2-9c00-c9a2c9c8b900'],['e8bb71e2-e5ac-417c-9cc6-3a69f2631eb6','Built-in Permissions Setup/Troubleshooting','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['7d291bb9-c1e8-401e-9232-4ea0d4cb30f0','Security in Non-XAF Apps','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['72145d5b-73db-4a8f-96fd-5029639e7872','Action Permissions','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['e2b4bb4d-b5ef-4bf9-8d46-736c3a95042a','Custom Permissions Implementation','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['d2a919bf-1dd2-41bb-abbf-7a58a498484b','Multiple databases','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['32a7e25e-822f-4381-8998-87ed5651bf8e','OAuth Identity Providers','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['4b6a8675-4d5a-42c0-95dc-8926493a5672','Middle-Tier Application Server','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['5f6b910a-bfb2-41af-b94e-9e34463039c0','Administrative UI','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['e4a92cab-7aad-43c7-a2f2-b8ed4896439b','Custom User & Role Classes','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['dd7361e5-289c-499e-aa7f-e4b772759733','Custom Authentication/Logon Parameters Form','cd782041-3d33-4b9f-93c8-290dc9a7980a'],['381b93d3-4451-4e9d-b8a5-8d1a7c9b55b0','Custom UI elements','d04bab85-5d67-4f63-8df4-d37c74df58f2'],['a0cc73f7-4015-434c-807f-d2ff016c6fa7','Skin-aware colors','d04bab85-5d67-4f63-8df4-d37c74df58f2'],['c128d6e8-e1af-45e2-b862-fd80d80ae1c1','Custom appearance on events','d04bab85-5d67-4f63-8df4-d37c74df58f2'],['84ce346c-6123-47b9-b70e-40202a50aa74','Model Editor configuration','d0abb96b-c72a-4d0d-8c58-a5f6fd36acd4'],['9e6d0a99-54f5-470f-9e2d-4a8859a0e47a','Navigation control customizations','d0abb96b-c72a-4d0d-8c58-a5f6fd36acd4'],['82b31121-d4b7-4039-b2f8-4b9a54c068cd','Accordion Control','d0abb96b-c72a-4d0d-8c58-a5f6fd36acd4'],['e03f2884-362a-4680-9a97-b559453a2da0','ShowNavigationItemController customizations','d0abb96b-c72a-4d0d-8c58-a5f6fd36acd4'],['ad74d393-6c39-4d09-86ec-04e3ff02490d','Inplace reports','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['1edab979-1e6e-43f9-a6be-0de1d456506c','Reports in Non-XAF Apps','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['3ecde012-6db1-4def-a86f-168b6458ed76','XtraReport parameters','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['4537e757-7009-497a-9fac-444d469446a9','Show Preview, Print, Export in code','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['5d3d3448-dc9e-40af-a29b-4791987fe523','Predefined reports','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['9c15afb1-74b3-4477-a471-87e069c25203','Report control customizations','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['4695c833-c007-4715-ab8c-a99cc954ffc3','XAF Report Object parameters','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['22caa727-448a-4d07-922f-adb79210d83b','Bind to Custom Data Sources','d22c9db4-62fa-4439-90ed-374cdf9e483a'],['5b6bca7a-e21f-4faf-b579-0da00398bee9','XAF | Current Object Parameter for XPO','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['b2ceaa95-1515-459d-9587-35ef96af9cb3','LINQ to XPO','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['cc7ecf3c-58f5-4473-93fc-4152597490f4','Built-in Criteria Operators','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['1443fa23-9dd5-4868-a651-5a1cb1912404','XAF | Lookup/Collection Property Filtering','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['1959e13d-4f1c-482a-9fb4-5aaa45105039','Direct SQL','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['237f6b68-a704-4dee-aede-68d08e1f9d46','Custom Criteria Functions','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['a97a3ff9-e86e-4690-9c17-c12e11a8eff8','Filter by Security User','d97b6e1f-1f48-4496-a63e-60b5428785f3'],['bc65e800-6313-48e2-90cf-107fe3cdf8af','Non-Persistent Objects','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['ae48895d-6e37-43ac-8d0f-17aaa8c4d2d4','EF Core Specifics','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['df330128-834f-44aa-bd02-1d20f2e3bc0c','Legacy DB Mapping (SQL View, composite key)','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['5578803d-b03d-45b4-b4e3-66398f391fb1','Property Changed Event','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['6512e7fd-9ada-4cf7-89a1-6a25a76edaa9','Sequential/Auto-increment numbers (T567184)','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['4c925dff-5491-4686-9f60-7771b06b2595','Aliased, Calculated and Custom Fields/Metadata','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['ec311c45-04e2-42fd-b7ce-787be69b0ed5','ORM Data Model Wizard/Designer','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['138836ce-fef9-411c-9950-8f624bef91d7','Custom implementations of default XAF BCL entities','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['3fcbab9e-64d1-4a94-932f-98a849d663e0','Issues with non-unuque key values (T639653)','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['d8a82bd6-68c7-4e07-9261-9ac0102f2fb2','(Deprecated) Domain Components - DC','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['a367f423-a72d-4f7d-88d8-a4d24b8ee118','Relationships/Associations','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['f6726544-dbfd-4d2a-9e62-c6b6d7aa4d5b','XPO IList Associations (+ManyToManyAliasAttribute)','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['f8ba43d4-dc5b-4487-87e7-ca77a0675378','(Deprecated) Entity Framework 6 - EF6','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['c2fe0dac-4805-45a7-a64c-d58e5e4e711a','ORM Database Schema Migrations','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['01a628f9-a0f5-4c00-b521-eb79ef474cdc','DateTime properties (UTC, conversion)','da5d0e9f-4f67-4431-86c2-cfb1b25620b7'],['854074ca-41d0-4d53-885f-645bff51ab29','Custom Activities Implementation','e0f70f19-9a88-4069-b4dd-80d93699a4d9'],['4dc45a62-6943-4b5c-9924-d1c2590cff67','Service start troubleshooting','e0f70f19-9a88-4069-b4dd-80d93699a4d9'],['ef2af076-3687-43a2-91f9-d98c23ee3226','Rehosted Runtime Workflow Designer','e0f70f19-9a88-4069-b4dd-80d93699a4d9'],['961eedad-da64-4824-a52a-a3c639fab837','Custom Filtering','e2d0fa52-f8ba-4bef-98bf-ebddf83b8870'],['7a8e4dc9-f8aa-4d9b-bc5a-f1716f73b05c','Bind to Custom Data Sources','e2d0fa52-f8ba-4bef-98bf-ebddf83b8870'],['6a98db2c-d918-4570-9f72-8ffbfe287cde','Rich Text Editor','e4e97d93-0576-4cf3-ac53-793044f0357a'],['0dcd5e80-a128-4d84-8b0b-96417ef769f5','Spreadsheet','e4e97d93-0576-4cf3-ac53-793044f0357a'],['c13f5fac-86f0-45f9-beae-aea3a13ae18e','PDF viewer','e4e97d93-0576-4cf3-ac53-793044f0357a'],['e8dba575-4ff3-4dc5-9ac6-4fc1920586be','LinqPad Driver','e50d3a48-582f-424a-aba5-814f6531f985'],['40568e3d-9509-4020-9673-53aa55ec67a9','XPO Profiler','e50d3a48-582f-424a-aba5-814f6531f985'],['11e4a994-08f4-4908-a3f6-98e71ecb8d78','Unit Testing','e50d3a48-582f-424a-aba5-814f6531f985'],['f550a5f9-5246-435f-a641-e19b66550328','EasyTest/Script Recorder Module','e50d3a48-582f-424a-aba5-814f6531f985'],['75d0bc38-d61f-4f9b-8d56-e8bafc354c66','Tracer','e50d3a48-582f-424a-aba5-814f6531f985'],['3edc2a2b-7fe3-469c-bceb-02af4716040a','Switch Tenants inside App (After Login)','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['fe65e9df-6e6b-4ef8-82e0-334028ee4ae0','Custom Tenant and other classes','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['14ff7dce-44f3-4e28-9862-4ba964e053c9','Custom Tenant Resolver (URL, ID, etc)','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['24be64fb-0450-4b00-8904-6a38aa0b00e9','Licensing (CAL, Compliance, limitations)','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['1483e0c6-6df2-4e88-8c30-73f2c45c2f46','Database (creation, share storage, etc.)','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['630ad0a7-5081-44a5-a252-8da2f714e47a','Select Tenants on Login','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['871bdbde-a2b4-45a5-bd00-8ee894158131','Limit Tenant Features (SKU, modules, user count)','eb23c4bb-7377-4921-9824-2c56bcde14a7'],['63fbddaa-1e2d-46ce-9210-9120968f82ce','Map control customizations','ee5c6665-015f-4b07-8215-2ef7fddd35ad'],['87f4ae61-04d4-412f-a55a-af1a7a26e7b6','ITreeNode implementation','ef10b6d8-628d-46a6-a195-a9ebecf5f0f6'],['08285b0d-41e2-4974-89f9-b7f55037fbf8','Inplace Editing','ef10b6d8-628d-46a6-a195-a9ebecf5f0f6'],['2700db41-f6a1-4955-88f6-d8507cfc3a1d','Custom Filtering','ef10b6d8-628d-46a6-a195-a9ebecf5f0f6'],['ee84add1-87b1-45c7-957b-db5ccac3dda6','Wizard Customizations','f5286067-0edd-4676-af3a-3ac4969e0a0c'],['e8861dab-c169-4254-827a-ef9d24f420ce','Code Customizations','f5286067-0edd-4676-af3a-3ac4969e0a0c'],['caeb4185-d113-4b4c-97fa-1af87af0ea25','Model Editor for .NET 6','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3'],['5a93e8a0-c951-4dd4-a62f-5857b1266156','Model Editor for .NET Framework','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3'],['0d5b3bbd-ecfa-47cb-8771-5c4747cb3058','Application Model in Non-XAF Apps','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3'],['0e13b0a9-40e3-43dd-9c8b-7c2a65b46cb6','Default & Custom Attributes To Customize Model','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3'],['3de49a6f-7116-454b-94ea-969586da26c4','Read, Customize or Extend the Application Model','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3'],['da4916e0-76e9-4009-ad24-a6f135a22c14','Cache (ModelAssembly, EnableModelCache, etc.)','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3'],['486ddb44-b4c8-443a-a24f-b3d7d6333618','Model Difference Storages','f5e7ae96-5e0d-48f0-aed0-ea344e07ebc3']]
    ;
    return lst;
}