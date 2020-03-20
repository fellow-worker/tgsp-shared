import { getPublicPrivateKeyToken, validatePublicPrivateKeyToken, signData, verifyData, generatePublicPrivateKeys, secondsSinceEpoch } from '../src/token.js';
import chai from 'chai';
import { performance } from 'perf_hooks';

const expect = chai.expect;

describe('token.js', () => {

    it('Sign / Verify', async () => {
        const { publicKey, privateKey } = await generatePublicPrivateKeys();

        const data = "test";
        const signature = signData(data,privateKey);
        const verified = verifyData(data,signature,publicKey);

        expect(verified).to.be.true;
    });

     it('publicPrivateKeyToken - valid token', async () => {
        const { publicKey, privateKey } = await generatePublicPrivateKeys();
        
        const token = getPublicPrivateKeyToken( 2, 'admin', '127.0.0.1', 92, secondsSinceEpoch() + 1000, privateKey);

        const start = performance.now();
        const result = validatePublicPrivateKeyToken( token,'127.0.0.1',publicKey);
        const end = performance.now();

        expect(end - start).to.be.lessThan(10);
        expect(result.valid).to.true;
        expect(result.userId).to.eql(2);
        expect(result.roles).to.eql('admin');
    });

    it('publicPrivateKeyToken - different ip', async () => {
        const { publicKey, privateKey } = await generatePublicPrivateKeys();

        const token = getPublicPrivateKeyToken( 2, 'admin', '127.0.0.1', 92,  secondsSinceEpoch() + 1000, privateKey);
        const result = validatePublicPrivateKeyToken( token,'128.0.0.1',publicKey);

        expect(result.valid).to.false;
        expect(result.error).to.eql('ip address mismatch');
    });

    it('publicPrivateKeyToken - token format', async () => {
        const { publicKey } = await generatePublicPrivateKeys();
        const result = validatePublicPrivateKeyToken('token','127.0.0.1',publicKey);
        expect(result.valid).to.false;
        expect(result.error).to.eql('token format incorrect');
    });

    it('publicPrivateKeyToken - token expired', async () => {

        const { publicKey, privateKey } = await generatePublicPrivateKeys();
        const token = getPublicPrivateKeyToken( 2, 'admin', '127.0.0.1', 92,  secondsSinceEpoch() - 1000, privateKey);
        const result = validatePublicPrivateKeyToken( token,'127.0.0.1',publicKey);

        expect(result.valid).to.false;
        expect(result.error).to.eql('token expired');
    });
});
