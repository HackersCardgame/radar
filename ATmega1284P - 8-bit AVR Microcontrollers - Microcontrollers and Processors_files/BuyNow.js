//var deviceFamilyNameForCart;
//var languageForCart = 'en';

$().ready(function () {
    externJsUrl = $('#scriptUrl').attr('url');
    GetProductDetailsFromMD();
    $(".close-button-cart").click(function () {
        $(this).closest("[data-role=window]").data("kendoWindow").close();
    });
    //$(window).resize(function () {
    //    var dialog = $("#divKWindow").data("kendoWindow");
    //    if ($(window).width() > 700)
    //        $(dialog).css('width', '700px');
    //    else {
    //        $(dialog).css('width', $(window).width());
    //        $(dialog).css('position', 'fixed');
    //    }
    //});
});

function GetProductDetailsFromMD() {
    microchipDIRECT.URL = externJsUrl;
    var url = window.location.href.split('/');
    //var selectedLanguage = url[url.length - 2];
    var selectedDeviceFamily = url[url.length - 1];
    microchipDIRECT.resetLanguage(selectedLanguage);
    //if (typeof deviceFamilyNameForCart === 'undefined' || deviceFamilyNameForCart === '')
    //    deviceFamilyNameForCart = url[url.length - 1];
    microchipDIRECT.getProductInfo(selectedDeviceFamily);
}

microchipDIRECT.handleProductInfo = function (data) {
    if (data === null || data === "") {
        $('#buy-now-active').hide();
        $('.pricing-icon').hide();
        $('#sectionNoBuyTab').removeClass("display-none");
        return false;
    }
    
        $('#sectionNoBuyTab').addClass("display-none");
        //$('.product_buy--tab').show();
        $('.pricing-icon').show();
    
    var productTemplate = kendo.template($("#productTemplate").html());
    $('.container-fluid .products-list').append(productTemplate(data));
    kendo.ui.progress($('.product_buy--tab'), false);
    setProductsFilterOptions();
}

microchipDIRECT.handleCartItemCount = function (data) {
    if ($('#spanProductCount').length > 0) {
        $('#spanProductCount').text(data);
    }
}

function addDevtoolToCart(cntrl, cpnId, prodQty, imageUrl) {
    productImageUrl = "http://www.microchipdirect.com/images/devtools/" + imageUrl;
    kendo.ui.progress(cntrl.closest('div.col-md-10'), true);
    microchipDIRECT.addToCart(cpnId, prodQty);
}


function addToCart(cntrl, cpnId, imageUrl, minOrderQty, maxOrderQty) {
    if ($('div.container-fluid.products-list.table-view').length === 0)
        qtyCntrl = cntrl.parent().prev().find('[name=QtyAmt]');
    else
        qtyCntrl = cntrl.prev();

    var prodQty = qtyCntrl.val();
    productImageUrl = imageUrl;
    kendo.ui.progress(cntrl.closest('div.buy-info'), true);
    microchipDIRECT.addToCart(cpnId, prodQty);
}


microchipDIRECT.handleAddToCart = function (data) {
    displayConfirmationPopup(data);
    $('#spanProductCount').text(data.TotalCartItemCount);
    kendo.ui.progress($('div.buy-info'), false);
    kendo.ui.progress($('div.col-md-10'), false);

}

function displayConfirmationPopup(result) {
    var confirmationTemplate = kendo.template($("#confirmationTemplate").html());
    var options = {};
    var templateData = {};
    templateData.CPNId = ""
    templateData.Quantity = "";
    templateData.PricePerUnit = "";
    templateData.Total = "";
    templateData.ProductImageUrl = "";
    templateData.Msg = "";
    if (result.IsValid) {
        //options = { width: "750px", height: "300px" };
        templateData.CPNId = result.CPN;
        templateData.Quantity = result.Quantity;
        templateData.PricePerUnit = result.PerUnitPricingValue;
        templateData.Total = result.TotalPricingValue;
        templateData.ProductImageUrl = productImageUrl;
        templateData.Msg = result.Msg;
        templateData.Currency = result.Currency;
        showAddToCartStatusPopUp(confirmationTemplate(templateData))
        if (typeof qtyCntrl != 'undefined')
            qtyCntrl.val('');
    }
    else {
        templateData.Msg = result.Msg;
        showAddToCartStatusPopUp(confirmationTemplate(templateData));
    }
}

function showAddToCartStatusPopUp(content, windowOptions) {
    var windowWidth = $(window).width() > 1000 ? '700px' : $(window).width();
    windowOptions = windowOptions || { width: windowWidth };
    var kendoWindow = $("<div id='divKWindow'/>").kendoWindow({
        //title: true,
        modal: true,
        draggable: false,
        actions: ["Close"]
    }).data("kendoWindow");
    kendoWindow.setOptions(windowOptions);
    kendoWindow.content(content).center().open();
}

function CloseCartPopUp(ele) {
    $(ele).closest("[data-role=window]").data("kendoWindow").close();
}

function ApplyFilter() {
    applyProductsFilter();
    setTimeout(HandleNoProducts, 500);

}
function ClearFilter() {
    $('#divMessage').html('');
    if ($('div.container-fluid.products-list.table-view').length > 0)
        $('div.row.products-head').show();
    clearProductFilter();
}
function HandleNoProducts() {
    var isTableViewSelected = $('div.container-fluid.products-list.table-view').length > 0;
    var noProducts = $('div.products-list div.row.product').length == $('div.products-list div.row.product.productHidden').length;
    $('#divMessage').html('');
    if (noProducts) {
        $('#divMessage').parent().removeClass('display-none');
        $('#divMessage').html('No products are available for the selected filter criteria.');

        if (isTableViewSelected)
            $('div.row.products-head').hide();
    }
    else
        if (isTableViewSelected)
            $('div.row.products-head').show();


}