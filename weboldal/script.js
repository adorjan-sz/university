function toggleStylesheet() {
    var defaultStylesheet = document.getElementById("defaultStylesheet");
    var alternateStylesheet = document.getElementById("alternateStylesheet");
    var textSizeButton = document.getElementById("textSizeButton");

    if (alternateStylesheet.disabled) {
        defaultStylesheet.disabled = true;
        alternateStylesheet.disabled = false;
        textSizeButton.textContent = "KisBetű";
    } else {
        defaultStylesheet.disabled = false;
        alternateStylesheet.disabled = true;
        textSizeButton.textContent = "NagyBetű";
    }
}