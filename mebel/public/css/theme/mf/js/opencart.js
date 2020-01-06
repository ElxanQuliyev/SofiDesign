/**
 * Created by MaDMaxX on 22.11.17.
 */
function getURLVar(key) {
    var value = [];

    var query = String(document.location).split('?');

    if (query[1]) {
        var part = query[1].split('&');

        for (i = 0; i < part.length; i++) {
            var data = part[i].split('=');

            if (data[0] && data[1]) {
                value[data[0]] = data[1];
            }
        }

        if (value[key]) {
            return value[key];
        } else {
            return '';
        }
    }
}

$(document).ready(function() {
    
    // Highlight any found errors
    $('.text-danger').each(function() {
        var element = $(this).parent().parent();

        if (element.hasClass('form-group')) {
            element.addClass('has-error');
        }
    });

    // Currency
    $('#currency [data-name]').on('click', function(e) {
        e.preventDefault();
        var parent = $('#currency');
        var form = '<form action="'+ parent.data('action') +'" method="post" hidden enctype="multipart/form-data" id="form-currency">';
            form +=     '<input type="hidden" name="code" value="'+ $(this).data('name') + '" />';
            form +=     '<input type="hidden" name="redirect" value="'+ parent.data('redirect') +'" />';
            form += '</form>';
        $('body').append(form);
        $('#form-currency').submit();
    });

    // Language
    $('#language [data-name]').on('click', function(e) {
        e.preventDefault();

        var parent = $('#language');
        var form = '<form action="'+ parent.data('action') +'" method="post" hidden enctype="multipart/form-data" id="form-language">';
        form +=     '<input type="hidden" name="code" value="'+ $(this).data('name') + '" />';
        form +=     '<input type="hidden" name="redirect" value="'+ parent.data('redirect') +'" />';
        form += '</form>';

        $('body').append(form);
        $('#form-language').submit();

    });

    /* Search */
    $('#search input[name=\'search\']').parent().find('button').on('click', function() {
        var url = $('base').attr('href') + 'index.php?route=product/search';

        var value = $('#search input[name=\'search\']').val();

        if (value) {
            url += '&search=' + encodeURIComponent(value);
        }

        location = url;
    });

    $('#search input[name=\'search\']').on('keydown', function(e) {
        if (e.keyCode == 13) {

            $(this).parent().find('button').trigger('click');
        }
    });

    $('#search input').on('keyup', function () {

        var search = $(this).val();

        $.ajax({
            url: 'index.php?route=extension/module/ela_quickly_search',
            type: 'post',
            data: {search: search},
            dataType: 'html',

            success: function(html) {
                if(html) {
                    $('#search .search-results').fadeIn(300);
                    $('#search .search-results').html(html);
                } else {
                    $('#search .search-results').fadeOut(300);
                    $('#search .search-results').html('');
                }
            }
        });
    });

    $('#search input').on('focusout', function () {
        $('#search .search-results').fadeOut(300);
    });

    $('#search input').on('focusin', function () {
        if ($('#search .search-results').children().length) {
            $('#search .search-results').fadeIn(300);
        }
    });

    // Checkout
    $(document).on('keydown', '#collapse-checkout-option input[name=\'email\'], #collapse-checkout-option input[name=\'password\']', function(e) {
        if (e.keyCode == 13) {
            $('#collapse-checkout-option #button-login').trigger('click');
        }
    });

    // tooltips on hover
    $('[data-toggle=\'tooltip\']').tooltip({container: 'body'});

    // Makes tooltips work on ajax generated content
    $(document).ajaxStop(function() {
        $('[data-toggle=\'tooltip\']').tooltip({container: 'body'});
    });

    // list or grid view store Cookies
    $(document).on('click', '#grid-view, #list-view', function() {

        $('#grid-view, #list-view').removeClass('active');
        $(this).addClass('active');
        var date = new Date(new Date().getTime() + 60 * 1000 * 3600 * 24 * 7);

        if ($(this).attr('id') == 'grid-view') {
            if ($(this).closest('#product-category').length || $(this).closest('#product-search').length) {
                $('#products-items .item').removeClass('col-md-12').addClass('col-md-6 col-xs-6');
                $('#products-items .item .figure-list').removeClass('figure-list').addClass('figure-grid');
            }

            if ($(this).closest('#simple-blog-article').length) {
                $('#products-items .item').removeClass('col-sm-12').addClass('col-sm-4');
                $('#products-items .item article').removeClass('article-table');
                $('#products-items .item .text').addClass('hidden');
                $('#products-items .item .entry-table').removeClass('hidden');

                alignHeight('#products-items .item .entry-table .title');
            }
            document.cookie = "view=grid; expires=" + date.toUTCString();

        } else {
            if ($(this).closest('#product-category').length || $(this).closest('#product-search').length) {
                $('#products-items .item').removeClass('col-md-6 col-xs-6').addClass('col-md-12');
                $('#products-items .item .figure-grid').removeClass('figure-grid').addClass('figure-list');
            }

            if ($(this).closest('#simple-blog-article').length) {
                $('#products-items .item').removeClass('col-sm-4').addClass('col-sm-12');
                $('#products-items .item article').addClass('article-table');
                $('#products-items .item .text').removeClass('hidden');
                $('#products-items .item .entry-table').addClass('hidden');
            }

            document.cookie = "view=list; expires=" + date.toUTCString();
        }

    });

    // ajax load in category page
    $(document).on('click', '.pagination a', function (e) {
        e.preventDefault();
        loadCatalogPage($(this).attr('href'));
    });

    $(document).on('change', '#input-limit, #input-sort', function (e) {
        loadCatalogPage(this.value);
    });

    // rating stars
    $('.js-rating').on('mouseover', function (event) {
        var rating = $(event.target).attr('data-rating');
        if (rating) {
            $(this).find('i.fa-star').each(function () {
                if ($(this).attr('data-rating') <= rating) {
                    $(this).addClass('active');
                } else {
                    $(this).removeClass('active');
                }
            });
        }
    });

    $('.js-rating').on('mouseout', function () {
        $(this).find('i.fa-star').removeClass('active');
        $('input[name="rating"]').trigger('change');
    });

    $('.js-rating').on('click', 'i.fa-star', function () {
        var rating = $(this).attr('data-rating');
        $('input[name="rating"]').val(rating);
    });

    $('input[name="rating"]').change(function () {

        var rating = $(this).val();
        $('.js-rating i.fa-star').each(function () {
            if ($(this).attr('data-rating') <= rating) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
        });
    });

});

// Cart add remove functions
var cartTimer;
var cart = {

    'add': function(product_id, quantity) {
        $.ajax({
            url: 'index.php?route=checkout/cart/add',
            type: 'post',
            data: 'product_id=' + product_id + '&quantity=' + (typeof(quantity) != 'undefined' ? quantity : 1),
            dataType: 'json',
            beforeSend: function() {
                // $('#cart > button').button('loading');
            },
            complete: function() {
                // $('#cart > button').button('reset');
            },
            success: function(json) {
                // $('.alert-dismissible, .text-danger').remove();

                if (json['redirect']) {
                    location = json['redirect'];
                }

                if (json['success']) {

                    showMessage('<div class="alert alert-success alert-dismissible"><i class="fa fa-check-circle"></i> ' + json['success'] + ' <button type="button" class="close mfp-close" data-dismiss="alert">&times;</button></div>');

                    $('.open-cart span').remove();

                    if (json['count_items']) {
                        $('.open-cart').append('<span>' + json['count_items'] + '</span>');
                    }

                    $('#cart').load('index.php?route=common/cart/info .checkout', function () {
                        if ($('#cart .cart-block.cart-block-item').length > 1) {
                            alignHeight('#cart .cart-block.cart-block-item .title');
                        }
                    });
                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    },
    'updateQueck': function(selector) {

        if (cartTimer) {
            clearTimeout(cartTimer);
        }

        cartTimer = setTimeout(function () {

            var quantity = {};

            $(selector).find('.quantity input').each(function () {
                quantity[$(this).data('cart-id')] = $(this).val();
            });

            $.ajax({
                url: 'index.php?route=checkout/cart/editqueck',
                type: 'post',
                data: {'quantity': quantity},
                dataType: 'json',
                beforeSend: function() {

                },
                complete: function() {

                },
                success: function(json) {

                        $('.open-cart span').remove();

                        if (json['count_items']) {
                            $('.open-cart').append('<span>' + json['count_items'] + '</span>');
                        }

                        $('#cart').load('index.php?route=common/cart/info .checkout', function () {
                            alignHeight('#cart .cart-block.cart-block-item .title');
                        });
                },
                error: function(xhr, ajaxOptions, thrownError) {
                    alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
                }
            });
        }, 3000);

    },
    'remove': function(key) {
        $.ajax({
            url: 'index.php?route=checkout/cart/remove',
            type: 'post',
            data: 'key=' + key,
            dataType: 'json',
            beforeSend: function() {
                // $('#cart > button').button('loading');
            },
            complete: function() {
                // $('#cart > button').button('reset');
            },
            success: function(json) {
                // Need to set timeout otherwise it wont update the total
                // setTimeout(function () {
                //     $('#cart > button').html('<span id="cart-total"><i class="fa fa-shopping-cart"></i> ' + json['total'] + '</span>');
                // }, 100);
                //
                if (getURLVar('route') == 'checkout/cart' || getURLVar('route') == 'checkout/checkout') {
                    location = 'index.php?route=checkout/cart';
                } else {

                    $('.open-cart span').remove();

                    if (json['count_items']) {
                        $('.open-cart').append('<span>' + json['count_items'] + '</span>');
                    }

                    $('#cart').load('index.php?route=common/cart/info .checkout', function () {
                        alignHeight('#cart .cart-block.cart-block-item .title');
                    });
                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
}

var voucher = {
    'add': function() {

    },
    'remove': function(key) {
        $.ajax({
            url: 'index.php?route=checkout/cart/remove',
            type: 'post',
            data: 'key=' + key,
            dataType: 'json',
            beforeSend: function() {
                $('#cart > button').button('loading');
            },
            complete: function() {
                $('#cart > button').button('reset');
            },
            success: function(json) {
                // Need to set timeout otherwise it wont update the total
                setTimeout(function () {
                    $('#cart > button').html('<span id="cart-total"><i class="fa fa-shopping-cart"></i> ' + json['total'] + '</span>');
                }, 100);

                if (getURLVar('route') == 'checkout/cart' || getURLVar('route') == 'checkout/checkout') {
                    location = 'index.php?route=checkout/cart';
                } else {
                    $('#cart > ul').load('index.php?route=common/cart/info ul li');
                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
}

var wishlist = {
    'add': function(product_id) {
        $.ajax({
            url: 'index.php?route=account/wishlist/add',
            type: 'post',
            data: 'product_id=' + product_id,
            dataType: 'json',
            success: function(json) {
                $('.alert-dismissible').remove();

                if (json['redirect']) {
                    location = json['redirect'];
                }

                if (json['success']) {

                    showMessage('<div class="alert alert-success alert-dismissible"><i class="fa fa-check-circle"></i> ' + json['success'] + ' <button type="button" class="close mfp-close" data-dismiss="alert">&times;</button></div>');

                    $('#wishlist-total span').html(json['total']);
                    $('#wishlist-total').attr('title', json['total']);

                    $('.wish_button').addClass('added');

                }

            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    },
    'remove': function(product_id) {

        $.ajax({
            url: 'index.php?route=account/wishlist/remove',
            type: 'post',
            data: 'product_id=' + product_id,
            dataType: 'json',
            success: function(json) {
                $('.alert-dismissible').remove();

                if (json['redirect']) {
                    location = json['redirect'];
                }

                if (json['success']) {

                    $('#wishlist-total span').html(json['total']);
                    $('#wishlist-total').attr('title', json['total']);

                    $('.wish_button').removeClass('added');

                }

            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });

    }
}

var compare = {
    'add': function(product_id) {
        $.ajax({
            url: 'index.php?route=product/compare/add',
            type: 'post',
            data: 'product_id=' + product_id,
            dataType: 'json',
            success: function(json) {
                $('.alert-dismissible').remove();

                if (json['success']) {

                    showMessage('<div class="alert alert-success alert-dismissible"><i class="fa fa-check-circle"></i> ' + json['success'] + ' <button type="button" class="close mfp-close" data-dismiss="alert">&times;</button></div>');
                    $('#compare-total').html(json['total']);
                    
                    $('.compare_button').addClass('added');

                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    },
    'remove': function(product_id) {
        $.ajax({
            url: 'index.php?route=product/compare/remove',
            type: 'post',
            data: 'product_id=' + product_id,
            dataType: 'json',
            success: function(json) {
                $('.alert-dismissible').remove();

                if (json['success']) {

                    $('#compare-total').html(json['total']);

                    $('.compare_button').removeClass('added');

                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
}

var preview = {
    'product' : function (product_id) {
        $.ajax({
            url: 'index.php?route=product/preview',
            type: 'get',
            data: 'product_id=' + product_id,
            dataType: 'json',
            success: function(json) {

                if (json['content']) {

                    $.magnificPopup.open({
                        items: {
                            src: json['content'],
                            type: 'inline'
                        },
                        fixedContentPos: false,
                        fixedBgPos: true,
                        overflowY: 'auto',
                        closeBtnInside: true,
                        preloader: false,
                        midClick: true,
                        removalDelay: 300,
                        mainClass: 'my-mfp-zoom-in',

                    });

                }
            },
            error: function(xhr, ajaxOptions, thrownError) {
                alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
};


$('.mfp-open').magnificPopup({
    type: 'inline',
    fixedContentPos: false,
    fixedBgPos: true,
    overflowY: 'auto',
    closeBtnInside: true,
    preloader: false,
    midClick: true,
    removalDelay: 300,
    mainClass: 'my-mfp-zoom-in',
    callbacks: {
        open: function () {
            // wait on popup initalization
            // then load owl-carousel
            $('.popup-main .owl-carousel').hide();
            setTimeout(function () {
                $('.popup-main .owl-carousel').slideDown();
            }, 500);
        }
    }
});

var showMessage = function (message) {
    var mfp = $.magnificPopup.instance;

    if (mfp.items && mfp.currItem != null) {
        mfp.items[1] = {
            src: message,
            type: 'inline',
            callbacks: {
                close: function () {
                    console.log('fsf');
                }
            }
        };
        mfp.goTo(1);
        $('.close.mfp-close').one('click', function (e) {
            e.stopPropagation();
            delete mfp.items[1];
            mfp.goTo(0);
        });
    } else {
        $.magnificPopup.open({
            items: {
                src: message,
                type: 'inline'
            },
            fixedContentPos: false,
            fixedBgPos: true,
            overflowY: 'auto',
            closeBtnInside: true,
            preloader: false,
            midClick: true,
            removalDelay: 300,
            mainClass: 'my-mfp-zoom-in',

        });
    }
};

/* Agree to Terms */
$(document).delegate('.agree', 'click', function(e) {
    e.preventDefault();

    $('#modal-agree').remove();

    var element = this;

    $.ajax({
        url: $(element).attr('href'),
        type: 'get',
        dataType: 'html',
        success: function(data) {
            html  = '<div id="modal-agree" class="modal">';
            html += '  <div class="modal-dialog">';
            html += '    <div class="modal-content">';
            html += '      <div class="modal-header">';
            html += '        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
            html += '        <h4 class="modal-title">' + $(element).text() + '</h4>';
            html += '      </div>';
            html += '      <div class="modal-body">' + data + '</div>';
            html += '    </div>';
            html += '  </div>';
            html += '</div>';

            $('body').append(html);

            $('#modal-agree').modal('show');
        }
    });
});

// Autocomplete */
(function($) {
    $.fn.autocomplete = function(option) {
        return this.each(function() {
            this.timer = null;
            this.items = new Array();

            $.extend(this, option);

            $(this).attr('autocomplete', 'off');

            // Focus
            $(this).on('focus', function() {
                this.request();
            });

            // Blur
            $(this).on('blur', function() {
                setTimeout(function(object) {
                    object.hide();
                }, 200, this);
            });

            // Keydown
            $(this).on('keydown', function(event) {
                switch(event.keyCode) {
                    case 27: // escape
                        this.hide();
                        break;
                    default:
                        this.request();
                        break;
                }
            });

            // Click
            this.click = function(event) {
                event.preventDefault();

                value = $(event.target).parent().attr('data-value');

                if (value && this.items[value]) {
                    this.select(this.items[value]);
                }
            }

            // Show
            this.show = function() {
                var pos = $(this).position();

                $(this).siblings('ul.dropdown-menu').css({
                    top: pos.top + $(this).outerHeight(),
                    left: pos.left
                });

                $(this).siblings('ul.dropdown-menu').show();
            }

            // Hide
            this.hide = function() {
                $(this).siblings('ul.dropdown-menu').hide();
            }

            // Request
            this.request = function() {
                clearTimeout(this.timer);

                this.timer = setTimeout(function(object) {
                    object.source($(object).val(), $.proxy(object.response, object));
                }, 200, this);
            }

            // Response
            this.response = function(json) {
                html = '';

                if (json.length) {
                    for (i = 0; i < json.length; i++) {
                        this.items[json[i]['value']] = json[i];
                    }

                    for (i = 0; i < json.length; i++) {
                        if (!json[i]['category']) {
                            html += '<li data-value="' + json[i]['value'] + '"><a href="#">' + json[i]['label'] + '</a></li>';
                        }
                    }

                    // Get all the ones with a categories
                    var category = new Array();

                    for (i = 0; i < json.length; i++) {
                        if (json[i]['category']) {
                            if (!category[json[i]['category']]) {
                                category[json[i]['category']] = new Array();
                                category[json[i]['category']]['name'] = json[i]['category'];
                                category[json[i]['category']]['item'] = new Array();
                            }

                            category[json[i]['category']]['item'].push(json[i]);
                        }
                    }

                    for (i in category) {
                        html += '<li class="dropdown-header">' + category[i]['name'] + '</li>';

                        for (j = 0; j < category[i]['item'].length; j++) {
                            html += '<li data-value="' + category[i]['item'][j]['value'] + '"><a href="#">&nbsp;&nbsp;&nbsp;' + category[i]['item'][j]['label'] + '</a></li>';
                        }
                    }
                }

                if (html) {
                    this.show();
                } else {
                    this.hide();
                }

                $(this).siblings('ul.dropdown-menu').html(html);
            }

            $(this).after('<ul class="dropdown-menu"></ul>');
            $(this).siblings('ul.dropdown-menu').delegate('a', 'click', $.proxy(this.click, this));

        });
    }
})(window.jQuery);

var alignHeight = function (selector) {

    if ($(selector).length > 1) {
        var maxH = 0;

        $(selector).each(function () {
            maxH = ( $(this).height() > maxH ) ? $(this).height() : maxH;
        });

        $(selector).height(maxH);
    }

};

// for ajax load
var loadCatalogPage = function (href) {

    var load_href = href;

    $.ajax({
        url: load_href,
        type: 'get',

        beforeSend: function () {
            $('#content').addClass('loading');
        },
        success: function(html) {

            $('#content').replaceWith(
                $(html).find('#content')
            );

            history.pushState(null, null, href);

            if ($('.sort-bar').length) {
                $('html,body').animate({
                    scrollTop: $('.sort-bar').offset().top - $('.floating-menu').height() - $('.sort-bar').height()
                }, 800);
            }

            $('#content').removeClass('loading');
        },
        error: function(xhr, ajaxOptions, thrownError) {
            alert(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
        }
    });
    
};