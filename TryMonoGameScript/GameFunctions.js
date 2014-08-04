
var updateFunctionCounter = 0;
updateFunctions = [];
function registerUpdateFunction(object) {
    updateFunctions[updateFunctionCounter] = object;
    updateFunctionCounter++;
}

function Update(gametime) {
    for (var x = 0; x < updateFunctions.length; x++) {
        updateFunctions[x].update(gametime);

    }
}