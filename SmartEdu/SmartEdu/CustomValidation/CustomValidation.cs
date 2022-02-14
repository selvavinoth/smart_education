using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Xml.Linq;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;
using SmartEdu.SessionHolder;
using SmartEdu.Bll.Services;
using SmartEdu.Data.infrastructure;
using SmartEdu.Helper;

namespace SmartEdu.CustomValidation
{
    public class ValidationField
    {
        public bool IsMandatory { get; set; }
        public string DisplayName { get; set; }
        public string AllowWhiteSpace { get; set; }
        public bool HasMinLength { get; set; }
        public bool HasMaxLength { get; set; }
        public bool IsIntegr { get; set; }
        public bool IsAmount { get; set; }
        public bool BlockSpecialCharacter { get; set; }
        public bool BlockWhiteSpace { get; set; }
        public bool NeedDateValidation { get; set; }
        public bool NeedRangeValidation { get; set; }
        public bool NeedDependentFieldValidation { get; set; }
        public bool NeedEmailValidation { get; set; }
        public bool NeedPhoneNumberValidation { get; set; }
        public bool IsAlphaNumeric { get; set; }

        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public string Range { get; set; }
        public string DateDependence { get; set; }
        public string Type { get; set; }
        public string RegExp { get; set; }
        public string IsSelectionField { get; set; }
    }

    public class CustomValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private string ViewModelName = "";
        private bool NeedToGetFilterList = false;
        private string FieldString = "";
        private readonly IFieldLevelValidationConfigRepository fieldLevelValidationConfigRepository;

        private string FeildValidator
        {
            get
            {
                CONFIG_FIELDLEVEL_VALIDATION model = null;
                try {
                    if (SessionPersistor.ViewModelName == null || SessionPersistor.ViewModelName.Trim() == "" || SessionPersistor.ViewModelName != this.ViewModelName)
                    {
                        SessionPersistor.ViewModelName = this.ViewModelName;
                        this.NeedToGetFilterList = true;
                        model = GetFieldConfig(this.ViewModelName);
                        if (model == null) { SessionPersistor.FieldLevelConfig = null; return null; }
                        this.FieldString = model.CFV_ValidationString;
                        return model.CFV_ValidationString;
                    }
                    return null;
                }
                catch (Exception exe) { return null; }
                finally { model = null; }
            }
        }
        private void FillFieldValidationList()
        {
            try
            {
                if (this.NeedToGetFilterList)
                {
                    SetValidationField();
                    SessionPersistor.FieldLevelConfig = this.FieldList;
                }
            }
            finally { this.FieldList = null; }
        }
        public CustomValidationAttribute() {

            try { 
                this.fieldLevelValidationConfigRepository=new FieldLevelValidationConfigRepository(new DataBaseFactory());
            }
            finally { }
        }

        public CustomValidationAttribute(IFieldLevelValidationConfigRepository fieldLevelValidationConfigRepository)
        {
            this.fieldLevelValidationConfigRepository = fieldLevelValidationConfigRepository;
        }
        private enum ControlType
        {
            EMAIL,
            PHONENUMBER,
            DATE,
            ALPHANUMERIC,
            INTEGER,
            AMOUNT,
        }
        private List<ModelClientValidationRule> rules = null;

        // Server Side Validation
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) { return ValidationResult.Success; }

        // Client Side Validation
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            rules = new List<ModelClientValidationRule>();
            // string path = GetPath(metadata.ContainerType.Name); // get tenant and bu name from session
            try
            {
                this.ViewModelName = metadata.ContainerType.Name;
                if (metadata.PropertyName.ToUpper() == "RELEASEMEMORY") {
                    SessionPersistor.ViewModelName = null;
                    SessionPersistor.FieldLevelConfig = null;
                    return rules;
                }
                ValidationField validationField = null;
                if (this.FeildValidator == null) { validationField = null; }
                validationField = GetValidationFieldData(metadata.PropertyName);
                if (validationField != null)
                {
                    metadata.DisplayName = validationField.DisplayName;
                    if (validationField.IsMandatory) { rules.Add(new ModelClientValidationRequiredRule(ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource(metadata.DisplayName))); }
                    if (validationField.HasMinLength) { MinLengthValidation(validationField.MinLength); }
                    if (validationField.HasMaxLength) { MaxLengthValidation(validationField.MaxLength); }
                    if (validationField.BlockWhiteSpace) { ValidateWhiteSpace(); }
                    if (validationField.BlockSpecialCharacter) { SpecialCharacterValidation(); }
                    if (validationField.IsIntegr) { rules.Add(new ModelClientValidationRegexRule(ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("CMN_Valid") + " " + ResourceKeeper.GetResource("CMN_Integer"), @"^[0-9]+$")); }
                    if (validationField.IsAmount) { rules.Add(new ModelClientValidationRegexRule(ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("CMN_Valid") + " " + ResourceKeeper.GetResource("CMN_Amout"), @"^\d+(\.\d{1,2})?$")); }
                    if (validationField.NeedEmailValidation) { rules.Add(new ModelClientValidationRegexRule(ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("CMN_Valid") + " " + ResourceKeeper.GetResource("CMN_Email"), @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")); }
                    if (validationField.NeedPhoneNumberValidation) { rules.Add(new ModelClientValidationRegexRule(ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("CMN_Valid") + " " + ResourceKeeper.GetResource("CMN_PhoneNumber"), @"^[0-9-+ ]+$")); }
                    if (validationField.NeedDateValidation) { }
                    if (validationField.NeedDependentFieldValidation) { }
                    if (validationField.NeedRangeValidation) { }
                    //AlphaNumericValidation();
                }
                return rules;
            }
            finally
            {
                rules = null;
                //path = string.Empty;
            }

        }
        private void SetValidationByType(ValidationField validationField, string displayName)
        {
            try
            {
                if (validationField.Type != null && validationField.Type != "")
                {
                    if (validationField.Type == ControlType.EMAIL.ToString())
                        rules.Add(new ModelClientValidationRegexRule("Please enter valid email.", @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"));
                    else if (validationField.Type != null && validationField.Type == ControlType.PHONENUMBER.ToString())
                        rules.Add(new ModelClientValidationRegexRule("Please enter valid " + displayName + ".", @"^[0-9-+ ]+$"));
                    else if (validationField.Type != null && validationField.Type == ControlType.ALPHANUMERIC.ToString())
                        rules.Add(new ModelClientValidationRegexRule("Please enter valid " + displayName + ".", @"^[0-9a-zA-Z]+$"));
                    else if (validationField.Type != null && validationField.Type == ControlType.INTEGER.ToString())
                        rules.Add(new ModelClientValidationRegexRule("Please enter valid " + displayName + ".", @"^[0-9]+$"));
                    else if (validationField.Type != null && validationField.Type == ControlType.AMOUNT.ToString())
                        rules.Add(new ModelClientValidationRegexRule("Please enter valid " + displayName + ".", @"^\d+(\.\d{1,2})?$"));
                }
                else if (validationField.RegExp != null && validationField.RegExp != "")
                {
                    rules.Add(new ModelClientValidationRegexRule("Please enter valid " + displayName + ". ", validationField.RegExp));
                }
                else
                {
                    rules.Add(new ModelClientValidationRegexRule("Please enter valid " + displayName + ".", "^((?!@@)(?!')(?!\")(?!>)(?!<)(?!!).)*$"));
                }
            }
            finally { }
        }
        public string GetPath(string viewModel)
        {
            return System.Web.HttpContext.Current.Server.MapPath(@"~/CustomValidtionSchema" + "/" + viewModel + ".xml");
        }
        public ValidationField GetXmlForm(string xmlForm, string fieldName)
        {
            XDocument xdoc = null;
            ValidationField xForms = null;
            try
            {

                xdoc = XDocument.Load(xmlForm);
                xForms = (from element in xdoc.Descendants("FIELD")
                          where (element.Attribute("NAME") != null ? element.Attribute("NAME").Value : "") == fieldName
                          select new ValidationField
                          {
                             // IsMandatory = (element.Attribute("ISREQUIRED") == null ? "" : (string)element.Attribute("ISREQUIRED").Value),
                              MaxLength = (element.Attribute("MAXLENGTH") == null ? (element.Attribute("LENGTH") == null ? 0 : Convert.ToInt32(element.Attribute("LENGTH").Value)) : Convert.ToInt32(element.Attribute("MAXLENGTH").Value)),
                              DisplayName = (element.Attribute("DISPLAYNAME") == null ? "" : (string)element.Attribute("DISPLAYNAME").Value),
                              MinLength = (element.Attribute("MINLENGTH") == null ? 0 : Convert.ToInt32(element.Attribute("MINLENGTH").Value)),
                              Type = (element.Attribute("TYPE") == null ? "" : (string)element.Attribute("TYPE").Value),
                              RegExp = (element.Attribute("REGEXP") == null ? "" : (string)element.Attribute("REGEXP").Value),
                          }).SingleOrDefault();
                return xForms;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                xdoc = null;
                xForms = null;
            }
        }
        public ValidationField GetValidationFieldData(string fieldName)
        {
            ValidationField model = new ValidationField();
            Dictionary<string, string> vList = null;
            Dictionary<string, Dictionary<string, string>> FieldList = null;
            try
            {
                FillFieldValidationList();
                FieldList=SessionPersistor.FieldLevelConfig;
                if (FieldList == null || FieldList.Count == 0) { return new ValidationField(); }
                if (!FieldList.Keys.Contains(fieldName)) { return new ValidationField(); }
                vList = FieldList[fieldName];
                if (vList == null || vList.Count == 0) { return new ValidationField(); }
                foreach (var element in vList)
                {
                    switch (element.Key.ToUpper())
                    {
                        case "ISMANDATORY":
                            model.IsMandatory = true;
                            break;
                        case "MAXLENGTH":
                            model.MaxLength = Convert.ToInt32(element.Value);
                            model.HasMaxLength = true;
                            break;
                        case "MINLENGTH":
                            model.MinLength = Convert.ToInt32(element.Value);
                            model.HasMinLength = true;
                            break;
                        case "NEEDEMAILVALIDATION":
                            model.NeedEmailValidation = true;
                            break;
                        case "ISINTEGER":
                            model.IsIntegr = true;
                            break;
                        case "ISAMOUNT":
                            model.IsAmount = true;
                            break;
                        case "ISALPHANUMERIC":
                            model.IsAlphaNumeric = true;
                            break;
                        case "ALLOWWHITESPACE":
                            model.BlockWhiteSpace = true;
                            break;
                        case "ALLOWSPECIALCHARACTER":
                            model.BlockSpecialCharacter = true;
                            break;
                        case "DATEFORMAT":
                            model.NeedDateValidation = true;
                            model.DateDependence = element.Value;
                            break;
                        case "RANGE":
                            model.NeedRangeValidation = true;
                            model.Range = element.Value;
                            break;
                        case "DISPLAYNAME":
                            model.DisplayName = element.Value;
                            break;
                        default:
                            break;
                    }
                }
                return model;
                //xForms = (from element in FieldList
                //          where element.Key==fieldName
                //          select new ValidationField
                //          {
                //              IsRequired = (element.Value=="ISREQUIRED" ? element.Value:""),
                //              MaxLength = (element.Key == "MAXLENGTH" ? (element.Value == null ? 0 : Convert.ToInt32(element.Value)) : Convert.ToInt32(element.Value)),
                //              DisplayName = (element.Key=="DISPLAYNAME" ? "" : element.Value),
                //              MinLength = (element.Key == "MINLENGTH" ? (element.Value == null ? 0 : Convert.ToInt32(element.Value)) : Convert.ToInt32(element.Value)),
                //              Type = (element.Key=="TYPE"? element.Value:""),
                //              RegExp = (element.Key=="REGEXP" ?element.Value:""),
                //          }).SingleOrDefault();
                //return xForms;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //xForms = null;
            }
        }

        private Dictionary<string, Dictionary<string, string>> FieldList = null;
        public void SetValidationField()
        {
            FieldList = new Dictionary<string, Dictionary<string, string>>();
            List<string> tempList = new List<string>();
            string[] type = new string[2];
            try
            {
                if (this.FieldString == null || this.FieldString.Trim() == "") { return; }
                tempList = this.FieldString.Split('/').ToList();
                if (tempList.Count == 0) { return; }
                foreach (string s in tempList)
                {
                    type = s.Split('?');
                    if (type != null)
                    {
                        FillFieldSummary(type[0], type[1]);
                    }
                }
            }
            finally { tempList = null; type = null; }
        }
        private void FillFieldSummary(string fieldName, string data)
        {
            Dictionary<string, string> summary = new Dictionary<string, string>();
            List<string> tempList = new List<string>();
            string[] type = new string[2];
            try
            {
                if (data == null || data.Trim() == "") { return; }
                tempList = data.Split(',').ToList();
                if (tempList.Count == 0) { return; }
                foreach (string s in tempList)
                {
                    type = s.Split('-');
                    if (type != null)
                    {
                        summary.Add(type[0].ToUpper(), type[1]);
                    }
                }
                if (summary.Count != 0) { FieldList.Add(fieldName, summary); }
            }
            finally { type = null; }
        }
        private CONFIG_FIELDLEVEL_VALIDATION GetFieldConfig(string viewModelName) {
            try {
                return fieldLevelValidationConfigRepository.Get(exp => exp.CFV_ViewModelName == viewModelName && exp.CFV_IsActive);
            }
            finally { }
        }

        private void MinLengthValidation(int minLength)
        {
            ModelClientValidationRule mcvrTwo = new ModelClientValidationRule();
            try
            {
                if (minLength == 0) { return; }
                mcvrTwo.ValidationType = "minlengthvalidation";
                mcvrTwo.ErrorMessage = ResourceKeeper.GetResource("CMN_MinimumLength") + " : " + minLength;
                mcvrTwo.ValidationParameters.Add("minlength", minLength);
                rules.Add(mcvrTwo);
            }
            finally { mcvrTwo = null; }
        }
        private void MaxLengthValidation(int maxLength)
        {
            ModelClientValidationRule mcvrTwo = new ModelClientValidationRule();
            try
            {
                if (maxLength == 0) { return; }
                mcvrTwo.ValidationType = "maxlengthvalidation";
                mcvrTwo.ErrorMessage = ResourceKeeper.GetResource("CMN_MaximumLength") + " : " + maxLength;
                mcvrTwo.ValidationParameters.Add("maxlength", maxLength);
                rules.Add(mcvrTwo);
            }
            finally { mcvrTwo = null; }
        }
        private void ValidateWhiteSpace()
        {
            ModelClientValidationRule mcvrTwo = new ModelClientValidationRule();
            try
            {
                mcvrTwo.ValidationType = "whitespacevalidation";
                mcvrTwo.ErrorMessage = ResourceKeeper.GetResource("CMN_RemoveWhiteSpace");
                rules.Add(mcvrTwo);
            }
            finally { mcvrTwo = null; }
        }
        private void AlphaNumericValidation(string displayName)
        {
            ModelClientValidationRule mcvrTwo = new ModelClientValidationRule();
            try
            {
                mcvrTwo.ValidationType = "alphanumericvalidation";
                mcvrTwo.ErrorMessage = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("CMN_AlphaNumeric")+" "+ResourceKeeper.GetResource(displayName)+".";
                rules.Add(mcvrTwo);
            }
            finally { mcvrTwo = null; }
        }
        private void SpecialCharacterValidation()
        {
            ModelClientValidationRule mcvrTwo = new ModelClientValidationRule();
            try
            {
                mcvrTwo.ValidationType = "specialcharectervalidation";
                mcvrTwo.ErrorMessage = ResourceKeeper.GetResource("CMN_SpecialChar_NotAllowed");
                rules.Add(mcvrTwo);
            }
            finally { mcvrTwo = null; }
        }
    }
}