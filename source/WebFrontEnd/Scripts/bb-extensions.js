/*
 * Roli Christen
 * <roland.christen@hslu.ch>
 * 2012
 */

// extension for string class
String.prototype.format = function () {

    // load arguments into array
    var data = Array.prototype.slice.call(arguments);

    // replace placeholder with values
    return this.replace(/%s/g, function (match, number) {
        if (data.length == 0) return match;
        return data.shift();
    });
};
// extension for string class
String.prototype.formatEx = function () {

    // load arguments into array
    var data = Array.prototype.slice.call(arguments);

    // replace placeholder with values
    return this.replace(/\{([0-9])\}/g, function (match, group) {
        return data[parseInt(group)];
    });
};
String.prototype.endsWith = function (exp) {

    var tail = this.substring(this.length - exp.length);
    return (tail == exp);
};

// extension for date class
Date.prototype.format = function (formatExp) {

    // get parts
    var h = this.getHours();
    var n = this.getMinutes();
    var s = this.getSeconds();

    var d = this.getDate();
    var m = this.getMonth() + 1;
    var y = this.getFullYear();

    // prepare values
    var map = {};
    map['hh'] = h < 10 ? '0' + h : h;
    map['nn'] = m < 10 ? '0' + n : n;
    map['ss'] = s < 10 ? '0' + s : s;
    map['h'] = h;
    map['n'] = n;
    map['s'] = s;
    map['dd'] = d < 10 ? '0' + d : d;
    map['mm'] = m < 10 ? '0' + m : m;
    map['yyyy'] = y;
    map['d'] = d;
    map['m'] = m;
    map['yy'] = y;

    // replace values
    var ret = formatExp;
    $.each(map, function (key, value) {
        ret = ret.replace(new RegExp(key, 'g'), value);
    });
};

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function isDate(exp) {
    var comp = exp.split('.');
    var d = parseInt(comp[0]);
    var m = parseInt(comp[1]);
    var y = parseInt(comp[2]);
    var date = new Date(y, m - 1, d);
    if (date.getFullYear() != y) return false;
    if (date.getMonth() + 1 != m) return false;
    if (date.getDate() != d) return false;
    return true;
}

// extend jquery with hasAttr method
(function (jQuery) {
    jQuery.fn.hasAttr = function (name) {
        for (var i = 0, l = this.length; i < l; i++) {
            if (!!(this.attr(name) !== undefined)) {
                return true;
            }
        }
        return false;
    };
})(jQuery);

// extend jquery with change Instant function
(function (jQuery) {
    jQuery.fn.changeInstant = function (callback) {
        for (var i = 0, l = this.length; i < l; i++) {
            this.on('change keypress paste textInput input', callback);
        }
        return this;
    };
})(jQuery);
(function (jQuery) {
    jQuery.fn.changeInstantOff = function () {
        for (var i = 0, l = this.length; i < l; i++) {
            this.off('change keypress paste textInput input');
        }
        return this;
    };
})(jQuery);