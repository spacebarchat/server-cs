String.prototype.replaceAt = function (index, replacement) {
    return this.substring(0, index) + replacement + this.substring(index + 1);
}

var legacyRights = 0n;
var modernRights = '0'.repeat(document.getElementsByTagName("input").length);
var configRights = {};

function calculate(a) {
    console.log(a)
    legacyRights += a.checked ? (1n << BigInt(a.value)) : -(1n << BigInt(a.value))
    modernRights = modernRights.replaceAt(a.value, a.checked ? '1' : '0')
    configRights[a.name] = a.checked
    document.getElementById("legacyRights").innerText = "Legacy rights (fosscord-server-ts): " + legacyRights
    document.getElementById("modernRights").innerText = "User rights: " + modernRights
    document.getElementById("configRights").value = JSON.stringify(configRights, null, 4)
}