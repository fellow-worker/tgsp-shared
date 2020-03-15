import chai from 'chai';
import { compare, hash } from '../src/password.js';

describe('Password Tests', () => {
    it("password - simple test", async () => {
        const password = await hash("password");
        chai.expect(password).not.to.be.equal("password");
        chai.expect(await compare("password",password)).to.be.true;
        chai.expect(await compare("password-1",password)).to.be.false;
    });
});