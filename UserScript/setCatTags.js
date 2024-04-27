// ==UserScript==
// @name         Set tags
// @namespace    http://tampermonkey.net/
// @version      0.1.1
// @description  Set and search CAT Tags
// @author       AndreyK
// @match        https://isc.devexpress.com/internal/ticket/details/*
// @grant        none
// ==/UserScript==

//when change the validation rules, change the https://gitserver/support/DXNotifications project accordingly

(function() {
  "use strict";

  var cssError = "tmXError";
  var cssToolTipError = "tmXErrorTooltip";
  var prefixes = (function() {
      var platform = ["WinForms", "Web", "Blazor"];
      var modules = [
          "AuditTrail",
          "Chart",
          "CloneObject",
          "ConditionalAppearance",
          "Dashboards",
          "FileAttachments",
          "HtmlPropertyEditor",
          "KPI",
          "Maps",
          "Notifications",
          "Office",
          "PivotChart",
          "PivotGrid",
          "ReportsV2",
          "Scheduler",
          "EasyTest",
          "StateMachine",
          "Security",
          "TreeListEditors",
          "Validation",
          "ViewVariants",
          "Workflow"
      ];
      var r = platform.concat("Core","API", "Web API", modules);
      platform.forEach(function(p) {
          r = r.concat(
              modules.map(function(v) {
                  return v + "." + p;
              })
          );
      });
      return r;
  })();
  let subjectElement=getSubjectElement();
  let platformElement;
  let typeElement;
  let processingTeamElement;
  function isValidSubject() {
      typeElement = getTicketTypeElement();
      platformElement = getPlatformElement();
      processingTeamElement=getProcessingTeamElement();
      if (!isXAF()) return true;
      if (!isBug()) return true;
      var text = getTicketSubject();
      if (isVeraCode(text)) return true;
      var separatorIndex = text.indexOf(" - ");
      if (separatorIndex < 0) return false;
      var prefix = text.substr(0, separatorIndex);
      return prefixes.indexOf(prefix) >= 0;
  }
  function addStyle(css) {
      var head, style;
      head = document.getElementsByTagName("head")[0];
      if (!head) {
          return;
      }
      style = document.createElement("style");
      style.type = "text/css";
      style.innerHTML = css;
      head.appendChild(style);
  }
  function isXAF() {
     let vmSelectedPlatofrm = ko.contextFor(platformElement);
     let selectedPlatforms = vmSelectedPlatofrm.$data.ticketField.selectedValues();
     if(selectedPlatforms.includes('d76afe22-512e-45a3-ad81-7b245352e111:9eda7be2-8f23-467b-bb4e-9d546db79c87')){
      return false; //xpo
     }
     return selectedPlatforms.includes('d76afe22-512e-45a3-ad81-7b245352e111');
  }
  function isBug() {
    let viewModel = ko.contextFor(typeElement);
    let selectedValue = viewModel.$data.ticketProperty.selectedValue()
    return selectedValue == 2;
  }
  function isVeraCode(text){
     return text.startsWith('VERACODE ISSUE:');
   }
  function getTicketSubject(){
    let koSubject = ko.contextFor(subjectElement);
    return koSubject.$data.value();
  }
  var tooltipDiv;
  function validateSubject() {
      if (isValidSubject()) {
        subjectElement.classList.remove(cssError);
        removeToolTip(subjectElement);
      } else {
        subjectElement.classList.add(cssError);
        addToolTip(subjectElement);
      }
  }
  function getSubjectElement(){
    let el = document.getElementsByClassName('subject')
    let mArr = Array.from(el)
    let e2 = mArr.find(x=>x.attributes.getNamedItem('data-bind'))
    return e2;
  }
  function getPlatformElement(){
   return document.getElementById('property-PlatformedProductId');
  }

  function getTicketTypeElement(){
    return document.getElementById('property-EntityType');
  }
  function getProcessingTeamElement(){
    return document.getElementById('property-ProcessingTent');
  }
  function addToolTip(element) {
      if (!tooltipDiv) {
          createToolTip();
      }
      element.appendChild(tooltipDiv);
  }

  function createToolTip() {
      tooltipDiv = document.createElement("div");
      tooltipDiv.classList.add(cssToolTipError);
      var matrix = [];

      let columnCount = 5;
      let rowCount = Math.ceil(prefixes.length / 5);
      prefixes.sort(function(a, b) {
          return ("" + a).localeCompare(b);
      });
      for (var i = 0; i < rowCount; i++) {
          matrix[i] = new Array(columnCount);
      }
      let r = 0;
      let c = 0;
      for (let i = 0; i < prefixes.length; i++) {
          matrix[r][c] = prefixes[i];
          r = r + 1;
          if (r == rowCount) {
              r = 0;
              c = c + 1;
          }
      }
      let table = document.createElement("table");
      for (let i = 0; i < matrix.length; i++) {
          let tr = document.createElement("tr");
          for (let j = 0; j < matrix[i].length; j++) {
              let td = document.createElement("td");
              if (matrix[i][j]) {
                  td.appendChild(document.createTextNode(matrix[i][j]));
              }
              tr.appendChild(td);
          }
          table.appendChild(tr);
      }
      tooltipDiv.appendChild(table);
  }

  function removeToolTip(element) {
      if (tooltipDiv) {
          if (tooltipDiv.parentNode==element){
            element.removeChild(tooltipDiv);
          }
          tooltipDiv = null;
      }
  }

  addStyle("h2." + cssError + ".subject, ." + cssError + " input " + "{ color: red; }");
  addStyle(
      "." +
          cssError +
          " ." +
          cssToolTipError +
          " {visibility: collapse; position: absolute; z-index:10; " +
          "background-color: #f5f5f5; " +
          "border-radius: 4px; box-shadow: 8px 8px 24px #ccc;" +
          "padding-left: 15px; padding-right: 15px; padding-top: 10px; padding-bottom: 10px;" +
          "}"
  );
  addStyle("." + cssError + ":hover ." + cssToolTipError + " {visibility: visible;}");

  function subscribeToSubjectChanges(){
    let subjectVM = ko.contextFor(subjectElement);
    subjectVM.$data.value.subscribe(()=>{validateSubject()});
  }
  function subscribeToPlatformChanges(){
    let platformViewModel = ko.contextFor(platformElement);
    platformViewModel.$data.ticketField.selectedValues.subscribe(()=>{validateSubject()});
  }
  function subscribeToTypeChanges(){
    let viewModel = ko.contextFor(typeElement);
    viewModel.$data.ticketProperty.selectedValue.subscribe(()=>{validateSubject()});
  }
  function subscribeToProcessingTeamChanges(){
    let vmProcessingTeam = ko.contextFor(processingTeamElement);
    vmProcessingTeam.$data.ticketField.selectedValues.subscribe(()=>{validateSubject()});
  }
  validateSubject();
  subscribeToSubjectChanges();
  subscribeToPlatformChanges();
  subscribeToTypeChanges();
  subscribeToProcessingTeamChanges();
})();
