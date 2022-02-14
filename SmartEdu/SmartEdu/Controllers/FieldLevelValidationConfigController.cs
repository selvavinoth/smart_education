using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using System.Reflection;
using SmartEdu.Data.Models;
using SmartEdu.Bll.Services;
using System.Text;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class FieldLevelValidationConfigController : Controller
    {
        //
        // GET: /FieldLevelValidationConfig/
        private int CollegeId = 1;
        private long UserId = 0;
        private int DP_Id = 1;

        private readonly IFieldLevelValidationService fieldLevelValidationService;
        public FieldLevelValidationConfigController(IFieldLevelValidationService fieldLevelValidationService) {
            this.fieldLevelValidationService = fieldLevelValidationService;
        }
        public ActionResult Index()
        {
            ConfigFieldLevelValidation ViewModel=new ConfigFieldLevelValidation();
            try {
                ViewModel.ViewModelNameList = GetViewModelList();
                ViewModel.DataTypeList = GetDataTypeList();
                ViewModel.FilteredFieldList = new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } };
                return View(ViewModel);
            }
            catch(Exception exe){return View(ViewModel);}
            finally { ViewModel = null; }
        }

        public ActionResult GetFieldsByType(string ViewModelName,string DataType) {
            try {
                return Json(new { FilteredFieldList = GetFieldsByDataType(ViewModelName, DataType) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        public ActionResult GetConfigModel(string ViewModelName, string SelectedField)
        {
            try
            {
                return Json(new { ConfigViewModel = FillConfigModel(ViewModelName, SelectedField) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        private ConfigFieldLevelValidation FillConfigModel(string viewModelName,string SelectedField) {
            ConfigFieldLevelValidation viewModel = new ConfigFieldLevelValidation();
            Dictionary<string, Dictionary<string, string>> FieldValidationList = null;
            CONFIG_FIELDLEVEL_VALIDATION model = null;
            try {
                model = fieldLevelValidationService.GetFieldLevelConfig(CollegeId, DP_Id, viewModelName);
                if (model == null) { return viewModel; }
                FieldValidationList = GetValidationDicionary(model.CFV_ValidationString);
                if (FieldValidationList.Keys.Contains(SelectedField)) { SetFieldValue(viewModel, FieldValidationList[SelectedField]); }
                return viewModel;
            }
            finally { viewModel = null; model = null; FieldValidationList = null; }
        }
        private void SetFieldValue(ConfigFieldLevelValidation viewModel, Dictionary<string, string> FormattedDictionary)
        {
            try
            {
                foreach (var s in FormattedDictionary) {
                    switch (s.Key) { 
                        case "IsMandatory":
                            viewModel.IsMandatory = true;
                            break;
                        case "IsInteger":
                            viewModel.IsInteger = true;
                            break;
                        case "IsAlphaNumeric":
                            viewModel.IsAlphaNumeric = true;
                            break;
                        case "AllowSpecialCharacter":
                            viewModel.Allow_SpecialCharacter = true;
                            break;
                        case "AllowWhiteSpace":
                            viewModel.Allow_WhiteSpace = true;
                            break;
                        case "IsAmount":
                            viewModel.IsAmount = true;
                            break;
                        case "NeedEmailValidation":
                            viewModel.Need_EmailValidation = true;
                            break;
                        case "MaxLength":
                            viewModel.MaximumLength = Convert.ToInt32(s.Value);
                            break;
                        case "MinLength":
                            viewModel.MinimumLength = Convert.ToInt32(s.Value);
                            break;
                        case "Range":
                            viewModel.Range = s.Value;
                            break;
                        case "DateFormat":
                            viewModel.DateFormat = s.Value;
                            break;
                        case "DisplayName":
                            viewModel.DisplayName = s.Value;
                            break;
                        default :
                            break;
                    }
                }
            }
            finally { }
        }

        #region DropDown Filling
        private enum DataType
        {
            Boolean = 1,
            Byte = 2,
            Int16 = 3,
            Int32=4,
            Int64=5,
            Decimal = 6,
            Binary = 7,
            String = 8,
            DateTime = 9,
        }
        private IEnumerable<SelectListItem> GetViewModelList()
        {
            try
            {
                Type[] ClassNameList = GetClassesInNamespace(Assembly.GetExecutingAssembly(), "SmartEdu.ViewModel");
                try
                {
                    return (new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } }).Union((
                            from m in ClassNameList select new SelectListItem { Text = m.Name, Value = m.Name }).OrderBy(exp=>exp.Text)).ToList();
                }
                finally { ClassNameList = null; }

            }
            finally {  }
        }
        private IEnumerable<SelectListItem> GetDataTypeList()
        {
            List<string> DataList = new List<string>();
            try
            {
                DataList=Enum.GetValues(typeof(DataType)).Cast<DataType>().Select(v => v.ToString()).ToList();
                return (new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } }).Union((
                        from m in DataList select new SelectListItem { Text = m, Value = m }).OrderBy(exp=>exp.Text)).ToList();
            }
            finally { DataList = null; }
        }
        private Type[] GetClassesInNamespace(Assembly assembly, string nameSpace)
        {
            try
            {
                return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
            }
            finally { }
        }
        private IEnumerable<SelectListItem> GetFieldsByDataType(string viewModelName,string FieldType) {
            List<PropertyInfo> FieldInfoList = null;
            try {
                FieldInfoList = GetFieldList(viewModelName, FieldType);
                if (FieldInfoList.Count != 0) {
                    return (from m in FieldInfoList select new SelectListItem { Text = m.Name, Value = m.Name }).OrderBy(exp => exp.Text).ToList();
                }
                return null;
            }
            finally { }
        }
        private List<PropertyInfo> GetFieldList(string ClassName,string DataType) {
            try {
                Type[] ClassNameList = GetClassesInNamespace(Assembly.GetExecutingAssembly(), "SmartEdu.ViewModel");
                try {
                    if (ClassNameList.Length != 0) {
                        return ClassNameList.Where(exp => exp.Name == ClassName).FirstOrDefault().GetProperties().Where(exp=>exp.PropertyType.Name==DataType).ToList();
                    }
                    return null;
                }
                finally { ClassNameList = null; }
            }
            finally { }
        }
        #endregion

        #region Grid Loading
        //  Field Name,Mandatory,Min Length,Max Length,Email Validation,Special Char,Range,Dateformat,Block WiteSpace,IsInteger,IsAmount
        public ActionResult LoadGrid(string viewModelName) {
            try {
                return Json(new { GridData = GetGridData(viewModelName) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        private object GetGridData(string viewModelName) {
            Dictionary<string, Dictionary<string, string>> FieldValidationList = null;
            CONFIG_FIELDLEVEL_VALIDATION tempModel = null;
            try {
                if (viewModelName != null && viewModelName.Trim() != "") {
                    tempModel = fieldLevelValidationService.GetFieldLevelConfig(CollegeId, DP_Id, viewModelName);
                    if (tempModel == null) { return new { total_count = 0, rows = new { }}; }
                    FieldValidationList = GetValidationDicionary(tempModel.CFV_ValidationString);
                    return new
                    {
                        total_count=FieldValidationList.Count,
                        rows=(from f in FieldValidationList
                              select new{
                                id=f.Key,
                                data = GetRowData(f.Key,f.Value)
                              }).ToList()
                    };
                }
                return new { total_count = 0, roes = new { } }; 
            }
            finally { FieldValidationList = null; tempModel = null; }
        }
        private string[] GetRowData(string key,Dictionary<string, string> stringDataList) {
            List<string> ValList = new List<string>();
            try {
                ValList.Add(key);
                if (stringDataList.Keys.Contains("IsMandatory")) { ValList.Add("True"); } else { ValList.Add("False"); }
                if (stringDataList.Keys.Contains("MaxLength")) { ValList.Add(stringDataList["MaxLength"]); } else { ValList.Add(""); }
                if (stringDataList.Keys.Contains("MinLength")) { ValList.Add(stringDataList["MinLength"]); } else { ValList.Add(""); }
                if (stringDataList.Keys.Contains("EmailValidation")) { ValList.Add("True"); } else { ValList.Add("False"); }
                if (stringDataList.Keys.Contains("AllowSpecialCharacter")) { ValList.Add("True"); } else { ValList.Add("False"); }
                if (stringDataList.Keys.Contains("Range")) { ValList.Add(stringDataList["Range"]); } else { ValList.Add(""); }
                if (stringDataList.Keys.Contains("DateFormat")) { ValList.Add(stringDataList["DateFormat"]); } else { ValList.Add(""); }
                if (stringDataList.Keys.Contains("BlockWhiteSpace")) { ValList.Add("True"); } else { ValList.Add("False"); }
                if (stringDataList.Keys.Contains("IsInteger")) { ValList.Add("True"); } else { ValList.Add("False"); }
                if (stringDataList.Keys.Contains("IsAmount")) { ValList.Add("True"); } else { ValList.Add("False"); }
                if (stringDataList.Keys.Contains("IsAlphaNumeric")) { ValList.Add("True"); } else { ValList.Add("False"); }
                return ValList.ToArray();
            }
            finally { ValList = null; }
        }
        private Dictionary<string, Dictionary<string, string>> GetValidationDicionary(string validationString) {
            List<string> ValStringList = null;
            Dictionary<string, Dictionary<string, string>> FieldDictionary = new Dictionary<string, Dictionary<string, string>>();
            string[] data=new string[2];
            try {
                ValStringList = validationString.Split('/').ToList();
                foreach(string s in ValStringList){
                    data=s.Split('?');
                    FieldDictionary.Add(data[0], GetFieldDicionary(data[1]));
                }
                //  "CU_PhoneNumber?IsRequired-true,DisplayName-Phone Number,MaxLength-15,MinLength-10/CU_Email_Id?IsRequired-true,DisplayName-Email Id,MaxLength-25/CU_RegisterNumber?IsRequired-true,DisplayName-Register Number,MaxLength-15";
                return FieldDictionary;
            }
            finally { ValStringList = null; FieldDictionary = null; data = null; }
        }
        private Dictionary<string, string> GetFieldDicionary(string FieldString)
        {
            List<string> ValStringList = null;
            Dictionary<string, string> FieldDictionary = new Dictionary<string, string>();
            string[] data = new string[2];
            try
            {
                ValStringList = FieldString.Split(',').ToList();
                foreach (string s in ValStringList)
                {
                    data = s.Split('-');
                    FieldDictionary.Add(data[0], data[1]);
                }
                return FieldDictionary;
                //  "CU_PhoneNumber?IsRequired-true,DisplayName-Phone Number,MaxLength-15,MinLength-10/CU_Email_Id?IsRequired-true,DisplayName-Email Id,MaxLength-25/CU_RegisterNumber?IsRequired-true,DisplayName-Register Number,MaxLength-15";
            }
            finally { ValStringList = null; FieldDictionary = null; data = null; }
        }
        #endregion

        #region IUD Action
        private Dictionary<string, string> FormattedDictionary = null;
        public ActionResult FieldLevelValidationIUAction(ConfigFieldLevelValidation viewModel) {
            string msg = "";
            CONFIG_FIELDLEVEL_VALIDATION model = null;
            Dictionary<string, Dictionary<string, string>> FieldValidationList = new Dictionary<string,Dictionary<string,string>>();
            try {
                if (viewModel == null) { return Json(false); }
                if (viewModel.SelectedFieldName == null || viewModel.SelectedFieldName.Trim() == "") { return Json(new { Msg=msg, Status=false ,GridData=false},JsonRequestBehavior.AllowGet); }
                if (!ValidateAndSetFieldValue(viewModel) && viewModel.CFV_ID<=0) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Fill_Form"), Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                model = fieldLevelValidationService.GetFieldLevelConfig(CollegeId, DP_Id, viewModel.CFV_ViewModelName);
                if (model == null) {    // insert
                    model = new CONFIG_FIELDLEVEL_VALIDATION();
                    model.CFV_College_Id = CollegeId;
                    model.CFV_DP_Id = DP_Id;
                    model.CFV_ViewModelName = viewModel.CFV_ViewModelName;
                    FieldValidationList.Add(viewModel.SelectedFieldName,FormattedDictionary);
                    model.CFV_ValidationString = GetValidationString(FieldValidationList);
                    fieldLevelValidationService.InsertFieldLevelConfig(model, UserId);
                }
                else {    // update
                    FieldValidationList = GetValidationDicionary(model.CFV_ValidationString);
                    if (FieldValidationList.Keys.Contains(viewModel.SelectedFieldName)) { FieldValidationList[viewModel.SelectedFieldName] = FormattedDictionary; }
                    else { FieldValidationList.Add(viewModel.SelectedFieldName, FormattedDictionary); }
                    model.CFV_ValidationString = GetValidationString(FieldValidationList);
                    fieldLevelValidationService.UpdateFieldLevelConfig(model, UserId);
                    
                }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Saved_Successfully"), Status = true, GridData = GetGridData(viewModel.CFV_ViewModelName) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { FormattedDictionary = null; }
        }
        private string GetValidationString(Dictionary<string, Dictionary<string, string>> valFieldStringDictionary) {
            StringBuilder fb = new StringBuilder();
            int i=0,j=0,valDicCout=0,innerDicCout=0;
            try {
                valDicCout = valFieldStringDictionary.Count;
                foreach (var d in valFieldStringDictionary) {
                    fb.Append(d.Key).Append("?");
                    j = 0;
                    i++;
                    innerDicCout = d.Value.Count;
                    foreach (var s in d.Value) {
                        fb.Append(s.Key).Append("-").Append(s.Value);
                        j++;
                        if (j < innerDicCout) { fb.Append(","); }
                    }
                    if (i < valDicCout) { fb.Append("/"); }
                }
                return fb.ToString();
            }
            finally { fb = null; }
        }
        private bool ValidateAndSetFieldValue(ConfigFieldLevelValidation viewModel)
        {
            FormattedDictionary = new Dictionary<string, string>();
            bool Flag = false;
            try {
                if (viewModel.DisplayName != null && viewModel.DisplayName.Trim() != "") { FormattedDictionary.Add("DisplayName", viewModel.DisplayName); Flag = true; }
                if (viewModel.IsMandatory) { FormattedDictionary.Add("IsMandatory", "True"); Flag = true; }
                if (viewModel.IsInteger) { FormattedDictionary.Add("IsInteger", "True"); Flag = true; }
                if (viewModel.IsAlphaNumeric) { FormattedDictionary.Add("IsAlphaNumeric", "True"); Flag = true; }
                if (viewModel.Allow_SpecialCharacter) { FormattedDictionary.Add("AllowSpecialCharacter", "True"); Flag = true; }
                if (viewModel.Allow_WhiteSpace) { FormattedDictionary.Add("AllowWhiteSpace", "True"); Flag = true; }
                if (viewModel.IsAmount) { FormattedDictionary.Add("IsAmount", "True"); Flag = true; }
                if (viewModel.Need_EmailValidation) { FormattedDictionary.Add("NeedEmailValidation", "True"); Flag = true; }
                if (viewModel.MaximumLength.HasValue) { FormattedDictionary.Add("MaxLength", viewModel.MaximumLength.Value.ToString()); Flag = true; }
                if (viewModel.MinimumLength.HasValue) { FormattedDictionary.Add("MinLength", viewModel.MinimumLength.Value.ToString()); Flag = true; }
                if (viewModel.Range != null && viewModel.Range.Trim() == "") { FormattedDictionary.Add("Range", viewModel.Range); Flag = true; }
                if (viewModel.DateFormat != null && viewModel.DateFormat.Trim() == "") { FormattedDictionary.Add("DateFormat", viewModel.DateFormat); Flag = true; }
                return Flag;
            }
            finally {  }
        }
        #endregion

        #region Reset or Delete Config
        public ActionResult ResetConfig(string ViewModelName, string SelectedField)
        {
            CONFIG_FIELDLEVEL_VALIDATION model = new CONFIG_FIELDLEVEL_VALIDATION();
            Dictionary<string, Dictionary<string, string>> FieldValidationList = new Dictionary<string, Dictionary<string, string>>();
            try {
                if (SelectedField == null || SelectedField.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseSelect") + " " + ResourceKeeper.GetResource("APP_FieldName"), Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                model = fieldLevelValidationService.GetFieldLevelConfig(CollegeId, DP_Id, ViewModelName);
                if (model == null)
                {    // insert
                    return Json(new { Msg = ResourceKeeper.GetResource("CMN_NoConfigFound"), Status = false, GridData = false }, JsonRequestBehavior.AllowGet); 
                }
                else
                {    // update
                    FieldValidationList = GetValidationDicionary(model.CFV_ValidationString);
                    if (FieldValidationList.Keys.Contains(SelectedField)) { FieldValidationList.Remove(SelectedField); }
                    else { return Json(new { Msg = ResourceKeeper.GetResource("CMN_NoConfigFound"), Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                    model.CFV_ValidationString = GetValidationString(FieldValidationList);
                    if (model.CFV_ValidationString == null || model.CFV_ValidationString.Trim() == "") { fieldLevelValidationService.DeleteFieldLevelConfig(model.CFV_ID, UserId); }
                    else { fieldLevelValidationService.UpdateFieldLevelConfig(model, UserId); }
                }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Reset_Successfully"), Status = true, GridData = GetGridData(ViewModelName) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; FieldValidationList = null; }
        }
        #endregion
    }
}
