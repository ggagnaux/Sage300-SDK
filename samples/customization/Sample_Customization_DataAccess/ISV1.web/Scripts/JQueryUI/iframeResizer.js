// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

$(window).bind("load", function () {
    var iFrameHeight = window.top.$('iframe.screenIframe:visible');
    var th = iFrameHeight.contents().find('body').height();
    if (iFrameHeight.contents().find('#CrystalReportViewerSage300').length) {
        th = th + 10;
    }
    iFrameHeight.css('height', th);

    // get the observer that work with all browsers
    var MutationObserver = window.MutationObserver || window.WebKitMutationObserver || window.MozMutationObserver;

    // create observer to adjust height
    var observer = new MutationObserver(function (mutations) {
        var visibleIframe = window.top.$('iframe.screenIframe:visible');
        var padding = 0;
        // put back old code logic
        if (visibleIframe.contents().find('#CrystalReportViewerSage300').length) {
            padding = 10;
        }

        if (typeof sg === "undefined" || !sg.utls.isKendoIframe()) {
            visibleIframe.css('height', $('body').height() + padding);
        }
    });

    // put to observe only if it is inside one of the iframe, that's because empty iframe has no content to observe
    if (window.self !== window.top) {
        observer.observe(document.body, {
            childList: true,
            subtree: true,
            attributes: true
        });
    }
});

