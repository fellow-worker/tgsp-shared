import crypto from 'crypto';
import util from 'util';

/**
 * The number of bytes to use in the salt
 */
const saltBytes = 64;

/**
 * The number of bytes to use for the keys in scrypt
 */
const keyLength = 64;

/**
 * This method wraps around the scrypt function of node js
 * @param {string} password The password to hash
 * @param {string} saltString The salt to use
 */
const crypt = async(password,saltString) => {
    const scrypt = util.promisify(crypto.scrypt);
    const buffer = await scrypt(password,saltString,keyLength);
    const derivedKey = buffer.toString('base64');
    return derivedKey
}

/**
 * Hash a password using Node's asynchronous pbkdf2 (key derivation) function.
 * @returns A string that is can be saved in the database for storage
 */
export const hash = async (password) => {
	const randomBytes = util.promisify(crypto.randomBytes);
	try {
        const salt = await randomBytes(saltBytes);
        const saltString = salt.toString('base64');
        const derivedKey = await crypt(password,saltString);
        return saltString + "#" + derivedKey;
	}
    catch(err) { return null; }
};

/**
 * This method is capable of validating the password against a hash
 * @param {String} password The password to validate
 * @param {String} hash The hash as saved in the database for instance
 */
export const compare = async(password,hash) => {

    try {
        const data = hash.split("#");
        if(data.length !== 2) return false;

        const saltString = data[0];
        const derivedKey = data[1];
        const expected = await crypt(password,saltString);

        return expected === derivedKey;

    } catch(err) { return false;}
}