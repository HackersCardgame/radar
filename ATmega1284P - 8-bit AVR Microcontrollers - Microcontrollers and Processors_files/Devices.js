/// <reference path="~/Includes/jquery-vsdoc.js" />


//*********beggining of: bubble code - Lance Version Modified 5-16-2012*******************
// Added the callout classes and divs to pop up the bubble relative to the td tag

//--indicates the mouse is currently over a div
var onDiv = false;
//--indicates the mouse is currently over a link
var onImg = false;
//--indicates that the bubble currently exists
var bubbleExists = false;
//--this is the ID of the timeout that will close the window if the user mouseouts the link
var timeoutID;

var ln = 'en';

function LaunchInCenter(url, h, w, opts) {
    var height = h, width = w;
    var str = "height=" + height + ",innerHeight=" + height;
    str += ",width=" + width + ",innerWidth=" + width + "," + opts;

    if (window.screen) {
        var ah = screen.availHeight - 30;
        var aw = screen.availWidth - 10;

        var xc = (aw - width) / 2;
        var yc = (ah - height) / 2;

        str += ",left=" + xc + ",screenX=" + xc;
        str += ",top=" + yc + ",screenY=" + yc;
        str += ",resizable=yes,scrollbars=yes";

    }
    window.open(url, '', str);

}

//use imgAddToCartClick event handler to call after replacing tblDocumentation table with new language data
function imgAddToCartClick() {
    var cur = this;
    var dlCount = 0;

    dlCount = parseInt($('#lblCartCount').html());

    if (cur.src.match(/add.png/) != null) {
        this.src = 'Images/ajax_wait.gif';
        $.get('UpdatCart.aspx', { RowNum: this.id, Add: 'y', TotalDocs: '', bbDeviceID: getParameterByName('dDocName') },
							function (data) {
							    if (data == 'Error') {
							        cur.src = 'Images/add.png';
							        $(cur).attr("Title", "Error, Please try again.");
							    }
							    else {
							        cur.src = 'Images/remove.png';
							        cur.alt = 'Remove from Document Cart';
							        $(cur).attr("Title", "Remove from cart");
							        dlCount = dlCount + 1;
							        $('#lblCartCount').html(data);
							    }
							    $("#dvMiniCart").html("");
							});
    }
    else if (cur.src.match(/remove.png/) != null) {
        this.src = 'Images/ajax_wait.gif';
        $.get('UpdatCart.aspx', { RowNum: this.id, Add: 'n', bbDeviceID: getParameterByName('dDocName') },
							function (data) {
							    if (data == 'Error') {
							        cur.src = 'Images/remove.png';
							        $(cur).attr("Title", "Error, Please try again.");
							    }
							    else {
							        cur.src = 'Images/add.png';
							        cur.alt = 'Add To Document Cart';
							        $(cur).attr("Title", "Add to cart");
							        dlCount = dlCount - 1;
							        $('#lblCartCount').html(data);
							    }

							    $("#dvMiniCart").html("");
							});
    }
};


function hideBubble() {
    clearTimeout(timeoutID);
    //--if the mouse isn't on the div then hide the bubble
    if (bubbleExists && !onDiv) {
        $("#bubbleWrap").remove();

        bubbleExists = false;
    }
}

function showBubble(event) {

    if (bubbleExists)
        hideBubble();

    var parentRow = $(this).parents("tr:first");
    var parentCell = $(this).parents("td:first");
    var tPosX = -185;  //get parent row width
    var tPosY = -40; //get parent td top
    var divPopupDataCtrl = $(parentRow).find('input.hdnLinks:first');

    //create div tag and add it to any dom element
    var divPopUp = $('<div class="calloutWrapper" ID="bubbleWrap"><div class="border-callout calloutLeft" ID="bubbleID" style="display:block"><div class="calloutDesc">' + divPopupDataCtrl.val() +
                '<b class="border-notch notch"></b><b class="notch"></b></div></div></div>').mouseenter(keepBubbleOpen).mouseleave(letBubbleClose).appendTo(parentCell);

    $(divPopUp).find("#bubbleID").find("table").addClass("tblPopup");
    $('#bubbleID').animate({ top: tPosY, left: tPosX }, { queue: false, duration: 0 });

    bubbleExists = true;

}

function keepBubbleOpen() {
    onDiv = true;
}

function letBubbleClose() {
    onDiv = false;

    hideBubble();
}

function setupImgHover() {

    $('#tblDocumentation').find('.docLinkImgType').hover(
        function (event) {
            if (onDiv || onImg) {
                return false;
            }
            onImg = true;

            showBubble.call(this, event);
        },
        function (event) {
            onImg = false;
            timeoutID = setTimeout(hideBubble, 300);
        }
    );
}


//*********end of: bubble code*******************

//*********beggining of: bubble code - Tiempo Version*******************
/*
//--indicates the mouse is currently over a div
var onDiv = false;
//--indicates the mouse is currently over a link
var onImg = false;
//--indicates that the bubble currently exists
var bubbleExists = false;
//--this is the ID of the timeout that will close the window if the user mouseouts the link
var timeoutID;

var ln = 'en';



//use imgAddToCartClick event handler to call after replacing tblDocumentation table with new language data
function imgAddToCartClick() {
    var cur = this;
    var dlCount = 0;

    dlCount = parseInt($('#lblCartCount').html());

    if (cur.src.match(/add.png/) != null) {
        this.src = 'Images/ajax_wait.gif';
        $.get('UpdatCart.aspx', { RowNum: this.id, Add: 'y', TotalDocs: '', bbDeviceID: getParameterByName('dDocName') },
							function(data) {
							    if (data == 'Error') {
							        cur.src = 'Images/add.png';
							        $(cur).attr("Title", "Error, Please try again.");
							    }
							    else {
							        cur.src = 'Images/remove.png';
							        cur.alt = 'Remove from Document Cart';
							        $(cur).attr("Title", "Remove from cart");
							        dlCount = dlCount + 1;
							        $('#lblCartCount').html(data);
							    }
							    $("#dvMiniCart").html("");
							});
    }
    else if (cur.src.match(/remove.png/) != null) {
        this.src = 'Images/ajax_wait.gif';
        $.get('UpdatCart.aspx', { RowNum: this.id, Add: 'n', bbDeviceID: getParameterByName('dDocName') },
							function(data) {
							    if (data == 'Error') {
							        cur.src = 'Images/remove.png';
							        $(cur).attr("Title", "Error, Please try again.");
							    }
							    else {
							        cur.src = 'Images/add.png';
							        cur.alt = 'Add To Document Cart';
							        $(cur).attr("Title", "Add to cart");
							        dlCount = dlCount - 1;
							        $('#lblCartCount').html(data);
							    }

							    $("#dvMiniCart").html("");
							});
    }
};

function hideBubble() {
    clearTimeout(timeoutID);
    //--if the mouse isn't on the div then hide the bubble
    if (bubbleExists && !onDiv) {
        $("#bubbleID").remove();

        bubbleExists = false;
    }
}

function showBubble(event) {

    if (bubbleExists)
    hideBubble();

    var parentRow = $(this).parents("tr:first");
    var parentCell = $(this).parents("td:first");
    var tPosX = Math.ceil($(parentRow).width()) + $(this).width() + 5;  //get parent row width
    var tPosY = Math.ceil($(parentRow).offset().top - ($(this).height() / 2)); //get parent td top
    var divPopupDataCtrl = $(parentRow).find('input.hdnLinks:first');

    //create div tag and add it to any dom element
    var divPopUp = $('<div ID="bubbleID" >' + divPopupDataCtrl.val() +
    '</div>').mouseenter(keepBubbleOpen).mouseleave(letBubbleClose).appendTo("body");

    $(divPopUp).find("#bubbleID").addClass("divPopup").find("table").addClass("tblPopup");
    $('#bubbleID').animate({ top: tPosY, left: tPosX }, { queue: false, duration: 0 });

    bubbleExists = true;

}

function keepBubbleOpen() {
    onDiv = true;
}

function letBubbleClose() {
    onDiv = false;

    hideBubble();
}

function setupImgHover() {

    $('#tblDocumentation').find('img.docLinkImgType').hover(
        function(event) {
            if (onDiv || onImg) {
                return false;
            }
            onImg = true;

            showBubble.call(this, event);
        },
        function(event) {
            onImg = false;
            timeoutID = setTimeout(hideBubble, 300);
        }
    );
}
*/
//*********end of: bubble code*******************

function tblDocStyle() {
    try {
        $("#tblDocumentation tr").find("td:eq(0)").attr("width", "74%");

        // give the border-right style for table "tblDocumentation"
        $("#divDocs table tr:eq(1) td.ContentTD").css("border-right", "0px solid #CCCCCC");
        $("#divDocs table tr:eq(1) td.ContentTD").css("border-bottom", "0px solid #CCCCCC");

        $("#divDocs table tr:eq(1) td #tblDocumentation tr.Header").find("td:eq(0)").css("border-right", "2px solid #CCCCCC");
        $("#divDocs table tr:eq(1) td #tblDocumentation tr.GridItem").find("td:eq(3)").css("border-right", "2px solid #CCCCCC");
        $("#divDocs table tr:eq(1) td #tblDocumentation tr.GridAltItem").find("td:eq(3)").css("border-right", "2px solid #CCCCCC");
        $("#divDocs table tr:eq(1) td #tblDocumentation tr").find("td:eq(n-1)").css("border-left", "1px solid #CCCCCC");
    }
    catch (e)
    { }
}


// set the page element styles which are overlapping by header styles.
function SetPageStyles() {
    try {
        //set the styles for tblParams
        $("#dvParams table tr.Content").css({ "border-bottom": "0px", "border-left": "0px" });
        $("#dvParams table tr td.ContentTD").css({ "border-bottom": "0px", "border-left": "0px" });
        //$("#dvParams table tr td table.tblParams").css({ "border-right": "solid 2px #CCCCCC", "border-bottom": "solid 2px #CCCCCC", "border-left": "solid 1px #CCCCCC" });
        $("#dvParams table tr td #tblParams").css({ "border-right": "solid 2px #CCCCCC", "border-bottom": "solid 2px #CCCCCC", "border-left": "solid 1px #CCCCCC" });

        // set the styles for tblFeatures
        $("#dvFeatures table tr.Content").css({ "border-bottom": "0px", "border-left": "0px" });
        $("#dvFeatures table tr td.ContentTD").css({ "border-bottom": "0px", "border-left": "0px" });
        $("#dvFeatures table tr td table.tblFeatures").css({ "border-right": "solid 2px #CCCCCC", "border-bottom": "solid 2px #CCCCCC", "border-left": "solid 1px #CCCCCC" });

        //TODO: Set the styles for Jump links. Spacing b/w Jump link sections.
        //$("table.tblJumpLinks").css({ "cellSpacing": "3px", "cellPadding": "0px", "border":"solid 1px #CCCCCC"});

        // set the li item styles for tblFeatuers. li styles are not working
        $("table#tblFeatures tr.Content td table tr td pre span#lblFeatures.NormalText li").css({ "display": "list-item", "list-style-image": "none", "list-style-type": "disc", "list-style-position": "inside" });

    }
    catch (e) {
    }
}

//****************jQuery function to get querystring values in array
function queryStringHash() {
    var r = {};
    var q = location.search;
    q = q.replace(/^\?/, ''); // remove the leading ?	
    q = q.replace(/\&$/, ''); // remove the trailing & 
    jQuery.each(q.split('&'), function () {
        var key = (this.split('=')[0] + '').toLowerCase();
        var val = this.split('=')[1];
        // ingnore empty values 
        if (val)
            r[key] = val;
    });
    return r;
};


//minize documentation table on img, link click. 
function fnMinTabWrapper() {
    if ($("#divDocs")[0].style.display == "none")
        $("#tabContainer").hide();
    else
        $("#tabContainer").show();
};

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return getParameter("ddocname");
    else
        return results[1];
}
function getParameter(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

// Function to popup the document and make disable the flag once it download completes.
function downloaddoc(link) {

    var cookieVals = document.cookie.split(";");
    var cookieVal;
    //find the cookie and get the value.
    for (var i = 0; i < cookieVals.length; i++)
        if (cookieVals[i].indexOf("startToDownloaddoc") > -1)
            cookieVal = cookieVals[i];

    if (cookieVal.indexOf("startToDownloaddoc='true'") > -1) {
        window.open(link, 'mchpdownload', 'location=0,status=1,scrollbars=1,toolbar=1,resizable=1');
        document.cookie = "startToDownloaddoc='false'"; //to disable to flag for not to downlod the doc again.
    }
    return true;
}

//set the cookie to enable the flag to download the doc.    
function setAutoDownloadDoc() {
    document.cookie = "startToDownloaddoc='true'";
}

$(document).ready(function () {


    //    $('a[href^=http]:not("[href*=://' + document.domain + ']")').click(function() {
    //        var cURL = escape(document.URL);
    //        var tURL = escape($(this).attr("href"));
    //        window.location = "http://www.microchip.com/ad/href.asp?href=" + tURL + "&ref=" + cURL;
    //        return false;
    //    });

    //bookbag styles. popup event handle
    /*$("#lblCart").css("text-decoration", "none");
    $("#hypBasket").css("text-decoration", "none");
    $("#tdBasket").children().unbind();
    $("#tdBasket").hover(function() {
        LoadMiniCart('dvMiniCart', 'trLoadIMG', 'spnCartCount', '');
    }, function() {
        hideCart('tblShoppingCart');
    });

    $(".imgAddToCart").click(imgAddToCartClick);

    //showMenu('', 15, 'Hide');
    */
    setupImgHover();

    //****************create tabs and assign text
    $.each(document.cookie.split(";"), function () {
        if (this != "" && this.toString().indexOf("MCHP=Style=") >= 0) {
            ln = this.toString().substr(this.toString().lastIndexOf("=") + 1, 2)
            return;
        }
    });

    if (ln == null || ln == '')
        ln = 'en';

    try {

        $.each($("#hdnLang").val().split(","), function () {
            if (this == "" || this.toString().indexOf(":") < 0)
                return;
            $("#tabContainer tr").append("<td id='" + this.split(":")[0] + "'" + (ln == this.split(":")[0] ? ' class=Active ' : ' class=Inactive ') + ">" + this.split(":")[1] + "</td>");
        });
    } catch (E) { }

    tblDocStyle();
    SetPageStyles();
    //*********Tab code**************

    //****************this is to control space b/w tabs
    $("#tabContainer").attr("cellPadding", "0");

    //$("#imgDocPlusMinus").click(fnMinTabWrapper);
    $("#lnPlusMinus").click(fnMinTabWrapper);

    $("#tabContainer td").click(function () {

        $('#tabContainer td').removeClass('Active').addClass('Inactive');
        $(this).addClass('Active');

        var qryparams = queryStringHash();
        var divToBeWorkedOn = '#tblDocumentation';
        var webMethod = 'TranslatedDocs.asmx/BuildDocHTML'
        var parameters = {
            dDocName: qryparams['ddocname'],
            lang: this.id
        };
        $("#tabContainer tr").append("<td id='tdimgLoad'><img alt='' src='Images/Loader-Small.gif' /></td>");
        $.ajax({
            type: "Post",
            url: webMethod,
            data: parameters,
            async: true,
            timeout: 30000,
            success: function (msg) {
                if ($.browser.msie)
                    $(divToBeWorkedOn).html(msg.text);
                else
                    $(divToBeWorkedOn).html(msg.firstChild.textContent);

                tblDocStyle();

                $(".imgAddToCart").click(imgAddToCartClick); //bind click event for bookbag functionality
                setupImgHover(); //bind hover event to imgType image
                $("#tdimgLoad").remove();
            },
            error: function (e) {
                $(divToBeWorkedOn).html("Unavailable");
                $("#tdimgLoad").remove();
            }
        });
    });
});

//*********end of: document ready func***********