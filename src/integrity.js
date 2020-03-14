// This module can be used to exchange datas that need some can of signing
import crypto from 'crypto';

/**
 * This method will create a signature for the given data and secret 
 * @param {*} data The data to create a signaure for
 * @param {String} secret The secret to use when creating the signature
 * @param {String} digest The digest to use (hex or base64), default is base64
 */
const createHash = (data, secret, digest = 'base64') => {

    const text = JSON.stringify(data);
    const buffer = Buffer.from(text, 'utf8');
    const encryptor = crypto.createHmac('sha256', secret);
    return encryptor.update(buffer).digest(digest); 
}

/**
 * This method will sign data with a signature
 * @param {*} data The data to create a signaure for
 * @param {String} secret The secret to use when creating the signature
 * @param {String} digest The digest to use (hex or base64), default is base64
 */
const signData = (data, secret, digest = 'base64') => {
    const hash = createHash(data, secret, digest);
    return { data : data, signature : hash }
}

/**
 * This method will validate the data that was created using the signData method
 * @param {*} data The data that was create with the signData and should be validate
 * @param {String} secret The secret to use when creating the signature
 * @param {String} digest The digest to use (hex or base64), default is base64
 * @return {Boolean} True when the data was valid, else false
 */
const validateData = ({data, signature}, secret, digest = 'base64') => {
    const hash = createHash(data, secret, digest);
    return signature == hash;
}

export { signData, validateData };