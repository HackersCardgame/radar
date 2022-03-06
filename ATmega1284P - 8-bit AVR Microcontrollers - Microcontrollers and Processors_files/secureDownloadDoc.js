/*
This file is used to download secure documents.
*/
var secureAssetLink;
var sGatingType;

function downloadSecureDocFromProductPage(url) {
    if (readCookie('wwwmicrochipuser') != null || readCookie('wwwmicrochipuser') != undefined) {
        window.location.href = url;
    }
    else {
        openPopupForSecureDownloadDocfromproductpage(url);
    }
}

function openPopupForSecureDownloadDocfromproductpage(url) {
    if (readCookie('wwwmicrochipuser') == null || readCookie('wwwmicrochipuser') == undefined) {
        sGatingType = "HardGate";
        secureAssetLink = url;
        //gating type parameter is required to change look and feel of pop-up window
        var currentUrlToOpen = wwwRegisterUrl + '/' + loginPageName + '?GatingType=' + sGatingType + "&SecureDownLoad=yes";

        currentfavWindow.data("kendoWindow").setOptions({
            content: currentUrlToOpen
        });

        //force refresh method to call for new url change
        currentfavWindow.data("kendoWindow").refresh({
            url: currentUrlToOpen
        });
    }
}

function handlePostLoginSecureDownload(isLoggedIn) {
    PostLoginSecureDownloadDocCommonHandler(isLoggedIn);
}

function handlePostRegistrationSecureDownload(isRegistered) {
    PostRegistrationSecureDownloadDocCommandHandler(isRegistered);
}

function PostLoginSecureDownloadDocCommonHandler(isInvokedFromLogin) {
    if (isInvokedFromLogin.toUpperCase() == 'TRUE') {
        closePopup();//calls this function from Hardgate.js file

        // if current gated link clicked is of hard gate type then download its' links via program
        if (sGatingType.toUpperCase() == "HARDGATE") {
            setTimeout(function () {
                window.location.href = secureAssetLink;
            }, 2200);
        }
    }
}
function PostRegistrationSecureDownloadDocCommandHandler(isLogInRegisterSucceeded) {
    if (isLogInRegisterSucceeded.toUpperCase() == 'TRUE') {
        closePopup();//calls this function from Hardgate.js file

        // if current gated link clicked is of hard gate type then download its' links via program
        if (sGatingType.toUpperCase() == "HARDGATE") {
            setTimeout(function () {
                window.location.href = secureAssetLink;
            }, 2200);
        }
    }
}