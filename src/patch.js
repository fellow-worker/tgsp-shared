/**
 * This method will simply replace the values of the properties in source with the once in patch
 * @param {Object} source The source object for which the properties should be replaced
 * @param {Object} patch The patch object which contains the new values
 * @param {Array} excluded A list of properties to exclude in the update
 */
export const patch = (source, patch, excluded) => {
    // Ensure that excluded is an array 
    if(Array.isArray(excluded) === false) excluded = [];
    const targetKeys = Object.keys(patch);

    // Loop through the properties of target and update source
    Object.keys(source).forEach(key => {
        if(excluded.includes(key) === true) return;
        if(targetKeys.includes(key) === false) return;
        source[key] = patch[key];
    });

    // return the result
    return source;
}