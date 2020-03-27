import chai from 'chai';
import { validation } from '../src/index.js'

describe('Validation Tests', () => {

    it("isHttpsUri - Correct uri" ,() => {
        chai.expect(validation.isHttpsUri("https://example.com") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com/") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com/test") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com/test.phph") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com:8001") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com:8001/") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com:8001/test") ).to.be.true;
        chai.expect(validation.isHttpsUri("https://example.com:8001/test.phph") ).to.be.true;
    });

    it("isHttpsUri - Incorrect uri" ,() => {
        chai.expect(validation.isHttpsUri("http://example.com") ).to.be.false;
        chai.expect(validation.isHttpsUri("file://example.com") ).to.be.false;
        chai.expect(validation.isHttpsUri(this) ).to.be.false;
    });

    it("isGuid" ,() => {
        chai.expect(validation.isGuid("879fdc97-0ff1-4c03-90ec-74de6d53290e") ).to.be.true;
        chai.expect(validation.isGuid("879fdc97-0ff1-4c03-90ec-74de6d5290e") ).to.be.false;
        chai.expect(validation.isGuid(this) ).to.be.false;
        chai.expect(validation.isGuid("00000000-0000-0000-0000-000000000000") ).to.be.false;
        chai.expect(validation.isGuid("00000000-0000-0000-0000-000000000000",true) ).to.be.true;
    });

    it("hasValue" ,() => {
        chai.expect(validation.hasValue("879fdc97-0ff1-4c03-90ec-74de6d53290e") ).to.be.true;
        chai.expect(validation.hasValue(null)).to.be.false;
        chai.expect(validation.hasValue(undefined)).to.be.false;
    });

    it("hasMinLength - allowEmpty = true" ,() => {
        chai.expect(validation.hasMinLength("version",3)).to.be.true;
        chai.expect(validation.hasMinLength("ver",3)).to.be.true;
        chai.expect(validation.hasMinLength("ve",3)).to.be.false;
        chai.expect(validation.hasMinLength("v",3)).to.be.false;

        chai.expect(validation.hasMinLength("",3)).to.be.true;
        chai.expect(validation.hasMinLength(null,3)).to.be.true;
        chai.expect(validation.hasMinLength(undefined,3)).to.be.true;
    });

    it("hasMinLength - allowEmpty = false" ,() => {
        chai.expect(validation.hasMinLength("version",3, false)).to.be.true;
        chai.expect(validation.hasMinLength("ver",3, false)).to.be.true;
        chai.expect(validation.hasMinLength("ve",3, false)).to.be.false;
        chai.expect(validation.hasMinLength("v",3, false)).to.be.false;

        chai.expect(validation.hasMinLength("",3, false)).to.be.false;
        chai.expect(validation.hasMinLength(null,3, false)).to.be.false;
        chai.expect(validation.hasMinLength(undefined, 3, false)).to.be.false;
    });

    it("hasMaxLength" ,() => {
        chai.expect(validation.hasMaxLength("version",3)).to.be.false;
        chai.expect(validation.hasMaxLength("vers",3)).to.be.false;
        chai.expect(validation.hasMaxLength("ver",3)).to.be.true;
        chai.expect(validation.hasMaxLength("ve",3)).to.be.true;
        chai.expect(validation.hasMaxLength("v",3)).to.be.true;

        chai.expect(validation.hasMaxLength("",3)).to.be.true;
        chai.expect(validation.hasMaxLength(null)).to.be.true;
        chai.expect(validation.hasMaxLength(undefined)).to.be.true;
    });

    it("hasLength - allowEmpty = true" ,() => {
        chai.expect(validation.hasLength("version ",3,7)).to.be.false;
        chai.expect(validation.hasLength("version",3,7)).to.be.true;
        chai.expect(validation.hasLength("ver",3,7)).to.be.true;
        chai.expect(validation.hasLength("ve",3,7)).to.be.false;
        chai.expect(validation.hasLength("v",3,7)).to.be.false;

        chai.expect(validation.hasLength(null,3,7)).to.be.true;
        chai.expect(validation.hasLength(undefined,3,7)).to.be.true;
        chai.expect(validation.hasLength("",3,7)).to.be.true;

    });

    it("isEmail - simple test", () => {
        chai.expect(validation.isEmail("isEmail")).to.be.false;
        chai.expect(validation.isEmail("text@example")).to.be.false;
        chai.expect(validation.isEmail("text@example.com")).to.be.true;
    });

    it("validate - regexp rule", () => {
        const rules = [ { prop: "name", name: "regexp", regexp: /^[a-z-]+$/ }, ];
        
        let result = validation.validate({ name : "aaz" }, rules);
        chai.expect(result.hasErrors).to.be.false;

        result = validation.validate({ name : "aaz1" }, rules);
        chai.expect(result.hasErrors).to.be.true;

        result = validation.validate({ name : "aaz-" }, rules);
        chai.expect(result.hasErrors).to.be.false;

        result = validation.validate({ name : ",aaz" }, rules);
        chai.expect(result.hasErrors).to.be.true;
    })

});