var spinner = document.getElementById('spinner');

function showSpinner() {
    spinner.style.display = 'flex';
}

function hideSpinner() {
    spinner.style.display = 'none';
}

showSpinner();

window.onload = function () {
    hideSpinner();
};





