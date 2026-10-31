const { superFunctionalStrings } = require('./solution.js');
const data = require('fs').readFileSync(0, 'utf8').trim();
console.log(superFunctionalStrings(data.split('\n')[0]));
