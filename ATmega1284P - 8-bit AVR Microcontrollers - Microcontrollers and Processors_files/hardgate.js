/*
This file is common js side click handler for all gated asset links. it has to be embedded in any application with in www which needs asset to be gated.
It mainly does these:
1.	On dom ready insert a div with iframe pointing to www register default page(login page)
2.	On dom ready look for all links which are having name attr with SoftGate/HardGate in it and binds a common click handler
3.	On click on those links by user, it invokes common handler which check if user logged in by login related cookies and then opens popup if not logged in.
4.	When popup opens, then dom ready on the page in popup, we style/resize page/iframe to fit to popup.
5.	When user clicks login with invalid details, he remains on page with login errors shown, but if logs in with valid one, popup closes.
6.	For soft gated links, on click first download is started and at the similar time popup is shown. For hard gate first popup is shown then on login/register, download starts and popup closes.s
7. on widnow resize of browser it adjusts popup size, also adjusts for different device resolutions
*/

var wwwRegisterUrl = location.protocol + "//" + location.host + "/wwwregister";
//remove before release prachi
//wwwRegisterUrl =  "http://localhost/wwwregister";

var loginPageName = 'GatedLogin.aspx';

//var currentPopupUrl = wwwRegisterUrl + '/' + loginPageName + '?GatingType=SoftGate';
//var currentGatingType = 'SOFTGATE';

var currentfavWindow;
var isFirstLoad = true;

var currentAssetName, currentAssetLink;
var isUserOnRegisterPage = false;
var signinText = "Please sign in or register if you want to add the selection to preference list on myMicrochip";

//delay for download to start, so that user can see login popup for moment before downlaod starts
var downloadDelay = 6000;
var SelectedproductYN = "";
var SelectedproddocYN = "";

var pdocid = "";
function gatedLinkClickHandlerSetup() {
    //prachi commented
    //var softGatedLinks = $("[name*='" + "SoftGate" + "']");
    var hardGatedLinks = $("[name*='" + "HardGate" + "']");


    //assign click handler for each gated links passing current href as param
    var assignClickhandler = function (index, item) {
        $(item).on('click', {
            href: $(item).attr('href')
        }, openPopupfromproductpage);//prachi changed it from openPopup to openPopupfromproductpage
    };

    //softGatedLinks.each(assignClickhandler);
    hardGatedLinks.each(assignClickhandler);

    //softGatedLinks.contextmenu(disableRightClick);
    hardGatedLinks.contextmenu(disableRightClick);

    //hide hard gated links only, again only if user is not logged in
    if (readCookie('wwwmicrochipuser') == null || readCookie('wwwmicrochipuser') == undefined) {

        //clear out href so user can't see it on mouse hover or can't copy paste url
        //store current one in data-href, and then hide current one
        hardGatedLinks.attr('data-href', hardGatedLinks.attr('href'));
        hardGatedLinks.attr('href', '#');
    }
}
//prachi added to differntiate the click for favorite product or Productdocument
function openPopupforproductDoc(did) {

    $(this).addClass('imageyellowstar')
    pdocid = did;
    SelectedproductYN = "";
    SelectedproddocYN = "yes";
    openPopupfromproductpage()

}
//prachi added to differntiate the click for favorite product or Productdocument
function openPopupforproduct() {


    //devtooltype = "devtool";
    SelectedproductYN = "Yes";
    SelectedproddocYN = "";
    openPopupfromproductpage()
}
function disableRightClick(event) {

    //disbale only if not logged in
    if (readCookie('wwwmicrochipuser') == null || readCookie('wwwmicrochipuser') == undefined) {
        event.preventDefault();
        alert('Right click is disabled for this asset! You can still download via direct/left click.');
    }
}

function onLoginPopupClose() {


    closePopup();
    if (SelectedproductYN) {
        SaveDataProduct(); //Prachi added to do the postlogin work to save the data
    }
    if (SelectedproddocYN) {

        // alert(pdocid);
        SaveProductDoc(pdocid);
    }

}

function closePopup() {

    //clear old content
    //$("#window").html("");//prachi renamed it as below
    $("#windowhardgateproductpage").html("");


    //click close manually, if fails ignore, as that means close is done
    try {
        var loginPopup = currentfavWindow.data("kendoWindow");
        loginPopup.close();
    }
    catch (error) {
        try {
            $('.k-widget.k-window').find('.k-icon.k-i-close').click();
        }
        catch (error) {//ignore if further error...
        }
    }
}

// on load  event for kendo wondow containing iframe to show www login/register pages
// does styling per page user is on
function ApplyCustomStyleToLoginRegister(iframe) {

    //first focus out from current focus on iframe
    //as this method is called after iframe is refreshed hence got focus and brings up android keyboard
    document.activeElement.blur();

    var currentPopupURL = currentfavWindow.data("kendoWindow").options.content;
    isUserOnRegisterPage = currentPopupURL.indexOf('RegisterStep1.aspx') > 0;

    isUserOnRegisterPage = $('.k-widget.k-window').find('iframe')[0].contentWindow.location.href.indexOf('RegisterStep1.aspx') > 0;

    if (!isUserOnRegisterPage) {
        positionPopupWindow(false, true, false);
    } else {
        //align label divs to fit with other input fields
        var iframeContent = $('.k-content-frame').contents();
        iframeContent.find('.row:last div.twelve.input-field:lt(7)').find('div.three').html('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;');

        positionPopupWindow(false, false, false);
    }

    DelayCustomStyling();
}

// this method invoke styling methods per page displayed
function DelayCustomStyling() {

    if (!isUserOnRegisterPage) {
        StyleLoginPage();
    } else {
        StyleRegisterPage();
    }
}

function StyleRegisterPage() {

    var iframeContent = $('.k-content-frame').contents();

    iframeContent.find('.row:last div.twelve.input-field:lt(7)').find('div.three').html('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;');
    iframeContent.find('[id$=selCountry]').addClass('width_textbox');

    // set event to assign pwd field value to hidden retype pwd field so that form on submission remains valid
    iframeContent.find('[id$=txtPassword1]').blur(function () {
        iframeContent.find('[id$=txtPassword2]').val(iframeContent.find('[id$=txtPassword1]').val());
    });

    //set all text fields with text placeholder attr
    iframeContent.find('[id$=txtEmail]').attr("placeholder", "Email Address");
    iframeContent.find('[id$=txtPassword1]').attr("placeholder", "Password");
    iframeContent.find('[id$=txtFirstName]').attr("placeholder", "First Name");
    iframeContent.find('[id$=txtLastName]').attr("placeholder", "Last Name");
    iframeContent.find('[id$=txtEmployerName]').attr("placeholder", "Company Name");
    iframeContent.find('[id$=txtAddressLine1]').attr("placeholder", "AddressLine1");
    iframeContent.find('[id$=txtAddressLine2]').attr("placeholder", "AddressLine2");
    iframeContent.find('[id$=txtCity]').attr("placeholder", "City");
    iframeContent.find('[id$=Uc_state1_txtState]').attr("placeholder", "State");
    iframeContent.find('[id$=txtPostalCode]').attr("placeholder", "PostalCode");

    iframeContent.find('[id$=lnkNextStep]').width(iframeContent.find('[id$=divSecurityCheck]').width());

    // TODO - keeping it for future usage, as currently its on server side, but if that has any issues, then will rollback below ones
    /*
        //iframeContent.find('[id$=divLblCheckLabel]').removeClass('three').addClass('five');
        //iframeContent.find('[id$=divSecurityTextError]').removeClass('five').addClass('four');
    
        //iframeContent.find('[id$=divLblEmailOptIn]').remove();
        //iframeContent.find('[id$=divCheckEmailOptIn]').removeClass('nine').addClass('twelve');
    
        //iframeContent.find('[id$=divLblRemember]').remove();
        //iframeContent.find('[id$=divCheckRemember]').removeClass('nine').addClass('twelve');
    */

    setRegisterButtons();

    // enable back save button and after first click again disable until page loads back
    iframeContent.find('a[id$=lnkNextStep]').on("click", toggleButtonDisabling);

    //disable save button on country change until page is fully loaded back
    iframeContent.find('select[id$=selCountry]').on("change", toggleButtonDisabling);

    // align state field for better fit in popup
    iframeContent.find('span[id$=state1_RequiredFieldValidator3]').css('margin-left', '0px');

    ShowPopup();
}

function StyleLoginPage() {
    //use below selector as root if iframe set as false for kendo window, else use next one
    //iframeContent = $('.k-widget.k-window');//.find('header')
    var iframeContent = $('.k-content-frame').contents();

    iframeContent.find('body').css("overflow", "visible");

    //set sign in text per gated link type and asset name
    var SignInMsg = iframeContent.find('[id$=dvPromoMessage]');
    var signInLabel = iframeContent.find('[id$=Login1_lblLogin]');
    SignInMsg.css('color', 'red').text(signinText.replace('<<AssetName>>', currentAssetName)).css('padding-bottom', '5px').insertAfter(signInLabel).show();

    iframeContent.find('div#maincol div.row:first').css('min-height', '400');
    //$('.k-widget.k-window').find('.k-icon.k-i-close').css("margin-top", "0px");







    ShowPopup();

    setLoginButtons();


}

// gated link click handler
var gatingType;
//prachi changed the name from openPopup to openPopupfromproductpage
function openPopupfromproductpage() {



    if (readCookie('wwwmicrochipuser') == null || readCookie('wwwmicrochipuser') == undefined) {

        var tempAssetNameForURL = "prodfav";
        // gatingType = $(this).attr('name').trim();
        gatingType = "HardGate";

        // currentGatingType = gatingType;

        //  currentAssetName = ($(this).attr('title') != undefined) ? $(this).attr('title').trim() : $(this).text().trim();
        //  currentAssetLink = event.data.href;

        //change iframe url based on link's gating type...
        // var tempAssetNameForURL = currentAssetName.replace(/</g, "&lt;").replace(/>/g, "&gt;");//encode < and >, and decode back at server
        // tempAssetNameForURL = encodeURIComponent(tempAssetNameForURL);

        var currentUrlToOpen = wwwRegisterUrl + '/' + loginPageName + '?GatingType=' + gatingType + "&prodfav=true";

        // if user not logged in then invoke popup display flow, else download will start automatically
        //if (readCookie('wwwmicrochipuser') == null || readCookie('wwwmicrochipuser') == undefined)
        //{

        //if (gatingType.trim().toUpperCase() == "HARDGATE") {
        //  signinText = "Please sign in or register to download <<AssetName>>";

        // disable default behavior as we will handle download via code
        // event.preventDefault();
        // event.stopPropagation();
        // }

        currentfavWindow.data("kendoWindow").setOptions({
            content: currentUrlToOpen
        });

        //force refresh method to call for new url change
        currentfavWindow.data("kendoWindow").refresh({
            url: currentUrlToOpen
        });
        // }
    }
}

//prachi changed the name
function handlePostLoginDownloadfromproductpage(isLoggedIn) {


    //window.parent.location.reload();//prachi added
    //PostLoginRegisterCommonHandlerproductpage(isLoggedIn, true);
    //window.parent.location.SaveData();
}

function handlePostRegistrationDownload(isRegistered) {
    PostLoginRegisterCommonHandler(isRegistered, false);
}

// method which is invoked from post +ve login/register submit
// it will close popup, and continue download for hard gated links
//prachi changed the name
function PostLoginRegisterCommonHandlerproductpage(isLogInRegisterSucceeded, isInvokedFromLogin) {

    if (isLogInRegisterSucceeded.toUpperCase() == 'TRUE') {

        onLoginPopupClose();//close current login popup always


        ////replace content with success message and then show it back to user
        // showSuccessMsg();

        //setTimeout(showLinksForHardGates, 0);//unhinde href for hard gated links

        //setTimeout(onLoginPopupClose, 2000);//close success message popup

        //// if current gated link clicked is of hard gate type then download its' links via program
        //if (currentGatingType.toUpperCase() == "HARDGATE") {
        //    setTimeout(function () {
        //        setup();
        //        //downloadInSameWindow();
        //    }, 2200);
        //}
    }
}

function downloadInSameWindow() {
    window.location.href = currentAssetLink;
}

//post login/register - show hard gated links back, which were hidden initially as user was not logged in
function showLinksForHardGates() {

    var hardGatedLinks = $("[name*='" + "HardGate" + "']");

    //showBackLinks for each hard gated links
    var showBackLinks = function (index, item) {
        $(item).attr('href', $(item).attr('data-href'));
    };

    hardGatedLinks.each(showBackLinks);
}

//function showSuccessMsg() {

//    //first hide existing popup
//    var loginPopup = currentfavWindow.data("kendoWindow");
//    loginPopup.close();

//    //now replace content and show
//    var contentMsg = '<div id="divProductpageSuccessMsg" style="text-align:center"><div><br/><b>Login/Registration was Successful and Prefernce saved on myMicrochip.</b><br/>{{downloadMsgPlaceholder}}<br/></div></div>';

//    //var contentMsg = '<div id="divSuccessMsg" style="text-align:center"><div><br/><b>Login/Registration was Successful.</b><br/>{{downloadMsgPlaceholder}}<br/></div></div>';

//    // if its hard gate asset, then show download message as well
//    if (currentGatingType.toUpperCase() == "HARDGATE") {
//        contentMsg = contentMsg.replace('{{downloadMsgPlaceholder}}', '<br/><b>Your download will start shortly...</b>');
//    }
//    //else {
//    //    contentMsg = contentMsg.replace('{{downloadMsgPlaceholder}}', '');
//    //}//prachi commented

//    //$("#window").html(contentMsg);


//    $("#windowhardgateproductpage").html(contentMsg);


//    loginPopup.setOptions({
//        width: 300,
//        height: 120
//    });

//    loginPopup.center().open();
//}

function downloadAsset() {

    var link = document.createElement('a');
    $(link).attr('target', "_blank");

    //self.focus();
    var e = window;
    while (e.frameElement !== null) {
        e = e.parent;
    }

    document.body.appendChild(link);
    link.href = currentAssetLink;
    link.click();

    e.focus();
    e.parent.focus();

}

function readCookie(name) {
    var nameEQ = encodeURIComponent(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return decodeURIComponent(c.substring(nameEQ.length, c.length));
    }
    return null;
}

// this will insert kendo window as div into parent application's current page
function addLoginPopupToDOM() {

    //prachi changed the window name
    var kendoPopupHtml = '<div id="windowhardgateproductpage" style="display: none;overflow: hidden;" class="row" padding-top: 0px"></div>';
    $(kendoPopupHtml).appendTo(document.body);

    currentfavWindow = $("#windowhardgateproductpage");//prachi

    // if its undefined, then don't invoke
    if (currentfavWindow.kendoWindow != undefined) {
        setKendoWindow();
    }
}
function onDeactivate() {
    hideKeyBoard();
}
function hideKeyBoard() {
    //hide soft keyboard in android devices
    document.activeElement.blur();
}
function setKendoWindow() {
    currentfavWindow.kendoWindow({
        iframe: true,
        title: 'Login/Register',
        draggable: true,
        width: "50%",
        height: "50%",
        modal: true,
        resizable: false,
        visible: false,
        pinned: false,
        position: {
            top: 100,
            left: 100
        },
        actions: [
            "Close"
        ],
        deactivate: onDeactivate,
        refresh: ApplyCustomStyleToLoginRegister
    });
}

// it is called on popup open before popup is shown to user
// to postion popup and resize it per page size and device resolution
function positionPopupWindow(isInvokedOnWindowResize, isLoginScreen, shallOpen) {

    var height = $(window).height();
    var width = $(window).width();
    var loginPopup = currentfavWindow.data("kendoWindow");

    var popHeight = 500;
    var popWidth = 500;

    if (isLoginScreen == null) {
        var isLoginScreen = !isUserOnRegisterPage;
    }

    // decide right width and height for popup based on current resolution by using form factor or ratio based calculations
    if (isLoginScreen) {
        if (width > 1006) {
            popHeight = height / 1.1;
            popWidth = 500;
        }
        else if (width > 622) {
            popHeight = height / 1.1;
            popWidth = 400;
        }
        else if (width > 450) {
            popHeight = height / 1.1;
            popWidth = 370;
        }
        else if (width > 302) {
            popHeight = height / 1.15;
            popWidth = 250;
        }
        else {
            popHeight = height / 1.15;
            popWidth = 200;
        }
    } else {
        if (width > 1006) {
            popWidth = 800;
        }
        else if (width > 622) {
            popWidth = 550;
        }
        else if (width > 450) {
            popWidth = 370;
        }
        else if (width > 302) {
            popWidth = 250;
        }
        else {
            popWidth = 200;
        }

        popHeight = height / 1.1;
    }

    if (isInvokedOnWindowResize) {
        loginPopup.setOptions({
            width: popWidth,
            height: popHeight
        });

        loginPopup.center();
    } else {
        if (shallOpen) {
            loginPopup.setOptions({
                width: popWidth,
                height: popHeight
            });
            loginPopup.center().open();
        } else {
            loginPopup.setOptions({
                width: popWidth,
                height: popHeight
            });
            loginPopup.center();
        }
    }

    // if by now popup is not opened, open it
    if (!isInvokedOnWindowResize && currentfavWindow.data("kendoWindow") && (currentfavWindow.data("kendoWindow").element.is(":hidden"))) {
        loginPopup.open();
    }

    if (isLoginScreen) {
        setLoginButtons();
    }
    else {
        setRegisterButtons();
    }
}

function ShowPopup() {
    var loginPopup = currentfavWindow.data("kendoWindow");
    loginPopup.center().open();
}

var firstTimeLoad = true;
$(document).ready(function () {

    // only run this domready logic if current page is not www register page, 
    // as for www register pages also this js file gets added from headerfooter but shall not run any dom ready logic
    if ((document.location.pathname.indexOf('wwwregister') == -1) && firstTimeLoad) {

        //firstTimeLoad = false;

        firstTimeLoad = true;
        // uncomment below when testing gated links from www products
        //$('#tblDocumentation a:eq(0)').attr('name', ' SoftGate             ').text('MPLAB<sup>®</sup> XC Compiler Functional Safety Manual-SoftGated').attr('title', 'MPLAB<sup>®</sup> XC Compiler Functional Safety Manual-SoftGated');
        //$('#tblDocumentation a:eq(2)').attr('name', '         HardGate ').text('MPLAB<sup>®</sup> XC Compiler Functional Safety Manual-HardGated');

        //$('#tblDocumentation a:eq(4)').attr('name', ' SoftGate             ').attr('href','https://download.mozilla.org/?product=firefox-stub&os=win&lang=en-US').attr('title', 'PIC10F200/202/204/206 6-Pin, 8-Bit Flash Microcontrollers Data Sheet');
        //$('#tblDocumentation a:eq(7)').attr('name', ' HardGate             ').attr('href','https://download.sysinternals.com/files/ProcessExplorer.zip').attr('title', 'PIC10F200/202/204/206 6-Pin, 8-Bit Flash Microcontrollers Data Sheet');
        //$('#tblDocumentation a:eq(9)').attr('name', ' SoftGate             ').attr('href','http://techslides.com/demos/samples/sample.rar').attr('title', 'PIC10F200/202/204/206 6-Pin, 8-Bit Flash Microcontrollers Data Sheet');

        //after this call, as dom will get modfied so dom ready will get fired again, 
        //where in we will stop running it with firstTimeLoad flag.
        addLoginPopupToDOM();

        gatedLinkClickHandlerSetup();

        $(window).resize(function () {
            // if window is opened then adjust to resized size, else ignore
            if (currentfavWindow.data("kendoWindow") && !(currentfavWindow.data("kendoWindow").element.is(":hidden"))) {
                var currentframecontent = $('.k-content-frame').contents();
                var currentFocusedElement = currentframecontent.find(":focus");
                var elementHasFocus = $('.k-content-frame').contents().find(":focus").length > 0;

                positionPopupWindow(true, null);

                // for android apps, when keyoboard comes up, window resizes, and hence textbox loses focus post calling "positionpopupwindow" in resize event 
                // hence need to focus text field back, for login and register both setting focus on first text field post keyoboard is shown
                if (elementHasFocus && currentFocusedElement.is("input")) {
                    currentFocusedElement.focus();
                }
            }
        });
    }
});

var saveClickCount = 0;

function toggleButtonDisabling(e) {

    var isEnabled = $(this).css("pointer-events") == "auto" || $(this).attr('disabled') === undefined;

    // prevent default only if its invoked on save button click, not on country ddl change
    if ((isEnabled == "true" && saveClickCount > 0) && e.target.id.indexOf('selCountry') == -1) {
        e.preventDefault();
        enableSaveButton(false);
    } else {
        enableSaveButton(false);
    }

    saveClickCount++;
}

function enableSaveButton(shallEnable) {
    var button = $('.k-content-frame').contents().find('a[id$=lnkNextStep]');
    if (shallEnable) {
        button.removeAttr("disabled").addClass('enabled').removeClass('disabled');

        saveClickCount = 0;
    } else {
        button.attr("disabled", "disabled").addClass('disabled').removeClass('enabled');
    }
}

// method to resize buttons on register per screen fit
function setRegisterButtons() {
    var loginPopup = $('.k-content-frame').contents();


    // enable back save button as page has reloaded
    enableSaveButton(true);
}

// method to resize buttons on login per screen fit
var registerButtonOriginalHeight = -1;
var registerButtonOriginalWidth = -1;

function setLoginButtons() {

    var loginPopup = $('.k-content-frame').contents();

    var height = $(window).height();
    var width = $(window).width();

    // if its first resize call, then save current sizes for future resizing events firing
    if (registerButtonOriginalHeight === -1)
        registerButtonOriginalHeight = loginPopup.find('[id$=Login1_Register]').height();

    if (registerButtonOriginalWidth === -1)
        registerButtonOriginalWidth = loginPopup.find('[id$=Login1_Register]').width();

    var newHeight;
    // if it is low end device, set buttons accordingly to lower size
    if (width <= 768 && width > 500) {
        newHeight = registerButtonOriginalHeight * 1.25;
    }
    else if (width < 500) {
        newHeight = registerButtonOriginalHeight * 1.3;
    }

    loginPopup.find('[id$=Login1_Register]').height(newHeight + 'px');
    loginPopup.find('[id$=Login1_Register]').width('100%');

    loginPopup.find('[id$=Login1_LinkButton1]').width(loginPopup.find('[id$=Login1_Register]').width());
}
