import chai from 'chai';
import { signData, validateData } from '../src/integrity.js';

describe('Integrity Tests', () => {
    it("validateData - Correct data" ,() => {
        const data = { value : "test" };
        const secret = "secret";
        const signed = signData(data,secret);

        chai.expect(signed).to.be.not.null;
        chai.expect(signed.data).to.be.not.null;
        chai.expect(signed.signature).to.be.not.null;

        const valid = validateData(signed, secret);
        chai.expect(valid).to.be.true;
    });

    it("validateData - Tampered signature" ,() => {
        const data = { value : "test" };
        const secret = "secret";
        const signed = signData(data,secret);

        chai.expect(signed).to.be.not.null;
        chai.expect(signed.data).to.be.not.null;
        chai.expect(signed.signature).to.be.not.null;
        signed.signature = secret;

        const valid = validateData(signed, secret);
        chai.expect(valid).to.be.false;
    });

    it("validateData - Tampered data" ,() => {
        const data = { value : "test" };
        const secret = "secret";
        const signed = signData(data,secret);

        chai.expect(signed).to.be.not.null;
        chai.expect(signed.data).to.be.not.null;
        chai.expect(signed.signature).to.be.not.null;
        signed.data = { value : "test 1" };

        const valid = validateData(signed, secret);
        chai.expect(valid).to.be.false;
    });
});