// Minimum Length Validation
jQuery.validator.addMethod("minlengthvalidation", function (value, element, minLength) {
    if (value == null || value == "") { return false; }
    if (value.length < minLength) { return false; }
    return true;
});
jQuery.validator.unobtrusive.adapters.add("minlengthvalidation", ["minlength"], function (options) {
    options.rules["minlengthvalidation"] = options.params.minlength;
    options.messages["minlengthvalidation"] = options.message;
});

// Maximum Length Validation
jQuery.validator.addMethod("maxlengthvalidation", function (value, element, maxLength) {
    if (value == null || value == "") { return false; }
    if (value.length > maxLength) { return false; }
    return true;
});
jQuery.validator.unobtrusive.adapters.add("maxlengthvalidation", ["maxlength"], function (options) {
    options.rules["maxlengthvalidation"] = options.params.maxlength;
    options.messages["maxlengthvalidation"] = options.message;
});

// White Space Validation
jQuery.validator.addMethod("whitespacevalidation", function (value, element, param) {
    if (value == null || value == "" || value.indexOf(' ') >= 0) { return false; }
    return true;
});
jQuery.validator.unobtrusive.adapters.add("whitespacevalidation", ["param"], function (options) {
    options.rules["whitespacevalidation"] = options.params;
    options.messages["whitespacevalidation"] = options.message;
});

// Alphanumeric Validation
jQuery.validator.addMethod("alphanumericvalidation", function (value, element, param) {
    if (value = null || value == "") { return false; }
    var length = value.length;
    for (var i = 0; i < length; i++) {
        if (fnIsAlphabet(value.charAt(i)) && fnIsInteger()) { }
    }
    return /^[a-zA-Z0-9]+$/.test(value);
});
jQuery.validator.unobtrusive.adapters.add("alphanumericvalidation", ["param"], function (options) {
    options.rules["alphanumericvalidation"] = options.params;
    options.messages["alphanumericvalidation"] = options.message;
});
function fnIsAlphabet(c) {
    var alphabets = "abcdefghijklmnopqrstuvwxyz";
    if (alphabets.indexOf(c) == -1) { return false; }
    return true;
}
function fnIsInteger(c) {
    var alphabets = "0123456789";
    if (alphabets.indexOf(c) == -1) { return false; }
    return true;
}


// Special Character Validation
jQuery.validator.addMethod("specialcharectervalidation", function (value, element, param) {
    return /^[a-zA-Z0-9]+$/.test(value);
});
jQuery.validator.unobtrusive.adapters.add("specialcharectervalidation", ["param"], function (options) {
    options.rules["specialcharectervalidation"] = options.params;
    options.messages["specialcharectervalidation"] = options.message;
});