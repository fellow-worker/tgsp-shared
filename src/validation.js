import URL from 'url';
import validUrl from 'valid-url'

/**
 * @function isHttpsUri(value)
 * @description This method will test if the potential URI is valid RFC 3986 URI and is using the https protocol
 * @param {*} value The potential URI to test.
 * @returns {Boolean} True when if is valid https URI else false
 */
export const isHttpsUri = (value) => {
    return validUrl.isHttpsUri(value) !== undefined;
}

/**
 * This method will test if the given value is a correct GUID
 * @param {String} value The value value to test 
 * @param {Boolean} acceptNill When set to false (default) the NIL GUID (00000000-0000-0000-0000-000000000000) is not accepted
 * @returns {Boolean} True when the value is an acceptable GUID
 */
export const isGuid = (value, acceptNill = false) => {

    if (acceptNill === true) {
        const regexp = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-5][0-9a-f]{3}-[089ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
        return regexp.test(value);
    } else {
        const regexp = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
        return regexp.test(value);
    }
}

/**
 * Validate the given value is not null and not undefined
 */
export const hasValue = value => {
    return value !== undefined && value !== null && value.toString() !== "";
};

/**
 * validate the value string value has a maximum of max
 * remarks; 
 */
export const hasMinLength = (value, min, validateEmpty = true) => {
    if (value === null || value === undefined || value === "") return validateEmpty;
    return value.toString().length >= min;
};

/**
 * validate the value string value has a minimum length of min
 */
export const hasMaxLength = (value, max) => {
    if (value === null || value === undefined || value === "") return true;
    return value.toString().length <= max;
};

/**
 * validate the value string value has a minimum length of min and maximum of max
 */
export const hasLength = (value, min, max, validateEmpty = true) => {
    return hasMinLength(value, min, validateEmpty) && hasMaxLength(value, max);
};

export const hasType = (value, type) => {
    switch (type) {
        case 'url': return isUrl(value);
        case 'email': return isEmail(value);
        case 'password': return isPassword(value);
        case 'array': return Array.isArray(value);
        default: return true;
    }
}

export const hasCount = (value, min) => {
    if (Array.isArray(value) === false) return false;
    if (value.length < min) return false;
    return true;
}

export const isEmail = (value) => {
    // this bizarre regex will match most of our case
    const expression = /(?!.*\.{2})^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return expression.test(String(value).toLowerCase())
}

export const isPassword = (value) => {
    var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!-@#$%^&*])(?=.{12,})");
    return strongRegex.test(String(value))
}

export const isUrl = (value) => {
    try {
        const parsed = new URL(value);
        if (parsed.hostname !== null) return true;
        return false;
    }
    catch (exp) { return false; }
}

/**
 * this method validate rules on an object
 */
export const validate = (value, rules) => {
    const result = { hasErrors: false, errors: {} };
    rules.forEach(rule => {
        let valid = false;
        switch (rule.name) {
            case "required":
                valid = hasValue(value[rule.prop]);
                break;
            case "minLength":
                valid = hasMinLength(value[rule.prop], rule.min);
                break;
            case "maxLength":
                valid = hasMaxLength(value[rule.prop], rule.max);
                break;
            case "length":
                valid = hasLength(value[rule.prop], rule.min, rule.max);
                break;
            case "count":
                valid = hasCount(value[rule.prop], rule.min);
                break;
            case "type":
                valid = hasType(value[rule.prop], rule.type);
                break;
            case "regexp" : 
                const regex = new RegExp(rule.regexp);    
                valid = regex.test(value[rule.prop]);
            default:
                break;
        }
        if (valid === false) {
            if (result.errors.hasOwnProperty(rule.prop) === false) result.errors[rule.prop] = [];
            result.errors[rule.prop].push(rule);
            result.hasErrors = true;
        }
    });
    return result;
}

export const isInt = (value) => {
    if (value === undefined || value === null) return false;
    var parsed = parseInt(value);
    if (isNaN(parsed) === true) return false;
    return parsed.toString() === value.toString();
}