import chai from 'chai';
import * as patcher from '../src/patch.js';

describe('Patch Tests', () => {
    it("patch - simple test", async () => {

        const source = { name : "a" };
        const patch = { name : "b" };

        const result = patcher.patch(source, patch);
        chai.expect(result.name).to.equal(patch.name);
    });

    it("patch - souce multi prop", async () => {

        const source = { name : "a", goal : 1 };
        const patch = { name : "b" };

        const result = patcher.patch(source, patch);
        chai.expect(result.name).to.equal(patch.name);
        chai.expect(result.goal).to.equal(source.goal);
    });    

    it("patch - multi prop", async () => {

        const source = { name : "a", goal : 1 };
        const patch = { name : "b", goal : 2 };

        const result = patcher.patch(source, patch);
        chai.expect(result.name).to.equal(patch.name);
        chai.expect(result.goal).to.equal(patch.goal);
    });   

    it("patch - target multi prop", async () => {

        const source = { name : "a" };
        const patch = { name : "b", goal : 2 };

        const result = patcher.patch(source, patch);
        chai.expect(result.name).to.equal(patch.name);
        chai.expect(result.goal).to.not.exist;
    });   

    it("patch - exclded", async () => {

        const source = { name : "a", goal : 1 };
        const patch = { name : "b", goal : 2 };

        const result = patcher.patch(source, patch, ["goal"]);
        chai.expect(result.name).to.equal(patch.name);
        chai.expect(result.goal).to.equal(source.goal);
    });   
});