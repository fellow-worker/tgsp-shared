import crypto from 'crypto';
import util from 'util'

const algorithm = 'rsa'
const signatureAlgorith = `RSA-SHA256`;

/**
 * This method will simple return the seconds since epoch, handy for easy time validation
 */
export const secondsSinceEpoch = () => {
    return Math.floor(Date.now() / 1000);
};

export const generatePublicPrivateKeys = async () => {
    const generateKeyPair = util.promisify(crypto.generateKeyPair);
    const generated = await generateKeyPair(algorithm, { modulusLength: 2048, });
    const publicKey = generated.publicKey.export({ type: 'pkcs1', format: 'der' }).toString('base64');
    const privateKey = generated.privateKey.export({ type: 'pkcs1', format: 'der' }).toString('base64');
    return { privateKey, publicKey };
}

/**
 * this method will sign the given data using and private RSA key 
 * @param {*} data The data
 * @param {String} privateKey The private key in a base64 format
 */
export const signData = (data, privateKey) => {

    const sign = crypto.createSign(signatureAlgorith);
    sign.write(JSON.stringify(data));
    sign.end();
    const buffer = Buffer.from(privateKey, 'base64');
    const key = crypto.createPrivateKey({ type: 'pkcs1', format: 'der', key: buffer });
    const signature = sign.sign(key, "base64");
    return signature;
}

/**
 * this method will verify if the given data was signed you the public key
 * @param {*} data The data
 * @param {String} publicKey The private key in a base64 format
 */
export const verifyData = (data, signature, publicKey) => {
    const verify = crypto.createVerify(signatureAlgorith);
    verify.write(JSON.stringify(data));
    verify.end();
    const buffer = Buffer.from(publicKey, 'base64');
    const key = crypto.createPublicKey({ type: 'pkcs1', format: 'der', key: buffer });
    return verify.verify(key, signature, "base64");
}

/**
 * This method can be used to hash data given a secret
 * @param {String} data The data to create a hash for
 * @param {String} secret The secret to use as salt
 * @returns {String} A base64 uri-encoded has (so it can be used in headers and uri's)
 */
export const hashData = (data, secret) => {

    const hash = crypto.createHash('sha256').update(JSON.stringify(data)).update(secret).digest("base64");
    return encodeURIComponent(hash);
}

/**
 * This method will verifies the data against the hash given the secret
 * @param {*} data The original data that was hashed
 * @param {*} hash The hash that should be verified
 * @param {*} secret The secret that is used as salt
 */
export const validateHash = (data, hash, secret) => {
    return hashData(data, secret) === hash;
}

/**
 * This method will generate a token for a userId and role that is signed with a public key
 * @param {string|number} userId The id of the user
 * @param {string|array} roles The role of the user
 * @param {string|number} ip An identifier for the connection
 * @param {string} serviceId The id for the service this token is for
 * @param {String} privateKey The private key in a base64 format
 * @param {number} expires A unix time stamp at which moment the token expires
 * @return {string} A base64 string with the full token
 */
export const getPublicPrivateKeyToken = (userId, roles, ip, serviceId, expires, privateKey) => {

    // create the data object and the signature
    const data = { userId, roles, ip, serviceId, expires };
    const signature = signData(data, privateKey);

    // create the token object and from that a base64 string that uri encoded to it can be used as a bearer token
    return encodeURIComponent(Buffer.from(JSON.stringify({ data, signature })).toString('base64'));
};

/**
 * This method validate the token
 * @param {String} token The token that is provided by the user
 * @param {String} ip An identifier for the
 * @param {String} publicKey The public key in a base64 format. With this key it can be verified that the token by somebody who knows the private key
 */
export const validatePublicPrivateKeyToken = (token, ip, publicKey) => {
    try {
        // first reverse the token as it create
        const tokenObject = JSON.parse(Buffer.from(decodeURIComponent(token), 'base64').toString('utf-8'));

        // Check if the data is not expired
        if (tokenObject.data.expires < secondsSinceEpoch()) return { valid: false, error: 'token expired' };

        // Check if the ip address is matching
        if (tokenObject.data.ip !== ip) return { valid: false, error: 'ip address mismatch' };

        // Verify the data
        const verified = verifyData(tokenObject.data, tokenObject.signature, publicKey);
        if (verified == false) return { valid: false, error: 'verification error' };

        // Return the data
        return { ...tokenObject.data, valid: true };

    }
    catch (error) { return { valid: false, error: 'token format incorrect', info: error }; }
};

/**
 * This method will generate a token that is hash with a secret key (synchronous encryption)
 * @param {String} serviceId The id of the service this token is for
 * @param {String} ip The ip address of the user
 * @param {String} secret The secret to hash the token
 * @param {number} expires A unix time stamp at which moment the token expires
 */
export const getSecretToken = (serviceId, ip, expires, secret) => {
    const data = { serviceId, ip, expires };
    const token = { hash : hashData(data,secret), data : data }
    return encodeURIComponent(JSON.stringify(token));
};

/**
 * This method will validate that is secret using a secret key (synchronous encryption)
 * @param {String} token The created token
 * @param {String} ip The ip address of the user
 * @param {String} secret The secret that was used to hash the token
 */
export const validateSecretToken = (token, ip, secret) => {
    try {
        // convert the token back to the original data
        token = JSON.parse(decodeURIComponent(token));

        // Check if the data is not expired
        if (token.data.expires < secondsSinceEpoch()) return { valid: false, error: 'token expired' };

        // Check if the ip address is matching
        if (token.data.ip !== ip) return { valid: false, error: 'ip address mismatch' };

        // Verify the data
        const verified = validateHash(token.hash, token.data, secret);
        if (verified == false) return { valid: false, error: 'verification error' };

        // Return the data
        return { ...token.data, valid: true };

    } catch (error) { return { valid : false, error : "token format error" , info : error }; }
};