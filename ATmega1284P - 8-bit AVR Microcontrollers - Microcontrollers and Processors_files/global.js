var productImages = 1,
    currentSlide = 1,
    numberOfSlides = 0,
    slideTimer,
    prevSearchPos,
    slideWidth,
    sliderLength;

function setUp() {

    //Language Selector
    var currentLocation = window.location.pathname + window.location.search;
    var currentLanguageLocation = window.location.pathname;
    var langCookie = 'MCHP=';
    var currentSavedLang;
    var cookies = document.cookie.split(';');
    var selectedLanguage;
    for (var i = 0; i < cookies.length; i++) {
        var c = cookies[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(langCookie) == 0) {
            var langTaken = c.substring(langCookie.length, c.length);
        }
    }

    //Language selector tool - is here
    $('.language').change(function () {
        var currentLocation = window.location.pathname + window.location.search;

        var redirectPage = currentLocation;

        if (currentLocation.indexOf('/ja/') > -1) {
            currentLocation = currentLocation.replace('/ja/', '/');
        } else if (currentLocation.indexOf('/zh/') > -1) {
            currentLocation = currentLocation.replace('/zh/', '/');
        } else if (currentLocation.indexOf('/en/') > -1) {
            currentLocation = currentLocation.replace('/en/', '/');
        }
        selectedLanguage = $('.language').find(':selected').text();

        if (selectedLanguage == 'Japanese') {
            document.cookie = 'MCHP=Style=jp; expires=31 Dec 9999 12:00:00 UTC; path=/';
            document.cookie = 'CurrentLanguage=jp; expires=31 Dec 9999 12:00:00 UTC; path=/';
            if (isSitefinityPage) {
                redirectPage = '/ja' + currentLocation;
                $.ajax({
                    type: "GET",
                    url: redirectPage,
                    error: function (jqXHR, textStatus, errorThrown) {
                        if (jqXHR.status == 404) {
                            $('body').prepend('<section class="container messageLang" style="position: relative;text-align: center;"><div class="languageMessages" style="position: absolute; background: red; padding: 10px; border-radius: 0 0 20px 20px; width: 100%; z-index: 999999; color: white; font-weight: bold; box-shadow: 0 -5px 20px #000000; font-family: arial; top: -60px;">There is currently no Japanese translation for this page</div></section>');
                            $('.languageMessages').animate({ top: 0 });
                            window.setTimeout(function () {
                                $('.languageMessages').animate({ top: -60 }, function () {
                                    $('.messageLang').remove();
                                    $('.language').val(currentSavedLang);
                                });
                            }, 3000);
                        }
                    },
                    success: function () {
                        location.href = redirectPage;
                    }
                })

            } else {
                location.href = currentLocation;
            }
        } else if (selectedLanguage == 'Chinese') {
            document.cookie = 'MCHP=Style=cn; expires=31 Dec 9999 12:00:00 UTC; path=/';
            document.cookie = 'CurrentLanguage=cn; expires=31 Dec 9999 12:00:00 UTC; path=/';
            if (isSitefinityPage) {
                redirectPage = '/zh' + currentLocation;
                $.ajax({
                    type: "GET",
                    url: redirectPage,
                    error: function (jqXHR, textStatus, errorThrown) {
                        if (jqXHR.status == 404) {
                            $('body').prepend('<section class="container messageLang" style="position: relative;text-align: center;"><div class="languageMessages" style="position: absolute; background: red; padding: 10px; border-radius: 0 0 20px 20px; width: 100%; z-index: 999999; color: white; font-weight: bold; box-shadow: 0 -5px 20px #000000; font-family: arial; top: -60px;">There is currently no Chinese translation for this page</div></section>');
                            $('.languageMessages').animate({ top: 0 });
                            window.setTimeout(function () {
                                $('.languageMessages').animate({ top: -60 }, function () {
                                    $('.messageLang').remove();
                                    $('.language').val(currentSavedLang);
                                });
                            }, 3000);
                        }
                    },
                    success: function () {
                        location.href = redirectPage;
                    }
                })
            } else {
                location.href = currentLocation;
            }

        } else if (selectedLanguage == 'English') {
            document.cookie = 'MCHP=Style=en; expires=31 Dec 9999 12:00:00 UTC; path=/';
            document.cookie = 'CurrentLanguage=en; expires=31 Dec 9999 12:00:00 UTC; path=/';
            location.href = currentLocation;
        }
    });
    if (langTaken == 'Style=jp') {
        currentSavedLang = 'Japanese';
        $('.language').empty();
        $('.language').append('<option>Japanese</option><option>Chinese</option><option>English</option>');
    } else if (langTaken == 'Style=cn') {
        currentSavedLang = 'Chinese';
        $('.language').empty();
        $('.language').append('<option>Chinese</option><option>English</option><option>Japanese</option>');
    } else if (langTaken == 'Style=en') {
        currentSavedLang = 'English';
        $('.language').empty();
        $('.language').append('<option>English</option><option>Japanese</option><option>Chinese</option>');

    } else {
        $('.language').empty();
        $('.language').append('<option>English</option><option>Japanese</option><option>Chinese</option>');
    }



    selectedLanguage = $('.language').find(':selected').text();

    // Open all acorrdions for editing
    window.setTimeout(function () {
        if ($('.sfInlineEditingWorkflowMenu').length) {
            $('.accordion').each(function () {
                $(this).children('.contents').show();
            });
        }
    }, 3000);

    // Splitting the il items into two columns
    var count = $('ul.split li').length,
        eachSideAmt = count / 2;

    $('ul.split').each(function () {
        var itemsListed = $(this).children('li');
        for (var i = 0; i < itemsListed.length; i += eachSideAmt) {
            itemsListed.slice(i, i + eachSideAmt).wrapAll('<div class="half"></div>');
        }
    });


    // Product accordion trees on the homepage
    $('.ProductTreeContent > div > div > ul').each(function () {
        var treeListCount = $(this).children('li').length,
            treeListAmt = treeListCount / 3;
        var itemsTreeListed = $(this).children('li');
        for (var i = 0; i < itemsTreeListed.length; i += treeListAmt) {
            itemsTreeListed.slice(i, i + treeListAmt).wrapAll('<div class="four columns"></div>');
        }
    });


    // Slider image Banners setup
    var setupImagesWidth = $('.productSlider ul li img').width();
    $('.productSlider').css({ height: setupImagesWidth });

    $('.sliderNav').next('ul').children('li').each(function () {
        $('.productSlider .sliderNav ul').append('<li></li>');
    });
    $('.sliderNav ul li:first-child').addClass('active');


    // Dev tools navigation setup on Product Pages
    var devToolAmt = $('.tools ul').length;
    $('.tools > ul').each(function () {
        var numberOfItems = $(this).children('li').length;
        var currentLable = $(this).attr('class');
        $('li[data-open="' + currentLable + '"]').append('<span>' + numberOfItems + '</span>');
    });

    // Set the height of the border boxes to be the same in eacg row

    $('.box-row').each(function () {
        if ($(this).attr('class').indexOf('noHeight') > -1) {
            //            Nothing
        } else {
            var currentHeight = 0;
            var savedHeight = 0;
            $(this).find('.box-border').each(function () {
                currentHeight = $(this).outerHeight() - 40;
                if (currentHeight > savedHeight) {
                    savedHeight = currentHeight;
                }
            });
            $(this).find('.box-border').css({ height: savedHeight });
        }

    });

    // Set the height of the border list

    $('.three-cols').each(function () {
        var currentHeight = 0;
        var savedHeight = 0;
        $(this).find('.four.columns').each(function () {
            currentHeight = $(this).outerHeight();
            if (currentHeight > savedHeight) {
                savedHeight = currentHeight;
            }
        });
        $(this).find('.four.columns').css({ height: savedHeight });
    });
}

function performSearch() {
    $('.searchInput').keypress(function (e) {
        var keyPressed = e.keyCode;
        if (keyPressed == 13) {
            e.preventDefault();
            $('.seachBarSubmit').trigger('click');
        }
    });
}

function openAccordion() {
    $('.accordion .title').each(function () {
        $(this).next('.contents').slideUp();
        $(this).removeClass('open');
        $(this).unbind('click');
        $(this).bind('click', openAccordion);
    });

    $(this).next('.contents').slideDown();
    $(this).unbind('click');
    $(this).bind('click', closeAccordion);
    $(this).addClass('open');
}

function closeAccordion() {
    $(this).next('.contents').slideUp();
    $(this).removeClass('open');
    $(this).unbind('click');
    $(this).bind('click', openAccordion);
}

function productNav() {
    $('.productSlider .sliderNav div').on('click', function () {
        var numberOfImages = $(this).parent('.sliderNav').next('ul').children('li').length;
        var imageHeight = $('.productSlider ul li img').height() + 6;
        var direction = $(this).attr('class');
        if (direction == 'next') {
            if (productImages == numberOfImages) {
                var amtToMove = '0';
            } else {
                var amtToMove = '-=' + imageHeight;
                productImages++;
                $(this).parent('.sliderNav').next('ul').animate({
                    marginTop: amtToMove
                });
            }
        } else {
            if (productImages == 1) {
                var amtToMove = '';
            } else {
                var amtToMove = '+=' + imageHeight;
                productImages--;
                $(this).parent('.sliderNav').next('ul').animate({
                    marginTop: amtToMove
                });
            }
        }

        $('.sliderNav ul li.active').removeClass('active');
        $('.sliderNav ul li:nth-child(' + productImages + ')').addClass('active');
    });
}

function scrollTo() {
    $('.icon').on('click', function (e) {
        e.preventDefault();
        var scrollToArea = $(this).parent('a').attr('href');
        $(scrollToArea + ' div').trigger('click');
        $('html, body').animate({
            scrollTop: $('#documents').offset().top
        }, 400);
    });
}

function devtoolsNav() {
    $('.devtools-nav > ul li').on('click', function () {
        $('.tools > ul').slideUp();
        $('.devtools-nav > ul li').removeClass('openItem');
        var devtoolsOpen = $(this).data('open');
        $(this).addClass('openItem');
        $('.' + devtoolsOpen).slideDown();
    });
}

function loadBanners() {
    var BanerContainer = $('.sliderContainer .slider');

    var PathLookUp = "";
    if (document.location.pathname == '/') {
        PathLookUp = "home";
    } else if (document.location.pathname == '/zh/') {
        PathLookUp = "zh/home";
    } else if (document.location.pathname == '/ja/') {
        PathLookUp = "ja/home";
    } else {
        var currentProtocol = document.location.protocol,
            currentDomain = document.location.host,
            currentPathname = document.location.href;
        currentPathname = currentPathname.replace(currentProtocol + '//', '');
        currentPathname = currentPathname.replace(currentDomain + '/', '');
        PathLookUp = currentPathname;
    }
    /* RD: commented 3/21/2018 as productpages threw error
	$.get("/GetBannerFeed/" + PathLookUp, function (data) {
        if (data.length > 0) {
            $.each(data, function (i, banner) {
                if (banner.Link == '') {
                    BanerContainer.append('<div class="slide" id="' + banner.MarketoRtpId + '"><img src="' + banner.Image + '" alt="' + banner.Title + '"/><div class="description top-' + banner.TextFloat + ' ' + banner.TextColor + '"><h2>' + banner.HeadLine + '</h2>' + banner.SubLine + '</div></div>');
                } else {
                    BanerContainer.append('<div class="slide" id="' + banner.MarketoRtpId + '"><a href="' + banner.Link + '"><img src="' + banner.Image + '" alt="' + banner.Title + '"/><div class="description top-' + banner.TextFloat + ' ' + banner.TextColor + '"><h2>' + banner.HeadLine + '</h2>' + banner.SubLine + '</div></a></div>');
                }
            });
        }

    }).done(sliderSetUp);*/
}

function leftNavHeight() {
    var leftNavHeight = $('.left-nav').height();
    var contentHeight = $('section .ten.columns').height();

    if (leftNavHeight > contentHeight) {
        $('section .ten.columns').animate({ minHeight: leftNavHeight });
    }
}

function leftNavSetUp() {

    $('.left-nav ul > li').each(function () {
        $(this).has('ul').addClass('subNav');
    });
    leftNavHeight();
}

// Event binding for left nav clicking
$(document).on("click", ".left-nav ul > li.subNav:not(.open) > a", function (e) {
    e.preventDefault();
    $(this).parent('li').children('ul').slideDown(300);
    $(this).parent('li').addClass('open');
    window.setTimeout(leftNavHeight, 200);
});
$(document).on("click", ".left-nav ul > li.subNav.open > a", function (e) {
    e.preventDefault();
    $(this).parent('li').children('ul').slideUp();
    $(this).parent('li').removeClass('open');
});

function openLeftNavToCurrentPage() {
    //Open left navigation to the current page

    if ($('.left-nav').length) {

        if ($('.Selected').length) {
            var selectedLeftNav = $('.Selected').attr('class');

            if (selectedLeftNav.indexOf('Level3') > -1) {
                $('.Selected').parent('li').parent('ul').parent('li').parent('ul').show();
                $('.Selected').parent('li').parent('ul').parent('li').parent('ul').parent('li').addClass('open');

                $('.Selected').parent('li').parent('ul').show();
                $('.Selected').parent('li').parent('ul').parent('li').addClass('open');

            } else if (selectedLeftNav.indexOf('Level2') > -1) {
                $('.Selected').parent('li').parent('ul').show();
                $('.Selected').parent('li').parent('ul').parent('li').addClass('open');
            }
        }

        leftNavHeight();
    }
}

function treeMenu() {
    $('.ProductTreeTitles > div').on('click', function () {

        $('.ProductTreeTitles div.open').animate({ height: '-=6' });
        $('.ProductTreeContent div div > ul').slideUp();
        $('.ProductTreeTitles div.open div').text('Expand');
        $('.ProductTreeTitles div.open').removeClass('open');

        var menuToShow = $(this).children('h3').data('submenu');
        $(this).animate({ height: '+=6' });
        $(this).addClass('open');
        $(this).children('div').text('Collapse');
        $('.ProductTreeContent .' + menuToShow + ' > ul').slideDown();
        $('.ProductTreeContent').slideDown();
    });
}

function treeSubMenu() {

    $('.ProductTreeContent > div > div > ul > div > li.submenu > a').on('click', function (event) {
        event.preventDefault();
        $('.ProductTreeContent > div > div > ul > div > li.submenu').removeClass('open');
        $('.ProductTreeContent > div > div > ul > div > li.submenu ul').slideUp();
        $(this).parent('li').addClass('open');
        $(this).next('ul').slideDown();
    });

    $('.subMenuSubItem > a').on('click', function (event) {
        event.preventDefault();
        $('.ProductTreeContent > div > div > ul > div > li.open > ul > li.open > ul').slideUp();
        $('.ProductTreeContent > div > div > ul > div > li.open > ul > li.open').removeClass('open');
        $(this).parent('li').addClass('open');
        $(this).next('ul').slideDown();
    });
}

function sliderSetUp() {
    numberOfSlides = $('section.sliderContainer .slider .slide').length;
    slideWidth = $('.sliderContainer').css('width');
    sliderLength = (numberOfSlides + 1) * parseInt(slideWidth, 10);

    $('.slider').css({ width: sliderLength });
    $('.slide').css({ width: slideWidth });
    if (numberOfSlides === 1) {
        // Nothing
    } else {
        $('.slider .slide:first-child').clone().appendTo('.slider');
        $('.sliderContainer').append('<div class="sliderNav"></div>');

        for (i = 0; i < numberOfSlides; i++) {
            $('.sliderContainer .sliderNav').append('<div slideOrder="' + (i + 1) + '"></div>');
        }

        $('.sliderContainer .sliderNav div:nth-of-type(1)').addClass('currentSlide');
    }
    sliderNavSelect();
}

function sliderResize() {
    var RSNumberOfSlides = $('section.sliderContainer .slider .slide').length;
    var RSslideWidth = $('.sliderContainer').css('width');
    var RSsliderLength = (RSNumberOfSlides + 1) * parseInt(RSslideWidth, 10);

    $('.slider').css({ width: RSsliderLength });
    $('.slide').css({ width: RSslideWidth });
}

function sliderRoatation() {

    if (numberOfSlides === 1) {
        //        Do Nothing

    } else if (numberOfSlides === 0) {
        //Do Nothing
    } else {

        if (currentSlide == numberOfSlides) {
            slideWidth = $('.sliderContainer').css('width');
            $('.slider').animate({ left: '-=' + slideWidth }, 600);
            currentSlide = 1;

            setTimeout(function () {
                $('.slider').css({ left: 0 });
            }, 700);

        } else {
            slideWidth = $('.sliderContainer').css('width');
            $('.slider').animate({ left: '-=' + slideWidth }, 600);
            currentSlide++;
        }

        $('.sliderContainer .sliderNav div').removeClass('currentSlide');
        $('.sliderContainer .sliderNav div:nth-of-type(' + currentSlide + ')').addClass('currentSlide');
    }

}

function sliderNavSelect() {
    $('.sliderContainer > div.sliderNav > div').on('click', function () {
        window.clearInterval(slideTimer);
        var slideNavClicked = $(this).attr('slideOrder');
        var slideWidthNumber = slideWidth.replace('px', '');
        var slideToSelection = (slideWidthNumber * slideNavClicked) - slideWidthNumber;
        currentSlide = slideNavClicked;


        $('.slider').animate({ left: '-' + slideToSelection }, 600);

        $('.sliderContainer .sliderNav div').removeClass('currentSlide');
        $('.sliderContainer .sliderNav div:nth-of-type(' + slideNavClicked + ')').addClass('currentSlide');
        slideTimer = window.setInterval(sliderRoatation, 7000);
    });
}

function tabNav() {
    $('.tabs .tab-nav div').on('click', function () {
        $('.tabs .tab-nav div').removeClass('active');
        $('.tabs .tab-content div').removeClass('active');
        $('.tabs .tab-content div.tab-item').fadeOut('fast');
        $(this).addClass('active');
        var tabOrderData = $(this).data('order');
        $('.tabs .tab-content div').each(function () {
            if ($(this).data('order') == tabOrderData) {
                $(this).addClass('active');
                $(this).parent('div.tab-item').fadeIn();
            }
        });
    });

    $('.GoToTab').on('click', function () {
        var goToTab = $(this).data('order');
        $('.tabs .tab-nav div[data-order="' + goToTab + '"]').trigger('click');
    })
}

function promoBoxes() {


    $('.three-columns.whiteList ul').each(function () {
        var itemsSelector = $(this).children('li');
        var itemsSelectorHalf = itemsSelector.length / 2;

        for (var i = 0; i < itemsSelector.length; i += itemsSelectorHalf) {
            itemsSelector.slice(i, i + itemsSelectorHalf).wrapAll('<div class="page"></div>');
        }
    });

    $('.promoBoxes .three-columns').mouseenter(function () {
        $('.promoBoxes .three-columns').animate({ width: '24.500000000025%' }, { queue: false });
        $(this).animate({ width: '49.9333333333%' }, { queue: false });
        $(this).find('.page:nth-of-type(2)').delay(300).fadeIn();
    });

    $('.promoBoxes').mouseleave(function () {
        $('.promoBoxes .three-columns').animate({ width: '32.6666666667%' }, { queue: false });
        $('.promoBoxes .three-columns:first-child').animate({ width: '33.1333333333%' }, { queue: false });
        $('.promoBoxes .three-columns:last-child').animate({ width: '33.1333333333%' }, { queue: false });
        $('.promoBoxes .three-columns').find('.page:nth-of-type(2)').fadeOut();
    });

    $('.promoBoxes .three-columns.half-height > div').mouseenter(function () {
        $('.promoBoxes .three-columns.half-height > div').animate({ height: '21%' }, { queue: false });
        $(this).find('.page:nth-of-type(2)').delay(300).fadeIn();
        $(this).find('p').delay(300).fadeIn();
        $(this).animate({ height: '80%' }, { queue: false })
    });

    $('.promoBoxes .three-columns.half-height > div').mouseleave(function () {
        $('.promoBoxes .three-columns.half-height > div').animate({ height: '50%' }, { queue: false });
        $('.promoBoxes .three-columns.half-height > div').find('.page:nth-of-type(2)').fadeOut();
        $('.promoBoxes .three-columns.half-height > div').find('p').fadeOut();
    });
}

function pressRelease() {
    var newHeight = $('section.press .four.columns').height() + 48;
    $('section.press').animate({ height: newHeight }, 400);

    $('html, body').animate({
        scrollTop: $('footer').offset().top
    }, 400);
    $(this).unbind('click');
    $(this).bind('click', closePressRelease);
    $(this).addClass('open');
}

function closePressRelease() {
    $('section.press').animate({ height: 27 }, 400);
    $(this).unbind('click');
    $(this).bind('click', pressRelease);
    $(this).removeClass('open');
}

function SearchEnter() {
    try {
        if (document.layers) {
            searchObj = eval(document.searchbar);
        }
        else if (navigator.userAgent.indexOf("Opera") != -1) {
            searchObj = eval(document.all.searchbar);
        }
        else if (document.all && !document.getElementById) {
            searchObj = eval(document.all.searchbar);
        }
        else if (document.getElementById) {
            searchObjs = document.getElementsByName('searchbar');
            searchObj = searchObjs[0];
        }
        else { }
        var UserText = searchObj.value;

        if (UserText == '') {
            return

        }

        window.location = "https://www.microchip.com/search/searchapp/searchhome.aspx?id=2&q=" + UserText;

    }
    catch (err) {
    }
}

function SearchAC() {
    try {
        if (document.layers) {
            searchObj = eval(document.searchbar);
        }
        else if (navigator.userAgent.indexOf("Opera") != -1) {
            searchObj = eval(document.all.searchbar);
        }
        else if (document.all && !document.getElementById) {
            searchObj = eval(document.all.searchbar);
        }
        else if (document.getElementById) {
            searchObjs = document.getElementsByName('searchbar');
            searchObj = searchObjs[0];
        }
        else { }
        var UserText = searchObj.value;

        if (UserText == '') {
            return

        }

        window.location = "https://www.microchip.com/search/searchapp/searchhome.aspx?id=2&q=" + UserText + "&ac=1";


    }
    catch (err) {
    }
}

function formatItem(row) {
    var cpn = row[0].replace(/(<.+?>)/gi, '');
    var retStr = "";

    if (row[1] != null && row[1].length > 0)
        retStr = row[1].replace(/(<.+?>)/gi, '');
    if (retStr.length > 0)
        retStr = cpn + " - " + retStr;
    else
        retStr = cpn;
    return retStr;
}

function formatResult(row) {
    var retStr = row[0].replace(/(<.+?>)/gi, '');
    if (retStr == null || retStr.length == 0)
        retStr = row[1].replace(/(<.+?>)/gi, '');
    return retStr;
}

$(window).load(function () {
    setUp();
    scrollTo();
    devtoolsNav();
    leftNavSetUp();
    productNav();
    tabNav();
    promoBoxes();
    treeMenu();
    treeSubMenu();
    performSearch();
    $('.button.press').on('click', pressRelease);
    openLeftNavToCurrentPage();
    slideTimer = window.setInterval(sliderRoatation, 7000);

    $('.ProductTreeTitles > div:nth-of-type(1)').trigger('click');
    $('.ProductTreeContent > div:nth-child(1) > div > ul > div:nth-child(1) > li:nth-child(1) a').trigger('click');

    $('.accordion .title').on('click', openAccordion);

});

$(document).ready(function () {
    $('a[href*=".jpg"],a[href*=".png"]').bind("click", function (e) {
        e.preventDefault();
        $('body').prepend('<div id="window" style="text-align: center;"><img src="' + $(this).attr('href') + '" style="max-width: 900px; max-height: 900px;" /></div>');
        $("#window").kendoWindow({
            actions: ["Close"],
            draggable: false,
            modal: true,
            resizable: false,
            title: ""
        });
        win = $("#window").data("kendoWindow");
        win.center();
        $(".k-icon").attr("style", "margin: 0px;");
        $(".k-window-action").removeAttr("href");
        $('.k-window-actions, .k-window-actions *, .k-overlay, .k-overlay *').bind("click", function () {
            $(".k-widget, .k-overlay, #window").remove();
        });
    });
});

$(window).resize(function () {
    var windowSize = $(window).width();
    var pageWidth = $('.container').width();
    var marginLeft = (windowSize - pageWidth) / 2;
    sliderResize();
    $('.betaBox').css({ left: marginLeft });
});


//************************************************************************************************

//                                              Updated Nav and Mobile Begins

//************************************************************************************************

var menuOpen = false;               // Whether or not the menu is currently open
var animatingMenu = false;          // Whether or not there is an animation happening
var selectedTopLevelMenu = "";      // Name of the selected top level menu item
var animationTime = 200;            // Time in ms for menu animations
var mobileWidth = 1020;             // Width when mobile menu should show in px
var hId = null;                       // Hightlight data-attr
var hSize = 0;                      // Stores prior highlight size
var totalNavHeight = 0;             // Stors current total height
var arrowParentSize = 0;            // ArrowNav parent height size
var lastArrow = null;               // last open arrow in subNav



/*
 * Check to see if user's screen size is less than or equal to mobileWidth
 */
function checkMobile() {
    return (window.innerWidth <= mobileWidth);
}

/*
 * Open the mobile navigation menu
 */
function openMenu() {

    // Check if the menu is open and return if already open
    if (menuOpen) return;
    menuOpen = true;

    // Add .noscroll to body
    $("body").addClass("noscroll");

    // Add .noscroll to html
    $("html").addClass("noscroll");

    // Add .active to .mobileMenu
    $(".mobileMenu").addClass("active");

    // Animate .mobileMenu into screen with a left of 0
    $(".mobileMenu").animate({ "left": "0" }, animationTime);

    // Reset overflow-y on .mobileMenu .container so it scrolls
    $(".mobileMenu .container").css("overflow-y", "auto");
}

/*
 * Select submenu to open and open it
 */
function selectMenu(context) {

    // Submenu data attribute of link
    var targetSubmenu = context.dataset.submenu;

    // Use targetSubmenu to grab corresponding submenu in .foldingNavigation
    var selectedSubmenu =
        $(".foldingNavigation .container").filter(function (index) {
            return $(this).hasClass(targetSubmenu);
        })[0];

    // Check if the menu is being animated or if there is no corresponding
    // submenu and return if so
    if (animatingMenu || !selectedSubmenu) return;
    animatingMenu = true;

    // Add .active to .foldingNavigation
    $(".foldingNavigation").addClass("active");

    // Add .active to the selected submenu
    $(selectedSubmenu).addClass("active");

    // Hide overflow-y on .mobileMenu .container
    $(".mobileMenu .container").css("overflow-y", "hidden");

    // Set title of menu to the selected navigation item's title
    selectedTopLevelMenu = $(context).clone().children().remove().end().text();
    $(".mobileMenu__title").text(selectedTopLevelMenu);

    // Add .submenu-active to .mobileMenu
    $(".mobileMenu").addClass("submenu-active");

    // Animate the top level navigation item .container within .mobileMenu
    // off of the page
    $(".mobileMenu .container").animate({ "left": "-100%" }, animationTime, function () {

        // Add .hidden to navigation item .container
        $(this).addClass("hidden");

    });

    // Animate .foldingNavigation into the page
    $(".foldingNavigation").animate({ "left": "0" }, animationTime, function () {
        animatingMenu = false;
    });

}



/*
 * On load, add arrows to the end of menu items that contain submenus
 */
$(document).ready(function () {

    // Append arrows to .mobileMenu links with submenus
    $(".mobileMenu li a:not(.linkTo)").append("<span class='nav-arrow'>&rsaquo;</span>");

    // Find anchors within lists in .nav-list and append to them
    $(".nav-list li a:not(.linkTo)").append(function () {

        // Check if menu item has a nested list or targets a submenu
        if (!($(this).parent("li").children("ul")[0] === undefined)) {
            if ($(this).parent("li").children("ul")[0] || this.dataset.submenu)
                return "<span class='nav-arrow'>&rsaquo;</span>";
        }

    });

});



/*
 * Click event for opening the mobile navigation menu
 */
$(document).on("click", ".nav__open-menu", function (e) {

    // Check if user is on a mobile-sized screen and return if not
    if (!checkMobile()) return;

    // Open the mobile menu
    openMenu();

});

/*
 * Click event for selecting a tab in the navigation bar
 */
$(document).on("click", ".nav__tabs a:not(.linkTo)", function (e) {

    // Check if user is on a mobile-sized screen and return if not
    if (!checkMobile()) return;

    // Check if the menu is open and return if it is
    if (menuOpen) return;

    // Prevent default action of clicking an anchor
    e.preventDefault();

    // Check if link has a submenu in dataset
    if (this.dataset.submenu) {

        // Open the navigation menu
        openMenu();

        // Use clicked tab to determine selected submenu and open it
        selectMenu(this);

    }

});

/*
 * Click event for links in .mobileMenu, which consists of all top level
 * navigation items
 */
$(document).on("click", ".mobileMenu li > a:not(.linkTo)", function (e) {

    // Check if user is on a mobile-sized screen and return if not
    if (!checkMobile()) return;

    // Check if the menu is open and return if closed
    if (!menuOpen) return;

    // Prevent default action of clicking an anchor
    e.preventDefault();

    // Use clicked menu item to determine selected submenu and open it
    selectMenu(this);

});

/*
 * Click even of links in .foldingNavigation, which consists of all submenus
 * and is displayed after an initial selection in .mobileMenu has been made
 */
$(document).on("click", ".foldingNavigation li:not(.btm) > a", function (e) {

    // Check if user is on a mobile-sized screen and return if not
    if (!checkMobile()) return;

    // Check if the menu is open and returns if closed
    if (!menuOpen) return;

    // Prevent default action of clicking an anchor
    e.preventDefault();

    // The next menu nested in the li that contains the anchor clicked
    var nextMenu = $(this).parent("li").children("ul")[0];

    // The current menu displayed
    var currentMenu = $(this).parent("li").parent("ul")[0];

    // Check if the menu is being animated or if there is no next menu and
    // return if so
    if (animatingMenu || !nextMenu) return;
    animatingMenu = true;

    // Set next menu to .active and .current
    $(nextMenu).addClass("active current");

    // Set title of menu to the selected navigation item's title
    $(".mobileMenu__title")
        .text($(this).clone().children().remove().end().text());

    // Reset the scroll position of .foldingNavigation to top (0)
    $(".foldingNavigation")[0].scrollTop = 0;

    // Animate active .container in .foldingNavigation left -100% in order to
    // display next submenu
    $(".foldingNavigation .container.active")
        .animate({ "left": "-=100%" }, animationTime, function () {

            // Remove .current from the current menu
            if (currentMenu) $(currentMenu).removeClass("current");

            // Ensure .foldingNavigation has .submenu-active set
            $(".foldingNavigation").addClass("submenu-active");

            animatingMenu = false;

        });

});

/*
 * Click event of menu back button, navigate back through the navigation menu
 */
$(document).on("click", "a.mobileMenu__back", function (e) {

    // Check if user is on a mobile-sized screen and return if not
    if (!checkMobile()) return;

    // Check if the menu is open and returns if closed
    if (!menuOpen) return;

    // Prevent default action of clicking an anchor
    e.preventDefault();

    // The menu that was previously open (contains the current menu)
    var previousMenu = $("ul.current").parent("li").parent("ul")[0];

    // The current menu displayed
    var currentMenu = $("ul.current")[0];

    // Check if the menu is being animated and returns if so
    if (animatingMenu) return;
    animatingMenu = true;

    // Navigate back to top tier navigation menu if no previous menu exists
    if (!previousMenu) {

        // Remove .hidden from .mobileMenu .container
        $(".mobileMenu .container").removeClass("hidden");

        // Remove .submenu-active from .mobileMenu
        $(".mobileMenu").removeClass("submenu-active");

        // Animate .foldingNavigation off of the page
        $(".foldingNavigation").animate({ "left": "100%" }, animationTime, function () {

            // Remove .active from .foldingNavigation
            $(".foldingNavigation").removeClass("active");

            // Remove .active from all .containers in .foldingNavigation
            $(".foldingNavigation .container").removeClass("active");

            animatingMenu = false;

        });

        // Animate the top level navigation item container within .mobileMenu
        // back on to the page
        $(".mobileMenu .container").animate({ "left": "0" }, animationTime, function () {

            // Reset overflow-y on .mobileMenu .container so it scrolls
            $(this).css("overflow-y", "auto");
        });

        // Return because previous menu doesn't exist
        return;

    }

    // Add .active and .current to previous menu if it isn't the first menu of
    // .foldingNavigation, which consists of uls of all columns
    if ($(previousMenu).parent("li").parent("ul")[0])
        $(previousMenu).addClass("active current");

    // Set title of menu to the previous menu's title if there is one, or
    // the selected top level menu's name if not
    $(".mobileMenu__title")
        .text($(previousMenu)
            .parent("li").children("a").clone().children().remove().end()
            .text() || selectedTopLevelMenu);

    // Reset the scroll position of .foldingNavigation to top (0)
    $(".foldingNavigation")[0].scrollTop = 0;

    // Remove submenu-active class if previous menu is not within an li
    if (!$("ul.current").parent("li").parent("ul").parent("li")[0])
        $(".foldingNavigation").removeClass("submenu-active");

    // Animate active .container in .foldingNavigation left +100% in order to
    // display previous submenu
    $(".foldingNavigation .container.active")
        .animate({ "left": "+=100%" }, animationTime, function () {

            // Remove .active and .current from current menu if it exists
            if (currentMenu) $(currentMenu).removeClass("active current");

            animatingMenu = false;

        });

});

/*
 * Click event of the menu close button, close navigation menu
 */
$(document).on("click", "a.mobileMenu__close", function (e) {

    // Check if user is on a mobile-sized screen and return if not
    if (!checkMobile()) return;

    // Check if the menu is closed and returns if already closed
    if (!menuOpen) return;
    menuOpen = false;

    // Prevent default action of clicking an anchor
    e.preventDefault();

    // Animate .mobileMenu to left 100% (off the screen)
    $(".mobileMenu").animate({ "left": "100%" }, animationTime);

    // Animate .foldingNavigation to left 100% (off the screen)
    $(".foldingNavigation").animate({ "left": "100%" }, animationTime, function () {

        // Reset .container back to its default left of 0
        $(".container").css("left", "0");

        // Remove .active and .current from all uls
        $("ul.active").removeClass("current").removeClass("active");

        // Removes .active from all .containers in .foldingNavigation
        $(".foldingNavigation .container").removeClass("active");

        // Remove .submenu-active from .foldingNavigation
        $(".foldingNavigation").removeClass("submenu-active");

        // Remove .hidden from .mobileMenu .container
        $(".mobileMenu .container").removeClass("hidden");

        // Remove .submenu-active from .mobileMenu
        $(".mobileMenu").removeClass("submenu-active");

        // Remove .active from .mobileMenu and .foldingNavigation
        $(".mobileMenu").removeClass("active");
        $(".foldingNavigation").removeClass("active");

        // Check if left nav is closed
        //if (!leftNavOpen) {

        // Remove .noscroll from body
        $("body").removeClass("noscroll");

        // Remove .noscroll from html
        $("html").removeClass("noscroll");

        //}

    });

});


$(document).ready(function () {

    var checkContactMyMicrochip = new RegExp(/\/contactus|\/mymicrochip|\/myMicrochip|\/wwwregister/)
    $('header nav a').on('click', function (e) {

        // index placeholder to connect nav item to foldingnav item
        var openNavIndex = 0;

        // for mobile
        if (checkMobile()) return;

        // check for contact us tab or Microchip login  also accounts for /zh/ and /ja/
        if (!(checkContactMyMicrochip.test($(this).attr('href')))) {

            // disallow it act like an anchor
            e.preventDefault();

            // check if this is the second click
            if (typeof $(this).parent().data('priorClick') !== 'undefined') {

                // stores prior n-thchild index -- +1 b/c jquery starts at 1
                openNavIndex = Number($(this).parent().data('priorClick')) + 1;


                // reset prior foldingNav
                collapesPriorNavTab(openNavIndex);

            }

            // close tab
            if ($(this).parent().data('priorClick') === $(this).parent().children('a').index(this)) {
                $(".foldingNavigation").removeClass('open').removeAttr('style');
                $(this).parent().removeData('priorClick');
                $(this).removeClass('open');
                return;
            }

            // stores nav nth-child index
            openNavIndex = $(this).parent().children('a').index(this) + 1;
            // display a default highlight item
            $('.foldingNavigation > div:nth-child(' + openNavIndex + ') .highlight div:nth-child(1)').addClass('active');

            // creates the tab on current nav header
            $(this).addClass('open');

            $(this).parent().data('priorClick', $(this).parent().children('a').index(this));
            // adds foldingnav container
            $('.foldingNavigation').addClass('open');
            // displays nav content
            $('.foldingNavigation > div:nth-child(' + openNavIndex + ')').addClass('open');
            ExpandTopNavHighlight();

            DisplayHighlightImage($('.foldingNavigation > div.open > .highlight > div:nth-child(1)').attr('id'));

        }
    });

    function ExpandTopNavHighlight() {
        $('div.open > div.open').children().each(function () {
            if ($(this).hasClass('highlight')) {
                var highlightHeight = $($(this).children('.active').children('.active')).height() != null ? $($(this).children('.active').children('.active')).height() : 350;
                // expand column dividers
                $('.foldingNavigation').css({ height: highlightHeight });
                $('.foldingNavigation .active').css({ height: highlightHeight });
                $('.foldingNavigation .container .columns').css({ height: highlightHeight });
            }
        });
    }

    // works backwards to restart the all the heights and remove all open classes
    function collapesPriorNavTab(PriorNavTab) {


        // removes tab from the last clicked nav item
        $('nav a:nth-of-type(' + PriorNavTab + 'n)').removeClass('open');

        // remove subMenuSub Height
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .columns > ul > li.open > ul > li.open >  ul').css({ height: '0px' });

        // remove subMenu open class
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') .container.open > div >  ul > li.open').removeClass('open');

        // remove subMenu height
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .columns > ul > li.open > ul').removeAttr('style');


        // remove menu open class
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .columns > ul > li.open').removeClass('open');

        // removes open class from last folding nav item
        $('.foldingNavigation > div:nth-child(' + PriorNavTab + ')').removeClass('open');

        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .highlight ').removeAttr('style');
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .highlight .active').removeAttr('style');
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .highlight .active .active').removeAttr('style');
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .highlight .active').removeClass('active');
        $('.foldingNavigation.open > div:nth-child(' + PriorNavTab + ') > .highlight .active .active').removeClass('active');

    }

    function DisplayHighlightImage(id) {
        var highlightImage = $('#' + id + ' div.active > img');
        var highlightImgUrl = highlightImage.attr('data-navImgUrl');
        var highlightImgWidth = highlightImage.attr('data-navImgWidth');
        highlightImage.attr('src', highlightImgUrl);
        highlightImage.attr('width', highlightImgWidth);
    }

    function expandChildren(obj) {
        var multiLineCounter = 0;
        if ($(obj).is('ul')) {
            if (lastArrow != null) {
                $(lastArrow).removeAttr('style');
            }

            if ($(obj).parent().parent().height() < $('.foldingNavigation.open').height()) {
                $(obj).parent().parent().css({ height: 'auto' });

            }
            var multiLineCounter = 0;
            $(obj).children().each(function () {
                if ($(this).children('a').text().length > 35) // check for text wrapping in subnav items
                    multiLineCounter += 19;
            });
            var flex = ($(obj[0]).children().length * 19) + multiLineCounter;
            $(obj).css({ height: flex });
            lastArrow = obj;
            CompareTopNavHeights($(obj));
        }
    }

    function CompareTopNavHeights(obj) {
        window.setTimeout(function () {
            var currentColumn = $(obj).closest('div').children('ul').height();
            $(obj).closest('div').siblings().each(function () {
                if ($(this).height() > currentColumn)
                    currentColumn = $(this).height();
                else if ($(this).hasClass('highlight')) {
                    if ($($('.highlight div.active').children('.active')[1]).find('p').outerHeight() > currentColumn)
                        currentColumn = $($('.highlight div.active').children('.active')[1]).find('p').outerHeight();
                }
                $('.highlight div.active').css({ height: currentColumn });
                $('.foldingNavigation.open').css({ height: currentColumn });
            });
        }, 280)
    }

    $('.foldingNavigation .container .columns > ul > li > a').on('click', function (e) {

        if (checkMobile()) return;
        if (!$(this).parent().hasClass('btm'))
            e.preventDefault();

        // remove active class from prior highlight item

        if (hId !== null) {
            if ($('#' + hId).hasClass('active')) {
                $('#' + hId).removeClass('active');
            }

            if ($('.foldingNavigation .container.open .highlight #' + hId).index() !== 0 || $('.foldingNavigation .container.open .highlight #' + hId).index() !== -1) {
                $('#' + hId).removeClass('active');
            }
        }

        $('.foldingNavigation .container .columns ul li.open ul').removeAttr('style');
        $('.foldingNavigation .container .columns ul li.open').removeClass('open');
        $(this).parent().addClass('open');

        // global var - get data attribute
        hId = $(this).parent().attr('data-hid');

        // display corresponding highlight
        if ($('#' + hId).parent('.highlight').length === 1) {
            $('.highlight #' + hId).addClass('active');
            DisplayHighlightImage(hId);
        }
        else {
            var defaultHighlight = $('.foldingNavigation > div:nth-child(1) .highlight div:nth-child(1)');
            defaultHighlight.addClass('active');
            DisplayHighlightImage(defaultHighlight.attr('id'));
        }

        expandChildren($(this).siblings('ul'));

    });

    $('.foldingNavigation .container .columns ul > li > ul > li > a').on('click', function (e) {

        if (checkMobile()) return;

        e.stopPropagation();

        //Close all parent menu items
        $('.foldingNavigation .container .columns ul > li > ul > li.open').removeClass('open');
        var subHeight = Number($(this).attr('subHeight'));
        var parentHeight = Number($(this).parent('ul').parent('li').attr('subHeight'));
        $(this).parent().addClass('open');

        expandChildren($(this).siblings('ul'));

    });


});



/** MOBILE LEFT NAV **/
var leftNavOpen = false;

function toggleLeftNav() {

    var leftNav = $(".left-nav");
    var leftNavOpenButton = $(".left-nav-open-button");
    var htmlTag = $("html");
    var bodyTag = $("body");
    var curtain = $(".design-center .curtain");

    if (!leftNavOpen) {
        leftNav.css("height", "100%");
        leftNav.addClass("open");
        leftNavOpenButton.addClass("hidden");
        curtain.addClass("open");
    } else {
        leftNav.removeClass("open");
        leftNavOpenButton.removeClass("hidden");
        curtain.removeClass("open");
    }

    leftNavOpen = !leftNavOpen;

}

// Close mobile left nav when curtain is clicked
$(document).on("click", ".design-center .curtain", function () {
    $(".left-nav").removeClass("open");
    $(".left-nav-open-button").removeClass("hidden");
    $(".design-center .curtain").removeClass("open");
    leftNavOpen = false;
});



//Get Country Code
function GetCountryCode() {
    var currentCountryCode = '';
    var ccCookie = 'Country_Code=';
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(ccCookie) == 0)
            currentCountryCode = c.substring(ccCookie.length, c.length);
    }
    if (currentCountryCode === '') {
        var xhr = new XMLHttpRequest();
        xhr.open('GET', '//freegeoip.net/json/?callback=');
        xhr.onload = function () {
            if (xhr.status === 200) {
                var cData = JSON.parse(xhr.responseText);
                currentCountryCode = cData.country_code;
                var date = new Date();
                date.setTime(date.getTime() + (24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
                document.cookie = ccCookie + cData.country_code + expires + '; path=/';
            }
        }
        xhr.send();
    }

    return currentCountryCode;
}


/** Vimeo Player M21194 **/
$(document).ready(function () {
    if (GetCountryCode() !== 'CN') {
        $('.Player').each(function () {
            _Player($(this));
        });
    } else {
        //hide the videoplayer
        $('.Player').closest('.row').hide()
    }
});
function _GetVideo(ID) {
    var LocalCacheID = 'VimeoVideo' + ID;
    var LocalCahcedData = sessionStorage.getItem(LocalCacheID);
    if (!LocalCahcedData) {
        $.ajax({ url: 'https://vimeo.com/api/v2/video/' + ID + '.json', async: false, dataType: 'json', success: function (ResponseData) { sessionStorage.setItem(LocalCacheID, JSON.stringify(ResponseData[0])); } });
    }
    return JSON.parse(sessionStorage.getItem(LocalCacheID));
}
function _GetAlbum(ID) {
    var LocalCacheID = 'VimeoAlbum' + ID;
    var LocalCahcedData = sessionStorage.getItem(LocalCacheID);
    if (!LocalCahcedData) {
        $.ajax({ url: 'https://vimeo.com/api/v2/album/' + ID + '/videos.json', async: false, dataType: 'json', success: function (ResponseData) { sessionStorage.setItem(LocalCacheID, JSON.stringify(ResponseData)); } });
    }
    return JSON.parse(sessionStorage.getItem(LocalCacheID));
}
function _GetChannel(ID) {
    var LocalCacheID = 'VimeoChannel' + ID;
    var LocalCahcedData = sessionStorage.getItem(LocalCacheID);
    if (!LocalCahcedData) {
        $.ajax({ url: 'https://vimeo.com/api/v2/channel/' + ID + '/videos.json', async: false, dataType: 'json', success: function (ResponseData) { sessionStorage.setItem(LocalCacheID, JSON.stringify(ResponseData)); } });
    }
    return JSON.parse(sessionStorage.getItem(LocalCacheID));
}
function _PlayerLoadVideo(Target, ID) {
    VideoDetails = _GetVideo(ID);
    Target.find('.Stage').css('padding-top', 100 / (VideoDetails.width / VideoDetails.height) + '%');
    var IframeArguments = new Array();
    var IframeSource = 'https://player.vimeo.com/video/' + ID + '?title=0&byline=0&portrait=0&badge=0&autopause=0&player_id=0';
    if (Target.find('.Stage iframe').length > 0) {
        Target.find('.Stage iframe').prop('src', IframeSource);
    } else {
        IframeArguments.push('src=\"' + IframeSource + '\"');
        IframeArguments.push('width=\"' + VideoDetails.width + '\"');
        IframeArguments.push('height=\"' + VideoDetails.height + '\"');
        IframeArguments.push('frameborder=\"0\"');
        IframeArguments.push('webkitallowfullscreen');
        IframeArguments.push('mozallowfullscreen');
        IframeArguments.push('allowfullscreen');
        Target.find('.Stage').html('<iframe ' + IframeArguments.join(' ') + '></iframe>');
    }
    _PlayerAddMeta(Target, VideoDetails.title, VideoDetails.description);
    ActiveMenuItem = Target.find('.PlayListItem' + ID);
    if (ActiveMenuItem.length > 0) {
        ActiveMenuItem.parents('.Player').find('a').removeClass('ActiveItem');
        ActiveMenuItem.addClass('ActiveItem');
    }
    _PlayerResponsive(Target);
}
function _PlayerAddMeta(Target, Title, Description) {
    Target.next('.PlayerMeta').remove();
    if (Title.trim().length || Description.trim().length) {
        var HTML = [];
        if (Title.trim().length) {
            HTML.push('<h3>' + Title + '</h3>');
        }
        if (Description.trim().length) {
            HTML.push('<p>' + Description + '</p>');
        }
        Target.after('<div class="PlayerMeta">' + HTML.join('') + '</div>');
    }
}
function _Player(Target) {
    var HTML = [];
    var Playlist = [];
    if (Target.data('source')) {
        if (Regs = Target.data('source').match(/(vimeo\.com)\/([\d]+)/)) {
            VideoItems = [_GetVideo(Regs[2])];
        } else if (Regs = Target.data('source').match(/(vimeo\.com)\/(channels)\/([\d]+)/)) {
            VideoItems = _GetChannel(Regs[3]);
        } else if (Regs = Target.data('source').match(/(vimeo\.com)\/(album)\/([\d]+)/)) {
            VideoItems = _GetAlbum(Regs[3]);
        }
    }
    if (VideoItems.length > 1) {
        $.each(VideoItems, function (key, value) {
            Playlist.push('<a href="' + value.url + '" title="' + value.title + '" class="PlayListItem' + value.id + '" data-id="' + value.id + '">');
            Playlist.push('<img src="' + value.thumbnail_small + '" />');
            Playlist.push('<div>' + value.title + '</div>');
            Playlist.push('</a>');
        });
        HTML.push('<div class="ListVerticalContainer"><a class="GoUp" href=""><div><i></i></div></a><div class="ListVertical">' + Playlist.join('') + '</div><a class="GoDown" href=""><div><i></i></div></a></div>');
        HTML.push('<div class="StageContainer"><div class="Stage"></div></div>');
        HTML.push('<a class="GoLeft" href=""><div><i></i></div></a>');
        HTML.push('<a class="GoRight" href=""><div><i></i></div></a>');
        HTML.push('<div class="ListHorizontalContainer"><div class="ListHorizontal">' + Playlist.join('') + '</div></div>');
        Target.html(HTML.join(''));
        Target.find('a').unbind('click').bind('click', function (event) {
            event.preventDefault();
            _PlayerLoadVideo(Target, $(this).data('id'));
        });
        $(window).resize(function () {
            _PlayerResponsive(Target);
        });
        $(window).on('orientationchange', function () {
            _PlayerResponsive(Target);
        });
    } else {
        Target.html('<div class="Stage"></div>');
    }
    _PlayerLoadVideo(Target, VideoItems[0].id);
    if (VideoItems.length > 1) {
        _PlayerResponsive(Target);
    }
}
function _PlayerResponsive(Target) {
    if (Target.width() > 640) {
        Target.find('.StageContainer').css('margin-right', '200px');
        Target.find('.ListHorizontalContainer, .GoLeft, .GoRight').hide();
        Target.find('.ListVerticalContainer').show();
        setTimeout(function () {
            Target.find('.ListVertical').height(Target.find('.StageContainer').height() - 60);
            if ((Target.find('.ListVertical').find('a').first().height() * Target.find('.ListVertical').find('a').length) < Target.find('.ListVertical').height()) {
                Target.find('.GoUp, .GoDown').hide();
            } else {
                Target.find('.GoUp, .GoDown').show();
            }
        }, 100);

    } else {
        Target.find('.StageContainer').css('margin-right', '0px');
        Target.find('.ListHorizontalContainer, .GoLeft, .GoRight').show();
        Target.find('.ListVerticalContainer').hide();

        if ((Target.find('.ListHorizontal').find('a').first().width() * Target.find('.ListHorizontal').find('a').length) < Target.find('.ListHorizontal').width()) {
            Target.find('.GoLeft, .GoRight').hide();
        } else {
            Target.find('.GoLeft, .GoRight').show();
        }
    }
    Target.find('.GoUp').unbind('click').bind('click', function (event) {
        event.preventDefault();
        Target.find('.ListVertical').stop().animate({ scrollTop: '-=' + (Target.find('.StageContainer').height() * 0.75) }, 750);
    });
    Target.find('.GoDown').unbind('click').bind('click', function (event) {
        event.preventDefault();
        Target.find('.ListVertical').stop().animate({ scrollTop: '+=' + (Target.find('.StageContainer').height() * 0.75) }, 750);
    });
    Target.find('.GoLeft').unbind('click').bind('click', function (event) {
        event.preventDefault();
        Target.find('.ListHorizontalContainer').stop().animate({ scrollLeft: '-=' + (Target.find('.StageContainer').width() * 0.75) }, 750);
    });
    Target.find('.GoRight').unbind('click').bind('click', function (event) {
        event.preventDefault();
        Target.find('.ListHorizontalContainer').stop().animate({ scrollLeft: '+=' + (Target.find('.StageContainer').width() * 0.75) }, 750);
    });
}

// <!-- 20170628 -->

// New Paremtric chart
var rowsPerPage = 10; // Rows per page of chart
var currentPage = 0; // Current page of chart

var allColumnHeaders = []; // Array of all table column headers
var allRowData = []; // Array of each row of table data
var visibleRowData = []; // Array of visible table row's data

var branchData = []; // Array of data for all branch options (internally used)
var parametricBranchData = []; // Same as branchData but is set in HTML
var visibleBranchIndex; // Index (from branchData) of currently visible branch

var dataLoaded = false; // Whether or not JSON data has finished loading
var chartLoadTicks = 0; // Amount of times the chart checked if data was loaded


/*
 * On load of page
 */
$(document).ready(function () {
    if (parametricBranchData.length <= 0) return;
    writeParametricChartHTML();
    setParametricBranches(parametricBranchData);
});


function writeParametricChartHTML() {
    $('.parametric').append("<div class='parametric__header'><div class='parametric__title'><span class='parametric__title-text'><span class='parametric__title-text-name'></span><i class='fa fa-chevron-down parametric__title-arrow' aria-hidden='true'></i></span><ul class='parametric__title-options'></ul></div><a href='#' target='_blank' class='parametric__view-all-btn'>View All Parametrics</a></div><div class='parametric__options'><div class='parametric__search'><div class='parametric__search-box'><span class='parametric__search-icon'><i class='fa fa-search' aria-hidden='true'></i></span><input type='text' class='parametric__search-input' onkeyup='searchTable(event);' placeholder='Search term' required='required' /><span class='clear-input' onclick='clearSearch();'>&times;</span></div></div><input type='checkbox' id='parametricColumnsBox' class='parametric__columns-toggle-box' /><label for='parametricColumnsBox' class='parametric__columns-toggle'>Show/Hide Columns</label><ul class='parametric__columns-list'><li class='columns-list__top'><span class='columns-list__reset'>Reset Columns</span><span class='columns-list__close'>&times;</span></li></ul></div><div class='parametric__chart'><div class='parametric-table' data-sortby='0' data-sortorder='0' data-page='0'><div class='table__scrollable-area'><div class='table__row--header'></div></div></div><div class='parametric__pagination'><div class='parametric__pagination-pages'></div><div class='parametric__pagination-total'><span id='currentPageItemNumbers'></span> of <span id='totalTableItems'></span> items</div></div></div>");
}


/*
 * Sets dropdown options for different parametric chart branches and starts
 * initial loading of chart
 * @param branches [array]
      - array of branch information, each item is an object containing a
        branchID, branchName, and array of defaultColumns
 */
function setParametricBranches(branches) {

    if (!branches) return;

    branchData = branches;
    visibleBranchIndex = 0;

    var dropdownHTML = "";

    $.each(branchData, function (index, object) {
        dropdownHTML = dropdownHTML + "<li class='" + (index === 0 ? 'active' : '') + "' data-branchid=" + object['branchID'] + ">" + object['branchName'] + (window.location.href.indexOf('admin') > -1 ? " - " + object['branchID'] : "") + "</li>";
    });

    $(".parametric__title-options").html(dropdownHTML);
    $(".parametric__title-text-name").text(branchData[visibleBranchIndex]["branchName"]);

    if (window.location.href.indexOf('admin') > -1) {
        $(".parametric__header .parametric__admin-info").remove();
        $(".parametric__header").append("<div class='parametric__admin-info'></div>");
        $.each(branchData, function (index, branch) {
            $(".parametric__header .parametric__admin-info").append(function () {
                var branchInfo = "";
                $.each(branch, function (i, j) {
                    branchInfo = branchInfo + "<b>" + i + ":</b> " + j + "; ";
                });
                return branchInfo + "<br>";
            });
        });
    }

    // Retrieve JSON data for visible branch's ID
    getJSONData(branchData[visibleBranchIndex]["branchID"]);

    // Start loading chart
    loadChart();

}


/*
 * Loads JSON data for chart
 * @param branchID [integer]
      - ID of branch for which to pull JSON data
 */
function getJSONData(branchID) {

    var connectionURL = '/ParamChartSearch/chart.aspx?branchID=';
    var connectionString = connectionURL + branchID + (branchData[visibleBranchIndex]["automotive"] ? '&automotive=1' : '') + (branchData[visibleBranchIndex]["popular"] ? '&popular=1' : '') + '&data=json';

    // Set dataLoaded to false, as new data is starting to load
    dataLoaded = false;

    // Empty arrays containing data from JSON file
    allRowData.length = 0;
    visibleRowData.length = 0;
    allColumnHeaders.length = 0;

    $.getJSON(connectionString, function (data) {

        var JSONData = [];
        JSONData.push(data);

        // Build paginatedData array using all JSONData
        $.each(JSONData[0], function (index, value) {

            $.each(value, function (key, value) {
                if (!allRowData[index]) allRowData[index] = [];
                allRowData[index].push(value);

                if (!visibleRowData[index]) visibleRowData[index] = [];
                visibleRowData[index].push(value);
            });

        });

        $.each(JSONData[0][0], function (key, value) {
            var keyWithoutUnderscores = key.replace(/_/g, " ");
            allColumnHeaders.push(keyWithoutUnderscores);
        });

    }).done(function () { dataLoaded = true; });

}


/*
 * Attempts to load chart every 250ms, will load when allColumnHeaders
 * and allRowData are populated
 */
function loadChart() {

    // Hide parametric chart and show loading message
    if (!$(".parametric__message").length) {
        $(".parametric .parametric__chart").hide();
        $(".parametric")
            .append("<div class='parametric__message'>Loading</div>");
    } else {
        if (chartLoadTicks % 2 === 0)
            $(".parametric .parametric__message").append(" .");

        if (chartLoadTicks % 8 === 0)
            $(".parametric .parametric__message").text("Loading");
    }

    chartLoadTicks += 1;

    setTimeout(function () {

        // Check if JSON data is done loading
        if (dataLoaded) {

            chartLoadTicks = 0;

            // Remove loading message and show parametric chart
            if ($(".parametric__message").length) {
                $(".parametric .parametric__message").remove();
                $(".parametric .parametric__chart").show();
            }

            // Build table with buildTable's 'newData' parameter set to true
            buildTable(true);

        } else {

            // Recursively call loadChart
            loadChart();

        }

    }, 250);

}


/*
 * Builds table rows from JSON data
 * @param newData [optional, true/false, default false]
      - whether or not the table is being built with new data
 *
 */
function buildTable(newData) {

    newData = newData || false; // If newData isn't specified, default to false

    if (newData) {

        // Set link for View All Parametrics link
        $(".parametric__view-all-btn").attr("href", "//www.microchip.com/ParamChartSearch/chart.aspx?branchID=" + branchData[visibleBranchIndex]["branchID"] + (branchData[visibleBranchIndex]["automotive"] ? '&automotive=1' : '') + (branchData[visibleBranchIndex]["popular"] ? '&popular=1' : ''));

        // Empty chart headers and list of columns
        $(".parametric .table__row--header").empty();
        $(".parametric .parametric__columns-list").contents(":not(.columns-list__top)").remove();

        // Loop through allColumnHeaders, add each to .parametric__columns-list with
        // corresponding checkboxes and default columns automatically checked
        for (var i = 0; i < allColumnHeaders.length; i++) {
            $(".parametric .table__row--header")
                .append("<div class='table__cell'>" + allColumnHeaders[i] + "</div>");
            if (i === 0) continue; // don't include first column "Product" in dropdown
            $(".parametric")
                .find(".parametric__columns-list")
                .append("<li><input type='checkbox' class='table__header-checkbox' id='tableHeader" + i + "' " + (branchData[visibleBranchIndex]['defaultColumns'].indexOf(i) > -1 ? "checked" : "") + "><label for='tableHeader" + i + "'>" + allColumnHeaders[i] + "</label></li>"); // + (i < 5 ? "checked" : "") +
        }

        // Load default sorting parameters
        var parametricChart = $(".parametric .parametric-table");
        var defaultSortBy = branchData[visibleBranchIndex]["sortBy"] || 0;
        var defaultSortOrder = branchData[visibleBranchIndex]["sortOrder"] || 0;
        parametricChart.attr("data-sortby", defaultSortBy);
        parametricChart.attr("data-sortorder", defaultSortOrder);

        // Sort chart data
        sortData();

    }

    // Remove all table rows from table
    $(".parametric .table__row").remove();

    // Append rows on current page to table
    $.each(visibleRowData, function (index, data) { // index, data array

        // Check if current row index is within the current page, return if not
        if (index < (currentPage * rowsPerPage)) return;
        if (index >= ((currentPage * rowsPerPage) + rowsPerPage)) return false;

        // HTML for the entire row
        var rowHTML = "<div class='table__row'>";

        // Loop through row data and add individual cells to row
        $.each(data, function (index, value) {

            var cell = "<div class='table__cell'>";
            if (index === 0) {
                cell = cell + "<a href='/wwwproducts/" + value + "'>" + value + "</a>";
            } else {
                cell = cell + value;
            }
            cell = cell + "</div>";

            // Add cell to row HTML
            rowHTML = rowHTML + cell;

        });

        // Append rowHTML to table
        $(".table__scrollable-area").append(rowHTML);

    });

    // Only display columns that are checked in the columns list
    $(".parametric").find(".parametric__columns-list input[type=checkbox]:checked")
        .each(function (index) {
            var headerNumber = $(this).attr("id").split("tableHeader")[1];
            $(".parametric").find("[class*='table__row']").each(function () {
                $(this)
                    .find(".table__cell")[headerNumber].style.display = "inline-block";
            });
        });

    buildPagination();
    resizeColumns();
    restripeRows();

}


/*
 * Builds pagination below table for navigating through table pages
 */
function buildPagination() {

    // Update total item count
    $("#totalTableItems").text(visibleRowData.length);

    // Update item number range
    var lastOnPage = (currentPage * rowsPerPage) + 10;
    if (lastOnPage > visibleRowData.length) lastOnPage = visibleRowData.length;
    $("#currentPageItemNumbers")
        .text(((currentPage * rowsPerPage) + 1) + "-" + lastOnPage);

    // Update HTML of pagination pages element
    $(".parametric__pagination-pages").html(function () {

        // Start allPageButtons with first page and previous page buttons
        var allPageButtons = "<span class='parametric__pagination-page-button' data-page='first'><<</span><span class='parametric__pagination-page-button' data-page='prev'><</span>";

        // Determine lowest page in list of immediate 5
        var lowPage = function () {
            var page = currentPage - 1;
            if (page <= 0) page = 1;
            return page;
        }

        // Determine highest page in list of immediate 5
        var highPage = function () {
            var page = lowPage() + 5;
            if (page > Math.ceil(visibleRowData.length / rowsPerPage))
                page = Math.ceil(visibleRowData.length / rowsPerPage) + 1;
            return page;
        }

        // Create page buttons for each page between the low and high pages
        for (var i = lowPage(); i < highPage(); i++) {
            allPageButtons = allPageButtons + "<span class='parametric__pagination-page-button " + ((i - 1) === currentPage ? "current-page" : "") + "' data-page='" + (i - 1) + "'>" + i + "</span>";
        }

        // Add next page and last page buttons to the end of allPageButtons
        allPageButtons = allPageButtons + "<span class='parametric__pagination-page-button' data-page='next'>></span><span class='parametric__pagination-page-button' data-page='last'>>></span>";

        return allPageButtons;

    });

}


/*
 * Sorts JSON data array based on data-sortby and data-sortorder
 */
function sortData() {

    var parametricChart = $(".parametric .parametric-table");
    var columnToSortBy = parametricChart.attr("data-sortby");
    var sortOrder = parametricChart.attr("data-sortorder");

    // Sort visibleRowData array based on sortOrder and columnToSortBy
    visibleRowData.sort(function (a, b) {

        var compareValue;
        var compareTo;

        if (sortOrder == 0) {
            compareValue = a[columnToSortBy];
            compareTo = b[columnToSortBy];
        }
        else if (sortOrder == 1) {
            compareValue = b[columnToSortBy];
            compareTo = a[columnToSortBy];
        }

        if (compareValue.startsWith('$')) compareValue = compareValue.slice(1);
        if (compareTo.startsWith('$')) compareTo = compareTo.slice(1);

        if (isNaN(compareValue) || isNaN(compareTo)) {
            return compareValue.toLowerCase() > compareTo.toLowerCase() ? 1 : -1;
        }
        return compareValue - compareTo;

    });

    // Add arrow to end of sorted column header based on sortOrder
    var sortedColumnHeader = $(".parametric .parametric-table .table__row--header")
        .find(".table__cell")[columnToSortBy];

    $(".parametric .sort-indicator").remove(); // remove all sort indicators
    $(sortedColumnHeader).append("<span class='sort-indicator'></span>");
    $(sortedColumnHeader).find(".sort-indicator").html(function () {
        return (sortOrder == 0 ? " <i class='fa fa-caret-down' aria-hidden='true'></i>" : (sortOrder == 1 ? " <i class='fa fa-caret-up' aria-hidden='true'></i>" : ""));
    });

}


/*
 * Returns an array of visible column ids
 * @return array
 */
var getVisibleColumnHeaders = function () {

    // Array of visible header ids
    var visibleHeaders = ["0"]; // always show Products column (index of "0")

    // Array of all checked header elements
    var checkedHeaders = $(".parametric__columns-list input[type=checkbox]:checked");

    // Loop through checkedHeaders and push id of each header to visibleHeaders
    $.each(checkedHeaders, function (index, value) {
        visibleHeaders.push($(this).attr("id").split("tableHeader")[1]);
    });

    return visibleHeaders;

};


/*
 * Searches parametric tables for specified search term
 * @param e [optional]
      - the event that called searchTable
 */
function searchTable(e) {

    // Clear search input if escape key is pressed
    if (e != null && e.keyCode == 27) {
        clearSearch();
        return;
    }

    // Remove no results message and show parametric chart
    if ($(".parametric__message").length) {
        $(".parametric .parametric__message").remove();
        $(".parametric .parametric__chart").show();
    }

    // Search term from search input box
    var searchTerm = $(".parametric .parametric__search input").val();

    // Get indexes of all visible columns
    var visibleColumnIndexes = getVisibleColumnHeaders();

    // Array of row data of rows that contain searchTerm
    var containingRowData = [];

    // Loop through allRowData
    $.each(allRowData, function (rowIndex, data) {

        // Loop through all columns in each data set
        $.each(data, function (index, value) {

            // Check if current column index is visible and return if not
            if (visibleColumnIndexes.indexOf(index.toString()) === -1) return;

            // Add row data to containingRowData if column includes searchTerm
            if (value.toLowerCase().includes(searchTerm.toLowerCase())) {
                containingRowData.push(data);
                return false;
            }

        });

    });

    // Set visibleRowData to containingRowData
    visibleRowData = containingRowData;

    // Reset current page to 0
    currentPage = 0;

    //Re-build table
    sortData();
    buildTable();

    // Check if containingRowData contains data
    if (containingRowData.length <= 0) {

        // Hide parametric chart and show no results message
        $(".parametric .parametric__chart").hide();
        $(".parametric")
            .append("<div class='parametric__message'>No results.</div>");

    }

}


/*
 * Clears parametric chart search field
 */
function clearSearch() {

    // Empty search input box
    $(".parametric .parametric__search input").val("");

    // Re-search chart
    searchTable();

}


/*
 * Restripes table rows
 */
function restripeRows() {

    // Loop through each visible table row
    $(".parametric .table__row:not(.table__row--header):visible")
        .each(function (index) {

            // Color odd/even row backgrounds differently
            if (index % 2 === 0) {
                $(this).find(".table__cell").css("background-color", "#FFF");
            } else {
                $(this).find(".table__cell").css("background-color", "#F4F4F4");
            }

        });

}


/*
 * Resize columns based on number displayed
 */
function resizeColumns() {

    // Number of columns checked
    var numColumnsChecked = $(".parametric__columns-list input[type=checkbox]:checked").length;

    // Change widths of all table cells to percentage based on number of columns
    $(".parametric")
        .find("[class*='table__cell']")
        .css("width", (100 / numColumnsChecked) + "%");

}


/*
 * Handle clicking of item in parametric title dropdown
 */
$(document).on("click", ".parametric__title-options > li", function () {

    // Disallow changing of branch when data is loading
    if (!dataLoaded) return;

    // Return if click is on currently active branch
    if ($(this).hasClass("active")) return;

    $(".parametric__title-options .active").removeClass("active");
    $(this).addClass("active");
    $(".parametric__title-text-name").text($(this).text());

    visibleBranchIndex = $(this).index();
    currentPage = 0;

    getJSONData($(this).attr("data-branchid"));
    loadChart();

});


/*
 * Handle clicking of any pagination button
 */
$(document).on("click", ".parametric__pagination-page-button", function () {

    var dataPage = $(this).attr("data-page");
    var newPage = (isNaN(dataPage) ? dataPage : parseInt(dataPage));
    var lastPage = Math.ceil(visibleRowData.length / rowsPerPage);

    switch (newPage) {

        case 'first':
            currentPage = 0;
            break;

        case 'last':
            currentPage = lastPage - 1;
            break;

        case 'prev':
            if ((currentPage - 1) >= 0) {
                currentPage--;
            } else {
                return;
            }
            break;

        case 'next':
            if ((currentPage + 1) <= (lastPage - 1)) {
                currentPage++;
            } else {
                return;
            }
            break;

        default:
            if (currentPage === newPage) return;
            currentPage = newPage;
            break;

    }

    buildTable();

});


/*
 * Handle clicking of table column header
 */
$(document).on("click", ".parametric .table__row--header .table__cell", function () {

    var parametricChart = $(".parametric .parametric-table");
    var columnToSortBy = $(this).index();
    var newColumnSelected = false;

    if (parametricChart.attr("data-sortby") != columnToSortBy
        || !parametricChart.attr("data-sortby")) {
        newColumnSelected = true;
    }

    parametricChart.attr("data-sortby", columnToSortBy);
    parametricChart.attr("data-sortorder", function () {
        var newSortOrder = parametricChart.attr("data-sortorder") || -1;
        if (newColumnSelected) newSortOrder = -1;
        newSortOrder++;
        return (newSortOrder < 2 ? newSortOrder : 0);
    });

    sortData();
    buildTable();

});


/*
 * Handle clicking of column menu close button
 */
$(document).on("click", ".parametric .columns-list__close", function () {

    // Uncheck columns toggle box if columns list is visible
    if ($(".parametric__columns-list").css("display") === "block") {
        $(".parametric__columns-toggle-box").prop("checked", false);
    }

});


/*
 * Handle clicking of reset column button in column menu
 */
$(document).on("click", ".parametric .columns-list__reset", function () {

    // Rebuild table with newData set to true
    buildTable(true);

});


/*
 * On change of header checkboxes
 */
$(document).on("change", ".parametric .table__header-checkbox", function () {

    var headerNumber = $(this).attr("id").split("tableHeader")[1];
    var checked = $(this).prop("checked");

    $(".parametric").find("[class*='table__row']").each(function () {
        $(this)
            .find(".table__cell")[headerNumber].style.display = (checked ? "inline-block" : "none");
    });

    var parametricChart = $(".parametric .parametric-table");
    if (parametricChart.attr("data-sortby") == headerNumber && !checked) {
        parametricChart.attr("data-sortby", "0");
        parametricChart.attr("data-sortorder", "0");
        sortData();
    }

    searchTable();
    resizeColumns();

});


/*
 * Hide/show elements based on clicks in document
 */
$(document).on("click", function (e) {

    if (parametricBranchData.length <= 0) return;

    var options = $(".parametric__title-options");
    var title = $(".parametric__title");
    var titleText = $(".parametric__title-text");
    var columnsToggleBox = $(".parametric__columns-toggle-box");
    var columnsToggle = $(".parametric__columns-toggle");
    var columnsList = $(".parametric__columns-list");

    // Disallow column toggle menu from showing if data isn't loaded
    if (columnsToggle.is(e.target)) {
        if (!dataLoaded) e.preventDefault();
    }

    // Hide options with any click on page
    //if (!options.is(e.target) && options.has(e.target).length === 0) {
    if (options.css("display") === "block") {
        options.hide();
        titleText.css("opacity", "1");
        return;
    }
    //}

    // Show options if click is on parametric title
    if (title.is(e.target) || title.has(e.target).length > 0) {
        if (!dataLoaded) return;
        options.show();
        titleText.css("opacity", ".3");
    }

    // Hide column dropdown if click is outside of visible dropdown menu
    if (!columnsToggle.is(e.target)
        && !columnsToggleBox.is(e.target)
        && !columnsList.is(e.target)
        && columnsList.has(e.target).length === 0) {

        if (columnsList.css("display") === "block") {
            columnsToggleBox.prop("checked", false);
        }

    }

});

/*
 * Javascript for search autoComplete in global search box area
 */
var JsHost = (("https:" == document.location.protocol) ?
    "https://" : "http://");
$(document).ready(function () {
    $(document).on('click', 'div.nav__tabs > a', function () {
        var applicationMenu = $(this).attr('href');
        if (applicationMenu == '/application/') {
            hj('trigger', 'ApplicationMenu');
        }
    })
    $("#headright").hide();

    if ($("#searchbarAuto").length > 0) {
        submitFormAuto(3);
        $("#searchbarAuto").autocompleteV1((JsHost + 'www.microchip.com/search/searchapp/AutoComplete.aspx'),
            {
                matchContains: true,
                minChars: 3,
                width: 300,
                autoFill: false,
                max: 10,
                cacheLength: 20,
                formatItem: formatItem,
                formatResult: formatResult,
                scroll: false,
                scrollHeight: 300,
                extraParams: { doc: "no" },
                selectFirst: false
            })

        $("#searchbarAuto").result(function (event, data, formatted) {
            if (data) {
                SearchEnterAuto(1);
            }
        })
    }
});

var ajaxcall = null;
function submitFormAuto(count) {

    if (document.getElementById("searchbarAuto").value.length < 3) {
        $("#bestresults").empty();
        return;
    }
    if (ajaxcall != null) {
        ajaxcall.abort();
        $("#bestresults").empty();
    }
    url1 = JsHost + "www.microchip.com/search/searchapp/AutoSuggest.aspx?q=" + document.getElementById("searchbarAuto").value + "&doc=yes&limit=" + count;
}

function SearchEnterAuto(ac) {
    try {
        if (document.layers) {
            searchObj = eval(document.searchbarAuto);
        }
        else if (navigator.userAgent.indexOf("Opera") != -1) {
            searchObj = eval(document.all.searchbarAuto);
        }
        else if (document.all && !document.getElementById) {
            searchObj = eval(document.all.searchbarAuto);
        }
        else if (document.getElementById) {
            searchObjs = document.getElementsByName('searchbarAuto');
            searchObj = searchObjs[0];
        }
        else { }
        var UserText = searchObj.value;

        if (UserText == '') {
            window.location = JsHost + "www.microchip.com/search/searchapp/searchhome.aspx";
        }
        if (ac == 1)
            window.location = JsHost + "www.microchip.com/search/searchapp/searchhome.aspx?id=2&q=" + UserText + "&ac=1";
        else
            window.location = JsHost + "www.microchip.com/search/searchapp/searchhome.aspx?id=2&q=" + UserText;
    }
    catch (err) {
        window.location = JsHost + "www.microchip.com/search/searchapp/searchhome.aspx";
    }
}
